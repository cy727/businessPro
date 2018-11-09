﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Microsoft.Office.Interop.Word;

namespace business
{
    public partial class FormCGHT_EDIT : Form
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

        public bool isSaved = false;
        public int iDJID = 0;

        private int iNumRow = 0;

        private int RowPos;              // Position of currently printing row 
        private bool NewPage;            // Indicates if a new page reached
        private int PageNo;              // Number of pages to print

        public int iStyle = 0;

        private DateTime dtStart = Convert.ToDateTime("1999-1-1");

        private string sGSMC = "";
        private string sGSDZ = "";
        private string sGSDH = "";
        private string sGSCZ = "";
        private string sGSYB = "";
        private string sGSZH = "";
        private string sGSKHYH = "";
        private string sGSSH = "";
        private string sK = "", sM1 = "", sM2 = "", sM3 = "", sM4 = "";
        
        public FormCGHT_EDIT()
        {
            InitializeComponent();
        }

        private void FormCGHT_EDIT_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Left = 1;
            this.Top = 1;


            sqlConn.Open();
            sqlComm.CommandText = "SELECT 公司宣传, 质量目标1, 质量目标2, 质量目标3, 质量目标4, 管理员权限, 总经理权限, 职员权限, 经理权限, 业务员权限 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                sK = sqldr.GetValue(0).ToString();
                sM1 = sqldr.GetValue(1).ToString();
                sM2 = sqldr.GetValue(2).ToString();
                sM3 = sqldr.GetValue(3).ToString();
                sM4 = sqldr.GetValue(4).ToString();
            }
            sqldr.Close();

            //初始化员工列表
            sqlComm.CommandText = "SELECT ID, 职员编号, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";

            //得到开始时间
            sqlComm.CommandText = "SELECT 公司名, 地址, 电话, 传真, 税号, 开户银行, 帐号, 邮政编码, 开始时间, 负责人 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                sGSMC = sqldr.GetValue(0).ToString();
                sGSDZ = sqldr.GetValue(1).ToString();
                sGSDH = sqldr.GetValue(2).ToString();
                sGSCZ = sqldr.GetValue(3).ToString();
                sGSYB = sqldr.GetValue(7).ToString();
                sGSZH = sqldr.GetValue(6).ToString();
                sGSKHYH = sqldr.GetValue(5).ToString();
                sGSSH = sqldr.GetValue(4).ToString();
                dtStart = Convert.ToDateTime(sqldr.GetValue(8).ToString());
                textBoxFZR1.Text = sqldr.GetValue(9).ToString();
            }
            sqldr.Close();
            sqlConn.Close();

            switch (iStyle)
            {
                case 0: //冲红
                    this.Text += ":单据冲红";
                    break;

                case 1: //修改
                    this.Text += ":单据修改";
                    break;
            }

            if (iDJID!=0)
            {
                initDJ();
                return;
            }

            initHTDefault();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
        }

        private void initDJ()
        {

            dataGridViewCGHT.CellValidating -= dataGridViewCGHT_CellValidating;
            dataGridViewCGHT.RowValidating -= dataGridViewCGHT_RowValidating;
            dataGridViewCGHT.CellDoubleClick -= dataGridViewCGHT_CellDoubleClick;
            comboBoxYWY.SelectedIndexChanged -= comboBoxYWY_SelectedIndexChanged;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 单位表.单位编号, 单位表.单位名称, 业务员.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 采购合同表.签订地点, 采购合同表.签订时间, 采购合同表.质量要求, 采购合同表.异议期限, 采购合同表.质量负责, 采购合同表.随机备品, 采购合同表.运输方式, 采购合同表.运输至, 采购合同表.费用承担, 采购合同表.交货地点, 采购合同表.现货交货时间, 采购合同表.现货付款方式, 采购合同表.期货交货时间, 采购合同表.预付金额, 采购合同表.提货应付余额, 采购合同表.期货付款方式, 采购合同表.违约责任, 采购合同表.仲裁委员会, 采购合同表.其他约定事项, 采购合同表.合同有效期, 采购合同表.合同编号, 单位表.ID, 采购合同表.供方单位名称, 采购合同表.供方税号, 采购合同表.供方电话, 采购合同表.供方开户银行, 采购合同表.供方银行账号, 采购合同表.供方联系人, 采购合同表.供方地址, 采购合同表.供方传真, 采购合同表.供方邮编, 采购合同表.需方单位名称, 采购合同表.需方税号, 采购合同表.需方电话, 采购合同表.需方开户银行, 采购合同表.需方银行账号, 采购合同表.需方联系人, 采购合同表.需方地址, 采购合同表.需方传真, 采购合同表.需方邮编 FROM 采购合同表 INNER JOIN 单位表 ON 采购合同表.供方单位ID = 单位表.ID INNER JOIN 职员表 业务员 ON 采购合同表.业务员ID = 业务员.ID INNER JOIN 职员表 操作员 ON 采购合同表.操作员ID = 操作员.ID WHERE (采购合同表.ID = " + iDJID.ToString() + ") AND (采购合同表.BeActive<>0)";
            sqldr = sqlComm.ExecuteReader();

            if (!sqldr.HasRows)
            {
                isSaved = true;
                sqldr.Close();
                sqlConn.Close();
                return;
            }

            while (sqldr.Read())
            {
                textBoxDWBH.Text = sqldr.GetValue(0).ToString();
                textBoxDWMC.Text = sqldr.GetValue(1).ToString();
                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxQDDD.Text = sqldr.GetValue(4).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(5).ToString()).ToString("yyyy年M月dd日");
                textBoxZLYQ.Text = sqldr.GetValue(6).ToString();
                textBoxYYQX.Text = sqldr.GetValue(7).ToString();
                textBoxFZTJ.Text = sqldr.GetValue(8).ToString();
                textBoxSJBP.Text = sqldr.GetValue(9).ToString();
                comboBoxYSFS.Text = sqldr.GetValue(10).ToString();
                textBoxYSZ.Text = sqldr.GetValue(11).ToString();
                comboBoxFYCD.Text = sqldr.GetValue(12).ToString();
                textBoxJHDD.Text = sqldr.GetValue(13).ToString();
                textBoxXHJHSJ.Text = sqldr.GetValue(14).ToString();
                textBoxXHFKFS.Text = sqldr.GetValue(15).ToString();
                textBoxQHJHSJ.Text = sqldr.GetValue(16).ToString();
                textBoxYFJE.Text = sqldr.GetValue(17).ToString();
                textBoxTHYFYE.Text = sqldr.GetValue(18).ToString();
                textBoxQHFKFS.Text = sqldr.GetValue(19).ToString();
                textBoxWYZR.Text = sqldr.GetValue(20).ToString();
                textBoxZCWYH.Text = sqldr.GetValue(21).ToString();
                textBoxQTYDSX.Text = sqldr.GetValue(22).ToString();
                textBoxHTYXQ.Text = sqldr.GetValue(23).ToString();
                labelHTH.Text = sqldr.GetValue(24).ToString();

                iSupplyCompany = Convert.ToInt32(sqldr.GetValue(25).ToString());

                textBoxSH.Text = sqldr.GetValue(27).ToString();
                textBoxGSDH.Text = sqldr.GetValue(28).ToString();
                textBoxKHYH.Text = sqldr.GetValue(29).ToString();
                textBoxZH.Text = sqldr.GetValue(30).ToString();
                textBoxFZR.Text = sqldr.GetValue(31).ToString();
                textBoxGSDZ.Text = sqldr.GetValue(32).ToString();
                textBoxGSCZ.Text = sqldr.GetValue(33).ToString();
                textBoxYZBM.Text = sqldr.GetValue(34).ToString();

                sGSMC = sqldr.GetValue(35).ToString();
                sGSSH = sqldr.GetValue(36).ToString();
                sGSDH = sqldr.GetValue(37).ToString();
                sGSKHYH = sqldr.GetValue(38).ToString();
                sGSZH = sqldr.GetValue(39).ToString();
                textBoxFZR1.Text = sqldr.GetValue(40).ToString();
                sGSDZ = sqldr.GetValue(41).ToString();
                sGSCZ = sqldr.GetValue(42).ToString();
                sGSYB = sqldr.GetValue(43).ToString();


                this.Text = "采购合同：" + labelHTH.Text;
            }
            sqldr.Close();

            //初始化商品列表
            sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品名称, 商品表.商品编号, 采购合同明细表.数量, 采购合同明细表.单价, 采购合同明细表.总价, 采购合同明细表.备注, 商品表.商品规格, 商品表.最小计量单位 AS 单位, 商品表.最终进价 FROM 采购合同明细表 INNER JOIN 商品表 ON 采购合同明细表.商品ID = 商品表.ID WHERE (采购合同明细表.采购合同ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewCGHT.DataSource = dSet.Tables["商品表"];
            dataGridViewCGHT.Columns[0].Visible = false;
            dataGridViewCGHT.Columns[5].ReadOnly = true;
            dataGridViewCGHT.Columns[7].ReadOnly = true;
            dataGridViewCGHT.Columns[8].ReadOnly = true;
            dataGridViewCGHT.Columns[9].ReadOnly = true;
            dataGridViewCGHT.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCGHT.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCGHT.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCGHT.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCGHT.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCGHT.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCGHT.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCGHT.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCGHT.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridViewCGHT.Columns[3].DefaultCellStyle.Format = "f0";
            dataGridViewCGHT.Columns[4].DefaultCellStyle.Format = "f2";
            dataGridViewCGHT.Columns[5].DefaultCellStyle.Format = "f2";

            dataGridViewCGHT.ShowCellErrors = true;
            iNumRow = dSet.Tables["商品表"].Rows.Count;

            sqlConn.Close();
            //getCompanyInfoDetail();
            countAmount1();

            dataGridViewCGHT.CellValidating += dataGridViewCGHT_CellValidating;
            dataGridViewCGHT.RowValidating += dataGridViewCGHT_RowValidating;
            dataGridViewCGHT.CellDoubleClick += dataGridViewCGHT_CellDoubleClick;
            comboBoxYWY.SelectedIndexChanged += comboBoxYWY_SelectedIndexChanged;
        }
        private void countAmount1()
        {
            decimal fSum = 0;
            decimal fTemp, fTemp1;
            decimal fCount = 0;


            this.dataGridViewCGHT.CellValidating -= dataGridViewCGHT_CellValidating;
            for (int i = 0; i < dataGridViewCGHT.Rows.Count; i++)
            {
                if (dataGridViewCGHT.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewCGHT.Rows[i].Cells[3].Value.ToString() == "")
                    fTemp = 0;
                else
                    fTemp = Convert.ToDecimal(dataGridViewCGHT.Rows[i].Cells[3].Value);

                if (dataGridViewCGHT.Rows[i].Cells[4].Value.ToString() == "")
                    fTemp1 = 0;
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewCGHT.Rows[i].Cells[4].Value);

                if (Convert.ToDecimal(dataGridViewCGHT.Rows[i].Cells[4].Value) > Convert.ToDecimal(dataGridViewCGHT.Rows[i].Cells[9].Value))
                    dataGridViewCGHT.Rows[i].Cells[4].Style.BackColor = Color.LightPink;
                else
                    dataGridViewCGHT.Rows[i].Cells[4].Style.BackColor = Color.White;


                dataGridViewCGHT.Rows[i].Cells[5].Value = Math.Round(fTemp * fTemp1, 2);

                fCount += 1;
                fSum += Convert.ToDecimal(dataGridViewCGHT.Rows[i].Cells[5].Value);
            }

            labelJEHJ.Text = fSum.ToString("f2");
            labelHTMXJL.Text = fCount.ToString();
            labelDX.Text = cGetInformation.changeDAXIE(labelJEHJ.Text);


        }

        private void initHTDefault()
        {
            textBoxZLYQ.Text = "原厂标准";
            textBoxYYQX.Text = "收到货后，壹周内";
            textBoxFZTJ.Text = "壹年";
            textBoxSJBP.Text = "随机提供";
            textBoxJHDD.Text = "北京";
            textBoxZCWYH.Text = "北京";
            textBoxQDDD.Text = "北京";
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1, "") == 0)
            {
                return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
            }
            getCompanyInfoDetail();
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(10, textBoxDWMC.Text.Trim()) == 0)
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
                }
                getCompanyInfoDetail();
            }
        }

        private void getCompanyInfoDetail()
        {
            if (iSupplyCompany == 0)
            {
                textBoxDWBH.Text = "";
                textBoxDWMC.Text = "";
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 单位编号, 单位名称, 税号, 电话, 开户银行, 银行账号, 联系人, 地址, 传真, 邮编, 联系地址 FROM 单位表 WHERE (ID = " + iSupplyCompany.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                textBoxDWBH.Text = sqldr.GetValue(0).ToString();
                textBoxDWMC.Text = sqldr.GetValue(1).ToString();
                textBoxFZR.Text = sqldr.GetValue(6).ToString();
                textBoxYZBM.Text = sqldr.GetValue(9).ToString();
                textBoxGSDZ.Text = sqldr.GetValue(10).ToString();
                textBoxGSDH.Text = sqldr.GetValue(3).ToString();
                textBoxGSCZ.Text = sqldr.GetValue(8).ToString();
                textBoxSH.Text = sqldr.GetValue(2).ToString();
                textBoxKHYH.Text = sqldr.GetValue(4).ToString();
                textBoxZH.Text = sqldr.GetValue(5).ToString();
            }
            sqlConn.Close();
        }

        private void dataGridViewCGHT_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2) //商品编号
            {
                if (cGetInformation.getCommInformation(1, "") == 0) //失败
                {
                    return;
                }
                else
                {
                    this.dataGridViewCGHT.CellValidating -= dataGridViewCGHT_CellValidating;

                    ClearDataGridViewErrorText(dataGridViewCGHT);
                    dataGridViewCGHT.Rows[e.RowIndex].Cells[0].Value = cGetInformation.iCommNumber;
                    dataGridViewCGHT.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewCGHT.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewCGHT.Rows[e.RowIndex].Cells[7].Value = cGetInformation.strCommGG;
                    dataGridViewCGHT.Rows[e.RowIndex].Cells[8].Value = cGetInformation.strCommCount;
                    dataGridViewCGHT.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommZZJJ.ToString();
                    dataGridViewCGHT.Rows[e.RowIndex].Cells[4].Value = Math.Round(Decimal.Zero, 2);
                    dataGridViewCGHT.EndEdit();
                    dataGridViewCGHT.CurrentCell = dataGridViewCGHT.Rows[e.RowIndex].Cells[3];
                    dataGridViewCGHT.BeginEdit(false);
                    this.dataGridViewCGHT.CellValidating += dataGridViewCGHT_CellValidating;

                }
            }
        }

        private void dataGridViewCGHT_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int iRe = 0;

            if (dataGridViewCGHT.Rows[e.RowIndex].IsNewRow)
                return;

            ClearDataGridViewErrorText(dataGridViewCGHT);

            switch (e.ColumnIndex)
            {
                case 2: //商品编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewCGHT.CellValidating -= dataGridViewCGHT_CellValidating;

                        dataGridViewCGHT.Rows[e.RowIndex].Cells[0].Value = 0;
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[7].Value = "";
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[8].Value = "";
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[4].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[9].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewCGHT.CellValidating += dataGridViewCGHT_CellValidating;
                        break;

                    }
                    iRe = cGetInformation.getCommInformation(20, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewCGHT.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //失败
                    //if (cGetInformation.getCommInformation(20, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewCGHT.CancelEdit();
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[2].ErrorText = "商品编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewCGHT.CellValidating -= dataGridViewCGHT_CellValidating;

                        if (dataGridViewCGHT.Rows[e.RowIndex].Cells[0].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewCGHT.CellValidating += dataGridViewCGHT_CellValidating;
                            break;
                        }

                        dataGridViewCGHT.Rows[e.RowIndex].Cells[0].Value = cGetInformation.iCommNumber;
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[7].Value = cGetInformation.strCommGG;
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[8].Value = cGetInformation.strCommCount;
                        //dataGridViewCGHT.Rows[e.RowIndex].Cells[4].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommZZJJ.ToString();
                        this.dataGridViewCGHT.CellValidating += dataGridViewCGHT_CellValidating;

                    }
                    break;

                case 1: //商品名称
                    //if (e.RowIndex < iNumRow) break;
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewCGHT.CellValidating -= dataGridViewCGHT_CellValidating;

                        dataGridViewCGHT.Rows[e.RowIndex].Cells[0].Value = 0;
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[7].Value = "";
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[8].Value = "";
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[4].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[9].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewCGHT.CellValidating += dataGridViewCGHT_CellValidating;
                        break;

                    }

                    iRe = cGetInformation.getCommInformation(10, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewCGHT.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //失败
                    //if (cGetInformation.getCommInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[1].ErrorText = "商品助记码输入错误";
                    }
                    else
                    {
                        this.dataGridViewCGHT.CellValidating -= dataGridViewCGHT_CellValidating;

                        if (dataGridViewCGHT.Rows[e.RowIndex].Cells[0].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewCGHT.CellValidating += dataGridViewCGHT_CellValidating;
                            break;
                        }
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[0].Value = cGetInformation.iCommNumber;
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[7].Value = cGetInformation.strCommGG;
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[8].Value = cGetInformation.strCommCount;
                        //dataGridViewCGHT.Rows[e.RowIndex].Cells[4].Value = Math.Round(Decimal.Zero, 2);
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommZZJJ.ToString();
                        this.dataGridViewCGHT.CellValidating += dataGridViewCGHT_CellValidating;
                    }
                    break;

                case 3:  //商品数量
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut <= 0)
                        {
                            dataGridViewCGHT.Rows[e.RowIndex].Cells[3].ErrorText = "商品数量输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[3].ErrorText = "商品数量输入类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 4: //商品价格
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;
                    if (dataGridViewCGHT.Rows[e.RowIndex].Cells[9].Value.ToString() == "")
                    {
                        MessageBox.Show("请先输入合同商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.dataGridViewCGHT.CellValidating -= dataGridViewCGHT_CellValidating;
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[4].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewCGHT.CellValidating += dataGridViewCGHT_CellValidating;
                        break;
                    }

                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        if (detOut >= 0)
                        {
                            detOut = Math.Round(detOut, 2);

                            if (detOut.CompareTo(dataGridViewCGHT.Rows[e.RowIndex].Cells[9].Value) > 0)
                            {
                                if (MessageBox.Show("商品价格高于最终进价，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                                    e.Cancel = true;
                                else
                                {
                                    this.dataGridViewCGHT.CellValidating -= dataGridViewCGHT_CellValidating;
                                    dataGridViewCGHT.Rows[e.RowIndex].Cells[4].Value = detOut;
                                    this.dataGridViewCGHT.CellValidating += dataGridViewCGHT_CellValidating;
                                }

                            }
                        }
                        else
                        {
                            dataGridViewCGHT.Rows[e.RowIndex].Cells[4].ErrorText = "商品价格输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[4].ErrorText = "商品数量价格类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 6:
                    if (e.FormattedValue.ToString().Length > 100)
                    {
                        dataGridViewCGHT.Rows[e.RowIndex].Cells[6].ErrorText = "备注过多";
                        e.Cancel = true;
                    }
                    break;
                default:
                    break;



            }
            dataGridViewCGHT.EndEdit();

        }


        private bool countAmount()
        {
            decimal fSum = 0;
            decimal fTemp, fTemp1;
            decimal fCount = 0;
            bool bCheck = true;

            this.dataGridViewCGHT.CellValidating -= dataGridViewCGHT_CellValidating;
            for (int i = 0; i < dataGridViewCGHT.Rows.Count; i++)
            {
                if (dataGridViewCGHT.Rows[i].IsNewRow)
                    continue;

                ClearDataGridViewErrorText(dataGridViewCGHT);
                if (dataGridViewCGHT.Rows[i].Cells[1].Value.ToString() == "")
                {
                    dataGridViewCGHT.Rows[i].Cells[1].ErrorText = "输入所售商品";
                    bCheck = false;
                }

                if (dataGridViewCGHT.Rows[i].Cells[2].Value.ToString() == "")
                {
                    dataGridViewCGHT.Rows[i].Cells[2].ErrorText = "输入所售商品";
                    bCheck = false;
                }

                if (dataGridViewCGHT.Rows[i].Cells[3].Value.ToString() == "")
                {
                    dataGridViewCGHT.Rows[i].Cells[3].ErrorText = "输入所售商品数量";
                    bCheck = false;
                }
                if (dataGridViewCGHT.Rows[i].Cells[4].Value.ToString() == "")
                {
                    dataGridViewCGHT.Rows[i].Cells[4].ErrorText = "输入所售商品价格";
                    bCheck = false;
                }

                if (!bCheck)
                    continue;

                if (dataGridViewCGHT.Rows[i].Cells[3].Value.ToString() == "")
                    fTemp = 0;
                else
                    fTemp = Convert.ToDecimal(dataGridViewCGHT.Rows[i].Cells[3].Value);

                if (dataGridViewCGHT.Rows[i].Cells[4].Value.ToString() == "")
                    fTemp1 = 0;
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewCGHT.Rows[i].Cells[4].Value);

                if (Convert.ToDecimal(dataGridViewCGHT.Rows[i].Cells[4].Value) > Convert.ToDecimal(dataGridViewCGHT.Rows[i].Cells[9].Value))
                    dataGridViewCGHT.Rows[i].Cells[4].Style.BackColor = Color.LightPink;
                else
                    dataGridViewCGHT.Rows[i].Cells[4].Style.BackColor = Color.White;


                dataGridViewCGHT.Rows[i].Cells[5].Value = Math.Round(fTemp * fTemp1, 2);

                fCount += 1;
                fSum += Convert.ToDecimal(dataGridViewCGHT.Rows[i].Cells[5].Value);
            }
            this.dataGridViewCGHT.CellValidating += dataGridViewCGHT_CellValidating;
            dataGridViewCGHT.EndEdit();

            labelJEHJ.Text = fSum.ToString();
            labelHTMXJL.Text = fCount.ToString();
            labelDX.Text = cGetInformation.changeDAXIE(labelJEHJ.Text);

            return bCheck;

        }

        private void ClearDataGridViewErrorText(DataGridView dvIn)
        {
            for (int i = 0; i < dvIn.ColumnCount; i++)
            {
                dataGridViewCGHT.Rows[dvIn.CurrentCell.RowIndex].Cells[i].ErrorText = String.Empty;
            }
        }

        private void dataGridViewCGHT_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            //return EnterToTab(ref   msg, keyData, true);

            Control ctr = (Control)Control.FromHandle(msg.HWnd);

            if (ctr != null)
            {
                if (ctr.GetType() == typeof(System.Windows.Forms.DataGridViewTextBoxEditingControl))
                {
                    DataGridViewTextBoxEditingControl dvTextBoxEC = (DataGridViewTextBoxEditingControl)FromHandle(msg.HWnd);
                    DataGridView dv = (DataGridView)dvTextBoxEC.EditingControlDataGridView;
                    if (dv.Columns.Count > 0)
                    {
                        if (keyData == Keys.Enter)
                        {
                            try
                            {
                                dv.EndEdit();
                                switch (dv.CurrentCell.ColumnIndex)
                                {
                                    case 1:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[3];
                                        break;
                                    case 2:
                                    case 3:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[dv.CurrentCell.ColumnIndex + 1];
                                        break;
                                    case 4:
                                    //    dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[6];
                                    //    break;
                                    //case 6:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[1];
                                        break;
                                    default:
                                        break;
                                }
                                dv.BeginEdit(true);
                            }
                            catch (Exception)
                            {
                            }
                            return true;
                        }
                        if (keyData == Keys.F6)
                        {
                            //dv.Rows.Insert(dv.CurrentCell.RowIndex+1, 1);
                            DataRow drTemp = dSet.Tables["商品表"].NewRow();
                            dSet.Tables["商品表"].Rows.InsertAt(drTemp, dv.CurrentCell.RowIndex + 1);
                            return true;
                        }

                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dataGridViewCGHT_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(11, textBoxDWBH.Text.Trim()) == 0)
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
                }
                getCompanyInfoDetail();
            }
        }

        private void FormCGHT_EDIT_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
                return;

            DialogResult dr = MessageBox.Show(this, "单据尚未保存，确定要退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;
            int i;

            dataGridViewCGHT.EndEdit();

            string strDateSYS;
            cGetInformation.getSystemDateTime();
            strDateSYS = cGetInformation.strSYSDATATIME;


            switch (iStyle)
            {
                case 0://冲红
                    //保存完毕
                    if (isSaved)
                    {
                        MessageBox.Show("购进合同已经冲红,单据号为：" + labelHTH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    //格式确认
                    if (!countAmount())
                    {
                        MessageBox.Show("购进合同明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        //表单汇总
                        sqlComm.CommandText = "UPDATE 采购合同表 SET BeActive = 0 WHERE (ID = " + iDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //表单汇总
                        sqlComm.CommandText = "UPDATE 采购合同表 SET 冲红时间 = '" + strDateSYS + "' WHERE (ID = " + iDJID.ToString() + ")";
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

                    //MessageBox.Show("购进合同冲红成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isSaved = true;

                    if (MessageBox.Show("购进合同冲红成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        this.Close();
                    }
                    break;

                case 1://修改

                    //保存完毕
                    if (isSaved)
                    {
                        MessageBox.Show("合同已经修改,合同号为：" + labelHTH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (iSupplyCompany == 0)
                    {
                        MessageBox.Show("请选择合同单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //格式确认
                    if (!countAmount())
                    {
                        MessageBox.Show("销售合同明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (labelHTMXJL.Text == "0")
                    {
                        MessageBox.Show("没有合同商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (MessageBox.Show("请检查合同内容,是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        return;

                    string ss1 = "NULL", ss2 = "NULL", ss3 = "NULL";

                    if (textBoxXHJHSJ.Text.Trim() != "")
                        ss1 = "N'" + textBoxXHJHSJ.Text.Trim() + "'";

                    if (textBoxQHJHSJ.Text.Trim() != "")
                        ss2 = "N'" + textBoxQHJHSJ.Text.Trim() + "'";

                    if (textBoxHTYXQ.Text.Trim() != "")
                        ss3 = "N'" + textBoxHTYXQ.Text.Trim() + "'";

                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {
                        sqlComm.CommandText = "UPDATE 采购合同表 SET 供方单位ID = " + iSupplyCompany.ToString() + ", 业务员ID = " + comboBoxYWY.SelectedValue.ToString() + ", 操作员ID = " + intUserID.ToString() + ", 签订地点 = N'" + textBoxQDDD.Text + "', 签订时间 = '" + strDateSYS + "', 质量要求 = N'" + textBoxZLYQ.Text + "', 异议期限 = N'" + textBoxYYQX.Text + "', 质量负责 = N'" + textBoxFZTJ.Text + "', 随机备品 = N'" + textBoxSJBP.Text + "', 运输方式 = N'" + comboBoxYSFS.Text + "', 运输至 = N'" + textBoxYSZ.Text + "', 费用承担 = N'" + comboBoxFYCD.Text + "', 交货地点 = N'" + textBoxJHDD.Text + "', 现货交货时间 = " + ss1 + ", 现货付款方式 = N'" + textBoxXHFKFS.Text + "', 期货交货时间 = " + ss2 + ", 预付金额 = N'" + textBoxYFJE.Text + "', 提货应付余额 = N'" + textBoxTHYFYE.Text + "', 期货付款方式 = N'" + textBoxQHFKFS.Text + "', 违约责任 = N'" + textBoxWYZR.Text + "', 仲裁委员会 = N'" + textBoxZCWYH.Text + "', 其他约定事项 = N'" + textBoxQTYDSX.Text + "', 合同有效期 = " + ss3 + ", BeActive = 1, 金额 = " + labelJEHJ.Text + ", 供方单位名称 = N'" + textBoxDWMC.Text + "', 供方税号 = N'" + textBoxSH.Text + "', 供方电话 = N'" + textBoxGSDH.Text + "', 供方开户银行 = N'" + textBoxKHYH.Text + "', 供方银行账号 = '" + textBoxZH.Text + "', 供方联系人 = N'" + textBoxFZR.Text + "', 供方地址 = N'" + textBoxGSDZ.Text + "', 供方传真 = N'" + textBoxGSCZ.Text + "', 供方邮编 = '" + textBoxYZBM.Text + "', 需方联系人 = N'"+textBoxFZR1.Text+"' WHERE (ID = " + iDJID.ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //删除明细
                        sqlComm.CommandText = "DELETE FROM 采购合同明细表 WHERE (采购合同ID = "+iDJID.ToString()+")";
                        sqlComm.ExecuteNonQuery();

                        //合同明细
                        for (i = 0; i < dataGridViewCGHT.Rows.Count; i++)
                        {
                            if (dataGridViewCGHT.Rows[i].IsNewRow)
                                continue;

                            sqlComm.CommandText = "INSERT INTO 采购合同明细表 (采购合同ID, 商品ID, 数量, 单价, 总价, 备注) VALUES (" + iDJID.ToString() + ", " + dataGridViewCGHT.Rows[i].Cells[0].Value.ToString() + ", " + dataGridViewCGHT.Rows[i].Cells[3].Value.ToString() + ", " + dataGridViewCGHT.Rows[i].Cells[4].Value.ToString() + ", " + dataGridViewCGHT.Rows[i].Cells[5].Value.ToString() + ", N'" + dataGridViewCGHT.Rows[i].Cells[6].Value.ToString() + "')";
                            sqlComm.ExecuteNonQuery();
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

                    //MessageBox.Show("合同修改保存成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isSaved = true;
                    if (MessageBox.Show("合同修改保存成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        this.Close();
                    }


                    break;
            }

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {

                if (ppd.ShowDialog() != DialogResult.OK)
                    return;

                // Showing the Print Preview Page
                printDoc.BeginPrint += PrintDoc_BeginPrint;
                printDoc.PrintPage += PrintDoc_PrintPage;

                ppw.Width = 1000;
                ppw.Height = 800;
                if (ppw.ShowDialog() != DialogResult.OK)
                {
                    printDoc.BeginPrint -= PrintDoc_BeginPrint;
                    printDoc.PrintPage -= PrintDoc_PrintPage;
                    return;
                }

                // Printing the Documnet
                printDoc.Print();
                printDoc.BeginPrint -= PrintDoc_BeginPrint;
                printDoc.PrintPage -= PrintDoc_PrintPage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {

                if (ppd.ShowDialog() != DialogResult.OK)
                    return;

                // Showing the Print Preview Page
                printDoc.BeginPrint += PrintDoc_BeginPrint;
                printDoc.PrintPage += PrintDoc_PrintPage;

                // Printing the Documnet
                printDoc.Print();
                printDoc.BeginPrint -= PrintDoc_BeginPrint;
                printDoc.PrintPage -= PrintDoc_PrintPage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void PrintDoc_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                PageNo = 1;
                NewPage = true;
                RowPos = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void PrintDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int iyRow;
            int i;
            iyRow = 0;

            // Formatting the Content of Text Cell to print
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;
            StrFormat.LineAlignment = StringAlignment.Center;
            StrFormat.Trimming = StringTrimming.EllipsisCharacter;

            StringFormat StrFormatL = new StringFormat();
            StrFormatL.Alignment = StringAlignment.Near;
            StrFormatL.LineAlignment = StringAlignment.Center;
            StrFormatL.Trimming = StringTrimming.EllipsisCharacter;

            StringFormat StrFormatR = new StringFormat();
            StrFormatR.Alignment = StringAlignment.Far;
            StrFormatR.LineAlignment = StringAlignment.Center;
            StrFormatR.Trimming = StringTrimming.EllipsisCharacter;

            StringFormat StrFormatL1 = new StringFormat();
            StrFormatL1.Alignment = StringAlignment.Near;
            StrFormatL1.LineAlignment = StringAlignment.Near;
            StrFormatL1.Trimming = StringTrimming.EllipsisCharacter;



            System.Drawing.Font _Font22 = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12 = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9 = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12U = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            int iTopM = 20;
            int iLeftM = 60;
            int iWidth = 700;
            int iWidth_R = 260;
            int iWidth_L = 440;

            int iHeight12 = 22;
            int iHeight22 = 50;
            int iHeight9 = 17;

            float fTemp = 0;

            int iRowsPerPage = 39;
            int iPages = 0;

            int iTemp = 0;

            //iTopM = e.MarginBounds.Top;
            foreach (ToolStripMenuItem tsMI in toolStripDropDownButtonBJ.DropDownItems)
            {
                if (tsMI.Checked)
                {
                    iTopM = int.Parse(tsMI.Text);
                    break;
                }
            }


            Brush b = new SolidBrush(Color.Black);

            try
            {
                if (PageNo == 1) //首页
                {
                    //e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM, iTopM, iWidth, iHeight22));
                    e.Graphics.DrawString("采购合同", _Font22, b, new System.Drawing.RectangleF(iLeftM, iTopM, iWidth, iHeight22), StrFormat);
                    iyRow += iTopM + iHeight22;

                    e.Graphics.DrawString("供方：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth_L, iHeight12), StrFormatL);
                    e.Graphics.DrawString(textBoxDWMC.Text, _Font12U, b, new System.Drawing.RectangleF(iLeftM + 45, iyRow, iWidth_L, iHeight12), StrFormatL);
                    e.Graphics.DrawString("合同编号：" + labelHTH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth_L, iyRow, iWidth_R, iHeight12), StrFormatL);
                    iyRow += iHeight12;

                    e.Graphics.DrawString("需方：" + sGSMC, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth_L, iHeight12), StrFormatL);

                    e.Graphics.DrawString("签订地点：" + textBoxQDDD.Text + "　　制单：" + labelCZY.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth_L, iyRow, iWidth_R, iHeight12), StrFormatL);
                    iyRow += iHeight12;

                    e.Graphics.DrawString("签订时间：" + labelZDRQ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth_L, iyRow, iWidth_R, iHeight12), StrFormatL);
                    iyRow += iHeight12;

                    e.Graphics.DrawString("一、产品名称、型号、厂家、数量、单价、金额", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    iyRow += iHeight12;

                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                    e.Graphics.DrawString("序号", _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 260, iHeight12));
                    e.Graphics.DrawString("产品名称", _Font12, b, new System.Drawing.RectangleF(iLeftM + 60, iyRow, 260, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 320, iyRow, 80, iHeight12));
                    e.Graphics.DrawString("数量", _Font12, b, new System.Drawing.RectangleF(iLeftM + 320, iyRow, 80, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 400, iyRow, 100, iHeight12));
                    e.Graphics.DrawString("单价", _Font12, b, new System.Drawing.RectangleF(iLeftM + 400, iyRow, 100, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 100, iHeight12));
                    e.Graphics.DrawString("总价", _Font12, b, new System.Drawing.RectangleF(iLeftM + 500, iyRow, 100, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 600, iyRow, 100, iHeight12));
                    e.Graphics.DrawString("备注", _Font12, b, new System.Drawing.RectangleF(iLeftM + 600, iyRow, 100, iHeight12), StrFormat);
                    iyRow += iHeight12;

                    if (Convert.ToInt32(labelHTMXJL.Text) <= 6) //本页
                    {
                        for (i = 0; i < dataGridViewCGHT.Rows.Count; i++)
                        {
                            if (dataGridViewCGHT.Rows[i].IsNewRow)
                                continue;
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                            e.Graphics.DrawString(Convert.ToString(i + 1), _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 260, iHeight12));
                            e.Graphics.DrawString(dataGridViewCGHT.Rows[i].Cells[1].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iLeftM + 60, iyRow, 260, iHeight12), StrFormat);
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 320, iyRow, 80, iHeight12));
                            e.Graphics.DrawString(dataGridViewCGHT.Rows[i].Cells[3].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iLeftM + 320, iyRow, 80, iHeight12), StrFormat);
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 400, iyRow, 100, iHeight12));
                            e.Graphics.DrawString(Convert.ToDecimal(dataGridViewCGHT.Rows[i].Cells[4].Value.ToString()).ToString("f2"), _Font12, b, new System.Drawing.RectangleF(iLeftM + 400, iyRow, 100, iHeight12), StrFormat);
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 100, iHeight12));
                            e.Graphics.DrawString(Convert.ToDecimal(dataGridViewCGHT.Rows[i].Cells[5].Value.ToString()).ToString("f2"), _Font12, b, new System.Drawing.RectangleF(iLeftM + 500, iyRow, 100, iHeight12), StrFormat);
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 600, iyRow, 100, iHeight12));
                            e.Graphics.DrawString(dataGridViewCGHT.Rows[i].Cells[6].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iLeftM + 600, iyRow, 100, iHeight12), StrFormat);
                            iyRow += iHeight12;


                        }
                    }
                    else //新页
                    {
                        //iPages = (int)Math.Ceiling((decimal)(dataGridViewCGHT.Rows.Count)/(decimal)(iRowsPerPage));
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                        e.Graphics.DrawString("1", _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 260, iHeight12));
                        e.Graphics.DrawString("详见合同清单", _Font12, b, new System.Drawing.RectangleF(iLeftM + 60, iyRow, 200, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 320, iyRow, 80, iHeight12));
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 400, iyRow, 100, iHeight12));
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 100, iHeight12));
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 600, iyRow, 100, iHeight12));

                        iyRow += iHeight12;


                    }
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                    e.Graphics.DrawString("合计", _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 440, iHeight12));
                    e.Graphics.DrawString(labelDX.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + 80, iyRow, 420, iHeight12), StrFormatL);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 200, iHeight12));
                    e.Graphics.DrawString("￥" + labelJEHJ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + 520, iyRow, 180, iHeight12), StrFormatL);

                    iyRow += iHeight12 * 2;
                    //iyRow = 450;

                    e.Graphics.DrawString("二、质量要求、技术标准：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("二、质量要求、技术标准：", _Font12).Width;
                    e.Graphics.DrawString(textBoxZLYQ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("三、需方对产品型号、质量的提出异议的期限：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("三、需方对产品型号、质量的提出异议的期限：", _Font12).Width;
                    e.Graphics.DrawString(textBoxYYQX.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("四、供方对质量负责的条件、方法和期限：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("四、供方对质量负责的条件、方法和期限：", _Font12).Width;
                    e.Graphics.DrawString(textBoxFZTJ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("五、随机备品、配件工具数量及供应方法：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("五、随机备品、配件工具数量及供应方法：", _Font12).Width;
                    e.Graphics.DrawString(textBoxSJBP.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("六、运输方式（" + comboBoxYSFS.Text + "）代办运输至：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("六、运输方式（　" + comboBoxYSFS.Text + "　）代办运输至：：", _Font12).Width;
                    e.Graphics.DrawString(textBoxYSZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("七、费用承担：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("七、费用承担：", _Font12).Width;
                    e.Graphics.DrawString(comboBoxFYCD.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth / 2, iyRow + iHeight12 - 6));


                    e.Graphics.DrawString("交货地点：", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("交货地点：", _Font12).Width;
                    e.Graphics.DrawString(textBoxJHDD.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow, iWidth / 2 - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));

                    iyRow += iHeight12;

                    e.Graphics.DrawString("八、现货：交货时间：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("八、现货：交货时间：", _Font12).Width;

                    string ss1 = textBoxXHJHSJ.Text.Trim();

                    try
                    {
                        ss1 = DateTime.Parse(textBoxXHJHSJ.Text.Trim()).ToString("yyyy年M月dd日");
                    }
                    catch
                    {
                        ss1 = textBoxXHJHSJ.Text.Trim();
                    }


                    e.Graphics.DrawString(ss1, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth / 2, iyRow + iHeight12 - 6));

                    e.Graphics.DrawString("付款方式：", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("付款方式：", _Font12).Width;
                    e.Graphics.DrawString(textBoxXHFKFS.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow, iWidth / 2 - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;


                    e.Graphics.DrawString("九、期货：交货时间：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("九、期货：交货时间：", _Font12).Width;
                    //ss1 = textBoxQHJHSJ.Text.Trim();

                    try
                    {
                        ss1 = DateTime.Parse(textBoxQHJHSJ.Text.Trim()).ToString("yyyy年M月dd日");
                    }
                    catch
                    {
                        ss1 = textBoxQHJHSJ.Text.Trim();
                    }

                    e.Graphics.DrawString(ss1, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth / 2, iyRow + iHeight12 - 6));

                    e.Graphics.DrawString("预付金额：", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("预付金额：：", _Font12).Width;
                    e.Graphics.DrawString(textBoxYFJE.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow, iWidth / 2 - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("　　提货应付余额：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("　　提货应付余额：", _Font12).Width;
                    e.Graphics.DrawString(textBoxTHYFYE.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point(iLeftM + (int)fTemp, iyRow + iHeight12 - 6), new Point(iLeftM + iWidth / 2, iyRow + iHeight12 - 6));

                    e.Graphics.DrawString("付款方式：", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("付款方式：：", _Font12).Width;
                    e.Graphics.DrawString(textBoxQHFKFS.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow, iWidth / 2 - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;


                    e.Graphics.DrawString("十、违约责任：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("十、违约责任：", _Font12).Width;
                    e.Graphics.DrawString(textBoxWYZR.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));

                    iyRow += iHeight12;

                    e.Graphics.DrawString("十一、解决合同纠纷的方式：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    iyRow += iHeight12;
                    e.Graphics.DrawString("　　执行合同发生争议，由当事人双方协商解决。协商不成，双方同意由" + textBoxZCWYH.Text + "仲裁委员会仲裁。", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    iyRow += iHeight12;

                    e.Graphics.DrawString("十二、其他约定事项：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("十二、其他约定事项：", _Font12).Width;
                    e.Graphics.DrawString(textBoxQTYDSX.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("十三、合同有效期：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormatL);
                    fTemp = e.Graphics.MeasureString("十三、合同有效期：", _Font12).Width;
                    try
                    {
                        ss1 = DateTime.Parse(textBoxHTYXQ.Text.Trim()).ToString("yyyy年M月dd日");
                    }
                    catch
                    {
                        ss1 = textBoxHTYXQ.Text.Trim();
                    }
                    e.Graphics.DrawString(ss1, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow, iWidth - fTemp, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth, iyRow + iHeight12 - 6));
                    iyRow += iHeight12 + iHeight9;

                    //iyRow = 780;

                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM - 4, iyRow, iWidth + 8, 322));
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM, iyRow + 4, iWidth / 2, 314));
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + iWidth / 2, iyRow + 4, iWidth / 2, 314));

                    iyRow += 10;

                    e.Graphics.DrawString("需方", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2, iHeight12), StrFormat);
                    e.Graphics.DrawString("供方", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2, iHeight12), StrFormat);
                    iyRow += iHeight12;

                    fTemp = e.Graphics.MeasureString("公司名称：", _Font12).Width;

                    /*
                    e.Graphics.DrawString("公司名称："+sGSMC, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("公司名称：" + textBoxDWMC.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;
                    */
                    e.Graphics.DrawString("公司名称：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString(sGSMC, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow + 3, iWidth / 2 - 5 - fTemp, iHeight12 * 2), StrFormatL1);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 4)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 * 2 - 4)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 * 2 - 6));
                    e.Graphics.DrawString("公司名称：", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString(textBoxDWMC.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow + 3, iWidth / 2 - 5 - fTemp, iHeight12 * 2), StrFormatL1);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 4)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 * 2 - 4)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 * 2 - 6));
                    iyRow += iHeight12 * 2;

                    //e.Graphics.DrawString("负责人：　" + textBoxFZR1.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString("负责人：　" + comboBoxYWY.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString("负责人：　" + textBoxFZR.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    iyRow += iHeight12;

                    e.Graphics.DrawString("（公章）", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString("（公章）", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    iyRow += iHeight12;


                    e.Graphics.DrawString("公司地址：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString(sGSDZ, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow + 3, iWidth / 2 - 5 - fTemp, iHeight12 * 2), StrFormatL1);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 4)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 * 2 - 4)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 * 2 - 6));
                    e.Graphics.DrawString("公司地址：", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString(textBoxGSDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow + 3, iWidth / 2 - 5 - fTemp, iHeight12 * 2), StrFormatL1);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 4)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 * 2 - 4)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 * 2 - 6));
                    iyRow += iHeight12 * 2;

                    e.Graphics.DrawString("公司电话：", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString(sGSDH, _Font12, b, new System.Drawing.RectangleF(iLeftM + fTemp, iyRow + 3, iWidth / 2 - 5 - fTemp, iHeight12 * 2), StrFormatL1);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 4)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 * 2 - 4)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 * 2 - 6));
                    e.Graphics.DrawString("公司电话：", _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawString(textBoxGSDH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2 + fTemp, iyRow + 3, iWidth / 2 - 5 - fTemp, iHeight12 * 2), StrFormatL1);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 4)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 * 2 - 4)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 * 2 - 6));
                    /*
                    e.Graphics.DrawString("公司电话："+sGSDH, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("公司电话：" + textBoxGSDH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                     */
                    iyRow += iHeight12 * 2;


                    e.Graphics.DrawString("公司传真：" + sGSCZ, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("公司传真：" + textBoxGSCZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("税　　号：" + sGSSH, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("税　　号：" + textBoxSH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("开户银行：" + sGSKHYH, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("开户银行：" + textBoxKHYH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("帐　　号：" + sGSZH, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("帐　　号：" + textBoxZH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));
                    iyRow += iHeight12;

                    e.Graphics.DrawString("邮政编码：" + sGSYB, _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + fTemp), (int)(iyRow + iHeight12 - 6)), new Point((int)(iLeftM + iWidth / 2 - 5), iyRow + iHeight12 - 6));
                    e.Graphics.DrawString("邮政编码：" + textBoxYZBM.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth / 2, iyRow, iWidth / 2 - 5, iHeight12), StrFormatL);
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + iWidth / 2 + fTemp), (int)(iyRow + iHeight12 - 6)), new Point(iLeftM + iWidth - 5, iyRow + iHeight12 - 6));

                    iyRow = e.PageSettings.PaperSize.Height - 80;

                    fTemp = e.Graphics.MeasureString(sK, _Font9).Width;
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM), (int)(iHeight9 / 2 + iyRow)), new Point((int)(iLeftM + (iWidth - fTemp) / 2), (int)(iHeight9 / 2 + iyRow)));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + (iWidth + fTemp) / 2), (int)(iHeight9 / 2 + iyRow)), new Point(iLeftM + iWidth - 5, (int)(iHeight9 / 2 + iyRow)));
                    e.Graphics.DrawString(sK, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight9), StrFormat);
                    iyRow += iHeight9;
                    e.Graphics.DrawString(sM1, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, (int)(iWidth / 4), iHeight9), StrFormat);
                    e.Graphics.DrawString(sM2, _Font9, b, new System.Drawing.RectangleF((int)(iLeftM + iWidth / 4), iyRow, (int)(iWidth / 4), iHeight9), StrFormat);
                    e.Graphics.DrawString(sM3, _Font9, b, new System.Drawing.RectangleF((int)(iLeftM + iWidth / 2), iyRow, (int)(iWidth / 4), iHeight9), StrFormat);
                    e.Graphics.DrawString(sM4, _Font9, b, new System.Drawing.RectangleF((int)(iLeftM + iWidth * 3 / 4), iyRow, (int)(iWidth / 4), iHeight9), StrFormat);

                    if (Convert.ToInt32(labelHTMXJL.Text) > 6) //
                    {
                        PageNo++;
                        e.HasMorePages = true;
                        return;
                    }
                }
                else //明细
                {
                    iPages = (int)Math.Ceiling((decimal)(dataGridViewCGHT.Rows.Count - 1) / (decimal)(iRowsPerPage));
                    e.Graphics.DrawString("采购合同商品清单", _Font22, b, new System.Drawing.RectangleF(iLeftM, iTopM, iWidth, iHeight22), StrFormat);
                    iyRow += iTopM + iHeight22;
                    e.Graphics.DrawString("合同编号（" + labelHTH.Text + "）", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight12), StrFormat);
                    iyRow += iHeight12;

                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                    e.Graphics.DrawString("序号", _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 260, iHeight12));
                    e.Graphics.DrawString("产品名称", _Font12, b, new System.Drawing.RectangleF(iLeftM + 60, iyRow, 260, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 320, iyRow, 80, iHeight12));
                    e.Graphics.DrawString("数量", _Font12, b, new System.Drawing.RectangleF(iLeftM + 320, iyRow, 80, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 400, iyRow, 100, iHeight12));
                    e.Graphics.DrawString("单价", _Font12, b, new System.Drawing.RectangleF(iLeftM + 400, iyRow, 100, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 100, iHeight12));
                    e.Graphics.DrawString("总价", _Font12, b, new System.Drawing.RectangleF(iLeftM + 500, iyRow, 100, iHeight12), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 600, iyRow, 100, iHeight12));
                    e.Graphics.DrawString("备注", _Font12, b, new System.Drawing.RectangleF(iLeftM + 600, iyRow, 100, iHeight12), StrFormat);
                    iyRow += iHeight12;

                    for (i = 0; i < iRowsPerPage; i++)
                    {

                        if (RowPos >= dataGridViewCGHT.Rows.Count) //结束
                        {
                            NewPage = false;
                            break;
                        }

                        if (dataGridViewCGHT.Rows[RowPos].IsNewRow)
                        {
                            break;
                        }

                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                        e.Graphics.DrawString(Convert.ToString(RowPos + 1), _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 260, iHeight12));
                        e.Graphics.DrawString(dataGridViewCGHT.Rows[RowPos].Cells[1].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iLeftM + 60, iyRow, 260, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 320, iyRow, 80, iHeight12));
                        e.Graphics.DrawString(dataGridViewCGHT.Rows[RowPos].Cells[3].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iLeftM + 320, iyRow, 80, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 400, iyRow, 100, iHeight12));
                        e.Graphics.DrawString(Convert.ToDecimal(dataGridViewCGHT.Rows[RowPos].Cells[4].Value.ToString()).ToString("f2"), _Font12, b, new System.Drawing.RectangleF(iLeftM + 400, iyRow, 100, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 100, iHeight12));
                        e.Graphics.DrawString(Convert.ToDecimal(dataGridViewCGHT.Rows[RowPos].Cells[5].Value.ToString()).ToString("f2"), _Font12, b, new System.Drawing.RectangleF(iLeftM + 500, iyRow, 100, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 600, iyRow, 100, iHeight12));
                        e.Graphics.DrawString(dataGridViewCGHT.Rows[RowPos].Cells[6].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iLeftM + 600, iyRow, 100, iHeight12), StrFormat);


                        iyRow += iHeight12;
                        RowPos++;
                    }
                    iTemp = iyRow;
                    iyRow = e.PageSettings.PaperSize.Height - 70;

                    e.Graphics.DrawString("第" + (PageNo - 1).ToString() + "页，共" + iPages.ToString() + "页", _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight9), StrFormatR);
                    iyRow += iHeight9;

                    fTemp = e.Graphics.MeasureString(sK, _Font9).Width;
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM), (int)(iHeight9 / 2 + iyRow)), new Point((int)(iLeftM + (iWidth - fTemp) / 2), (int)(iHeight9 / 2 + iyRow)));
                    e.Graphics.DrawLine(Pens.Black, new Point((int)(iLeftM + (iWidth + fTemp) / 2), (int)(iHeight9 / 2 + iyRow)), new Point(iLeftM + iWidth - 5, (int)(iHeight9 / 2 + iyRow)));
                    e.Graphics.DrawString(sK, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iWidth, iHeight9), StrFormat);
                    iyRow += iHeight9;
                    e.Graphics.DrawString(sM1, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, (int)(iWidth / 4), iHeight9), StrFormat);
                    e.Graphics.DrawString(sM2, _Font9, b, new System.Drawing.RectangleF((int)(iLeftM + iWidth / 4), iyRow, (int)(iWidth / 4), iHeight9), StrFormat);
                    e.Graphics.DrawString(sM3, _Font9, b, new System.Drawing.RectangleF((int)(iLeftM + iWidth / 2), iyRow, (int)(iWidth / 4), iHeight9), StrFormat);
                    e.Graphics.DrawString(sM4, _Font9, b, new System.Drawing.RectangleF((int)(iLeftM + iWidth * 3 / 4), iyRow, (int)(iWidth / 4), iHeight9), StrFormat);


                    //金额合计
                    if (RowPos >= dataGridViewCGHT.Rows.Count - 1)
                    {
                        iyRow = iTemp;

                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 10, iyRow, 50, iHeight12));
                        e.Graphics.DrawString("合计", _Font12, b, new System.Drawing.RectangleF(iLeftM + 10, iyRow, 50, iHeight12), StrFormat);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 60, iyRow, 440, iHeight12));
                        e.Graphics.DrawString(labelDX.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + 80, iyRow, 420, iHeight12), StrFormatL);
                        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 500, iyRow, 200, iHeight12));
                        e.Graphics.DrawString("￥" + labelJEHJ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + 520, iyRow, 180, iHeight12), StrFormatL);
                        e.HasMorePages = false;

                    }
                    else
                    {

                        if (NewPage)
                        {
                            PageNo++;
                            e.HasMorePages = true;
                        }
                        else
                        {
                            e.HasMorePages = false;
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void textBoxHTYXQ_Validating(object sender, CancelEventArgs e)
        {
            /*
            if (textBoxHTYXQ.Text.Trim() == "")
            {
                this.errorProviderHT.Clear();
                return;
            }

            try
            {
                DateTime.Parse(textBoxHTYXQ.Text.Trim());
                this.errorProviderHT.Clear();
            }
            catch
            {
                this.errorProviderHT.SetError(this.textBoxHTYXQ, "请输入有效日期，例如：2000-12-31");
                e.Cancel = true;
            }
             */
        }

        private void toolStripButtonWord_Click(object sender, EventArgs e)
        {
            int i, j;
            int iNUMROW = 0;

            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("销售合同明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }



            SaveFileDialog saveFileDialogOutput = new SaveFileDialog();
            saveFileDialogOutput.Filter = "WORD files(*.doc)|*.doc";//
            saveFileDialogOutput.FilterIndex = 0;
            saveFileDialogOutput.RestoreDirectory = true;

            if (saveFileDialogOutput.ShowDialog() != DialogResult.OK) return;

            try
            {

                string FullFileName = saveFileDialogOutput.FileName.ToString();
                FileInfo info = new FileInfo(FullFileName);

                Microsoft.Office.Interop.Word.Application docApp = new Microsoft.Office.Interop.Word.ApplicationClass();
                object missingValue = Type.Missing;

                object template = Directory.GetCurrentDirectory() + "\\Sample\\gjhtmb1.dot";

                _Document doc = docApp.Documents.Add(ref template, ref missingValue, ref missingValue, ref missingValue);

                string bookmarkName = "HTBH";
                object oBookmarkName = bookmarkName;
                Microsoft.Office.Interop.Word.Range rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = labelHTH.Text;
                object oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GF";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxDWMC.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "XF";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSMC;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "QDDD";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxQDDD.Text + "　　制单：" + labelCZY.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "QDSJ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = labelZDRQ.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "MXBG";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;

                if (dataGridViewCGHT.Rows[dataGridViewCGHT.Rows.Count - 1].IsNewRow)
                {
                    iNUMROW = dataGridViewCGHT.Rows.Count - 1;
                }
                else
                    iNUMROW = dataGridViewCGHT.Rows.Count;
                Table tbMX = doc.Tables.Add(rng, iNUMROW + 2, 6, ref missingValue, ref missingValue);

                //Table tbMX = doc.Tables.Add(rng, dataGridViewCGHT.Rows.Count + 1, 6, ref missingValue, ref missingValue);

                tbMX.Borders[WdBorderType.wdBorderLeft].LineStyle = WdLineStyle.wdLineStyleSingle;
                tbMX.Borders[WdBorderType.wdBorderLeft].LineWidth = WdLineWidth.wdLineWidth050pt;

                tbMX.Borders[WdBorderType.wdBorderRight].LineStyle = WdLineStyle.wdLineStyleSingle;
                tbMX.Borders[WdBorderType.wdBorderRight].LineWidth = WdLineWidth.wdLineWidth050pt;

                tbMX.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
                tbMX.Borders[WdBorderType.wdBorderTop].LineWidth = WdLineWidth.wdLineWidth050pt;

                tbMX.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
                tbMX.Borders[WdBorderType.wdBorderBottom].LineWidth = WdLineWidth.wdLineWidth050pt;

                tbMX.Borders[WdBorderType.wdBorderHorizontal].LineStyle = WdLineStyle.wdLineStyleSingle;
                tbMX.Borders[WdBorderType.wdBorderHorizontal].LineWidth = WdLineWidth.wdLineWidth050pt;

                tbMX.Borders[WdBorderType.wdBorderVertical].LineStyle = WdLineStyle.wdLineStyleSingle;
                tbMX.Borders[WdBorderType.wdBorderVertical].LineWidth = WdLineWidth.wdLineWidth050pt;

                tbMX.Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                tbMX.Range.Rows.Alignment = WdRowAlignment.wdAlignRowCenter;
                //tbMX.Range.w



                tbMX.Cell(1, 1).Range.Text = "序号";
                tbMX.Cell(1, 2).Range.Text = "产品名称";
                tbMX.Cell(1, 3).Range.Text = "数量";
                tbMX.Cell(1, 4).Range.Text = "单价";
                tbMX.Cell(1, 5).Range.Text = "总价";
                tbMX.Cell(1, 6).Range.Text = "备注";

                tbMX.Columns[1].SetWidth(35, WdRulerStyle.wdAdjustNone);
                tbMX.Columns[2].SetWidth(115, WdRulerStyle.wdAdjustNone);
                tbMX.Columns[3].SetWidth(50, WdRulerStyle.wdAdjustNone);
                tbMX.Columns[4].SetWidth(80, WdRulerStyle.wdAdjustNone);
                tbMX.Columns[5].SetWidth(80, WdRulerStyle.wdAdjustNone);
                tbMX.Columns[6].SetWidth(60, WdRulerStyle.wdAdjustNone);


                for (i = 0; i < iNUMROW; i++)
                {
                    if (dataGridViewCGHT.Rows[i].IsNewRow)
                    {
                        continue;
                    }

                    tbMX.Cell(i + 2, 1).Range.Text = (i + 1).ToString();
                    tbMX.Cell(i + 2, 2).Range.Text = dataGridViewCGHT.Rows[i].Cells[1].Value.ToString();
                    tbMX.Cell(i + 2, 3).Range.Text = dataGridViewCGHT.Rows[i].Cells[3].Value.ToString();
                    tbMX.Cell(i + 2, 4).Range.Text = decimal.Parse(dataGridViewCGHT.Rows[i].Cells[4].Value.ToString()).ToString("f2");
                    tbMX.Cell(i + 2, 5).Range.Text = decimal.Parse(dataGridViewCGHT.Rows[i].Cells[5].Value.ToString()).ToString("f2");
                    tbMX.Cell(i + 2, 6).Range.Text = dataGridViewCGHT.Rows[i].Cells[6].Value.ToString();

                }

                i = iNUMROW + 2;
                tbMX.Cell(i, 1).Range.Text = "合计";
                tbMX.Cell(i, 2).Merge(tbMX.Cell(i, 3));
                tbMX.Cell(i, 2).Merge(tbMX.Cell(i, 3));
                tbMX.Cell(i, 3).Merge(tbMX.Cell(i, 4));
                tbMX.Cell(i, 2).Range.Text = labelDX.Text;
                tbMX.Cell(i, 3).Range.Text = decimal.Parse(labelJEHJ.Text).ToString("f2"); ;


                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "ZLYQ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxZLYQ.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);


                bookmarkName = "YYQX";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxYYQX.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "FZTJ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxFZTJ.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);


                bookmarkName = "SJBP";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxSJBP.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "YSFS";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = comboBoxYSFS.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "YSZ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxYSZ.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "FYCD";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = comboBoxFYCD.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "JHDD";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxJHDD.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                string ss1 = textBoxXHJHSJ.Text.Trim();

                try
                {
                    ss1 = DateTime.Parse(textBoxXHJHSJ.Text.Trim()).ToString("yyyy年M月dd日");
                }
                catch
                {
                    ss1 = textBoxXHJHSJ.Text.Trim();
                }

                bookmarkName = "XHJHSJ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = ss1;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "XHFKFS";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxXHFKFS.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                try
                {
                    ss1 = DateTime.Parse(textBoxQHJHSJ.Text.Trim()).ToString("yyyy年M月dd日");
                }
                catch
                {
                    ss1 = textBoxQHJHSJ.Text.Trim();
                }

                bookmarkName = "QHJHSJ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = ss1;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "YFJE";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxYFJE.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "THYFYE";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxTHYFYE.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "QHFKFS";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxQHFKFS.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "WYZR";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxWYZR.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "ZCWYH";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxZCWYH.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "QTYDSX";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxQTYDSX.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "QTYDSX";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxQTYDSX.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);


                try
                {
                    ss1 = DateTime.Parse(textBoxHTYXQ.Text.Trim()).ToString("yyyy年M月dd日");
                }
                catch
                {
                    ss1 = textBoxHTYXQ.Text.Trim();
                }
                bookmarkName = "HTYXQ";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = ss1;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GSMC1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSMC;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GSMC2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxDWMC.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "FZR1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                //rng.Text = textBoxFZR1.Text;
                rng.Text = comboBoxYWY.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "FZR2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxFZR.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GSDZ1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSDZ;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);


                bookmarkName = "GSDZ2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxGSDZ.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GSDH1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSDH;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);


                bookmarkName = "GSDH2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxGSDH.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GSCZ1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSCZ;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "GSCZ2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxGSCZ.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "SH1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSSH;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "SH2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxSH.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "KHYH1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSKHYH;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "KHYH2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxKHYH.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "ZH1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSZH;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "ZH2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxZH.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "YZBM1";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = sGSYB;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);

                bookmarkName = "YZBM2";
                oBookmarkName = bookmarkName;
                rng = doc.Bookmarks.get_Item(ref oBookmarkName).Range;
                rng.Text = textBoxYZBM.Text;
                oRng = rng;
                doc.Bookmarks.Add(bookmarkName, ref oRng);
                docApp.Visible = true;
                doc.PrintPreview();

                object fileName = FullFileName;
                doc.SaveAs(ref fileName, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue);
                MessageBox.Show("合同保存完毕");

                doc.Close(ref missingValue, ref missingValue, ref missingValue);
                docApp.Quit(ref missingValue, ref missingValue, ref missingValue);



                if (doc != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                    doc = null;
                }
                if (docApp != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(docApp);
                    docApp = null;
                }
                GC.Collect();
                //KillWordProcess();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "输出失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem tsMI in toolStripDropDownButtonBJ.DropDownItems)
            {
                tsMI.Checked = false;
            }

            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.Checked = true;

        }

        private void comboBoxYWY_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxFZR1.Text = comboBoxYWY.Text;
        }




    }
}
