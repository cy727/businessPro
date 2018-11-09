using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSelectKF : Form
    {
        public string strConn = "";
        public string strSelectText = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iKFNumber = 0;
        public string strKFName = "";
        public string strKFCode = "";

        public FormSelectKF()
        {
            InitializeComponent();
        }

        private void FormSelectKF_Load(object sender, EventArgs e)
        {
            if (strSelectText == "")
            {
                this.Close();
            }

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            //初始化库房列表
            sqlComm.CommandText = strSelectText;
            sqlConn.Open();
            if (dSet.Tables.Contains("库房表")) dSet.Tables.Remove("库房表");
            sqlDA.Fill(dSet, "库房表");

            dataGridViewKFLB.DataSource = dSet.Tables["库房表"];
            dataGridViewKFLB.Columns[0].Visible = false;
            dataGridViewKFLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewKFLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            sqlConn.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dataGridViewKFLB.SelectedRows.Count < 1)
            {
                iKFNumber = 0;
                this.Close();
                return; ;
            }
            iKFNumber = Int32.Parse(dataGridViewKFLB.SelectedRows[0].Cells[0].Value.ToString());
            strKFName = dataGridViewKFLB.SelectedRows[0].Cells[2].Value.ToString();
            strKFCode = dataGridViewKFLB.SelectedRows[0].Cells[1].Value.ToString();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iKFNumber = 0;
            this.Close();
        }

        private void dataGridViewKFLB_DoubleClick(object sender, EventArgs e)
        {
            btnSelect_Click(null, null);
        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && dataGridViewKFLB.Focused)
            {
                //System.Windows.Forms.SendKeys.Send("{tab}");
                btnSelect_Click(null, null);//
                return true;
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}