using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace business
{
    public partial class FormXSCKZD : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        private System.Data.DataSet dSetP1 = new DataSet();

        public string strConn = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;


        private int iSupplyCompany = 0;
        private int intHTH = 0;
        private ClassGetInformation cGetInformation;

        public bool isSaved = false;
        public bool isLimit = false;
        public int iDJID = 0;

        private int RowPos;              // Position of currently printing row 
        private bool NewPage;            // Indicates if a new page reached
        private int PageNo;
        private int intNo; //序号
        private bool bCheck = true;

        private string sGSMC = "";
        private string sGSDZ = "";
        private string sGSDH = "";
        private string sGSCZ = "";
        private string sGSYB = "";
        private string sGSZH = "";
        private string sGSKHYH = "";
        private string sGSSH = "";

        private const int iPageZX = 20; //装箱单个数
        private const int iPageNZX = 10;

        public int LIMITACCESS = 18;

        public int iVersion = 1;

        public FormXSCKZD()
        {
            InitializeComponent();
        }

        private void FormXSCKZD_Load(object sender, EventArgs e)
        {
            int i;

            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            string dFileName = Directory.GetCurrentDirectory() + "\\print1.xml";

            if (File.Exists(dFileName)) //存在文件
            {
                dSetP1.ReadXml(dFileName);
            }

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
            }
            sqldr.Close();
            sqlConn.Close();


            if (isSaved)
            {
                dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                initDJ();
                
                return;
            }

            sqlConn.Open();

            //初始化员工列表
            sqlComm.CommandText = "SELECT ID, 职员编号, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.Text = strUserName;

            //初始化部门列表
            comboBoxBM.SelectedIndexChanged -= comboBoxBM_SelectedIndexChanged;
            sqlComm.CommandText = "SELECT ID, 部门名称 FROM 部门表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("部门表")) dSet.Tables.Remove("部门表");
            sqlDA.Fill(dSet, "部门表");

            DataRow drTemp = dSet.Tables["部门表"].NewRow();
            drTemp[0] = 0;
            drTemp[1] = "全部";
            dSet.Tables["部门表"].Rows.Add(drTemp);

            comboBoxBM.DataSource = dSet.Tables["部门表"];
            comboBoxBM.DisplayMember = "部门名称";
            comboBoxBM.ValueMember = "ID";
            comboBoxBM.SelectedValue = intUserBM;;
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;


            //初始化商品列表
            sqlComm.CommandText = "SELECT 销售商品制单明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.数量, 销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品定义表.库存成本价, 销售商品定义表.成本金额, 销售商品制单明细表.毛利, 销售商品制单明细表.赠品, 销售商品制单明细表.扣率, 销售商品制单明细表.实计金额, 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID, 商品表.库存数量, 销售商品定义表.统计标志 FROM 销售商品制单明细表 INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID LEFT OUTER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID CROSS JOIN 销售商品定义表 WHERE (销售商品制单明细表.ID = 0)";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[15].Visible = false;
            dataGridViewDJMX.Columns[16].Visible = false;
            dataGridViewDJMX.Columns[18].Visible = false;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[4].ReadOnly = true;
            dataGridViewDJMX.Columns[5].ReadOnly = true;
            dataGridViewDJMX.Columns[8].ReadOnly = true;
            dataGridViewDJMX.Columns[9].ReadOnly = true;
            dataGridViewDJMX.Columns[10].ReadOnly = true;
            dataGridViewDJMX.Columns[11].ReadOnly = true;
            dataGridViewDJMX.Columns[13].ReadOnly = true;
            dataGridViewDJMX.Columns[14].ReadOnly = true;
            dataGridViewDJMX.Columns[17].ReadOnly = true;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";

            if (intUserLimit < LIMITACCESS)
            {
                dataGridViewDJMX.Columns[9].Visible = false;
                dataGridViewDJMX.Columns[10].Visible = false;
                dataGridViewDJMX.Columns[11].Visible = false;

                labelCBHJ.Visible = false;
                labelMLHJ.Visible = false;
            }

            dataGridViewDJMX.ShowCellErrors = true;




            sqlConn.Close();

            //initHTDefault();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;



        }

        private void initDJ()
        {

            this.dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;
            int iBM = 0;
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 销售商品制单表.单据编号, 销售商品制单表.日期, 职员表.职员姓名 AS 业务员, 操作员.职员姓名 AS 操作员, 销售商品制单表.备注, 单位表.单位编号, 单位表.单位名称, 销售合同表.合同编号, 销售商品制单表.联系电话, 销售商品制单表.联系人, 销售商品制单表.收货人, 销售商品制单表.到站, 销售商品制单表.运输方式, 销售商品制单表.详细地址, 销售商品制单表.物流名称, 销售商品制单表.单号, 销售商品制单表.邮政编码, 销售商品制单表.部门ID, 销售商品制单表.BeActive FROM 销售商品制单表 INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 职员表 ON 销售商品制单表.业务员ID = 职员表.ID INNER JOIN 职员表 操作员 ON 销售商品制单表.操作员ID = 操作员.ID LEFT OUTER JOIN 销售合同表 ON 销售商品制单表.合同ID = 销售合同表.ID WHERE (销售商品制单表.ID = " + iDJID.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy年M月dd日");

                if (sqldr.GetValue(17).ToString() != "")
                {
                    try
                    {
                        iBM = int.Parse(sqldr.GetValue(17).ToString());
                    }
                    catch
                    {
                        iBM = 0;
                    }

                }
                if (!bool.Parse(sqldr.GetValue(18).ToString()))
                {
                    labelDJBH.ForeColor = Color.Red;
                }

                comboBoxYWY.Items.Add(sqldr.GetValue(2).ToString());
                comboBoxYWY.Text = sqldr.GetValue(2).ToString();
                labelCZY.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                textBoxDWBH.Text = sqldr.GetValue(5).ToString();
                textBoxDWMC.Text = sqldr.GetValue(6).ToString();

                textBoxHTH.Text = sqldr.GetValue(7).ToString();
                textBoxLXDH.Text = sqldr.GetValue(8).ToString();
                textBoxLXR.Text = sqldr.GetValue(9).ToString();
                textBoxSHR.Text = sqldr.GetValue(10).ToString();
                textBoxDZ.Text = sqldr.GetValue(11).ToString();
                comboBoxYSFS.Text = sqldr.GetValue(12).ToString();
                textBoxXXDZ.Text = sqldr.GetValue(13).ToString();
                textBoxWLMC.Text = sqldr.GetValue(14).ToString();
                textBoxDH.Text = sqldr.GetValue(15).ToString();
                textBoxYZBM.Text = sqldr.GetValue(16).ToString();


                this.Text = "销售出库制单：" + labelDJBH.Text;
            }
            sqldr.Close();
            
            /*
            comboBoxBM.SelectedIndexChanged -= comboBoxBM_SelectedIndexChanged;
            sqlComm.CommandText = "SELECT 部门表.部门名称 FROM 部门表 INNER JOIN 职员表 ON 部门表.ID = 职员表.岗位ID WHERE (职员表.职员姓名 = N'" + comboBoxYWY.Text + "')";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                comboBoxBM.Items.Add(sqldr.GetValue(0).ToString());
                comboBoxBM.Text = sqldr.GetValue(0).ToString();
                break;
            }
            sqldr.Close();
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;
             */
            if (iBM != 0)
            {
                comboBoxBM.SelectedIndexChanged -= comboBoxBM_SelectedIndexChanged;
                sqlComm.CommandText = "SELECT 部门名称 FROM 部门表 WHERE (ID = " + iBM.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    comboBoxBM.Items.Add(sqldr.GetValue(0).ToString());
                    comboBoxBM.Text = sqldr.GetValue(0).ToString();
                    break;
                }
                sqldr.Close();
                comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;
            }

            //初始化商品列表
            sqlComm.CommandText = "SELECT 销售商品制单明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库房表.库房编号, 库房表.库房名称, 销售商品制单明细表.数量, 销售商品制单明细表.单价, 销售商品制单明细表.金额, 销售商品制单明细表.库存成本价, 销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价 AS 成本金额, 销售商品制单明细表.毛利, 销售商品制单明细表.赠品, 销售商品制单明细表.扣率, 销售商品制单明细表.实计金额, 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID, 商品表.库存数量, 库房表.ID AS 统计标记 FROM 销售商品制单明细表 INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID INNER JOIN 库房表 ON 销售商品制单明细表.库房ID = 库房表.ID WHERE (销售商品制单明细表.表单ID = " + iDJID.ToString() + ")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
            
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[15].Visible = false;
            dataGridViewDJMX.Columns[16].Visible = false;
            dataGridViewDJMX.Columns[17].Visible = false;
            dataGridViewDJMX.Columns[18].Visible = false;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[7].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[8].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[9].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[10].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[11].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[14].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.ShowCellErrors = true;



            if (intUserLimit < LIMITACCESS)
            {
                dataGridViewDJMX.Columns[9].Visible = false;
                dataGridViewDJMX.Columns[10].Visible = false;
                dataGridViewDJMX.Columns[11].Visible = false;

                labelCBHJ.Visible = false;
                labelMLHJ.Visible = false;
            }
            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;

            dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.RowValidating -= dataGridViewDJMX_RowValidating;
            dataGridViewDJMX.CellDoubleClick -= dataGridViewDJMX_CellDoubleClick;
            

            sqlConn.Close();

            dataGridViewDJMX.CellPainting += dataGridViewDJMX_CellPainting;
            countAmount();

            if (intUserLimit < LIMITACCESS)
            {
                dataGridViewDJMX.Columns[9].Visible = false;
                dataGridViewDJMX.Columns[10].Visible = false;
                dataGridViewDJMX.Columns[11].Visible = false;
                labelCBHJ.Text = "";
                labelMLHJ.Text = "";
            }


            
        }



        private void comboBoxBM_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            sqlConn.Open();
            //初始化员工列表
            if (comboBoxBM.Text.Trim() != "全部")
                sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员姓名, 职员表.职员编号 FROM 职员表 INNER JOIN 部门表 ON 职员表.部门ID = 部门表.ID WHERE (部门表.部门名称 = N'" + comboBoxBM.Text.Trim() + "') AND (职员表.beactive = 1)";
            else
                sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员姓名, 职员表.职员编号 FROM 职员表 INNER JOIN 部门表 ON 职员表.部门ID = 部门表.ID WHERE (职员表.beactive = 1)";

            sqldr = sqlComm.ExecuteReader();
            if (!sqldr.HasRows)
            {
                sqldr.Close();
                sqlConn.Close();
                return;
            }
            sqldr.Close();

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";
            sqlConn.Close(); 
             */
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
                if (cGetInformation.iBMID != 0)
                    comboBoxBM.SelectedValue = cGetInformation.iBMID;

                comboBoxYWY.Text = cGetInformation.sCompanyYWY;
            }
            intHTH = 0;
            textBoxHTH.Text = "";
            getCompanyInfoDetail();
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(20, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    intHTH = 0;
                    textBoxHTH.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    if (cGetInformation.iBMID != 0)
                        comboBoxBM.SelectedValue = cGetInformation.iBMID;

                    comboBoxYWY.Text = cGetInformation.sCompanyYWY;
                }
                getCompanyInfoDetail();
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
                    if (cGetInformation.iBMID != 0)
                        comboBoxBM.SelectedValue = cGetInformation.iBMID;

                    comboBoxYWY.Text = cGetInformation.sCompanyYWY;
                }
                getCompanyInfoDetail();
            }
            /*
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(22, textBoxDWMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                    intHTH = 0;
                    textBoxHTH.Text = "";
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                }
                getCompanyInfoDetail();
            }
            */
        }

        private void getCompanyInfoDetail()
        {
            int iBM = 0;
            if (iSupplyCompany == 0)
            {
                textBoxDWBH.Text = "";
                textBoxDWMC.Text = "";
                textBoxLXR.Text = "";
                textBoxSHR.Text = "";
                textBoxLXDH.Text = "";
                textBoxXXDZ.Text = "";
                textBoxYZBM.Text = "";

                return;
            }
            comboBoxBM.SelectedIndex = 0;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 单位编号, 单位名称, 税号, 收货电话, 开户银行, 银行账号, 联系人, 地址, 邮编, 联系地址, 收货人, 地址, 到站名称, 业务员, 部门ID FROM 单位表 WHERE (ID = " + iSupplyCompany.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                textBoxDWBH.Text = sqldr.GetValue(0).ToString();
                textBoxDWMC.Text = sqldr.GetValue(1).ToString();
                textBoxLXR.Text = sqldr.GetValue(6).ToString();
                textBoxSHR.Text = sqldr.GetValue(10).ToString();
                textBoxLXDH.Text = sqldr.GetValue(3).ToString();
                textBoxXXDZ.Text = sqldr.GetValue(7).ToString();
                textBoxYZBM.Text = sqldr.GetValue(8).ToString();
                textBoxDZ.Text = sqldr.GetValue(12).ToString();
                comboBoxYWY.Text = sqldr.GetValue(13).ToString().Trim();

                try
                {
                    iBM = int.Parse(sqldr.GetValue(14).ToString().Trim());
                }
                catch
                {
                    iBM = 0;
                }
                comboBoxBM.SelectedValue = iBM;

            }

            sqldr.Close();

            /*
            comboBoxBM.SelectedIndexChanged -= comboBoxBM_SelectedIndexChanged;
            sqlComm.CommandText = "SELECT 部门表.部门名称 FROM 部门表 INNER JOIN 职员表 ON 部门表.ID = 职员表.岗位ID WHERE (职员表.职员姓名 = N'" + comboBoxYWY.Text + "')";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                comboBoxBM.Text = sqldr.GetValue(0).ToString();
                break;
            }
            sqldr.Close();
            comboBoxBM.SelectedIndexChanged += comboBoxBM_SelectedIndexChanged;
             */

            sqlConn.Close();
        }

        private void textBoxHTH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getBillInformation(52, textBoxHTH.Text.Trim()) == 0)
            {
                textBoxHTH.Text = "";
                intHTH = 0;
                return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iBillCNumber;
                textBoxDWMC.Text = cGetInformation.strBillCName;
                textBoxDWBH.Text = cGetInformation.strBillCCode;
                textBoxHTH.Text = cGetInformation.strBillCode;
                intHTH = cGetInformation.iBillNumber;

                getCompanyInfoDetail();
                getHTDetail();
                dataGridViewDJMX.Focus();
            }
        }

        private void getHTDetail()
        {
            if (intHTH == 0)
                return;

            bCheck = false;

            sqlConn.Open();

            sqlComm.CommandText = "SELECT 销售合同明细表.ID, 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库房表.库房编号, 库房表.库房名称, 销售合同明细表.数量, 销售合同明细表.单价, 销售商品制单明细表.金额, 商品表.库存成本价, 销售商品定义表.成本金额, 销售商品制单明细表.毛利, 销售商品制单明细表.赠品, 销售商品制单明细表.扣率, 销售商品制单明细表.实计金额,  销售合同明细表.商品ID, 销售商品制单明细表.库房ID, 商品表.库存数量 , 销售商品定义表.统计标志 FROM 库房表 INNER JOIN 销售商品制单明细表 ON  库房表.ID = 销售商品制单明细表.库房ID RIGHT OUTER JOIN  销售商品定义表 ON 销售商品制单明细表.ID = 销售商品定义表.ID CROSS JOIN 商品表 INNER JOIN  销售合同明细表 ON 商品表.ID = 销售合同明细表.商品ID WHERE (销售合同明细表.销售合同ID = " + intHTH.ToString() + ")";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];
            sqlConn.Close();

            //得到库房
            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                cGetInformation.iCommNumber = Convert.ToInt32(dataGridViewDJMX.Rows[i].Cells[15].Value);
                cGetInformation.getCommKF();

                dataGridViewDJMX.Rows[i].Cells[4].Value = cGetInformation.strKFCode;
                dataGridViewDJMX.Rows[i].Cells[5].Value = cGetInformation.strKFName;
                dataGridViewDJMX.Rows[i].Cells[16].Value = cGetInformation.iKFNumber;

            }

            countAmount();

            if (dataGridViewDJMX.Rows.Count > 0)
                dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[dataGridViewDJMX.Rows.Count - 1].Cells[1];
            bCheck = true;
        }

        private bool countAmount()
        {
            decimal fSum = 0;
            decimal fSum1 = 0;
            decimal fCSum = 0;

            decimal fCB= 0;
            decimal fML= 0;
            
            decimal fTemp, fTemp1;
            decimal fCount = 0;
            bool bCheck = true;

            isLimit = true;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[1].ErrorText = "输入所售商品";
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "输入所售商品";
                    bCheck = false;
                }

                if (dataGridViewDJMX.Rows[i].Cells[16].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[16].Value.ToString() == "0")
                {
                    dataGridViewDJMX.Rows[i].Cells[4].ErrorText = "输入所售商品库房";
                    dataGridViewDJMX.Rows[i].Cells[5].ErrorText = "输入所售商品库房";
                    bCheck = false;
                }


                if (dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[6].ErrorText = "输入所售商品数量";
                    bCheck = false;
                }

                if (dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[7].ErrorText = "输入所售商品价格";
                    bCheck = false;
                }

                if (!bCheck)
                    continue;

                //赠品
                if (dataGridViewDJMX.Rows[i].Cells[12].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[12].Value = 0;

                //库存成本
                if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[9].Value = 0;

                //扣率
                if (dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[13].Value = 100;

                //库存量
                if (dataGridViewDJMX.Rows[i].Cells[17].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[17].Value = 0;

                //颜色表示
                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value) < Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value))
                {
                    dataGridViewDJMX.Rows[i].Cells[7].Style.BackColor = Color.LightPink;
                    isLimit = false;
                }
                else
                    dataGridViewDJMX.Rows[i].Cells[7].Style.BackColor = Color.White;

                if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value) > Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[17].Value))
                    dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.LightPink;
                else
                    dataGridViewDJMX.Rows[i].Cells[6].Style.BackColor = Color.White;

                
                //数量
                fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                //单价
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);

                //金额
                if (Convert.ToBoolean(dataGridViewDJMX.Rows[i].Cells[12].Value.ToString())) //赠品
                {
                    //金额
                    dataGridViewDJMX.Rows[i].Cells[8].Value = 0;
                }
                else
                {
                    dataGridViewDJMX.Rows[i].Cells[8].Value = Math.Round(fTemp * fTemp1, 2);
                }
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                //成本金额
                dataGridViewDJMX.Rows[i].Cells[10].Value = Math.Round(fTemp * fTemp1, 2);
                //毛利
                dataGridViewDJMX.Rows[i].Cells[11].Value = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value.ToString()) - Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value.ToString());

                //实计
                dataGridViewDJMX.Rows[i].Cells[14].Value = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value.ToString()) * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[13].Value.ToString())/100;


                fCount += 1;
                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[14].Value);
                fCSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);

                fCB += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                fML += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value);

            }
            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelJEHJ.Text = fSum.ToString("f2");
            labelSJJE.Text = fSum1.ToString("f2");
            labelSLHJ.Text = fCSum.ToString("f0");
            toolStripStatusLabelMXJLS.Text = fCount.ToString("f0");
            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);

            labelCBHJ.Text = fCB.ToString("f2");
            labelMLHJ.Text = fML.ToString("f2");

            if (fML < 0)
            {
                labelMLHJ.ForeColor = Color.Red;
            }
            else
            {
                labelMLHJ.ForeColor = Color.Black;
            }

            return bCheck;

        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2) //商品编号
            {
                if (cGetInformation.getCommInformation(1, "") == 0) //失败
                {
                    return;
                }
                else
                {
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value = cGetInformation.iCommNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommKCCBJ;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value = cGetInformation.iKFNumber;

                    dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommPFJ;
                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value.ToString() == "")
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value = 0;

                    cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value));
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[17].Value = cGetInformation.dKCL;

                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[4];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }
            /*
            if (e.ColumnIndex == 5 || e.ColumnIndex == 4) //库房编号
            {
                if (cGetInformation.getKFInformation(1, "") == 0) //失败
                {
                    return;
                }
                else
                {
                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value = cGetInformation.iKFNumber;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;

                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value.ToString() == "")
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value = 0;

                    cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value));
                    dataGridViewDJMX.Rows[e.RowIndex].Cells[17].Value = cGetInformation.dKCL;
                    dataGridViewDJMX.EndEdit();
                    dataGridViewDJMX.CurrentCell = dataGridViewDJMX.Rows[e.RowIndex].Cells[6];
                    dataGridViewDJMX.BeginEdit(true);
                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                }
            }
            */

        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数据输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
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
                                    case 2:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[6];
                                        break;
                                    case 4:
                                    case 5:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[6];
                                        break;
                                    case 6:
                                        dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[7];
                                        break;
                                    case 7:
                                        //dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex].Cells[13];
                                        //break;
                                    //case 13:
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
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dataGridViewDJMX_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            countAmount();
        }

        private void dataGridViewDJMX_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int iRe = 0;

            if (!bCheck)
                return;
            if (isSaved)
                return;

            if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
                return;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDJMX);
            switch (e.ColumnIndex)
            {
                case 2: //商品编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[17].Value = 0;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }

                    iRe = cGetInformation.getCommInformation(20, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewDJMX.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //失败
                    //if (cGetInformation.getCommInformation(20, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].ErrorText = "商品编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        } 
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommKCCBJ;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value = cGetInformation.iKFNumber;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommPFJ;


                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[17].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;

                case 1: //商品名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[17].Value = 0;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                        break;

                    }

                    iRe = cGetInformation.getCommInformation(10, e.FormattedValue.ToString());
                    if (iRe == -1) //cancel
                    {
                        dataGridViewDJMX.CancelEdit();
                        return;
                    }

                    if (iRe == 0) //失败
                    //if (cGetInformation.getCommInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].ErrorText = "商品助记码输入错误";
                    }
                    else
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value.ToString() == cGetInformation.iCommNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        } 
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value = cGetInformation.iCommNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[2].Value = cGetInformation.strCommCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[1].Value = cGetInformation.strCommName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[3].Value = cGetInformation.strCommGG;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value = cGetInformation.decCommKCCBJ;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value = cGetInformation.iKFNumber;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = cGetInformation.decCommPFJ;


                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[17].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                    }
                    break;
                case 4: //库房编号
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[17].Value = "";

                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(10, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].ErrorText = "库房编号输入错误";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;

                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[17].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }
                    break;
                case 5: //库房名称
                    if (e.FormattedValue.ToString() == "")
                    {
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value = 0;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = "";
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[17].Value = "";
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;

                    }
                    if (cGetInformation.getKFInformation(20, e.FormattedValue.ToString()) == 0) //失败
                    {
                        e.Cancel = true;
                        //dataGridViewDJMX.CancelEdit();
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].ErrorText = "库房助记码输入错误";
                    }
                    else
                    {

                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value.ToString() == cGetInformation.iKFNumber.ToString())
                        {
                            this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                            break;
                        }
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value = cGetInformation.iKFNumber;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strKFCode;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strKFName;
                        if (dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value.ToString() == "")
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value = 0;

                        cGetInformation.getKCL(Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value), Convert.ToInt32(dataGridViewDJMX.Rows[e.RowIndex].Cells[16].Value));
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[17].Value = cGetInformation.dKCL;
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;

                    }

                    break;
                case 6:  //商品数量
                    int intOut = 0;
                    if (e.FormattedValue.ToString() == "") break;
                    if (Int32.TryParse(e.FormattedValue.ToString(), out intOut))
                    {
                        if (intOut <= 0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "商品数量输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[6].ErrorText = "商品数量输入类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 7: //商品价格
                    decimal detOut = 0;

                    if (e.FormattedValue.ToString() == "") break;
                    if (dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value.ToString() == "" || dataGridViewDJMX.Rows[e.RowIndex].Cells[15].Value.ToString() == "0")
                    {
                        MessageBox.Show("请先输入购进商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = Math.Round(Decimal.Zero, 2);
                        this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                        break;
                    }

                    if (Decimal.TryParse(e.FormattedValue.ToString(), out detOut))
                    {
                        if (detOut >= 0)
                        {
                            detOut = Math.Round(detOut, 2);

                            if (detOut.CompareTo(dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value) <= 0)
                            {
                                if (MessageBox.Show("商品价格低于库存成本价，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                                    e.Cancel = true;
                                else
                                {
                                    this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
                                    dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value = detOut;
                                    this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
                                }

                            }
                        }
                        else
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "商品价格输入错误";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[7].ErrorText = "商品数量价格类型错误";
                        e.Cancel = true;
                    }
                    break;
                case 13:  //扣率
                    double dOut = 0.0;
                    if (e.FormattedValue.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[11].Value = 100;
                        break;
                    }
                    if (Double.TryParse(e.FormattedValue.ToString(), out dOut))
                    {
                        if (dOut <= 0 || dOut > 100.0)
                        {
                            dataGridViewDJMX.Rows[e.RowIndex].Cells[13].ErrorText = "商品扣率输入错误，请输入0.01-100.0之间的数字";
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        dataGridViewDJMX.Rows[e.RowIndex].Cells[13].ErrorText = "商品扣率输入错误，请输入0.01-100.0之间的数字";
                        e.Cancel = true;
                    }
                    break;

                default:
                    break;

            }
            dataGridViewDJMX.EndEdit();

        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal dKUL = 0, dKCCBJ = 0, dML = 0, dSJJE = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dML1 = 0, dSJJE1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;

            //保存完毕
            if (isSaved)
            {
                MessageBox.Show("销售出库制单已经保存,单据号为：" + labelDJBH.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (iSupplyCompany == 0)
            {
                MessageBox.Show("请选择销售单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("销售出库制单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (toolStripStatusLabelMXJLS.Text == "0")
            {
                MessageBox.Show("没有销售出库制单商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string strLimitPass = "";
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 权限码 FROM 权限码表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                strLimitPass = sqldr.GetValue(0).ToString();
                break;
            }
            sqldr.Close();
            sqlConn.Close();

            if (strLimitPass.Trim() != "")
            {
                if (!isLimit && intUserLimit < LIMITACCESS) //权限管理
                {
                    FormLACCESS frmLACCESS = new FormLACCESS();
                    frmLACCESS.strPass = strLimitPass.Trim();
                    frmLACCESS.ShowDialog();
                    if (!frmLACCESS.isAccept)
                        return;
                }
            }

            //if (MessageBox.Show("请检查销售出库制单内容,是否继续保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            //    return;


            saveToolStripButton.Enabled = false;
            string strCount = "", strDateSYS = "", strKey = "BKP";
            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //得到表单号
                //得到服务器日期
                sqlComm.CommandText = "SELECT GETDATE() AS 日期";
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                }
                sqldr.Close();

                //得到日期
                sqlComm.CommandText = "SELECT 时间 FROM 表单计数表 WHERE (时间 = CONVERT(DATETIME, '" + strDateSYS + " 00:00:00', 102))";
                sqldr = sqlComm.ExecuteReader();

                if (sqldr.HasRows)
                    sqldr.Close();
                else //服务器时间不吻合
                {
                    sqldr.Close();
                    //修正日期及计数器
                    sqlComm.CommandText = "UPDATE 表单计数表 SET 时间 = '" + strDateSYS + "', 计数 = 1";
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


                if (strCount != "")
                {
                    if (iVersion <= 0)
                    {
                        if (int.Parse(strCount) > 2)
                        {
                            MessageBox.Show("预览版用户每天只可以做两单", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            sqlConn.Close();
                            return;
                        }
                    }

                    strCount = string.Format("{0:D3}", Int32.Parse(strCount));
                    strCount = strKey.ToUpper() + Convert.ToDateTime(strDateSYS).ToString("yyyyMMdd") + strCount;
                }
                else
                {
                    MessageBox.Show("数据错误", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sqlConn.Close();
                    return;
                }

                string sBMID = "NULL";
                if (comboBoxBM.SelectedValue.ToString() != "0")
                    sBMID = comboBoxBM.SelectedValue.ToString();

                //表单汇总
                sqlComm.CommandText = "INSERT INTO 销售商品制单表 (单据编号, 单位ID, 日期, 业务员ID, 操作员ID, 合同ID, 价税合计, 联系电话, 联系人, 收货人, 到站, 运输方式, 详细地址, 物流名称, 单号, 邮政编码, 备注, 出库标记, BeActive, 未付款金额, 已付款金额, 部门ID) VALUES (N'" + strCount + "', " + iSupplyCompany.ToString() + ", '" + strDateSYS + "', " + comboBoxYWY.SelectedValue.ToString() + ", " + intUserID.ToString() + ", " + intHTH.ToString() + ", " + labelSJJE.Text + ", N'" + textBoxLXDH.Text.Trim() + "', N'" + textBoxLXR.Text.Trim() + "', N'" + textBoxSHR.Text.Trim() + "', N'" + textBoxDZ.Text.Trim() + "', N'" + comboBoxYSFS.Text.Trim() + "', N'" + textBoxXXDZ.Text.Trim() + "', N'" + textBoxWLMC.Text.Trim() + "', N'" + textBoxDH.Text.Trim() + "', N'" + textBoxYZBM.Text.Trim() + "', N'" + textBoxBZ.Text.Trim() + "', 0, 1, " + labelSJJE.Text + " , 0, " + sBMID + ")";
                sqlComm.ExecuteNonQuery();


                //取得单据号 
                sqlComm.CommandText = "SELECT @@IDENTITY";
                sqldr = sqlComm.ExecuteReader();
                sqldr.Read();
                string sBillNo = sqldr.GetValue(0).ToString();
                sqldr.Close();


                //相关合同结束
                if (intHTH != 0)
                {
                    sqlComm.CommandText = "UPDATE 销售合同表 SET 执行标记 = 1 WHERE (ID = " + intHTH.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                //单位应收账
                sqlComm.CommandText = "SELECT 应收账款 FROM 单位表 WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();


                dKCJE = 0;
                while (sqldr.Read())
                {
                    dKCJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                }
                sqldr.Close();
                dKCJE1 = dKCJE + Convert.ToDecimal(labelSJJE.Text);
                sqlComm.CommandText = "UPDATE 单位表 SET 应收账款 = " + dKCJE1.ToString() + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //单位历史纪录
                sqlComm.CommandText = "INSERT INTO 单位历史账表 (单位ID, 日期, 单据编号, 摘要, 销出金额, 应收金额, 销售标记, 业务员ID, 冲抵单号, BeActive, 部门ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + strCount + "', N'销售商品制单', " + labelSJJE.Text.ToString() + ", " + dKCJE1.ToString() + ", 1, " + comboBoxYWY.SelectedValue.ToString() + ", N'" + textBoxHTH.Text + "', 1,"+sBMID+")";
                sqlComm.ExecuteNonQuery();


                //单据明细
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    sqlComm.CommandText = "INSERT INTO 销售商品制单明细表 (表单ID, 商品ID, 库房ID, 数量, 单价, 金额, 扣率, 实计金额, 毛利, 赠品, 未出库数量, 已出库数量, BeActive, 校对标志, 未付款金额, 已付款金额, 未付款数量, 已付款数量, 库存成本价) VALUES (" + sBillNo + ", " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[16].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[7].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[13].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() + ", " + Convert.ToInt32(dataGridViewDJMX.Rows[i].Cells[12].Value).ToString() + ", " + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ", 0, 1, 0, " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ",0," + dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() + ",0," + dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }



               //标志复位
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    dataGridViewDJMX.Rows[i].Cells[18].Value = 1;
                }

                //总库存
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[18].Value) == 0) //已经计算过
                        continue;

                    //计算该单的每个商品库存
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                    dKCJE1=Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                    dML = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value);
                    dSJJE1=Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[14].Value);
                    dZZJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);

                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[18].Value) == 0) //已经计算过
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[15].Value == dataGridViewDJMX.Rows[i].Cells[15].Value) //同种商品
                        {
                            dKUL1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[6].Value);
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[10].Value); ;
                            dZZJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[7].Value);
                            dML += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[11].Value);
                            dSJJE1+=Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[14].Value);

                            dataGridViewDJMX.Rows[j].Cells[18].Value = 0;
                        }

                    }
                    //dYSYE1 = dKCJE1;
                    dYSYE1 = dSJJE1;

                    //总库存变更
                    sqlComm.CommandText = "SELECT 库存数量, 库存成本价, 最高进价, 最低进价, 最终进价, 库存金额, 应收金额  FROM 商品表 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(5).ToString());
                        dYSYE = Convert.ToDecimal(sqldr.GetValue(6).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                    }
                    sqldr.Close();

                    //余额
                    dYSYE += dYSYE1;

                    dKUL -= dKUL1;
                    //dKCJE -= dKCJE1;
                    dKCJE = dKUL * dKCCBJ;
                    sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = " + dKUL.ToString() + ", 库存金额=" + dKCJE.ToString() + ", 应收金额=" + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //总账历史纪录
                    sqlComm.CommandText = "INSERT INTO 商品历史账表 (日期, 商品ID, 单位ID, 业务员ID, 原单据编号, 单据编号, 摘要, 销售数量, 销售单价, 销售金额, 出库数量, 出库单价, 出库金额, 毛利, BeActive, 总结存数量, 总结存金额, 应收金额, 部门ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + textBoxHTH.Text + "', N'" + strCount + "', N'销售商品制单', " + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", " + dSJJE1.ToString() + ", " + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", " + dSJJE1.ToString() + ", " + dML.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + "," + dYSYE.ToString() + ","+sBMID+")";
                    sqlComm.ExecuteNonQuery();

                }

                //标志复位
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    dataGridViewDJMX.Rows[i].Cells[18].Value = 1;
                }

                //分库存
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[18].Value) == 0) //已经计算过
                        continue;

                    //计算该单的每个商品数量
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                    dML = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value);
                    dSJJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[14].Value);
                    dZZJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[7].Value);
                    for (j = i + 1; j < dataGridViewDJMX.Rows.Count; j++)
                    {
                        if (dataGridViewDJMX.Rows[j].IsNewRow)
                            continue;

                        if (Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[18].Value) == 0) //已经计算过
                            continue;

                        if (dataGridViewDJMX.Rows[j].Cells[15].Value == dataGridViewDJMX.Rows[i].Cells[15].Value && dataGridViewDJMX.Rows[j].Cells[16].Value == dataGridViewDJMX.Rows[i].Cells[16].Value) //同种商品，同样库存
                        {
                            dKUL1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[6].Value);
                            dKCJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[10].Value); ;
                            dZZJJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[7].Value);
                            dML += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[11].Value);
                            dSJJE1 += Convert.ToDecimal(dataGridViewDJMX.Rows[j].Cells[14].Value);

                            dataGridViewDJMX.Rows[j].Cells[18].Value = 0;
                        }

                    }
                    dYSYE1 = dSJJE1;



                    //分库存更新
                    sqlComm.CommandText = "SELECT 库存数量, 库存金额, 库存成本价, 应收金额 FROM 库存表 WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[16].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ") AND (BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows) //存在库存
                    {
                        sqldr.Read();
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                        dKCCBJ1= Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        
                        sqldr.Close();
                        //余额
                        dYSYE += dYSYE1;

                        dKUL -= dKUL1;
                        //dKCJE -= dKCJE1;
                        dKCJE = dKCCBJ1 * dKUL;
                        sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = " + dKUL.ToString() + "  ,库存金额=" + dKCJE.ToString() + ", 应收金额=" + dYSYE.ToString() + "WHERE (库房ID = " + dataGridViewDJMX.Rows[i].Cells[16].Value.ToString() + ") AND (商品ID = " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ") AND (BeActive = 1)";
                        sqlComm.ExecuteNonQuery();

                        //库房账历史纪录
                        sqlComm.CommandText = "INSERT INTO 商品库房历史账表 (库房ID, 日期, 商品ID, 单位ID, 业务员ID, 原单据编号, 单据编号, 摘要, 销售数量, 销售单价, 销售金额, 出库数量, 出库单价, 出库金额, 毛利, BeActive, 库房结存数量, 库房结存金额, 应收金额, 部门ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[16].Value.ToString() + ", '" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + comboBoxYWY.SelectedValue.ToString() + ", N'" + textBoxHTH.Text + "', N'" + strCount + "', N'销售商品制单', " + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", " + dSJJE1.ToString() + ", " + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", " + dSJJE1.ToString() + ", " + dML.ToString() + ", 1," + dKUL.ToString() + "," + dKCJE.ToString() + "," + dYSYE.ToString() + ","+sBMID+")";
                        sqlComm.ExecuteNonQuery();

                    }

                }

                
                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                saveToolStripButton.Enabled = true;
                return;
            }
            finally
            {
                sqlConn.Close();
            }

            labelDJBH.Text = strCount;
            this.Text = "销售出库制单：" + labelDJBH.Text;
            isSaved = true;

            /*
            if (MessageBox.Show("销售出库制单保存成功,是否打印单据？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {

                toolStripButtonPrnFHD_Click(null, null);
                toolStripButtonPrnZXD_Click(null,null);

            }

            bool bClose = false;
            if (MessageBox.Show("销售出库制单保存成功，是否关闭单据窗口？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                bClose = true;
            }

            if (MessageBox.Show("是否继续开始另一份单据？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                MDIBusiness mdiT = (MDIBusiness)this.MdiParent;
                mdiT.销售出库制单BToolStripMenuItem_Click(null, null);
            }

            if (bClose)
                this.Close();
             */

            FormMessage frmMessage = new FormMessage();
            frmMessage.labelWarn.Text = "是否继续开始另一份单据？";
            frmMessage.checkBox1.Text = "打印发货单";
            frmMessage.checkBox2.Text = "打印装箱单";
            frmMessage.checkBox3.Text = "关闭单据窗口";
            frmMessage.checkBox3.Checked = true;

            frmMessage.ShowDialog();
            if (frmMessage.checkBox1.Checked)
            {
                toolStripButtonPrnFHD_Click(null, null);
                MessageBox.Show("发货单打印完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
            }
            if (frmMessage.checkBox2.Checked)
            {
                //toolStripButtonPrnZXD_Click(null, null);
                toolStripButtonZXDNew_Click(null, null);
                MessageBox.Show("装箱单打印完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            if (frmMessage.bOK)
            {
                MDIBusiness mdiT = (MDIBusiness)this.MdiParent;
                mdiT.销售出库制单BToolStripMenuItem_Click(null, null);
            }

            if (frmMessage.checkBox3.Checked)
                this.Close();
            
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("销售出库制单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "销售出库制单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //格式确认
            if (!countAmount())
            {
                MessageBox.Show("销售出库制单明细格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string strT = "销售出库制单(单据编号:" + labelDJBH.Text + ");制单日期：" + labelZDRQ.Text + ";业　务员：" + comboBoxYWY.Text + ";单位名称：" + textBoxDWMC.Text + ";价税合计：" + labelSJJE.Text + "(大写:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void toolStripButtonPrnFHD_Click(object sender, EventArgs e)
        {
            try
            {

                if (ppd.ShowDialog() != DialogResult.OK)
                    return;

                // Showing the Print Preview Page
                printDoc.BeginPrint += PrintDoc_BeginPrintFHD;
                printDoc.PrintPage += PrintDoc_PrintPageFHD;

                // Printing the Documnet
                printDoc.Print();
                printDoc.BeginPrint -= PrintDoc_BeginPrintFHD;
                printDoc.PrintPage -= PrintDoc_PrintPageFHD;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }


        private void PrintDoc_BeginPrintFHD(object sender, System.Drawing.Printing.PrintEventArgs e)
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

        private void PrintDoc_PrintPageFHD(object sender, System.Drawing.Printing.PrintPageEventArgs e)
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


            System.Drawing.Font _Font22 = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12 = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9 = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12U = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            int iTopM = 90;
            int iLeftM = 160;
            int iWidth1 = 170;
            int iWidth2 = 110;
            int iWidth3 = 100;
            int iWidth4 = 90;
            int iWidth5 = 180;

            int iHeight = 45;

            if (dSetP1.Tables.Contains("PRN1"))
            {
                iTopM = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][0].ToString());
                iLeftM = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][1].ToString());
                iWidth1 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][2].ToString());
                iWidth2 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][3].ToString());
                iWidth3 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][4].ToString());
                iWidth4 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][5].ToString());
                iWidth5 = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][6].ToString());

                iHeight = Convert.ToInt32(dSetP1.Tables["PRN1"].Rows[0][7].ToString());
            }


            Brush b = new SolidBrush(Color.Black);

            try
            {
                //发货方式
                e.Graphics.DrawString(comboBoxYSFS.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM, iWidth1, iHeight), StrFormatL);

                //到站
                e.Graphics.DrawString(textBoxDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2, iTopM, iWidth3, iHeight), StrFormatL);

                //发货时间
                e.Graphics.DrawString(labelZDRQ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2 + iWidth3 + iWidth4, iTopM, iWidth5, iHeight), StrFormatL);
                //e.Graphics.DrawString(labelZDRQ.Text, _Font12, b, (decimal)(iLeftM + iWidth1 + iWidth2 + iWidth3 + iWidth4), (decimal)(iTopM), StrFormatL);

                //收货单位
                e.Graphics.DrawString(textBoxDWMC.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM + iHeight, iWidth1 + iWidth2 + iWidth3 + iWidth4 + iWidth5, iHeight), StrFormatL);

                //收货地址
                e.Graphics.DrawString(textBoxXXDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM + 2 * iHeight, iWidth1 + iWidth2 + iWidth3, iHeight), StrFormatL);

                //邮编
                e.Graphics.DrawString(textBoxYZBM.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2 + iWidth3 + iWidth4, iTopM + 2 * iHeight, iWidth5, iHeight), StrFormatL);
                //e.Graphics.DrawString(textBoxYZBM.Text, _Font12, b, (decimal)(iLeftM + iWidth1 + iWidth2 + iWidth3 + iWidth4), (decimal)(iTopM + 2 * iHeight), StrFormatL);

                //收货人
                e.Graphics.DrawString(textBoxSHR.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM + 3 * iHeight, iWidth1, iHeight), StrFormatL);


                //联系电话
                e.Graphics.DrawString(textBoxLXDH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2, iTopM + 3 * iHeight, iWidth3+iWidth4+iWidth5, iHeight), StrFormatL);


                //发货人
                e.Graphics.DrawString(comboBoxYWY.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM, iTopM + 6 * iHeight+30, iWidth3 + iWidth4 + iWidth5, iHeight), StrFormatL);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            int i,j;
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


            System.Drawing.Font _Font22 = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12 = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9 = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12U = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            int iTopM1 = 100;
            int iLeftM1 = 80;
            int iHeight1 = 22;
            int iWidth1 = 700;
            int iWidth2 = 500;


            int iTopM = 218;
            int iLeftM = 95;
            int iLeftM2 = 545;
            int iWidth01 = 220;
            int iWidth02 = 120;
            int iWidth03 = 30;
            int iHeight2 = 40;


            int iHeight12 = 22;
            int iHeight22 = 50;
            int iHeight9 = 17;

            int iLM1 = 60;
            int iLM2 = 460;
            int iLM3 = 710;

            int iX1 = 430;
            int iY1 = 580;
            int iX2 = 200;
            int iY2 = 22;
            int iX3 = 760;
            
            if (dSetP1.Tables.Contains("PRN2"))
            {

                iTopM1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][0].ToString());
                iLeftM1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][1].ToString());
                iHeight1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][2].ToString());
                iWidth1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][3].ToString());
                iWidth2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][4].ToString());
                
                iTopM = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][5].ToString());
                iLeftM = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][6].ToString());
                iLeftM2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][7].ToString());
                iWidth01 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][8].ToString());
                iWidth02 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][9].ToString());
                iWidth03 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][10].ToString());
                iHeight2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][11].ToString());


                iHeight12 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][12].ToString());
                iHeight22 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][13].ToString());
                iHeight9 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][14].ToString());

                iLM1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][15].ToString());
                iLM2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][16].ToString());
                iLM3 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][17].ToString());

                iX1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][18].ToString());
                iY1 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][19].ToString());
                iX2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][20].ToString());
                iY2 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][21].ToString());
                iX3 = Convert.ToInt32(dSetP1.Tables["PRN2"].Rows[0][22].ToString());


            }
            




            //decimal fTemp = 0;
            int iTemp = 0;

            Brush b = new SolidBrush(Color.Black);
  

            try
            {
                e.Graphics.DrawString("单据编号（" + labelDJBH.Text + "）", _Font12, b, new System.Drawing.RectangleF(iLeftM1, iTopM1, iWidth1, iHeight1), StrFormatL);

                e.Graphics.DrawString(textBoxDWMC.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM1, iTopM1 + iHeight1, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(textBoxSHR.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM2, iTopM1 + iHeight1, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(textBoxLXDH.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM3, iTopM1 + iHeight1, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(textBoxXXDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM1, iTopM1 + iHeight1 * 2, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(comboBoxYSFS.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM2, iTopM1 + iHeight1 * 2, iWidth2, iHeight1), StrFormatL);
                e.Graphics.DrawString(textBoxDZ.Text, _Font12, b, new System.Drawing.RectangleF(iLeftM1 + iLM3, iTopM1 + iHeight1 * 2, iWidth2, iHeight1*2), StrFormatL);
                //e.Graphics.DrawString(comboBoxYSFS.Text, _Font12, b,(decimal)(iLeftM1 + iLM2), (decimal)(iTopM1 + iHeight1 * 2),StrFormatL);
                //e.Graphics.DrawString(textBoxDZ.Text, _Font12, b, (decimal)(iLeftM1 + iLM3), (decimal)(iTopM1 + iHeight1 * 2), StrFormatL);

                for (i = 0; i < 20; i++)
                {
                    if (RowPos >= dataGridViewDJMX.Rows.Count)
                    {
                        NewPage = false;
                        break;
                    }

                    if (dataGridViewDJMX.Rows[RowPos].IsNewRow)
                    {
                        NewPage = false;
                        break;
                    }

                    if (i < 10)
                    {
                        j = i;
                        iTemp = iLeftM;
                    }
                    else
                    {
                        j = i - 10;
                        iTemp = iLeftM2;
                    }

                    e.Graphics.DrawString(dataGridViewDJMX.Rows[RowPos].Cells[1].Value.ToString(), _Font12, b, new System.Drawing.RectangleF(iTemp, iTopM+j*iHeight2, iWidth01, iHeight2), StrFormatL);

                    e.Graphics.DrawString(Convert.ToDecimal(dataGridViewDJMX.Rows[RowPos].Cells[6].Value.ToString()).ToString("f0"), _Font12, b, new System.Drawing.RectangleF(iTemp+iWidth01, iTopM + j * iHeight2, iWidth02, iHeight2), StrFormatL);


                    RowPos++;
                }

                e.Graphics.DrawString(labelZDRQ.Text, _Font12, b, new System.Drawing.RectangleF(iX1, iY1, iX2, iY2), StrFormatL);
                e.Graphics.DrawString(labelZDRQ.Text, _Font12, b, new System.Drawing.RectangleF(iX3, iY1, iX2, iY2), StrFormatL);



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


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
             
        }

        private void toolStripButtonPrnZXD_Click(object sender, EventArgs e)
        {

            try
            {

                if (ppd.ShowDialog() != DialogResult.OK)
                    return;
                //printDoc.DefaultPageSettings.PaperSize = printDoc.PrinterSettings.PaperSizes[2]; 
                foreach (System.Drawing.Printing.PaperSize ps in printDoc.PrinterSettings.PaperSizes)
                {
                 if(ps.PaperName=="A3")
                 {
                  printDoc.PrinterSettings.DefaultPageSettings.PaperSize=ps;
                  printDoc.DefaultPageSettings.PaperSize=ps;
                 }
                }
                // Showing the Print Preview Page
                printDoc.BeginPrint += PrintDoc_BeginPrint;
                printDoc.PrintPage += PrintDoc_PrintPage;

                ppw.Width = 1000;
                ppw.Height = 800;

                /*
                if (ppw.ShowDialog() != DialogResult.OK)
                {
                    printDoc.BeginPrint -= PrintDoc_BeginPrint;
                    printDoc.PrintPage -= PrintDoc_PrintPage;
                    return;
                }
                 */


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

        private void FormXSCKZD_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
            {
                return;
            }

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

        private void textBoxHTH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {

                if (cGetInformation.getBillInformation(512, textBoxHTH.Text.Trim()) == 0)
                {
                    textBoxHTH.Text = "";
                    intHTH = 0;
                    return;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iBillCNumber;
                    textBoxDWMC.Text = cGetInformation.strBillCName;
                    textBoxDWBH.Text = cGetInformation.strBillCCode;
                    textBoxHTH.Text = cGetInformation.strBillCode;
                    intHTH = cGetInformation.iBillNumber;
                    
                    getCompanyInfoDetail();
                    getHTDetail();
                    dataGridViewDJMX.Focus();
                }

            }
        }

        private void toolStripMenuItemUP_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender;
            ContextMenuStrip cmenu = (ContextMenuStrip)menuitem.GetCurrentParent();


            DataGridView dv = (DataGridView)(cmenu.SourceControl);

            if (dv.CurrentCell.RowIndex <= 0 || dv.Rows.Count <= 1 || dv.Rows[dv.CurrentCell.RowIndex].IsNewRow)
                return;

            int i, count = dv.ColumnCount;
            object[] dr1 = new object[count];

            for (i = 0; i < count; i++)
            {
                dr1[i] = dv.Rows[dv.CurrentCell.RowIndex - 1].Cells[i].Value;
            }

            for (i = 0; i < count; i++)
            {
                dv.Rows[dv.CurrentCell.RowIndex - 1].Cells[i].Value = dv.Rows[dv.CurrentCell.RowIndex].Cells[i].Value;
            }


            for (i = 0; i < count; i++)
            {
                dv.Rows[dv.CurrentCell.RowIndex].Cells[i].Value = dr1[i];
            }
            dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex - 1].Cells[dv.CurrentCell.ColumnIndex];

            countAmount();

        }

        private void toolStripMenuItemDOWN_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender;
            ContextMenuStrip cmenu = (ContextMenuStrip)menuitem.GetCurrentParent();


            DataGridView dv = (DataGridView)(cmenu.SourceControl);

            if (dv.CurrentCell.RowIndex >= dv.Rows.Count - 2 || dv.Rows.Count <= 1 || dv.Rows[dv.CurrentCell.RowIndex].IsNewRow)
                return;

            int i, count = dv.ColumnCount;
            object[] dr1 = new object[count];

            for (i = 0; i < count; i++)
            {
                dr1[i] = dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[i].Value;
            }

            for (i = 0; i < count; i++)
            {
                dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[i].Value = dv.Rows[dv.CurrentCell.RowIndex].Cells[i].Value;
            }


            for (i = 0; i < count; i++)
            {
                dv.Rows[dv.CurrentCell.RowIndex].Cells[i].Value = dr1[i];
            }
            dv.CurrentCell = dv.Rows[dv.CurrentCell.RowIndex + 1].Cells[dv.CurrentCell.ColumnIndex];

            countAmount();
        }

        private void toolStripButtonZXDNew_Click(object sender, EventArgs e)
        {

            try
            {

                if (ppd.ShowDialog() != DialogResult.OK)
                    return;
                //printDoc.DefaultPageSettings.PaperSize = printDoc.PrinterSettings.PaperSizes[2]; 
                foreach (System.Drawing.Printing.PaperSize ps in printDoc.PrinterSettings.PaperSizes)
                {
                    if (ps.PaperName == "A3")
                    {
                        printDoc.PrinterSettings.DefaultPageSettings.PaperSize = ps;
                        printDoc.DefaultPageSettings.PaperSize = ps;
                    }
                }
                // Showing the Print Preview Page
                printDoc.BeginPrint += PrintDoc_BeginPrintN;
                printDoc.PrintPage += PrintDoc_PrintPageN;

                ppw.Width = 1000;
                ppw.Height = 800;

                
                //if (ppw.ShowDialog() != DialogResult.OK)
                //{
                //    printDoc.BeginPrint -= PrintDoc_BeginPrint;
                //    printDoc.PrintPage -= PrintDoc_PrintPage;
                //    return;
                //}
                


                // Printing the Documnet
                printDoc.Print();
                printDoc.BeginPrint -= PrintDoc_BeginPrintN;
                printDoc.PrintPage -= PrintDoc_PrintPageN;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void PrintDoc_BeginPrintN(object sender, System.Drawing.Printing.PrintEventArgs e)
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

        private void PrintDoc_PrintPageN(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            int iyRow;
            int iyRow1;
            int i, j;
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


            System.Drawing.Font _Font22 = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12 = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9 = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font9I = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font _Font12U = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            //e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(0, 0, 870, 500));

            int iTopM = 10;
            int iLeftM = 10;
            int iLeftM1 = 100;
            int iWidth1 = 30;
            int iWidth2 = 200;
            int iWidth3 = 50;


            int iHeight12 = 22;
            int iHeight22 = 50;
            int iHeight9 = 17;
            int iHeight2 = 40;

            int iPaperWidth = 870;

            if (dSetP1.Tables.Contains("PRN3"))
            {

                iTopM = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][0].ToString());
                iLeftM = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][1].ToString());
                iLeftM1 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][2].ToString());

                iHeight2 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][3].ToString());
                iHeight12 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][4].ToString());
                iHeight22 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][5].ToString());
                iHeight9 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][6].ToString());

                iWidth1 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][7].ToString());
                iWidth2 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][8].ToString());
                iWidth3 = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][9].ToString());

                iPaperWidth = Convert.ToInt32(dSetP1.Tables["PRN3"].Rows[0][10].ToString());
            }





            //decimal fTemp = 0;
            int iTemp = 0;
            int iTemp1 = 0;
            bool rTitle;

            Brush b = new SolidBrush(Color.Black);
            try
            {
                //e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM, iTopM, iPaperWidth - 2 * iLeftM, iHeight22));
                e.Graphics.DrawString(sGSMC + "出库单", _Font22, b, new System.Drawing.RectangleF(iLeftM, iTopM, iPaperWidth - 2 * iLeftM, iHeight22), StrFormat);

                iyRow += iTopM + iHeight22;
                e.Graphics.DrawString("单据编号（" + labelDJBH.Text + "）", _Font12, b, new System.Drawing.RectangleF(iLeftM, iyRow, iPaperWidth - 2 * iLeftM, iHeight12), StrFormatR);

                iyRow += iHeight12;
                //e.Graphics.DrawString("制单日期：" + labelZDRQ.Text + "", _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iPaperWidth - 2 * iLeftM, iHeight12), StrFormatR);
                //iyRow += iHeight9;

                e.Graphics.DrawString("业　务员:"+comboBoxYWY.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1-iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("制单日期：" + labelZDRQ.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth-iLeftM1, iHeight9), StrFormatL);
                iyRow+=iHeight9;
                e.Graphics.DrawString("单位名称:"+textBoxDWMC.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1-iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("收　货人:" + textBoxSHR.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);
               iyRow+=iHeight9;
                e.Graphics.DrawString("收货地址:"+textBoxXXDZ.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1-iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("联系电话:" + textBoxLXDH.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);

                iyRow+=iHeight9;
                e.Graphics.DrawString("运输方式:"+comboBoxYSFS.Text, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1-iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("装　箱人:", _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);
                iyRow+=iHeight9;

                //e.Graphics.DrawString(comboBoxYSFS.Text, _Font12, b,(decimal)(iLeftM1 + iLM2), (decimal)(iTopM1 + iHeight1 * 2),StrFormatL);
                //e.Graphics.DrawString(textBoxDZ.Text, _Font12, b, (decimal)(iLeftM1 + iLM3), (decimal)(iTopM1 + iHeight1 * 2), StrFormatL);

                //表头
                e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM, iyRow, iWidth1, iHeight9+2));
                e.Graphics.DrawString("序号", _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow+2, iWidth1, iHeight9), StrFormat);
                e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + iWidth1, iyRow, iWidth2, iHeight9 + 2));
                e.Graphics.DrawString("产品型号", _Font9, b, new System.Drawing.RectangleF(iLeftM + iWidth1, iyRow+2, iWidth2, iHeight9), StrFormat);
                e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + iWidth1 + iWidth2, iyRow, iWidth3, iHeight9 + 2));
                e.Graphics.DrawString("数量", _Font9, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2, iyRow+2, iWidth3, iHeight9), StrFormat);
                rTitle = false;
                if (!IsLastRow(RowPos))
                {
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + iWidth1 + iWidth2 + iWidth3, iyRow, iWidth1, iHeight9 + 2));
                    e.Graphics.DrawString("序号", _Font9, b, new System.Drawing.RectangleF(iLeftM + iWidth1 + iWidth2 + iWidth3, iyRow+2, iWidth1, iHeight9), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 2 * iWidth1 + iWidth2 + iWidth3, iyRow, iWidth2, iHeight9 + 2));
                    e.Graphics.DrawString("产品型号", _Font9, b, new System.Drawing.RectangleF(iLeftM + 2 * iWidth1 + iWidth2 + iWidth3, iyRow+2, iWidth2, iHeight9), StrFormat);
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iLeftM + 2 * iWidth1 + 2 * iWidth2 + iWidth3, iyRow, iWidth3, iHeight9 + 2));
                    e.Graphics.DrawString("数量", _Font9, b, new System.Drawing.RectangleF(iLeftM + 2 * iWidth1 + 2 * iWidth2 + iWidth3, iyRow+2, iWidth3, iHeight9), StrFormat);
                    rTitle = true;
                }
                iyRow += iHeight9+2;

                for (i = 0; i < iPageZX; i++)
                {
                    //序号
                    if (RowPos >= dataGridViewDJMX.Rows.Count && dataGridViewDJMX.ReadOnly)
                    {
                        NewPage = false;

                        iTemp1 = iyRow + (i / 2) * iHeight9;

                        //右表格
                        if (i % 2 != 0 & rTitle)
                        {
                            iTemp = iLeftM + iWidth1 + iWidth2 + iWidth3;
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp, iTemp1, iWidth1, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1, iTemp1, iWidth2, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9));
                        }

                        break;
                    }

                    if (i % 2 == 0 && !dataGridViewDJMX.Rows[RowPos].IsNewRow)
                    {
                        iTemp = iLeftM;
                        iTemp1 =  iyRow+(i / 2) * iHeight9;
                    }
                    else
                    {
                        iTemp = iLeftM + iWidth1 + iWidth2 + iWidth3;
                    }

                    if (RowPos >= dataGridViewDJMX.Rows.Count)
                    {
                        NewPage = false;

                        //右表格
                        if (i % 2 != 0 & rTitle)
                        {
                            iTemp = iLeftM + iWidth1 + iWidth2 + iWidth3;
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp, iTemp1, iWidth1, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1, iTemp1, iWidth2, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9));
                        }

                        break;
                    }

                    if (dataGridViewDJMX.Rows[RowPos].IsNewRow)
                    {
                        NewPage = false;

                        //右表格
                        if (i % 2 != 0 & rTitle)
                        {
                            iTemp = iLeftM + iWidth1 + iWidth2 + iWidth3;
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp, iTemp1, iWidth1, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1, iTemp1, iWidth2, iHeight9));
                            e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9));
                        }
                        break;
                    }

                    //序号
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp, iTemp1, iWidth1, iHeight9));
                    e.Graphics.DrawString(((PageNo - 1) * iPageZX + i + 1).ToString(), _Font9I, b, new System.Drawing.RectangleF(iTemp, iTemp1, iWidth1, iHeight9), StrFormat);

                    //表格
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1, iTemp1, iWidth2, iHeight9));
                    e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9));




                    e.Graphics.DrawString(dataGridViewDJMX.Rows[RowPos].Cells[1].Value.ToString(), _Font9, b, new System.Drawing.RectangleF(iTemp + iWidth1, iTemp1, iWidth2, iHeight9), StrFormat);

                    e.Graphics.DrawString(Convert.ToDecimal(dataGridViewDJMX.Rows[RowPos].Cells[6].Value.ToString()).ToString("f0"), _Font9, b, new System.Drawing.RectangleF(iTemp + iWidth1 + iWidth2, iTemp1, iWidth3, iHeight9), StrFormat);


                    if (IsLastRow(RowPos))
                    {
                        NewPage = false;
                    }
                    RowPos++;

                    
                }

                //iyRow = iTemp1+iHeight9+10;
                iyRow += iHeight9 * iPageNZX+10;
                //页脚
                e.Graphics.DrawString("联系我们：", _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iPaperWidth-2*iLeftM, iHeight9), StrFormatL);
                iyRow += iHeight9+5;
                e.Graphics.DrawString(sGSDZ, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("http://www.century-twinkle.com", _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);

                iyRow += iHeight9;

                e.Graphics.DrawString("电话：" + sGSDH, _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iLeftM1 - iLeftM, iHeight9), StrFormatL);
                e.Graphics.DrawString("传真：" + sGSCZ + " 邮编:" + sGSYB, _Font9, b, new System.Drawing.RectangleF(iLeftM1, iyRow, iPaperWidth - iLeftM1, iHeight9), StrFormatL);

                //
                iyRow += iHeight9;
                if (!dataGridViewDJMX.ReadOnly)
                    iTemp = (int)Math.Ceiling((decimal)(dataGridViewDJMX.Rows.Count - 1) / (decimal)(iPageZX));
                else
                    iTemp = (int)Math.Ceiling((decimal)(dataGridViewDJMX.Rows.Count) / (decimal)(iPageZX));

                //e.Graphics.DrawString(PageNo.ToString()+"\\" + iTemp.ToString(), _Font9, b, new System.Drawing.RectangleF(iLeftM, iyRow, iPaperWidth - 2 * iLeftM, iHeight9), StrFormatR);
                e.Graphics.DrawString(PageNo.ToString() + "/" + iTemp.ToString(), _Font22, b, new System.Drawing.RectangleF(iLeftM, iTopM, iPaperWidth - 2 * iLeftM, iHeight22), StrFormatR);






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


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsLastRow(int iRow)
        {
            if (!dataGridViewDJMX.ReadOnly || dataGridViewDJMX.AllowUserToAddRows) //有新行
            {
                if (iRow == dataGridViewDJMX.RowCount - 2)
                    return true;
                else
                    return false;
            }
            else
            {
                if(iRow == dataGridViewDJMX.RowCount - 1)
                    return true;
                else
                    return false;
            }
        }

        private void dataGridViewDJMX_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex != 7 || e.RowIndex<0 )
                return;

            if (Convert.ToDecimal(dataGridViewDJMX.Rows[e.RowIndex].Cells[7].Value) < Convert.ToDecimal(dataGridViewDJMX.Rows[e.RowIndex].Cells[9].Value))
            {
                e.CellStyle.BackColor = Color.LightPink;
            }

        }




    }
}