using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSelectGDFP : Form
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
        public decimal dSUMJE = 0;
        private DataView dvSelect;

        private ClassGetInformation cGetInformation;

        public FormSelectGDFP()
        {
            InitializeComponent();
        }

        private void FormSelectGDFP_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            dataGridViewLB.DataSource = dtSelect;

            switch (iSelectStyle)
            {
                case 1: //按单据勾兑
                    dataGridViewLB.Columns[4].Visible = false;
                    dataGridViewLB.Columns[5].Visible = false;
                    dataGridViewLB.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.ShowCellErrors = true;
                    this.Text = "按单据勾兑";

                    break;

                case 2://按明细勾兑
                    dataGridViewLB.Columns[6].Visible = false;
                    dataGridViewLB.Columns[7].Visible = false;
                    dataGridViewLB.Columns[8].Visible = false;
                    dataGridViewLB.Columns[9].Visible = false;
                    dataGridViewLB.Columns[1].ReadOnly = true;
                    dataGridViewLB.Columns[2].ReadOnly = true;
                    dataGridViewLB.Columns[3].ReadOnly = true;
                    dataGridViewLB.Columns[4].ReadOnly = true;
                    dataGridViewLB.Columns[5].ReadOnly = true;
                    dataGridViewLB.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.ShowCellErrors = true;

                    this.Text = "按商品明细勾兑";
                    break;

                case 3://按单据勾兑
                    dataGridViewLB.Columns[6].Visible = false;
                    dataGridViewLB.Columns[1].ReadOnly = true;
                    dataGridViewLB.Columns[2].ReadOnly = true;
                    dataGridViewLB.Columns[3].ReadOnly = true;
                    dataGridViewLB.Columns[4].ReadOnly = true;
                    dataGridViewLB.Columns[5].ReadOnly = true;
                    dataGridViewLB.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.ShowCellErrors = true;

                    this.Text = "按单据勾兑";
                    break;

                case 4://按明细勾兑

                    dataGridViewLB.Columns[8].Visible = false;
                    dataGridViewLB.Columns[9].Visible = false;
                    dataGridViewLB.Columns[5].Visible = false;
                    dataGridViewLB.Columns[1].ReadOnly = true;
                    dataGridViewLB.Columns[2].ReadOnly = true;
                    dataGridViewLB.Columns[3].ReadOnly = true;
                    dataGridViewLB.Columns[4].ReadOnly = true;
                    dataGridViewLB.Columns[5].ReadOnly = true;
                    dataGridViewLB.Columns[6].ReadOnly = true;
                    dataGridViewLB.Columns[7].ReadOnly = true;

                    dataGridViewLB.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                    dataGridViewLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewLB.ShowCellErrors = true;

                    this.Text = "按商品明细勾兑";
                    break;


                default:
                    this.Dispose();
                    return;
            }

        }

        private void dataGridViewLB_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        //return true 正确  false 错误
        private bool countAmount()
        {
            decimal fSum = 0;

            bool bCheck = true;

            switch (iSelectStyle)
            {
                case 1: //按单据勾兑
                    for (int i = 0; i < dataGridViewLB.Rows.Count; i++)
                    {
                        if (dataGridViewLB.Rows[i].IsNewRow)
                            continue;

                        //选择
                        if (dataGridViewLB.Rows[i].Cells[0].Value.ToString() == "")
                        {
                            dataGridViewLB.Rows[i].Cells[0].Value = 0;
                        }

                        if (Convert.ToBoolean(dataGridViewLB.Rows[i].Cells[0].Value))
                        {
                            fSum += Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[3].Value);
                        }
                    }
                    break;
                case 2://按明细勾兑

                    for (int i = 0; i < dataGridViewLB.Rows.Count; i++)
                    {
                        if (dataGridViewLB.Rows[i].IsNewRow)
                            continue;

                        //选择
                        if (dataGridViewLB.Rows[i].Cells[0].Value.ToString() == "")
                        {
                            dataGridViewLB.Rows[i].Cells[0].Value = 0;
                        }

                        if (Convert.ToBoolean(dataGridViewLB.Rows[i].Cells[0].Value))
                        {
                            fSum += Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[5].Value);
                        }
                    }

                    break;

                case 3: //按单据勾兑
                    for (int i = 0; i < dataGridViewLB.Rows.Count; i++)
                    {
                        if (dataGridViewLB.Rows[i].IsNewRow)
                            continue;

                        //选择
                        if (dataGridViewLB.Rows[i].Cells[0].Value.ToString() == "")
                        {
                            dataGridViewLB.Rows[i].Cells[0].Value = 0;
                        }

                        if (Convert.ToBoolean(dataGridViewLB.Rows[i].Cells[0].Value))
                        {
                            fSum += Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[2].Value);
                        }
                    }
                    break;

                case 4://按明细勾兑

                    for (int i = 0; i < dataGridViewLB.Rows.Count; i++)
                    {
                        if (dataGridViewLB.Rows[i].IsNewRow)
                            continue;

                        //选择
                        if (dataGridViewLB.Rows[i].Cells[0].Value.ToString() == "")
                        {
                            dataGridViewLB.Rows[i].Cells[0].Value = 0;
                        }

                        if (Convert.ToBoolean(dataGridViewLB.Rows[i].Cells[0].Value))
                        {
                            fSum += Convert.ToDecimal(dataGridViewLB.Rows[i].Cells[4].Value);
                        }
                    }

                    break;


            }

            toolStripStatusLabelJE.Text = fSum.ToString();
            dSUMJE = fSum;

            return bCheck;


        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            countAmount();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dSUMJE = 0;
        }

        private void btnALL_Click(object sender, EventArgs e)
        {
            //dataGridViewLB.CellValidating -= dataGridViewLB_CellValidating;

            for (int i = 0; i < dataGridViewLB.Rows.Count; i++)
            {
                dataGridViewLB.Rows[i].Cells[0].Value = 1;
            }
            countAmount();
            //dataGridViewLB.CellValidating += dataGridViewLB_CellValidating;
        }

        private void dataGridViewLB_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0)
                return;
            this.btnALL.Focus();
        }
    }
}