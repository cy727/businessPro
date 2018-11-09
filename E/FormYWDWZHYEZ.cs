using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormYWDWZHYEZ : Form
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
        private bool isSaved = false;

        public FormYWDWZHYEZ()
        {
            InitializeComponent();
        }

        private void FormYWDWZHYEZ_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;


            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
            comboBoxDWXL.SelectedIndex = 0;
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal dTemp=0;

            if (dSet.Tables.Contains("余额表"))
                dSet.Tables.Remove("余额表");

            sqlConn.Open();
            if (!dSet.Tables.Contains("余额表")) //没有
            {
                sqlComm.CommandText = "SELECT 单位表.单位编号, 单位表.单位名称, 购进表.购进金额, 购进表.购进批次, 退货表.退出批次, 付款表.付款金额, 单位表.应付账款, 销出表.销出金额, 销出表.销出批次, 退回表.退回批次, 收款表.收入金额, 单位表.应收账款, 单位表.应收账款 AS 总应收账款, 单位表.应付账款 AS 总应付账款,单位表.是否进货, 单位表.是否销售 FROM 单位表 LEFT OUTER JOIN (SELECT SUM(收入金额) AS 收入金额, 单位ID FROM 单位历史账表 WHERE (BeActive = 1) AND (单据编号 LIKE N'%BYS%') GROUP BY 单位ID) 收款表 ON  单位表.ID = 收款表.单位ID LEFT OUTER JOIN (SELECT COUNT(*) AS 退回批次, 单位ID FROM 单位历史账表 WHERE (BeActive = 1) AND (单据编号 LIKE N'%BTH%') GROUP BY 单位ID) 退回表 ON 单位表.ID = 退回表.单位ID LEFT OUTER JOIN (SELECT SUM(销出金额) AS 销出金额, COUNT(*) AS 销出批次, 单位ID FROM 单位历史账表 WHERE (BeActive = 1) AND (单据编号 LIKE N'%BKP%') GROUP BY 单位ID) 销出表 ON 单位表.ID = 销出表.单位ID LEFT OUTER JOIN (SELECT SUM(付款金额) AS 付款金额, 单位ID    FROM 单位历史账表 WHERE (BeActive = 1) AND (单据编号 LIKE N'%AYF%') GROUP BY 单位ID) 付款表 ON  单位表.ID = 付款表.单位ID LEFT OUTER JOIN (SELECT COUNT(*) AS 退出批次, 单位ID FROM 单位历史账表 WHERE (BeActive = 1) AND (单据编号 LIKE N'%ATH%') GROUP BY 单位ID) 退货表 ON 单位表.ID = 退货表.单位ID LEFT OUTER JOIN (SELECT SUM(购进金额) AS 购进金额, COUNT(*) AS 购进批次, 单位ID FROM 单位历史账表 WHERE (BeActive = 1) AND (单据编号 LIKE N'%ADH%') GROUP BY 单位ID) 购进表 ON 单位表.ID = 购进表.单位ID WHERE (单位表.BeActive = 1)";

                if (!checkBoxALL.Checked && iCompanyID != 0)
                {
                    sqlComm.CommandText += " AND 单位表.ID="+iCompanyID.ToString();
                }

                sqlDA.Fill(dSet, "余额表");

                for (i = 0; i < dSet.Tables["余额表"].Rows.Count; i++)
                {
                    for (j = 3; j < dSet.Tables["余额表"].Columns.Count; j++)
                    {
                        if (dSet.Tables["余额表"].Rows[i][j].ToString() == "")
                            dSet.Tables["余额表"].Rows[i][j] = 0;

                    }

                    dTemp = Convert.ToDecimal(dSet.Tables["余额表"].Rows[i][6].ToString()) - Convert.ToDecimal(dSet.Tables["余额表"].Rows[i][11].ToString());
                    if (dTemp < 0)
                    {
                        dSet.Tables["余额表"].Rows[i][12] = Math.Abs(dTemp);
                        dSet.Tables["余额表"].Rows[i][13] = 0;
                    }
                    else
                    {
                        dSet.Tables["余额表"].Rows[i][12] = 0;
                        dSet.Tables["余额表"].Rows[i][13] = dTemp;
                    }

                }
            }

            DataView dv;
            switch(comboBoxDWXL.SelectedIndex)
            {
                default:                    
                    dv = new DataView(dSet.Tables["余额表"]);
                    break;
                case 1:
                    dv = new DataView(dSet.Tables["余额表"], "是否进货=1","",DataViewRowState.CurrentRows);
                    break;
                case 2:
                    dv = new DataView(dSet.Tables["余额表"], "是否销售=1", "", DataViewRowState.CurrentRows);
                    break;
                    

            }
            dataGridViewDJMX.DataSource = dv;
            if (!isSaved)
            {
                dataGridViewDJMX.Columns[14].Visible = false;
                dataGridViewDJMX.Columns[15].Visible = false;

                dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[12].DefaultCellStyle.Format = "f2";
                dataGridViewDJMX.Columns[13].DefaultCellStyle.Format = "f2";
                
                dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f0";
                dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f0";
                dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f0";
                dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f0";

                for (i = 0; i < dataGridViewDJMX.ColumnCount; i++)
                {
                    dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                isSaved = true;
            }
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
            sqlConn.Close();

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "业务单位综合余额账;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "业务单位综合余额账;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1000, "") == 0) //失败
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
                if (cGetInformation.getCompanyInformation(1100, textBoxDWBH.Text) == 0) //失败
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
                if (cGetInformation.getCompanyInformation(1200, textBoxDWMC.Text) == 0) //失败
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
    }
}