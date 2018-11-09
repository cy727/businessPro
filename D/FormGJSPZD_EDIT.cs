using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormGJSPZD_EDIT : Form
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

        public int intDJID = 0;

        private int iYWY = 0;
         
        private int iSupplyCompany = 0;
        private int intHTH = 0;
        private decimal dDJSUM = 0;
        private int iRowCount = 0;
        private bool isSaved = false;
        private ClassGetInformation cGetInformation;

        private bool bCheck = true;
        private int iBM = 0;
        private int iHT = 0;


        public FormGJSPZD_EDIT()
        {
            InitializeComponent();
        }

        private void FormGJSPZD_EDIT_Load(object sender, EventArgs e)
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

            this.Text += ":单据冲红";

            sqlConn.Open();


            sqlComm.CommandText = "SELECT 购进商品制单表.单据编号, 购进商品制单表.日期, [职员表_1].职员姓名, 职员表.职员姓名 AS Expr1, 单位表.单位编号, 单位表.单位名称, 购进商品制单表.发票号, 购进商品制单表.付款方式, 采购合同表.合同编号, 购进商品制单表.价税合计, 单位表.ID, 购进商品制单表.备注,购进商品制单表.业务员ID, 购进商品制单表.部门ID, 购进商品制单表.合同ID FROM 购进商品制单表 INNER JOIN 单位表 ON 购进商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 购进商品制单表.业务员ID = 职员表.ID INNER JOIN 职员表 [职员表_1] ON 购进商品制单表.操作员ID = [职员表_1].ID LEFT OUTER JOIN 采购合同表 ON 购进商品制单表.合同ID = 采购合同表.ID WHERE (购进商品制单表.ID = " + intDJID.ToString() + " AND 购进商品制单表.BeActive<>0 )";
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

                if (sqldr.GetValue(14).ToString() != "")
                {
                    try
                    {
                        iHT = int.Parse(sqldr.GetValue(14).ToString());
                    }
                    catch
                    {
                        iHT = 0;
                    }

                }


                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelCZY.Text = sqldr.GetValue(2).ToString();

                comboBoxYWY.Text = sqldr.GetValue(3).ToString();
                textBoxDWBH.Text = sqldr.GetValue(4).ToString();
                textBoxDWMC.Text = sqldr.GetValue(5).ToString();
                textBoxFPH.Text = sqldr.GetValue(6).ToString();
                textBoxBZ.Text = sqldr.GetValue(11).ToString();

                iYWY = Convert.ToInt32(sqldr.GetValue(12).ToString());
                iSupplyCompany = Convert.ToInt32(sqldr.GetValue(10).ToString());
                dDJSUM = 0;
                if (sqldr.GetValue(9).ToString()!="")
                    dDJSUM = Convert.ToDecimal(sqldr.GetValue(9).ToString());
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

            sqlComm.CommandText = "SELECT 购进商品制单明细定义表.保留, 商品表.商品名称, 商品表.商品编号,商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 购进商品制单明细表.数量, 购进商品制单明细表.单价, 购进商品制单明细表.金额, 购进商品制单明细表.赠品, 购进商品制单明细表.扣率, 购进商品制单明细表.实计金额, 购进商品制单明细表.商品ID, 购进商品制单明细表.库房ID, 商品表.最终进价, 购进商品制单明细表.ID, 购进商品制单明细表.数量 AS 原单数量, 购进商品制单明细表.单价 AS 原单单价 FROM 购进商品制单明细表 INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 购进商品制单明细表.库房ID = 库房表.ID CROSS JOIN 购进商品制单明细定义表 WHERE (购进商品制单明细表.表单ID = "+intDJID.ToString()+")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewGJSPZD.DataSource = dSet.Tables["商品表"];
            sqlConn.Close();

            dataGridViewGJSPZD.Columns[0].Visible = false;
            dataGridViewGJSPZD.Columns[13].Visible = false;
            dataGridViewGJSPZD.Columns[14].Visible = false;
            dataGridViewGJSPZD.Columns[16].Visible = false;
            dataGridViewGJSPZD.Columns[3].ReadOnly = true;
            dataGridViewGJSPZD.Columns[4].ReadOnly = true;
            //dataGridViewGJSPZD.Columns[7].ReadOnly = true;
            //dataGridViewGJSPZD.Columns[8].ReadOnly = true;
            dataGridViewGJSPZD.Columns[9].ReadOnly = true;
            dataGridViewGJSPZD.Columns[12].ReadOnly = true;
            dataGridViewGJSPZD.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGJSPZD.Columns[17].ReadOnly = true;
            dataGridViewGJSPZD.Columns[18].ReadOnly = true;
            dataGridViewGJSPZD.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewGJSPZD.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewGJSPZD.Columns[9].DefaultCellStyle.Format = "f2";

            dataGridViewGJSPZD.ShowCellErrors = true;

            for(i=0;i<dataGridViewGJSPZD.Columns.Count;i++)
            {
                dataGridViewGJSPZD.Columns[i].SortMode=DataGridViewColumnSortMode.NotSortable;
            }

            iRowCount = dataGridViewGJSPZD.Rows.Count - 1;


            countAmount();

            if (dataGridViewGJSPZD.Rows.Count > 0)
                dataGridViewGJSPZD.CurrentCell = dataGridViewGJSPZD.Rows[dataGridViewGJSPZD.Rows.Count - 1].Cells[1];

        }

        private bool countAmount()
        {
            decimal fSum = 0, fSum1 = 0;
            decimal fTemp, fTemp1;
            decimal fCount = 0, fCSum = 0;
            bool bCheck = true;

            this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
            cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);

            for (int i = 0; i < dataGridViewGJSPZD.Rows.Count; i++)
            {
                if (dataGridViewGJSPZD.Rows[i].IsNewRow)
                    continue;

                if (i >= iRowCount)
                    dataGridViewGJSPZD.Rows[i].Cells[0].Value = 1;

                if (dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() == "" || dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() == "0")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[1].ErrorText = "输入所购商品";
                    dataGridViewGJSPZD.Rows[i].Cells[2].ErrorText = "输入所购商品";
                    bCheck = false;
                }

                if (dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[2].ErrorText = "输入所购商品";
                    bCheck = false;
                }
                if (dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[8].ErrorText = "输入所购商品价格";
                    bCheck = false;
                }
                if (dataGridViewGJSPZD.Rows[i].Cells[11].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[11].Value = 100;
                }

                if (!bCheck)
                    continue;

                if (!Convert.ToBoolean(dataGridViewGJSPZD.Rows[i].Cells[0].Value))
                {
                    //数量
                    if (dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() == "")
                        fTemp = 0;
                    else
                        fTemp = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[7].Value);
                    fCSum += fTemp;

                    //单价
                    if (dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() == "")
                        fTemp1 = 0;
                    else
                        fTemp1 = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[8].Value);

                    dataGridViewGJSPZD.Rows[i].Cells[9].Value = Math.Round(fTemp * fTemp1, 2);
                    continue;
                }


                //数量
                if (dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() == "")
                    fTemp = 0;
                else
                    fTemp = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[7].Value);
                fCSum += fTemp;

                //单价
                if (dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() == "")
                    fTemp1 = 0;
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[8].Value);

                if (dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() != "" && dataGridViewGJSPZD.Rows[i].Cells[15].Value.ToString() != "")
                {
                    if (Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[8].Value) > Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[15].Value))
                        dataGridViewGJSPZD.Rows[i].Cells[8].Style.BackColor = Color.LightPink;
                    else
                        dataGridViewGJSPZD.Rows[i].Cells[8].Style.BackColor = Color.White;
                }

                //金额
                dataGridViewGJSPZD.Rows[i].Cells[9].Value = Math.Round(fTemp * fTemp1, 2);

                //扣率
                if (dataGridViewGJSPZD.Rows[i].Cells[11].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[11].Value = 100;
                }

                //
                if (dataGridViewGJSPZD.Rows[i].Cells[10].Value.ToString() == "")
                {
                    dataGridViewGJSPZD.Rows[i].Cells[10].Value = 0;
                }

                //赠品
                if (Convert.ToBoolean(dataGridViewGJSPZD.Rows[i].Cells[10].Value))
                {
                    dataGridViewGJSPZD.Rows[i].Cells[11].Value = 0.0;
                }
                fTemp = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[11].Value);

                //实计金额
                fTemp1 = Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[9].Value);
                dataGridViewGJSPZD.Rows[i].Cells[12].Value = fTemp * fTemp1 / 100;


                fSum += Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[9].Value);
                fSum1 += Convert.ToDecimal(dataGridViewGJSPZD.Rows[i].Cells[12].Value);

                fCount += 1;

            }
            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
            dataGridViewGJSPZD.EndEdit();

            labelSLHJ.Text = fCSum.ToString();
            labelJEHJ.Text = fSum.ToString();
            labelSJJE.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return bCheck;


        }

        private void dataGridViewGJSPZD_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
                    this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iCommNumber;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = cGetInformation.decCommZZJJ.ToString();
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;

                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                    dataGridViewGJSPZD.EndEdit();
                    dataGridViewGJSPZD.CurrentCell = dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7];
                    dataGridViewGJSPZD.BeginEdit(true);
                    this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

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
                    this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;

                    dataGridViewGJSPZD.EndEdit();
                    dataGridViewGJSPZD.CurrentCell = dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7];
                    dataGridViewGJSPZD.BeginEdit(true);
                    this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                }
            }
            

        }

        private void dataGridViewGJSPZD_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridViewGJSPZD_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int iRe = 0;

            if (!bCheck)
                return;

            if (dataGridViewGJSPZD.Rows[e.RowIndex].IsNewRow)
                return;
            if (e.RowIndex < iRowCount && e.ColumnIndex != 0) return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewGJSPZD);
            switch (e.ColumnIndex)
            {
                case 2: //商品编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = Math.Round(Decimal.Zero, 2);

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                        break;

                    }

                    iRe = cGetInformation.getCommInformation(20, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewGJSPZD.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //失败
                    //if (cGetInformation.getCommInformation(20, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewGJSPZD.CancelEdit();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].ErrorText = "商品编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                            break;
                        }
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iCommNumber;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = cGetInformation.decCommZZJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                    }
                    break;

                case 1: //商品名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                        break;

                    }

                    iRe = cGetInformation.getCommInformation(10, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewGJSPZD.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //失败
                    //if (cGetInformation.getCommInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].ErrorText = "商品助记码输入错误";
                    }
                    else
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                            break;
                        }
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iCommNumber;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value = cGetInformation.decCommZZJJ.ToString();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                    }
                    break;
                case 5: //库房编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = 0;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = "";
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewGJSPZD.CancelEdit();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].ErrorText = "库房编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                            break;
                        }
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                    }
                    break;
                case 6: //库房名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = 0;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = "";
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(20, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewGJSPZD.CancelEdit();
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].ErrorText = "库房助记码输入错误";
                    }
                    else
                    {

                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;

                        if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                            break;
                        }
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;

                    }

                    break;
                case 7:  //商品数量
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut <= 0)
                        {
                            dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].ErrorText = "商品数量输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[7].ErrorText = "商品数量输入类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 8: //商品价格
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;
                    if (dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value.ToString() == "")
                    {
                        MessageBox.Show("请先输入购进商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                        break;
                    }

                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        if (detOut >= 0)
                        {
                            detOut = Math.Round(detOut, 2);

                            if (detOut.CompareTo(dataGridViewGJSPZD.Rows[e.RowIndex].Cells[15].Value) > 0)
                            {
                                if (MessageBox.Show("商品价格高于最终进价，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                                    e.Cancel = true;
                                else
                                {
                                    this.dataGridViewGJSPZD.CellValidating -= dataGridViewGJSPZD_CellValidating;
                                    dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].Value = detOut;
                                    this.dataGridViewGJSPZD.CellValidating += dataGridViewGJSPZD_CellValidating;
                                }

                            }
                        }
                        else
                        {
                            dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].ErrorText = "商品价格输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[8].ErrorText = "商品数量价格类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 11:  //扣率
                    double dOut = 0.0;
                    if (e.FormattedValue.ToString() == "")
                    {
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[11].Value = 100;
                        break;
                    }
                    if (Double.TryParse(e.FormattedValue.ToString(), out dOut))
                    {
                        if (dOut <= 0 || dOut > 100.0)
                        {
                            dataGridViewGJSPZD.Rows[e.RowIndex].Cells[11].ErrorText = "商品扣率输入错误，请输入0.01-100.0之间的数字";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewGJSPZD.Rows[e.RowIndex].Cells[11].ErrorText = "商品扣率输入错误，请输入0.01-100.0之间的数字";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;



            }
            dataGridViewGJSPZD.EndEdit();

        }

        private void dataGridViewGJSPZD_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
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
                                    case 5:
                                    case 6:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                        break;
                                    case 7:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[8];
                                        break;
                                    case 8:
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



        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i,j=0,k,iCount;
            decimal dTemp=0;

            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("购进商品制单已经冲红完毕,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //
            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            //查询入库
            sqlComm.CommandText = "SELECT 单据编号 FROM 进货入库汇总表 WHERE (购进ID = "+intDJID.ToString()+") AND (BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                while (sqldr.Read())
                {
                    MessageBox.Show("已有入库记录,单据号为：" + sqldr.GetValue(0).ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                }
                sqldr.Close();
                sqlConn.Close();
                return;
            }
            sqldr.Close();

            //发票记录
            sqlComm.CommandText = "SELECT 发票号, ID FROM 购进商品制单表 WHERE (发票号 IS NOT NULL) AND (发票号 NOT LIKE N'不开票%') AND (ID = " + intDJID.ToString() + ") AND (发票号 NOT LIKE N'现金不开票%')";
            sqldr = sqlComm.ExecuteReader();
            bool b = false;
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
            string strDate1="";
            sqlComm.CommandText = "SELECT 日期 from 购进商品制单表 WHERE (ID = " + intDJID.ToString() + ")";
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
            

            //sqlConn.Close();

            string strCount = "", strDateSYS = "", strKey = "AKP";
            
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            saveToolStripButton.Enabled = false;
            try
            {

                //表单汇总
                sqlComm.CommandText = "UPDATE 购进商品制单表 SET BeActive = 0 WHERE (ID = " + intDJID.ToString() + ")";
                sqlComm.ExecuteNonQuery();


                //修改合同未执行状态
                //相关合同结束
                if (iHT != 0)
                {
                    sqlComm.CommandText = "UPDATE 采购合同表 SET 执行标记 = 0 WHERE (ID = " + iHT.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                string sBMID = "NULL";
                if (iBM != 0)
                    sBMID = iBM.ToString();
                //得到服务器日期
                sqlComm.CommandText = "SELECT GETDATE() AS 日期";
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                }
                sqldr.Close();


                sqlComm.CommandText = "UPDATE 购进商品制单表 SET 冲红时间 = '" + strDateSYS + "' WHERE (ID = " + intDJID.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //单位历史纪录
                dTemp = Convert.ToDecimal(labelSJJE.Text);
                sqlComm.CommandText = "INSERT INTO 单位历史账表 (单位ID, 日期, 单据编号, 摘要, 购进未入库金额, 业务员ID, 冲抵单号, BeActive, 部门ID) VALUES ( " + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + labelDJBH.Text + "冲', N'购进商品制单冲红', -1*" + dTemp.ToString() + ", " + iYWY.ToString()  + ", N'" + textBoxHTH.Text + "', 1, "+sBMID+")";
                sqlComm.ExecuteNonQuery();


                //单据明细
                for (i = 0; i < iRowCount; i++)
                {

                        //sqlComm.CommandText = "DELETE FROM 购进商品制单明细表 WHERE (ID = " + dataGridViewGJSPZD.Rows[i].Cells[16].Value.ToString() + ")";
                        //sqlComm.ExecuteNonQuery();

                        //商品库房历史表
                        sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (日期, 商品ID, 单位ID, 库房ID, 业务员ID, 单据编号, 摘要, 购进数量, 购进单价, 购进金额, BeActive, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[14].Value.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "改', N'购进商品制单修改', -1*" + dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() + ", -1*" + dataGridViewGJSPZD.Rows[i].Cells[9].Value.ToString() + ", 1,"+sBMID+")";
                        sqlComm.ExecuteNonQuery();

                        //商品历史表
                        sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 单据编号, 摘要, 购进数量, 购进单价, 购进金额, BeActive, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewGJSPZD.Rows[i].Cells[13].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "改', N'购进商品制单修改', -1*" + dataGridViewGJSPZD.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewGJSPZD.Rows[i].Cells[8].Value.ToString() + ", -1*" + dataGridViewGJSPZD.Rows[i].Cells[9].Value.ToString() + ", 1,"+sBMID+")";
                        sqlComm.ExecuteNonQuery();
                



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

            //MessageBox.Show("购进商品制单修改成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isSaved = true;

            if (MessageBox.Show("购进商品制单冲红成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void FormGJSPZD_EDIT_FormClosing(object sender, FormClosingEventArgs e)
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

        private void dataGridViewGJSPZD_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (dataGridViewGJSPZD.CurrentCell.RowIndex < iRowCount)
                e.Cancel = true;
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("购进商品制单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "购进商品制单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewGJSPZD, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("购进商品制单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "购进商品制单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewGJSPZD, strT, false, intUserLimit);
        }



    }
}