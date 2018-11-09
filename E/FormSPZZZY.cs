using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPZZZY : Form
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

        private int intKFID = 0;
        private int intCommID = 0;

        private ClassGetInformation cGetInformation;


        public FormSPZZZY()
        {
            InitializeComponent();
        }

        private void FormSPZZZY_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            this.Top = 1;
            this.Left = 1;

            sqlConn.Open();
            //得到开始时间
            sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            sqlConn.Close();

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            comboBoxDJLB.SelectedIndex = 0;

        }

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
                if (cGetInformation.getCommInformation(1, "") == 0) //失败
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;

                }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                }

            }
        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                }

            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal dsum = 0, dsum1 = 0, dsum2 = 0; ;

            if (intCommID==0)
            {
                MessageBox.Show("请选择要查询的商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            sqlConn.Open();
            //sqlComm.CommandText = "SELECT 日期, 单据编号, 摘要, 入库数量, 入库单价, 入库金额, 出库数量, 出库金额, 退出数量, 退出金额, 退回数量, 退回金额, 盘损数量, 盘损金额, 报损数量, 借物金额, 借物数量, 报损金额, 总结存数量, 总结存金额, 销售金额, 毛利, 销售退补价数量, 销售退补价单价, 销售退补价金额, 购进退补价数量, 购进退补价单价,购进退补价金额 FROM 商品历史账表 WHERE (商品历史账表.商品ID = " + intCommID.ToString() + ") AND (商品历史账表.BeActive = 1) AND (商品历史账表.日期 >= CONVERT(DATETIME, '"+dateTimePickerS.Value.ToShortDateString()+" 00:00:00', 102)) ORDER BY 商品历史账表.日期";

            sqlComm.CommandText = "SELECT 单据编号, 数量, 单价, 实计金额, 毛利, '' AS 说明, 表单ID FROM 单据明细汇总视图 WHERE (BeActive = 1) AND (商品ID = " + intCommID.ToString() + ") AND (日期 >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) ";
            switch(comboBoxDJLB.SelectedIndex)
            {
                case 1:
                    sqlComm.CommandText += " AND 单据编号  LIKE N'%AKP%' ";
                    break;
                case 2:
                    sqlComm.CommandText += " AND 单据编号  LIKE N'%ADH%' ";
                    break;
                case 3:
                    sqlComm.CommandText += " AND 单据编号  LIKE N'%ATH%' ";
                    break;
                case 4:
                    sqlComm.CommandText += " AND 单据编号  LIKE N'%ATB%' ";
                    break;
                case 5:
                    sqlComm.CommandText += " AND 单据编号  LIKE N'%BKP%' ";
                    break;
                case 6:
                    sqlComm.CommandText += " AND 单据编号  LIKE N'%BCK%' ";
                    break;
                case 7:
                    sqlComm.CommandText += " AND 单据编号  LIKE N'%BTH%' ";
                    break;
                case 8:
                    sqlComm.CommandText += " AND 单据编号  LIKE N'%BTB%' ";
                    break;
                case 9:
                    sqlComm.CommandText += " AND 单据编号  LIKE N'%CCK%' AND 数量 > 0 ";
                    break;
                case 10:
                    sqlComm.CommandText += " AND 单据编号  LIKE N'%CCK%' AND 数量 < 0 ";
                    break;
            }
            sqlComm.CommandText +=" ORDER BY 日期";
            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            /*
            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
                for (j = 1; j < dSet.Tables["商品表"].Columns.Count; j++)
                {
                    if (dSet.Tables["商品表"].Rows[i][j].ToString() == "")
                        dSet.Tables["商品表"].Rows[i][j] = 0;
 
                }
            */
            string stemp="";
            dsum = 0; dsum1 = 0; dsum2 = 0;


            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表"].Rows[i][4].ToString() != "")
                    dsum += decimal.Parse(dSet.Tables["商品表"].Rows[i][4].ToString());
                if (dSet.Tables["商品表"].Rows[i][1].ToString() != "")
                    dsum1 += decimal.Parse(dSet.Tables["商品表"].Rows[i][1].ToString());
                if (dSet.Tables["商品表"].Rows[i][3].ToString() != "")
                    dsum2 += decimal.Parse(dSet.Tables["商品表"].Rows[i][3].ToString());
                stemp = dSet.Tables["商品表"].Rows[i][0].ToString().Substring(0, 3);

                switch (stemp)
                {
                    case "AKP":
                        dSet.Tables["商品表"].Rows[i][5] = "购进商品开票";
                        break;
                    case "ADH":
                        dSet.Tables["商品表"].Rows[i][5] = "购进商品入库";
                        break;
                    case "ATH":
                        dSet.Tables["商品表"].Rows[i][5] = "购进商品退出";
                        break;
                    case "ATB":
                        dSet.Tables["商品表"].Rows[i][5] = "购进商品退补差价";
                        break;
                    case "BKP":
                        dSet.Tables["商品表"].Rows[i][5] = "销售商品出库";
                        break;
                    case "BCK":
                        dSet.Tables["商品表"].Rows[i][5] = "销售商品出库校对";
                        break;
                    case "BTH":
                        dSet.Tables["商品表"].Rows[i][5] = "销售商品退回";
                        break;
                    case "BTB":
                        dSet.Tables["商品表"].Rows[i][5] = "销售商品退补差价";
                        break;
                    case "CCK":
                        dSet.Tables["商品表"].Rows[i][5] = "借物出库";
                        sqlComm.CommandText = "SELECT 出库金额, 冲抵单号ID, 单据编号 FROM 借物出库汇总表 WHERE (单据编号 = N'" + dSet.Tables["商品表"].Rows[i][0].ToString() + "')";
                        sqldr = sqlComm.ExecuteReader();

                        if(!sqldr.HasRows)
                        {
                            if (decimal.Parse(dSet.Tables["商品表"].Rows[i][1].ToString()) > 0)
                                dSet.Tables["商品表"].Rows[i][5] = "借物出库";
                            else
                                dSet.Tables["商品表"].Rows[i][5] = "借物入库";
                            sqldr.Close();
                            break;
                        }
                        sqldr.Read();
                        try
                        {
                            if (decimal.Parse(dSet.Tables["商品表"].Rows[i][1].ToString()) > 0)
                            {
                                if (sqldr.GetValue(1).ToString() == "-1")
                                    dSet.Tables["商品表"].Rows[i][5] = "借物冲抵单（出库）";
                                else
                                {
                                    if (sqldr.GetValue(1).ToString() == "")
                                        dSet.Tables["商品表"].Rows[i][5] = "借物出库单（未充抵）";
                                    else
                                        dSet.Tables["商品表"].Rows[i][5] = "借物出库单（已充抵）";
                                }

                            }
                            else
                            {
                                if (sqldr.GetValue(1).ToString() == "-1")
                                    dSet.Tables["商品表"].Rows[i][5] = "借物冲抵单（入库）";
                                else
                                {
                                    if (sqldr.GetValue(1).ToString() == "")
                                        dSet.Tables["商品表"].Rows[i][5] = "借物入库单（未充抵）";
                                    else
                                        dSet.Tables["商品表"].Rows[i][5] = "借物入库单（已充抵）";
                                }
                            }


                        }
                        catch
                        {
                            if (decimal.Parse(dSet.Tables["商品表"].Rows[i][1].ToString())>0)
                                dSet.Tables["商品表"].Rows[i][5] = "借物出库";
                            else
                                dSet.Tables["商品表"].Rows[i][5] = "借物入库";
                        }
                        sqldr.Close();
                        break;
                    default:
                        break;


                }
            }
            dataGridViewDJMX.Columns[1].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f2";

            dataGridViewDJMX.Columns[6].Visible = false;


            for (i = 0; i < dataGridViewDJMX.ColumnCount; i++)
            {
                dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
            if (comboBoxDJLB.SelectedIndex == 0)
                toolStripStatusLabelMXJLS.Text += " 毛利:" + dsum.ToString("f2");
            else
                toolStripStatusLabelMXJLS.Text += " 数量:" + dsum1.ToString("f0")+" 金额:" + dsum2.ToString("f2");
            sqlConn.Close();
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品总账账页(商品:" + textBoxSPMC.Text + ");当前日期：" + labelZDRQ.Text + ";";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品总账账页(商品:" + textBoxSPMC.Text + ");当前日期：" + labelZDRQ.Text + ";";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.Add)
            {
                //System.Windows.Forms.SendKeys.Send("{tab}");
                toolStripButtonGD_Click(null, null);//
                return true;
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewDJMX.RowCount < 1)
                return;

            if (dataGridViewDJMX.SelectedRows.Count < 1)
                return;

            string sTemp = "", sTemp1 = "";

            if (e == null)
            {
                sTemp = dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString().ToUpper();
                sTemp1 = dataGridViewDJMX.SelectedRows[0].Cells[6].Value.ToString();
            }
            else
            {
                sTemp = dataGridViewDJMX.Rows[e.RowIndex].Cells[0].Value.ToString().ToUpper();
                sTemp1 = dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value.ToString();
            }

            //if(e.RowIndex<0)
            //    return;

            //if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
            //    return;


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





    }
}