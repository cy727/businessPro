using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormYWSJJZ : Form
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

        public bool isSaved = false;
        public int iJZID = 0;

        private string sSCJZSJ = "", sBCJZSJ = "";


        public FormYWSJJZ()
        {
            InitializeComponent();
        }

        private void FormYWSJJZ_Load(object sender, EventArgs e)
        {
            int i;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;


            //if (isSaved)
            //{
            //    initDJ();
            //    return;
            //}

            //得到上次结转时间
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 结算时间,ID FROM 结转汇总表 ORDER BY 结算时间 DESC";
            sqldr=sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString(); 
                iJZID = Convert.ToInt32(sqldr.GetValue(1).ToString());
            }
            sqldr.Close();

            if (sSCJZSJ == "") //没有结算
            {
                sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString(); 
                }
                iJZID = 0;
                sqldr.Close();
            }
            labelSCJZRQ.Text = Convert.ToDateTime(sSCJZSJ).ToString("yyyy年M月dd日");

            sqlConn.Close();

            //initHTDefault();
            cGetInformation.getSystemDateTime();
            //sBCJZSJ = cGetInformation.strSYSDATATIME;
            sBCJZSJ = Convert.ToDateTime(cGetInformation.strSYSDATATIME).AddDays(-1).ToShortDateString();
            labelBCJZRQ.Text = Convert.ToDateTime(sBCJZSJ).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            initdataGridViewJXCHZ();
            //initdataGridViewCBLRB();
            initdataGridViewKF();
            initdataGridViewKFHZ();
            dataGridViewKF_Click(null,null);
            initdataGridViewGJWLHZ();

        }

        private void initdataGridViewKF()
        {
            int i, j;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ID, 库房编号, 库房名称 FROM 库房表 WHERE (BeActive = 1)";

            if (dSet.Tables.Contains("库房表")) dSet.Tables.Remove("库房表");
            sqlDA.Fill(dSet, "库房表");
            dataGridViewKF.DataSource = dSet.Tables["库房表"];
            sqlConn.Close();


            dataGridViewKF.Columns[0].Visible = false;
            for (i = 1; i < dataGridViewKF.ColumnCount; i++)
            {
                dataGridViewKF.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            
        }
        private void initdataGridViewJXCHZ()
        {
            int i, j;
            try
            {
                string strA = "", strB = "", strC = "", strD = "", strE = "", strF = "", strG = "", strH = "", strI = "", strJ = "", strK = "", strL = "", strM = "";

                strA = "SELECT * FROM 结转进销存汇总表 WHERE (结转ID =  " + iJZID.ToString() + ")";

                strB = "SELECT 销售商品制单明细表.商品ID, SUM(销售商品制单明细表.数量) AS 销出数量, SUM(销售商品制单明细表.库存成本价 * 销售商品制单明细表.数量)  AS 销出成本, SUM(销售商品制单明细表.实计金额) AS 销出金额, SUM(销售商品制单明细表.毛利) AS 销出毛利 FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID WHERE (销售商品制单表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1) GROUP BY 销售商品制单明细表.商品ID";

                strC = "SELECT 商品ID, SUM(数量) AS 出库数量, SUM(实计金额) AS 出库金额 FROM 出库视图 WHERE (日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 商品ID";

                strD = "SELECT 商品ID, SUM(数量) AS 入库数量, SUM(金额) AS 入库金额 FROM 入库视图 WHERE (日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 商品ID";

                strE = "SELECT 进货入库明细表.商品ID, SUM(进货入库明细表.数量) AS 购进入库数量, SUM(进货入库明细表.金额) AS 购进入库金额 FROM 进货入库明细表 INNER JOIN 进货入库汇总表 ON 进货入库明细表.单据ID = 进货入库汇总表.ID WHERE (进货入库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102) AND (进货入库汇总表.BeActive = 1)) GROUP BY 进货入库明细表.商品ID";

                strF = "SELECT 进货退出明细表.商品ID, SUM(进货退出明细表.数量) AS 进货退出数量, SUM(进货退出明细表.实计金额) AS 进货退出金额 FROM 进货退出汇总表 INNER JOIN 进货退出明细表 ON 进货退出汇总表.ID = 进货退出明细表.单据ID WHERE (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 进货退出明细表.商品ID";

                strG = "SELECT 销售退出明细表.商品ID, SUM(销售退出明细表.数量) AS 销售退出数量, SUM(销售退出明细表.数量*销售退出明细表.库存成本价) AS 销售退出成本, SUM(销售退出明细表.实计金额) AS 销售退出金额 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID WHERE (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 销售退出明细表.商品ID";


                strH = "SELECT 借物出库明细表.商品ID, SUM(借物出库明细表.数量) AS 借物出库数量, SUM(借物出库明细表.出库金额) AS 借物出库金额 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (借物出库明细表.数量>0) AND ((借物出库汇总表.冲抵单号ID IS NULL) OR (借物出库汇总表.冲抵单号ID <> -1)) GROUP BY 借物出库明细表.商品ID";

                strJ = "SELECT 借物出库明细表.商品ID, SUM(借物出库明细表.数量) AS 借物入库数量, SUM(借物出库明细表.出库金额) AS 借物入库金额 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (借物出库明细表.数量<0) AND ((借物出库汇总表.冲抵单号ID IS NULL) OR (借物出库汇总表.冲抵单号ID <> -1)) GROUP BY 借物出库明细表.商品ID";

                /*

                strH = "SELECT 借物出库明细表.商品ID, SUM(借物出库明细表.数量) AS 借物出库数量, SUM(借物出库明细表.出库金额) AS 借物出库金额 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (借物出库明细表.数量>0) GROUP BY 借物出库明细表.商品ID";

                strJ = "SELECT 借物出库明细表.商品ID, SUM(借物出库明细表.数量) AS 借物入库数量, SUM(借物出库明细表.出库金额) AS 借物入库金额 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (借物出库明细表.数量<0) GROUP BY 借物出库明细表.商品ID";
                */

                strM = "SELECT 借物出库明细表.商品ID, SUM(借物出库明细表.数量) AS 借物冲抵数量, SUM(借物出库明细表.出库金额) AS 借物冲抵金额 FROM 借物出库明细表 INNER JOIN 借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (借物出库汇总表.冲抵单号ID = -1) GROUP BY 借物出库明细表.商品ID";

                strI = "SELECT  库存报损明细表.商品ID, SUM(库存报损明细表.报损数量) AS 库存报损数量, SUM(库存报损明细表.报损金额) AS 库存报损金额 FROM 库存报损明细表 INNER JOIN 库存报损汇总表 ON 库存报损明细表.单据ID = 库存报损汇总表.ID WHERE (库存报损汇总表.BeActive = 1) AND (库存报损汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (库存报损汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 库存报损明细表.商品ID";

                strL = "SELECT 销售退补差价明细表.商品ID, SUM(销售退补差价明细表.补价数量) AS 销售补价数量, SUM(销售退补差价明细表.金额) AS 销售补价金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID WHERE (销售退补差价汇总表.BeActive = 1) AND (销售退补差价汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 销售退补差价明细表.商品ID";

                strK = "SELECT 购进退补差价明细表.商品ID, SUM(购进退补差价明细表.补价数量) AS 购进补价数量, SUM(购进退补差价明细表.金额) AS 购进补价金额 FROM 购进退补差价汇总表 INNER JOIN 购进退补差价明细表 ON 购进退补差价汇总表.ID = 购进退补差价明细表.单据ID WHERE (购进退补差价汇总表.BeActive = 1) AND (购进退补差价汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 购进退补差价明细表.商品ID";


                sqlComm.CommandText = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 结转进销存汇总表1.结转数量 AS 上期结转数量, 结转进销存汇总表1.结转单价 AS 上期结转单价, 结转进销存汇总表1.结转金额 AS 上期结转金额, 购进入库表.购进入库数量, 购进入库表.购进入库金额,进货退出表.进货退出数量,进货退出表.进货退出金额,入库表.入库数量, 入库表.入库金额,销出表.销出数量, 销出表.销出成本, 销出表.销出金额,销售退出表.销售退出数量,销售退出表.销售退出成本,销售退出表.销售退出金额,销出表.销出毛利,出库表.出库数量, 出库表.出库金额, 借物出库表.借物出库数量,借物出库表.借物出库金额,借物入库表.借物入库数量,借物入库表.借物入库金额,借物冲抵表.借物冲抵数量,借物冲抵表.借物冲抵金额,库存报损表.库存报损数量,库存报损表.库存报损金额,商品表.库存数量 AS 本期结转数量, 商品表.库存成本价 AS 本期结转单价, 商品表.库存数量*商品表.库存成本价 AS 本期结转金额, 购进退补差价表.购进补价数量,购进退补差价表.购进补价金额,销售退补差价表.销售补价数量,销售退补差价表.销售补价金额,商品表.ID FROM 商品表 LEFT OUTER JOIN (" + strA + ") 结转进销存汇总表1 ON 商品表.ID = 结转进销存汇总表1.商品ID LEFT OUTER JOIN (" + strB + ") 销出表 ON 商品表.ID = 销出表.商品ID LEFT OUTER JOIN (" + strC + ") 出库表 ON 商品表.ID = 出库表.商品ID LEFT OUTER JOIN (" + strD + ") 入库表 ON 商品表.ID = 入库表.商品ID LEFT OUTER JOIN (" + strE + ") 购进入库表 ON 商品表.ID = 购进入库表.商品ID LEFT OUTER JOIN (" + strF + ") 进货退出表 ON 商品表.ID = 进货退出表.商品ID LEFT OUTER JOIN (" + strG + ") 销售退出表 ON 商品表.ID = 销售退出表.商品ID LEFT OUTER JOIN (" + strH + ") 借物出库表 ON 商品表.ID = 借物出库表.商品ID LEFT OUTER JOIN (" + strI + ") 库存报损表 ON 商品表.ID = 库存报损表.商品ID LEFT OUTER JOIN (" + strJ + ") 借物入库表 ON 商品表.ID = 借物入库表.商品ID LEFT OUTER JOIN (" + strK + ") 购进退补差价表 ON 商品表.ID = 购进退补差价表.商品ID LEFT OUTER JOIN (" + strL + ") 销售退补差价表 ON 商品表.ID = 销售退补差价表.商品ID LEFT OUTER JOIN (" + strM + ") 借物冲抵表 ON 商品表.ID = 借物冲抵表.商品ID WHERE (商品表.beactive = 1)";

                sqlConn.Open();
                
                if (dSet.Tables.Contains("结转进销存汇总表")) dSet.Tables.Remove("结转进销存汇总表");
                sqlDA.Fill(dSet, "结转进销存汇总表");

                //计算合计
                object[] rowVals = new object[37];
                decimal[] rowDTemp = new decimal[37];

                rowVals[0] = "";
                rowVals[2] = "";
                rowVals[1] = "合计";
                for (i = 0; i < dSet.Tables["结转进销存汇总表"].Columns.Count-1; i++)
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
                    for (j = 3; j < dSet.Tables["结转进销存汇总表"].Columns.Count-1; j++)
                    {
                        rowDTemp[j] += Convert.ToDecimal(dSet.Tables["结转进销存汇总表"].Rows[i][j]);

                    }

                for (i = 3; i < dSet.Tables["结转进销存汇总表"].Columns.Count-1; i++)
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

        private void initdataGridViewKFHZ()
        {
            int i, j;

           try
            {

                string strA = "", strB = "", strC = "", strD = "", strE = "";

                strA = "SELECT * FROM 结转库房汇总表 WHERE (结转ID = " + iJZID.ToString() + ")";

                strB = "SELECT 商品ID, SUM(数量) AS 出库数量, SUM(实计金额) AS 出库金额, 库房ID FROM 出库视图 WHERE (日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 商品ID, 库房ID";

                strC = "SELECT 进货入库明细表.商品ID, SUM(进货入库明细表.数量) AS 购进入库数量, SUM(进货入库明细表.金额) AS 购进入库金额, 进货入库明细表.库房ID FROM 进货入库明细表 INNER JOIN 进货入库汇总表 ON 进货入库明细表.单据ID = 进货入库汇总表.ID WHERE (进货入库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (进货入库汇总表.BeActive = 1) GROUP BY 进货入库明细表.商品ID, 进货入库明细表.库房ID";

                strD = "SELECT 销售商品制单明细表.商品ID, SUM(销售商品制单明细表.数量) AS 销出数量, SUM(销售商品制单明细表.库存成本价 * 销售商品制单明细表.数量) / SUM(销售商品制单明细表.数量) AS 销出成本, SUM(销售商品制单表.价税合计) AS 销出金额, SUM(销售商品制单明细表.毛利) AS 销出毛利, 销售商品制单明细表.库房ID FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID WHERE (销售商品制单表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1) GROUP BY 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID";

                strE = "SELECT 商品ID, SUM(数量) AS 入库数量, SUM(金额) AS 入库金额, 库房ID FROM 入库视图 WHERE (日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) GROUP BY 商品ID, 库房ID";

                sqlComm.CommandText = "SELECT 库存表.库房ID, 商品表.ID, 商品表.商品名称, 商品表.商品编号, 结转库房汇总表1.结转数量 AS 上期结转数量, 结转库房汇总表1.结转单价 AS 上期结转单价, 结转库房汇总表1.结转金额 AS 上期结转金额, 入库表.入库数量, 入库表.入库金额, 购进入库表.购进入库数量, 购进入库表.购进入库金额, 出库表.出库数量, 出库表.出库金额, 销出表.销出数量, 销出表.销出成本, 销出表.销出金额, 库存表.库存数量 AS 本期结转数量, 库存表.库存金额 AS 本期结转金额, 库存表.库存成本价 AS 本期结转单价, 销出表.销出毛利 FROM 商品表 INNER JOIN 库存表 ON 商品表.ID = 库存表.商品ID LEFT OUTER JOIN ("+strA+") 结转库房汇总表1 ON 库存表.库房ID = 结转库房汇总表1.库房ID AND 库存表.商品ID = 结转库房汇总表1.商品ID LEFT OUTER JOIN ("+strB+") 出库表 ON 库存表.商品ID = 出库表.商品ID AND 库存表.库房ID = 出库表.库房ID LEFT OUTER JOIN ("+strC+") 购进入库表 ON 库存表.库房ID = 购进入库表.库房ID AND 库存表.商品ID = 购进入库表.商品ID LEFT OUTER JOIN ("+strD+") 销出表 ON 库存表.商品ID = 销出表.商品ID AND 库存表.库房ID = 销出表.库房ID LEFT OUTER JOIN ("+strE+") 入库表 ON 库存表.库房ID = 入库表.库房ID AND 库存表.商品ID = 入库表.商品ID WHERE (商品表.beactive = 1)";


            sqlConn.Open();
            if (dSet.Tables.Contains("结转库存汇总表")) dSet.Tables.Remove("结转库存汇总表");
            sqlDA.Fill(dSet, "结转库存汇总表");

            for (i = 0; i < dSet.Tables["结转库存汇总表"].Rows.Count; i++)
                for (j = 3; j < dSet.Tables["结转库存汇总表"].Columns.Count; j++)
                {
                    if (dSet.Tables["结转库存汇总表"].Rows[i][j].ToString() == "")
                        dSet.Tables["结转库存汇总表"].Rows[i][j] = 0;
                }
            //sqlConn.Close();
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

        private void dataGridViewKF_Click(object sender, EventArgs e)
        {
            int i, j;

            int iSelectKF = 0;
            if (dataGridViewKF.SelectedRows.Count < 1)
            {
                iSelectKF = Convert.ToInt32(dataGridViewKF.Rows[0].Cells[0].Value.ToString());
            }
            else
            {
                iSelectKF = Convert.ToInt32(dataGridViewKF.SelectedRows[0].Cells[0].Value.ToString());
            }

            DataView dt = new DataView(dSet.Tables["结转库存汇总表"], "库房ID=" + iSelectKF.ToString(), "", DataViewRowState.CurrentRows);
            dataGridViewKFHZ.DataSource=dt;

            dataGridViewKFHZ.Columns[0].Visible = false;
            dataGridViewKFHZ.Columns[1].Visible = false;
            dataGridViewKFHZ.Columns[7].Visible = false;
            dataGridViewKFHZ.Columns[8].Visible = false;
            dataGridViewKFHZ.Columns[9].Visible = false;
            dataGridViewKFHZ.Columns[10].Visible = false;
            dataGridViewKFHZ.Columns[11].Visible = false;
            dataGridViewKFHZ.Columns[12].Visible = false;
            dataGridViewKFHZ.Columns[13].Visible = false;
            dataGridViewKFHZ.Columns[14].Visible = false;
            dataGridViewKFHZ.Columns[15].Visible = false;
            dataGridViewKFHZ.Columns[19].Visible = false;
            for (i = 1; i < dataGridViewKFHZ.ColumnCount; i++)
            {
                dataGridViewKFHZ.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

        }

        private void initdataGridViewGJWLHZ()
        {
            int i, j;

            try		

            {

                string strA = "", strB = "", strC = "", strD = "", strE = "";
                /*
                strA = "SELECT * FROM 结转往来汇总表 WHERE (结转ID = " + iJZID.ToString() + ")";

                strB = "SELECT SUM(实计金额) AS 收款金额, 单位ID FROM 结算收款汇总表 WHERE (日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (BeActive = 1) GROUP BY 单位ID";

                strC = "SELECT SUM(销售商品制单表.价税合计) AS 本期销出金额, 销售商品制单表.单位ID FROM 销售商品制单明细表 INNER JOIN 销售商品制单表 ON 销售商品制单明细表.表单ID = 销售商品制单表.ID WHERE (销售商品制单表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1) GROUP BY 销售商品制单表.单位ID";

                strD = "SELECT SUM(实计金额) AS 本期付款金额, 单位ID FROM 结算收款汇总表 WHERE (日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (BeActive = 1) GROUP BY 单位ID";

                strE = "SELECT SUM(进货入库明细表.数量) AS 购进入库数量, SUM(进货入库明细表.金额) AS 本期购进金额, 进货入库汇总表.单位ID FROM 进货入库明细表 INNER JOIN 进货入库汇总表 ON 进货入库明细表.单据ID = 进货入库汇总表.ID WHERE (进货入库汇总表.日期 > CONVERT(DATETIME, '" + sSCJZSJ + " 23:59:59', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + sBCJZSJ + " 23:59:59', 102)) AND (进货入库汇总表.BeActive = 1) GROUP BY 进货入库汇总表.单位ID";
               
                sqlComm.CommandText = "SELECT 单位表.ID, 单位表.是否进货, 单位表.是否销售, 单位表.单位编号, 单位表.单位名称,结转往来汇总表1.应付余额 AS 上期应付余额, 购进入库表.本期购进金额, 付款表.本期付款金额, 单位表.应付账款 AS 本期应付余额, 结转往来汇总表1.应收余额 AS 上期应收余额, 销出表.本期销出金额, 收款表.收款金额, 单位表.应收账款 AS 本期应收余额 FROM 单位表 LEFT OUTER JOIN ("+strA+") 结转往来汇总表1 ON  单位表.ID = 结转往来汇总表1.单位ID LEFT OUTER JOIN ("+strB+") 收款表 ON 单位表.ID = 收款表.单位ID LEFT OUTER JOIN ("+strC+") 销出表 ON 单位表.ID = 销出表.单位ID LEFT OUTER JOIN ("+strD+") 付款表 ON 单位表.ID = 付款表.单位ID LEFT OUTER JOIN ("+strE+") 购进入库表 ON 单位表.ID = 购进入库表.单位ID WHERE (单位表.BeActive = 1)";
                */

                strA = "SELECT * FROM 结转往来汇总表 WHERE (结转ID = " + iJZID.ToString() + ")";

                strB = "SELECT 单位ID, SUM(未付款金额) AS 应收余额 FROM 收款汇总视图 WHERE (BeActive = 1) GROUP BY 单位ID";

                strC = "SELECT 单位ID, SUM(未付款金额) AS 应付余额 FROM 付款汇总视图 WHERE (BeActive = 1) GROUP BY 单位ID";

                sqlComm.CommandText = "SELECT 单位表.ID, 单位表.单位编号, 单位表.单位名称,结转往来汇总表1.应付余额 AS 上期应付余额, 结转往来汇总表1.应收余额 AS 上期应收余额, 付款表.应付余额 AS 本期应付金额, 收款表.应收余额 AS 本期应收金额 FROM 单位表 LEFT OUTER JOIN (" + strA + ") 结转往来汇总表1 ON  单位表.ID = 结转往来汇总表1.单位ID LEFT OUTER JOIN (" + strB + ") 收款表 ON 单位表.ID = 收款表.单位ID LEFT OUTER JOIN (" + strC + ") 付款表 ON 单位表.ID = 付款表.单位ID WHERE (单位表.BeActive = 1)";


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
                dataGridViewXSWLHZ.Columns[4].DefaultCellStyle.Format = "f2";
                dataGridViewXSWLHZ.Columns[6].DefaultCellStyle.Format = "f2";

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

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i,j;

            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("本期转结已经完成，转结时间为:"+labelBCJZRQ.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("是否进行转结？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;


            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                //表单汇总
                sqlComm.CommandText = "INSERT INTO 结转汇总表 (结算时间, 操作员ID) VALUES ('"+sBCJZSJ+"', "+intUserID.ToString()+")";
                sqlComm.ExecuteNonQuery();

                //取得单据号 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //结转进销存汇总表
                for (i = 0; i < dSet.Tables["结转进销存汇总表"].Rows.Count; i++)
                {
                    if (dSet.Tables["结转进销存汇总表"].Rows[i][36].ToString() == "")
                        continue;
                    if (dSet.Tables["结转进销存汇总表"].Rows[i][36].ToString() == "0")
                        continue;

                    sqlComm.CommandText = "INSERT INTO 结转进销存汇总表 (结转ID, 商品ID, 结转数量, 结转单价, 结转金额) VALUES (" + sBillNo + ", " + dSet.Tables["结转进销存汇总表"].Rows[i][36].ToString() + ", " + dSet.Tables["结转进销存汇总表"].Rows[i][29].ToString() + ", " + dSet.Tables["结转进销存汇总表"].Rows[i][30].ToString() + ", " + dSet.Tables["结转进销存汇总表"].Rows[i][31].ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                //结转库房汇总表
                for (i = 0; i < dSet.Tables["结转库存汇总表"].Rows.Count ; i++)
                {
                    if (dSet.Tables["结转进销存汇总表"].Rows[i][1].ToString() == "")
                        continue;
                    if (dSet.Tables["结转进销存汇总表"].Rows[i][1].ToString() == "0")
                        continue;
                    if (dSet.Tables["结转进销存汇总表"].Rows[i][0].ToString() == "")
                        continue;
                    if (dSet.Tables["结转进销存汇总表"].Rows[i][0].ToString() == "0")
                        continue;

                    sqlComm.CommandText = "INSERT INTO 结转库房汇总表 (结转ID, 商品ID, 库房ID, 结转数量, 结转单价, 结转金额) VALUES (" + sBillNo + ", " + dSet.Tables["结转库存汇总表"].Rows[i][1].ToString() + ", " + dSet.Tables["结转库存汇总表"].Rows[i][0].ToString() + ", " + dSet.Tables["结转库存汇总表"].Rows[i][16].ToString() + ", " + dSet.Tables["结转库存汇总表"].Rows[i][18].ToString() + ", " + dSet.Tables["结转库存汇总表"].Rows[i][17].ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                //结转往来汇总表
                for (i = 0; i < dSet.Tables["结转往来汇总表"].Rows.Count ; i++)
                {
                    if (dSet.Tables["结转往来汇总表"].Rows[i][0].ToString() == "")
                        continue;
                    if (dSet.Tables["结转往来汇总表"].Rows[i][0].ToString() == "0")
                        continue;

                    sqlComm.CommandText = "INSERT INTO 结转往来汇总表 (结转ID, 单位ID, 应付余额, 应收余额) VALUES (" + sBillNo + ", " + dSet.Tables["结转往来汇总表"].Rows[i][0].ToString() + ", " + dSet.Tables["结转往来汇总表"].Rows[i][5].ToString() + ", " + dSet.Tables["结转往来汇总表"].Rows[i][6].ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                sqlComm.CommandText = "UPDATE 商品表 SET 库存金额 = 库存数量 * 库存成本价";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 库存表 SET 库存金额 = 库存数量 * 库存成本价";
                sqlComm.ExecuteNonQuery();



                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                return;
            }
            finally
            {
                sqlConn.Close();
            }



            MessageBox.Show("本期转结完成，转结时间为:" + labelBCJZRQ.Text, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isSaved = true;

        }

        private void FormYWSJJZ_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
            {
                return;
            }

            DialogResult dr = MessageBox.Show(this, "尚未进行业务数据结转，确定要退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";            
            switch (tabControlDJMX.SelectedIndex)
            {
                case 0:
                    strT = "业务数据结转（进销存汇总）;结转日期：" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewJXCHZ, strT, true, intUserLimit);
                    break;

                case 1:
                    strT = "业务数据结转（库房汇总）;结转日期：" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewKFHZ, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "业务数据结转（购进往来汇总）;结转日期：" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewGJWLHZ, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "业务数据结转（销售往来汇总）;结转日期：" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewXSWLHZ, strT, true, intUserLimit);
                    break;


            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";
            switch (tabControlDJMX.SelectedIndex)
            {
                case 0:
                    strT = "业务数据结转（进销存汇总）;结转日期：" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewJXCHZ, strT, false, intUserLimit);
                    break;

                case 1:
                    strT = "业务数据结转（库房汇总）;结转日期：" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewKFHZ, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "业务数据结转（购进往来汇总）;结转日期：" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewGJWLHZ, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "业务数据结转（销售往来汇总）;结转日期：" + labelBCJZRQ.Text + ";";
                    PrintDGV.Print_DataGridView(dataGridViewXSWLHZ, strT, false, intUserLimit);
                    break;


            }
        }

        private void dateTimePickerJZ_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerJZ.Value < DateTime.Parse(sSCJZSJ))
             return;

            sBCJZSJ = dateTimePickerJZ.Value.ToShortDateString();
            labelBCJZRQ.Text = Convert.ToDateTime(sBCJZSJ).ToString("yyyy年M月dd日");

            initdataGridViewJXCHZ();
            //initdataGridViewCBLRB();
            initdataGridViewKF();
            initdataGridViewKFHZ();
            dataGridViewKF_Click(null, null);
            initdataGridViewGJWLHZ();
            
        }

        private void buttonBR_Click(object sender, EventArgs e)
        {
            //得到上次结转时间
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 结算时间,ID FROM 结转汇总表 ORDER BY 结算时间 DESC";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                iJZID = Convert.ToInt32(sqldr.GetValue(1).ToString());
            }
            sqldr.Close();

            if (sSCJZSJ == "") //没有结算
            {
                sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                }
                iJZID = 0;
                sqldr.Close();
            }
            labelSCJZRQ.Text = Convert.ToDateTime(sSCJZSJ).ToString("yyyy年M月dd日");

            sqlConn.Close();

            //initHTDefault();
            cGetInformation.getSystemDateTime();
            //sBCJZSJ = cGetInformation.strSYSDATATIME;
            sBCJZSJ = Convert.ToDateTime(cGetInformation.strSYSDATATIME).ToShortDateString();
            labelBCJZRQ.Text = Convert.ToDateTime(sBCJZSJ).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            initdataGridViewJXCHZ();
            //initdataGridViewCBLRB();
            initdataGridViewKF();
            initdataGridViewKFHZ();
            dataGridViewKF_Click(null, null);
            initdataGridViewGJWLHZ();
        }

    }
}