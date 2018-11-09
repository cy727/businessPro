using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJWCKCX : Form
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

        public int iSupplyCompany = 0;
        public int intCommID = 0;

        private decimal[] cTemp = new decimal[3] { 0, 0, 0 };
        private decimal[] cTemp1 = new decimal[3] { 0, 0, 0 };

        
        public FormJWCKCX()
        {
            InitializeComponent();
        }

        private void FormJWCKCX_Load(object sender, EventArgs e)
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

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1000, "") == 0)
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
                if (cGetInformation.getCompanyInformation(1200, textBoxDWBH.Text.Trim()) == 0)
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
                if (cGetInformation.getCompanyInformation(1200, textBoxDWMC.Text.Trim()) == 0)
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

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0)
            {
                intCommID = 0;
                textBoxSPMC.Text = "";
                textBoxSPBH.Text = "";
                //return;
            }
            else
            {
                intCommID = cGetInformation.iCommNumber;
                textBoxSPMC.Text = cGetInformation.strCommName;
                textBoxSPBH.Text = cGetInformation.strCommCode;
            }
        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0)
                {
                    intCommID = 0;
                    textBoxSPMC.Text = "";
                    textBoxSPBH.Text = "";
                    //return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                }
            }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0)
                {
                    intCommID = 0;
                    textBoxSPMC.Text = "";
                    textBoxSPBH.Text = "";
                    //return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 商品表.商品编号, 商品表.商品名称, 借物出库明细表.数量, 借物出库明细表.库存成本价, 借物出库明细表.出库金额, 借物出库汇总表.备注, 借物出库汇总表.物流名称, 借物出库汇总表.单号 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID INNER JOIN 商品表 ON 借物出库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 借物出库明细表.库房ID = 库房表.ID INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID WHERE (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.BeActive = 1) AND (借物出库明细表.数量>0) AND (借物出库汇总表.冲抵单号ID IS NULL)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (intCommID != 0)
                sqlComm.CommandText += "AND  (商品表.ID = " + intCommID.ToString() + ")";



            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");
            dataGridView1.DataSource = dSet.Tables["商品表1"];
            dataGridView1.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView1.Columns[10].DefaultCellStyle.Format = "f2";

            sqlComm.CommandText = "SELECT 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 商品表.商品编号, 商品表.商品名称, 借物出库明细表.数量, 借物出库明细表.库存成本价, 借物出库明细表.出库金额, 借物出库汇总表.备注, 借物出库汇总表.物流名称, 借物出库汇总表.单号 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID INNER JOIN 商品表 ON 借物出库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 借物出库明细表.库房ID = 库房表.ID INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID WHERE (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.BeActive = 1) AND (借物出库明细表.数量<0) AND (借物出库汇总表.冲抵单号ID IS NULL)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (intCommID != 0)
                sqlComm.CommandText += "AND  (商品表.ID = " + intCommID.ToString() + ")";



            if (dSet.Tables.Contains("商品表2")) dSet.Tables.Remove("商品表2");
            sqlDA.Fill(dSet, "商品表2");



            dataGridView2.DataSource = dSet.Tables["商品表2"];
            dataGridView2.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView2.Columns[10].DefaultCellStyle.Format = "f2";

            sqlComm.CommandText = "SELECT 借物出库汇总表.单据编号, 借物出库汇总表.日期, 单位表.单位编号, 单位表.单位名称, 库房表.库房编号, 库房表.库房名称, 商品表.商品编号, 商品表.商品名称, 借物出库明细表.数量, 借物出库明细表.库存成本价, 借物出库明细表.出库金额, 借物出库汇总表.备注, 借物出库汇总表.物流名称, 借物出库汇总表.单号 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID INNER JOIN 商品表 ON 借物出库明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 借物出库明细表.库房ID = 库房表.ID INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID WHERE (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.冲抵单号ID = -1)";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += "AND  (单位表.ID = " + iSupplyCompany.ToString() + ")";

            if (intCommID != 0)
                sqlComm.CommandText += "AND  (商品表.ID = " + intCommID.ToString() + ")";



            if (dSet.Tables.Contains("商品表3")) dSet.Tables.Remove("商品表3");
            sqlDA.Fill(dSet, "商品表3");

            dataGridView3.DataSource = dSet.Tables["商品表3"];
            dataGridView3.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridView3.Columns[10].DefaultCellStyle.Format = "f2";
            sqlConn.Close();

            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);

            //toolStripStatusLabelMXJLS.Text = dSet.Tables["商品表3"].Rows.Count.ToString();
        }

        private void countfTemp()
        {
            int c = 0, c1 = 0;
            int i, j;

            for (i = 1; i <= 3; i++)
            {
                cTemp[i - 1] = 0;
                cTemp1[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 8;
                        c1 = 10;
                        break;
                    case 2:
                        c = 8;
                        c1 = 10;
                        break;
                    case 3:
                        c = 8;
                        c1 = 10;
                        break;

                    default:
                        c = 0;
                        c1 = 0;
                        break;
                }

                if (c != 0)
                {

                    for (j = 0; j < dSet.Tables["商品表" + i.ToString()].Rows.Count; j++)
                    {
                        try
                        {
                            cTemp[i - 1] += decimal.Parse(dSet.Tables["商品表" + i.ToString()].Rows[j][c].ToString());
                            cTemp1[i - 1] += decimal.Parse(dSet.Tables["商品表" + i.ToString()].Rows[j][c1].ToString());
                        }
                        catch
                        {
                        }
                    }
                }


            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "借物出库查询;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "借物出库查询;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;

            if (!dSet.Tables.Contains("商品表" + c1.ToString()))
                return;


            toolStripStatusLabelMXJLS.Text = "共有" + dSet.Tables["商品表" + c1.ToString()].Rows.Count.ToString() + "条记录 数量合计" + cTemp[tabControl1.SelectedIndex].ToString("f0") + " 出库金额合计" + cTemp1[tabControl1.SelectedIndex].ToString("f2") + "元";
        }


    }
}