using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSelectGD : Form
    {
        public string strConn = "";
        public string strSelectText = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iSelectStyle = 0;
        public DataTable dtSelect;
        public decimal dSUMJE=0;
        public int iSUMSELECT = 0;
        private DataView dvSelect;

        private ClassGetInformation cGetInformation;

        
        public FormSelectGD()
        {
            InitializeComponent();
            //cGetInformation = new ClassGetInformation(strConn);

        }

        private void FormSelectGD_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);


            dvSelect = new DataView(dtSelect);
            dataGridViewLB.DataSource = dvSelect;



            switch (iSelectStyle)
            {
                case 1: //按单据勾兑
                    dataGridViewLB.Columns[7].Visible = false;
                    dataGridViewLB.Columns[8].Visible = false;
                    dataGridViewLB.Columns[1].ReadOnly = true;
                    dataGridViewLB.Columns[2].ReadOnly = true;
                    dataGridViewLB.Columns[3].ReadOnly = true;
                    dataGridViewLB.Columns[4].ReadOnly = true;
                    dataGridViewLB.Columns[6].ReadOnly = true;
                    dataGridViewLB.Columns[9].ReadOnly = true;
                    dataGridViewLB.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.ShowCellErrors = true;

                    textBoxLB.Enabled = false;
                    textBoxMC.Enabled = false;

                    this.Text = "按单据勾兑";

                    break;

                case 2://按明细勾兑
                    dataGridViewLB.Columns[11].Visible = false;
                    dataGridViewLB.Columns[12].Visible = false;
                    dataGridViewLB.Columns[13].Visible = false;
                    dataGridViewLB.Columns[14].Visible = false;
                    dataGridViewLB.Columns[15].Visible = false;
                    dataGridViewLB.Columns[16].Visible = false;
                    dataGridViewLB.Columns[1].ReadOnly = true;
                    dataGridViewLB.Columns[2].ReadOnly = true;
                    dataGridViewLB.Columns[3].ReadOnly = true;
                    dataGridViewLB.Columns[4].ReadOnly = true;
                    dataGridViewLB.Columns[6].ReadOnly = true;
                    dataGridViewLB.Columns[7].ReadOnly = true;
                    dataGridViewLB.Columns[8].ReadOnly = true;
                    dataGridViewLB.Columns[10].ReadOnly = true;
                    dataGridViewLB.Columns[17].ReadOnly = true;
                    dataGridViewLB.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.ShowCellErrors = true;

                    this.Text = "按进货明细勾兑";
                    break;

                case 3://按明细数量勾兑
                    dataGridViewLB.Columns[14].Visible = false;
                    dataGridViewLB.Columns[15].Visible = false;
                    dataGridViewLB.Columns[16].Visible = false;
                    dataGridViewLB.Columns[17].Visible = false;
                    dataGridViewLB.Columns[18].Visible = false;
                    dataGridViewLB.Columns[19].Visible = false;
                    dataGridViewLB.Columns[1].ReadOnly = true;
                    dataGridViewLB.Columns[2].ReadOnly = true;
                    dataGridViewLB.Columns[3].ReadOnly = true;
                    dataGridViewLB.Columns[4].ReadOnly = true;
                    dataGridViewLB.Columns[6].ReadOnly = true;
                    dataGridViewLB.Columns[7].ReadOnly = true;
                    dataGridViewLB.Columns[9].ReadOnly = true;
                    dataGridViewLB.Columns[10].ReadOnly = true;
                    dataGridViewLB.Columns[11].ReadOnly = true;
                    dataGridViewLB.Columns[12].ReadOnly = true;
                    dataGridViewLB.Columns[13].ReadOnly = true;
                    dataGridViewLB.Columns[20].ReadOnly = true;
                    dataGridViewLB.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[20].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.ShowCellErrors = true;

                    this.Text = "按进货明细数量勾兑";
                    break;

                default:
                    this.Dispose();
                    return;
            }


        }

        private void dataGridViewLB_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridViewLB_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridViewLB.Rows[e.RowIndex].IsNewRow)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewLB);

           

            switch (iSelectStyle)
            {
                case 1: //按单据勾兑
                    switch (e.ColumnIndex)
                    {
                        case 5: //支付金额
                            decimal detOut = 0;

                            if (e.FormattedValue.ToString() == "") break;


                            if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                            {
                                detOut = Math.Round(detOut, 2);
                                if (detOut > Convert.ToDecimal(dataGridViewLB.Rows[e.RowIndex].Cells[6].Value))
                                {
                                    dataGridViewLB.Rows[e.RowIndex].Cells[5].ErrorText = "将支付金额大于未支付金额";
                                    e.Cancel = true;
                                }
                            }
                            else
                            {
                                dataGridViewLB.Rows[e.RowIndex].Cells[5].ErrorText = "将支付金额类型错误";
                                e.Cancel = true;
                            }
                            break;
                        default:
                            break;

                    }
                    break;

                case 2://按明细勾兑
                    switch (e.ColumnIndex)
                    {
                        case 0: //结清


                            if (!Convert.ToBoolean(dataGridViewLB.Rows[e.RowIndex].Cells[0].Value))
                                dataGridViewLB.Rows[e.RowIndex].Cells[9].Value = 0;
                            else
                                dataGridViewLB.Rows[e.RowIndex].Cells[9].Value = dataGridViewLB.Rows[e.RowIndex].Cells[10].Value;


                            break;

                        case 9: //支付金额
                            decimal detOut = 0;

                            if (e.FormattedValue.ToString() == "") break;


                            if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                            {
                                detOut = Math.Round(detOut, 2);
                                if (detOut > Convert.ToDecimal(dataGridViewLB.Rows[e.RowIndex].Cells[10].Value))
                                {
                                    dataGridViewLB.Rows[e.RowIndex].Cells[9].ErrorText = "将支付金额大于未支付金额";
                                    e.Cancel = true;
                                }
                            }
                            else
                            {
                                dataGridViewLB.Rows[e.RowIndex].Cells[9].ErrorText = "将支付金额类型错误";
                                e.Cancel = true;
                            }
                            break;
                        default:
                            break;

                    }
                    break;

                case 3://按明细数量勾兑
                    switch (e.ColumnIndex)
                    {
                        case 8: //支付数量
                            decimal detOut = 0;

                            if (e.FormattedValue.ToString() == "") break;


                            if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                            {
                                detOut = Math.Round(detOut, 2);
                                if (detOut < 0)
                                {
                                    dataGridViewLB.Rows[e.RowIndex].Cells[8].ErrorText = "将支付数量必须大于0";
                                    e.Cancel = true;
                                    break;
                                }
                                if (detOut > Convert.ToDecimal(dataGridViewLB.Rows[e.RowIndex].Cells[9].Value))
                                {
                                    dataGridViewLB.Rows[e.RowIndex].Cells[8].ErrorText = "将支付数量大于未支付数量";
                                    e.Cancel = true;
                                }

                            }
                            else
                            {
                                dataGridViewLB.Rows[e.RowIndex].Cells[8].ErrorText = "将支付金额数量错误";
                                e.Cancel = true;
                            }
                            break;
                        default:
                            break;

                    }
                    break;

                default:
                    break;
            }
            dataGridViewLB.EndEdit();
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

                                switch (iSelectStyle)
                                {
                                    case 1: //按单据勾兑
                                        switch (dv.CurrentCell.ColumnIndex)
                                        {
                                            case 0:
                                                dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[5];
                                                break;
                                            default:
                                                break;
                                        }
                                        break;
                                    case 2://按明细勾兑
                                        switch (dv.CurrentCell.ColumnIndex)
                                        {
                                            case 0:
                                                dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[9];
                                                break;
                                            default:
                                                break;
                                        }
                                        break;

                                    case 3://按明细数量勾兑
                                        switch (dv.CurrentCell.ColumnIndex)
                                        {
                                            case 0:
                                                dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[8];
                                                break;
                                            default:
                                                break;
                                        }
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

            this.dataGridViewLB.CellValidating -= dataGridViewLB_CellValidating;
            bool bCheck = true;

            iSUMSELECT = 0;
            switch (iSelectStyle)
            {
                case 1: //按单据勾兑
                    for (int i = 0; i < dataGridViewLB.Rows.Count; i++)
                    {
                        if (dataGridViewLB.Rows[i].IsNewRow)
                            continue;

                        //结清
                        if (dataGridViewLB.Rows[i].Cells[0].Value.ToString() == "" )
                        {
                            dataGridViewLB.Rows[i].Cells[0].Value = 0;
                        }

                        if (Convert.ToBoolean(dataGridViewLB.Rows[i].Cells[0].Value))
                        {
                            dataGridViewLB.Rows[i].Cells[5].Value = dataGridViewLB.Rows[i].Cells[6].Value;
                            iSUMSELECT ++;
                        }

                        //将支付金额
                        if (Math.Abs(Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[5].Value)) >  Math.Abs(Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[6].Value)))
                        {
                            dataGridViewLB.Rows[i].Cells[5].Value = dataGridViewLB.Rows[i].Cells[6].Value;
                        }
                        if (Math.Abs(Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[5].Value)) > 0)
                        {
                            fSum += Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[5].Value);
                        }

                    }
                    break;
                case 2://按明细勾兑

                    for (int i = 0; i < dataGridViewLB.Rows.Count; i++)
                    {
                        if (dataGridViewLB.Rows[i].IsNewRow)
                            continue;

                        //结清
                        if (dataGridViewLB.Rows[i].Cells[0].Value.ToString() == "")
                        {
                            dataGridViewLB.Rows[i].Cells[0].Value = 0;
                        }

                        if (Convert.ToBoolean(dataGridViewLB.Rows[i].Cells[0].Value))
                        {
                            dataGridViewLB.Rows[i].Cells[9].Value = dataGridViewLB.Rows[i].Cells[10].Value;
                            iSUMSELECT++;
                        }

                        //将支付金额
                        if (Math.Abs(Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[9].Value)) > Math.Abs((Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[10].Value))))
                        {
                            dataGridViewLB.Rows[i].Cells[9].Value = dataGridViewLB.Rows[i].Cells[10].Value;
                        }
                        if (Math.Abs(Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[9].Value)) > 0)
                        {
                            fSum += Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[9].Value);
                        }

                    }

                    break;

                case 3://按明细数量勾兑
                    for (int i = 0; i < dataGridViewLB.Rows.Count; i++)
                    {
                        if (dataGridViewLB.Rows[i].IsNewRow)
                            continue;

                        //结清
                        if (dataGridViewLB.Rows[i].Cells[0].Value.ToString() == "")
                        {
                            dataGridViewLB.Rows[i].Cells[0].Value = 0;
                        }

                        if (Convert.ToBoolean(dataGridViewLB.Rows[i].Cells[0].Value))
                        {
                            dataGridViewLB.Rows[i].Cells[8].Value = dataGridViewLB.Rows[i].Cells[9].Value;
                            iSUMSELECT++;
                        }

                        //将支付金额
                        fTemp = Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[10].Value) / Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[6].Value) * Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[8].Value);
                        dataGridViewLB.Rows[i].Cells[12].Value = fTemp;

                        if (Math.Abs(Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[12].Value)) > 0)
                        {
                            fSum += Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[12].Value);
                            fCount += Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[8].Value);
                        }

                    }
                    break;

            }
            this.dataGridViewLB.CellValidating += dataGridViewLB_CellValidating;
            dataGridViewLB.EndEdit();

            toolStripStatusLabelJE.Text = fSum.ToString();
            toolStripStatusLabelSL.Text = fCount.ToString();

            return bCheck;


        }

        private void dataGridViewLB_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {

            if (dataGridViewLB.Rows[e.RowIndex].IsNewRow)
                return;


            switch (iSelectStyle)
            {
                case 2://按明细勾兑
                    if (!Convert.ToBoolean(dataGridViewLB.Rows[e.RowIndex].Cells[0].Value))
                        dataGridViewLB.Rows[e.RowIndex].Cells[9].Value = 0;
                        
                    break;

                default:
                    break;
            }

            countAmount();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            int i;
            DataRow []drTemp1;

            btnSelect.Focus();

            
            dataGridViewLB.RowValidating -= dataGridViewLB_RowValidating;
            if (textBoxMC.Text == "")
            dvSelect.RowFilter = "";
            dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
            dataGridViewLB.RowValidating += dataGridViewLB_RowValidating;

            if (!countAmount())
                return;

            dSUMJE = Convert.ToDecimal(toolStripStatusLabelJE.Text);
            /*
            if (dSUMJE <= 0)
            {
                this.Close();
                return;
            }
            */
            switch (iSelectStyle)
            {
                case 1: //按单据勾兑
                    //数据表整理
                    for (i = 0; i < dataGridViewLB.RowCount; i++)
                    {
                        if (Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[5].Value) == 0)
                            continue;

                        //找到纪录
                        //drTemp = dtSelect.Rows.Find(dataGridViewLB.Rows[i].Cells[7].Value);
                        drTemp1 = dtSelect.Select("单据编号='" + dataGridViewLB.Rows[i].Cells[1].Value.ToString() + "' AND ID=" + dataGridViewLB.Rows[i].Cells[7].Value);
                        if (drTemp1.Length < 1)
                            continue;

                        DataRow drTemp = drTemp1[0];

                        drTemp[4] = Convert.ToDecimal(drTemp[4]) + Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[5].Value);
                        drTemp[6] = Convert.ToDecimal(drTemp[6]) - Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[5].Value);
                        drTemp[9] = Convert.ToDecimal(drTemp[9]) + Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[5].Value);

                        drTemp[5] = 0;
                        drTemp[8] = 1;
                    }
                    break;

                case 2://按明细勾兑
                    for (i = 0; i < dataGridViewLB.RowCount; i++)
                    {
                        if (Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[9].Value) == 0)
                            continue;

                        //找到纪录
                        //drTemp = dtSelect.Rows.Find(dataGridViewLB.Rows[i].Cells[13].Value);
                        drTemp1 = dtSelect.Select("单据编号='" + dataGridViewLB.Rows[i].Cells[1].Value.ToString() + "' AND ID=" + dataGridViewLB.Rows[i].Cells[13].Value);
                        if (drTemp1.Length < 1)
                            continue;

                        DataRow drTemp = drTemp1[0];
                        if (drTemp[8].ToString() == "")
                            drTemp[8] = 0;
                        if (drTemp[10].ToString() == "")
                            drTemp[10] = 0;

                        drTemp[8] = Convert.ToDecimal(drTemp[8]) + Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[9].Value);
                        drTemp[10] = Convert.ToDecimal(drTemp[10]) - Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[9].Value);
                        drTemp[17] = Convert.ToDecimal(drTemp[17]) + Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[9].Value);

                        drTemp[9] = 0;
                        drTemp[11] = 1;

                    }
                    break;

                case 3://按明细数量勾兑
                    for (i = 0; i < dataGridViewLB.RowCount; i++)
                    {
                        if (Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[8].Value) == 0)
                            continue;

                        //找到纪录
                        //drTemp = dtSelect.Rows.Find(dataGridViewLB.Rows[i].Cells[16].Value);
                        drTemp1 = dtSelect.Select("单据编号='" + dataGridViewLB.Rows[i].Cells[1].Value.ToString() + "' AND ID=" + dataGridViewLB.Rows[i].Cells[16].Value);
                        if (drTemp1.Length < 1)
                            continue;

                        DataRow drTemp = drTemp1[0];
                        if (drTemp[7].ToString() == "")
                            drTemp[7] = 0;
                        if (drTemp[9].ToString() == "")
                            drTemp[9] = 0;
                        drTemp[7] = Convert.ToDecimal(drTemp[7]) + Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[8].Value);
                        drTemp[9] = Convert.ToDecimal(drTemp[9]) - Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[8].Value);
                        drTemp[20] = Convert.ToDecimal(drTemp[20]) + Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[8].Value);

                        drTemp[8] = 0;
                        drTemp[14] = 1;

                    }
                    break;
            }
            this.Close();
        }

        private void textBoxMC_Validating(object sender, CancelEventArgs e)
        {
            int iTemp=0;
            dataGridViewLB.RowValidating -= dataGridViewLB_RowValidating;
            if (textBoxMC.Text == "")
            {
                dvSelect.RowFilter="";
                dvSelect.RowStateFilter=DataViewRowState.CurrentRows;
                dataGridViewLB.RowValidating += dataGridViewLB_RowValidating;
                return;
            }

            if (cGetInformation.getCommInformation(10, textBoxMC.Text) == 0) //失败
            {
                textBoxMC.Text = "";
            }
            else
            {
                iTemp = cGetInformation.iCommNumber;
                dvSelect.RowFilter = "商品ID="+iTemp.ToString();
                dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
            }
            dataGridViewLB.RowValidating += dataGridViewLB_RowValidating;
        }

        private void textBoxLB_Validating(object sender, CancelEventArgs e)
        {
            int iTemp = 0;
            dataGridViewLB.RowValidating -= dataGridViewLB_RowValidating;
            if (textBoxLB.Text == "")
            {
                dvSelect.RowFilter = "";
                dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
                dataGridViewLB.RowValidating += dataGridViewLB_RowValidating;
                return;
            }

            if (cGetInformation.getClassInformation(10, textBoxLB.Text) == 0) //失败
            {
                textBoxLB.Text = "";
            }
            else
            {
                iTemp = cGetInformation.iClassNumber;
                dvSelect.RowFilter = "分类编号=" + iTemp.ToString();
                dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
            }
            dataGridViewLB.RowValidating += dataGridViewLB_RowValidating;
        }

        private void btnALL_Click(object sender, EventArgs e)
        {
            dataGridViewLB.CellValidating -= dataGridViewLB_CellValidating;

            for (int i = 0; i < dataGridViewLB.Rows.Count; i++)
            {
                dataGridViewLB.Rows[i].Cells[0].Value = 1;
            }
            countAmount();
            dataGridViewLB.CellValidating += dataGridViewLB_CellValidating;
        }

        private void dataGridViewLB_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if(e.ColumnIndex != 0)
                return;

         
            dataGridViewLB.CurrentCell = dataGridViewLB.Rows[dataGridViewLB.CurrentCell.RowIndex].Cells[1];

            this.btnALL.Focus();
            this.dataGridViewLB.Focus();
            */

        }




    }
}