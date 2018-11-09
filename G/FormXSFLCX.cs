using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormXSFLCX : Form
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

        private int iSupplyCompany = 0;
        private ClassGetInformation cGetInformation;
        private decimal[] cTemp = new decimal[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private decimal[] cTemp1 = new decimal[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private int intClassID = 0;

        public int LIMITACCESS = 18;

        public FormXSFLCX()
        {
            InitializeComponent();
        }

        private void FormXSFLCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            //得到开始时间
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 公司宣传, 质量目标1, 质量目标2, 质量目标3, 质量目标4, 管理员权限, 总经理权限, 职员权限, 经理权限, 业务员权限 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {

                try
                {
                    LIMITACCESS = int.Parse(sqldr.GetValue(6).ToString());
                }
                catch
                {
                    LIMITACCESS = 18;
                }
            }
            sqldr.Close();
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

            //初始化员工列表
            sqlComm.CommandText = "SELECT ID, 职员编号, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");

            if (dSet.Tables.Contains("职员表1")) dSet.Tables.Remove("职员表1");
            sqlDA.Fill(dSet, "职员表1");

            object[] OTemp = new object[3];
            OTemp[0] = 0;
            OTemp[1] = "全部";
            OTemp[2] = "全部";
            dSet.Tables["职员表"].Rows.Add(OTemp);

            object[] OTemp1 = new object[3];
            OTemp1[0] = 0;
            OTemp1[1] = "全部";
            OTemp1[2] = "全部";
            dSet.Tables["职员表1"].Rows.Add(OTemp);


            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.SelectedIndex = comboBoxYWY.Items.Count - 1;

            comboBoxCZY.DataSource = dSet.Tables["职员表1"];
            comboBoxCZY.DisplayMember = "职员姓名";
            comboBoxCZY.ValueMember = "ID";
            comboBoxCZY.SelectedIndex = comboBoxCZY.Items.Count - 1;
            sqlConn.Close();

            cGetInformation.getSystemDateTime();
            string sDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(sDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(2, "") == 0)
            {
                //return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
                checkBoxSYDW.Checked = false;
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(20, textBoxDWBH.Text.Trim()) == 0)
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
                    checkBoxSYDW.Checked = false;
                }
            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(22, textBoxDWMC.Text.Trim()) == 0)
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
                    checkBoxSYDW.Checked = false;
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i;
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                cGetInformation.getUnderClassInformation(intClassID);
            }
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, SUM(毛利视图.数量) AS 数量, SUM(毛利视图.金额) AS 金额, SUM(毛利视图.毛利) AS 毛利, 商品表.分类编号 FROM 毛利视图 INNER JOIN 商品表 ON 毛利视图.商品ID = 商品表.ID WHERE (毛利视图.BeActive = 1) AND (毛利视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (毛利视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (毛利视图.单位ID = " + iSupplyCompany.ToString() + ")";
            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber;i++)
                    sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }

            sqlComm.CommandText += " GROUP BY 商品表.分类编号, 商品表.商品编号, 商品表.商品名称, 毛利视图.毛利";



            if (dSet.Tables.Contains("商品表1")) dSet.Tables.Remove("商品表1");
            sqlDA.Fill(dSet, "商品表1");


            sqlComm.CommandText = "SELECT 单位表.单位编号, 单位表.单位名称, SUM(毛利视图.数量) AS 数量, SUM(毛利视图.金额) AS 金额, SUM(毛利视图.毛利) AS 毛利 FROM 毛利视图 INNER JOIN 单位表 ON 毛利视图.单位ID = 单位表.ID INNER JOIN 商品表 ON 毛利视图.商品ID = 商品表.ID WHERE (毛利视图.BeActive = 1) AND (毛利视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (毛利视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (毛利视图.单位ID = " + iSupplyCompany.ToString() + ")";
            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY 毛利视图.单位ID, 单位表.单位编号, 单位表.单位名称";

            if (dSet.Tables.Contains("商品表2")) dSet.Tables.Remove("商品表2");
            sqlDA.Fill(dSet, "商品表2");

            sqlComm.CommandText = "SELECT 商品表.商品编号, 商品表.商品名称, SUM(毛利视图.数量) AS 数量, SUM(毛利视图.金额) AS 金额, SUM(毛利视图.毛利) AS 毛利 FROM 毛利视图 INNER JOIN 商品表 ON 毛利视图.商品ID = 商品表.ID WHERE (毛利视图.BeActive = 1) AND (毛利视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (毛利视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (毛利视图.单位ID = " + iSupplyCompany.ToString() + ")";
            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY 毛利视图.商品ID, 商品表.商品编号, 商品表.商品名称";

            if (dSet.Tables.Contains("商品表3")) dSet.Tables.Remove("商品表3");
            sqlDA.Fill(dSet, "商品表3");

            sqlComm.CommandText = "SELECT 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称,SUM(毛利视图.数量) AS 数量, SUM(毛利视图.金额) AS 金额, SUM(毛利视图.毛利) AS 毛利 FROM 毛利视图 INNER JOIN 商品表 ON 毛利视图.商品ID = 商品表.ID INNER JOIN 单位表 ON 毛利视图.单位ID = 单位表.ID WHERE (毛利视图.BeActive = 1) AND (毛利视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (毛利视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (毛利视图.单位ID = " + iSupplyCompany.ToString() + ")";
            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY 毛利视图.商品ID, 商品表.商品编号, 商品表.商品名称, 单位表.单位编号, 单位表.单位名称, 毛利视图.单位ID ";

            if (dSet.Tables.Contains("商品表4")) dSet.Tables.Remove("商品表4");
            sqlDA.Fill(dSet, "商品表4");

            sqlComm.CommandText = "SELECT 职员表.职员姓名 AS 业务员, 商品表.商品编号, 商品表.商品名称, SUM(毛利视图.数量) AS 数量, SUM(毛利视图.金额) AS 金额, SUM(毛利视图.毛利) AS 毛利 FROM 毛利视图 INNER JOIN 商品表 ON 毛利视图.商品ID = 商品表.ID INNER JOIN 职员表 ON 毛利视图.业务员ID = 职员表.ID WHERE (毛利视图.BeActive = 1) AND (毛利视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (毛利视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";

            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND  (毛利视图.单位ID = " + iSupplyCompany.ToString() + ")";
            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY 毛利视图.商品ID, 商品表.商品编号, 商品表.商品名称, 毛利视图.业务员ID, 职员表.职员姓名 ";

            if (dSet.Tables.Contains("商品表5")) dSet.Tables.Remove("商品表5");
            sqlDA.Fill(dSet, "商品表5");

            sqlComm.CommandText = "SELECT 毛利视图.表单ID, 毛利视图.单据编号, 毛利视图.日期, 单位表.单位编号, 单位表.单位名称, 商品表.商品编号, 商品表.商品名称, N'', N'', 毛利视图.数量,毛利视图.金额, 毛利视图.毛利 FROM 毛利视图 INNER JOIN 商品表 ON 毛利视图.商品ID = 商品表.ID INNER JOIN 单位表 ON 单位表.ID = 毛利视图.单位ID WHERE (毛利视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (毛利视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (毛利视图.BeActive = 1) ";

            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if(iSupplyCompany!=0)
                sqlComm.CommandText += " AND (单位表.ID = " + iSupplyCompany.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }




            if (dSet.Tables.Contains("商品表6")) dSet.Tables.Remove("商品表6");
            sqlDA.Fill(dSet, "商品表6");

            sqlComm.CommandText = "SELECT 职员表.职员姓名 , A.售单数, SUM(毛利视图.数量) AS 数量,SUM(毛利视图.金额) AS 金额,SUM(毛利视图.毛利) AS 毛利 FROM 毛利视图 INNER JOIN 商品表 ON 毛利视图.商品ID = 商品表.ID INNER JOIN 职员表 ON 毛利视图.业务员ID = 职员表.ID INNER JOIN (SELECT 毛利视图.业务员ID, COUNT(*) AS 售单数 FROM 毛利视图 INNER JOIN 商品表 ON 毛利视图.商品ID = 商品表.ID WHERE (毛利视图.BeActive = 1) AND (毛利视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (毛利视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";

            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND (毛利视图.单位ID = " + iSupplyCompany.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }

            sqlComm.CommandText += " GROUP BY 业务员ID) A ON 毛利视图.业务员ID = A.业务员ID WHERE (毛利视图.BeActive = 1) AND (毛利视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (毛利视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND (毛利视图.单位ID = " + iSupplyCompany.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY 毛利视图.业务员ID, 职员表.职员姓名, A.售单数 ORDER BY 职员表.职员姓名 ";


            if (dSet.Tables.Contains("商品表7")) dSet.Tables.Remove("商品表7");
            sqlDA.Fill(dSet, "商品表7");


            sqlComm.CommandText = "SELECT 部门表.部门名称, A.售单数, SUM(毛利视图.数量) AS 数量,SUM(毛利视图.金额) AS 金额, SUM(毛利视图.毛利) AS 毛利 FROM 毛利视图 INNER JOIN 商品表 ON 毛利视图.商品ID = 商品表.ID INNER JOIN 部门表 ON 毛利视图.部门ID = 部门表.ID RIGHT OUTER JOIN (SELECT 毛利视图.部门ID, COUNT(*) AS 售单数 FROM 毛利视图 INNER JOIN 商品表 ON 毛利视图.商品ID = 商品表.ID WHERE (毛利视图.BeActive = 1) AND (毛利视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (毛利视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) ";

            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND (毛利视图.单位ID = " + iSupplyCompany.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }

            sqlComm.CommandText += " GROUP BY 毛利视图.部门ID) A ON 毛利视图.部门ID = A.部门ID WHERE (毛利视图.BeActive = 1) AND (毛利视图.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (毛利视图.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102))";

            if (Convert.ToInt32(comboBoxYWY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ")";
            if (Convert.ToInt32(comboBoxCZY.SelectedValue) != 0)
                sqlComm.CommandText += " AND (毛利视图.操作员ID = " + comboBoxCZY.SelectedValue.ToString() + ")";
            if (iSupplyCompany != 0)
                sqlComm.CommandText += " AND (毛利视图.单位ID = " + iSupplyCompany.ToString() + ")";
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }


            sqlComm.CommandText += " GROUP BY 毛利视图.部门ID, 部门表.部门名称, A.售单数 ORDER BY 部门表.部门名称 ";


            if (dSet.Tables.Contains("商品表8")) dSet.Tables.Remove("商品表8");
            sqlDA.Fill(dSet, "商品表8");


            sqlConn.Close();
            adjustDataView1();
            dataGridView2.DataSource = dSet.Tables["商品表2"];
            dataGridView2.Columns[2].DefaultCellStyle.Format = "f0"; 
            dataGridView3.DataSource = dSet.Tables["商品表3"];
            dataGridView3.Columns[2].DefaultCellStyle.Format = "f0"; 
            dataGridView4.DataSource = dSet.Tables["商品表4"];
            dataGridView4.Columns[4].DefaultCellStyle.Format = "f0"; 
            dataGridView5.DataSource = dSet.Tables["商品表5"];
            dataGridView5.Columns[3].DefaultCellStyle.Format = "f0"; 
            dataGridView6.DataSource = dSet.Tables["商品表6"];
            dataGridView6.Columns[9].DefaultCellStyle.Format = "f0"; 
            dataGridView6.Columns[0].Visible = false;
            dataGridView6.Columns[7].Visible = false;
            dataGridView6.Columns[8].Visible = false;
            dataGridView6.Columns[9].Visible = false;
            dataGridView7.DataSource = dSet.Tables["商品表7"];
            dataGridView7.Columns[1].DefaultCellStyle.Format = "f0";
            dataGridView7.Columns[2].DefaultCellStyle.Format = "f0";

            dataGridView8.DataSource = dSet.Tables["商品表8"];
            dataGridView8.Columns[1].DefaultCellStyle.Format = "f0";
            dataGridView8.Columns[2].DefaultCellStyle.Format = "f0";
 

            countfTemp();
            tabControl1_SelectedIndexChanged(null, null);

        }

        private void adjustDataView1()
        {
            int i;

            for (i = 0; i < dSet.Tables["商品表1"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表1"].Rows[i][2].ToString() == "")
                    dSet.Tables["商品表1"].Rows[i][2] = 0;
                if (dSet.Tables["商品表1"].Rows[i][3].ToString() == "")
                    dSet.Tables["商品表1"].Rows[i][3] = 0;
                if (dSet.Tables["商品表1"].Rows[i][5].ToString() == "")
                    dSet.Tables["商品表1"].Rows[i][5] = 0;

            }

            int j, k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[3];
            decimal[] dSum1 = new decimal[3];

            for (t = 0; t < dSum1.Length; t++)
            {
                dSum1[t] = 0;
            }


            sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM 商品分类表 ORDER BY 上级分类";
            if (dSet.Tables.Contains("商品分类表")) dSet.Tables.Remove("商品分类表");
            sqlDA.Fill(dSet, "商品分类表");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("分类编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("分类名称", System.Type.GetType("System.String"));
            dTable.Columns.Add("数量", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("金额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("毛利", System.Type.GetType("System.Decimal"));


            DataRow[] dtC = dSet.Tables["商品分类表"].Select("上级分类 = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[5];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];

                for (t = 2; t < oTemp.Length; t++)
                    oTemp[t] = 0;


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["商品分类表"].Select("上级分类 = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[5];
                    oTemp1[0] = dtC1[j][1];
                    oTemp1[1] = "　　" + dtC1[j][2];
                    //oTemp1[8] = dtC1[j][0];
                    //oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;
                    for (t = 2; t < oTemp1.Length; t++)
                        oTemp1[t] = 0;

                    dTable.Rows.Add(oTemp1);
                    iRow1 = dTable.Rows.Count - 1;

                    DataRow[] dtC2 = dSet.Tables["商品表1"].Select("分类编号 = " + dtC1[j][0]);

                    for (t = 0; t < dSum.Length; t++)
                        dSum[t] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {

                        for (t = 0; t < dSum.Length; t++)
                            dSum[t] += Convert.ToDecimal(dtC2[k][t + 2].ToString());

                    }

                    for (t = 2; t < dSum.Length + 2; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 2];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 2; t < dSum.Length + 2; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }

            object[] oTemp3 = new object[5];
            oTemp3[0] = "合计";
            oTemp3[1] = "";
            for (t = 2; t < oTemp3.Length; t++)
                oTemp3[t] = dSum1[t - 2];
            dTable.Rows.Add(oTemp3);


            dataGridView1.DataSource = dTable;
            dataGridView1.Columns[2].DefaultCellStyle.Format = "f0"; 


        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "销售商品分类汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "销售单位汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "销售商品汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, true, intUserLimit);
                    break;
                case 3:
                    strT = "销售单位商品汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, true, intUserLimit);
                    break;
                case 4:
                    strT = "销售业务员商品汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, true, intUserLimit);
                    break;
                case 5:
                    strT = "销售明细;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, true, intUserLimit);
                    break;
                case 6:
                    strT = "销售业务员汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView7, strT, true, intUserLimit);
                    break;
                case 7:
                    strT = "销售部门汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView7, strT, true, intUserLimit);
                    break;

            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "销售商品分类汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView1, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "销售单位汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView2, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "销售商品汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView3, strT, false, intUserLimit);
                    break;
                case 3:
                    strT = "销售单位商品汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView4, strT, false, intUserLimit);
                    break;
                case 4:
                    strT = "销售业务员商品汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView5, strT, false, intUserLimit);
                    break;
                case 5:
                    strT = "销售明细;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView6, strT, false, intUserLimit);
                    break;
                case 6:
                    strT = "销售业务员汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView7, strT, false, intUserLimit);
                    break;
                case 7:
                    strT = "销售部门汇总;" + toolStripStatusLabelC.Text + " 日期:" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridView7, strT, false, intUserLimit);
                    break;


            }
        }

        private void countfTemp()
        {
            int c = 0, c1 = 0;
            int i, j;

            for (i = 1; i <= 8; i++)
            {
                cTemp[i - 1] = 0;
                cTemp1[i - 1] = 0;
                switch (i)
                {
                    case 1:
                        c = 0;
                        c1 = 0;
                        break;
                    case 2:
                        c = 3;
                        c1 = 4;
                        break;
                    case 3:
                        c = 3;
                        c1 = 4;
                        break;
                    case 4:
                        c = 5;
                        c1 = 6;
                        break;
                    case 5:
                        c = 4;
                        c1 = 5;
                        break;
                    case 6:
                        c = 10;
                        c1 = 11;
                        break;
                    case 7:
                        c = 3;
                        c1 = 4;
                        break;
                    case 8:
                        c = 3;
                        c1 = 4;
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
                            cTemp[i - 1] += Convert.ToDecimal(dSet.Tables["商品表" + i.ToString()].Rows[j][c].ToString());
                            cTemp1[i - 1] += Convert.ToDecimal(dSet.Tables["商品表" + i.ToString()].Rows[j][c1].ToString());
                        }
                        catch
                        {
                        }
                    }
                }


            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c1 = tabControl1.SelectedIndex + 1;

            if (!dSet.Tables.Contains("商品表" + c1.ToString())) 
                return;


            if (c1 != 1)
                toolStripStatusLabelC.Text = "共有" + dSet.Tables["商品表" + c1.ToString()].Rows.Count.ToString() + "条记录 金额合计" + cTemp[tabControl1.SelectedIndex].ToString("f2") + "元 利润合计" + cTemp1[tabControl1.SelectedIndex].ToString("f2") + "元";
            else
                toolStripStatusLabelC.Text = "";
        }

        private void dataGridView6_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgV = (DataGridView)sender;
            if (dgV.RowCount < 1)
                return;

            if (dgV.SelectedRows.Count < 1)
                return;

            string sTemp = "", sTemp1 = "";
            sTemp = dgV.SelectedRows[0].Cells[1].Value.ToString().ToUpper();
            sTemp1 = dgV.SelectedRows[0].Cells[0].Value.ToString();

            switch (sTemp.Substring(0, 2))
            {
                case "CG":
                    // 创建此子窗体的一个新实例。
                    FormCGHT childFormCGHT = new FormCGHT();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormCGHT.MdiParent = this.MdiParent;

                    childFormCGHT.strConn = strConn;
                    childFormCGHT.iDJID = Convert.ToInt32(sTemp1);
                    childFormCGHT.isSaved = true;

                    childFormCGHT.intUserID = intUserID;
                    childFormCGHT.intUserLimit = intUserLimit;
                    childFormCGHT.strUserLimit = strUserLimit;
                    childFormCGHT.strUserName = strUserName;
                    childFormCGHT.Show();
                    break;
                case "XS":
                    // 创建此子窗体的一个新实例。
                    FormXSHT childFormXSHT = new FormXSHT();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSHT.MdiParent = this.MdiParent;

                    childFormXSHT.strConn = strConn;
                    childFormXSHT.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSHT.isSaved = true;

                    childFormXSHT.intUserID = intUserID;
                    childFormXSHT.intUserLimit = intUserLimit;
                    childFormXSHT.strUserLimit = strUserLimit;
                    childFormXSHT.strUserName = strUserName;
                    childFormXSHT.Show();
                    break;
            }

            switch (sTemp.Substring(0, 3))
            {
                case "AKP":
                    // 创建此子窗体的一个新实例。
                    FormGJSPZD childFormGJSPZD = new FormGJSPZD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormGJSPZD.MdiParent = this.MdiParent;

                    childFormGJSPZD.strConn = strConn;
                    childFormGJSPZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormGJSPZD.isSaved = true;

                    childFormGJSPZD.intUserID = intUserID;
                    childFormGJSPZD.intUserLimit = intUserLimit;
                    childFormGJSPZD.strUserLimit = strUserLimit;
                    childFormGJSPZD.strUserName = strUserName;
                    childFormGJSPZD.Show();
                    break;

                case "ADH":
                    // 创建此子窗体的一个新实例。
                    FormJHRKYHD childFormJHRKYHD = new FormJHRKYHD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormJHRKYHD.MdiParent = this.MdiParent;

                    childFormJHRKYHD.strConn = strConn;
                    childFormJHRKYHD.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHRKYHD.isSaved = true;

                    childFormJHRKYHD.intUserID = intUserID;
                    childFormJHRKYHD.intUserLimit = intUserLimit;
                    childFormJHRKYHD.strUserLimit = strUserLimit;
                    childFormJHRKYHD.strUserName = strUserName;
                    childFormJHRKYHD.Show();
                    break;

                case "ATH":
                    // 创建此子窗体的一个新实例。
                    FormJHTCZD childFormJHTCZD = new FormJHTCZD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormJHTCZD.MdiParent = this.MdiParent;

                    childFormJHTCZD.strConn = strConn;
                    childFormJHTCZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHTCZD.isSaved = true;

                    childFormJHTCZD.intUserID = intUserID;
                    childFormJHTCZD.intUserLimit = intUserLimit;
                    childFormJHTCZD.strUserLimit = strUserLimit;
                    childFormJHTCZD.strUserName = strUserName;
                    childFormJHTCZD.Show();
                    break;

                case "ATB":
                    // 创建此子窗体的一个新实例。
                    FormJHTBJDJ childFormJHTBJDJ = new FormJHTBJDJ();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormJHTBJDJ.MdiParent = this.MdiParent;

                    childFormJHTBJDJ.strConn = strConn;
                    childFormJHTBJDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHTBJDJ.isSaved = true;

                    childFormJHTBJDJ.intUserID = intUserID;
                    childFormJHTBJDJ.intUserLimit = intUserLimit;
                    childFormJHTBJDJ.strUserLimit = strUserLimit;
                    childFormJHTBJDJ.strUserName = strUserName;
                    childFormJHTBJDJ.Show();
                    break;

                case "AYF":
                    // 创建此子窗体的一个新实例。
                    FormYFZKJS childFormYFZKJS = new FormYFZKJS();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormYFZKJS.MdiParent = this.MdiParent;

                    childFormYFZKJS.strConn = strConn;
                    childFormYFZKJS.iDJID = Convert.ToInt32(sTemp1);
                    childFormYFZKJS.isSaved = true;

                    childFormYFZKJS.intUserID = intUserID;
                    childFormYFZKJS.intUserLimit = intUserLimit;
                    childFormYFZKJS.strUserLimit = strUserLimit;
                    childFormYFZKJS.strUserName = strUserName;
                    childFormYFZKJS.Show();
                    break;

                case "BKP":
                    // 创建此子窗体的一个新实例。
                    FormXSCKZD childFormXSCKZD = new FormXSCKZD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSCKZD.MdiParent = this.MdiParent;

                    childFormXSCKZD.strConn = strConn;
                    childFormXSCKZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSCKZD.isSaved = true;

                    childFormXSCKZD.intUserID = intUserID;
                    childFormXSCKZD.intUserLimit = intUserLimit;
                    childFormXSCKZD.strUserLimit = strUserLimit;
                    childFormXSCKZD.strUserName = strUserName;
                    childFormXSCKZD.Show();
                    break;

                case "BCK":
                    // 创建此子窗体的一个新实例。
                    FormXSCKJD childFormXSCKJD = new FormXSCKJD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSCKJD.MdiParent = this.MdiParent;

                    childFormXSCKJD.strConn = strConn;
                    childFormXSCKJD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSCKJD.isSaved = true;

                    childFormXSCKJD.intUserID = intUserID;
                    childFormXSCKJD.intUserLimit = intUserLimit;
                    childFormXSCKJD.strUserLimit = strUserLimit;
                    childFormXSCKJD.strUserName = strUserName;
                    childFormXSCKJD.Show();
                    break;

                case "BTH":
                    // 创建此子窗体的一个新实例。
                    FormXSTHZD childFormXSTHZD = new FormXSTHZD();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSTHZD.MdiParent = this.MdiParent;

                    childFormXSTHZD.strConn = strConn;
                    childFormXSTHZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSTHZD.isSaved = true;

                    childFormXSTHZD.intUserID = intUserID;
                    childFormXSTHZD.intUserLimit = intUserLimit;
                    childFormXSTHZD.strUserLimit = strUserLimit;
                    childFormXSTHZD.strUserName = strUserName;
                    childFormXSTHZD.Show();
                    break;

                case "BTB":
                    // 创建此子窗体的一个新实例。
                    FormXSTBJDJ childFormXSTBJDJ = new FormXSTBJDJ();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormXSTBJDJ.MdiParent = this.MdiParent;

                    childFormXSTBJDJ.strConn = strConn;
                    childFormXSTBJDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSTBJDJ.isSaved = true;

                    childFormXSTBJDJ.intUserID = intUserID;
                    childFormXSTBJDJ.intUserLimit = intUserLimit;
                    childFormXSTBJDJ.strUserLimit = strUserLimit;
                    childFormXSTBJDJ.strUserName = strUserName;
                    childFormXSTBJDJ.Show();
                    break;

                case "BYS":
                    // 创建此子窗体的一个新实例。
                    FormYSZKJS childFormYSZKJS = new FormYSZKJS();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormYSZKJS.MdiParent = this.MdiParent;

                    childFormYSZKJS.strConn = strConn;
                    childFormYSZKJS.iDJID = Convert.ToInt32(sTemp1);
                    childFormYSZKJS.isSaved = true;

                    childFormYSZKJS.intUserID = intUserID;
                    childFormYSZKJS.intUserLimit = intUserLimit;
                    childFormYSZKJS.strUserLimit = strUserLimit;
                    childFormYSZKJS.strUserName = strUserName;
                    childFormYSZKJS.Show();
                    break;

                case "CPD":
                    // 创建此子窗体的一个新实例。
                    FormKCSPPD2 childFormKCSPPD2 = new FormKCSPPD2();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormKCSPPD2.MdiParent = this.MdiParent;

                    childFormKCSPPD2.strConn = strConn;
                    childFormKCSPPD2.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCSPPD2.isSaved = true;

                    childFormKCSPPD2.intUserID = intUserID;
                    childFormKCSPPD2.intUserLimit = intUserLimit;
                    childFormKCSPPD2.strUserLimit = strUserLimit;
                    childFormKCSPPD2.strUserName = strUserName;
                    childFormKCSPPD2.Show();
                    break;

                case "CBS":
                    // 创建此子窗体的一个新实例。
                    FormKCSPBSCL childFormKCSPBSCL = new FormKCSPBSCL();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormKCSPBSCL.MdiParent = this.MdiParent;

                    childFormKCSPBSCL.strConn = strConn;
                    childFormKCSPBSCL.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCSPBSCL.isSaved = true;

                    childFormKCSPBSCL.intUserID = intUserID;
                    childFormKCSPBSCL.intUserLimit = intUserLimit;
                    childFormKCSPBSCL.strUserLimit = strUserLimit;
                    childFormKCSPBSCL.strUserName = strUserName;
                    childFormKCSPBSCL.Show();
                    break;

                case "CCK":
                    // 创建此子窗体的一个新实例。
                    FormKCJWCKDJ childFormKCJWCKDJ = new FormKCJWCKDJ();
                    // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
                    childFormKCJWCKDJ.MdiParent = this.MdiParent;

                    childFormKCJWCKDJ.strConn = strConn;
                    childFormKCJWCKDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCJWCKDJ.isSaved = true;

                    childFormKCJWCKDJ.intUserID = intUserID;
                    childFormKCJWCKDJ.intUserLimit = intUserLimit;
                    childFormKCJWCKDJ.strUserLimit = strUserLimit;
                    childFormKCJWCKDJ.strUserName = strUserName;
                    childFormKCJWCKDJ.Show();
                    break;
            }

        }

        private void checkBoxSYDW_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSYDW.Checked)
            {
                textBoxDWBH.Text = "";
                textBoxDWMC.Text = "";
                iSupplyCompany = 0;
            }
        }

        private void btnBY_Click(object sender, EventArgs e)
        {
            System.Globalization.GregorianCalendar cGregorianCalendar=new System.Globalization.GregorianCalendar();

            dateTimePickerS.Value = DateTime.Parse(System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-1 00:00:00");
            dateTimePickerE.Value = DateTime.Parse(System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + cGregorianCalendar.GetDaysInMonth(System.DateTime.Now.Year,System.DateTime.Now.Month).ToString()+" 23:59:59");

        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            DateTime dt = System.DateTime.Now.AddMonths(-1);

            System.Globalization.GregorianCalendar cGregorianCalendar = new System.Globalization.GregorianCalendar();

            dateTimePickerS.Value = DateTime.Parse(System.DateTime.Now.AddMonths(-1).Year.ToString() + "-" + System.DateTime.Now.AddMonths(-1).Month.ToString() + "-1 00:00:00");
            dateTimePickerE.Value = DateTime.Parse(System.DateTime.Now.AddMonths(-1).Year.ToString() + "-" + System.DateTime.Now.AddMonths(-1).Month.ToString() + "-" + cGregorianCalendar.GetDaysInMonth(System.DateTime.Now.AddMonths(-1).Year, System.DateTime.Now.AddMonths(-1).Month).ToString() + " 23:59:59");
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

 
    }
}