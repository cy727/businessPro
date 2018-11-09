using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormGJZPCX : Form
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


        public FormGJZPCX()
        {
            InitializeComponent();
        }

        private void FormGJZPCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            //得到开始时间
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1, "") == 0)
            {
                //return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(10, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                }
            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(12, textBoxDWMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    textBoxDWBH.Text = "";
                    textBoxDWMC.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 购进商品制单表.单据编号, 购进商品制单表.日期, 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 购进商品制单明细表.赠品 FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID INNER JOIN 商品表 ON 购进商品制单明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 购进商品制单表.单位ID = 单位表.ID LEFT OUTER JOIN 库房表 ON 购进商品制单明细表.库房ID = 库房表.ID WHERE (购进商品制单表.单位ID = 2) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '"+dateTimePickerE.Value.ToShortDateString()+" 00:00:00', 102)) AND (购进商品制单表.BeActive = 1) AND (购进商品制单明细表.赠品 = 1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");


            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["商品表1"];
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "购进赠品查询;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "购进赠品查询;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
        }

   }
}