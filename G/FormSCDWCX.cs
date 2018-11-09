using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSCDWCX : Form
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



        private ClassGetInformation cGetInformation;
        
        public FormSCDWCX()
        {
            InitializeComponent();
        }

        private void FormSCDWCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;
            toolStripButtonGD_Click(null, null);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "删除单位查询;当前日期：" + labelSCJZRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMM, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "删除单位查询;当前日期：" + labelSCJZRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMM, strT, false, intUserLimit);
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            //初始列表
            sqlComm.CommandText = "SELECT 单位编号, 单位名称, 税号, 银行账号, 联系人, 地区名称, 地址 FROM 单位表 WHERE (BeActive = 0)";
            if (textBoxMC.Text.Trim() != "")
                sqlComm.CommandText += " AND ((单位名称 LIKE N'%" + textBoxMC.Text.Trim() + "%') OR (助记码 LIKE N'%" + textBoxMC.Text.Trim() + "%'))";

            if (dSet.Tables.Contains("单位表")) dSet.Tables.Remove("单位表");
            sqlDA.Fill(dSet, "单位表");
            dataGridViewDJMM.DataSource = dSet.Tables["单位表"];

            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelSCJZRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            toolStripStatusLabelC.Text = "共有" + dSet.Tables["单位表"].Rows.Count.ToString() + "条记录";
        }
    }
}