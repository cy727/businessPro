using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKFZLCX : Form
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


        private int iKFID = 0;
        private ClassGetInformation cGetInformation;

        public FormKFZLCX()
        {
            InitializeComponent();
        }

        private void FormKFZLCX_Load(object sender, EventArgs e)
        {
            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
        }

        private void textBoxKFBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                iKFID = cGetInformation.iKFNumber;
                textBoxKFBH.Text = cGetInformation.strKFCode;
                textBoxKFMC.Text = cGetInformation.strKFName;
            }
        }

        private void textBoxKFBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cGetInformation.getKFInformation(20, textBoxKFBH.Text) == 0) //失败
            {
                iKFID = 0;
                textBoxKFBH.Text = "";
                textBoxKFMC.Text = "";
                return;
            }
            else
            {
                iKFID = cGetInformation.iKFNumber;
                textBoxKFBH.Text = cGetInformation.strKFCode;
                textBoxKFMC.Text = cGetInformation.strKFName;
            }
        }

        private void textBoxKFMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFMC.Text) == 0) //失败
                {
                    iKFID = 0;
                    textBoxKFBH.Text = "";
                    textBoxKFMC.Text = "";

                    return;
                }
                else
                {
                    iKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i, j;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 库房编号, 库房名称, 助记码, 简称 FROM 库房表 WHERE (BeActive = 1)";

            if (iKFID != 0)
                sqlComm.CommandText += " AND (ID = " + iKFID.ToString() + ")";


            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            sqlConn.Close();
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "库房资料查询;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "库房资料查询;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}