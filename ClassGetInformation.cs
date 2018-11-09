using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace business
{
    class ClassGetInformation
    {
        public string strConn = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iCompanyNumber = 0;
        public string strCompanyName = "";
        public string strCompanyCode = "";
        public string strCompanySH = "";
        public decimal dCompanyYFZK = 0;
        public decimal dCompanyYSZK = 0;
        public string sCompanyYWY = "";
        public int iBMID = 0;

        public int iVersion = 1;

        public ClassGetInformation(string strConnectionString)
        {
            strConn = strConnectionString;
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
        }

        public void initClassGetInformation()
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
        }

        //iStyle:1,所有供应单位, 2,所有需求单位, 10,所有助记码供应单位, 20,所有助记码需求单位,11,所有单位编号供应单位, 21,所有单位编号需求单位,  100,所有单位,110,所有助记码单位,,120,所有单位编号单位,1000,所有单位，1100,所有单位编号单位, 1200,所有助记码单位,
        //返回: 成功1, 失败0
        public int getCompanyInformation(int iStyle, string strZJM)
        {
            strZJM = strZJM.ToUpper();
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID FROM 单位表 WHERE (是否进货 = 1) AND (BeActive = 1)";
                    break;
                case 2:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID  FROM 单位表 WHERE (是否销售 = 1) AND (BeActive = 1)";
                    break;
                case 10:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID FROM 单位表 WHERE (是否进货 = 1) AND (BeActive = 1) AND (助记码 LIKE '%" + strZJM + "%') OR (单位编号 LIKE '" + strZJM + "%') ";
                    break;
                case 11:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID FROM 单位表 WHERE (是否进货 = 1) AND (单位编号 LIKE '%" + strZJM + "%') AND (BeActive = 1)";
                    break;
                case 12:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID FROM 单位表 WHERE (是否进货 = 1) AND ((助记码 LIKE '%" + strZJM + "%') OR (单位名称 LIKE '%" + strZJM + "%')) AND (BeActive = 1)";
                    break;
                case 20:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID FROM 单位表 WHERE (是否销售 = 1)  AND (BeActive = 1) AND (助记码 LIKE '%" + strZJM + "%') OR (单位编号 LIKE '" + strZJM + "%')";
                    break;
                case 21:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID FROM 单位表 WHERE (是否销售 = 1) AND (单位编号 LIKE '%" + strZJM + "%') AND (BeActive = 1)";
                    break;
                case 22:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID FROM 单位表 WHERE (是否销售 = 1) AND ((助记码 LIKE '%" + strZJM + "%') OR (单位名称 LIKE '%" + strZJM + "%')) AND (BeActive = 1)";
                    break;
                case 100:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID FROM 单位表 WHERE (BeActive = 1) AND (BeActive = 1)";
                    break;
                case 110:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员 FROM 单位表 WHERE (BeActive = 1) AND (助记码 LIKE '%" + strZJM + "%')OR (单位编号 LIKE '" + strZJM + "%')"; 
                    break;
                case 121:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID  FROM 单位表 WHERE (BeActive = 1) AND (单位编号 LIKE '%" + strZJM + "%') AND (BeActive = 1)";
                    break;
                case 120:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID  FROM 单位表 WHERE (BeActive = 1) AND ((助记码 LIKE '%" + strZJM + "%') OR (单位名称 LIKE '%" + strZJM + "%')) AND (BeActive = 1)";
                    break;
                case 1000:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID  FROM 单位表 WHERE (BeActive = 1)";
                    break;
                case 1100:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID  FROM 单位表 WHERE (单位编号 LIKE '%" + strZJM + "%') AND (BeActive = 1)"; 
                    break;
                case 1200:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID  FROM 单位表 WHERE (助记码 LIKE '%" + strZJM + "%') OR (单位名称 LIKE '%" + strZJM + "%') AND (BeActive = 1) ";
                    break;
                case 1300:
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID  FROM 单位表 WHERE (助记码 LIKE '%" + strZJM + "%') OR (单位名称 LIKE '%" + strZJM + "%') AND (BeActive = 1)";
                    break;
                case 41000: //删除单位
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID  FROM 单位表 WHERE (BeActive <> 1)";
                    break;
                case 41200://删除编号单位
                    sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 税号, 应付账款, 应收账款, 业务员, 部门ID  FROM 单位表 WHERE ((助记码 LIKE '%" + strZJM + "%') OR (单位名称 LIKE '%" + strZJM + "%')) AND (BeActive <> 1) ";
                    break;
                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("单位表")) dSet.Tables.Remove("单位表");
            sqlDA.Fill(dSet, "单位表");

            if (dSet.Tables["单位表"].Rows.Count < 1) //没有单位
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["单位表"].Rows.Count == 1) //只有一个单位
            {
                iCompanyNumber = Int32.Parse(dSet.Tables["单位表"].Rows[0][0].ToString());
                strCompanyName = dSet.Tables["单位表"].Rows[0][2].ToString();
                strCompanyCode = dSet.Tables["单位表"].Rows[0][1].ToString();
                strCompanySH = dSet.Tables["单位表"].Rows[0][3].ToString();
                if (dSet.Tables["单位表"].Rows[0][5].ToString() != "")
                    dCompanyYSZK = Convert.ToDecimal(dSet.Tables["单位表"].Rows[0][5].ToString());
                if (dSet.Tables["单位表"].Rows[0][4].ToString() != "")
                    dCompanyYFZK = Convert.ToDecimal(dSet.Tables["单位表"].Rows[0][4].ToString());

                sCompanyYWY = dSet.Tables["单位表"].Rows[0][6].ToString();
                try
                {
                    iBMID = int.Parse(dSet.Tables["单位表"].Rows[0][7].ToString());
                }
                catch
                {
                    iBMID = 0;
                }
                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //多个单位
            FormSelectComPany frmSelectComPany = new FormSelectComPany();
            frmSelectComPany.strConn = strConn;
            frmSelectComPany.strSelectText = sqlComm.CommandText;
            frmSelectComPany.ShowDialog();

            if (frmSelectComPany.iCompanyNumber == 0)
                return 0;
            else
            {
                iCompanyNumber = frmSelectComPany.iCompanyNumber;
                strCompanyName = frmSelectComPany.strCompanyName;
                strCompanyCode = frmSelectComPany.strCompanyCode;
                strCompanySH = frmSelectComPany.strCompanySH;
                dCompanyYSZK = frmSelectComPany.dCompanyYSZK;
                dCompanyYFZK = frmSelectComPany.dCompanyYFZK;
                sCompanyYWY = frmSelectComPany.sCompanyYWY;
                iBMID = frmSelectComPany.iBMID;

                sqlConn.Close();
                return 1;
            }
        }

        public int iClassNumber = 0;
        public string strClassName = "";
        public string strClassCode = "";
        //iStyle:1,所有类别, 10,所有助记码类别, 20,所有编号类别
        //返回: 成功1, 失败0
        public int getClassInformation(int iStyle, string strZJM)
        {
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM 商品分类表 WHERE BeActive = 1 ";
                    break;
                case 10:
                    sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM 商品分类表 WHERE (分类编号 LIKE '%" + strZJM + "%') AND  BeActive = 1";
                    break;
                case 20:
                    sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM 商品分类表 WHERE (分类名称 LIKE '%" + strZJM + "%') AND  BeActive = 1";
                    break;

                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("商品分类表")) dSet.Tables.Remove("商品分类表");
            sqlDA.Fill(dSet, "商品分类表");

            if (dSet.Tables["商品分类表"].Rows.Count < 1) //没有商品分类表
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["商品分类表"].Rows.Count == 1) //只有一个商品分类表
            {
                iClassNumber = Int32.Parse(dSet.Tables["商品分类表"].Rows[0][0].ToString());
                strClassName = dSet.Tables["商品分类表"].Rows[0][2].ToString();
                strClassCode = dSet.Tables["商品分类表"].Rows[0][1].ToString();

                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //多个商品分类表
            FormSelectClass frmSelectClass = new FormSelectClass();
            frmSelectClass.strConn = strConn;
            frmSelectClass.strSelectText = sqlComm.CommandText;
            frmSelectClass.ShowDialog();

            if (frmSelectClass.iClassNumber == 0)
                return 0;
            else
            {
                iClassNumber = frmSelectClass.iClassNumber;
                strClassName = frmSelectClass.strClassName;
                strClassCode = frmSelectClass.strClassCode;
                sqlConn.Close();
                return 1;
            }

        }


        //
        public int intUnderClassNumber = 0;
        public int[] intUnderClass=new int[100];
        public int intUpClassNumber = 0;

        //得到所有下级类别号
        public void getUnderClassInformation(int iClassNumber)
        {
            intUnderClassNumber = 0;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ID FROM 商品分类表 WHERE (上级分类 = '0," + iClassNumber.ToString() + "')";

            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                if (intUnderClassNumber >= 100)
                    break;
                intUnderClass[intUnderClassNumber]=int.Parse(sqldr.GetValue(0).ToString());
                intUnderClassNumber++;
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT 上级分类 FROM 商品分类表 WHERE (ID = " + iClassNumber.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            string sTem="";
            while (sqldr.Read())
            {
                sTem=sqldr.GetValue(0).ToString();
            }
            if (sTem.LastIndexOf(',') == 0)
            {
                try
                {
                    intUpClassNumber = int.Parse(sTem);
                }
                catch
                {
                    intUpClassNumber = 0;
                }
            }
            else
            {
                sTem = sTem.Substring(sTem.LastIndexOf(',')+1, sTem.Length - sTem.LastIndexOf(',')-1);
                try
                {
                    intUpClassNumber = int.Parse(sTem);
                }
                catch
                {
                    intUpClassNumber = 0;
                }
            }
            sqlConn.Close();
        }


        public int iCommNumber = 0;
        public string strCommName = "";
        public string strCommCode = "";
        public string strCommCount = "";
        public string strCommGG = "";
        public decimal decCommKCCBJ;
        public decimal decCommHSCBJ;
        public decimal decCommZZJJ;
        public decimal decCommJJ=0;
        public decimal decCommPFJ=0;
        public decimal decCommZGJJ = 0;
        public decimal decCommZDJJ = 0;
        public decimal decCommKCSL=0;


        //iStyle:1,所有商品, 10,所有助记码商品, 20,所有编号商品, 30, 模糊查询, 40, 所有ID商品，101，所有组合商品，102。所有助记码组合商品
        //返回: 成功1, 失败0
        public int getCommInformation(int iStyle, string strZJM)
        {
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, CONVERT(numeric(8, 2), ROUND(商品表.库存成本价, 2)) AS 库存成本价, CONVERT(numeric(8, 2), ROUND(商品表.核算成本价, 2)) AS 核算成本价, CONVERT(numeric(8, 2), ROUND(商品表.最终进价, 2)) AS 最终进价, 商品表.分类编号, 商品分类表.上级分类, 商品表.进价, 商品表.批发价, 商品表.最高进价, 商品表.最低进价, 商品表.库存数量 FROM 商品表 LEFT OUTER JOIN 商品分类表 ON 商品表.分类编号 = 商品分类表.ID WHERE (商品表.beactive = 1) ORDER BY 商品表.商品编号";
                    break;
                case 10:
                    //同名商品

                    sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, CONVERT(numeric(8, 2), ROUND(商品表.库存成本价, 2)) AS 库存成本价, CONVERT(numeric(8, 2), ROUND(商品表.核算成本价, 2)) AS 核算成本价, CONVERT(numeric(8, 2), ROUND(商品表.最终进价, 2)) AS 最终进价, 商品表.分类编号, 商品分类表.上级分类, 商品表.进价, 商品表.批发价, 商品表.最高进价, 商品表.最低进价, 商品表.库存数量  FROM 商品表 LEFT OUTER JOIN 商品分类表 ON 商品表.分类编号 = 商品分类表.ID WHERE (商品表.beactive = 1) AND ((商品表.助记码 LIKE '%" + strZJM + "%') OR (商品表.商品名称 LIKE '%" + strZJM + "%')) ORDER BY 商品表.商品编号";
                    break;
                case 20:
                    sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, CONVERT(numeric(8, 2), ROUND(商品表.库存成本价, 2)) AS 库存成本价, CONVERT(numeric(8, 2), ROUND(商品表.核算成本价, 2)) AS 核算成本价, CONVERT(numeric(8, 2), ROUND(商品表.最终进价, 2)) AS 最终进价, 商品表.分类编号, 商品分类表.上级分类, 商品表.进价, 商品表.批发价, 商品表.最高进价, 商品表.最低进价, 商品表.库存数量 FROM 商品表 LEFT OUTER JOIN 商品分类表 ON 商品表.分类编号 = 商品分类表.ID WHERE (商品表.beactive = 1) AND (商品表.商品编号 LIKE '%" + strZJM + "%') ORDER BY 商品表.商品编号";
                    break;
                case 40:
                    sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, CONVERT(numeric(8, 2), ROUND(商品表.库存成本价, 2)) AS 库存成本价, CONVERT(numeric(8, 2), ROUND(商品表.核算成本价, 2)) AS 核算成本价, CONVERT(numeric(8, 2), ROUND(商品表.最终进价, 2)) AS 最终进价, 商品表.分类编号, 商品分类表.上级分类, 商品表.进价, 商品表.批发价, 商品表.最高进价, 商品表.最低进价, 商品表.库存数量 FROM 商品表 LEFT OUTER JOIN 商品分类表 ON 商品表.分类编号 = 商品分类表.ID WHERE (商品表.beactive = 1) AND (商品表.ID = " + strZJM + ") ORDER BY 商品表.商品编号";
                    break;
                case 101:
                    sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, CONVERT(numeric(8, 2), ROUND(商品表.库存成本价, 2)) AS 库存成本价, CONVERT(numeric(8, 2), ROUND(商品表.核算成本价, 2)) AS 核算成本价, CONVERT(numeric(8, 2), ROUND(商品表.最终进价, 2)) AS 最终进价, 商品表.分类编号, 商品分类表.上级分类, 商品表.进价, 商品表.批发价, 商品表.最高进价, 商品表.最低进价, 商品表.库存数量 FROM 商品表 LEFT OUTER JOIN 商品分类表 ON 商品表.分类编号 = 商品分类表.ID WHERE (商品表.beactive = 1) AND (商品表.组装商品 = 1) ORDER BY 商品表.商品编号";
                    break;
                case 102:
                    sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 商品表.最小计量单位 AS 单位, CONVERT(numeric(8, 2), ROUND(商品表.库存成本价, 2)) AS 库存成本价, CONVERT(numeric(8, 2), ROUND(商品表.核算成本价, 2)) AS 核算成本价, CONVERT(numeric(8, 2), ROUND(商品表.最终进价, 2)) AS 最终进价, 商品表.分类编号, 商品分类表.上级分类, 商品表.进价, 商品表.批发价, 商品表.最高进价, 商品表.最低进价, 商品表.库存数量 FROM 商品表 LEFT OUTER JOIN 商品分类表 ON 商品表.分类编号 = 商品分类表.ID WHERE (商品表.beactive = 1) AND (商品表.组装商品 = 1) AND ((商品表.助记码 LIKE '%" + strZJM + "%') OR (商品表.商品名称 LIKE '%" + strZJM + "%')) ORDER BY 商品表.商品编号";
                    break;
                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");

            if (dSet.Tables["商品表"].Rows.Count < 1) //没有商品
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["商品表"].Rows.Count == 1) //只有一个商品
            {
                iCommNumber = Int32.Parse(dSet.Tables["商品表"].Rows[0][0].ToString());
                strCommName = dSet.Tables["商品表"].Rows[0][1].ToString();
                strCommCode = dSet.Tables["商品表"].Rows[0][2].ToString();
                strCommGG = dSet.Tables["商品表"].Rows[0][3].ToString();
                strCommCount = dSet.Tables["商品表"].Rows[0][4].ToString();

                if (dSet.Tables["商品表"].Rows[0][5].ToString().Trim() != "")
                    decCommKCCBJ = Decimal.Parse(dSet.Tables["商品表"].Rows[0][5].ToString().Trim());
                else
                    decCommKCCBJ = 0;

                if (dSet.Tables["商品表"].Rows[0][6].ToString().Trim() != "")
                    decCommHSCBJ = Decimal.Parse(dSet.Tables["商品表"].Rows[0][6].ToString().Trim());
                else
                    decCommHSCBJ = 0;

                if (dSet.Tables["商品表"].Rows[0][7].ToString().Trim() != "")
                    decCommZZJJ = Decimal.Parse(dSet.Tables["商品表"].Rows[0][7].ToString().Trim());
                else
                    decCommZZJJ = 0;

                if (dSet.Tables["商品表"].Rows[0][10].ToString().Trim() != "")
                    decCommJJ = Decimal.Parse(dSet.Tables["商品表"].Rows[0][10].ToString().Trim());
                else
                    decCommJJ = 0;

                if (dSet.Tables["商品表"].Rows[0][11].ToString().Trim() != "")
                    decCommPFJ = Decimal.Parse(dSet.Tables["商品表"].Rows[0][11].ToString().Trim());
                else
                    decCommPFJ = 0;

                if (dSet.Tables["商品表"].Rows[0][12].ToString().Trim() != "")
                    decCommZGJJ = Decimal.Parse(dSet.Tables["商品表"].Rows[0][12].ToString().Trim());
                else
                    decCommZGJJ = 0;

                if (dSet.Tables["商品表"].Rows[0][13].ToString().Trim() != "")
                    decCommZDJJ = Decimal.Parse(dSet.Tables["商品表"].Rows[0][13].ToString().Trim());
                else
                    decCommZDJJ = 0;

                if (dSet.Tables["商品表"].Rows[0][14].ToString().Trim() != "")
                    decCommKCSL = Decimal.Parse(dSet.Tables["商品表"].Rows[0][14].ToString().Trim());
                else
                    decCommKCSL = 0;

                
                sqlConn.Close();
                getCommKF();
                return 1;
            }

            sqlConn.Close();
            //多个商品
            FormSelectCommodities frmSelectCommodities = new FormSelectCommodities();
            frmSelectCommodities.strConn = strConn;
            frmSelectCommodities.strSelectText = sqlComm.CommandText;
            frmSelectCommodities.ShowDialog();

            if (frmSelectCommodities.iCommNumber == -1)
                return -1;

            if (frmSelectCommodities.iCommNumber== 0)
                return 0;
            else
            {
                iCommNumber = frmSelectCommodities.iCommNumber;
                strCommName = frmSelectCommodities.strCommName;
                strCommCode = frmSelectCommodities.strCommCode;
                strCommGG = frmSelectCommodities.strCommGG;
                strCommCount = frmSelectCommodities.strCommCount;
                decCommKCCBJ = frmSelectCommodities.decCommKCCBJ;
                decCommHSCBJ = frmSelectCommodities.decCommHSCBJ;
                decCommZZJJ = frmSelectCommodities.decCommZZJJ;
                decCommJJ = frmSelectCommodities.decCommJJ;
                decCommPFJ = frmSelectCommodities.decCommPFJ;
                decCommZGJJ = frmSelectCommodities.decCommZGJJ;
                decCommZDJJ = frmSelectCommodities.decCommZDJJ;
                decCommKCSL = frmSelectCommodities.decCommKCSL;

                getCommKF();
                sqlConn.Close();

                return 1;
            }

        }

        //得到商品缺省库房
        public void getCommKF()
        {
            iKFNumber = 0;
            strKFName = "";
            strKFCode = "";

            if (iCommNumber == 0)
                return;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品分类表.库房ID, 库房表.库房编号, 库房表.库房名称 FROM 商品表 INNER JOIN 商品分类表 ON 商品表.分类编号 = 商品分类表.ID INNER JOIN 库房表 ON 商品分类表.库房ID = 库房表.ID WHERE (商品表.ID = " + iCommNumber .ToString()+ ")";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                iKFNumber = Convert.ToInt32(sqldr.GetValue(0).ToString());
                strKFCode = sqldr.GetValue(1).ToString();
                strKFName = sqldr.GetValue(2).ToString();
            }
            sqldr.Close();


            sqlConn.Close();
        }

        public int iKFNumber = 0;
        public string strKFName = "";
        public string strKFCode = "";

        //iStyle:1,所有库房, 10,编号所有库房 20，所有助记码库房
        //返回: 成功1, 失败0
        public int getKFInformation(int iStyle, string strZJM)
        {
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ID, 库房编号, 库房名称 FROM 库房表 WHERE (BeActive = 1) ORDER BY 库房编号";
                    break;
                case 10:
                    sqlComm.CommandText = "SELECT ID, 库房编号, 库房名称 FROM 库房表 WHERE (BeActive = 1) AND (库房编号 LIKE '%" + strZJM + "%') ORDER BY 库房编号";
                    break;
                case 20:
                    sqlComm.CommandText = "SELECT ID, 库房编号, 库房名称 FROM 库房表 WHERE (BeActive = 1) AND ((助记码 LIKE '%" + strZJM + "%') OR 库房名称 LIKE N'%" + strZJM + "%') ORDER BY 库房编号";
                    break;
                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("库房表")) dSet.Tables.Remove("库房表");
            sqlDA.Fill(dSet, "库房表");

            if (dSet.Tables["库房表"].Rows.Count < 1) //没有库房
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["库房表"].Rows.Count == 1) //只有一个商品
            {
                iKFNumber = Int32.Parse(dSet.Tables["库房表"].Rows[0][0].ToString());
                strKFName = dSet.Tables["库房表"].Rows[0][2].ToString();
                strKFCode = dSet.Tables["库房表"].Rows[0][1].ToString();

                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //多个库房
            FormSelectKF frmSelectKF = new FormSelectKF();
            frmSelectKF.strConn = strConn;
            frmSelectKF.strSelectText = sqlComm.CommandText;
            frmSelectKF.ShowDialog();

            if (frmSelectKF.iKFNumber == 0)
                return 0;
            else
            {
                iKFNumber = frmSelectKF.iKFNumber;
                strKFName = frmSelectKF.strKFName;
                strKFCode = frmSelectKF.strKFCode;
                sqlConn.Close();
                return 1;
            }

        }

        public int iZBNumber = 0;
        public string strZBName = "";
        public string strZBCode = "";
        public decimal dZBKL = 100;

        //iStyle:1,所有支付账簿, 10,编号所有支付账簿 11，所有助记所有支付账簿
        //返回: 成功1, 失败0
        public int getZBInformation(int iStyle, string strZJM)
        {
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ID, 账簿编号, 账簿名称, 扣率 FROM 账簿表 WHERE (BeActive = 1) AND (是否可支付 = 1)";
                    break;
                case 10:
                    sqlComm.CommandText = "SELECT ID, 账簿编号, 账簿名称, 扣率 FROM 账簿表 WHERE (BeActive = 1) AND (是否可支付 = 1) AND (账簿编号 LIKE N'%"+strZJM+"%')";
                    break;
                case 11:
                    sqlComm.CommandText = "SELECT ID, 账簿编号, 账簿名称, 扣率 FROM 账簿表 WHERE (BeActive = 1) AND (是否可支付 = 1) AND ((助记码 LIKE '%" + strZJM + "%') OR (账簿名称 LIKE '%" + strZJM + "%'))";
                    break;
                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("账簿表")) dSet.Tables.Remove("账簿表");
            sqlDA.Fill(dSet, "账簿表");

            if (dSet.Tables["账簿表"].Rows.Count < 1) //没有账簿
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["账簿表"].Rows.Count == 1) //只有一个账簿
            {
                iZBNumber = Int32.Parse(dSet.Tables["账簿表"].Rows[0][0].ToString());
                strZBName = dSet.Tables["账簿表"].Rows[0][2].ToString();
                strZBCode = dSet.Tables["账簿表"].Rows[0][1].ToString();
                dZBKL = Convert.ToDecimal(dSet.Tables["账簿表"].Rows[0][3].ToString());

                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //多个账簿
            FormSelectZB frmSelectZB = new FormSelectZB();
            frmSelectZB.strConn = strConn;
            frmSelectZB.strSelectText = sqlComm.CommandText;
            frmSelectZB.ShowDialog();

            if (frmSelectZB.iZBNumber == 0)
                return 0;
            else
            {
                iZBNumber = frmSelectZB.iZBNumber;
                strZBName = frmSelectZB.strZBName;
                strZBCode = frmSelectZB.strZBCode;
                dZBKL = frmSelectZB.dZBKL;
                sqlConn.Close();
                return 1;
            }

        }

        public int iBMNumber = 0;
        public string strBMName = "";
        public string strBMCode = "";

        //iStyle:1,所有部门
        //返回: 成功1, 失败0
        public int getBMInformation(int iStyle, string strZJM)
        {
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ID, 部门编号, 部门名称 FROM 部门表 WHERE (BeActive = 1)";
                    break;
                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("部门表")) dSet.Tables.Remove("部门表");
            sqlDA.Fill(dSet, "部门表");

            if (dSet.Tables["部门表"].Rows.Count < 1) //没有部门
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["部门表"].Rows.Count == 1) //只有一个部门
            {
                iBMNumber = Int32.Parse(dSet.Tables["部门表"].Rows[0][0].ToString());
                strBMName = dSet.Tables["部门表"].Rows[0][2].ToString();
                strBMCode = dSet.Tables["部门表"].Rows[0][1].ToString();

                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //多个
            FormSelectBM frmSelectBM = new FormSelectBM();
            frmSelectBM.strConn = strConn;
            frmSelectBM.strSelectText = sqlComm.CommandText;
            frmSelectBM.ShowDialog();

            if (frmSelectBM.iBMNumber == 0)
                return 0;
            else
            {
                iBMNumber = frmSelectBM.iBMNumber;
                strBMName = frmSelectBM.strBMName;
                strBMCode = frmSelectBM.strBMCode;
                sqlConn.Close();
                return 1;
            }

        }

        public int iGWNumber = 0;
        public string strGWName = "";
        public string strGWCode = "";

        //iStyle:1,所有岗位
        //返回: 成功1, 失败0
        public int getGWInformation(int iStyle, string strZJM)
        {
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ID, 岗位编号, 岗位名称 FROM 岗位表";
                    break;
                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("岗位表")) dSet.Tables.Remove("岗位表");
            sqlDA.Fill(dSet, "岗位表");

            if (dSet.Tables["岗位表"].Rows.Count < 1) //没有部门
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["岗位表"].Rows.Count == 1) //只有一个部门
            {
                iGWNumber = Int32.Parse(dSet.Tables["岗位表"].Rows[0][0].ToString());
                strGWName = dSet.Tables["岗位表"].Rows[0][2].ToString();
                strGWCode = dSet.Tables["岗位表"].Rows[0][1].ToString();

                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //多个
            FormSelectGW frmSelectGW = new FormSelectGW();
            frmSelectGW.strConn = strConn;
            frmSelectGW.strSelectText = sqlComm.CommandText;
            frmSelectGW.ShowDialog();

            if (frmSelectGW.iGWNumber == 0)
                return 0;
            else
            {
                iGWNumber = frmSelectGW.iGWNumber;
                strGWName = frmSelectGW.strGWName;
                strGWCode = frmSelectGW.strGWCode;
                sqlConn.Close();
                return 1;
            }

        }




        //得到大写金额
        public string changeDAXIE(string sIn)
        {
            string s = double.Parse(sIn).ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");//d + "\n" +
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            string t=Regex.Replace(d, ".", delegate(Match m) { return "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString(); });

            if (t == "")
                t = "零";

            return t+"整";

        }

        //得到单据计数器，错误返回""
        public string strSYSDATATIME = "";
                        //得到服务器日期
        public void getSystemDateTime()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT GETDATE() AS 日期";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                strSYSDATATIME = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
            }
            sqlConn.Close();
        }

        public string getBillNumber(string strKey)
        {
            string strDateSYS="",strCount="";


            if (strKey == "")
                return "";

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //得到服务器日期
                sqlComm.CommandText = "SELECT GETDATE() AS 日期";
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                }
                strSYSDATATIME = strDateSYS;
                sqldr.Close();

                //得到日期
                sqlComm.CommandText = "SELECT 时间 FROM 表单计数表 WHERE (时间 = CONVERT(DATETIME, '"+strDateSYS+" 00:00:00', 102))";
                sqldr = sqlComm.ExecuteReader();

                if (sqldr.HasRows)
                    sqldr.Close();
                else //服务器时间不吻合
                {
                    sqldr.Close();
                    //修正日期及计数器
                    sqlComm.CommandText = "UPDATE 表单计数表 SET 时间 = '"+strDateSYS+"', 计数 = 1";
                    sqlComm.ExecuteNonQuery();
                }

                //得到计数器
                sqlComm.CommandText = "SELECT 计数 FROM 表单计数表 WHERE (关键词 = N'" + strKey + "')";
                sqldr = sqlComm.ExecuteReader();
                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    strCount = sqldr.GetValue(0).ToString();
                    sqldr.Close();

                    //增加计数器
                    sqlComm.CommandText = "UPDATE 表单计数表 SET 计数 = 计数 + 1 WHERE (关键词 = N'" + strKey + "')";
                    sqlComm.ExecuteNonQuery();
                }
                else
                    sqldr.Close();


                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                return "";
            }
            finally
            {
                sqlConn.Close();
            }

            if (strCount!="")
            {
                if (iVersion <= 0)
                {
                    if (int.Parse(strCount) > 2)
                    {
                        MessageBox.Show("预览版用户每天只可以做两单", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        strCount = "";
                        return strCount;
                    }
                }
                strCount = string.Format("{0:D3}", Int32.Parse(strCount));
                strCount = strKey.ToUpper()+Convert.ToDateTime(strDateSYS).ToString("yyyyMMdd") + strCount;
             }
            return strCount;
        }


        public void ClearDataGridViewErrorText(DataGridView dvIn)
        {
            if (dvIn.CurrentCell == null)
                return;
            for (int i = 0; i < dvIn.ColumnCount; i++)
            {
                dvIn.Rows[dvIn.CurrentCell.RowIndex].Cells[i].ErrorText = String.Empty;
            }
        }

        public int iBillNumber = 0;
        public string strBillCode = "";
        public int iBillCNumber =0;
        public string strBillCCode="";
        public string strBillCName = "";
        public int iPeopleNumber = 0;
        public string sPeopleName = "";
        public int iBillBMID = 0;

        //iStyle:1,所有购进单, 2,所有销售单, 10,编号所有购进单, 20,编号所有销售单, 3,所有借物单, 30,编号所有借物单,4,所有调价单, 40,编号所有调价单,5
        //返回: 成功1, 失败0
        public int getBillInformation(int iStyle, string strZJM)
        {
            bool bDWMC = true;
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT 购进商品制单表.ID, 单位表.ID AS 单位ID, 购进商品制单表.单据编号, 单位表.单位编号, 单位表.单位名称, 购进商品制单表.日期, 购进商品制单表.价税合计 AS 金额, 购进商品制单表.备注, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID FROM 购进商品制单表 INNER JOIN 单位表 ON 购进商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 购进商品制单表.业务员ID = 职员表.ID WHERE (购进商品制单表.入库标记 = 0) AND (购进商品制单表.BeActive = 1) ORDER BY 购进商品制单表.日期 DESC";
                    break;
                case 2:
                    sqlComm.CommandText = "SELECT 销售商品制单表.ID, 单位表.ID AS 单位ID, 销售商品制单表.单据编号, 单位表.单位编号, 单位表.单位名称, 销售商品制单表.日期, 销售商品制单表.价税合计 AS 金额, 销售商品制单表.备注, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售商品制单表.业务员ID = 职员表.ID WHERE (销售商品制单表.出库标记 = 0) AND (销售商品制单表.BeActive = 1) ORDER BY 销售商品制单表.日期 DESC";
                    break;

                case 6602:
                    sqlComm.CommandText = "SELECT 销售商品制单表.ID, 单位表.ID AS 单位ID, 销售商品制单表.单据编号, 单位表.单位编号, 单位表.单位名称, 销售商品制单表.日期, 销售商品制单表.价税合计 AS 金额, 销售商品制单表.备注, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售商品制单表.业务员ID = 职员表.ID WHERE (销售商品制单表.出库标记 = 0) AND (销售商品制单表.BeActive = 1) ORDER BY 销售商品制单表.日期 DESC";
                    break;
                case 211:
                    sqlComm.CommandText = "SELECT 销售商品制单表.ID, 单位表.ID AS 单位ID, 销售商品制单表.单据编号, 单位表.单位编号, 单位表.单位名称, 销售商品制单表.日期, 销售商品制单表.价税合计 AS 金额, 销售商品制单表.备注, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售商品制单表.业务员ID = 职员表.ID WHERE (销售商品制单表.BeActive = 1) ORDER BY 销售商品制单表.日期 DESC";
                    break;
                case 80211:
                    sqlComm.CommandText = "SELECT 销售商品制单表.ID, 单位表.ID AS 单位ID, 销售商品制单表.单据编号, 单位表.单位编号, 单位表.单位名称, 销售商品制单表.日期, 销售商品制单表.价税合计 AS 金额,  销售商品制单表.备注, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售商品制单表.业务员ID = 职员表.ID INNER JOIN (SELECT     表单ID, SUM(数量) AS 商品数量 FROM 销售商品制单明细表 GROUP BY 表单ID) AS B ON 销售商品制单表.ID = B.表单ID LEFT OUTER JOIN (SELECT     COUNT(*) AS 条码数量, 单据编号 FROM 商品条码表 GROUP BY 单据编号) AS A ON 销售商品制单表.单据编号 = A.单据编号 WHERE     (销售商品制单表.BeActive = 1) AND (A.条码数量 IS NULL OR A.条码数量 < B.商品数量) ORDER BY 销售商品制单表.日期 DESC";
                    break;

                case 3:
                    sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 单位表.ID AS 单位ID, 借物出库汇总表.单据编号, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.日期, 借物出库汇总表.价税合计 AS 金额, 借物出库汇总表.备注, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID WHERE (借物出库汇总表.BeActive = 1) ORDER BY 借物出库汇总表.日期 DESC";
                    break;
                case 80003:
                    sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 单位表.ID AS 单位ID, 借物出库汇总表.单据编号, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.日期, 借物出库汇总表.价税合计 AS 金额, 借物出库汇总表.备注, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID INNER JOIN (SELECT     表单ID, SUM(数量) AS 商品数量 FROM 借物出库明细表 GROUP BY 表单ID) AS B ON 借物出库汇总表.ID = B.表单ID LEFT OUTER JOIN (SELECT     COUNT(*) AS 条码数量, 单据编号 FROM 商品条码表 GROUP BY 单据编号) AS A ON 借物出库汇总表.单据编号 = A.单据编号 WHERE (借物出库汇总表.BeActive = 1) AND (A.条码数量 IS NULL OR A.条码数量 < B.商品数量) ORDER BY 借物出库汇总表.日期 DESC";
                    break;
                case 10:
                    sqlComm.CommandText = "SELECT 购进商品制单表.ID, 单位表.ID AS 单位ID, 购进商品制单表.单据编号, 单位表.单位编号, 单位表.单位名称, 购进商品制单表.日期, 购进商品制单表.价税合计 AS 金额, 购进商品制单表.备注, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 购进商品制单表 INNER JOIN 单位表 ON 购进商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 购进商品制单表.业务员ID = 职员表.ID WHERE (购进商品制单表.入库标记 = 0) AND (购进商品制单表.BeActive = 1) AND (购进商品制单表.单据编号 LIKE '%" + strZJM + "%') ORDER BY 购进商品制单表.日期 DESC";
                    break;
                case 20:
                    sqlComm.CommandText = "SELECT 销售商品制单表.ID, 单位表.ID AS 单位ID, 销售商品制单表.单据编号, 单位表.单位编号, 单位表.单位名称, 销售商品制单表.日期, 销售商品制单表.价税合计 AS 金额, 销售商品制单表.备注, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售商品制单表.业务员ID = 职员表.ID WHERE (销售商品制单表.出库标记 = 0) AND (销售商品制单表.BeActive = 1) AND (销售商品制单表.单据编号 LIKE '%" + strZJM + "%') ORDER BY 销售商品制单表.日期 DESC";
                    break;
                case 2011:
                    sqlComm.CommandText = "SELECT 销售商品制单表.ID, 单位表.ID AS 单位ID, 销售商品制单表.单据编号, 单位表.单位编号, 单位表.单位名称, 销售商品制单表.日期, 销售商品制单表.价税合计 AS 金额, 销售商品制单表.备注, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售商品制单表.业务员ID = 职员表.ID WHERE (销售商品制单表.BeActive = 1) AND (销售商品制单表.单据编号 LIKE '%" + strZJM + "%') ORDER BY 销售商品制单表.日期 DESC";
                    break;
                case 30:
                    sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 单位表.ID AS 单位ID, 借物出库汇总表.单据编号, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.日期, 借物出库汇总表.价税合计 AS 金额, 借物出库汇总表.备注, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.单据编号 LIKE '%" + strZJM + "%') ORDER BY 借物出库汇总表.日期 DESC";
                    break;
                case 31:
                    sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 单位表.ID AS 单位ID, 借物出库汇总表.单据编号, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.日期, 借物出库汇总表.价税合计 AS 金额, 借物出库汇总表.备注, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.单据编号 LIKE '%" + strZJM + "%') AND (借物出库汇总表.冲抵单号ID IS NULL) ORDER BY 借物出库汇总表.日期 DESC";
                    break;
                case 32:
                    sqlComm.CommandText = "SELECT 借物出库汇总表.ID, 单位表.ID AS 单位ID, 借物出库汇总表.单据编号, 单位表.单位编号, 单位表.单位名称, 借物出库汇总表.日期, 借物出库汇总表.价税合计 AS 金额, 借物出库汇总表.备注, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 借物出库汇总表 INNER JOIN 单位表 ON 借物出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 借物出库汇总表.业务员ID = 职员表.ID WHERE (借物出库汇总表.BeActive = 1) AND (借物出库汇总表.冲抵单号ID IS NULL) ORDER BY 借物出库汇总表.日期 DESC";
                    break;

                case 4:
                    sqlComm.CommandText = "SELECT 调价通知单汇总表.ID, 调价通知单汇总表.ID AS Expr1,调价通知单汇总表.单据编号, [职员表_1].职员姓名 AS 业务员, [职员表_1].职员姓名 AS 操作员, 调价通知单汇总表.日期, 调价通知单汇总表.执行标记, 调价通知单汇总表.备注, [职员表_2].ID AS 业务员ID, [职员表_2].职员姓名 AS 业务员,[职员表_1].部门ID  FROM 调价通知单汇总表 INNER JOIN 职员表 [职员表_1] ON 调价通知单汇总表.操作员ID = [职员表_1].ID INNER JOIN 职员表 [职员表_2] ON 调价通知单汇总表.业务员ID = [职员表_2].ID WHERE (调价通知单汇总表.BeActive <> 0) AND (调价通知单汇总表.执行标记 = 0) ORDER BY 调价通知单汇总表.日期 DESC";
                    break;

                case 40:
                    sqlComm.CommandText = "SELECT 调价通知单汇总表.ID, 调价通知单汇总表.ID AS Expr1,调价通知单汇总表.单据编号, [职员表_1].职员姓名 AS 业务员, [职员表_1].职员姓名 AS 操作员, 调价通知单汇总表.日期, 调价通知单汇总表.执行标记, 调价通知单汇总表.备注, [职员表_2].ID AS 业务员ID, [职员表_2].职员姓名 AS 业务员, 单位表.部门ID  FROM 调价通知单汇总表 INNER JOIN 职员表 [职员表_1] ON 调价通知单汇总表.操作员ID = [职员表_1].ID INNER JOIN 职员表 [职员表_2] ON 调价通知单汇总表.业务员ID = [职员表_2].ID WHERE (调价通知单汇总表.BeActive <> 0) AND (调价通知单汇总表.执行标记 = 0) AND (调价通知单汇总表.单据编号 LIKE '%" + strZJM + "%') ORDER BY 调价通知单汇总表.日期 DESC";
                    break;

                case 50: //进货合同,未执行
                    sqlComm.CommandText = "SELECT 采购合同表.ID, 采购合同表.供方单位ID, 采购合同表.合同编号 AS 单据编号, 单位表.单位编号, 单位表.单位名称, 采购合同表.签订时间 AS 日期, 采购合同表.金额, 单位表.开户银行, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID WHERE (采购合同表.BeActive = 1) AND (采购合同表.退货标记 = 0) AND (采购合同表.执行标记 = 0) ORDER BY 采购合同表.签订时间 DESC";
                    break;
                case 51:  //已进货，非退货进货合同
                    sqlComm.CommandText = "SELECT 采购合同表.ID, 采购合同表.供方单位ID, 采购合同表.合同编号 AS 单据编号, 单位表.单位编号, 单位表.单位名称, 采购合同表.签订时间 AS 日期, 采购合同表.金额, 单位表.开户银行, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID WHERE (采购合同表.BeActive = 1) AND (采购合同表.退货标记 = 0) AND (采购合同表.执行标记 = 1) ORDER BY 采购合同表.签订时间 DESC";
                    break;
                case 52:  //销售合同
                    sqlComm.CommandText = "SELECT 销售合同表.ID, 销售合同表.供方单位ID, 销售合同表.合同编号 AS 单据编号, 单位表.单位编号, 单位表.单位名称, 销售合同表.签订时间 AS 日期, 销售合同表.金额, 单位表.开户银行, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID WHERE (销售合同表.BeActive = 1) AND (销售合同表.退货标记 = 0) AND (销售合同表.执行标记 = 0) ORDER BY 销售合同表.签订时间 DESC";
                    break;
                case 53:  //已出货，非退货销售合同
                    sqlComm.CommandText = "SELECT 销售合同表.ID, 销售合同表.供方单位ID, 销售合同表.合同编号 AS 单据编号, 单位表.单位编号, 单位表.单位名称, 销售合同表.签订时间 AS 日期, 销售合同表.金额, 单位表.开户银行, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID WHERE (销售合同表.BeActive = 1) AND (销售合同表.退货标记 = 0) AND (销售合同表.执行标记 = 1) ORDER BY 销售合同表.签订时间 DESC";
                    break;
                case 54://所有合同
                    sqlComm.CommandText = "(SELECT 采购合同表.ID, 采购合同表.供方单位ID, 采购合同表.合同编号 AS 单据编号, 单位表.单位编号, 单位表.单位名称, 采购合同表.签订时间 AS 日期, 采购合同表.金额, 单位表.开户银行, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID WHERE (采购合同表.BeActive = 1) ORDER BY 采购合同表.签订时间 DESC) UNION (SELECT 销售合同表.ID, 销售合同表.供方单位ID, 销售合同表.合同编号 AS 单据编号, 单位表.单位编号, 单位表.单位名称, 销售合同表.签订时间 AS 日期, 销售合同表.金额, 单位表.开户银行, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员 FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID WHERE (销售合同表.BeActive = 1) ORDER BY 销售合同表.签订时间 DESC)";
                    break;

                case 510: //编码进货合同
                    sqlComm.CommandText = "SELECT 采购合同表.ID, 采购合同表.供方单位ID, 采购合同表.合同编号 AS 单据编号, 单位表.单位编号, 单位表.单位名称, 采购合同表.签订时间 AS 日期, 采购合同表.金额, 单位表.开户银行, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID WHERE (采购合同表.BeActive = 1) AND (采购合同表.合同编号 LIKE '%" + strZJM + "%') AND (采购合同表.退货标记 = 0) AND (采购合同表.执行标记 = 0) ORDER BY 采购合同表.签订时间 DESC";
                    break;
                case 511:  //编码已进货，非退货进货合同
                    sqlComm.CommandText = "SELECT 采购合同表.ID, 采购合同表.供方单位ID, 采购合同表.合同编号, 单位表.单位编号, 单位表.单位名称, 采购合同表.签订时间 AS 日期, 采购合同表.金额, 单位表.开户银行, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID WHERE (采购合同表.BeActive = 1) AND (采购合同表.退货标记 = 0) AND (采购合同表.合同编号 LIKE '%" + strZJM + "%') AND (采购合同表.执行标记 = 1)  ORDER BY 采购合同表.签订时间 DESC";
                    break;
                case 512:  //编码销售合同
                    sqlComm.CommandText = "SELECT 销售合同表.ID, 销售合同表.供方单位ID, 销售合同表.合同编号 AS 单据编号, 单位表.单位编号, 单位表.单位名称, 销售合同表.签订时间 AS 日期, 销售合同表.金额, 单位表.开户银行, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID WHERE (销售合同表.BeActive = 1) AND (销售合同表.合同编号 LIKE '%" + strZJM + "%') AND (销售合同表.退货标记 = 0)  AND (销售合同表.执行标记 = 0)  ORDER BY 销售合同表.签订时间 DESC";
                    break;
                case 513:  //编码已出货，非退货销售合同
                    sqlComm.CommandText = "SELECT 销售合同表.ID, 销售合同表.供方单位ID, 销售合同表.合同编号 AS 单据编号, 单位表.单位编号, 单位表.单位名称, 销售合同表.签订时间 AS 日期, 销售合同表.金额, 单位表.开户银行, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID WHERE (销售合同表.BeActive = 1) AND (销售合同表.退货标记 = 0) AND (销售合同表.合同编号 LIKE '%" + strZJM + "%') AND (销售合同表.执行标记 = 1)  ORDER BY 销售合同表.签订时间 DESC";
                    break;
                case 514://编码所有合同
                    sqlComm.CommandText = "(SELECT 销售合同表.ID, 销售合同表.供方单位ID, 销售合同表.合同编号 AS 单据编号, 单位表.单位编号, 单位表.单位名称, 销售合同表.签订时间 AS 日期, 销售合同表.金额, 单位表.开户银行, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售合同表 INNER JOIN 单位表 ON 销售合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 销售合同表.业务员ID = 职员表.ID WHERE (销售合同表.BeActive = 1)  AND (销售合同表.合同编号 LIKE '%" + strZJM + "%')) UNION (SELECT 采购合同表.ID, 采购合同表.供方单位ID, 采购合同表.合同编号 AS 单据编号, 单位表.单位编号, 单位表.单位名称, 采购合同表.签订时间 AS 日期, 采购合同表.金额, 单位表.开户银行, 职员表.ID AS 职员ID, 职员表.职员姓名 AS 业务员 FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 ON 采购合同表.业务员ID = 职员表.ID WHERE (采购合同表.BeActive = 1) AND (采购合同表.合同编号 LIKE '%" + strZJM + "%')) ORDER BY 日期 DESC";
                    break;

                case 61://所有进货入库单
                    sqlComm.CommandText = "SELECT 进货入库汇总表.ID, 进货入库汇总表.单位ID, 进货入库汇总表.单据编号,单位表.单位编号, 单位表.单位名称, 进货入库汇总表.日期, 进货入库汇总表.价税合计 AS 金额, 进货入库汇总表.备注, 进货入库汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 进货入库汇总表 INNER JOIN 单位表 ON 进货入库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 进货入库汇总表.业务员ID = 职员表.ID WHERE (进货入库汇总表.BeActive = 1) ORDER BY 进货入库汇总表.日期 DESC";
                    break;

                case 80061://所有进货入库单
                    sqlComm.CommandText = "SELECT 进货入库汇总表.ID, 进货入库汇总表.单位ID, 进货入库汇总表.单据编号, 单位表.单位编号, 单位表.单位名称, 进货入库汇总表.日期, 进货入库汇总表.价税合计 AS 金额,  进货入库汇总表.备注, 进货入库汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID FROM 进货入库汇总表 INNER JOIN 单位表 ON 进货入库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 进货入库汇总表.业务员ID = 职员表.ID INNER JOIN (SELECT     单据ID, SUM(数量) AS 商品数量 FROM 进货入库明细表 GROUP BY 单据ID) AS B ON 进货入库汇总表.ID = B.单据ID LEFT OUTER JOIN (SELECT     COUNT(*) AS 条码数量, 单据编号 FROM 商品条码表 GROUP BY 单据编号) AS A ON 进货入库汇总表.单据编号 = A.单据编号 WHERE     (进货入库汇总表.BeActive = 1) AND (A.条码数量 IS NULL OR A.条码数量 < B.商品数量) ORDER BY 进货入库汇总表.日期 DESC";
                    break;
                case 611://编号所有进货入库单
                    sqlComm.CommandText = "SELECT 进货入库汇总表.ID, 进货入库汇总表.单位ID, 进货入库汇总表.单据编号,单位表.单位编号, 单位表.单位名称, 进货入库汇总表.日期, 进货入库汇总表.价税合计 AS 金额, 进货入库汇总表.备注, 进货入库汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 进货入库汇总表 INNER JOIN 单位表 ON 进货入库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 进货入库汇总表.业务员ID = 职员表.ID WHERE (进货入库汇总表.BeActive = 1) AND (进货入库汇总表.单据编号 LIKE '%" + strZJM + "%') ORDER BY 进货入库汇总表.日期 DESC";
                    break;
                case 62://所有退回单
                    sqlComm.CommandText = "SELECT 销售退出汇总表.ID, 销售退出汇总表.单位ID, 销售退出汇总表.单据编号,单位表.单位编号, 单位表.单位名称, 销售退出汇总表.日期, 销售退出汇总表.价税合计, 销售退出汇总表.备注, 销售退出汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售退出汇总表 INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售退出汇总表.业务员ID = 职员表.ID WHERE (销售退出汇总表.BeActive = 1) ORDER BY 销售退出汇总表.日期 DESC";
                    break;
                case 80062://所有退回单
                    sqlComm.CommandText = "SELECT 销售退出汇总表.ID, 销售退出汇总表.单位ID, 销售退出汇总表.单据编号, 单位表.单位编号, 单位表.单位名称, 销售退出汇总表.日期, 销售退出汇总表.价税合计, 销售退出汇总表.备注, 销售退出汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID FROM 销售退出汇总表 INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售退出汇总表.业务员ID = 职员表.ID INNER JOIN (SELECT     COUNT(*) AS 条码数量, 单据编号 FROM 商品条码表 GROUP BY 单据编号) AS A ON 销售退出汇总表.单据编号 = A.单据编号 LEFT OUTER JOIN (SELECT 单据ID, SUM(数量) AS 商品数量 FROM 销售退出明细表 GROUP BY 单据ID) AS B ON 销售退出汇总表.ID = B.单据ID WHERE (销售退出汇总表.BeActive = 1) AND (A.条码数量 IS NULL OR A.条码数量 < B.商品数量) ORDER BY 销售退出汇总表.日期 DESC";
                    break;
                case 621://编号所有退回单
                    sqlComm.CommandText = "SELECT 销售退出汇总表.ID, 销售退出汇总表.单位ID, 销售退出汇总表.单据编号,单位表.单位编号, 单位表.单位名称, 销售退出汇总表.日期, 销售退出汇总表.价税合计, 销售退出汇总表.备注, 销售退出汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售退出汇总表 INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售退出汇总表.业务员ID = 职员表.ID WHERE (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.单据编号 LIKE '%" + strZJM + "%') ORDER BY 销售退出汇总表.日期 DESC";
                    break;
                case 63://所有库存单
                    sqlComm.CommandText = "SELECT 库存盘点汇总表.ID, 库存盘点汇总表.库房ID, 库存盘点汇总表.单据编号, 库房表.库房编号, 库房表.库房名称, 库存盘点汇总表.盘点时间 AS 日期, 库存盘点汇总表.数量合计, 库存盘点汇总表.金额合计, 库存盘点汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 0  FROM 库存盘点汇总表 INNER JOIN 库房表 ON 库存盘点汇总表.库房ID = 库房表.ID INNER JOIN 职员表 ON 库存盘点汇总表.业务员ID = 职员表.ID WHERE (库存盘点汇总表.BeActive = 1) ORDER BY 库存盘点汇总表.盘点时间 DESC";
                    bDWMC = false;
                    break;
                case 80063://所有库存单
                    sqlComm.CommandText = "SELECT 库存盘点汇总表.ID, 库存盘点汇总表.库房ID, 库存盘点汇总表.单据编号, 库房表.库房编号, 库房表.库房名称, 库存盘点汇总表.盘点时间 AS 日期,  库存盘点汇总表.数量合计, 库存盘点汇总表.金额合计, 库存盘点汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 0 AS Expr1, B.商品数量, A.条码数量 FROM 库存盘点汇总表 INNER JOIN 库房表 ON 库存盘点汇总表.库房ID = 库房表.ID INNER JOIN 职员表 ON 库存盘点汇总表.业务员ID = 职员表.ID INNER JOIN (SELECT     单据ID, SUM(盘损数量) AS 商品数量 FROM 库存盘点明细表 GROUP BY 单据ID) AS B ON 库存盘点汇总表.ID = B.单据ID LEFT OUTER JOIN (SELECT COUNT(*) AS 条码数量, 单据编号 FROM 商品条码表 GROUP BY 单据编号) AS A ON 库存盘点汇总表.单据编号 = A.单据编号 WHERE (库存盘点汇总表.BeActive = 1) AND (A.条码数量 IS NULL OR A.条码数量 < B.商品数量) AND (B.商品数量 <> 0) ORDER BY 库存盘点汇总表.盘点时间 DESC";
                    bDWMC = false;
                    break;
                case 631://编号所有库存单
                    sqlComm.CommandText = "SELECT 库存盘点汇总表.ID, 库存盘点汇总表.库房ID, 库存盘点汇总表.单据编号, 库房表.库房编号, 库房表.库房名称, 库存盘点汇总表.盘点时间 AS 日期, 库存盘点汇总表.数量合计, 库存盘点汇总表.金额合计, 库存盘点汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 0  FROM 库存盘点汇总表 INNER JOIN 库房表 ON 库存盘点汇总表.库房ID = 库房表.ID INNER JOIN 职员表 ON 库存盘点汇总表.业务员ID = 职员表.ID WHERE (库存盘点汇总表.BeActive = 1) AND (库存盘点汇总表.单据编号 LIKE '%" + strZJM + "%') ORDER BY 库存盘点汇总表.盘点时间 DESC";
                    bDWMC = false;
                    break;
                case 64://所有报损单
                    sqlComm.CommandText = "SELECT 库存报损汇总表.ID, 库存报损汇总表.库房ID, 库存报损汇总表.单据编号, 库房表.库房编号, 库房表.库房名称, 库存报损汇总表.日期, 库存报损汇总表.报损数量合计, 库存报损汇总表.报损金额合计, 库存报损汇总表.业务员ID, 职员表.职员姓名, 0  FROM 库存报损汇总表 INNER JOIN 库房表 ON 库存报损汇总表.库房ID = 库房表.ID INNER JOIN 职员表 ON 库存报损汇总表.业务员ID = 职员表.ID WHERE (库存报损汇总表.BeActive = 1) ORDER BY 库存报损汇总表.日期 DESC";
                    bDWMC = false;
                    break;
                case 80064://
                    sqlComm.CommandText = "SELECT 库存报损汇总表.ID, 库存报损汇总表.库房ID, 库存报损汇总表.单据编号, 库房表.库房编号, 库房表.库房名称, 库存报损汇总表.日期, 库存报损汇总表.报损数量合计, 库存报损汇总表.报损金额合计, 库存报损汇总表.业务员ID, 职员表.职员姓名, 0 AS Expr1, B.商品数量, A.条码数量 FROM 库存报损汇总表 INNER JOIN 库房表 ON 库存报损汇总表.库房ID = 库房表.ID INNER JOIN 职员表 ON 库存报损汇总表.业务员ID = 职员表.ID INNER JOIN (SELECT     单据ID, SUM(报损数量) AS 商品数量 FROM 库存报损明细表 GROUP BY 单据ID) AS B ON 库存报损汇总表.ID = B.单据ID LEFT OUTER JOIN (SELECT COUNT(*) AS 条码数量, 单据编号 FROM 商品条码表 GROUP BY 单据编号) AS A ON 库存报损汇总表.单据编号 = A.单据编号 WHERE (库存报损汇总表.BeActive = 1) AND (A.条码数量 IS NULL OR A.条码数量 < B.商品数量) ORDER BY 库存报损汇总表.日期 DESC";
                    bDWMC = false;
                    break;

                case 641://编号所有报损单
                    sqlComm.CommandText = "SELECT 库存报损汇总表.ID, 库存报损汇总表.库房ID, 库存报损汇总表.单据编号, 库房表.库房编号, 库房表.库房名称, 库存报损汇总表.日期, 库存报损汇总表.报损数量合计, 库存报损汇总表.报损金额合计, 库存报损汇总表.业务员ID, 职员表.职员姓名, 0  FROM 库存报损汇总表 INNER JOIN 库房表 ON 库存报损汇总表.库房ID = 库房表.ID INNER JOIN 职员表 ON 库存报损汇总表.业务员ID = 职员表.ID WHERE (库存报损汇总表.BeActive = 1) AND (库存报损汇总表.单据编号 LIKE '%" + strZJM + "%')  ORDER BY 库存报损汇总表.日期 DESC";
                    bDWMC = false;
                    break;
                case 65://所有销售出库单
                    sqlComm.CommandText = "SELECT 销售出库汇总表.ID, 销售出库汇总表.单位ID, 销售出库汇总表.单据编号,单位表.单位编号, 单位表.单位名称, 销售出库汇总表.日期, 销售出库汇总表.价税合计 AS 金额, 销售出库汇总表.备注, 销售出库汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售出库汇总表 INNER JOIN 单位表 ON 销售出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售出库汇总表.业务员ID = 职员表.ID WHERE (销售出库汇总表.BeActive = 1) ORDER BY 销售出库汇总表.日期 DESC";
                    break;
                case 651://编号所有销售出库单
                    sqlComm.CommandText = "SELECT 销售出库汇总表.ID, 销售出库汇总表.单位ID, 销售出库汇总表.单据编号,单位表.单位编号, 单位表.单位名称, 销售出库汇总表.日期, 销售出库汇总表.价税合计 AS 金额, 销售出库汇总表.备注, 销售出库汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 销售出库汇总表 INNER JOIN 单位表 ON 销售出库汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售出库汇总表.业务员ID = 职员表.ID WHERE (销售出库汇总表.BeActive = 1)  AND (销售出库汇总表.单据编号 LIKE '%" + strZJM + "%') ORDER BY 销售出库汇总表.日期 DESC";
                    break;
                case 66://所有销售退回单
                    sqlComm.CommandText = "SELECT 进货退出汇总表.ID, 进货退出汇总表.单位ID, 进货退出汇总表.单据编号,单位表.单位编号, 单位表.单位名称, 进货退出汇总表.日期, 进货退出汇总表.价税合计, 进货退出汇总表.备注, 进货退出汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 进货退出汇总表 INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 进货退出汇总表.业务员ID = 职员表.ID WHERE (进货退出汇总表.BeActive = 1) ORDER BY 进货退出汇总表.日期 DESC";
                    break;
                case 80066://所有销售退回单
                    sqlComm.CommandText = "SELECT 进货退出汇总表.ID, 进货退出汇总表.单位ID, 进货退出汇总表.单据编号, 单位表.单位编号, 单位表.单位名称, 进货退出汇总表.日期, 进货退出汇总表.价税合计, 进货退出汇总表.备注, 进货退出汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID FROM 进货退出汇总表 INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 进货退出汇总表.业务员ID = 职员表.ID INNER JOIN (SELECT     COUNT(*) AS 条码数量, 单据编号 FROM 商品条码表 GROUP BY 单据编号) AS A ON 进货退出汇总表.单据编号 = A.单据编号 LEFT OUTER JOIN (SELECT     单据ID, SUM(数量) AS 商品数量 FROM 进货退出明细表 GROUP BY 单据ID) AS B ON 进货退出汇总表.ID = B.单据ID WHERE (进货退出汇总表.BeActive = 1) AND (A.条码数量 IS NULL OR A.条码数量 < B.商品数量) ORDER BY 进货退出汇总表.日期 DESC";
                    break;
                case 661://编号所有销售退回单
                    sqlComm.CommandText = "SELECT 进货退出汇总表.ID, 进货退出汇总表.单位ID, 进货退出汇总表.单据编号,单位表.单位编号, 单位表.单位名称, 进货退出汇总表.日期, 进货退出汇总表.价税合计, 进货退出汇总表.备注, 进货退出汇总表.业务员ID, 职员表.职员姓名 AS 业务员, 单位表.部门ID  FROM 进货退出汇总表 INNER JOIN 单位表 ON 进货退出汇总表.单位ID = 单位表.ID INNER JOIN 职员表 ON 进货退出汇总表.业务员ID = 职员表.ID WHERE (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.单据编号 LIKE '%" + strZJM + "%') ORDER BY 进货退出汇总表.日期 DESC";
                    break;

                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("单据表")) dSet.Tables.Remove("单据表");
            sqlDA.Fill(dSet, "单据表");

            if (dSet.Tables["单据表"].Rows.Count < 1) //没有单据
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["单据表"].Rows.Count == 1) //只有一个单据
            {
                iBillNumber = Int32.Parse(dSet.Tables["单据表"].Rows[0][0].ToString());
                strBillCode = dSet.Tables["单据表"].Rows[0][2].ToString();
                iBillCNumber = Int32.Parse(dSet.Tables["单据表"].Rows[0][1].ToString());
                strBillCCode = dSet.Tables["单据表"].Rows[0][3].ToString();
                strBillCName = dSet.Tables["单据表"].Rows[0][4].ToString();

                try
                {
                    iPeopleNumber = int.Parse(dSet.Tables["单据表"].Rows[0][8].ToString());
                }
                catch
                {
                    iPeopleNumber = 0;
                }
                sPeopleName = dSet.Tables["单据表"].Rows[0][9].ToString();

                try
                {
                    iBillBMID = int.Parse(dSet.Tables["单据表"].Rows[0][10].ToString());
                }
                catch
                {
                    iBillBMID = 0;
                }



                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //多个单据
            FormSelectBill frmSelectBill = new  FormSelectBill();
            frmSelectBill.strConn = strConn;
            frmSelectBill.strSelectText = sqlComm.CommandText;
            frmSelectBill.bShowDW = bDWMC;

            frmSelectBill.ShowDialog();

            if (frmSelectBill.iBillNumber == 0)
                return 0;
            else
            {
                iBillNumber = frmSelectBill.iBillNumber;
                strBillCode = frmSelectBill.strBillCode;
                iBillCNumber = frmSelectBill.iBillCNumber;
                strBillCCode = frmSelectBill.strBillCCode;
                strBillCName = frmSelectBill.strBillCName;
                iPeopleNumber = frmSelectBill.iPeopleNumber;
                sPeopleName = frmSelectBill.sPeopleName;
                iBillBMID = frmSelectBill.iBillBMID;
                sqlConn.Close();
                return 1;
            }

        }


        //计算库存成本价
        public decimal countKCCBJ(decimal dKCL, decimal dKCJE, decimal dKCLIN, decimal dKCJEIN)
        {
            decimal dTemp=0;

            dTemp = dKCL + dKCLIN;
            if (dTemp == 0)
            {
                if (dKCL == 0)
                    return 0;
                else
                    return dKCJE / dKCL;
            }

            //库存价
            return (dKCJE + dKCJEIN) / (dTemp);
        }

        //得到库存量
        public decimal dZKCL= 0;
        public decimal dKCL = 0;
        public decimal dKCJE = 0;
        public void getKCL(int intCommNumber,int intKFNumber)
        {

            dZBKL = 0; dKCL = 0;
            if (intCommNumber == 0)
                return;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 库存数量, 库存金额 FROM 库存表 WHERE (库房ID = " + intKFNumber.ToString() + ") AND (商品ID = " + intCommNumber.ToString() + ")";
            sqldr=sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dKCL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT 库存数量, 库存金额 FROM 商品表 WHERE (ID = " + intCommNumber.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dZKCL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
            }
            sqlConn.Close();
        }

        //private static int[] pyvalue = new int[]{-20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,-20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,-19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,-19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,-19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,-19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,-18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,-18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183,-18181,-18012,-17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,-17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,-17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,-16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,-16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,-16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,-15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,-15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,-15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,-15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,-14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,-14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,-14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,-14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,-14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,-13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,-13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,-13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,-13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,-12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,-12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,-11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,-11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,-10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,-10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254};
        //private static string[] pystr = new string[]{"a","ai","an","ang","ao","ba","bai","ban","bang","bao","bei","ben","beng","bi","bian","biao","bie","bin","bing","bo","bu","ca","cai","can","cang","cao","ce","ceng","cha","chai","chan","chang","chao","che","chen","cheng","chi","chong","chou","chu","chuai","chuan","chuang","chui","chun","chuo","ci","cong","cou","cu","cuan","cui","cun","cuo","da","dai","dan","dang","dao","de","deng","di","dian","diao","die","ding","diu","dong","dou","du","duan","dui","dun","duo","e","en","er","fa","fan","fang","fei","fen","feng","fo","fou","fu","ga","gai","gan","gang","gao","ge","gei","gen","geng","gong","gou","gu","gua","guai","guan","guang","gui","gun","guo","ha","hai","han","hang","hao","he","hei","hen","heng","hong","hou","hu","hua","huai","huan","huang","hui","hun","huo","ji","jia","jian","jiang","jiao","jie","jin","jing","jiong","jiu","ju","juan","jue","jun","ka","kai","kan","kang","kao","ke","ken","keng","kong","kou","ku","kua","kuai","kuan","kuang","kui","kun","kuo","la","lai","lan","lang","lao","le","lei","leng","li","lia","lian","liang","liao","lie","lin","ling","liu","long","lou","lu","lv","luan","lue","lun","luo","ma","mai","man","mang","mao","me","mei","men","meng","mi","mian","miao","mie","min","ming","miu","mo","mou","mu","na","nai","nan","nang","nao","ne","nei","nen","neng","ni","nian","niang","niao","nie","nin","ning","niu","nong","nu","nv","nuan","nue","nuo","o","ou","pa","pai","pan","pang","pao","pei","pen","peng","pi","pian","piao","pie","pin","ping","po","pu","qi","qia","qian","qiang","qiao","qie","qin","qing","qiong","qiu","qu","quan","que","qun","ran","rang","rao","re","ren","reng","ri","rong","rou","ru","ruan","rui","run","ruo","sa","sai","san","sang","sao","se","sen","seng","sha","shai","shan","shang","shao","she","shen","sheng","shi","shou","shu","shua","shuai","shuan","shuang","shui","shun","shuo","si","song","sou","su","suan","sui","sun","suo","ta","tai","tan","tang","tao","te","teng","ti","tian","tiao","tie","ting","tong","tou","tu","tuan","tui","tun","tuo","wa","wai","wan","wang","wei","wen","weng","wo","wu","xi","xia","xian","xiang","xiao","xie","xin","xing","xiong","xiu","xu","xuan","xue","xun","ya","yan","yang","yao","ye","yi","yin","ying","yo","yong","you","yu","yuan","yue","yun","za","zai","zan","zang","zao","ze","zei","zen","zeng","zha","zhai","zhan","zhang","zhao","zhe","zhen","zheng","zhi","zhong","zhou","zhu","zhua","zhuai","zhuan","zhuang","zhui","zhun","zhuo","zi","zong","zou","zu","zuan","zui","zun","zuo"};
        private static int[] pyvalue=new int[]{-20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,-20032,-20026, 
-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,-19756,-19751,-19746,-19741,-19739,-19728, 
-19725,-19715,-19540,-19531,-19525,-19515,-19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263, 
-19261,-19249,-19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,-19003,-18996, 
-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,-18731,-18722,-18710,-18697,-18696,-18526, 
-18518,-18501,-18490,-18478,-18463,-18448,-18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, 
-18181,-18012,-17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,-17733,-17730, 
-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,-17468,-17454,-17433,-17427,-17417,-17202, 
-17185,-16983,-16970,-16942,-16915,-16733,-16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459, 
-16452,-16448,-16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,-16212,-16205, 
-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,-15933,-15920,-15915,-15903,-15889,-15878, 
-15707,-15701,-15681,-15667,-15661,-15659,-15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416, 
-15408,-15394,-15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,-15149,-15144, 
-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,-14941,-14937,-14933,-14930,-14929,-14928, 
-14926,-14922,-14921,-14914,-14908,-14902,-14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668, 
-14663,-14654,-14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,-14170,-14159, 
-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,-14109,-14099,-14097,-14094,-14092,-14090, 
-14087,-14083,-13917,-13914,-13910,-13907,-13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658, 
-13611,-13601,-13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,-13340,-13329, 
-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,-13068,-13063,-13060,-12888,-12875,-12871, 
-12860,-12858,-12852,-12849,-12838,-12831,-12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346, 
-12320,-12300,-12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,-11781,-11604, 
-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,-11055,-11052,-11045,-11041,-11038,-11024, 
-11020,-11019,-11018,-11014,-10838,-10832,-10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331, 
-10329,-10328,-10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254}; 
private static string[] pystr=new string[]{"a","ai","an","ang","ao","ba","bai","ban","bang","bao","bei","ben","beng","bi","bian","biao", 
"bie","bin","bing","bo","bu","ca","cai","can","cang","cao","ce","ceng","cha","chai","chan","chang","chao","che","chen", 
"cheng","chi","chong","chou","chu","chuai","chuan","chuang","chui","chun","chuo","ci","cong","cou","cu","cuan","cui", 
"cun","cuo","da","dai","dan","dang","dao","de","deng","di","dian","diao","die","ding","diu","dong","dou","du","duan", 
"dui","dun","duo","e","en","er","fa","fan","fang","fei","fen","feng","fo","fou","fu","ga","gai","gan","gang","gao", 
"ge","gei","gen","geng","gong","gou","gu","gua","guai","guan","guang","gui","gun","guo","ha","hai","han","hang", 
"hao","he","hei","hen","heng","hong","hou","hu","hua","huai","huan","huang","hui","hun","huo","ji","jia","jian", 
"jiang","jiao","jie","jin","jing","jiong","jiu","ju","juan","jue","jun","ka","kai","kan","kang","kao","ke","ken", 
"keng","kong","kou","ku","kua","kuai","kuan","kuang","kui","kun","kuo","la","lai","lan","lang","lao","le","lei", 
"leng","li","lia","lian","liang","liao","lie","lin","ling","liu","long","lou","lu","lv","luan","lue","lun","luo", 
"ma","mai","man","mang","mao","me","mei","men","meng","mi","mian","miao","mie","min","ming","miu","mo","mou","mu", 
"na","nai","nan","nang","nao","ne","nei","nen","neng","ni","nian","niang","niao","nie","nin","ning","niu","nong", 
"nu","nv","nuan","nue","nuo","o","ou","pa","pai","pan","pang","pao","pei","pen","peng","pi","pian","piao","pie", 
"pin","ping","po","pu","qi","qia","qian","qiang","qiao","qie","qin","qing","qiong","qiu","qu","quan","que","qun", 
"ran","rang","rao","re","ren","reng","ri","rong","rou","ru","ruan","rui","run","ruo","sa","sai","san","sang", 
"sao","se","sen","seng","sha","shai","shan","shang","shao","she","shen","sheng","shi","shou","shu","shua", 
"shuai","shuan","shuang","shui","shun","shuo","si","song","sou","su","suan","sui","sun","suo","ta","tai", 
"tan","tang","tao","te","teng","ti","tian","tiao","tie","ting","tong","tou","tu","tuan","tui","tun","tuo", 
"wa","wai","wan","wang","wei","wen","weng","wo","wu","xi","xia","xian","xiang","xiao","xie","xin","xing", 
"xiong","xiu","xu","xuan","xue","xun","ya","yan","yang","yao","ye","yi","yin","ying","yo","yong","you", 
"yu","yuan","yue","yun","za","zai","zan","zang","zao","ze","zei","zen","zeng","zha","zhai","zhan","zhang", 
"zhao","zhe","zhen","zheng","zhi","zhong","zhou","zhu","zhua","zhuai","zhuan","zhuang","zhui","zhun","zhuo", 
"zi","zong","zou","zu","zuan","zui","zun","zuo"}; 

        //声母
        public string convertPYSM(string chrstr)
        {
            byte[] array = new byte[2];
            string returnstr = "";
            int chrasc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] nowchar = chrstr.ToCharArray();
            for (int j = 0; j < nowchar.Length; j++)
            {
                array = System.Text.Encoding.Default.GetBytes(nowchar[j].ToString());
                if (array.Length < 2) //单字节
                {
                    returnstr += nowchar[j].ToString();
                    continue;
                }

                i1 = (short)(array[0]);
                i2 = (short)(array[1]);
                chrasc = i1 * 256 + i2 - 65536;

                if (chrasc < -20319 || chrasc > -10247)
                { // 不知道的字符
                    if (chrasc == -4445) //睿
                    {
                        returnstr += "R";
                    }

                    continue;
                }

                if (chrasc > 0 && chrasc < 160)
                {
                    returnstr += nowchar[j];
                }
                else
                {
                        for (int i = (pyvalue.Length - 1); i >= 0; i--)
                        {
                            if (pyvalue[i] <= chrasc)
                            {
                                returnstr += pystr[i].Substring(0,1);
                                break;
                            }
                        }
                }
            }
            return returnstr.ToUpper();
        }

        //全部
        public string convertPY(string chrstr)
        {
            byte[] array = new byte[2];
            string returnstr = "";
            int chrasc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] nowchar = chrstr.ToCharArray();
            for (int j = 0; j < nowchar.Length; j++)
            {
                array = System.Text.Encoding.Default.GetBytes(nowchar[j].ToString());
                if (array.Length < 2) //单字节
                {
                    returnstr += nowchar[j].ToString();
                    continue;
                }
                i1 = (short)(array[0]);
                i2 = (short)(array[1]);
                chrasc = i1 * 256 + i2 - 65536;
                if (chrasc > 0 && chrasc < 160)
                {
                    returnstr += nowchar[j];
                }
                else
                {
                    for (int i = (pyvalue.Length - 1); i >= 0; i--)
                    {
                        if (pyvalue[i] <= chrasc)
                        {
                            returnstr += pystr[i];
                            break;
                        }
                    }
                }
            }
            return returnstr;
        }



    }
}
