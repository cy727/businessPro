using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormFPKJ : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private int iSupplyCompany = 0;

        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public int iDJID = 0;

        public FormFPKJ()
        {
            InitializeComponent();
        }

        private void FormFPKJ_Load(object sender, EventArgs e)
        {
            int i;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            if (isSaved)
            {
                //dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                initDJ();
                return;
            }

            sqlConn.Open();

            //得到开始时间
            sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();

            //初始化单据列表
            /*
            sqlComm.CommandText = "SELECT 发票明细表.ID, 销售商品制单表.单据编号, 销售商品制单表.价税合计 AS 原开票总额, 发票明细表.发票总额, 发票明细表.发货方式, 发票明细表.单号, 发票明细表.单据ID, 发票明细表.备注1, 发票明细表.备注2 FROM 发票明细表 INNER JOIN 销售商品制单表 ON 发票明细表.单据ID = 销售商品制单表.ID WHERE (发票明细表.ID = 0)";

            if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
            sqlDA.Fill(dSet, "单据明细表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[6].Visible = false;
            dataGridViewDJMX.Columns[2].ReadOnly = true;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;
             */
            DataTable dTable = new DataTable();
            dTable.Columns.Add("单据ID", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("冲抵ID", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("单据编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("冲抵编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("原开票总额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("发票总额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("发货方式", System.Type.GetType("System.String"));
            dTable.Columns.Add("单号", System.Type.GetType("System.String"));
            dTable.Columns.Add("备注1", System.Type.GetType("System.String"));
            dTable.Columns.Add("备注2", System.Type.GetType("System.String"));
            dataGridViewDJMX.DataSource = dTable;

            sqlConn.Close();

            //initHTDefault();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            comboBoxStyle.SelectedIndex = 0;
            comboBoxGD.SelectedIndex = 0;

        }


        private void initDJ()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 发票汇总表.发票号, 发票汇总表.日期, 操作员.职员姓名, 发票汇总表.备注, 单位表.单位编号, 单位表.单位名称, 发票汇总表.发货方式, 发票汇总表.单号, 发票汇总表.原开票金额, 发票汇总表.发票总额, 发票汇总表.发票类型 FROM 发票汇总表 INNER JOIN 单位表 ON 发票汇总表.单位ID = 单位表.ID INNER JOIN 职员表 操作员 ON 发票汇总表.操作员ID = 操作员.ID WHERE (发票汇总表.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                textBoxFPH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                labelCZY.Text = sqldr.GetValue(2).ToString();
                textBoxBZ.Text = sqldr.GetValue(3).ToString();
                textBoxDWBH.Text = sqldr.GetValue(4).ToString();
                textBoxDWMC.Text = sqldr.GetValue(5).ToString();

                comboBoxFHFS.Text = sqldr.GetValue(6).ToString();
                textBoxDH.Text = sqldr.GetValue(7).ToString();
                comboBoxStyle.SelectedIndex = Convert.ToInt32(sqldr.GetValue(10).ToString());

                this.Text = "发票开具：" + textBoxFPH.Text;
            }
            sqldr.Close();

            //初始化明细列表
            comboBoxGD.SelectedIndex = 0;
            sqlComm.CommandText = "SELECT 单据ID, 冲抵ID, 单据编号, 冲抵编号, 原开票总额, 发票总额, 发货方式, 单号, 备注1, 备注2 FROM 发票明细表 WHERE (发票ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("单据明细表")) dSet.Tables.Remove("单据明细表");
            sqlDA.Fill(dSet, "单据明细表");
            dataGridViewDJMX.DataSource = dSet.Tables["单据明细表"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[1].Visible = false;
           
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;

            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;


            sqlConn.Close();
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

                if (dSet.Tables.Contains("按照单据勾兑"))
                    dSet.Tables.Remove("按照单据勾兑");
                if (dSet.Tables.Contains("按照单据明细勾兑"))
                    dSet.Tables.Remove("按照单据明细勾兑");
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1100, textBoxDWBH.Text.Trim()) == 0)
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
                    if (dSet.Tables.Contains("按照单据勾兑"))
                        dSet.Tables.Remove("按照单据勾兑");
                    if (dSet.Tables.Contains("按照单据明细勾兑"))
                        dSet.Tables.Remove("按照单据明细勾兑");
                }
            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1300, textBoxDWMC.Text.Trim()) == 0)
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
                    if (dSet.Tables.Contains("按照单据勾兑"))
                        dSet.Tables.Remove("按照单据勾兑");
                    if (dSet.Tables.Contains("按照单据明细勾兑"))
                        dSet.Tables.Remove("按照单据明细勾兑");
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i;

            if (iSupplyCompany == 0)
            {
                MessageBox.Show("请选择相应开票单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable dTable = new DataTable();
            dTable.Columns.Add("单据ID", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("冲抵ID", System.Type.GetType("System.Int32"));
            dTable.Columns.Add("单据编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("冲抵编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("原开票总额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("发票总额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("发货方式", System.Type.GetType("System.String"));
            dTable.Columns.Add("单号", System.Type.GetType("System.String"));
            dTable.Columns.Add("备注1", System.Type.GetType("System.String"));
            dTable.Columns.Add("备注2", System.Type.GetType("System.String"));


            switch (comboBoxStyle.SelectedIndex)
            {
                case 0: //购进

                    switch (comboBoxGD.SelectedIndex)
                    {
                        case 0: //按照单据

                            if (dSet.Tables.Contains("按照单据勾兑"))  //初始化单据勾兑列表
                                dSet.Tables.Remove("按照单据勾兑");
                            sqlConn.Open();
                            sqlComm.CommandText = "(SELECT 发票定义表.选择, 进货入库汇总表.单据编号,购进商品制单表.单据编号 AS 冲抵单号, 进货入库汇总表.价税合计, 进货入库汇总表.ID, 进货入库汇总表.购进ID,进货入库汇总表.价税合计 AS 开票总额, 进货入库汇总表.日期, '' AS 备注1, '' AS 备注2 FROM 进货入库汇总表 INNER JOIN 购进商品制单表 ON 进货入库汇总表.购进ID = 购进商品制单表.ID CROSS JOIN 发票定义表 WHERE (进货入库汇总表.BeActive = 1) AND (进货入库汇总表.日期 >= CONVERT(DATETIME,  '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (进货入库汇总表.单位ID = " + iSupplyCompany.ToString() + ") AND ((进货入库汇总表.发票号 IS NULL) OR (进货入库汇总表.发票号 = N''))) UNION (SELECT 发票定义表.选择, 进货退出汇总表.单据编号, 进货退出汇总表.单据编号 AS [AS 冲抵单号], -1*进货退出汇总表.价税合计, 进货退出汇总表.ID, 进货退出汇总表.ID AS Expr1, -1*进货退出汇总表.价税合计 AS 开票总额, 进货退出汇总表.日期, '' AS 备注1, '' AS 备注2  FROM 发票定义表 CROSS JOIN 进货退出汇总表 WHERE (进货退出汇总表.BeActive = 1) AND (进货退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + "  23:59:59', 102)) AND (进货退出汇总表.单位ID = " + iSupplyCompany.ToString() + ") AND (进货退出汇总表.发票号 IS NULL OR 进货退出汇总表.发票号 = N'')) UNION (SELECT 发票定义表.选择, 购进退补差价汇总表.单据编号, 购进退补差价汇总表.单据编号 AS [AS 冲抵单号], 购进退补差价汇总表.价税合计, 购进退补差价汇总表.ID, 购进退补差价汇总表.ID AS Expr1, 购进退补差价汇总表.价税合计 AS 开票总额, 购进退补差价汇总表.日期, '' AS 备注1, '' AS 备注2  FROM 发票定义表 CROSS JOIN 购进退补差价汇总表 WHERE (购进退补差价汇总表.BeActive = 1) AND (购进退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (购进退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + "  23:59:59', 102)) AND (购进退补差价汇总表.单位ID = " + iSupplyCompany.ToString() + ") AND (购进退补差价汇总表.发票号 IS NULL OR 购进退补差价汇总表.发票号 = N''))";
                            sqlDA.Fill(dSet, "按照单据勾兑");

                            sqlConn.Close();
                            

                            FormSelectGDFP frmSelectGDFP=new FormSelectGDFP();
                            frmSelectGDFP.iSelectStyle = 1;

                            frmSelectGDFP.dtSelect = dSet.Tables["按照单据勾兑"];
                            frmSelectGDFP.ShowDialog();

                            if (true)//frmSelectGDFP.dSUMJE >= 0)
                            {
                                /*
                                DataRow []dr=dSet.Tables["按照单据勾兑"].Select("选择=1");


                                for (i = 0; i < dr.Length; i++)
                                {
                                    object[] oTemp = new object[10];
                                    oTemp[0] = dr[i][4];
                                    oTemp[1] = dr[i][5];
                                    oTemp[2] = dr[i][1]; 
                                    oTemp[3] = dr[i][2]; 
                                    oTemp[4] = dr[i][3]; 
                                    oTemp[5] = dr[i][6]; 
                                    oTemp[6] = ""; 
                                    oTemp[7] = "";
                                    oTemp[8] = "";
                                    oTemp[9] = "";
                                    dTable.Rows.Add(oTemp);
                                }
                                */

                                for (i = 0; i < dSet.Tables["按照单据勾兑"].Rows.Count; i++)
                                {
                                    if (bool.Parse(dSet.Tables["按照单据勾兑"].Rows[i][0].ToString()))
                                    {
                                        object[] oTemp = new object[10];
                                        oTemp[0] = dSet.Tables["按照单据勾兑"].Rows[i][4];
                                        oTemp[1] = dSet.Tables["按照单据勾兑"].Rows[i][5];
                                        oTemp[2] = dSet.Tables["按照单据勾兑"].Rows[i][1];
                                        oTemp[3] = dSet.Tables["按照单据勾兑"].Rows[i][2];
                                        oTemp[4] = dSet.Tables["按照单据勾兑"].Rows[i][3];
                                        oTemp[5] = dSet.Tables["按照单据勾兑"].Rows[i][6];
                                        oTemp[6] = "";
                                        oTemp[7] = "";
                                        oTemp[8] = "";
                                        oTemp[9] = "";
                                        dTable.Rows.Add(oTemp);
                                    }

                                }
                                dataGridViewDJMX.DataSource = dTable;
                                
                                dataGridViewDJMX.Columns[0].Visible = false;
                                dataGridViewDJMX.Columns[1].Visible = false;
                                dataGridViewDJMX.Columns[6].Visible = false;
                                dataGridViewDJMX.Columns[7].Visible = false;
                                dataGridViewDJMX.Columns[2].Visible = true;
                                dataGridViewDJMX.Columns[3].Visible = true;
                                dataGridViewDJMX.Columns[4].Visible = true;
                                dataGridViewDJMX.Columns[5].Visible = true;
                                dataGridViewDJMX.Columns[8].Visible = true;
                                dataGridViewDJMX.Columns[9].Visible = true;
                                dataGridViewDJMX.Columns[2].ReadOnly = true;
                                dataGridViewDJMX.Columns[3].ReadOnly = true;
                                dataGridViewDJMX.Columns[4].ReadOnly = true;
                               // dataGridViewDJMX.Columns[5].ReadOnly = true;

                                dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.AllowUserToAddRows = false;
                                dataGridViewDJMX.AllowUserToDeleteRows = false;
                            }

                            break;
                        case 1://按照商品

                            if (dSet.Tables.Contains("按照单据明细勾兑"))  //初始化单据勾兑列表
                                dSet.Tables.Remove("按照单据明细勾兑");
                            sqlConn.Open();
                            sqlComm.CommandText = "SELECT 发票定义表.选择, 进货入库汇总表.单据编号, 购进商品制单表.单据编号 AS 冲抵单号, 商品表.商品名称, 商品表.商品编号, 进货入库明细表.实计金额, 进货入库汇总表.ID, 进货入库汇总表.购进ID, 进货入库明细表.实计金额 AS 开票总额, 进货入库明细表.商品ID, 进货入库汇总表.日期  FROM 进货入库汇总表 INNER JOIN 购进商品制单表 ON 进货入库汇总表.购进ID = 购进商品制单表.ID INNER JOIN 进货入库明细表 ON 进货入库汇总表.ID = 进货入库明细表.单据ID INNER JOIN 商品表 ON 进货入库明细表.商品ID = 商品表.ID CROSS JOIN 发票定义表 WHERE (进货入库汇总表.BeActive = 1) AND (进货入库汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.日期 <= CONVERT(DATETIME,   '" + dateTimePickerE.Value.ToShortDateString() + " 00:00:00', 102)) AND (进货入库汇总表.单位ID = " + iSupplyCompany.ToString() + ") AND ((进货入库汇总表.发票号 IS NULL) OR (进货入库汇总表.发票号 = N''))";
                            sqlDA.Fill(dSet, "按照单据明细勾兑");

                            sqlConn.Close();


                            FormSelectGDFP frmSelectGDFP1 = new FormSelectGDFP();
                            frmSelectGDFP1.iSelectStyle = 2;

                            frmSelectGDFP1.dtSelect = dSet.Tables["按照单据明细勾兑"];
                            frmSelectGDFP1.ShowDialog();

                            if (frmSelectGDFP1.dSUMJE >= 0)
                            {
                                DataView dt = new DataView(dSet.Tables["按照单据明细勾兑"]);
                                dt.RowFilter = "选择=1";
                                dataGridViewDJMX.DataSource = dt;
                                dataGridViewDJMX.Columns[0].Visible = false;
                                dataGridViewDJMX.Columns[6].Visible = false;
                                dataGridViewDJMX.Columns[7].Visible = false;
                                dataGridViewDJMX.Columns[9].Visible = false;
                                dataGridViewDJMX.Columns[1].ReadOnly = true;
                                dataGridViewDJMX.Columns[2].ReadOnly = true;
                                dataGridViewDJMX.Columns[3].ReadOnly = true;
                                dataGridViewDJMX.Columns[4].ReadOnly = true;
                                dataGridViewDJMX.Columns[5].ReadOnly = true;

                                dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.AllowUserToAddRows = false;
                                dataGridViewDJMX.AllowUserToDeleteRows = false;
                            }

                            break; 
                    }
                    break;

                case 1: //销售

                    switch (comboBoxGD.SelectedIndex)
                    {
                        case 0: //按照单据
                            if (dSet.Tables.Contains("按照单据勾兑"))  //初始化单据勾兑列表
                                dSet.Tables.Remove("按照单据勾兑");
                            sqlConn.Open();
                            sqlComm.CommandText = "(SELECT 发票定义表.选择, 销售商品制单表.单据编号, 销售商品制单表.价税合计, 销售商品制单表.价税合计 AS 开票总额, 销售商品制单表.运输方式, 销售商品制单表.单号, 销售商品制单表.ID,销售商品制单表.日期, '' AS 备注1, '' AS 备注2   FROM 销售商品制单表 CROSS JOIN 发票定义表 WHERE (销售商品制单表.发票号 IS NULL) AND (销售商品制单表.单位ID = " + iSupplyCompany.ToString() + ") AND  (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND  (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + "  23:59:59', 102)) AND  (销售商品制单表.BeActive = 1) AND ((销售商品制单表.发票号 IS NULL) OR (销售商品制单表.发票号 = N''))) UNION (SELECT 发票定义表.选择, 销售退出汇总表.单据编号, -1*销售退出汇总表.价税合计, -1*销售退出汇总表.价税合计 AS 开票总额, '' AS 运输方式, '' AS 单号, 销售退出汇总表.ID, 销售退出汇总表.日期, '' AS 备注1, '' AS 备注2  FROM 发票定义表 CROSS JOIN 销售退出汇总表 WHERE (销售退出汇总表.BeActive = 1) AND (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退出汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + "  23:59:59', 102)) AND (销售退出汇总表.单位ID = " + iSupplyCompany.ToString() + ") AND (销售退出汇总表.发票号 IS NULL OR 销售退出汇总表.发票号 = N'')) UNION (SELECT 发票定义表.选择, 销售退补差价汇总表.单据编号, 销售退补差价汇总表.价税合计, 销售退补差价汇总表.价税合计 AS 开票总额, '' AS 运输方式, '' AS 单号, 销售退补差价汇总表.ID, 销售退补差价汇总表.日期, '' AS 备注1, '' AS 备注2  FROM 发票定义表 CROSS JOIN 销售退补差价汇总表 WHERE (销售退补差价汇总表.BeActive = 1) AND (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND (销售退补差价汇总表.单位ID = " + iSupplyCompany.ToString() + ") AND (销售退补差价汇总表.发票号 IS NULL OR 销售退补差价汇总表.发票号 = N'')) ";
                            sqlDA.Fill(dSet, "按照单据勾兑");

                            sqlConn.Close();


                            FormSelectGDFP frmSelectGDFP3 = new FormSelectGDFP();
                            frmSelectGDFP3.iSelectStyle = 3;

                            frmSelectGDFP3.dtSelect = dSet.Tables["按照单据勾兑"];
                            frmSelectGDFP3.ShowDialog();

                            if (true)//frmSelectGDFP3.dSUMJE >= 0)
                            {
                                /*
                                DataRow[] dr = dSet.Tables["按照单据勾兑"].Select("选择=1");


                                for (i = 0; i < dr.Length; i++)
                                {
                                    object[] oTemp = new object[10];
                                    oTemp[0] = dr[i][6];
                                    oTemp[1] = dr[i][6];
                                    oTemp[2] = dr[i][1];
                                    oTemp[3] = dr[i][1];
                                    oTemp[4] = dr[i][2];
                                    oTemp[5] = dr[i][2];
                                    oTemp[6] = dr[i][4];
                                    oTemp[7] = dr[i][5];
                                    oTemp[8] = "";
                                    oTemp[9] = "";
                                    dTable.Rows.Add(oTemp);
                                }
                                 * */

                                for (i = 0; i < dSet.Tables["按照单据勾兑"].Rows.Count; i++)
                                {
                                    if (bool.Parse(dSet.Tables["按照单据勾兑"].Rows[i][0].ToString()))
                                    {
                                        object[] oTemp = new object[10];
                                        oTemp[0] = dSet.Tables["按照单据勾兑"].Rows[i][6];
                                        oTemp[1] = dSet.Tables["按照单据勾兑"].Rows[i][6];
                                        oTemp[2] = dSet.Tables["按照单据勾兑"].Rows[i][1];
                                        oTemp[3] = dSet.Tables["按照单据勾兑"].Rows[i][1];
                                        oTemp[4] = dSet.Tables["按照单据勾兑"].Rows[i][2];
                                        oTemp[5] = dSet.Tables["按照单据勾兑"].Rows[i][2];
                                        oTemp[6] = dSet.Tables["按照单据勾兑"].Rows[i][4];
                                        oTemp[7] = dSet.Tables["按照单据勾兑"].Rows[i][5];
                                        oTemp[8] = "";
                                        oTemp[9] = "";
                                        dTable.Rows.Add(oTemp);
                                    }

                                }

                                dataGridViewDJMX.DataSource = dTable;

                                dataGridViewDJMX.Columns[0].Visible = false;
                                dataGridViewDJMX.Columns[1].Visible = false;
                                dataGridViewDJMX.Columns[3].Visible = false;
                                dataGridViewDJMX.Columns[2].Visible = true;
                                dataGridViewDJMX.Columns[4].Visible = true;
                                dataGridViewDJMX.Columns[5].Visible = true;
                                dataGridViewDJMX.Columns[6].Visible = true;
                                dataGridViewDJMX.Columns[7].Visible = true;
                                dataGridViewDJMX.Columns[8].Visible = true;
                                dataGridViewDJMX.Columns[9].Visible = true;
                                //dataGridViewDJMX.Columns[7].Visible = false;
                                dataGridViewDJMX.Columns[2].ReadOnly = true;
                                dataGridViewDJMX.Columns[3].ReadOnly = true;
                                dataGridViewDJMX.Columns[4].ReadOnly = true;
                                //dataGridViewDJMX.Columns[5].ReadOnly = true;

                                dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.AllowUserToAddRows = false;
                                dataGridViewDJMX.AllowUserToDeleteRows = false;

                            }

                            break;

                        case 1: //按照商品

                            if (dSet.Tables.Contains("按照单据明细勾兑"))  //初始化单据勾兑列表
                                dSet.Tables.Remove("按照单据明细勾兑");
                            sqlConn.Open();
                            sqlComm.CommandText = "SELECT 发票定义表.选择, 销售商品制单表.单据编号, 商品表.商品名称, 商品表.商品编号, 销售商品制单明细表.实计金额, 销售商品制单明细表.实计金额 AS 开票金额, 销售商品制单表.运输方式, 销售商品制单表.单号, 销售商品制单表.ID, 销售商品制单明细表.商品ID, 销售商品制单表.日期 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID CROSS JOIN 发票定义表 WHERE (销售商品制单表.发票号 IS NULL) AND (销售商品制单表.单位ID = " + iSupplyCompany.ToString() + ") AND (销售商品制单表.日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (销售商品制单表.日期 <= CONVERT(DATETIME, '" + dateTimePickerE.Value.ToShortDateString() + " 23:59:59', 102)) AND ((销售商品制单表.BeActive = 1) OR (销售商品制单表.发票号 <> N''))";
                            sqlDA.Fill(dSet, "按照单据明细勾兑");

                            sqlConn.Close();


                            FormSelectGDFP frmSelectGDFP4 = new FormSelectGDFP();
                            frmSelectGDFP4.iSelectStyle = 4;

                            frmSelectGDFP4.dtSelect = dSet.Tables["按照单据明细勾兑"];
                            frmSelectGDFP4.ShowDialog();

                            if (frmSelectGDFP4.dSUMJE != 0)
                            {
                                DataView dt = new DataView(dSet.Tables["按照单据明细勾兑"]);
                                dt.RowFilter = "选择=1";
                                dataGridViewDJMX.DataSource = dt;
                                dataGridViewDJMX.Columns[0].Visible = false;
                                dataGridViewDJMX.Columns[8].Visible = false;
                                dataGridViewDJMX.Columns[9].Visible = false;

                                dataGridViewDJMX.Columns[1].ReadOnly = true;
                                dataGridViewDJMX.Columns[2].ReadOnly = true;
                                dataGridViewDJMX.Columns[3].ReadOnly = true;
                                dataGridViewDJMX.Columns[4].ReadOnly = true;
                                dataGridViewDJMX.Columns[6].ReadOnly = true;
                                dataGridViewDJMX.Columns[7].ReadOnly = true;

                                dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewDJMX.AllowUserToAddRows = false;
                                dataGridViewDJMX.AllowUserToDeleteRows = false;
                            }
                            break; 
                    }

                    break;

            }
            countAmount();
        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //return true 正确  false 错误
        private bool countAmount()
        {
            decimal fSum = 0, fSum1 = 0;

            bool bCheck = true;

            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                switch (comboBoxStyle.SelectedIndex)
                {
                    case 0: //销售

                        switch (comboBoxGD.SelectedIndex)
                        {
                            case 0: //按照单据
                                if (dataGridViewDJMX.Rows[i].Cells[5].Value.ToString()=="")
                                {
                                    dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                                }

                                /*
                                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value) < 0)
                                {
                                    dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "请输入大于0的数字";
                                    bCheck = false;

                                }
                                else
                                {
                                 */
                                    fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);
                                    fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                                //}

                                break;

                            case 1://按照商品

                                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "")
                                {
                                    dataGridViewDJMX.Rows[i].Cells[8].Value = 0;
                                }

                                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value) < 0)
                                {
                                    dataGridViewDJMX.Rows[i].Cells[8].Value = 0;
                                    dataGridViewDJMX.Rows[i].Cells[8].ErrorText = "请输入大于0的数字";
                                    bCheck = false;

                                }
                                else
                                {
                                    fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                                    fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                                }
                                break;
                        }
                        break;

                    case 1:

                        switch (comboBoxGD.SelectedIndex)
                        {
                            case 0: //按照单据
                                if (dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() == "")
                                {
                                    dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                                }

                                /*
                                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value) < 0)
                                {
                                    dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "请输入大于0的数字";
                                    bCheck = false;

                                }
                                else
                                {
                                 */
                                    fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);
                                    fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                                //}
                                break;

                            case 1: //按照商品

                                if (dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() == "")
                                {
                                    dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                                }

                                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value) <= 0)
                                {
                                    dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "请输入大于0的数字";
                                    bCheck = false;

                                }
                                else
                                {
                                    fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);
                                    fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                                }

                                break;

                        }

                        break;

                }
            }
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.Rows.Count.ToString();
            labelJEHJ.Text = fSum.ToString();
            labelSJJE.Text = fSum1.ToString();
            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);


            return bCheck;


        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            string strDT;
            string sTemp;

            cGetInformation.getSystemDateTime();
            //strDT = cGetInformation.strSYSDATATIME;
            strDT = dateTimePickerKPRQ.Value.ToShortDateString();

            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("发票已经开具，发票号为：" + textBoxFPH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (textBoxFPH.Text.Trim() == "")
            {
                MessageBox.Show("请输入发票号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if (iSupplyCompany == 0)
            {
                MessageBox.Show("请选择单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!countAmount())
            {
                MessageBox.Show("发票明细错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (dataGridViewDJMX.RowCount<1)
            {
                MessageBox.Show("没有选择开票项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("请检查发票内容，是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;


            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();

            //发票号查重
            try
            {
                sTemp = textBoxFPH.Text.Trim().Substring(0, 3);
            }
            catch
            {
                sTemp = "";
            }

            if (textBoxFPH.Text.Trim() != "现金不开票" && sTemp != "不开票")
            {
                sqlComm.CommandText = "SELECT 发票汇总表.ID, 发票汇总表.发票号, 单位表.单位名称, 发票汇总表.发票总额, 发票汇总表.日期 FROM 发票汇总表 INNER JOIN 单位表 ON 发票汇总表.单位ID = 单位表.ID WHERE (发票汇总表.发票号 = N'" + textBoxFPH.Text + "') AND (发票汇总表.BeActive <> 0)";
                sqldr = sqlComm.ExecuteReader();

                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    MessageBox.Show("发票号重复：" + sqldr.GetValue(2).ToString() + "(" + sqldr.GetValue(4).ToString() + " ￥" + sqldr.GetValue(3).ToString() + ")", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    sqlConn.Close();
                    return;
                }
                sqldr.Close();
            }
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                //表单汇总
                sqlComm.CommandText = "INSERT INTO 发票汇总表 (发票号, 单位ID, 备注, 发货方式, 单号, 操作员ID, 原开票金额, 发票总额, BeActive, 发票类型, 日期) VALUES (N'" + textBoxFPH.Text + "', " + iSupplyCompany.ToString() + ", N'" + textBoxBZ.Text + "', N'" + comboBoxFHFS.Text + "', N'" + textBoxDH.Text + "', " + intUserID.ToString() + ", " + labelJEHJ.Text + ", " + labelSJJE.Text + ", 1, " + comboBoxStyle.SelectedIndex.ToString() + ", '" + strDT + "')";
                sqlComm.ExecuteNonQuery();

                //取得单据号 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();

                //明细
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    switch (comboBoxStyle.SelectedIndex)
                    {
                        case 0: //购进

                            switch (comboBoxGD.SelectedIndex)
                            {
                                case 0: //按照单据
                                    sqlComm.CommandText = "INSERT INTO 发票明细表 (发票ID, 单据ID, 冲抵ID, 单据编号, 冲抵编号, 原开票总额, 发票总额, 发货方式, 单号, 备注1, 备注2) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[1].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[2].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[3].Value.ToString() + "', " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + "')";
                                    sqlComm.ExecuteNonQuery();

                                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString().Substring(0, 3);
                                    switch (sTemp)
                                    {
                                        case "ADH":

                                            sqlComm.CommandText = "UPDATE 进货入库汇总表 SET 发票号 = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();

                                            sqlComm.CommandText = "UPDATE 购进商品制单表 SET 发票号 = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[1].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();
                                            break;
                                        case "ATH":

                                            sqlComm.CommandText = "UPDATE 进货退出汇总表 SET 发票号 = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();
                                            break;
                                        case "ATB":

                                            sqlComm.CommandText = "UPDATE 购进退补差价汇总表 SET 发票号 = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();
                                            break;

                                        default:
                                            break;
                                    }


                                    break;
                                case 1://按照商品
                                    sqlComm.CommandText = "INSERT INTO 发票明细表 (发票ID, 冲抵ID, 单据ID, 原开票总额, 发票总额, 发货方式, 单号, 商品ID, 单据编号) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", N'', N''," + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ",N'" + dataGridViewDJMX.Rows[i].Cells[1].Value.ToString() + "')";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE 进货入库汇总表 SET 发票号 = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE 购进商品制单表 SET 发票号 = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();


                                    break;
                            }
                            break;

                        case 1: //销售

                            switch (comboBoxGD.SelectedIndex)
                            {
                                case 0: //按照单据
                                    sqlComm.CommandText = "INSERT INTO 发票明细表 (发票ID, 单据ID, 冲抵ID, 单据编号, 冲抵编号, 原开票总额, 发票总额, 发货方式, 单号, 备注1, 备注2) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[1].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[2].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[3].Value.ToString() + "', " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + "')";
                                    sqlComm.ExecuteNonQuery();

                                    sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString().Substring(0, 3);
                                    switch (sTemp)
                                    {
                                        case "BKP":

                                            sqlComm.CommandText = "UPDATE 销售商品制单表 SET 发票号 = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();
                                            break;
                                        case "BTH":

                                            sqlComm.CommandText = "UPDATE 销售退出汇总表 SET 发票号 = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();
                                            break;
                                        case "BTB":

                                            sqlComm.CommandText = "UPDATE 销售退补差价汇总表 SET 发票号 = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                            sqlComm.ExecuteNonQuery();
                                            break;

                                        default:
                                            break;
                                    }
                                    break;
                                case 1://按照商品
                                    sqlComm.CommandText = "INSERT INTO 发票明细表 (发票ID, 冲抵ID, 单据ID, 原开票总额, 发票总额, 发货方式, 单号, 商品ID, 单据编号) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[4].Cells[2].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() + ", N'" + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + "', N'" + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + "'," + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ",N'" + dataGridViewDJMX.Rows[i].Cells[1].Value.ToString() + "')";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE 销售商品制单表 SET 发票号 = N'" + textBoxFPH.Text + "' WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                            }
                            break;

                    }

                }



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

            bool bClose = false;
            if (MessageBox.Show("发票开具成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                bClose = true;
            }

            //MessageBox.Show("发票开具成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isSaved = true;
            textBoxFPH.Enabled = false;
            if (MessageBox.Show("是否继续发票开具？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                MDIBusiness mdiT = (MDIBusiness)this.MdiParent;
                mdiT.开具发票AToolStripMenuItem_Click(null, null);
            }


            if (bClose)
                this.Close();
        }

        private void FormFPKJ_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
                return;

            DialogResult dr = MessageBox.Show(this, "单据修改尚未保存，确定要退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("发票开具明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "发票开具(发票号:" + textBoxFPH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" +  labelCZY.Text + ";单位名称：" + textBoxDWMC.Text + ";开票金额：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("发票开具明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "发票开具(发票号:" + textBoxFPH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + labelCZY.Text + ";单位名称：" + textBoxDWMC.Text + ";开票金额：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }



    }
}