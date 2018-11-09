using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKCSPZZ : Form
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

        private int intKFID = 0;
        private int intClassID = 0;

        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;

        public FormKCSPZZ()
        {
            InitializeComponent();
        }

        private void FormKCSPZZ_Load(object sender, EventArgs e)
        {
            int i;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            this.Top = 1;
            this.Left = 1;


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
            sqlComm.CommandText = "SELECT 部门名称 FROM 部门表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("部门表")) dSet.Tables.Remove("部门表");
            sqlDA.Fill(dSet, "部门表");
            //comboBoxBM.DataSource = dSet.Tables["部门表"];

            comboBoxBM.Items.Add("全部");
            for (i = 0; i < dSet.Tables["部门表"].Rows.Count; i++)
            {
                comboBoxBM.Items.Add(dSet.Tables["部门表"].Rows[i][0].ToString().Trim());
            }
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;


            //初始化组件列表
            sqlComm.CommandText = "SELECT 库存商品组装明细表.ID, 商品表.商品名称,商品表.商品编号, 商品表.商品规格,库房表.库房编号, 库房表.库房名称, 库存商品组装明细表.组件数量, 库存商品组装明细表.成本单价, 库存商品组装明细表.成本金额, 库存商品组装明细表.备注, 库存商品组装明细表.组件ID, 库存商品组装明细表.库房ID, 库存商品组装定义表.库存量, 库存商品组装定义表.统计标志 FROM 库存商品组装明细表 INNER JOIN 商品表 ON 库存商品组装明细表.组件ID = 商品表.ID INNER JOIN 库房表 ON 库存商品组装明细表.库房ID = 库房表.ID CROSS JOIN 库存商品组装定义表 WHERE (库存商品组装明细表.ID = 0)";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[7].ReadOnly = true;
            dataGridViewDJMX.Columns[8].ReadOnly = true;
            dataGridViewDJMX.Columns[12].ReadOnly = true;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;

            sqlConn.Close();

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
        }

        private void initDJ()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 库存商品组装汇总表.单据编号, 库存商品组装汇总表.日期, 职员表.职员姓名, 操作员.职员姓名 AS 操作员, 库存商品组装汇总表.备注, 库房表.库房编号, 库房表.库房名称, 库存商品组装汇总表.商品编号, 库存商品组装汇总表.商品名称, 库存商品组装汇总表.商品数量, 库存商品组装汇总表.组装费用 FROM 库存商品组装汇总表 INNER JOIN 库房表 ON 库存商品组装汇总表.成品库房ID = 库房表.ID INNER JOIN 职员表 操作员 ON 库存商品组装汇总表.操作员ID = 操作员.ID INNER JOIN 职员表 ON 库存商品组装汇总表.业务员ID = 职员表.ID WHERE (库存商品组装汇总表.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                textBoxKFBH.Text = sqldr.GetValue(5).ToString();
                textBoxKFMC.Text = sqldr.GetValue(6).ToString();

                textBoxSPBH.Text = sqldr.GetValue(7).ToString();
                textBoxSPMC.Text = sqldr.GetValue(8).ToString();
                numericUpDownCPSL.Value = Convert.ToDecimal(sqldr.GetValue(9).ToString());
                numericUpDownZZFY.Value = Convert.ToDecimal(sqldr.GetValue(10).ToString());



                this.Text = "库存商品组装制单：" + labelDJBH.Text;
            }
            sqldr.Close();

            //初始化商品列表
            sqlComm.CommandText = "SELECT 库存商品组装明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库房表.库房编号, 库房表.库房名称, 库存商品组装明细表.组件数量, 库存商品组装明细表.成本单价, 库存商品组装明细表.成本金额, 库存商品组装明细表.备注, 库存商品组装明细表.组件ID, 库存商品组装明细表.库房ID, 商品表.库存数量, 库存商品组装定义表.统计标志 FROM 库存商品组装明细表 INNER JOIN 商品表 ON 库存商品组装明细表.组件ID = 商品表.ID INNER JOIN 库房表 ON 库存商品组装明细表.库房ID = 库房表.ID CROSS JOIN 库存商品组装定义表 WHERE (库存商品组装明细表.单据ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;

            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;

            sqlConn.Close();
        }



        private void comboBoxBM_SelectedIndexChanged(object sender, EventArgs e)
        {
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
        }

        private void textBoxKFBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                intKFID = cGetInformation.iKFNumber;
                textBoxKFBH.Text = cGetInformation.strKFCode;
                textBoxKFMC.Text = cGetInformation.strKFName;
            }
        }

        private void textBoxKFBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFBH.Text.Trim()) == 0) //失败
                {
                    intKFID = 0;
                    textBoxKFBH.Text = "";
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                }
            }
        }

        private void textBoxKFMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFMC.Text.Trim()) == 0) //失败
                {
                    intKFID = 0;
                    textBoxKFMC.Text = "";
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                }
            }
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
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
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = cGetInformation.iCommNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommKCCBJ;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = Math.Round(Decimal.Zero, 2);
                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value.ToString() == "")
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 0;

                    cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value));
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCL;

                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[4];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }

            if (e.ColumnIndex == 5 || e.ColumnIndex == 4) //库房编号
            {
                if (cGetInformation.getKFInformation(1, "") == 0) //失败
                {
                    return;
                }
                else
                {
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;

                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value.ToString() == "")
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 0;

                    cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value));
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCL;
                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[6];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }

        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[9];
                                        break;
                                    case 9:
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

        private bool countAmount()
        {
            decimal fSum = 0;
            decimal fSum1 = 0;
            decimal fCSum = 0;

            decimal fTemp, fTemp1;
            decimal fCount = 0;
            bool bCheck = true;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[1].ErrorText = "输入组装商品组件";
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "输入组装商品组件";
                    bCheck = false;
                }

                if (dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[4].ErrorText = "输入组件商品库房";
                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "输入组件商品库房";
                    bCheck = false;
                }


                if (dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[6].ErrorText = "输入组件商品数量";
                    bCheck = false;
                }

                if (!bCheck)
                    continue;

                //成本单价
                if (dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[7].Value = 0;
                }

                //库存量
                if (dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[12].Value = 0;

                //颜色表示
                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value)*numericUpDownCPSL.Value > Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[12].Value))
                    dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.LightPink;
                else
                    dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.White;


                //数量
                fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                //单价
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);

                //金额
                dataGridViewDJMX.Rows[i].Cells[8].Value = Math.Round(fTemp * fTemp1, 2);

                fCount += 1;
                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);

            }
            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelJEHJ.Text = fSum.ToString();
            labelSLHJ.Text = fCSum.ToString();
            toolStripStatusLabelMXJLS.Text = fCount.ToString();
            fSum1 = fSum + numericUpDownZZFY.Value;
            labelSPCB.Text = fSum1.ToString();
            return bCheck;

        }

        private void numericUpDownZZFY_ValueChanged(object sender, EventArgs e)
        {
            countAmount();
        }

        private void dataGridViewDJMX_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            switch (e.ColumnIndex)
            {
                case 2: //商品编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getCommInformation(20, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].ErrorText = "商品编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value.ToString() == cGetInformation.iCommNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommKCCBJ.ToString();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;


                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;

                case 1: //商品名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                        break;

                    }

                    if (cGetInformation.getCommInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].ErrorText = "商品助记码输入错误";
                    }
                    else
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value.ToString() == cGetInformation.iCommNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommKCCBJ.ToString();

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;


                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    }
                    break;
                case 4: //库房编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;

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

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value.ToString() == cGetInformation.iKFNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;
                case 5: //库房名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = 0;
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
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value.ToString() == cGetInformation.iKFNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }

                    break;
                case 6:  //商品数量
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut <= 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "组件数量输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "组件数量输入类型错误";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;

            }
            dataGridViewDJMX.EndEdit();


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
            decimal fTemp=0,fTemp1=0;

            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("商品组装单已经保存,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (intClassID == 0)
            {
                MessageBox.Show("请输入组装成品类别", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if (numericUpDownCPSL.Value == 0)
            {
                MessageBox.Show("请输入组装成品数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (textBoxSPBH.Text == "")
            {
                MessageBox.Show("请输入组装商品编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (textBoxSPMC.Text == "")
            {
                MessageBox.Show("请输入组装商品名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (intKFID == 0)
            {
                MessageBox.Show("请选择成品库房", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("商品组装明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("没有商品组装组件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (MessageBox.Show("请检查商品组装单内容,该制单内容不可更改，是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;



            string strCount = "", strDateSYS = "", strKey = "CZZ";
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

                //标志复位
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    dataGridViewDJMX.Rows[i].Cells[13].Value = 1;
                }

                //总库存
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[13].Value) == 0) //已经计算过
                        continue;

                    //计算该单的每个商品库存
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[13].Value) == 0) //已经计算过
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[10].Value == dataGridViewDJMX.Rows[i].Cells[10].Value) //同种商品
                        {
                            dKUL1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[6].Value);
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                            dataGridViewDJMX.Rows[j].Cells[13].Value = 0;
                        }

                    }
                    dKUL1 = dKUL1 * numericUpDownCPSL.Value;
                    dKCJE1=dKCJE1 * numericUpDownCPSL.Value;

                    //总库存变更
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价 FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                    }
                    sqldr.Close();

                    dKUL -= dKUL1;
                    dKCJE -= dKUL1;

                    sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额="+dKCJE.ToString()+" WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //商品历史纪录
                    sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 业务员ID, 单据编号, 摘要, 组装数量, 组装单价, 组装金额, 总结存数量, 总结存金额, BeActive) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'库存商品组装', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", 1)";
                    sqlComm.ExecuteNonQuery();
                }

                //标志复位
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    dataGridViewDJMX.Rows[i].Cells[13].Value = 1;
                }

                //分库存
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[13].Value) == 0) //已经计算过
                        continue;

                    //计算该单的每个商品数量
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[13].Value) == 0) //已经计算过
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[10].Value == dataGridViewDJMX.Rows[i].Cells[10].Value && dataGridViewDJMX.Rows[j].Cells[11].Value == dataGridViewDJMX.Rows[i].Cells[11].Value) //同种商品，同样库存
                        {
                            dKUL1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[6].Value);
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);

                            dataGridViewDJMX.Rows[j].Cells[13].Value = 0;
                        }

                    }
                    dKUL1 = dKUL1 * numericUpDownCPSL.Value;
                    dKCJE1 = dKCJE1 * numericUpDownCPSL.Value;

                    //分库存更新
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ") AND (BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows) //存在库存
                    {
                        sqldr.Read();
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        sqldr.Close();

                        dKUL -= dKUL1;
                        dKCJE -= dKCJE1;
                        sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额=" + dKCJE.ToString() + " WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ") AND (BeActive = 1)";
                        sqlComm.ExecuteNonQuery();

                        //商品库房历史纪录
                        sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 业务员ID, 单据编号, 摘要, 组装数量, 组装单价, 组装金额, 库房结存数量, 库房结存金额, BeActive) VALUES (" + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[10].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'库存商品组装', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", 1)";
                        sqlComm.ExecuteNonQuery();

                    }
                    sqldr.Close();

                }

                //商品定义
                fTemp=Convert.ToDecimal(labelSPCB.Text)*numericUpDownCPSL.Value;
                sqlComm.CommandText = "INSERT INTO 商品表 (商品编号, 商品名称, 库存数量, 库存成本价, 库存金额, 最终进价, 最高进价, 最低进价, 组装商品, beactive, 分类编号) VALUES (N'" + textBoxSPBH.Text + "', N'" + textBoxSPMC.Text + "', " + numericUpDownCPSL.Value.ToString() + ", " + labelSPCB.Text + ", " + fTemp.ToString() + ", " + labelSPCB.Text + ", " + labelSPCB.Text + ", " + labelSPCB.Text + ", 1, 1, " + intClassID.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //取得商品ID
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sSPID = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //表单汇总
                sqlComm.CommandText = "INSERT INTO 库存商品组装汇总表 (单据编号, 成品库房ID, 商品ID, 商品编号, 商品名称, 商品数量, 组装费用, 备注, BeActive, 日期, 操作员ID, 业务员ID) VALUES (N'" + strCount + "' ," + intKFID.ToString() + ", " + sSPID + ", N'" + textBoxSPBH.Text + "', N'" + textBoxSPMC.Text + "', " + numericUpDownCPSL.Value.ToString() + ", " + numericUpDownZZFY.Value.ToString() + ", N'" + textBoxBZ.Text + "', 1, '" + strDateSYS + "', "+intUserID.ToString()+", "+comboBoxYWY.SelectedValue.ToString()+")";
                sqlComm.ExecuteNonQuery();

                //取得单据号 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //表单明细
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    sqlComm.CommandText = "INSERT INTO 库存商品组装明细表 (单据ID, 组件ID, 库房ID, 组件数量, 成本单价, 成本金额, 备注) VALUES ("+sBillNo+", "+dataGridViewDJMX.Rows[i].Cells[10].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[11].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[6].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[7].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[8].Value.ToString()+", N'"+dataGridViewDJMX.Rows[i].Cells[9].Value.ToString()+"')";
                    sqlComm.ExecuteNonQuery();
                }

                //库存
                sqlComm.CommandText = "INSERT INTO 库存表 (库房ID, 商品ID, 库存数量, 库存金额, 库存成本价, BeActive) VALUES (" + intKFID.ToString() + ", " + sSPID + ", " + numericUpDownCPSL.Value.ToString() + ", " + fTemp .ToString()+ ", "+labelSPCB.Text+", 1)";
                sqlComm.ExecuteNonQuery();

                //商品历史纪录
                dKUL1 = numericUpDownCPSL.Value;
                dKCCBJ1 = Convert.ToDecimal(labelSPCB.Text);
                dKCJE1 = dKUL * dKCCBJ;

                sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 业务员ID, 单据编号, 摘要, 入库数量, 入库单价, 入库金额, 总结存数量, 总结存金额, BeActive) VALUES ('" + strDateSYS + "', " + sSPID + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'库存商品组装', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCJE1.ToString() + ", 1)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 业务员ID, 单据编号, 摘要, 入库数量, 入库单价, 入库金额, 库房结存数量, 库房结存金额, BeActive) VALUES (" + intKFID.ToString() + ", '" + strDateSYS + "', " + sSPID + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'库存商品组装', " + dKUL1.ToString() + ", " + dKCCBJ.ToString() + ", " + dKCJE1.ToString() + ", " + dKUL1.ToString() + ", " + dKCJE1.ToString() + ", 1)";
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

            //MessageBox.Show(" 库存商品组装制单保存成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = " 库存商品组装制单：" + labelDJBH.Text;
            isSaved = true;


            if (MessageBox.Show("库存商品组装制单保存成功，是否关闭制单窗口", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }


        }


        private void numericUpDownCPSL_ValueChanged(object sender, EventArgs e)
        {
            countAmount();
        }

        private void FormKCSPZZ_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
            {
                return;
            }

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

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("库存商品组装制单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "库存商品组装制单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";商品名称：" + textBoxSPMC.Text + "(编号:" + textBoxSPBH.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("库存商品组装制单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "库存商品组装制单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";商品名称：" + textBoxSPMC.Text + "(编号:" + textBoxSPBH.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void textBoxSPLB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getClassInformation(10, textBoxSPLB.Text) == 0) //失败
                {
                    textBoxSPLB.Text = "";
                    intClassID = 0;
                }
                else
                {
                    intClassID = cGetInformation.iClassNumber;
                    textBoxSPLB.Text = cGetInformation.strClassName;

                    //得到缺省库
                    if (intClassID == 0)
                        return;

                    sqlConn.Open();
                    sqlComm.CommandText = "SELECT 商品分类表.ID, 商品分类表.分类编号, 商品分类表.分类名称, 商品分类表.库房ID, 库房表.库房编号, 库房表.库房名称 FROM 商品分类表 INNER JOIN 库房表 ON 商品分类表.库房ID = 库房表.ID WHERE (商品分类表.BeActive = 1) AND (商品分类表.ID = " + intClassID.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    while (sqldr.Read())
                    {
                        intKFID = Convert.ToInt32(sqldr.GetValue(3).ToString());
                        textBoxKFBH.Text = sqldr.GetValue(4).ToString();
                        textBoxKFMC.Text = sqldr.GetValue(5).ToString();
                    }
                    sqldr.Close();
                    sqlConn.Close();
                }
            }
        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                sqlConn.Open();
                sqlComm.CommandText = "SELECT 商品编号, 商品名称, 商品规格 FROM 商品表 WHERE (商品编号 = N'" + textBoxSPBH.Text + "') AND (beactive = 1)";
                sqldr = sqlComm.ExecuteReader();

                if (sqldr.HasRows) //编码重复
                {
                    sqldr.Read();
                    MessageBox.Show("组装商品编码重复，商品名称为：" + sqldr.GetValue(1).ToString() + "，规格：" + sqldr.GetValue(2).ToString(), "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxSPBH.Text = "";
                }
                sqlConn.Close();
            }
        }

        private void textBoxSPLB_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getClassInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                intClassID = cGetInformation.iClassNumber;
                textBoxSPLB.Text = cGetInformation.strClassName;

                //得到缺省库
                if (intClassID == 0)
                    return;

                sqlConn.Open();
                sqlComm.CommandText = "SELECT 商品分类表.ID, 商品分类表.分类编号, 商品分类表.分类名称, 商品分类表.库房ID, 库房表.库房编号, 库房表.库房名称 FROM 商品分类表 INNER JOIN 库房表 ON 商品分类表.库房ID = 库房表.ID WHERE (商品分类表.BeActive = 1) AND (商品分类表.ID = " + intClassID .ToString()+ ")";
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    intKFID = Convert.ToInt32(sqldr.GetValue(3).ToString());
                    textBoxKFBH.Text = sqldr.GetValue(4).ToString();
                    textBoxKFMC.Text = sqldr.GetValue(5).ToString();
                }
                sqldr.Close();
                sqlConn.Close();
            }
        }



    }
}