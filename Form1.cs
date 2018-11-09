using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class Form1 : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlCommand sqlComm1 = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            sqlConn.ConnectionString = "workstation id=CY;packet size=4096;user id=sa;password=biadcoop;data source=\"172.16.5.183\";;initial catalog=bbb";
            sqlComm.Connection = sqlConn;
            sqlComm1.Connection = sqlConn;

            sqlDA.SelectCommand = sqlComm;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i,j;
            string st,stt,stt1;
            string st1, st2;
            sqlConn.Open();

            sqlComm.CommandText = "UPDATE 商品表 SET 应付金额 = 0, 已付金额 = 0, 应收金额 = 0, 已收金额 = 0, 库存数量 = 0";
            sqlComm.ExecuteNonQuery();


            sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = 0, 应付金额 = 0, 已付金额 = 0, 应收金额 = 0, 已收金额 = 0";
            sqlComm.ExecuteNonQuery();


            sqlComm.CommandText = "SELECT 商品表.ID, bbb.name, bbb.num, bbb.cb FROM bbb INNER JOIN 商品表 ON bbb.name = 商品表.商品名称";
            if (dSet.Tables.Contains("temp")) dSet.Tables.Remove("temp");
            sqlDA.Fill(dSet, "temp");

            for (i = 0; i < dSet.Tables["temp"].Rows.Count; i++)
            {
                sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dSet.Tables["temp"].Rows[i][2].ToString() + ", 库存成本价= " + dSet.Tables["temp"].Rows[i][3].ToString() + " WHERE (ID = " + dSet.Tables["temp"].Rows[i][0].ToString() + ")";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dSet.Tables["temp"].Rows[i][2].ToString() + ", 库存成本价= " + dSet.Tables["temp"].Rows[i][3].ToString() + " WHERE (商品ID = " + dSet.Tables["temp"].Rows[i][0].ToString() + ")";
                sqlComm.ExecuteNonQuery();





            }

            sqlComm.CommandText = "UPDATE 商品表 SET 库存金额 = 库存数量 * 库存成本价";
            sqlComm.ExecuteNonQuery();

            sqlComm.CommandText = "UPDATE 库存表 SET 库存金额 = 库存数量 * 库存成本价";
            sqlComm.ExecuteNonQuery();

            MessageBox.Show("完了");



                sqlConn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT JG.ID, 商品分类表.ID AS 分类ID FROM JG INNER JOIN 商品分类表 ON JG.分类名称 = 商品分类表.分类名称";
            if (dSet.Tables.Contains("商品分类表")) dSet.Tables.Remove("商品分类表");
            sqlDA.Fill(dSet, "商品分类表");

            int i;

            for (i = 0; i < dSet.Tables["商品分类表"].Rows.Count; i++)
            {
                sqlComm.CommandText = "UPDATE JG SET 分类编号 = N'" + dSet.Tables["商品分类表"].Rows[i][1].ToString() + "' WHERE (ID = " + dSet.Tables["商品分类表"].Rows[i][0].ToString() + ")";
                sqlComm.ExecuteNonQuery();

            }

            sqlComm.CommandText = "UPDATE JG SET 库存数量 = 0 WHERE (库存数量 IS NULL)";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "UPDATE JG SET 库存成本价 = 0 WHERE (库存成本价 IS NULL)";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "UPDATE JG SET 最高进价 = 0 WHERE (最高进价 IS NULL)";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "UPDATE JG SET 最低进价 = 0 WHERE (最低进价 IS NULL)";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "UPDATE JG SET 最终进价 = 0 WHERE (最终进价 IS NULL)";
            sqlComm.ExecuteNonQuery();

            sqlComm.CommandText = "SELECT COUNT(*) AS Expr1 FROM JG WHERE (分类编号 = N'')";
            sqldr = sqlComm.ExecuteReader();
            sqldr.Read();



            MessageBox.Show(sqldr.GetValue(0).ToString());

            sqlConn.Close();



        }

        private void button3_Click(object sender, EventArgs e)
        {
            sqlConn.Open();

            sqlComm.CommandText = "SELECT 商品编号, 商品名称, 助记码, 库存数量, 库存成本价, 最高进价, 最低进价, 最终进价, 分类编号 FROM JG";
            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");


            sqlComm.CommandText = "DELETE FROM 商品表";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "DELETE FROM 库存表";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "dbcc checkident(库存表,reseed,0)";
            sqlComm.ExecuteNonQuery();
            sqlComm.CommandText = "dbcc checkident(商品表,reseed,0)";
            sqlComm.ExecuteNonQuery();

            int i, iSelect;

            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                sqlComm.CommandText = "INSERT INTO 商品表 (商品编号, 商品名称, 助记码, 库存数量, 库存成本价, 最高进价, 最低进价, 最终进价, 分类编号, beactive) VALUES (N'" + dSet.Tables["商品表"].Rows[i][0].ToString() + "', N'" + dSet.Tables["商品表"].Rows[i][1].ToString() + "', N'" + dSet.Tables["商品表"].Rows[i][2].ToString() + "', " + dSet.Tables["商品表"].Rows[i][3].ToString() + ", " + dSet.Tables["商品表"].Rows[i][4].ToString() + ", " + dSet.Tables["商品表"].Rows[i][5].ToString() + ", " + dSet.Tables["商品表"].Rows[i][6].ToString() + ", " + dSet.Tables["商品表"].Rows[i][7].ToString() + ", " + dSet.Tables["商品表"].Rows[i][8].ToString() + ", 1)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                iSelect = Convert.ToInt32(sqldr.GetValue(0).ToString());
                sqldr.Close();

                //增加库存
                sqlComm.CommandText = "SELECT 库房ID FROM 商品分类表 WHERE (ID = " + dSet.Tables["商品表"].Rows[i][8].ToString() + ")";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sKF = sqldr.GetValue(0).ToString();
                sqldr.Close();

                if (sKF != "")
                {
                    sqlComm.CommandText = "INSERT INTO 库存表 (库房ID, 商品ID, 库存数量, 库存金额, 库存成本价, 核算成本价, 库存上限, 库存下限, 合理库存上限, 合理库存下限, 应付金额, 已付金额, 应收金额, 已收金额, BeActive) VALUES (" + sKF + ", " + iSelect.ToString() + ", " + dSet.Tables["商品表"].Rows[i][3].ToString() + ", 0, "+dSet.Tables["商品表"].Rows[i][4].ToString()+", 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)";
                    sqlComm.ExecuteNonQuery();
                }

            }

            sqlComm.CommandText = "UPDATE 商品表 SET 库存金额 = 库存数量 * 库存成本价";
            sqlComm.ExecuteNonQuery();

            sqlComm.CommandText = "UPDATE 库存表 SET 库存金额 = 库存数量 * 库存成本价";
            sqlComm.ExecuteNonQuery();

            sqlComm.CommandText = "UPDATE 商品表 SET 计量单位 = N'0', 计量规格 = 1, 最小计量单位 = N'个', 进项税率 = 0, 零售价 = 0, 进价 = 最终进价, 含税进价 = 最终进价, 批发价 = 最终进价, 含税批发价 = 最终进价, 库存件数 = 0, 核算成本价 = 库存成本价, 结转数量 = 0, 结转件数 = 0, 结转金额 = 0, 结转单价 = 0, 登录日期 = '2006-08-09', 提成比例 = 0, 保质期天数 = 0, 保质期方式 = N'没有', 库存上限 = 0, 库存下限 = 0, 合理库存上限 = 0, 合理库存下限 = 0, 经代 = N'经销', 预警天数 = 0, 是否特价商品 = 0, 是否会员商品 = 0, 会员特价 = 0, 销售方式 = 1, 限定批发价 = 0, 包装数量 = 1, 组装商品 = 0, 应付金额 = 0, 已付金额 = 0, 应收金额 = 0, 已收金额 = 0";
            sqlComm.ExecuteNonQuery();


            MessageBox.Show("OVER");

            sqlConn.Close();
        }

   }
}