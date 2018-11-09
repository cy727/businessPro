using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormBQJYGK : Form
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

        private int iJZID = 0;
        private string SDT = "";

        private ClassGetInformation cGetInformation;

        public FormBQJYGK()
        {
            InitializeComponent();
        }

        private void FormBQJYGK_Load(object sender, EventArgs e)
        {
            decimal dTemp = 0, dTemp1 = 0,dtt=0;
            decimal dtt1=0, dtt2=0, dtt3=0;


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
                SDT = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
            }

            sqldr.Close();


            if (SDT == "") //没有结算
            {
                sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    SDT = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                }
                iJZID = 0;
                sqldr.Close();
            }


            dTemp = 0;
            //sqlComm.CommandText = "SELECT SUM(结转金额) AS Expr1 FROM 结转库房汇总表 WHERE (结转ID = " + iJZID.ToString() + ")";
            sqlComm.CommandText = "SELECT SUM(结转金额) AS Expr1 FROM 结转进销存汇总表 WHERE (结转ID = " + iJZID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString()!="")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            object[] objTemp = new object[2];
            objTemp[0] = "上期库存结转余额";
            objTemp[1] = dTemp;
            dataGridViewDJMX.Rows.Add(objTemp);

            dTemp = 0; dTemp1 = 0;
            sqlComm.CommandText = "SELECT SUM(应付余额) AS Expr1, SUM(应收余额) AS Expr2 FROM 结转往来汇总表 WHERE (结转ID = " + iJZID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                if (sqldr.GetValue(1).ToString() != "")
                    dTemp1 = Convert.ToDecimal(sqldr.GetValue(1).ToString());
            }
            sqldr.Close();
            objTemp[0] = "上期应付结转余额";
            objTemp[1] = dTemp;
            dataGridViewDJMX.Rows.Add(objTemp);

            objTemp[0] = "上期应收结转余额";
            objTemp[1] = dTemp1;
            dataGridViewDJMX.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT SUM(进货入库明细表.实计金额) AS 金额 FROM 进货入库明细表 INNER JOIN 进货入库汇总表 ON 进货入库明细表.单据ID = 进货入库汇总表.ID WHERE (进货入库汇总表.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (进货入库汇总表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "本期购进入库金额";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(进货退出明细表.实计金额) AS 金额  FROM 进货退出明细表 INNER JOIN 进货退出汇总表 ON 进货退出明细表.单据ID = 进货退出汇总表.ID WHERE (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "本期购进退出金额";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();
            sqlComm.CommandText = "SELECT SUM(销售商品制单明细表.实计金额),SUM(销售商品制单明细表.毛利) AS 金额 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND  (销售商品制单表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                if (sqldr.GetValue(1).ToString() != "")
                    dtt = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                objTemp[0] = "本期销售金额";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();
            sqlComm.CommandText = "SELECT SUM(销售退出明细表.实计金额) AS 金额  FROM 销售退出明细表 INNER JOIN 销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID WHERE (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "本期销售退出金额";
                objTemp[1] = dTemp;

                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();
            decimal dt1 = 0;
            objTemp[0] = "实际销售金额";
            dTemp=Convert.ToDecimal(dataGridViewDJMX.Rows[dataGridViewDJMX.Rows.Count - 2].Cells[1].Value.ToString()) - Convert.ToDecimal(dataGridViewDJMX.Rows[dataGridViewDJMX.Rows.Count - 1].Cells[1].Value.ToString());
            objTemp[1] =dTemp;
            dt1 = dTemp;

            sqlComm.CommandText = "SELECT SUM(商品表.库存数量*商品表.库存成本价) AS Expr1 FROM 商品表 WHERE (beactive = 1)";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "本期库存余额";
                objTemp[1] = dTemp;
                
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            //sqlComm.CommandText = "SELECT SUM(应付账款) AS Expr1, SUM(应收账款) AS Expr2 FROM 单位表 WHERE (BeActive = 1)";
            sqlComm.CommandText = "SELECT SUM(未付款金额) FROM 付款明细视图 WHERE (付款明细视图.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "本期应付余额";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(未付款金额) FROM 收款明细视图 WHERE (收款明细视图.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "本期应收余额";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();


            sqlComm.CommandText = "SELECT SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";
            dTemp = 0;
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) FROM 销售退出明细表 INNER JOIN 销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID WHERE (销售退出汇总表.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (销售退出汇总表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp -= Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();


            objTemp[0] = "本期出库成本";
            objTemp[1] = dTemp;
            decimal dt2 = dTemp;
            dataGridViewDJMX.Rows.Add(objTemp);


            sqlComm.CommandText = "SELECT SUM(库存报损明细表.报损金额) FROM 库存报损明细表 INNER JOIN 库存报损汇总表 ON 库存报损明细表.单据ID = 库存报损汇总表.ID WHERE (库存报损汇总表.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (库存报损汇总表.BeActive = 1)";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "本期报损金额";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();


            sqlComm.CommandText = "SELECT SUM(借物出库汇总表.出库金额) AS Expr1 FROM 借物出库汇总表 WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 > 0) AND ((借物出库汇总表.冲抵单号ID IS NULL) OR (借物出库汇总表.冲抵单号ID <> -1))";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "本期借物出库金额";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();



            sqlComm.CommandText = "SELECT SUM(借物出库汇总表.出库金额) AS Expr1 FROM 借物出库汇总表 WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 <= 0) AND ((借物出库汇总表.冲抵单号ID IS NULL) OR (借物出库汇总表.冲抵单号ID <> -1))";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "本期借物入库金额";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(借物出库汇总表.出库金额) AS Expr1 FROM 借物出库汇总表 WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (借物出库汇总表.冲抵单号ID = -1)";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "本期借物冲抵金额";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();
           
           

            objTemp[0] = "本期销售毛利";
            //objTemp[1] = dt1-dt2;
            objTemp[1] = dtt;
            dataGridViewDJMX.Rows.Add(objTemp);

            sqlComm.CommandText = "SELECT SUM(购进退补差价汇总表.价税合计) AS Expr1 FROM 购进退补差价汇总表 WHERE (购进退补差价汇总表.BeActive = 1) AND (购进退补差价汇总表.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) ";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dTemp = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "本期购进退补差价";
                objTemp[1] = dTemp;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(销售退补差价汇总表.价税合计) AS Expr1 FROM 销售退补差价汇总表 WHERE (销售退补差价汇总表.BeActive = 1) AND (销售退补差价汇总表.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102))";

            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dtt1 = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dtt1 = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "本期销售退补差价";
                objTemp[1] = dtt1;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(销售退出明细表.实计金额-销售退出明细表.数量 * 销售退出明细表.库存成本价) FROM 销售退出明细表 INNER JOIN 销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID WHERE (销售退出汇总表.日期 > CONVERT(DATETIME, '" + SDT + " 23:59:59', 102)) AND (销售退出汇总表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                dtt2 = 0;
                if (sqldr.GetValue(0).ToString() != "")
                    dtt2 = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                objTemp[0] = "本期销售退出毛利";
                objTemp[1] = dtt2;
                dataGridViewDJMX.Rows.Add(objTemp);
            }
            sqldr.Close();

            dtt3 = dtt + dtt1 - dtt2;
            objTemp[0] = "本期毛利";
            objTemp[1] = dtt3;
            dataGridViewDJMX.Rows.Add(objTemp);


            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].DefaultCellStyle.Format = "f2";
            sqlConn.Close();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
            if (SDT == "")
                labelJZRQ.Text = "";
            else
                labelJZRQ.Text = Convert.ToDateTime(SDT).ToString("yyyy年M月dd日");

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "本期经营概况;当前日期：" + labelZDRQ.Text ;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "本期经营概况;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}