using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSelectBM : Form
    {
        public string strConn = "";
        public string strSelectText = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iBMNumber = 0;
        public string strBMName = "";
        public string strBMCode = "";
        
        public FormSelectBM()
        {
            InitializeComponent();
        }

        private void FormSelectBM_Load(object sender, EventArgs e)
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
            if (dSet.Tables.Contains("岗位表")) dSet.Tables.Remove("岗位表");
            sqlDA.Fill(dSet, "岗位表");

            dataGridViewBMLB.DataSource = dSet.Tables["岗位表"];
            dataGridViewBMLB.Columns[0].Visible = false;
            dataGridViewBMLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewBMLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            sqlConn.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dataGridViewBMLB.SelectedRows.Count < 1)
            {
                iBMNumber = 0;
                this.Close();
                return; ;
            }
            iBMNumber = Int32.Parse(dataGridViewBMLB.SelectedRows[0].Cells[0].Value.ToString());
            strBMName = dataGridViewBMLB.SelectedRows[0].Cells[2].Value.ToString();
            strBMCode = dataGridViewBMLB.SelectedRows[0].Cells[1].Value.ToString();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iBMNumber = 0;
            this.Close();
        }

        private void dataGridViewBMLB_DoubleClick(object sender, EventArgs e)
        {
            btnSelect_Click(null, null);
        }
    }
}