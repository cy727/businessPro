using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSelectZB : Form
    {
        public string strConn = "";
        public string strSelectText = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iZBNumber = 0;
        public string strZBName = "";
        public string strZBCode = "";
        public decimal dZBKL = 100;
        
        public FormSelectZB()
        {
            InitializeComponent();
        }

        private void FormSelectZB_Load(object sender, EventArgs e)
        {
            if (strSelectText == "")
            {
                this.Close();
            }

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            //初始化列表
            sqlComm.CommandText = strSelectText;
            sqlConn.Open();
            if (dSet.Tables.Contains("账簿表")) dSet.Tables.Remove("账簿表");
            sqlDA.Fill(dSet, "账簿表");

            dataGridViewLB.DataSource = dSet.Tables["账簿表"];
            dataGridViewLB.Columns[0].Visible = false;
            dataGridViewLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewLB.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            sqlConn.Close();


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iZBNumber = 0;
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dataGridViewLB.SelectedRows.Count < 1)
            {
                iZBNumber = 0;
                this.Close();
                return; ;
            }
            iZBNumber = Int32.Parse(dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString());
            strZBName = dataGridViewLB.SelectedRows[0].Cells[2].Value.ToString();
            strZBCode = dataGridViewLB.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridViewLB.SelectedRows[0].Cells[3].Value.ToString() != "")
                dZBKL = Convert.ToDecimal(dataGridViewLB.SelectedRows[0].Cells[3].Value.ToString());

            this.Close();
        }

        private void dataGridViewLB_DoubleClick(object sender, EventArgs e)
        {
            btnSelect_Click(null, null);
        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && dataGridViewLB.Focused)
            {
                btnSelect_Click(null, null);//弹出修改窗口
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}