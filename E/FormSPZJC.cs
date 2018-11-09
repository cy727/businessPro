using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPZJC : Form
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

        private int intClassID = 0;
        private int iJZID = 0;

        private ClassGetInformation cGetInformation;
        public FormSPZJC()
        {
            InitializeComponent();
        }

        private void FormSPZJC_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            this.Top = 1;
            this.Left = 1;
            
            //得到上次结转
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 结算时间,ID FROM 结转汇总表 ORDER BY 结算时间 DESC";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                iJZID = Convert.ToInt32(sqldr.GetValue(1).ToString());
            }
            sqldr.Close();



            sqlComm.CommandText = "SELECT 商品表.分类编号, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.库存数量, 商品表.库存成本价, 商品表.库存数量*商品表.库存成本价 AS 库存金额, 结转进销存汇总表1.结转数量, 结转进销存汇总表1.结转单价, 结转进销存汇总表1.结转数量*结转进销存汇总表1.结转单价 AS 结转金额 FROM 商品表 LEFT OUTER JOIN (SELECT * FROM 结转进销存汇总表 WHERE (结转ID = " + iJZID.ToString() + ")) 结转进销存汇总表1 ON  商品表.ID = 结转进销存汇总表1.商品ID WHERE (商品表.beactive = 1) ";


            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            for (int i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
                for (int j = 4; j < dSet.Tables["商品表"].Columns.Count; j++)
                {
                    if (dSet.Tables["商品表"].Rows[i][j].ToString() == "")
                        dSet.Tables["商品表"].Rows[i][j] = 0;
                }
            sqlConn.Close();

            toolStripButtonGD_Click(null, null);

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
        }

        private void checkBoxALL_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxALL.Checked)
            {
                intClassID = 0;
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i,j;
            decimal[] dSUm = new decimal[6];
            DataView dt;
            if (intClassID == 0)
            {
                dt = new DataView(dSet.Tables["商品表"]);
            }
            else
            {
                dt = new DataView(dSet.Tables["商品表"], "分类编号=" + intClassID.ToString(),"",DataViewRowState.CurrentRows);
            }
            dataGridViewDJMX.DataSource = dt;

            dataGridViewDJMX.Columns[0].Visible = false;
            for (i = 1; i < dataGridViewDJMX.ColumnCount; i++)
            {
                dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            
            sqlConn.Close();

            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[5].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";

            for (i = 0; i < 6; i++)
                dSUm[i] = 0;

            for (i = 0; i < dataGridViewDJMX.RowCount; i++)
            {
                for (j = 0; j < dSUm.Length; j++)
                {
                    dSUm[j] += decimal.Parse(dataGridViewDJMX.Rows[i].Cells[4+j].Value.ToString());
                }
            }

            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString() + " 库存数量:" + dSUm[0].ToString("f0") + " 库存金额:" + dSUm[2].ToString("f2") + " 结存数量:" + dSUm[3].ToString("f0") + " 结存金额:" + dSUm[5].ToString("f2");

        }

        private void textBoxSPLB_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getClassInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                intClassID = cGetInformation.iClassNumber;
                textBoxSPLB.Text = cGetInformation.strClassName;
                checkBoxALL.Checked = false;

            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品总结存查询;当前日期：" + labelZDRQ.Text + ";";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品总结存查询;当前日期：" + labelZDRQ.Text + ";";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void textBoxSPLB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getClassInformation(10, textBoxSPLB.Text) == 0) //失败
                {
                    textBoxSPLB.Text = "";
                    intClassID = 0;
                    checkBoxALL.Checked = true;

                }
                else
                {
                    intClassID = cGetInformation.iClassNumber;
                    textBoxSPLB.Text = cGetInformation.strClassName;
                    checkBoxALL.Checked = false;
                }
            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Add)
            {
                toolStripButtonGD_Click(null, null);
            }
        }
    }
}