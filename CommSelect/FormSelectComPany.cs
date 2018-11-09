using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSelectComPany : Form
    {
        public string strConn = "";
        public string strSelectText = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iCompanyNumber = 0;
        public string strCompanyName = "";
        public string strCompanyCode = "";
        public string strCompanySH = "";
        public decimal dCompanyYFZK = 0;
        public decimal dCompanyYSZK = 0;
        public string sCompanyYWY = "";
        public int iBMID = 0;

        private DataView dvSelect;
        
        public FormSelectComPany()
        {
            InitializeComponent();
        }

        private void FormSelectComPany_Load(object sender, EventArgs e)
        {
            if (strSelectText == "")
            {
                this.Close();
            }

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            //初始化单位列表
            sqlComm.CommandText = strSelectText;
            sqlConn.Open();
            if (dSet.Tables.Contains("单位表")) dSet.Tables.Remove("单位表");
            sqlDA.Fill(dSet, "单位表");

            dvSelect = new DataView(dSet.Tables["单位表"]);
            dataGridViewDWLB.DataSource = dvSelect;

            dataGridViewDWLB.Columns[0].Visible = false;
            dataGridViewDWLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDWLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDWLB.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDWLB.Columns[4].Visible=false;
            dataGridViewDWLB.Columns[5].Visible= false;
            dataGridViewDWLB.Columns[6].Visible = false;
            dataGridViewDWLB.Columns[7].Visible = false;
            sqlConn.Close();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iCompanyNumber = 0;
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dataGridViewDWLB.SelectedRows.Count < 1)
            {
                iCompanyNumber = 0;
                this.Close();
                return; ;
            }
            iCompanyNumber = Int32.Parse(dataGridViewDWLB.SelectedRows[0].Cells[0].Value.ToString());
            strCompanyName = dataGridViewDWLB.SelectedRows[0].Cells[2].Value.ToString();
            strCompanyCode = dataGridViewDWLB.SelectedRows[0].Cells[1].Value.ToString();
            strCompanySH = dataGridViewDWLB.SelectedRows[0].Cells[3].Value.ToString();
            if (dataGridViewDWLB.SelectedRows[0].Cells[4].Value.ToString() != "")
                dCompanyYFZK = Convert.ToDecimal(dataGridViewDWLB.SelectedRows[0].Cells[4].Value.ToString());
            if (dataGridViewDWLB.SelectedRows[0].Cells[5].Value.ToString() != "")
                dCompanyYSZK = Convert.ToDecimal(dataGridViewDWLB.SelectedRows[0].Cells[5].Value.ToString());

            sCompanyYWY = dataGridViewDWLB.SelectedRows[0].Cells[6].Value.ToString();
            try
            {
                iBMID = int.Parse(dataGridViewDWLB.SelectedRows[0].Cells[7].Value.ToString());
            }
            catch
            {
                iBMID = 0;
            }
            this.Close();
        }

        private void dataGridViewDWLB_DoubleClick(object sender, EventArgs e)
        {
            btnSelect_Click(null,null);
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            dvSelect.RowFilter = "";
            dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
            dataGridViewDWLB.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (textBoxDWMC.Text.Trim() == "")
                return;

            dvSelect.RowStateFilter = DataViewRowState.CurrentRows;
            if (radioButtonAll.Checked)
            {
                dvSelect.RowFilter = "单位名称 LIKE '%" + textBoxDWMC.Text.Trim() + "%'";
            }
            if (radioButtonF.Checked)
            {
                dvSelect.RowFilter = "单位名称 LIKE '" + textBoxDWMC.Text.Trim() + "%'";
            }
            if (radioButtonE.Checked)
            {
                dvSelect.RowFilter = "单位名称 LIKE '%" + textBoxDWMC.Text.Trim() + "'";
            }
            dataGridViewDWLB.Focus();

        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            if (textBoxDWMC.Text.Trim() == "")
                return;

            int iRow = -1; 
            string sTemp="";

            for (int i = 0; i < dataGridViewDWLB.Rows.Count; i++)
            {
                if (radioButtonAll.Checked)  //全匹配
                {
                    sTemp=dataGridViewDWLB.Rows[i].Cells[2].Value.ToString();
                    if (sTemp.IndexOf(textBoxDWMC.Text.Trim()) != -1)
                    {
                        iRow = i;
                        break;
                    }
                }

                if (radioButtonF.Checked) //前匹配
                {
                    sTemp = dataGridViewDWLB.Rows[i].Cells[2].Value.ToString();
                    if (sTemp.IndexOf(textBoxDWMC.Text.Trim()) == 0)
                    {
                        iRow = i;
                        break;
                    }
                }

                if (radioButtonE.Checked) //后匹配
                {
                    sTemp = dataGridViewDWLB.Rows[i].Cells[2].Value.ToString().Trim();
                    if (sTemp.Length < textBoxDWMC.Text.Trim().Length)
                        break;

                    if (sTemp.LastIndexOf(textBoxDWMC.Text.Trim()) == sTemp.Length - textBoxDWMC.Text.Trim().Length)
                    {
                        iRow = i;
                        break;
                    }
                }


            }


            if (iRow != -1)
            {
                //dataGridViewDWLB.Rows[iRow].Selected = false;
                dataGridViewDWLB.Rows[iRow].Selected = true;
                dataGridViewDWLB.FirstDisplayedScrollingRowIndex = iRow;
            }
            else
            {
                if (dataGridViewDWLB.Rows.Count > 0)
                {
                    dataGridViewDWLB.Rows[0].Selected = true;
                    dataGridViewDWLB.FirstDisplayedScrollingRowIndex = 0;
                }
            }
            dataGridViewDWLB.Focus();

        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.F9)
            {
                btnAll_Click(null, null);
                return true;
            }
            if (keyData == Keys.F7)
            {
                btnSearch_Click(null, null);
                return true;
            }
            if (keyData == Keys.F8)
            {
                btnLocation_Click(null, null);
                return true;
            }
            if (keyData == Keys.Enter && dataGridViewDWLB.Focused)
            {
                //System.Windows.Forms.SendKeys.Send("{tab}");
                btnSelect_Click(null, null);//
                return true;
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                btnSearch_Click(null, null);
            }
        }


    }
}