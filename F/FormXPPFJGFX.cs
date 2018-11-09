using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXPPFJGFX : Form
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

        private int intCommID = 0;
        private ClassGetInformation cGetInformation;

        public FormXPPFJGFX()
        {
            InitializeComponent();
        }

        private void FormXPPFJGFX_Load(object sender, EventArgs e)
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


            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
        }

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                intCommID = cGetInformation.iCommNumber;
                textBoxSPBH.Text = cGetInformation.strCommCode;
                textBoxSPMC.Text = cGetInformation.strCommName;

            }
        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0) //失败
                {
                    intCommID = 0;
                    textBoxSPBH.Text = "";
                    textBoxSPMC.Text = "";
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                }

            }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0) //失败
                {
                    intCommID = 0;
                    textBoxSPBH.Text = "";
                    textBoxSPMC.Text = "";

                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                }

            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            //保存完毕
            if (intCommID == 0)
            {
                MessageBox.Show("请选择要查询的商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 单位表.单位编号, 单位表.单位名称, 销售.数量, 销售.金额, 销售.毛利, 销售.交易批次, 销售.最高售价, 销售.最低售价, 销售.[毛利率%], 销售.平均成本 FROM 单位表 INNER JOIN (SELECT 销售商品制单表.单位ID, SUM(销售商品制单明细表.数量) AS 数量, SUM(销售商品制单明细表.实计金额) AS 金额, SUM(销售商品制单明细表.毛利) AS 毛利, COUNT(*) AS 交易批次, MAX(销售商品制单明细表.单价) AS 最高售价, MIN(销售商品制单明细表.单价) AS 最低售价, 0.00 AS [毛利率%], SUM(销售商品制单明细表.库存成本价 * 销售商品制单明细表.数量) AS 平均成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.BeActive = 1) AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) GROUP BY 销售商品制单表.单位ID) 销售 ON 单位表.ID = 销售.单位ID";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            sqlConn.Close();

            adjustDataView();
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
            


            toolStripStatusLabelMXJLS.Text = "记录数:" + dSet.Tables["商品表"].Rows.Count.ToString() + "  ";

        }

        private void adjustDataView()
        {
            int i;
            decimal iSL = 0, iJYPC = 0;
            decimal dJE = 0,dML=0,dMAX=0,dMIN=0,dMLV,dPJCB=0,dCB=0;

            if (dSet.Tables["商品表"].Rows.Count > 0)
            {
                if (dSet.Tables["商品表"].Rows[0][7].ToString() == "")
                    dMIN = 0;
                else
                    dMIN = decimal.Parse(dSet.Tables["商品表"].Rows[0][7].ToString());
            }


            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表"].Rows[i][2].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][2] = 0;
                if (dSet.Tables["商品表"].Rows[i][3].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][3] = 0;
                if (dSet.Tables["商品表"].Rows[i][4].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][4] = 0;
                if (dSet.Tables["商品表"].Rows[i][5].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][5] = 0;
                if (dSet.Tables["商品表"].Rows[i][6].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][6] = 0;
                if (dSet.Tables["商品表"].Rows[i][7].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][7] = 0;
                if (dSet.Tables["商品表"].Rows[i][8].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][8] = 0;
                if (dSet.Tables["商品表"].Rows[i][9].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][9] = 0;

                if (decimal.Parse(dSet.Tables["商品表"].Rows[i][3].ToString()) == 0)
                {
                    dSet.Tables["商品表"].Rows[i][8] = 0;
                }
                else
                {
                    dSet.Tables["商品表"].Rows[i][8] = decimal.Parse(dSet.Tables["商品表"].Rows[i][4].ToString()) / decimal.Parse(dSet.Tables["商品表"].Rows[i][3].ToString()) * 100;
                }

                dCB += decimal.Parse(dSet.Tables["商品表"].Rows[i][9].ToString());

                if (decimal.Parse(dSet.Tables["商品表"].Rows[i][2].ToString()) == 0)
                {
                    dSet.Tables["商品表"].Rows[i][9] = 0;
                }
                else
                {
                    dSet.Tables["商品表"].Rows[i][9] = decimal.Parse(dSet.Tables["商品表"].Rows[i][9].ToString()) / decimal.Parse(dSet.Tables["商品表"].Rows[i][2].ToString());
                }

                iSL += decimal.Parse(dSet.Tables["商品表"].Rows[i][2].ToString());
                dJE += decimal.Parse(dSet.Tables["商品表"].Rows[i][3].ToString());
                dML += decimal.Parse(dSet.Tables["商品表"].Rows[i][4].ToString());
                iJYPC += decimal.Parse(dSet.Tables["商品表"].Rows[i][5].ToString());
                dMAX = Math.Max(decimal.Parse(dSet.Tables["商品表"].Rows[i][6].ToString()), dMAX);
                dMIN = Math.Min(decimal.Parse(dSet.Tables["商品表"].Rows[i][7].ToString()), dMIN);
                
            }

            DataRow dr = dSet.Tables["商品表"].NewRow();
            dr[0] = "合计";
            dr[2] = iSL; dr[3] = dJE; dr[4] = dML; dr[5] = iJYPC; dr[6] = dMAX; dr[7] = dMIN;
            if (decimal.Parse(dr[3].ToString()) == 0)
            {
                dr[8] = 0;
            }
            else
            {
                dr[8] = decimal.Parse(dr[4].ToString()) / decimal.Parse(dr[3].ToString()) * 100;
            }

            if (decimal.Parse(dr[2].ToString()) == 0)
            {
                dr[9] = 0;
            }
            else
            {
                dr[9] = dCB / decimal.Parse(dr[2].ToString());
            }
            dSet.Tables["商品表"].Rows.Add(dr);

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "销售品批发价格分析;当前日期：" + labelZDRQ.Text+";商品："+textBoxSPMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "销售品批发价格分析;当前日期：" + labelZDRQ.Text + ";商品：" + textBoxSPMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}