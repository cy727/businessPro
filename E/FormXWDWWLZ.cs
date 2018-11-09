using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXWDWWLZ : Form
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


        private int iCompanyID = 0;

        private ClassGetInformation cGetInformation;

        public FormXWDWWLZ()
        {
            InitializeComponent();
        }

        private void FormXWDWWLZ_Load(object sender, EventArgs e)
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
                dateTimePickerS.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqlConn.Close();

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(2, "") == 0) //失败
            {
                return;
            }
            else
            {
                iCompanyID = cGetInformation.iCompanyNumber;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(20, textBoxDWBH.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    iCompanyID = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                }

            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(22, textBoxDWMC.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    iCompanyID = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                }

            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i, j;
            if (iCompanyID == 0)
            {
                MessageBox.Show("请选择要查询的单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 日期, 单据编号, 价税合计, 已付款金额, 未付款金额 FROM  收款汇总视图 WHERE (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND  (单位ID = " + iCompanyID.ToString() + ") ORDER BY 日期";



            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            decimal dt1 = 0, dt2 = 0, dt3 = 0;
            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                for (j = 3; j < dSet.Tables["商品表"].Columns.Count; j++)
                {
                    if (dSet.Tables["商品表"].Rows[i][j].ToString() == "")
                        dSet.Tables["商品表"].Rows[i][j] = 0;

                }
                dt1 += decimal.Parse(dSet.Tables["商品表"].Rows[i][2].ToString());
                dt2 += decimal.Parse(dSet.Tables["商品表"].Rows[i][3].ToString());
                dt3 += decimal.Parse(dSet.Tables["商品表"].Rows[i][4].ToString());
            }

            dataGridViewDJMX.Columns[0].Visible = false;
            for (i = 0; i < dataGridViewDJMX.ColumnCount; i++)
            {
                dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
            sqlConn.Close();

            object[] oTemp1 = new object[5];
            oTemp1[1] = "合计";
            oTemp1[0] = null;
            oTemp1[2] = dt1.ToString("f2"); oTemp1[3] = dt2.ToString("f2"); oTemp1[4] = dt3.ToString("f2");
            dSet.Tables["商品表"].Rows.Add(oTemp1);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "销往单位往来账;当前日期：" + labelZDRQ.Text + ";单位：" + textBoxDWMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "销往单位往来账;当前日期：" + labelZDRQ.Text + ";单位：" + textBoxDWMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}