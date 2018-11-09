using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormRZCX : Form
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


        private int iSupplyCompany = 0;
        private ClassGetInformation cGetInformation;


        public FormRZCX()
        {
            InitializeComponent();
        }

        private void FormRZCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;


            //得到开始时间
            sqlConn.Open();
            //sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
            sqlComm.CommandText = "SELECT GETDATE()";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");

            }
            sqldr.Close();
            sqlConn.Close();

            //初始化员工列表
            sqlComm.CommandText = "SELECT ID, 职员编号, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");

            object[] OTemp = new object[3];
            OTemp[0] = 0;
            OTemp[1] = "全部";
            OTemp[2] = "全部";
            dSet.Tables["职员表"].Rows.Add(OTemp);

            comboBoxCZY.DataSource = dSet.Tables["职员表"];
            comboBoxCZY.DisplayMember = "职员姓名";
            comboBoxCZY.ValueMember = "ID";
            comboBoxCZY.SelectedIndex = comboBoxCZY.Items.Count - 1;
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;


        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT DISTINCT 商品历史账表.日期, 职员表.职员姓名 AS 业务员, 商品历史账表.单据编号, 商品历史账表.摘要 FROM 商品历史账表 INNER JOIN 职员表 ON 商品历史账表.业务员ID = 职员表.ID WHERE (商品历史账表.日期 >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (商品历史账表.日期 <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102))";

            if (comboBoxCZY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " AND (商品历史账表.业务员ID = " + comboBoxCZY.SelectedValue.ToString() + ") ";
            }
            sqlComm.CommandText += " ORDER BY 商品历史账表.日期";


            if (dSet.Tables.Contains("日志表")) dSet.Tables.Remove("日志表");
            sqlDA.Fill(dSet, "日志表");

            sqlConn.Close();

            dataGridView1.DataSource = dSet.Tables["日志表"];

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "单据日志;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, true);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "单据日志;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, false);
        }

 
    }
}