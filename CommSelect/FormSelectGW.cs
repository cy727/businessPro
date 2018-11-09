using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSelectGW : Form
    {
        public string strConn = "";
        public string strSelectText = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iGWNumber = 0;
        public string strGWName = "";
        public string strGWCode = "";

        public FormSelectGW()
        {
            InitializeComponent();
        }

        private void FormSelectGW_Load(object sender, EventArgs e)
        {
            if (strSelectText == "")
            {
                this.Close();
            }

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            sqlComm.CommandText = strSelectText;
            sqlConn.Open();
            if (dSet.Tables.Contains("部门表")) dSet.Tables.Remove("部门表");
            sqlDA.Fill(dSet, "部门表");

            dataGridViewGWLB.DataSource = dSet.Tables["部门表"];
            dataGridViewGWLB.Columns[0].Visible = false;
            dataGridViewGWLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewGWLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            sqlConn.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dataGridViewGWLB.SelectedRows.Count < 1)
            {
                iGWNumber = 0;
                this.Close();
                return; ;
            }
            iGWNumber = Int32.Parse(dataGridViewGWLB.SelectedRows[0].Cells[0].Value.ToString());
            strGWName = dataGridViewGWLB.SelectedRows[0].Cells[2].Value.ToString();
            strGWCode = dataGridViewGWLB.SelectedRows[0].Cells[1].Value.ToString();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iGWNumber = 0;
            this.Close();
        }

        private void dataGridViewGWLB_DoubleClick(object sender, EventArgs e)
        {
            btnSelect_Click(null, null);
        }


    }
}