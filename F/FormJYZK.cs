using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJYZK : Form
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
        private string SDT0 = "", SDT1 = "", SDT2 = "", SDT3 = "", SDT4 = "", SDT5 = "",SDT6 = "", SDT7 = "";

        private ClassGetInformation cGetInformation;
        
        public FormJYZK()
        {
            InitializeComponent();
        }

        private void FormJYZK_Load(object sender, EventArgs e)
        {
            decimal dTemp;
            int i;
            decimal[] dt1 = { 0, 0, 0, 0 };
            decimal[] dt2 = { 0, 0, 0, 0 };

            decimal[] dt3 = { 0, 0, 0, 0 };
            decimal[] dt4 = { 0, 0, 0, 0 };

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

            //得到时间
            SDT0 = Convert.ToDateTime(strDT).Year + "-" + Convert.ToDateTime(strDT).Month+ "-01";
            SDT1 = Convert.ToDateTime(strDT).Year + "-" + Convert.ToDateTime(strDT).Month + "-" + DateTime.DaysInMonth(Convert.ToDateTime(strDT).Year, Convert.ToDateTime(strDT).Month).ToString();
            SDT2 = Convert.ToDateTime(strDT).Year + "-01-01";
            SDT3 = Convert.ToDateTime(strDT).Year + "-12-31";
            SDT4 = Convert.ToDateTime(strDT).AddYears(-1).Year + "-" + Convert.ToDateTime(strDT).Month + "-01";
            SDT5 = Convert.ToDateTime(strDT).AddYears(-1).Year + "-" + Convert.ToDateTime(strDT).Month + "-" + DateTime.DaysInMonth(Convert.ToDateTime(strDT).Year, Convert.ToDateTime(strDT).Month).ToString(); 
            SDT6 = Convert.ToDateTime(strDT).AddYears(-1).Year + "-01-01";
            SDT7 = Convert.ToDateTime(strDT).AddYears(-1).Year + "-12-31";


            sqlConn.Open();

            object[] objTemp = new object[5];
            objTemp[0] = "购进金额";

            sqlComm.CommandText = "SELECT SUM(购进商品制单明细表.实计金额) AS 金额 FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '" + SDT0 + " 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + SDT1 + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[1] = dTemp;

            sqlComm.CommandText = "SELECT SUM(购进商品制单明细表.实计金额) AS 金额 FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '" + SDT2 + " 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + SDT3 + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[2] = dTemp;

            sqlComm.CommandText = "SELECT SUM(购进商品制单明细表.实计金额) AS 金额 FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '" + SDT4 + " 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + SDT5 + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[3] = dTemp;

            sqlComm.CommandText = "SELECT SUM(购进商品制单明细表.实计金额) AS 金额 FROM 购进商品制单表 INNER JOIN 购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID WHERE (购进商品制单表.BeActive = 1) AND (购进商品制单表.日期 >= CONVERT(DATETIME, '" + SDT6 + " 00:00:00', 102)) AND (购进商品制单表.日期 <= CONVERT(DATETIME, '" + SDT7 + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[4] = dTemp;

            dataGridViewDJMX.Rows.Add(objTemp);


            objTemp[0] = "购进退出金额";

            sqlComm.CommandText = "SELECT SUM(进货退出明细表.实计金额) AS 金额  FROM 进货退出明细表 INNER JOIN 进货退出汇总表 ON 进货退出明细表.单据ID = 进货退出汇总表.ID WHERE (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.日期 >= CONVERT(DATETIME, '" + SDT0 + " 00:00:00', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + SDT1 + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[1] = dTemp;

            sqlComm.CommandText = "SELECT SUM(进货退出明细表.实计金额) AS 金额  FROM 进货退出明细表 INNER JOIN 进货退出汇总表 ON 进货退出明细表.单据ID = 进货退出汇总表.ID WHERE (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.日期 >= CONVERT(DATETIME, '" + SDT2 + " 00:00:00', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + SDT3 + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[2] = dTemp;

            sqlComm.CommandText = "SELECT SUM(进货退出明细表.实计金额) AS 金额  FROM 进货退出明细表 INNER JOIN 进货退出汇总表 ON 进货退出明细表.单据ID = 进货退出汇总表.ID WHERE (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.日期 >= CONVERT(DATETIME, '" + SDT4 + " 00:00:00', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + SDT5 + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[3] = dTemp;

            sqlComm.CommandText = "SELECT SUM(进货退出明细表.实计金额) AS 金额  FROM 进货退出明细表 INNER JOIN 进货退出汇总表 ON 进货退出明细表.单据ID = 进货退出汇总表.ID WHERE (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.日期 >= CONVERT(DATETIME, '" + SDT6 + " 00:00:00', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + SDT7 + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[4] = dTemp;

            dataGridViewDJMX.Rows.Add(objTemp);

            objTemp[0] = "销售金额";
            sqlComm.CommandText = "SELECT SUM(销售商品制单明细表.实计金额) AS 金额 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDT0 + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + SDT1 + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[1] = dTemp;

            sqlComm.CommandText = "SELECT SUM(销售商品制单明细表.实计金额) AS 金额 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDT2 + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + SDT3 + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[2] = dTemp;

            sqlComm.CommandText = "SELECT SUM(销售商品制单明细表.实计金额) AS 金额 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDT4 + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + SDT5 + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[3] = dTemp;

            sqlComm.CommandText = "SELECT SUM(销售商品制单明细表.实计金额) AS 金额 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDT6 + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + SDT7 + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[4] = dTemp;

            dataGridViewDJMX.Rows.Add(objTemp);


            objTemp[0] = "销售退出金额";
            sqlComm.CommandText = "SELECT SUM(销售退出明细表.实计金额) AS 金额  FROM 销售退出明细表 INNER JOIN 销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID WHERE (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDT0 + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + SDT1 + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[1] = dTemp;

            sqlComm.CommandText = "SELECT SUM(销售退出明细表.实计金额) AS 金额  FROM 销售退出明细表 INNER JOIN 销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID WHERE (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDT2 + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + SDT3 + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[2] = dTemp;

            sqlComm.CommandText = "SELECT SUM(销售退出明细表.实计金额) AS 金额  FROM 销售退出明细表 INNER JOIN 销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID WHERE (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDT4 + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + SDT5 + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[3] = dTemp;

            sqlComm.CommandText = "SELECT SUM(销售退出明细表.实计金额) AS 金额  FROM 销售退出明细表 INNER JOIN 销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID WHERE (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDT6 + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + SDT7 + " 23:59:59', 102))";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[4] = dTemp;

            dataGridViewDJMX.Rows.Add(objTemp);

            objTemp[0] = "实际销售金额";
            for (i = 0; i < 4; i++)
            {
                dTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[dataGridViewDJMX.Rows.Count - 2].Cells[i+1].Value.ToString()) - Convert.ToDecimal(dataGridViewDJMX.Rows[dataGridViewDJMX.Rows.Count - 1].Cells[i+1].Value.ToString());
                objTemp[i+1] = dTemp;
                dt1[i] = dTemp;
            }
            dataGridViewDJMX.Rows.Add(objTemp);


            objTemp[0] = "出库成本";
            sqlComm.CommandText = "SELECT SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDT0 + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + SDT1 + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";
            dTemp = 0;
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) FROM 销售退出明细表 INNER JOIN 销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDT0 + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + SDT1 + " 23:59:59', 102)) AND (销售退出汇总表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp -= Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[1] = dTemp;
            dt2[0] = dTemp;

            sqlComm.CommandText = "SELECT SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDT2 + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + SDT3 + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";
            dTemp = 0;
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) FROM 销售退出明细表 INNER JOIN 销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDT2 + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + SDT3 + " 23:59:59', 102)) AND (销售退出汇总表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp -= Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[2] = dTemp;
            dt2[1] = dTemp;


            sqlComm.CommandText = "SELECT SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDT4 + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + SDT5 + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";
            dTemp = 0;
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) FROM 销售退出明细表 INNER JOIN 销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDT4 + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + SDT5 + " 23:59:59', 102)) AND (销售退出汇总表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp -= Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[3] = dTemp;
            dt2[2] = dTemp;


            sqlComm.CommandText = "SELECT SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDT6 + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + SDT6 + " 23:59:59', 102)) AND (销售商品制单表.BeActive = 1)";
            dTemp = 0;
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) FROM 销售退出明细表 INNER JOIN 销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDT6 + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + SDT7 + " 23:59:59', 102)) AND (销售退出汇总表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp -= Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[4] = dTemp;
            dt2[3] = dTemp;


            dataGridViewDJMX.Rows.Add(objTemp);

            objTemp[0] = "报损金额";
            sqlComm.CommandText = "SELECT SUM(库存报损明细表.报损金额) FROM 库存报损明细表 INNER JOIN 库存报损汇总表 ON 库存报损明细表.单据ID = 库存报损汇总表.ID WHERE (库存报损汇总表.日期 >= CONVERT(DATETIME, '" + SDT0 + " 00:00:00', 102)) AND (库存报损汇总表.日期 <= CONVERT(DATETIME, '" + SDT1 + " 23:59:59', 102)) AND (库存报损汇总表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[1] = dTemp;

            sqlComm.CommandText = "SELECT SUM(库存报损明细表.报损金额) FROM 库存报损明细表 INNER JOIN 库存报损汇总表 ON 库存报损明细表.单据ID = 库存报损汇总表.ID WHERE (库存报损汇总表.日期 >= CONVERT(DATETIME, '" + SDT2 + " 00:00:00', 102)) AND (库存报损汇总表.日期 <= CONVERT(DATETIME, '" + SDT3 + " 23:59:59', 102)) AND (库存报损汇总表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[2] = dTemp;

            sqlComm.CommandText = "SELECT SUM(库存报损明细表.报损金额) FROM 库存报损明细表 INNER JOIN 库存报损汇总表 ON 库存报损明细表.单据ID = 库存报损汇总表.ID WHERE (库存报损汇总表.日期 >= CONVERT(DATETIME, '" + SDT4 + " 00:00:00', 102)) AND (库存报损汇总表.日期 <= CONVERT(DATETIME, '" + SDT5 + " 23:59:59', 102)) AND (库存报损汇总表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[3] = dTemp;

            sqlComm.CommandText = "SELECT SUM(库存报损明细表.报损金额) FROM 库存报损明细表 INNER JOIN 库存报损汇总表 ON 库存报损明细表.单据ID = 库存报损汇总表.ID WHERE (库存报损汇总表.日期 >= CONVERT(DATETIME, '" + SDT6 + " 00:00:00', 102)) AND (库存报损汇总表.日期 <= CONVERT(DATETIME, '" + SDT7 + " 23:59:59', 102)) AND (库存报损汇总表.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[4] = dTemp;
            dataGridViewDJMX.Rows.Add(objTemp);


            objTemp[0] = "借物出库金额";
            sqlComm.CommandText = "SELECT SUM(借物出库汇总表.出库金额) AS Expr1 FROM 借物出库汇总表 WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + SDT0 + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + SDT1 + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 > 0)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[1] = dTemp;

            sqlComm.CommandText = "SELECT SUM(借物出库汇总表.出库金额) AS Expr1 FROM 借物出库汇总表 WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + SDT2 + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + SDT3 + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 > 0)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[2] = dTemp;

            sqlComm.CommandText = "SELECT SUM(借物出库汇总表.出库金额) AS Expr1 FROM 借物出库汇总表  WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + SDT4 + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + SDT5 + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 > 0)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[3] = dTemp;

            sqlComm.CommandText = "SELECT SUM(借物出库汇总表.出库金额) AS Expr1 FROM 借物出库汇总表  WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + SDT6 + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + SDT7 + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 > 0)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[4] = dTemp;
            dataGridViewDJMX.Rows.Add(objTemp);

            objTemp[0] = "借物入库金额";
            sqlComm.CommandText = "SELECT SUM(借物出库汇总表.出库金额) AS Expr1 FROM 借物出库汇总表 WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + SDT0 + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + SDT1 + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 < 0)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[1] = Math.Abs(dTemp);

            sqlComm.CommandText = "SELECT SUM(借物出库汇总表.出库金额) AS Expr1 FROM 借物出库汇总表 WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + SDT2 + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + SDT3 + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 < 0)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[2] = Math.Abs(dTemp);

            sqlComm.CommandText = "SELECT SUM(借物出库汇总表.出库金额) AS Expr1 FROM 借物出库汇总表  WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + SDT4 + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + SDT5 + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 < 0)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[3] = Math.Abs(dTemp);

            sqlComm.CommandText = "SELECT SUM(借物出库汇总表.出库金额) AS Expr1 FROM 借物出库汇总表 WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.日期 >= CONVERT(DATETIME, '" + SDT6 + " 00:00:00', 102)) AND (借物出库汇总表.日期 <= CONVERT(DATETIME, '" + SDT7 + " 23:59:59', 102)) AND (借物出库汇总表.出库金额 < 0)";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[4] = Math.Abs(dTemp);
            dataGridViewDJMX.Rows.Add(objTemp);

            objTemp[0] = "购进退补价金额";
            sqlComm.CommandText = "SELECT SUM(购进退补差价汇总表.价税合计) AS Expr1 FROM 购进退补差价汇总表 WHERE (购进退补差价汇总表.BeActive = 1) AND (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDT0 + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + SDT1 + " 23:59:59', 102)) ";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[1] = dTemp;

            sqlComm.CommandText = "SELECT SUM(购进退补差价汇总表.价税合计) AS Expr1 FROM 购进退补差价汇总表 WHERE (购进退补差价汇总表.BeActive = 1) AND (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDT2 + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + SDT3 + " 23:59:59', 102)) ";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[2] = dTemp;

            sqlComm.CommandText = "SELECT SUM(购进退补差价汇总表.价税合计) AS Expr1 FROM 购进退补差价汇总表 WHERE (购进退补差价汇总表.BeActive = 1) AND (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDT4 + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + SDT5 + " 23:59:59', 102)) ";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[3] = dTemp;

            sqlComm.CommandText = "SELECT SUM(购进退补差价汇总表.价税合计) AS Expr1 FROM 购进退补差价汇总表 WHERE (购进退补差价汇总表.BeActive = 1) AND (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDT6 + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + SDT7 + " 23:59:59', 102)) ";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dTemp = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[4] = dTemp;
            dataGridViewDJMX.Rows.Add(objTemp);

            objTemp[0] = "销售退补价金额";
            sqlComm.CommandText = "SELECT SUM(销售退补差价汇总表.价税合计) AS Expr1 FROM 销售退补差价汇总表 WHERE (销售退补差价汇总表.BeActive = 1) AND (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDT0 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + SDT1 + " 23:59:59', 102)) ";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dt3[0] = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[1] = dt3[0];

            sqlComm.CommandText = "SELECT SUM(销售退补差价汇总表.价税合计) AS Expr1 FROM 销售退补差价汇总表 WHERE (销售退补差价汇总表.BeActive = 1) AND (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDT2 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + SDT3 + " 23:59:59', 102)) ";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dt3[1] = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[2] = dt3[1];

            sqlComm.CommandText = "SELECT SUM(销售退补差价汇总表.价税合计) AS Expr1 FROM 销售退补差价汇总表 WHERE (销售退补差价汇总表.BeActive = 1) AND (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDT4 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + SDT5 + " 23:59:59', 102)) ";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dt3[2] = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[3] = dt3[2];

            sqlComm.CommandText = "SELECT SUM(销售退补差价汇总表.价税合计) AS Expr1 FROM 销售退补差价汇总表 WHERE (销售退补差价汇总表.BeActive = 1) AND (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDT6 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + SDT7 + " 23:59:59', 102)) ";
            sqldr = sqlComm.ExecuteReader();
            dTemp = 0;
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(0).ToString() != "")
                    dt3[3] = Convert.ToDecimal(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            objTemp[4] = dt3[3];
            dataGridViewDJMX.Rows.Add(objTemp);

            objTemp[0] = "毛利";
            for (i = 0; i < 4; i++)
            {
                objTemp[i + 1] = dt1[i] - dt2[i]+dt3[i];
            }
            dataGridViewDJMX.Rows.Add(objTemp);



            sqlConn.Close();

            dataGridViewDJMX.Columns[1].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f2";

            dataGridViewDJMX.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewDJMX.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品经营总况;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品经营总况;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}