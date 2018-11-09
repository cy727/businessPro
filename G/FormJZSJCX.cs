using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJZSJCX : Form
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

        private int intKFID = 0;
        private int iJZID = 0;

        private string sSCJZSJ = "", sBCJZSJ = "";
 
        public FormJZSJCX()
        {
            InitializeComponent();
        }

        private void FormJZSJCX_Load(object sender, EventArgs e)
        {
            this.Left = 1;
            this.Top = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            //得到上次结转
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 结算时间,ID FROM 结转汇总表 ORDER BY 结算时间 DESC";
            if (dSet.Tables.Contains("结转汇总表")) dSet.Tables.Remove("结转汇总表");
            sqlDA.Fill(dSet, "结转汇总表");
            comboBoxJZSJ.DataSource = dSet.Tables["结转汇总表"];
            comboBoxJZSJ.DisplayMember = "结算时间";
            comboBoxJZSJ.ValueMember = "ID";
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            labelZDRQ.Text = Convert.ToDateTime(cGetInformation.strSYSDATATIME).ToString("yyyy年M月dd日");


        }

        private void textBoxKFBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(1, "") == 0)
            {
                //return;
            }
            else
            {
                intKFID = cGetInformation.iKFNumber;
                //textBoxKFMC.Text = cGetInformation.strKFName;
                textBoxKFBH.Text = cGetInformation.strKFCode;
            }
        }

        private void textBoxKFBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFBH.Text) == 0)
                {
                    intKFID = 0;
                    textBoxKFBH.Text = "";
                    //return;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    //extBoxKFMC.Text = cGetInformation.strKFName;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i;

            if (comboBoxJZSJ.Items.Count<1)
                return;

            if (comboBoxJZSJ.SelectedValue.ToString()=="")
                return;


            iJZID = int.Parse(comboBoxJZSJ.SelectedValue.ToString());

            //得到时间区间
            sqlConn.Open();
            if (iJZID == 1) //第一个转结
            {
                sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                    break;
                }
                sqldr.Close();
            }
            else
            {
                i = iJZID - 1;
                sqlComm.CommandText = "SELECT 结算时间,ID FROM 结转汇总表 WHERE ID = " + i.ToString();
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                    break;
                }
                sqldr.Close();
            }
            sBCJZSJ=comboBoxJZSJ.Text;
            sqlConn.Close();

            initdataGridViewJXCHZ();
            //initdataGridViewCBLRB();
            initdataGridViewGJWLHZ();

            /*
            
            sqlComm.CommandText = "SELECT 结转汇总表.结算时间, 商品表.商品编号, 商品表.商品名称, 结转进销存汇总表.结转数量, 结转进销存汇总表.结转单价, 结转进销存汇总表.结转金额, 结转进销存汇总表.入库数量, 结转进销存汇总表.入库金额, 结转进销存汇总表.购进入库数量, 结转进销存汇总表.购进入库金额, 结转进销存汇总表.出库数量, 结转进销存汇总表.出库金额, 结转进销存汇总表.销售数量, 结转进销存汇总表.销售金额 FROM 结转进销存汇总表 INNER JOIN 结转汇总表 ON 结转进销存汇总表.结转ID = 结转汇总表.ID INNER JOIN 商品表 ON 结转进销存汇总表.商品ID = 商品表.ID WHERE (结转汇总表.ID = "+iJZID.ToString()+")";

            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");


            sqlComm.CommandText = "SELECT 结转汇总表.结算时间, 商品表.商品编号, 商品表.商品名称, 结转进销存汇总表.出库数量, 结转进销存汇总表.出库金额, 结转进销存汇总表.销售数量, 结转进销存汇总表.销售金额, 结转进销存汇总表.出库毛利, 结转进销存汇总表.销出毛利率 FROM 结转进销存汇总表 INNER JOIN 结转汇总表 ON 结转进销存汇总表.结转ID = 结转汇总表.ID INNER JOIN 商品表 ON 结转进销存汇总表.商品ID = 商品表.ID WHERE (结转汇总表.ID = " + iJZID.ToString() + ")";

            if (dSet.Tables.Contains("商品表2")) dSet.Tables.Remove("商品表2");
            sqlDA.Fill(dSet, "商品表2");


            sqlComm.CommandText = "SELECT 结转汇总表.结算时间, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 结转库房汇总表.结转数量, 结转库房汇总表.结转单价, 结转库房汇总表.结转金额, 结转库房汇总表.入库数量, 结转库房汇总表.入库金额, 结转库房汇总表.购进入库数量, 结转库房汇总表.购进入库金额, 结转库房汇总表.出库数量, 结转库房汇总表.出库金额, 结转库房汇总表.销售数量, 结转库房汇总表.销售金额, 结转库房汇总表.出库毛利, 结转库房汇总表.销出毛利率 FROM 结转库房汇总表 INNER JOIN 结转汇总表 ON 结转库房汇总表.结转ID = 结转汇总表.ID INNER JOIN 商品表 ON 结转库房汇总表.商品ID = 商品表.ID INNER JOIN 库房表 ON 结转库房汇总表.库房ID = 库房表.ID WHERE (结转汇总表.ID = " + iJZID.ToString() + ")";

            if (intKFID!= 0)
                sqlComm.CommandText += " AND (库房表.ID = "+intKFID.ToString()+")";

            if (dSet.Tables.Contains("商品表3")) dSet.Tables.Remove("商品表3");
            sqlDA.Fill(dSet, "商品表3");

            sqlComm.CommandText = "SELECT 结转汇总表.结算时间, 商品表.商品编号, 商品表.商品名称, 库房表.库房编号, 库房表.库房名称, 结转库房汇总表.出库数量, 结转库房汇总表.出库金额, 结转库房汇总表.销售数量, 结转库房汇总表.销售金额, 结转库房汇总表.出库毛利, 结转库房汇总表.销出毛利率 FROM 结转库房汇总表 INNER JOIN 结转汇总表 ON 结转库房汇总表.结转ID = 结转汇总表.ID INNER JOIN 商品表 ON 结转库房汇总表.商品ID = 商品表.ID INNER JOIN 库房表 ON 结转库房汇总表.库房ID = 库房表.ID WHERE (结转汇总表.ID = " + iJZID.ToString() + ") ";

            if (intKFID != 0)
                sqlComm.CommandText += " AND (库房表.ID = " + intKFID.ToString() + ")";

            if (dSet.Tables.Contains("商品表4")) dSet.Tables.Remove("商品表4");
            sqlDA.Fill(dSet, "商品表4");


            sqlConn.Close();
            dataGridView1.DataSource = dSet.Tables["商品表1"];
            dataGridView2.DataSource = dSet.Tables["商品表2"];
            dataGridView3.DataSource = dSet.Tables["商品表3"];
            dataGridView4.DataSource = dSet.Tables["商品表4"];

            dataGridView1.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[6].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[10].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[11].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridView2.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[8].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[10].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[12].DefaultCellStyle.Format = "f0";
            dataGridView3.Columns[13].DefaultCellStyle.Format = "f0";
            dataGridView4.Columns[5].DefaultCellStyle.Format = "f0";
            dataGridView4.Columns[7].DefaultCellStyle.Format = "f0";
             */


        }

        /*
        private void initdataGridViewJXCHZ()
        {
            int i, j;
            try
            {
                string strA = "", strB = "", strC = "", strD = "", strE = "";

                strA = "SELECT * FROM 结转进销存汇总表 WHERE (结转ID =  " + iJZID.ToString() + ")";

                strB = "SELECT 销售商品制单明细表.商品ID, SUM(销售商品制单明细表.数量) AS 销出数量, SUM(销售商品制单明细表.库存成本价 * 销售商品制单明细表.数量)  AS 销出成本, SUM(销售商品制单明细表.实计金额) AS 销出金额, SUM(销售商品制单明细表.毛利) AS 销出毛利 FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID WHERE (销售商品制单表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1) GROUP BY 销售商品制单明细表.商品ID";

                strC = "SELECT 商品ID, SUM(数量) AS 出库数量, SUM(实计金额) AS 出库金额 FROM 出库视图 WHERE (日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 商品ID";

                strD = "SELECT 商品ID, SUM(数量) AS 入库数量, SUM(金额) AS 入库金额 FROM 入库视图 WHERE (日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 商品ID";

                strE = "SELECT 进货入库明细表.商品ID, SUM(进货入库明细表.数量) AS 购进入库数量, SUM(进货入库明细表.金额) AS 购进入库金额 FROM 进货入库明细表 INNER JOIN 进货入库汇总表 ON 进货入库明细表.单据ID = 进货入库汇总表.ID WHERE (进货入库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102) AND (进货入库汇总表.BeActive = 1)) GROUP BY 进货入库明细表.商品ID";

                sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品名称, 商品表.商品编号, 结转进销存汇总表1.结转数量 , 结转进销存汇总表1.结转单价 , 结转进销存汇总表1.结转金额, 入库表.入库数量, 入库表.入库金额, 购进入库表.购进入库数量, 购进入库表.购进入库金额, 出库表.出库数量, 出库表.出库金额, 销出表.销出数量, 销出表.销出成本, 销出表.销出金额, 销出表.销出毛利 FROM 商品表 LEFT OUTER JOIN (" + strA + ") 结转进销存汇总表1 ON 商品表.ID = 结转进销存汇总表1.商品ID LEFT OUTER JOIN (" + strB + ") 销出表 ON 商品表.ID = 销出表.商品ID LEFT OUTER JOIN (" + strC + ") 出库表 ON 商品表.ID = 出库表.商品ID LEFT OUTER JOIN (" + strD + ") 入库表 ON 商品表.ID = 入库表.商品ID LEFT OUTER JOIN (" + strE + ") 购进入库表 ON 商品表.ID = 购进入库表.商品ID WHERE (商品表.beactive = 1)";

                sqlConn.Open();

                if (dSet.Tables.Contains("结转进销存汇总表")) dSet.Tables.Remove("结转进销存汇总表");
                sqlDA.Fill(dSet, "结转进销存汇总表");

                //计算合计
                object[] rowVals = new object[16];
                decimal[] rowDTemp = new decimal[16];

                rowVals[0] = 0;
                rowVals[2] = "";
                rowVals[1] = "合计";
                for (i = 0; i < dSet.Tables["结转进销存汇总表"].Columns.Count; i++)
                    rowDTemp[i] = 0;

                for (i = 0; i < dSet.Tables["结转进销存汇总表"].Rows.Count; i++)
                    for (j = 3; j < dSet.Tables["结转进销存汇总表"].Columns.Count; j++)
                    {
                        if (dSet.Tables["结转进销存汇总表"].Rows[i][j].ToString() == "")
                            dSet.Tables["结转进销存汇总表"].Rows[i][j] = 0;
                        rowDTemp[j] += Convert.ToDecimal(dSet.Tables["结转进销存汇总表"].Rows[i][j]);

                    }

                for (i = 3; i < dSet.Tables["结转进销存汇总表"].Columns.Count; i++)
                    rowVals[i] = rowDTemp[i];


                dSet.Tables["结转进销存汇总表"].Rows.Add(rowVals);

                dataGridViewJXCHZ.DataSource = dSet.Tables["结转进销存汇总表"];


                dataGridViewJXCHZ.Columns[0].Visible = false;


                for (i = 1; i < dataGridViewJXCHZ.ColumnCount; i++)
                {
                    dataGridViewJXCHZ.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlConn.Close();
            }
            dataGridViewJXCHZ.Columns[3].DefaultCellStyle.Format = "f0";
        }
        */
        /*
        private void initdataGridViewCBLRB()
        {
            DataView dvtemp = new DataView(dSet.Tables["结转进销存汇总表"]);
            dataGridViewCBLRB.DataSource = dvtemp;

            dataGridViewCBLRB.Columns[0].Visible = false;
            dataGridViewCBLRB.Columns[3].Visible = false;
            dataGridViewCBLRB.Columns[4].Visible = false;
            dataGridViewCBLRB.Columns[5].Visible = false;
            dataGridViewCBLRB.Columns[6].Visible = false;
            dataGridViewCBLRB.Columns[7].Visible = false;
            dataGridViewCBLRB.Columns[8].Visible = false;
            dataGridViewCBLRB.Columns[9].Visible = false;
            dataGridViewCBLRB.Columns[12].Visible = false;
            dataGridViewCBLRB.Columns[13].Visible = false;
            dataGridViewCBLRB.Columns[14].Visible = false;


            for (int i = 1; i < dataGridViewCBLRB.ColumnCount; i++)
            {
                dataGridViewCBLRB.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

        }*/

        /*
        private void initdataGridViewJXCHZ()
        {
            int i, j;
            try
            {
                string strA = "", strB = "", strC = "", strD = "", strE = "", strF = "", strG = "", strH = "", strI = "", strJ = "";

                strA = "SELECT * FROM 结转进销存汇总表 WHERE (结转ID =  " + iJZID.ToString() + ")";

                strB = "SELECT 销售商品制单明细表.商品ID, SUM(销售商品制单明细表.数量) AS 销出数量, SUM(销售商品制单明细表.库存成本价 * 销售商品制单明细表.数量)  AS 销出成本, SUM(销售商品制单明细表.实计金额) AS 销出金额, SUM(销售商品制单明细表.毛利) AS 销出毛利 FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID WHERE (销售商品制单表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1) GROUP BY 销售商品制单明细表.商品ID";

                strC = "SELECT 商品ID, SUM(数量) AS 出库数量, SUM(实计金额) AS 出库金额 FROM 出库视图 WHERE (日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 商品ID";

                strD = "SELECT 商品ID, SUM(数量) AS 入库数量, SUM(金额) AS 入库金额 FROM 入库视图 WHERE (日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 商品ID";

                strE = "SELECT 进货入库明细表.商品ID, SUM(进货入库明细表.数量) AS 购进入库数量, SUM(进货入库明细表.金额) AS 购进入库金额 FROM 进货入库明细表 INNER JOIN 进货入库汇总表 ON 进货入库明细表.单据ID = 进货入库汇总表.ID WHERE (进货入库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102) AND (进货入库汇总表.BeActive = 1)) GROUP BY 进货入库明细表.商品ID";

                strF = "SELECT 进货退出明细表.商品ID, SUM(进货退出明细表.数量) AS 进货退出数量, SUM(进货退出明细表.实计金额) AS 进货退出金额 FROM 进货退出汇总表 INNER JOIN 进货退出明细表 ON 进货退出汇总表.ID = 进货退出明细表.单据ID WHERE (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 进货退出明细表.商品ID";

                strG = "SELECT 销售退出明细表.商品ID, SUM(销售退出明细表.数量) AS 销售退出数量, SUM(销售退出明细表.数量*销售退出明细表.库存成本价) AS 销售退出成本, SUM(销售退出明细表.实计金额) AS 销售退出金额 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID WHERE (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 销售退出明细表.商品ID";

                strH = "SELECT 借物出库明细表.商品ID, SUM(借物出库明细表.数量) AS 借物出库数量, SUM(借物出库明细表.出库金额) AS 借物出库金额 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 借物出库明细表.商品ID";

                strI = "SELECT  库存报损明细表.商品ID, SUM(库存报损明细表.报损数量) AS 库存报损数量, SUM(库存报损明细表.报损金额) AS 库存报损金额 FROM 库存报损明细表 INNER JOIN 库存报损汇总表 ON 库存报损明细表.单据ID = 库存报损汇总表.ID WHERE (库存报损汇总表.BeActive = 1) AND (库存报损汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (库存报损汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 库存报损明细表.商品ID";

                sqlComm.CommandText = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 结转进销存汇总表1.结转数量 AS 结转数量, 结转进销存汇总表1.结转单价 AS 结转单价, 结转进销存汇总表1.结转金额 AS 结转金额, 购进入库表.购进入库数量, 购进入库表.购进入库金额,进货退出表.进货退出数量,进货退出表.进货退出金额,入库表.入库数量, 入库表.入库金额,销出表.销出数量, 销出表.销出成本, 销出表.销出金额,销售退出表.销售退出数量,销售退出表.销售退出成本,销售退出表.销售退出金额,销出表.销出毛利,出库表.出库数量, 出库表.出库金额, 借物出库表.借物出库数量,借物出库表.借物出库金额,库存报损表.库存报损数量,库存报损表.库存报损金额,商品表.库存数量 AS 本期结转数量, 商品表.库存成本价 AS 本期结转单价, 商品表.库存数量*商品表.库存成本价 AS 本期结转金额, 商品表.ID FROM 商品表 LEFT OUTER JOIN (" + strA + ") 结转进销存汇总表1 ON 商品表.ID = 结转进销存汇总表1.商品ID LEFT OUTER JOIN (" + strB + ") 销出表 ON 商品表.ID = 销出表.商品ID LEFT OUTER JOIN (" + strC + ") 出库表 ON 商品表.ID = 出库表.商品ID LEFT OUTER JOIN (" + strD + ") 入库表 ON 商品表.ID = 入库表.商品ID LEFT OUTER JOIN (" + strE + ") 购进入库表 ON 商品表.ID = 购进入库表.商品ID LEFT OUTER JOIN (" + strF + ") 进货退出表 ON 商品表.ID = 进货退出表.商品ID LEFT OUTER JOIN (" + strG + ") 销售退出表 ON 商品表.ID = 销售退出表.商品ID LEFT OUTER JOIN (" + strH + ") 借物出库表 ON 商品表.ID = 借物出库表.商品ID LEFT OUTER JOIN (" + strI + ") 库存报损表 ON 商品表.ID = 库存报损表.商品ID WHERE (商品表.beactive = 1)";

                sqlConn.Open();

                if (dSet.Tables.Contains("结转进销存汇总表")) dSet.Tables.Remove("结转进销存汇总表");
                sqlDA.Fill(dSet, "结转进销存汇总表");

                //计算合计
                object[] rowVals = new object[28];
                decimal[] rowDTemp = new decimal[28];

                rowVals[0] = "";
                rowVals[2] = "";
                rowVals[1] = "合计";
                for (i = 0; i < dSet.Tables["结转进销存汇总表"].Columns.Count - 1; i++)
                    rowDTemp[i] = 0;

                for (i = 0; i < dSet.Tables["结转进销存汇总表"].Rows.Count; i++)
                    for (j = 3; j < dSet.Tables["结转进销存汇总表"].Columns.Count - 1; j++)
                    {
                        if (dSet.Tables["结转进销存汇总表"].Rows[i][j].ToString() == "")
                            dSet.Tables["结转进销存汇总表"].Rows[i][j] = 0;
                    }

                //毛利
                for (i = 0; i < dSet.Tables["结转进销存汇总表"].Rows.Count; i++)
                    dSet.Tables["结转进销存汇总表"].Rows[i][18] = Convert.ToDecimal(dSet.Tables["结转进销存汇总表"].Rows[i][14]) - Convert.ToDecimal(dSet.Tables["结转进销存汇总表"].Rows[i][17]) - (Convert.ToDecimal(dSet.Tables["结转进销存汇总表"].Rows[i][13]) - Convert.ToDecimal(dSet.Tables["结转进销存汇总表"].Rows[i][16]));

                for (i = 0; i < dSet.Tables["结转进销存汇总表"].Rows.Count; i++)
                    for (j = 3; j < dSet.Tables["结转进销存汇总表"].Columns.Count - 1; j++)
                    {
                        rowDTemp[j] += Convert.ToDecimal(dSet.Tables["结转进销存汇总表"].Rows[i][j]);

                    }

                for (i = 3; i < dSet.Tables["结转进销存汇总表"].Columns.Count - 1; i++)
                    rowVals[i] = rowDTemp[i];


                dSet.Tables["结转进销存汇总表"].Rows.Add(rowVals);

                dataGridViewJXCHZ.DataSource = dSet.Tables["结转进销存汇总表"];




                dataGridViewJXCHZ.Columns[28].Visible = false;
                dataGridViewJXCHZ.Columns[25].Visible = false;
                dataGridViewJXCHZ.Columns[26].Visible = false;
                dataGridViewJXCHZ.Columns[27].Visible = false;

                dataGridViewJXCHZ.Columns[3].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[6].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[8].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[10].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[12].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[15].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[19].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[21].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[23].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[25].DefaultCellStyle.Format = "f0";

                dataGridViewJXCHZ.Columns[4].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[5].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[7].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[9].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[11].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[13].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[14].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[16].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[17].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[18].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[20].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[22].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[24].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[26].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[27].DefaultCellStyle.Format = "f2";


                for (i = 1; i < dataGridViewJXCHZ.ColumnCount; i++)
                {
                    dataGridViewJXCHZ.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlConn.Close();
            }
        }
         * */
        private void initdataGridViewJXCHZ()
        {
            int i, j;
            try
            {
                string strAA = "", strA = "", strB = "", strC = "", strD = "", strE = "", strF = "", strG = "", strH = "", strI = "", strJ = "", strK = "", strL = "", strM = "";

                strA = "SELECT * FROM 结转进销存汇总表 WHERE (结转ID =  " + iJZID.ToString() + ")";

                strAA = "SELECT * FROM 结转进销存汇总表 WHERE (结转ID =  " + (iJZID-1).ToString() + ")";

                strB = "SELECT 销售商品制单明细表.商品ID, SUM(销售商品制单明细表.数量) AS 销出数量, SUM(销售商品制单明细表.库存成本价 * 销售商品制单明细表.数量)  AS 销出成本, SUM(销售商品制单明细表.实计金额) AS 销出金额, SUM(销售商品制单明细表.毛利) AS 销出毛利 FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID WHERE (销售商品制单表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1) GROUP BY 销售商品制单明细表.商品ID";

                strC = "SELECT 商品ID, SUM(数量) AS 出库数量, SUM(实计金额) AS 出库金额 FROM 出库视图 WHERE (日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 商品ID";

                strD = "SELECT 商品ID, SUM(数量) AS 入库数量, SUM(金额) AS 入库金额 FROM 入库视图 WHERE (日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 商品ID";

                strE = "SELECT 进货入库明细表.商品ID, SUM(进货入库明细表.数量) AS 购进入库数量, SUM(进货入库明细表.金额) AS 购进入库金额 FROM 进货入库明细表 INNER JOIN 进货入库汇总表 ON 进货入库明细表.单据ID = 进货入库汇总表.ID WHERE (进货入库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102) AND (进货入库汇总表.BeActive = 1)) GROUP BY 进货入库明细表.商品ID";

                strF = "SELECT 进货退出明细表.商品ID, SUM(进货退出明细表.数量) AS 进货退出数量, SUM(进货退出明细表.实计金额) AS 进货退出金额 FROM 进货退出汇总表 INNER JOIN 进货退出明细表 ON 进货退出汇总表.ID = 进货退出明细表.单据ID WHERE (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 进货退出明细表.商品ID";

                strG = "SELECT 销售退出明细表.商品ID, SUM(销售退出明细表.数量) AS 销售退出数量, SUM(销售退出明细表.数量*销售退出明细表.库存成本价) AS 销售退出成本, SUM(销售退出明细表.实计金额) AS 销售退出金额 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID WHERE (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 销售退出明细表.商品ID";

                strH = "SELECT 借物出库明细表.商品ID, SUM(借物出库明细表.数量) AS 借物出库数量, SUM(借物出库明细表.出库金额) AS 借物出库金额 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (借物出库明细表.数量>0) AND ((借物出库汇总表.冲抵单号ID IS NULL) OR (借物出库汇总表.冲抵单号ID <> -1)) GROUP BY 借物出库明细表.商品ID";

                strJ = "SELECT 借物出库明细表.商品ID, SUM(借物出库明细表.数量) AS 借物入库数量, SUM(借物出库明细表.出库金额) AS 借物入库金额 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (借物出库明细表.数量<0) AND ((借物出库汇总表.冲抵单号ID IS NULL) OR (借物出库汇总表.冲抵单号ID <> -1)) GROUP BY 借物出库明细表.商品ID";

                strI = "SELECT  库存报损明细表.商品ID, SUM(库存报损明细表.报损数量) AS 库存报损数量, SUM(库存报损明细表.报损金额) AS 库存报损金额 FROM 库存报损明细表 INNER JOIN 库存报损汇总表 ON 库存报损明细表.单据ID = 库存报损汇总表.ID WHERE (库存报损汇总表.BeActive = 1) AND (库存报损汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (库存报损汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 库存报损明细表.商品ID";

                strL = "SELECT 销售退补差价明细表.商品ID, SUM(销售退补差价明细表.补价数量) AS 销售补价数量, SUM(销售退补差价明细表.金额) AS 销售补价金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID WHERE (销售退补差价汇总表.BeActive = 1) AND (销售退补差价汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 销售退补差价明细表.商品ID";

                strM = "SELECT 借物出库明细表.商品ID, SUM(借物出库明细表.数量) AS 借物冲抵数量, SUM(借物出库明细表.出库金额) AS 借物冲抵金额 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (借物出库汇总表.冲抵单号ID = -1) GROUP BY 借物出库明细表.商品ID";

                strK = "SELECT 购进退补差价明细表.商品ID, SUM(购进退补差价明细表.补价数量) AS 购进补价数量, SUM(购进退补差价明细表.金额) AS 购进补价金额 FROM 购进退补差价汇总表 INNER JOIN 购进退补差价明细表 ON 购进退补差价汇总表.ID = 购进退补差价明细表.单据ID WHERE (购进退补差价汇总表.BeActive = 1) AND (购进退补差价汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 购进退补差价明细表.商品ID";


                sqlComm.CommandText = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 结转进销存汇总表1.结转数量 AS 上期结转数量, 结转进销存汇总表1.结转单价 AS 上期结转单价, 结转进销存汇总表1.结转金额 AS 上期结转金额, 购进入库表.购进入库数量, 购进入库表.购进入库金额,进货退出表.进货退出数量,进货退出表.进货退出金额,入库表.入库数量, 入库表.入库金额,销出表.销出数量, 销出表.销出成本, 销出表.销出金额,销售退出表.销售退出数量,销售退出表.销售退出成本,销售退出表.销售退出金额,销出表.销出毛利,出库表.出库数量, 出库表.出库金额, 借物出库表.借物出库数量,借物出库表.借物出库金额,借物入库表.借物入库数量,借物入库表.借物入库金额,借物冲抵表.借物冲抵数量,借物冲抵表.借物冲抵金额,库存报损表.库存报损数量,库存报损表.库存报损金额,结转进销存汇总表2.结转数量 AS 本期结转数量, 结转进销存汇总表2.结转单价 AS 本期结转单价, 结转进销存汇总表2.结转金额 AS 本期结转金额, 购进退补差价表.购进补价数量,购进退补差价表.购进补价金额,销售退补差价表.销售补价数量,销售退补差价表.销售补价金额,商品表.ID FROM 商品表 LEFT OUTER JOIN (" + strAA + ") 结转进销存汇总表1 ON 商品表.ID = 结转进销存汇总表1.商品ID LEFT OUTER JOIN (" + strB + ") 销出表 ON 商品表.ID = 销出表.商品ID LEFT OUTER JOIN (" + strC + ") 出库表 ON 商品表.ID = 出库表.商品ID LEFT OUTER JOIN (" + strD + ") 入库表 ON 商品表.ID = 入库表.商品ID LEFT OUTER JOIN (" + strE + ") 购进入库表 ON 商品表.ID = 购进入库表.商品ID LEFT OUTER JOIN (" + strF + ") 进货退出表 ON 商品表.ID = 进货退出表.商品ID LEFT OUTER JOIN (" + strG + ") 销售退出表 ON 商品表.ID = 销售退出表.商品ID LEFT OUTER JOIN (" + strH + ") 借物出库表 ON 商品表.ID = 借物出库表.商品ID LEFT OUTER JOIN (" + strI + ") 库存报损表 ON 商品表.ID = 库存报损表.商品ID LEFT OUTER JOIN (" + strJ + ") 借物入库表 ON 商品表.ID = 借物入库表.商品ID LEFT OUTER JOIN (" + strK + ") 购进退补差价表 ON 商品表.ID = 购进退补差价表.商品ID LEFT OUTER JOIN (" + strL + ") 销售退补差价表 ON 商品表.ID = 销售退补差价表.商品ID  LEFT OUTER JOIN (" + strA + ") 结转进销存汇总表2 ON 商品表.ID = 结转进销存汇总表2.商品ID LEFT OUTER JOIN (" + strM + ") 借物冲抵表 ON 商品表.ID = 借物冲抵表.商品ID WHERE (商品表.beactive = 1)";

                sqlConn.Open();

                if (dSet.Tables.Contains("结转进销存汇总表")) dSet.Tables.Remove("结转进销存汇总表");
                sqlDA.Fill(dSet, "结转进销存汇总表");

                //计算合计
                object[] rowVals = new object[37];
                decimal[] rowDTemp = new decimal[37];

                rowVals[0] = "";
                rowVals[2] = "";
                rowVals[1] = "合计";
                for (i = 0; i < dSet.Tables["结转进销存汇总表"].Columns.Count - 1; i++)
                    rowDTemp[i] = 0;

                for (i = 0; i < dSet.Tables["结转进销存汇总表"].Rows.Count; i++)
                    for (j = 3; j < dSet.Tables["结转进销存汇总表"].Columns.Count - 1; j++)
                    {
                        if (dSet.Tables["结转进销存汇总表"].Rows[i][j].ToString() == "")
                            dSet.Tables["结转进销存汇总表"].Rows[i][j] = 0;
                    }

                //毛利
                for (i = 0; i < dSet.Tables["结转进销存汇总表"].Rows.Count; i++)
                    dSet.Tables["结转进销存汇总表"].Rows[i][18] = Convert.ToDecimal(dSet.Tables["结转进销存汇总表"].Rows[i][14]) - Convert.ToDecimal(dSet.Tables["结转进销存汇总表"].Rows[i][17]) - (Convert.ToDecimal(dSet.Tables["结转进销存汇总表"].Rows[i][13]) - Convert.ToDecimal(dSet.Tables["结转进销存汇总表"].Rows[i][16])) + Convert.ToDecimal(dSet.Tables["结转进销存汇总表"].Rows[i][35]);

                for (i = 0; i < dSet.Tables["结转进销存汇总表"].Rows.Count; i++)
                    for (j = 3; j < dSet.Tables["结转进销存汇总表"].Columns.Count - 1; j++)
                    {
                        rowDTemp[j] += Convert.ToDecimal(dSet.Tables["结转进销存汇总表"].Rows[i][j]);

                    }

                for (i = 3; i < dSet.Tables["结转进销存汇总表"].Columns.Count - 1; i++)
                    rowVals[i] = rowDTemp[i];


                dSet.Tables["结转进销存汇总表"].Rows.Add(rowVals);

                dataGridViewJXCHZ.DataSource = dSet.Tables["结转进销存汇总表"];




                dataGridViewJXCHZ.Columns[36].Visible = false;
                dataGridViewJXCHZ.Columns[3].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[6].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[8].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[10].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[12].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[15].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[19].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[21].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[23].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[25].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[27].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[29].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[32].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[34].DefaultCellStyle.Format = "f0";

                dataGridViewJXCHZ.Columns[4].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[5].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[7].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[9].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[11].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[13].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[14].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[16].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[17].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[18].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[20].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[22].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[24].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[26].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[28].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[29].DefaultCellStyle.Format = "f0";
                dataGridViewJXCHZ.Columns[30].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[31].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[33].DefaultCellStyle.Format = "f2";
                dataGridViewJXCHZ.Columns[35].DefaultCellStyle.Format = "f2";


                for (i = 1; i < dataGridViewJXCHZ.ColumnCount; i++)
                {
                    dataGridViewJXCHZ.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlConn.Close();
            }
        }


        private void initdataGridViewGJWLHZ()
        {
            int i, j;
            string strA = "", strAA = "", strC = "", strD = "", strE = "";

            try
            {

                strA = "SELECT * FROM 结转往来汇总表 WHERE (结转ID = " + iJZID.ToString() + ")";

                strAA = "SELECT * FROM 结转往来汇总表 WHERE (结转ID = " + (iJZID-1).ToString() + ")";



                sqlComm.CommandText = "SELECT 单位表.ID, 单位表.单位编号, 单位表.单位名称,结转往来汇总表2.应付余额 AS 上期应付余额, 结转往来汇总表2.应收余额 AS 上期应收余额, 结转往来汇总表1.应付余额 AS 本期应付金额, 结转往来汇总表1.应收余额 AS 本期应收金额 FROM 单位表 LEFT OUTER JOIN (" + strA + ") 结转往来汇总表1 ON  单位表.ID = 结转往来汇总表1.单位ID  LEFT OUTER JOIN (" + strAA + ") 结转往来汇总表2 ON  单位表.ID = 结转往来汇总表2.单位ID WHERE (单位表.BeActive = 1)";


                sqlConn.Open();
                if (dSet.Tables.Contains("结转往来汇总表")) dSet.Tables.Remove("结转往来汇总表");
                sqlDA.Fill(dSet, "结转往来汇总表");

                //decimal dt1 = 0, dt2 = 0, dt3 = 0, dt4 = 0;

                //计算合计
                object[] rowVals = new object[7];
                decimal[] rowDTemp = new decimal[7];

                rowVals[0] = 0;
                rowVals[2] = "合计";
                for (i = 0; i < dSet.Tables["结转往来汇总表"].Columns.Count; i++)
                    rowDTemp[i] = 0;

                for (i = 0; i < dSet.Tables["结转往来汇总表"].Rows.Count; i++)
                    for (j = 3; j < dSet.Tables["结转往来汇总表"].Columns.Count; j++)
                    {
                        if (dSet.Tables["结转往来汇总表"].Rows[i][j].ToString() == "")
                            dSet.Tables["结转往来汇总表"].Rows[i][j] = 0;

                        rowDTemp[j] += decimal.Parse(dSet.Tables["结转往来汇总表"].Rows[i][j].ToString());

                    }
                for (i = 3; i < dSet.Tables["结转往来汇总表"].Columns.Count; i++)
                    rowVals[i] = rowDTemp[i];


                dSet.Tables["结转往来汇总表"].Rows.Add(rowVals);
                //sqlConn.Close();

                //DataView dt = new DataView(dSet.Tables["结转往来汇总表"], "是否进货=1", "", DataViewRowState.CurrentRows);
                DataView dt = new DataView(dSet.Tables["结转往来汇总表"]);
                dataGridViewGJWLHZ.DataSource = dt;


                dataGridViewGJWLHZ.Columns[0].Visible = false;
                dataGridViewGJWLHZ.Columns[4].Visible = false;
                dataGridViewGJWLHZ.Columns[6].Visible = false;
                dataGridViewGJWLHZ.Columns[3].DefaultCellStyle.Format = "f2";
                dataGridViewGJWLHZ.Columns[5].DefaultCellStyle.Format = "f2";

                for (i = 1; i < dataGridViewGJWLHZ.ColumnCount; i++)
                {
                    dataGridViewGJWLHZ.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                //DataView dt1 = new DataView(dSet.Tables["结转往来汇总表"], "是否销售=1", "", DataViewRowState.CurrentRows);
                DataView dt1 = new DataView(dSet.Tables["结转往来汇总表"]);
                dataGridViewXSWLHZ.DataSource = dt1;

                dataGridViewXSWLHZ.Columns[0].Visible = false;
                dataGridViewXSWLHZ.Columns[3].Visible = false;
                dataGridViewXSWLHZ.Columns[5].Visible = false;
                dataGridViewGJWLHZ.Columns[4].DefaultCellStyle.Format = "f2";
                dataGridViewGJWLHZ.Columns[6].DefaultCellStyle.Format = "f2";

                for (i = 1; i < dataGridViewXSWLHZ.ColumnCount; i++)
                {
                    dataGridViewXSWLHZ.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlConn.Close();
            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "结转数据查询（进销存汇总表）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewJXCHZ, strT, true, intUserLimit);
                    break;

                case 1:
                    strT = "结转数据查询（商品进销存明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJWLHZ, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "结转数据查询（商品进销存汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXSWLHZ, strT, true, intUserLimit);
                    break;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "结转数据查询（进销存汇总表）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewJXCHZ, strT, false, intUserLimit);
                    break;

                case 1:
                    strT = "结转数据查询（商品进销存明细）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewGJWLHZ, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "结转数据查询（商品进销存汇总）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewXSWLHZ, strT, false, intUserLimit);
                    break;
            }
        }


    }
}