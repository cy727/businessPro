using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKCSPZMCL : Form
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

        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;
        
        public FormKCSPZMCL()
        {
            InitializeComponent();
        }

        private void FormKCSPZMCL_Load(object sender, EventArgs e)
        {
            int i;

            this.Left = 1;
            this.Top = 1;

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

            //初始化商品列表
            sqlComm.CommandText = "SELECT 库存商品账目处理明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 库存商品账目处理明细表.库存成本价, 库存商品账目处理明细表.库存数量, 库存商品账目处理明细表.库存金额, 库存商品账目处理明细表.原库存成本价, 库存商品账目处理明细表.原库存数量, 库存商品账目处理明细表.原库存金额, 商品表.ID AS 商品ID, 库房表.ID AS 库房ID FROM 库存商品账目处理明细表 INNER JOIN 商品表 ON 库存商品账目处理明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 库存商品账目处理明细表.库房ID = 库房表.ID WHERE (库存商品账目处理明细表.ID = 0)";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[12].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;
            dataGridViewDJMX.Columns[14].Visible = false;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[4].ReadOnly = true;
            dataGridViewDJMX.Columns[9].ReadOnly = true;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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
            sqlComm.CommandText = "SELECT 库存商品账目处理汇总表.单据编号, 库存商品账目处理汇总表.日期, 职员表.职员姓名, 操作员.职员姓名 AS 操作员, 库存商品账目处理汇总表.备注 FROM 库存商品账目处理汇总表 INNER JOIN 职员表 ON 库存商品账目处理汇总表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 库存商品账目处理汇总表.操作员ID = 操作员.ID WHERE (库存商品账目处理汇总表.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();


                this.Text = "库存商品账目处理：" + labelDJBH.Text;
            }
            sqldr.Close();

            //初始化商品列表
            sqlComm.CommandText = "SELECT 库存商品账目处理明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 库房表.库房编号, 库房表.库房名称, 库存商品账目处理明细表.库存成本价, 库存商品账目处理明细表.库存数量, 库存商品账目处理明细表.库存金额, 库存商品账目处理明细表.原库存成本价, 库存商品账目处理明细表.原库存数量, 库存商品账目处理明细表.原库存金额, 商品表.ID AS 商品ID, 库房表.ID AS 库房ID FROM 库存商品账目处理明细表 INNER JOIN 商品表 ON 库存商品账目处理明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 库存商品账目处理明细表.库房ID = 库房表.ID WHERE (库存商品账目处理明细表.单据ID  = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[12].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;
            dataGridViewDJMX.Columns[14].Visible = false;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iCommNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = cGetInformation.decCommKCCBJ;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommKCCBJ;

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;

                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value.ToString() == "")
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;

                    cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value));
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.dKCL;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.dKCL;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.dKCJE;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCJE;

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
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;

                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == "")
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;

                    cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value));
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.dKCL;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.dKCL;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.dKCJE;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCJE;

                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[7];
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

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";

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

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iCommNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommKCCBJ.ToString();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = cGetInformation.decCommKCCBJ.ToString();

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.dKCL;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.dKCL;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.dKCJE;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCJE;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;

                case 1: //商品名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
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
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == cGetInformation.iCommNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strCommCount;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommKCCBJ.ToString();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[10].Value = cGetInformation.decCommKCCBJ.ToString();

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.dKCL;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.dKCL;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.dKCJE;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCJE;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    }
                    break;
                case 5: //库房编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
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

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value.ToString() == cGetInformation.iKFNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.dKCL;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.dKCL;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.dKCJE;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCJE;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;
                case 6: //库房名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
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

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value.ToString() == cGetInformation.iKFNumber.ToString())
                            break;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value = cGetInformation.strKFName;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[13].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[14].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[8].Value = cGetInformation.dKCL;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = cGetInformation.dKCL;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.dKCJE;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[12].Value = cGetInformation.dKCJE;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }

                    break;
                case 7:  //库存成本价
                    decimal dOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Decimal.TryParse(e.FormattedValue.ToString(), out dOut))
                    {
                        if (dOut < 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "商品成本价输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "商品成本价输入类型错误";
                        e.Cancel = true;
                    }
                    break;
                default:
                    break;

            }
            dataGridViewDJMX.EndEdit();
        }

        private bool countAmount()
        {
            decimal fTemp, fTemp1;
            bool bCheck = true;
            decimal fCount = 0;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[1].ErrorText = "输入调整商品";
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "输入调整商品";
                    bCheck = false;
                }

                if (dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[4].ErrorText = "输入调整商品库房";
                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "输入调整商品库房";
                    bCheck = false;
                }


                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[8].ErrorText = "输入调整商品库存数量";
                    bCheck = false;
                }

                if (dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[7].ErrorText = "输入调整商品库存成本价";
                    bCheck = false;
                }

                if (!bCheck)
                    continue;

                //库寸金额
                dataGridViewDJMX.Rows[i].Cells[9].Value = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value) * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);

                fCount ++;

            }
            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            toolStripStatusLabelMXJLS.Text = fCount.ToString();
            return bCheck;
        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i,j;
            decimal dKUL = 0 ;
            decimal dKUL1 = 0;

            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("库存商品账目处理单已经保存,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("库存商品账目处理单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("没有库存商品账目单商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("请检库存商品账目处理单内容，是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;



            string strCount = "", strDateSYS = "", strKey = "CTZ";
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
                sqlComm.CommandText = "INSERT INTO 库存商品账目处理汇总表 (单据编号, 日期, 业务员ID, 操作员ID, 备注, BeActive) VALUES (N'"+strCount+"', '"+strDateSYS+"', "+comboBoxYWY.SelectedValue.ToString()+", "+intUserID.ToString()+", N'"+textBoxBZ.Text+"', 1)";
                sqlComm.ExecuteNonQuery();

                //取得单据号 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //单据明细
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    sqlComm.CommandText = "INSERT INTO 库存商品账目处理明细表 (单据ID, 商品ID, 库房ID, 原库存成本价, 原库存数量, 原库存金额, 库存成本价, 库存数量, 库存金额) VALUES ("+sBillNo+", "+dataGridViewDJMX.Rows[i].Cells[13].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[14].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[10].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[11].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[12].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[7].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[8].Value.ToString()+", "+dataGridViewDJMX.Rows[i].Cells[9].Value.ToString()+")";
                    sqlComm.ExecuteNonQuery();

                    //总库存
                    sqlComm.CommandText = "UPDATE 商品表 SET 库存成本价 = " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ", 库存数量 = " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", 库存金额 = " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();


                    sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 业务员ID, 单据编号, 摘要, 总结存数量, 总结存金额, BeActive) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'库存商品账目处理', " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", 1)";
                    sqlComm.ExecuteNonQuery();


                    //分库存
                    sqlComm.CommandText = "UPDATE 库存表 SET 库存成本价 = " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ", 库存数量 = " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", 库存金额 = " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + " WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID , 日期, 商品ID, 业务员ID, 单据编号, 摘要, 库房结存数量, 库房结存金额, BeActive) VALUES (" + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + strCount + "', N'库存商品账目处理', " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ", 1)";
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

            //MessageBox.Show("库存商品账目处理单保存成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            labelDJBH.Text = strCount;
            this.Text = "库存商品账目处理单：" + labelDJBH.Text;
            isSaved = true;

            bool bClose = false;
            if (MessageBox.Show("库存商品账目处理单保存成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                bClose=true;
            }

            if (MessageBox.Show("是否继续开始另一份单椐？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                MDIBusiness mdiT = (MDIBusiness)this.MdiParent;
                mdiT.商品账目处理AToolStripMenuItem_Click(null, null);
            }

            if (bClose)
                this.Close();


        }

        private void FormKCSPZMCL_FormClosing(object sender, FormClosingEventArgs e)
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
            if (!countAmount())
            {
                MessageBox.Show("库存商品账目处理单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "库存商品账目处理单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            if (!countAmount())
            {
                MessageBox.Show("库存商品账目处理单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "库存商品账目处理单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }



    }
}