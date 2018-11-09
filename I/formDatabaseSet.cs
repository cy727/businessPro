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
    public partial class formDatabaseSet : Form
    {
        public string strConn="";
        private string dFileName="";
        public int intMode = 0;

        private System.Data.DataSet dSet = new DataSet();

        public formDatabaseSet()
        {
            InitializeComponent();
            dFileName = Directory.GetCurrentDirectory() + "\\appcon.xml";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            strConn = "";
            this.Close();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            
            strConn = "workstation id=CY;packet size=4096;user id=" + textBoxUser.Text.Trim().ToLower() + ";password=" + textBoxPassword.Text.Trim().ToLower() + ";data source=\"" + textBoxServer.Text.Trim() + "\";;initial catalog="+textBoxDatabase.Text.Trim().ToLower();

            sqlConn.ConnectionString = strConn;
            try
            {
                sqlConn.Open();
            }
            catch (System.Data.SqlClient.SqlException err)
            {
                MessageBox.Show("数据库连接错误，请与管理员联系");
                strConn = "";
                return;

            }

            MessageBox.Show("数据库连接正常");
            sqlConn.Close();

            dSet.Tables["数据库信息"].Rows[0][0] = textBoxServer.Text;
            dSet.Tables["数据库信息"].Rows[0][1] = textBoxUser.Text;

            if(checkBoxRember.Checked) //记住密码
                dSet.Tables["数据库信息"].Rows[0][2] = textBoxPassword.Text;
            else
                dSet.Tables["数据库信息"].Rows[0][2] = "";

            dSet.Tables["数据库信息"].Rows[0][3] = textBoxDatabase.Text;
            dSet.WriteXml(dFileName);


            this.Close();

        }

        private void formDatabaseSet_Load(object sender, EventArgs e)
        {
            if (intMode == 0)//测试
            {
                btnTest.Visible = true;
                btnCreate.Visible = false;
                this.Text = "数据库设置";
            }
            else //创建
            {
                btnTest.Visible = false;
                btnCreate.Visible = true;
                this.Text = "创建数据库";
            }

            sqlComm.Connection = sqlConn;

            if(File.Exists(dFileName)) //存在文件
            {
                dSet.ReadXml(dFileName);
            }
            else  //建立文件
            {
                dSet.Tables.Add("数据库信息");

                dSet.Tables["数据库信息"].Columns.Add("服务器地址", System.Type.GetType("System.String"));
                dSet.Tables["数据库信息"].Columns.Add("用户名", System.Type.GetType("System.String"));
                dSet.Tables["数据库信息"].Columns.Add("密码", System.Type.GetType("System.String"));
                dSet.Tables["数据库信息"].Columns.Add("数据库", System.Type.GetType("System.String"));

                string[]  strDRow ={ "","","",""};
                dSet.Tables["数据库信息"].Rows.Add(strDRow);
            }

            textBoxServer.Text = dSet.Tables["数据库信息"].Rows[0][0].ToString();
            textBoxUser.Text = dSet.Tables["数据库信息"].Rows[0][1].ToString();
            textBoxPassword.Text = dSet.Tables["数据库信息"].Rows[0][2].ToString();
            textBoxDatabase.Text = dSet.Tables["数据库信息"].Rows[0][3].ToString();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本版软件尚不能数据库创建", "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;

            
            strConn = "packet size=4096;user id=" + textBoxUser.Text.Trim().ToLower() + ";password=" + textBoxPassword.Text.Trim().ToLower() + ";data source=\"" + textBoxServer.Text.Trim() + "\";initial catalog=;Integrated Security=True";

            try
            {
                sqlConn.ConnectionString = strConn;
                sqlConn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库创建失败：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strConn = "";
                return;
            }

            try

            {
                sqlComm.CommandText = "create database " + textBoxDatabase.Text.Trim().ToLower();
                sqlComm.ExecuteNonQuery();

                sqlConn.Close();

                strConn = "packet size=4096;user id=" + textBoxUser.Text.Trim().ToLower() + ";password=" + textBoxPassword.Text.Trim().ToLower() + ";data source=\"" + textBoxServer.Text.Trim() + "\";Database=" + textBoxDatabase.Text.Trim().ToLower() + ";Integrated Security=SSPI";
                sqlConn.ConnectionString = strConn;
                sqlConn.Open();

                sqlComm.CommandText = "CREATE TABLE [dbo].[库存盘点明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据ID] [int] NULL,[商品ID] [int] NULL,[结存数量] [decimal](18, 0) NULL,[结存金额] [decimal](14, 2) NULL,[实盘数量] [decimal](18, 0) NULL,[备注] [nvarchar](200) NULL,[盘点标志] [int] NULL,[盘损数量] [decimal](18, 0) NULL,[盘损金额] [decimal](18, 0) NULL,[库房ID] [int] NULL, CONSTRAINT [PK_库存盘点明细表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[库存盘点汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据编号] [nvarchar](20) NOT NULL,[日期] [smalldatetime] NULL,[业务员ID] [int] NULL,[操作员ID] [int] NULL,[盘点标记] [bit] NULL,[备注] [nvarchar](200) NULL,[库房ID] [int] NULL,[商品ID] [int] NULL,[分类ID] [int] NULL,[数量合计] [decimal](18, 0) NULL,[金额合计] [decimal](14, 2) NULL,[盘损数量合计] [decimal](18, 0) NULL,[盘损金额合计] [decimal](14, 2) NULL,	[BeActive] [bit] NULL,[盘点时间] [smalldatetime] NULL,CONSTRAINT [PK_库存盘点汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[库存盘点定义表]([备注] [nvarchar](200) NULL,[记录选择] [bit] NULL,[盘损数量] [decimal](18, 0) NULL,[盘损金额] [decimal](14, 2) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[库存表]([ID] [int] IDENTITY(1,1) NOT NULL,[库房ID] [int] NULL,[商品ID] [int] NULL,[库存数量] [decimal](10, 0) NOT NULL,[库存金额] [decimal](12, 2) NOT NULL,[库存成本价] [decimal](14, 2) NOT NULL,[核算成本价] [decimal](14, 2) NULL,[库存上限] [decimal](12, 0) NOT NULL,[库存下限] [decimal](12, 0) NOT NULL,[合理库存上限] [decimal](12, 0) NOT NULL,[合理库存下限] [decimal](12, 0) NOT NULL,[应付金额] [decimal](18, 2) NOT NULL,[已付金额] [decimal](18, 2) NOT NULL,[应收金额] [decimal](14, 2) NOT NULL,[已收金额] [decimal](14, 2) NOT NULL,[BeActive] [bit] NULL,CONSTRAINT [PK_库存表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[库存报损明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据ID] [int] NULL,[商品ID] [int] NULL,[报损数量] [decimal](18, 0) NULL,[报损金额] [decimal](14, 2) NULL,[成本单价] [decimal](14, 2) NULL,[备注] [nvarchar](200) NULL,[原库存数量] [decimal](18, 0) NULL,CONSTRAINT [PK_库存报损明细表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[库存报损汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据编号] [nvarchar](20) NOT NULL,[日期] [smalldatetime] NULL,[业务员ID] [int] NULL,[操作员ID] [int] NULL,[备注] [nvarchar](200) NULL,[库房ID] [int] NULL,[商品ID] [int] NULL,[分类ID] [int] NULL,[报损数量合计] [decimal](18, 0) NULL,[报损金额合计] [decimal](14, 2) NULL,[BeActive] [bit] NULL,CONSTRAINT [PK_库存报损汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[进货退出明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据ID] [int] NOT NULL,[商品ID] [int] NOT NULL,[库房ID] [int] NOT NULL,[原单据ID] [int] NULL,[单位] [char](4) NULL,[规格] [int] NULL,[数量] [decimal](12, 2) NULL,[件数] [decimal](10, 2) NULL,[含税价] [decimal](14, 3) NULL,[单价] [decimal](12, 3) NULL,[金额] [decimal](14, 2) NULL,[税率] [decimal](7, 2) NULL,[税额] [decimal](14, 2) NULL,[扣率] [decimal](7, 2) NULL,[零售价] [decimal](14, 2) NULL,[毛利] [decimal](14, 2) NULL,[赠品] [bit] NULL,[零售金额] [decimal](14, 2) NULL,[批零差价] [decimal](14, 2) NULL,[实计金额] [decimal](14, 2) NULL,[付款金额] [decimal](18, 0) NULL,[BeActive] [bit] NULL,[未付款金额] [decimal](14, 2) NULL,[已付款金额] [decimal](14, 2) NULL,[未付款数量] [decimal](14, 2) NULL,[已付款数量] [decimal](14, 2) NULL,CONSTRAINT [PK_进货退出明细表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[进货退出汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[单位ID] [int] NOT NULL,[单据编号] [nvarchar](20) NOT NULL,[日期] [smalldatetime] NULL,[发票号] [varchar](200) NULL,[支票号] [varchar](200) NULL,[合同号] [nvarchar](20) NULL,[价税合计] [decimal](14, 2) NULL,[业务员ID] [int] NULL,[BeActive] [bit] NULL,[操作员ID] [int] NULL,[备注] [nvarchar](200) NULL,[未付款金额] [decimal](14, 2) NULL,[已付款金额] [decimal](14, 2) NULL,[付款标记] [bit] NULL,[结清时间] [smalldatetime] NULL,[部门ID] [int] NULL,[冲红时间] [smalldatetime] NULL,CONSTRAINT [PK_进货退出汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[进货入库明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据ID] [int] NOT NULL,[商品ID] [int] NOT NULL,[库房ID] [int] NOT NULL,[原单据ID] [int] NULL,[单位] [char](4) NULL,[规格] [int] NULL,[数量] [decimal](12, 2) NULL,[件数] [decimal](10, 2) NULL,[含税价] [decimal](14, 3) NULL,[单价] [decimal](12, 3) NULL,[金额] [decimal](14, 2) NULL,[税率] [decimal](7, 2) NULL,[税额] [decimal](14, 2) NULL,[扣率] [decimal](7, 2) NULL,[零售价] [decimal](14, 2) NULL,[毛利] [decimal](14, 2) NULL,[赠品] [bit] NULL,[零售金额] [decimal](14, 2) NULL,[批零差价] [decimal](14, 2) NULL,[实计金额] [decimal](14, 2) NULL,[BeActive] [bit] NULL,[未付款金额] [decimal](14, 2) NULL,[已付款金额] [decimal](14, 2) NULL,[未付款数量] [decimal](14, 2) NULL,[已付款数量] [decimal](14, 2) NULL,[原单据明细ID] [int] NULL,CONSTRAINT [PK_进货入库明细表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[进货入库汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[单位ID] [int] NOT NULL,[单据编号] [nvarchar](20) NOT NULL,[日期] [smalldatetime] NULL,[发票号] [varchar](200) NULL,[支票号] [varchar](200) NULL,[合同号] [nvarchar](20) NULL,[价税合计] [decimal](14, 2) NULL,[业务员ID] [int] NULL,[操作员ID] [int] NULL,[未付款金额] [decimal](14, 2) NULL,[已付款金额] [decimal](14, 2) NULL,[付款标记] [bit] NULL,[备注] [varchar](200) NULL,[BeActive] [bit] NULL,[购进ID] [int] NULL,[结清时间] [smalldatetime] NULL,[部门ID] [int] NULL,	[冲红时间] [smalldatetime] NULL,CONSTRAINT [PK_进货入库汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[借物信息修改表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据编号] [nvarchar](20) NOT NULL,[修改单据ID] [int] NOT NULL,[日期] [smalldatetime] NULL,[部门ID] [int] NULL,[业务员ID] [int] NULL,[操作员ID] [int] NULL,[原备注] [nvarchar](50) NULL,[原联系电话] [nvarchar](50) NULL,[原联系人] [nvarchar](20) NULL,[原收货人] [nvarchar](20) NULL,[原到站] [nvarchar](20) NULL,[原运输方式] [nvarchar](50) NULL,[原详细地址] [nvarchar](100) NULL,[原物流名称] [nvarchar](100) NULL,[原单号] [nvarchar](30) NULL,[原邮政编码] [nvarchar](6) NULL,[BeActive] [bit] NULL,[备注] [nvarchar](50) NULL,[联系电话] [nvarchar](50) NULL,[联系人] [nvarchar](20) NULL,[收货人] [nvarchar](20) NULL,[到站] [nvarchar](20) NULL,[运输方式] [nvarchar](50) NULL,[详细地址] [nvarchar](100) NULL,[物流名称] [nvarchar](100) NULL,[单号] [nvarchar](30) NULL,[邮政编码] [nvarchar](6) NULL,CONSTRAINT [PK_借物信息修改表] RIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[借物出库明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[表单ID] [int] NOT NULL,[商品ID] [int] NOT NULL,[库房ID] [int] NULL,[保质期] [smalldatetime] NULL,[数量] [decimal](18, 0) NULL,[单价] [decimal](12, 3) NULL,[金额] [decimal](14, 2) NULL,[库存成本价] [decimal](14, 2) NULL,[出库金额] [decimal](14, 2) NULL,[备注] [nvarchar](200) NULL,[BeActive] [bit] NULL,CONSTRAINT [PK_借物出库明细表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[借物出库汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据编号] [nvarchar](20) NOT NULL,[单位ID] [int] NOT NULL,[日期] [smalldatetime] NULL,[金额] [decimal](14, 2) NULL,[税额] [decimal](14, 2) NULL,[部门ID] [int] NULL,[业务员ID] [int] NULL,[操作员ID] [int] NULL,[付款方式] [int] NULL,[发票号] [nvarchar](12) NULL,[合同ID] [int] NULL,[备注] [nvarchar](50) NULL,[价税合计] [decimal](14, 2) NULL,[出库金额] [decimal](14, 2) NULL,[出库标记] [bit] NULL,[BeActive] [bit] NULL,[联系电话] [nvarchar](50) NULL,[联系人] [nvarchar](20) NULL,[收货人] [nvarchar](20) NULL,[到站] [nvarchar](20) NULL,[运输方式] [nvarchar](50) NULL,[详细地址] [nvarchar](100) NULL,[物流名称] [nvarchar](100) NULL,[单号] [nvarchar](30) NULL,[邮政编码] [nvarchar](6) NULL,[付款标记] [bit] NULL,[未付款金额] [decimal](14, 2) NULL,[已付款金额] [decimal](14, 2) NULL,[冲抵单号ID] [int] NULL,[冲红时间] [smalldatetime] NULL,CONSTRAINT [PK_借物出库汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[结转往来汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[结转ID] [int] NULL,[单位ID] [int] NULL,[应付余额] [decimal](14, 2) NULL,[应收余额] [decimal](14, 2) NULL,CONSTRAINT [PK_结转往来汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[结转库房汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[结转ID] [int] NULL,[商品ID] [int] NULL,[库房ID] [int] NULL,[结转数量] [decimal](18, 0) NULL,[结转单价] [decimal](14, 2) NULL,[结转金额] [decimal](14, 2) NULL,[入库数量] [decimal](18, 0) NULL,[入库金额] [decimal](18, 0) NULL,[购进入库数量] [decimal](18, 0) NULL,[购进入库金额] [decimal](18, 0) NULL,[出库数量] [decimal](18, 0) NULL,[出库金额] [decimal](18, 0) NULL,[销售数量] [decimal](18, 0) NULL,[销售金额] [decimal](18, 0) NULL,[出库毛利] [decimal](18, 0) NULL,[销出毛利率] [decimal](18, 0) NULL,CONSTRAINT [PK_结转库房汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[结转进销存汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[结转ID] [int] NULL,[商品ID] [int] NULL,[结转数量] [decimal](18, 0) NULL,[结转单价] [decimal](14, 2) NULL,[结转金额] [decimal](14, 2) NULL,[入库数量] [decimal](18, 0) NULL,[入库金额] [decimal](18, 0) NULL,[购进入库数量] [decimal](18, 0) NULL,[购进入库金额] [decimal](18, 0) NULL,[出库数量] [decimal](18, 0) NULL,[出库金额] [decimal](18, 0) NULL,[销售数量] [decimal](18, 0) NULL,[销售金额] [decimal](18, 0) NULL,[出库毛利] [decimal](18, 0) NULL,[销出毛利率] [decimal](18, 0) NULL,CONSTRAINT [PK_结转进销存汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[结转汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[结算时间] [smalldatetime] NULL,[操作员ID] [int] NULL,CONSTRAINT [PK_结转汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[结算收款明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据ID] [int] NOT NULL,[账簿ID] [int] NULL,[支票号] [nvarchar](100) NULL,[冲应付款] [decimal](14, 2) NULL,[扣率] [decimal](7, 2) NULL,[付款金额] [decimal](14, 2) NULL,[摘要] [nvarchar](50) NULL,[日期] [smalldatetime] NULL,[备注] [nvarchar](200) NULL,CONSTRAINT [PK_结算收款明细表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[结算收款汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据编号] [nvarchar](20) NOT NULL,[原单据ID] [int] NULL,[单位ID] [int] NULL,[税号] [nvarchar](50) NULL,[业务员ID] [int] NULL,[操作员ID] [int] NULL,[日期] [smalldatetime] NULL,[发票号] [nvarchar](12) NULL,[开票日期] [smalldatetime] NULL,[备注] [nvarchar](200) NULL,[BeActive] [bit] NULL,[实计金额] [decimal](14, 2) NULL,[部门ID] [int] NULL,[备注2] [varchar](200) NULL,[冲红时间] [smalldatetime] NULL,CONSTRAINT [PK_结算收款汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[结算收款勾兑表]([ID] [int] IDENTITY(1,1) NOT NULL,[付款ID] [int] NULL,[勾兑方式] [int] NULL,[勾兑ID] [int] NULL,[单据编号] [nvarchar](50) NULL,[已付款] [float] NULL,[BeActive] [int] NULL,CONSTRAINT [PK_结算收款勾兑表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[结算收款定义表]([结清] [bit] NOT NULL,[勾兑标记] [int] NULL,[勾兑纪录] [int] NULL,[将支付金额] [decimal](14, 2) NULL,[将付款数量] [decimal](18, 0) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[结算付款明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据ID] [int] NOT NULL,[账簿ID] [int] NULL,[支票号] [nvarchar](100) NULL,[冲应付款] [decimal](14, 2) NULL,[扣率] [decimal](7, 2) NULL,[付款金额] [decimal](14, 2) NULL,[摘要] [nvarchar](50) NULL,[日期] [smalldatetime] NULL,[备注] [nvarchar](200) NULL,CONSTRAINT [PK_结算付款明细表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[结算付款汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据编号] [nvarchar](20) NOT NULL,[原单据ID] [int] NULL,[单位ID] [int] NULL,[税号] [nvarchar](50) NULL,[业务员ID] [int] NULL,[操作员ID] [int] NULL,[日期] [smalldatetime] NULL,[发票号] [nvarchar](12) NULL,[开票日期] [smalldatetime] NULL,[备注] [nvarchar](200) NULL,[BeActive] [bit] NULL,[实计金额] [decimal](14, 2) NULL,[部门ID] [int] NULL,[备注2] varchar](200) NULL,	[冲红时间] [smalldatetime] NULL, CONSTRAINT [PK_结算付款汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[结算付款勾兑表]([ID] [int] IDENTITY(1,1) NOT NULL,[付款ID] [int] NULL,[勾兑方式] [int] NULL,[勾兑ID] [int] NULL,[单据编号] [nvarchar](50) NULL,[已付款] [float] NULL,[BeActive] [int] NULL,CONSTRAINT [PK_结算付款勾兑表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[结算付款定义表]([结清] [bit] NOT NULL,[勾兑标记] [int] NULL,[勾兑纪录] [int] NULL,[将支付金额] [decimal](14, 2) NULL,[将付款数量] [decimal](18, 0) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[结算方式表]([ID] [int] IDENTITY(1,1) NOT NULL,[djsid] [char](11) NOT NULL,[结算编号] [char](11) NOT NULL,[结算名称] [char](20) NULL,[助记码] [char](20) NULL,[方向] [char](2) NULL,[当日牌价] [decimal](12, 6) NOT NULL,[需要备用金] [char](2) NULL,[是否可支付] [char](2) NULL,[卡号管理] [char](2) NULL,[扣率] [decimal](7, 2) NOT NULL,[密码管理] [char](2) NULL,[使用方式] [char](4) NULL,[提示信息] [char](20) NULL,[结清] [char](2) NULL,[证件号管理] [char](2) NULL,[beactive] [char](2) NULL,CONSTRAINT [PK_结算方式表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[购进退补差价明细表]([ID] [int] IDENTITY(1,1) NOT NULL,	[单据ID] [int] NOT NULL,[商品ID] [int] NOT NULL,[库房ID] [int] NOT NULL,[补价数量] [decimal](12, 2) NULL,[差价] [decimal](12, 3) NULL,[金额] [decimal](14, 2) NULL,[BeActive] [bit] NULL,[未付款金额] [decimal](14, 2) NULL,[已付款金额] [decimal](14, 2) NULL,[未付款数量] [decimal](14, 2) NULL,[已付款数量] [decimal](14, 2) NULL,CONSTRAINT [PK_购进退补差价明细表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[购进退补差价汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,	[单位ID] [int] NOT NULL,[单据编号] [nvarchar](20) NOT NULL,[日期] [smalldatetime] NULL,[发票号] [varchar](200) NULL,[支票号] [varchar](200) NULL,[合同号] [nvarchar](20) NULL,[价税合计] [decimal](14, 2) NULL,[业务员ID] [int] NULL,	[操作员ID] [int] NULL,[BeActive] [bit] NULL,[付款标记] [bit] NULL,[备注] [nvarchar](200) NULL,[未付款金额] [decimal](14, 2) NULL,[已付款金额] [decimal](14, 2) NULL,[结清时间] [smalldatetime] NULL,[部门ID] [int] NULL,[冲红时间] [smalldatetime] NULL,CONSTRAINT [PK_购进退补差价汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[购进商品制单明细定义表]([ID] [int] IDENTITY(1,1) NOT NULL,[表单ID] [int] NOT NULL,[商品ID] [int] NOT NULL,[库房ID] [int] NOT NULL,[日期] [smalldatetime] NULL,[保质期] [smalldatetime] NULL,[单位] [nvarchar](10) NULL,[规格] [int] NULL,[数量] [decimal](12, 2) NULL,[含税价] [decimal](14, 3) NULL,[单价] [decimal](12, 3) NULL,[金额] [decimal](14, 2) NULL,[税率] [decimal](7, 2) NULL,[税额] [decimal](14, 2) NULL,[扣率] [decimal](7, 2) NULL,[实计金额] [decimal](14, 2) NULL,[零售价] [decimal](14, 2) NULL,[毛利] [decimal](14, 2) NULL,[赠品] [bit] NULL,[批零差价] [decimal](14, 2) NULL,[到货] [bit] NULL,[库存数量] [decimal](18, 0) NULL,[库房编号] [char](10) NULL,[库房名称] [nvarchar](50) NULL,[统计标志] [int] NULL,[保留] [bit] NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[购进商品制单明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[表单ID] [int] NOT NULL,[商品ID] [int] NOT NULL,[库房ID] [int] NULL,[日期] [smalldatetime] NULL,[保质期] [smalldatetime] NULL,[单位] [nvarchar](10) NULL,[规格] [int] NULL,[数量] [decimal](12, 2) NULL,[含税价] [decimal](14, 3) NULL,[单价] [decimal](12, 3) NULL,[金额] [decimal](14, 2) NULL,[税率] [decimal](7, 2) NULL,[税额] [decimal](14, 2) NULL,[扣率] [decimal](7, 2) NULL,[实计金额] [decimal](14, 2) NULL,[零售价] [decimal](14, 2) NULL,[毛利] [decimal](14, 2) NULL,[赠品] [bit] NULL,[批零差价] [decimal](14, 2) NULL,[未到货数量] [decimal](18, 0) NULL,[已到货数量] [decimal](18, 0) NULL, CONSTRAINT [PK_购进商品制单明细表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[购进商品制单表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据编号] [nvarchar](20) NOT NULL,[单位ID] [int] NOT NULL,[日期] [smalldatetime] NULL,[金额] [decimal](14, 2) NULL,[税额] [decimal](14, 2) NULL,[部门ID] [int] NULL,[业务员ID] [int] NULL,[操作员ID] [int] NULL,[付款方式] [nvarchar](20) NULL,[发票号] [varchar](200) NULL,[合同ID] [int] NULL,[备注] [nvarchar](50) NULL,[价税合计] [decimal](14, 2) NULL,[入库标记] [bit] NULL,[BeActive] [bit] NULL,[冲红时间] [smalldatetime] NULL, CONSTRAINT [PK_购进商品制单表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[岗位表]([ID] [int] IDENTITY(1,1) NOT NULL,[dgwid] [char](11) NULL,[岗位编号] [char](10) NOT NULL,[岗位名称] [nvarchar](50) NULL,[助记码] [char](20) NULL,[备注] [nvarchar](200) NULL,[权限] [int] NOT NULL,CONSTRAINT [PK_岗位表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[发票明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[发票ID] [int] NOT NULL,[冲抵ID] [int] NULL,[单据ID] [int] NULL,[单据编号] [nvarchar](30) NULL,[原开票总额] [decimal](18, 0) NULL,[发票总额] [decimal](14, 2) NULL,[发货方式] [nvarchar](50) NULL,[单号] [nvarchar](50) NULL,[商品ID] [int] NULL,[备注1] [varchar](200) NULL,[备注2] [varchar](200) NULL,[冲抵编号] [varchar](30) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[发票汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[发票号] [nvarchar](200) NULL,[单位ID] [int] NULL,[备注] [nvarchar](200) NULL,[发货方式] [nvarchar](50) NULL,[单号] [nvarchar](50) NULL,[操作员ID] [int] NULL,[原开票金额] [decimal](14, 2) NULL,[发票总额] [decimal](18, 2) NULL,[BeActive] [bit] NULL,[发票类型] [int] NULL,[日期] [smalldatetime] NULL,[作废日期] [smalldatetime] NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[发票定义表]([选择] [bit] NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[调价通知单明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据ID] [int] NULL,[商品ID] [int] NULL,[原进价] [decimal](14, 2) NULL,[进价] [decimal](14, 2) NULL,[原批发价] [decimal](14, 2) NULL,[批发价] [decimal](14, 2) NULL,CONSTRAINT [PK_调价通知单明细表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[调价通知单汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据编号] [nvarchar](20) NOT NULL,[日期] [smalldatetime] NULL,[业务员ID] [int] NULL,[操作员ID] [int] NULL,[备注] [nvarchar](200) NULL,[BeActive] [bit] NULL,[执行标记] [bit] NULL,[执行时间] [smalldatetime] NULL,CONSTRAINT [PK_调价通知单汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[地区表]([ID] [int] IDENTITY(1,1) NOT NULL,[地区] [nvarchar](50) NULL,[编号] [nchar](10) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[单位历史账表]([ID] [int] IDENTITY(1,1) NOT NULL,[单位ID] [int] NULL,[日期] [smalldatetime] NULL,[单据编号] [nvarchar](50) NULL,[摘要] [nvarchar](50) NULL,[购进未入库金额] [decimal](14, 2) NULL,[购进金额] [decimal](14, 2) NULL,[付款金额] [decimal](14, 2) NULL,[应付余额] [decimal](14, 2) NULL,[销出金额] [decimal](14, 2) NULL,[收入金额] [decimal](14, 2) NULL,[应收金额] [decimal](14, 2) NULL,[购进标记] [bit] NULL,[销售标记] [bit] NULL,[部门ID] [int] NULL,[业务员ID] [int] NULL,[冲抵单号] [nvarchar](50) NULL,[BeActive] [bit] NULL, CONSTRAINT [PK_单位历史账表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[单位表]([ID] [int] IDENTITY(1,1) NOT NULL,[ddwid] [char](11) NULL,[单位编号] [char](10) NULL,[单位名称] [nvarchar](50) NULL,[助记码] [char](20) NULL,[是否进货] [bit] NULL,[是否销售] [bit] NULL,	[是否调拨] [bit] NULL,[是否经销] [bit] NULL,[税号] [nchar](20) NULL,[电话] [nvarchar](50) NULL,	[开户银行] [nvarchar](50) NULL,[银行账号] [char](30) NULL,[联系人] [nvarchar](50) NULL,[扣率] [decimal](7, 2) NULL,	[地址] [nvarchar](100) NULL,[地区名称] [nvarchar](50) NULL,[行业名称] [nvarchar](50) NULL,[客户等级] [int] NULL,[传真] [varchar](50) NULL,[邮编] [char](6) NULL,[备注] [ntext] NULL,[登录日期] [smalldatetime] NULL,[业务员] [nvarchar](20) NULL,[收货人] [nvarchar](20) NULL,[联系地址] [nvarchar](60) NULL,[应付账款] [decimal](18, 2) NULL,[应收账款] [decimal](18, 2) NULL,[BeActive] [bit] NULL,[到站名称] [nvarchar](50) NULL,[部门ID] [int] NULL,[开票电话] [varchar](50) NULL,[收货电话] [varchar](50) NULL,CONSTRAINT [PK_单位表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[商品条码表]([ID] [int] IDENTITY(1,1) NOT NULL,[条码] [nvarchar](40) NULL,[商品ID] [int] NULL,[库房ID] [int] NULL,[单据编号] [nvarchar](30) NULL,[摘要] [nvarchar](50) NULL,[日期] [smalldatetime] NULL,[出入库标记] [bit] NULL,[操作员ID] [int] NULL,[单据明细ID] [int] NULL,CONSTRAINT [PK_商品条码表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[商品历史账表]([ID] [int] IDENTITY(1,1) NOT NULL,[日期] [smalldatetime] NULL,[商品ID] [int] NULL,[单位ID] [int] NULL,[部门ID] [int] NULL,[业务员ID] [int] NULL,[原单据编号] [nvarchar](30) NULL,[单据编号] [nvarchar](30) NULL,[摘要] [nvarchar](50) NULL,[购进数量] [decimal](18, 0) NULL,[购进单价] [decimal](14, 2) NULL,[购进金额] [decimal](14, 2) NULL,[入库数量] [decimal](18, 0) NULL,[入库单价] [decimal](14, 2) NULL,[入库金额] [decimal](14, 2) NULL,[销售数量] [decimal](18, 0) NULL,[销售单价] [decimal](14, 2) NULL,[销售金额] [decimal](14, 2) NULL,[出库数量] [decimal](18, 0) NULL,[出库单价] [decimal](14, 2) NULL,[出库金额] [decimal](18, 0) NULL,[总结存数量] [decimal](18, 0) NULL,[总结存金额] [decimal](14, 2) NULL,[毛利] [decimal](18, 0) NULL,[结算数量] [decimal](18, 0) NULL,[结算单价] [decimal](14, 2) NULL,[结算金额] [decimal](14, 2) NULL,[退出数量] [decimal](18, 0) NULL,[退出单价] [decimal](14, 2) NULL,[退出金额] [decimal](14, 2) NULL,[退回数量] [decimal](18, 0) NULL,[退回单价] [decimal](14, 2) NULL,[退回金额] [decimal](14, 2) NULL,[开票金额] [decimal](14, 2) NULL,[应付金额] [decimal](18, 0) NULL,[未付金额] [decimal](18, 0) NULL,[已付金额] [decimal](18, 0) NULL,[应收金额] [decimal](14, 2) NULL,[未收金额] [decimal](18, 0) NULL,[已收金额] [decimal](14, 2) NULL,[销售退补价数量] [decimal](18, 0) NULL,[销售退补价单价] [decimal](14, 2) NULL,[销售退补价金额] [decimal](14, 2) NULL,[盘损数量] [decimal](18, 0) NULL,[盘损单价] [decimal](14, 2) NULL,[盘损金额] [decimal](14, 2) NULL,[报损数量] [decimal](18, 0) NULL,[报损单价] [decimal](14, 2) NULL,[报损金额] [decimal](14, 2) NULL,[借物数量] [decimal](18, 0) NULL,[借物单价] [decimal](14, 2) NULL,[借物金额] [decimal](14, 2) NULL,[BeActive] [bit] NULL,[购进退补价数量] [decimal](18, 0) NULL,[购进退补价单价] [decimal](14, 2) NULL,[购进退补价金额] [decimal](14, 2) NULL,[组装数量] [decimal](18, 0) NULL,[组装单价] [decimal](14, 2) NULL,[组装金额] [decimal](14, 2) NULL,CONSTRAINT [PK_商品历史账表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[商品库房历史账表]([ID] [int] IDENTITY(1,1) NOT NULL,[日期] [smalldatetime] NULL,[商品ID] [int] NULL,[单位ID] [int] NULL,[库房ID] [int] NULL,[部门ID] [int] NULL,[业务员ID] [int] NULL,[原单据编号] [nvarchar](30) NULL,[单据编号] [nvarchar](30) NULL,[摘要] [nvarchar](50) NULL,[购进数量] [decimal](18, 0) NULL,[购进单价] [decimal](14, 2) NULL,[购进金额] [decimal](14, 2) NULL,[入库数量] [decimal](18, 0) NULL,[入库单价] [decimal](14, 2) NULL,[入库金额] [decimal](14, 2) NULL,[销售数量] [decimal](18, 0) NULL,[销售单价] [decimal](14, 2) NULL,[销售金额] [decimal](14, 2) NULL,[出库数量] [decimal](18, 0) NULL,[出库单价] [decimal](14, 2) NULL,[出库金额] [decimal](18, 0) NULL,[库房结存数量] [decimal](18, 0) NULL,[库房结存金额] [decimal](14, 2) NULL,[毛利] [decimal](18, 0) NULL,[结算数量] [decimal](18, 0) NULL,[结算单价] [decimal](14, 2) NULL,[结算金额] [decimal](14, 2) NULL,[退出数量] [decimal](18, 0) NULL,[退出单价] [decimal](14, 2) NULL,[退出金额] [decimal](14, 2) NULL,[退回数量] [decimal](18, 0) NULL,[退回单价] [decimal](14, 2) NULL,[退回金额] [decimal](14, 2) NULL,[开票金额] [decimal](14, 2) NULL,[应付金额] [decimal](18, 0) NULL,[未付金额] [decimal](18, 0) NULL,[已付金额] [decimal](18, 0) NULL,[应收金额] [decimal](14, 2) NULL,[未收金额] [decimal](18, 0) NULL,[已收金额] [decimal](14, 2) NULL,[销售退补价数量] [decimal](18, 0) NULL,[销售退补价单价] [decimal](14, 2) NULL,[销售退补价金额] [decimal](14, 2) NULL,[盘损数量] [decimal](18, 0) NULL,[盘损单价] [decimal](14, 2) NULL,[盘损金额] [decimal](14, 2) NULL,[报损数量] [decimal](18, 0) NULL,[报损单价] [decimal](14, 2) NULL,[报损金额] [decimal](14, 2) NULL,[借物数量] [decimal](18, 0) NULL,[借物单价] [decimal](14, 2) NULL,[借物金额] [decimal](14, 2) NULL,[BeActive] [bit] NULL,[购进退补价数量] [decimal](18, 0) NULL,[购进退补价单价] [decimal](14, 2) NULL,[购进退补价金额] [decimal](14, 2) NULL,[组装数量] [decimal](18, 0) NULL,[组装单价] [decimal](14, 2) NULL,[组装金额] [decimal](14, 2) NULL,CONSTRAINT [PK_商品库房历史账表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[商品分类表]([ID] [int] IDENTITY(1,1) NOT NULL,[分类编号] [nvarchar](20) NOT NULL,[分类名称] [nvarchar](50) NULL,[上级分类] [nvarchar](50) NULL,[库房ID] [int] NULL,[BeActive] [bit] NULL,CONSTRAINT [PK_商品分类表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[商品表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[dspid] [char](11) NULL,	[商品编号] [nvarchar](30) NULL,	[商品名称] [nvarchar](50) NULL,	[助记码] [nvarchar](50) NULL,	[计量单位] [nvarchar](4) NULL,	[计量规格] [int] NULL,	[最小计量单位] [nvarchar](4) NULL,	[进项税率] [decimal](12, 2) NULL,	[销项税率] [decimal](12, 2) NULL,	[零售价] [decimal](14, 2) NULL,	[进价] [decimal](14, 2) NULL,	[含税进价] [decimal](14, 2) NULL,	[批发价] [decimal](14, 2) NULL,	[含税批发价] [decimal](14, 2) NULL,	[库存数量] [decimal](12, 0) NOT NULL,	[库存成本价] [decimal](14, 2) NOT NULL,	[库存金额] [decimal](12, 2) NOT NULL,	[库存件数] [decimal](10, 0) NULL,	[核算成本价] [decimal](14, 2) NULL,	[最高进价] [decimal](12, 3) NOT NULL,	[最低进价] [decimal](12, 3) NOT NULL,	[最终进价] [decimal](12, 3) NOT NULL,	[结转数量] [decimal](12, 0) NULL,	[结转件数] [decimal](10, 0) NULL,	[结转金额] [decimal](12, 2) NOT NULL,	[结转单价] [decimal](14, 2) NOT NULL,	[登录日期] [smalldatetime] NULL,	[提成比例] [decimal](12, 2) NULL,	[保质期天数] [int] NULL,	[保质期方式] [nvarchar](8) NULL,	[商品产地] [nvarchar](50) NULL,	[商品规格] [nvarchar](20) NULL,	[西文] [char](20) NULL,	[备注] [ntext] NULL,	[库存上限] [decimal](12, 0) NOT NULL,	[库存下限] [decimal](12, 0) NOT NULL,	[合理库存上限] [decimal](12, 0) NOT NULL,	[合理库存下限] [decimal](12, 0) NOT NULL,	[经代] [nvarchar](8) NULL,	[预警天数] [int] NULL,	[是否特价商品] [bit] NULL,	[是否会员商品] [bit] NULL,	[会员特价] [decimal](12, 3) NULL,	[销售方式] [int] NULL,	[限定批发价] [decimal](14, 2) NULL,	[包装数量] [int] NULL,	[商品条码] [nvarchar](50) NULL,	[分类编号] [int] NULL,	[组装商品] [bit] NULL,	[beactive] [bit] NULL,	[应付金额] [decimal](18, 2) NOT NULL,	[已付金额] [decimal](18, 2) NOT NULL,	[应收金额] [decimal](14, 2) NOT NULL,	[已收金额] [decimal](14, 2) NOT NULL, CONSTRAINT [PK_商品表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[备注修改记录表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[日期] [smalldatetime] NULL,	[操作员ID] [int] NULL,	[单号] [nvarchar](30) NULL,	[原备注] [nvarchar](200) NULL,	[备注] [nvarchar](200) NULL,	[原备注] [nvarchar](200) NULL,	[备注] [nvarchar](200) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[销售商品制单明细表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[表单ID] [int] NOT NULL,	[商品ID] [int] NOT NULL,	[库房ID] [int] NULL,	[合同ID] [int] NULL,	[日期] [smalldatetime] NULL,	[保质期] [smalldatetime] NULL,	[单位] [nvarchar](10) NULL,	[规格] [int] NULL,	[数量] [decimal](12, 2) NULL,	[含税价] [decimal](14, 3) NULL,	[单价] [decimal](12, 3) NULL,	[金额] [decimal](14, 2) NULL,	[税率] [decimal](7, 2) NULL,	[税额] [decimal](14, 2) NULL,	[扣率] [decimal](7, 2) NULL,	[实计金额] [decimal](14, 2) NULL,	[零售价] [decimal](14, 2) NULL,	[毛利] [decimal](14, 2) NULL,	[赠品] [bit] NULL,	[批零差价] [decimal](14, 2) NULL,	[未出库数量] [decimal](18, 0) NULL,	[已出库数量] [decimal](18, 0) NULL,	[BeActive] [bit] NULL,	[校对标志] [bit] NULL,	[未付款金额] [decimal](14, 2) NULL,	[已付款金额] [decimal](14, 2) NULL,	[未付款数量] [decimal](14, 2) NULL,	[已付款数量] [decimal](14, 2) NULL,	[库存成本价] [decimal](14, 2) NULL, CONSTRAINT [PK_销售商品制单明细表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[销售商品制单表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[单据编号] [nvarchar](20) NOT NULL,	[单位ID] [int] NOT NULL,	[日期] [smalldatetime] NULL,	[金额] [decimal](14, 2) NULL,	[税额] [decimal](14, 2) NULL,	[部门ID] [int] NULL,	[业务员ID] [int] NULL,	[操作员ID] [int] NULL,	[付款方式] [int] NULL,	[发票号] [nvarchar](200) NULL,	[合同ID] [int] NULL,	[备注] [nvarchar](50) NULL,	[价税合计] [decimal](14, 2) NULL,	[出库标记] [bit] NULL,	[BeActive] [bit] NULL,	[联系电话] [nvarchar](50) NULL,	[联系人] [nvarchar](20) NULL,	[收货人] [nvarchar](20) NULL,	[到站] [nvarchar](20) NULL,	[运输方式] [nvarchar](50) NULL,	[详细地址] [nvarchar](100) NULL,	[物流名称] [nvarchar](100) NULL,	[单号] [nvarchar](30) NULL,	[邮政编码] [nvarchar](6) NULL,	[付款标记] [bit] NULL,	[未付款金额] [decimal](14, 2) NULL,	[已付款金额] [decimal](14, 2) NULL,	[结清时间] [smalldatetime] NULL,	[冲红时间] [smalldatetime] NULL, CONSTRAINT [PK_销售商品制单表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[销售商品定义表](	[ID] [int] NOT NULL,	[库存成本价] [decimal](14, 2) NULL,	[成本金额] [decimal](14, 2) NULL,	[统计标志] [int] NULL,	[保留] [bit] NULL, CONSTRAINT [PK_销售商品定义表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[销售合同明细表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[销售合同ID] [int] NOT NULL,	[商品ID] [int] NULL,	[数量] [int] NULL,	[单价] [decimal](18, 2) NULL,	[总价] [decimal](18, 2) NULL,	[备注] [ntext] NULL, CONSTRAINT [PK_销售合同明细表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[销售合同表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[合同编号] [nvarchar](20) NOT NULL,	[供方单位ID] [int] NULL,	[业务员ID] [int] NULL,	[操作员ID] [int] NULL,	[签订地点] [nvarchar](50) NULL,	[签订时间] [smalldatetime] NULL,	[质量要求] [nvarchar](50) NULL,	[异议期限] [nvarchar](50) NULL,	[质量负责] [nvarchar](50) NULL,	[随机备品] [nvarchar](50) NULL,	[运输方式] [nvarchar](50) NULL,	[运输至] [nvarchar](50) NULL,	[费用承担] [nvarchar](10) NULL,	[交货地点] [nvarchar](50) NULL,	[现货交货时间] [nvarchar](50) NULL,	[现货付款方式] [nvarchar](50) NULL,	[期货交货时间] [nvarchar](50) NULL,	[预付金额] [nvarchar](50) NULL,	[提货应付余额] [nvarchar](50) NULL,	[期货付款方式] [nvarchar](50) NULL,	[违约责任] [nvarchar](50) NULL,	[仲裁委员会] [nvarchar](50) NULL,	[其他约定事项] [nvarchar](50) NULL,	[合同有效期] [nvarchar](50) NULL,	[BeActive] [bit] NULL,	[退货标记] [bit] NULL,	[执行标记] [bit] NULL,	[金额] [decimal](14, 2) NULL,	[供方单位名称] [nvarchar](50) NULL,	[供方税号] [nchar](50) NULL,	[供方电话] [nvarchar](50) NULL,	[供方开户银行] [nvarchar](50) NULL,	[供方银行账号] [char](50) NULL,	[供方联系人] [nvarchar](50) NULL,	[供方地址] [nvarchar](100) NULL,	[供方传真] [nvarchar](50) NULL,	[供方邮编] [char](6) NULL,	[需方单位名称] [nvarchar](50) NULL,	[需方税号] [nchar](50) NULL,	[需方电话] [nvarchar](50) NULL,	[需方开户银行] [nvarchar](50) NULL,	[需方银行账号] [char](50) NULL,	[需方联系人] [nvarchar](50) NULL,	[需方地址] [nvarchar](100) NULL,	[需方传真] [nvarchar](50) NULL,	[需方邮编] [char](6) NULL,	[部门ID] [int] NULL,	[冲红时间] [smalldatetime] NULL, CONSTRAINT [PK_销售合同表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[销售出库明细表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[单据ID] [int] NOT NULL,	[商品ID] [int] NOT NULL,	[库房ID] [int] NOT NULL,	[原单据ID] [int] NULL,	[单位] [char](4) NULL,	[规格] [int] NULL,	[数量] [decimal](12, 2) NULL,	[件数] [decimal](10, 2) NULL,	[含税价] [decimal](14, 3) NULL,	[单价] [decimal](12, 3) NULL,	[金额] [decimal](14, 2) NULL,	[税率] [decimal](7, 2) NULL,	[税额] [decimal](14, 2) NULL,	[扣率] [decimal](7, 2) NULL,	[零售价] [decimal](14, 2) NULL,	[毛利] [decimal](14, 2) NULL,	[赠品] [bit] NULL,	[库存成本价] [decimal](14, 2) NULL,	[批零差价] [decimal](14, 2) NULL,	[实计金额] [decimal](14, 2) NULL,	[BeActive] [bit] NULL,	[未付款金额] [decimal](14, 2) NULL,	[已付款金额] [decimal](14, 2) NULL,	[未付款数量] [decimal](14, 2) NULL,	[已付款数量] [decimal](14, 2) NULL,	[原单据明细ID] [int] NULL, CONSTRAINT [PK_销售出库明细表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[销售出库汇总表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[单位ID] [int] NOT NULL,	[单据编号] [nvarchar](20) NOT NULL,	[日期] [smalldatetime] NULL,	[发票号] [varchar](200) NULL,	[支票号] [varchar](200) NULL,	[合同号] [nvarchar](20) NULL,	[价税合计] [decimal](14, 2) NULL,	[业务员ID] [int] NULL,	[操作员ID] [int] NULL,	[未付款金额] [decimal](14, 2) NULL,	[已付款金额] [decimal](14, 2) NULL,	[付款标记] [bit] NULL,	[备注] [nvarchar](200) NULL,	[BeActive] [bit] NULL,	[结清时间] [smalldatetime] NULL,	[销售ID] [int] NULL,	[部门ID] [int] NULL,	[冲红时间] [smalldatetime] NULL, CONSTRAINT [PK_销售出库汇总表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[系统参数表](	[ID] [int] NOT NULL,	[公司名] [nvarchar](100) NULL,	[地址] [nvarchar](100) NULL,	[电话] [nvarchar](50) NULL,	[传真] [nvarchar](50) NULL,	[税号] [nvarchar](50) NULL,	[开户银行] [nvarchar](100) NULL,	[帐号] [nvarchar](100) NULL,	[邮政编码] [nvarchar](50) NULL,	[开始时间] [smalldatetime] NULL,	[负责人] [nvarchar](100) NULL,	[公司宣传] [nvarchar](50) NULL,	[质量目标] [nvarchar](50) NULL,	[质量目标] [nvarchar](50) NULL,	[质量目标] [nvarchar](50) NULL,	[质量目标] [nvarchar](50) NULL,	[管理员权限] [int] NULL,	[总经理权限] [int] NULL,	[职员权限] [int] NULL,	[经理权限] [int] NULL,	[业务员权限] [int] NULL, CONSTRAINT [PK_系统参数表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[送货信息修改表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[单据编号] [nvarchar](20) NOT NULL,	[修改单据ID] [int] NOT NULL,	[日期] [smalldatetime] NULL,	[部门ID] [int] NULL,	[业务员ID] [int] NULL,	[操作员ID] [int] NULL,	[原备注] [nvarchar](50) NULL,	[原联系电话] [nvarchar](50) NULL,	[原联系人] [nvarchar](20) NULL,	[原收货人] [nvarchar](20) NULL,	[原到站] [nvarchar](20) NULL,	[原运输方式] [nvarchar](50) NULL,	[原详细地址] [nvarchar](100) NULL,	[原物流名称] [nvarchar](100) NULL,	[原单号] [nvarchar](30) NULL,	[原邮政编码] [nvarchar](6) NULL,	[BeActive] [bit] NULL,	[备注] [nvarchar](50) NULL,	[联系电话] [nvarchar](50) NULL,	[联系人] [nvarchar](20) NULL,	[收货人] [nvarchar](20) NULL,	[到站] [nvarchar](20) NULL,	[运输方式] [nvarchar](50) NULL,	[详细地址] [nvarchar](100) NULL,	[物流名称] [nvarchar](100) NULL,	[单号] [nvarchar](30) NULL,	[邮政编码] [nvarchar](6) NULL, CONSTRAINT [PK_送货信息修改表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[采购合同明细表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[采购合同ID] [int] NOT NULL,	[商品ID] [int] NULL,	[数量] [int] NULL,	[单价] [decimal](18, 2) NULL,	[总价] [decimal](18, 2) NULL,	[备注] [ntext] NULL, CONSTRAINT [PK_采购合同明细表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[采购合同定义表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[采购合同ID] [int] NULL,	[商品ID] [int] NULL,	[数量] [int] NULL,	[单价] [decimal](18, 2) NULL,	[总价] [decimal](18, 2) NULL,	[备注] [ntext] NULL, CONSTRAINT [PK_采购合同定义表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[采购合同表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[合同编号] [nvarchar](20) NOT NULL,	[供方单位ID] [int] NULL,	[业务员ID] [int] NULL,	[操作员ID] [int] NULL,	[签订地点] [nvarchar](50) NULL,	[签订时间] [smalldatetime] NULL,	[质量要求] [nvarchar](50) NULL,	[异议期限] [nvarchar](50) NULL,	[质量负责] [nvarchar](50) NULL,	[随机备品] [nvarchar](50) NULL,	[运输方式] [nvarchar](50) NULL,	[运输至] [nvarchar](50) NULL,	[费用承担] [nvarchar](10) NULL,	[交货地点] [nvarchar](50) NULL,	[现货交货时间] [nvarchar](50) NULL,	[现货付款方式] [nvarchar](50) NULL,	[期货交货时间] [nvarchar](50) NULL,	[预付金额] [nvarchar](50) NULL,	[提货应付余额] [nvarchar](50) NULL,	[期货付款方式] [nvarchar](50) NULL,	[违约责任] [nvarchar](50) NULL,	[仲裁委员会] [nvarchar](50) NULL,	[其他约定事项] [nvarchar](50) NULL,	[合同有效期] [nvarchar](50) NULL,	[BeActive] [bit] NULL,	[退货标记] [bit] NULL,	[执行标记] [bit] NULL,	[金额] [decimal](14, 2) NULL,	[供方单位名称] [nvarchar](50) NULL,	[供方税号] [nchar](50) NULL,	[供方电话] [nvarchar](50) NULL,	[供方开户银行] [nvarchar](50) NULL,	[供方银行账号] [char](50) NULL,	[供方联系人] [nvarchar](50) NULL,	[供方地址] [nvarchar](100) NULL,	[供方传真] [nvarchar](20) NULL,	[供方邮编] [char](6) NULL,	[需方单位名称] [nvarchar](50) NULL,	[需方税号] [nchar](50) NULL,	[需方电话] [nvarchar](50) NULL,	[需方开户银行] [nvarchar](50) NULL,	[需方银行账号] [char](50) NULL,	[需方联系人] [nvarchar](50) NULL,	[需方地址] [nvarchar](100) NULL,	[需方传真] [nvarchar](50) NULL,	[需方邮编] [char](6) NULL,	[部门ID] [int] NULL,	[冲红时间] [smalldatetime] NULL, CONSTRAINT [PK_采购合同表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[部门表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[dbmid] [char](11) NULL,	[部门编号] [nvarchar](50) NULL,	[部门名称] [nvarchar](50) NULL,	[助记码] [char](20) NULL,	[部门职责] [nvarchar](200) NULL,	[是否零售] [bit] NULL,	[是否批发] [bit] NULL,	[是否配送] [bit] NULL,	[BeActive] [bit] NULL, CONSTRAINT [PK_部门表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[表单计数表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[时间] [smalldatetime] NULL,	[关键词] [nvarchar](50) NULL,	[计数] [int] NULL, CONSTRAINT [PK_表单计数表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[职员表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[dzyid] [char](11) NULL,	[职员编号] [char](8) NOT NULL,	[职员姓名] [nvarchar](20) NULL,	[PASSWORD] [nvarchar](20) NOT NULL,	[出生日期] [datetime] NULL,	[文化程度] [nvarchar](30) NULL,	[身份证号] [char](18) NULL,	[助记码] [char](20) NULL,	[职员职务] [nvarchar](20) NULL,	[职员职称] [nvarchar](20) NULL,	[职员专业] [nvarchar](20) NULL,	[性别] [nvarchar](4) NULL,	[岗位ID] [int] NULL,	[管理级别] [int] NULL,	[是否操作员] [bit] NULL,	[是否业务员] [bit] NULL,	[是否销售员] [bit] NULL,	[是否收款员] [bit] NULL,	[职员电话] [char](40) NULL,	[家庭地址] [nvarchar](60) NULL,	[毕业时间] [datetime] NULL,	[毕业学校] [nvarchar](50) NULL,	[备注] [ntext] NULL,	[BeActive] [bit] NULL,	[部门ID] [int] NULL,	[登录状态] [varchar](50) NULL, CONSTRAINT [PK_职员表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[支付方式表](	[ID] [int] IDENTITY(1,1) NOT NULL,	[支付方式] [nvarchar](12) NULL, CONSTRAINT [PK_支付方式表] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[库存商品账目处理汇总表](	[ID] [int] IDENTITY(1,1) NOT NULL,[单据编号] [nvarchar](20) NOT NULL,	[日期] [smalldatetime] NULL,[业务员ID] [int] NULL,	[操作员ID] [int] NULL,[备注] [nvarchar](200) NULL,[BeActive] [bit] NULL, CONSTRAINT [PK_库存商品账目处理汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[库存商品拆散汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据编号] [nvarchar](30) NULL,[日期] [smalldatetime] NULL,[组装单据ID] [int] NULL,[拆散数量] [decimal](18, 0) NULL,[备注] [nvarchar](200) NULL,[BeActive] [bit] NULL,[操作员ID] [int] NULL,[业务员ID] [int] NULL, CONSTRAINT [PK_库存商品拆散汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[账簿表]([ID] [int] IDENTITY(1,1) NOT NULL,[账簿ID] [nvarchar](11) NULL,[账簿编号] [nvarchar](11) NOT NULL,[账簿名称] [nvarchar](20) NULL,[助记码] [nvarchar](20) NULL,[方向] [nvarchar](2) NULL,[是否可支付] [bit] NULL,[扣率] [decimal](7, 2) NOT NULL,[使用方式] [nvarchar](4) NULL,[提示信息] [nvarchar](20) NULL,[结清] [bit] NULL,[BeActive] [bit] NULL, CONSTRAINT [PK_账簿表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[销售退出明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据ID] [int] NOT NULL,[商品ID] [int] NOT NULL,[库房ID] [int] NOT NULL,[原单据ID] [int] NULL,[单位] [char](4) NULL,[规格] [int] NULL,[数量] [decimal](12, 2) NULL,[件数] [decimal](10, 2) NULL,[含税价] [decimal](14, 3) NULL,[单价] [decimal](12, 3) NULL,[金额] [decimal](14, 2) NULL,[税率] [decimal](7, 2) NULL,[税额] [decimal](14, 2) NULL,[扣率] [decimal](7, 2) NULL,[零售价] [decimal](14, 2) NULL,[毛利] [decimal](14, 2) NULL,[赠品] [bit] NULL,[零售金额] [decimal](14, 2) NULL,[批零差价] [decimal](14, 2) NULL,[实计金额] [decimal](14, 2) NULL,[未付款金额] [decimal](18, 0) NULL,[已付款金额] [decimal](14, 2) NULL,[未付款数量] [decimal](14, 2) NULL,[已付款数量] [decimal](14, 2) NULL,[BeActive] [bit] NULL,[库存成本价] [decimal](14, 2) NULL, CONSTRAINT [PK_销售退出明细表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[权限码表]([权限码] [nvarchar](50) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[模块表]([ID] [int] IDENTITY(1,1) NOT NULL,[模块名称] [nvarchar](200) NULL,[模块代码] [nvarchar](20) NULL,[模块指针] [nvarchar](100) NULL,[上级ID] [int] NULL,[权限显示] [bit] NULL,[模块级别] [int] NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[销售退出汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[单位ID] [int] NOT NULL,[单据编号] [nvarchar](20) NOT NULL,[日期] [smalldatetime] NULL,[发票号] [varchar](200) NULL,[支票号] [varchar](200) NULL,[合同号] [nvarchar](20) NULL,[价税合计] [decimal](14, 2) NULL,[业务员ID] [int] NULL,[BeActive] [bit] NULL,[付款标记] [bit] NULL,[操作员ID] [int] NULL,[备注] [nvarchar](200) NULL,[未付款金额] [decimal](14, 2) NULL,[已付款金额] [decimal](14, 2) NULL,[结清时间] [smalldatetime] NULL,[部门ID] [int] NULL,[冲红时间] [smalldatetime] NULL, CONSTRAINT [PK_销售退出汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[日志表]([ID] [int] IDENTITY(1,1) NOT NULL,[日期] [smalldatetime] NULL,[操作员ID] [int] NULL,[摘要] [nvarchar](200) NULL, CONSTRAINT [PK_日志表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[模块权限表]([ID] [int] IDENTITY(1,1) NOT NULL,[岗位ID] [int] NULL,[模块ID] [int] NULL,[权限] [bit] NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[销售退补差价明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据ID] [int] NOT NULL,[商品ID] [int] NOT NULL,[库房ID] [int] NOT NULL,[补价数量] [decimal](12, 2) NULL,[差价] [decimal](12, 3) NULL,[金额] [decimal](14, 2) NULL,[BeActive] [bit] NULL,[未付款金额] [decimal](14, 2) NULL,[已付款金额] [decimal](14, 2) NULL,[未付款数量] [decimal](14, 2) NULL,[已付款数量] [decimal](14, 2) NULL, CONSTRAINT [PK_销售退补差价明细表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[库房表]([ID] [int] IDENTITY(1,1) NOT NULL,[dkfid] [char](11) NULL,[库房编号] [nvarchar](30) NOT NULL,[库房名称] [nvarchar](50) NULL,[助记码] [nvarchar](20) NULL,[管理对象] [nvarchar](20) NULL,[管理面积] [int] NULL,[是否库房] [bit] NULL,[是否可配货] [bit] NULL,[是否柜组] [bit] NULL,[是否分店] [bit] NULL,[简称] [nvarchar](20) NULL,[对方标识] [nvarchar](4) NULL,[BeActive] [bit] NULL, CONSTRAINT [PK_库房表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[销售退补差价汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[单位ID] [int] NOT NULL,[单据编号] [nvarchar](20) NOT NULL,[日期] [smalldatetime] NULL,[发票号] [varchar](200) NULL,[支票号] [varchar](200) NULL,[合同号] [nvarchar](20) NULL,[价税合计] [decimal](14, 2) NULL,[业务员ID] [int] NULL,[操作员ID] [int] NULL,[BeActive] [bit] NULL,[付款标记] [bit] NULL,[备注] [nvarchar](200) NULL,[未付款金额] [decimal](14, 2) NULL,[已付款金额] [decimal](14, 2) NULL,[结清时间] [smalldatetime] NULL,[部门ID] [int] NULL,[冲红时间] [smalldatetime] NULL, CONSTRAINT [PK_销售退补差价汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[库存商品组装明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据ID] [int] NULL,[组件ID] [int] NULL,[库房ID] [int] NULL,[组件数量] [decimal](18, 0) NULL,[成本单价] [decimal](14, 2) NULL,[成本金额] [decimal](14, 2) NULL,[备注] [nvarchar](50) NULL, CONSTRAINT [PK_库存商品组装明细表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[库存商品账目处理明细表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据ID] [int] NULL,[商品ID] [int] NULL,[库房ID] [int] NULL,[原库存成本价] [decimal](18, 0) NULL,[原库存数量] [decimal](14, 2) NULL,[原库存金额] [decimal](18, 0) NULL,[库存成本价] [decimal](18, 0) NULL,[库存数量] [decimal](14, 2) NULL,[库存金额] [decimal](18, 0) NULL, CONSTRAINT [PK_库存商品账目处理明细表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[库存商品组装汇总表]([ID] [int] IDENTITY(1,1) NOT NULL,[单据编号] [nvarchar](30) NULL,[日期] [smalldatetime] NULL,[成品库房ID] [int] NULL,[商品ID] [int] NULL,[商品编号] [nvarchar](30) NULL,[商品名称] [nvarchar](50) NULL,[商品数量] [decimal](18, 0) NULL,[组装费用] [decimal](14, 2) NULL,[备注] [nvarchar](200) NULL,[BeActive] [bit] NULL,[操作员ID] [int] NULL,[业务员ID] [int] NULL, CONSTRAINT [PK_库存商品组装汇总表] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[库存商品组装定义表]([库存量] [decimal](18, 0) NULL,[统计标志] [smallint] NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                //视图
                sqlComm.CommandText = "CREATE VIEW [dbo].[入库视图]AS(SELECT     dbo.进货入库汇总表.ID, dbo.进货入库汇总表.日期, dbo.进货入库明细表.商品ID, dbo.进货入库明细表.库房ID, dbo.进货入库明细表.数量, dbo.进货入库明细表.单价,                       dbo.进货入库明细表.实计金额 AS 金额, 进货入库明细表.ID AS 明细IDFROM         dbo.进货入库明细表 INNER JOIN                      dbo.进货入库汇总表 ON dbo.进货入库明细表.单据ID = dbo.进货入库汇总表.IDWHERE     (dbo.进货入库汇总表.BeActive = 1))UNION(SELECT     借物出库汇总表.ID, 借物出库汇总表.日期, 借物出库明细表.商品ID, 借物出库明细表.库房ID, ABS(借物出库明细表.数量) AS 数量, 借物出库明细表.单价,                         ABS(借物出库明细表.出库金额), 借物出库明细表.ID FROM         借物出库汇总表 INNER JOIN                        借物出库明细表 ON 借物出库汇总表.ID = 借物出库明细表.表单ID WHERE     (借物出库汇总表.BeActive = 1) AND (借物出库明细表.数量 < 0))UNION(SELECT     进货退出汇总表.ID, 进货退出汇总表.日期, 进货退出明细表.商品ID, 进货退出明细表.库房ID, - (1 * 进货退出明细表.数量) AS Expr1, 进货退出明细表.单价,                         - (1 * 进货退出明细表.实计金额) AS Expr2, 进货退出明细表.ID FROM         进货退出汇总表 INNER JOIN                        进货退出明细表 ON 进货退出汇总表.ID = 进货退出明细表.单据ID WHERE     (进货退出汇总表.BeActive = 1))";
                sqlComm.ExecuteNonQuery();


                sqlComm.CommandText = "CREATE VIEW [dbo].[单据明细汇总视图]AS(SELECT     购进商品制单明细表.ID, 购进商品制单表.单据编号, 购进商品制单明细表.表单ID, 购进商品制单表.日期, 购进商品制单明细表.商品ID, 购进商品制单明细表.库房ID,                       购进商品制单明细表.数量, 购进商品制单明细表.单价, 购进商品制单明细表.实计金额, 0 AS 毛利, 购进商品制单表.BeActiveFROM         购进商品制单表 INNER JOIN                      购进商品制单明细表 ON 购进商品制单表.ID = 购进商品制单明细表.表单ID)UNION(SELECT     借物出库明细表.ID, 借物出库汇总表.单据编号, 借物出库明细表.表单ID, 借物出库汇总表.日期, 借物出库明细表.商品ID, 借物出库明细表.库房ID,                         借物出库明细表.数量, 借物出库明细表.单价, 借物出库明细表.出库金额, 0, 借物出库汇总表.BeActive FROM         借物出库明细表 INNER JOIN                        借物出库汇总表 ON 借物出库明细表.表单ID = 借物出库汇总表.ID)UNION(SELECT     购进退补差价明细表.ID, 购进退补差价汇总表.单据编号, 购进退补差价明细表.单据ID, 购进退补差价汇总表.日期, 购进退补差价明细表.商品ID,                         购进退补差价明细表.库房ID, 购进退补差价明细表.补价数量, 购进退补差价明细表.差价, 购进退补差价明细表.金额, 购进退补差价明细表.金额,                         购进退补差价汇总表.BeActive FROM         购进退补差价明细表 INNER JOIN                        购进退补差价汇总表 ON 购进退补差价明细表.单据ID = 购进退补差价汇总表.ID)UNION(SELECT     进货入库明细表.ID, 进货入库汇总表.单据编号, 进货入库明细表.单据ID, 进货入库汇总表.日期, 进货入库明细表.商品ID, 进货入库明细表.库房ID,                         进货入库明细表.数量, 进货入库明细表.单价, 进货入库明细表.实计金额, 0, 进货入库汇总表.BeActive FROM         进货入库明细表 INNER JOIN                        进货入库汇总表 ON 进货入库明细表.单据ID = 进货入库汇总表.ID)UNION(SELECT     进货退出明细表.ID, 进货退出汇总表.单据编号, 进货退出明细表.单据ID, 进货退出汇总表.日期, 进货退出明细表.商品ID, 进货退出明细表.库房ID,                         进货退出明细表.数量, 进货退出明细表.单价, 进货退出明细表.实计金额, 0, 进货退出汇总表.BeActive FROM         进货退出明细表 INNER JOIN                        进货退出汇总表 ON 进货退出明细表.单据ID = 进货退出汇总表.ID)UNION(SELECT     销售出库明细表.ID, 销售出库汇总表.单据编号, 销售出库明细表.单据ID, 销售出库汇总表.日期, 销售出库明细表.商品ID, 销售出库明细表.库房ID,                         销售出库明细表.数量, 销售出库明细表.单价, 销售出库明细表.实计金额, 0, 销售出库汇总表.BeActive FROM         销售出库明细表 INNER JOIN                        销售出库汇总表 ON 销售出库明细表.单据ID = 销售出库汇总表.ID)UNION(SELECT     销售商品制单明细表.ID, 销售商品制单表.单据编号, 销售商品制单明细表.表单ID, 销售商品制单表.日期, 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID,                         销售商品制单明细表.数量, 销售商品制单明细表.单价, 销售商品制单明细表.实计金额, 销售商品制单明细表.毛利, 销售商品制单表.BeActive FROM         销售商品制单表 INNER JOIN                        销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID)UNION(SELECT     销售退出明细表.ID, 销售退出汇总表.单据编号, 销售退出明细表.单据ID, 销售退出汇总表.日期, 销售退出明细表.商品ID, 销售退出明细表.库房ID,                         销售退出明细表.数量, 销售退出明细表.单价, 销售退出明细表.实计金额, - 1.0 * (销售退出明细表.实计金额 - 销售退出明细表.数量 * 销售退出明细表.库存成本价),                         销售退出汇总表.BeActive FROM         销售退出明细表 INNER JOIN                        销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID)UNION(SELECT     销售退补差价明细表.ID, 销售退补差价汇总表.单据编号, 销售退补差价明细表.单据ID, 销售退补差价汇总表.日期, 销售退补差价明细表.商品ID,                         销售退补差价明细表.库房ID, 销售退补差价明细表.补价数量, 销售退补差价明细表.差价, 销售退补差价明细表.金额, 销售退补差价明细表.金额,                         销售退补差价汇总表.BeActive FROM         销售退补差价明细表 INNER JOIN                        销售退补差价汇总表 ON 销售退补差价明细表.单据ID = 销售退补差价汇总表.ID)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[付款明细视图]AS(SELECT dbo.进货入库汇总表.单位ID, dbo.进货入库汇总表.单据编号,       dbo.进货入库汇总表.日期, dbo.商品表.商品编号, dbo.商品表.商品名称,       dbo.进货入库明细表.数量, dbo.进货入库明细表.实计金额,       dbo.进货入库明细表.未付款金额, dbo.进货入库明细表.已付款金额,       dbo.进货入库明细表.未付款数量, dbo.进货入库明细表.已付款数量,       dbo.进货入库明细表.单据ID, dbo.进货入库明细表.ID, dbo.进货入库明细表.商品ID,       dbo.商品表.分类编号, dbo.进货入库明细表.库房ID, dbo.进货入库汇总表.备注 , dbo.进货入库汇总表.业务员ID FROM dbo.进货入库汇总表 INNER JOIN      dbo.进货入库明细表 ON       dbo.进货入库汇总表.ID = dbo.进货入库明细表.单据ID INNER JOIN      dbo.商品表 ON dbo.进货入库明细表.商品ID = dbo.商品表.IDWHERE (dbo.进货入库明细表.BeActive = 1) AND (dbo.进货入库汇总表.BeActive = 1))UNION(SELECT 进货退出汇总表.单位ID, 进货退出汇总表.单据编号, 进货退出汇总表.日期,       商品表.商品编号, 商品表.商品名称, 进货退出明细表.数量, 进货退出明细表.实计金额*-1,       进货退出明细表.未付款金额*-1, 进货退出明细表.已付款金额*-1,       进货退出明细表.未付款数量, 进货退出明细表.已付款数量, 进货退出明细表.单据ID,       进货退出明细表.ID, 进货退出明细表.商品ID, 商品表.分类编号,       进货退出明细表.库房ID, 进货退出汇总表.备注 , 进货退出汇总表.业务员ID FROM 商品表 INNER JOIN      进货退出明细表 ON 商品表.ID = 进货退出明细表.商品ID INNER JOIN      进货退出汇总表 ON 进货退出明细表.单据ID = 进货退出汇总表.ID INNER JOIN      单位表 ON 进货退出汇总表.单位ID = 单位表.IDWHERE (进货退出明细表.BeActive = 1) AND (进货退出汇总表.BeActive = 1))UNION(SELECT 购进退补差价汇总表.单位ID, 购进退补差价汇总表.单据编号,       购进退补差价汇总表.日期, 商品表.商品编号, 商品表.商品名称,       购进退补差价明细表.补价数量 AS 数量, 购进退补差价明细表.金额,       购进退补差价明细表.未付款金额, 购进退补差价明细表.已付款金额,       购进退补差价明细表.未付款数量, 购进退补差价明细表.已付款数量,       购进退补差价明细表.单据ID, 购进退补差价明细表.ID, 购进退补差价明细表.商品ID,       商品表.分类编号, 购进退补差价明细表.库房ID, 购进退补差价汇总表.备注 , 购进退补差价汇总表.业务员ID  FROM 购进退补差价明细表 INNER JOIN      购进退补差价汇总表 ON       购进退补差价明细表.单据ID = 购进退补差价汇总表.ID INNER JOIN      商品表 ON 购进退补差价明细表.商品ID = 商品表.ID INNER JOIN      单位表 ON 购进退补差价汇总表.单位ID = 单位表.IDWHERE (购进退补差价汇总表.BeActive = 1) AND (购进退补差价明细表.BeActive = 1))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[单据汇总视图]AS(SELECT ID, 单据编号, 单位ID, 日期, 部门ID, 业务员ID, 操作员ID, 价税合计, BeActiveFROM dbo.购进商品制单表)UNION(SELECT ID, 单据编号, 单位ID, 日期, 部门ID, 业务员ID, 操作员ID, 价税合计, BeActiveFROM 借物出库汇总表)UNION(SELECT ID, 单据编号, 单位ID, 日期, 部门ID, 业务员ID, 操作员ID, 价税合计, BeActiveFROM 购进退补差价汇总表)UNION(SELECT ID, 单据编号, 单位ID, 日期, 部门ID, 业务员ID, 操作员ID, 价税合计, BeActiveFROM 进货入库汇总表)UNION(SELECT ID, 单据编号, 单位ID, 日期, 部门ID, 业务员ID, 操作员ID, 价税合计, BeActiveFROM 进货退出汇总表)UNION(SELECT ID, 单据编号, 单位ID, 日期, 部门ID, 业务员ID, 操作员ID, 价税合计, BeActiveFROM 销售出库汇总表)UNION(SELECT ID, 单据编号, 单位ID, 日期, 部门ID, 业务员ID, 操作员ID, 价税合计, BeActiveFROM 销售商品制单表)UNION(SELECT ID, 单据编号, 单位ID, 日期, 部门ID, 业务员ID, 操作员ID, 价税合计, BeActiveFROM 销售退出汇总表)UNION(SELECT ID, 单据编号, 单位ID, 日期, 部门ID, 业务员ID, 操作员ID, 价税合计, BeActiveFROM 销售退补差价汇总表)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[付款汇总视图]AS(SELECT 单位ID, 单据编号, 日期, 价税合计, 已付款金额, 未付款金额, ID, 业务员ID, 操作员ID, 备注, BeActive FROM dbo.进货入库汇总表WHERE (BeActive = 1))UNION(SELECT 单位ID, 单据编号, 日期, 价税合计*-1, 已付款金额*-1, 未付款金额*-1, ID, 业务员ID, 操作员ID, 备注, BeActive  FROM 进货退出汇总表WHERE (BeActive = 1))UNION(SELECT 单位ID, 单据编号, 日期, 价税合计, 已付款金额, 未付款金额, ID, 业务员ID, 操作员ID, 备注 , BeActive FROM 购进退补差价汇总表WHERE (BeActive = 1))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[出库视图]AS(SELECT     销售商品制单表.ID, 销售商品制单表.日期, 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID, 销售商品制单明细表.数量, 销售商品制单明细表.单价,                       销售商品制单明细表.实计金额, 销售商品制单明细表.库存成本价, 销售商品制单明细表.ID AS 明细表IDFROM         销售商品制单表 INNER JOIN                      销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单IDWHERE     (销售商品制单表.BeActive = 1))UNION(SELECT     借物出库汇总表.ID, 借物出库汇总表.日期, 借物出库明细表.商品ID, 借物出库明细表.库房ID, ABS(借物出库明细表.数量) AS 数量, 借物出库明细表.单价,                         借物出库明细表.出库金额, 借物出库明细表.库存成本价, 借物出库明细表.ID FROM         借物出库汇总表 INNER JOIN                        借物出库明细表 ON 借物出库汇总表.ID = 借物出库明细表.表单ID WHERE     (借物出库汇总表.BeActive = 1) AND (借物出库明细表.数量 > 0))UNION(SELECT     销售退出汇总表.ID, 销售退出汇总表.日期, 销售退出明细表.商品ID, 销售退出明细表.库房ID, - (1 * 销售退出明细表.数量) AS Expr1, 销售退出明细表.单价,                         - (1 * 销售退出明细表.实计金额) AS Expr2, 销售退出明细表.库存成本价, 销售退出明细表.ID FROM         销售退出汇总表 INNER JOIN                        销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID WHERE     (销售退出汇总表.BeActive = 1))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[收款明细视图]AS(SELECT dbo.销售商品制单表.单位ID, dbo.销售商品制单表.单据编号,       dbo.销售商品制单表.日期, dbo.商品表.商品编号, dbo.商品表.商品名称,       dbo.销售商品制单明细表.数量, dbo.销售商品制单明细表.实计金额,       dbo.销售商品制单明细表.未付款金额, dbo.销售商品制单明细表.已付款金额,       dbo.销售商品制单明细表.未付款数量, dbo.销售商品制单明细表.已付款数量,       dbo.销售商品制单明细表.表单ID AS 单据ID, dbo.销售商品制单明细表.ID,       dbo.销售商品制单明细表.商品ID, dbo.商品表.分类编号,       dbo.销售商品制单明细表.库房ID,  dbo.销售商品制单表.备注,  dbo.销售商品制单表.业务员IDFROM dbo.销售商品制单表 INNER JOIN      dbo.销售商品制单明细表 ON       dbo.销售商品制单表.ID = dbo.销售商品制单明细表.表单ID INNER JOIN      dbo.商品表 ON dbo.销售商品制单明细表.商品ID = dbo.商品表.IDWHERE (dbo.销售商品制单明细表.BeActive = 1) AND (dbo.销售商品制单表.BeActive = 1))UNION(SELECT 销售退出汇总表.单位ID, 销售退出汇总表.单据编号, 销售退出汇总表.日期,       商品表.商品编号, 商品表.商品名称, 销售退出明细表.数量, 销售退出明细表.实计金额*-1,       销售退出明细表.未付款金额*-1, 销售退出明细表.已付款金额*-1,       销售退出明细表.未付款数量, 销售退出明细表.已付款数量, 销售退出明细表.单据ID,       销售退出明细表.ID, 销售退出明细表.商品ID, 商品表.分类编号,       销售退出明细表.库房ID, 销售退出汇总表.备注 ,  dbo.销售退出汇总表.业务员ID FROM 商品表 INNER JOIN      销售退出明细表 ON 商品表.ID = 销售退出明细表.商品ID INNER JOIN      销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID INNER JOIN      单位表 ON 销售退出汇总表.单位ID = 单位表.IDWHERE (销售退出明细表.BeActive = 1) AND (销售退出汇总表.BeActive = 1))UNION(SELECT 销售退补差价汇总表.单位ID, 销售退补差价汇总表.单据编号,       销售退补差价汇总表.日期, 商品表.商品编号, 商品表.商品名称,       销售退补差价明细表.补价数量 AS 数量, 销售退补差价明细表.金额,       销售退补差价明细表.未付款金额, 销售退补差价明细表.已付款金额,       销售退补差价明细表.未付款数量, 销售退补差价明细表.已付款数量,       销售退补差价明细表.单据ID, 销售退补差价明细表.ID, 销售退补差价明细表.商品ID,       商品表.分类编号, 销售退补差价明细表.库房ID,  销售退补差价汇总表.备注   ,  dbo.销售退补差价汇总表.业务员ID FROM 销售退补差价明细表 INNER JOIN      销售退补差价汇总表 ON       销售退补差价明细表.单据ID = 销售退补差价汇总表.ID INNER JOIN      商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN      单位表 ON 销售退补差价汇总表.单位ID = 单位表.IDWHERE (销售退补差价汇总表.BeActive = 1) AND (销售退补差价明细表.BeActive = 1))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[销售视图]AS(SELECT dbo.销售商品制单明细表.ID, dbo.销售商品制单明细表.表单ID,       dbo.销售商品制单表.单据编号, dbo.销售商品制单表.单位ID,       dbo.销售商品制单表.日期, dbo.销售商品制单表.业务员ID,       dbo.销售商品制单表.部门ID, dbo.销售商品制单明细表.商品ID,       dbo.销售商品制单明细表.库房ID, dbo.销售商品制单明细表.数量,       dbo.销售商品制单明细表.单价, dbo.销售商品制单明细表.实计金额,       dbo.销售商品制单明细表.毛利, dbo.销售商品制单明细表.未付款金额,       dbo.销售商品制单明细表.已付款金额, dbo.销售商品制单明细表.库存成本价,       dbo.销售商品制单表.BeActiveFROM dbo.销售商品制单表 INNER JOIN      dbo.销售商品制单明细表 ON       dbo.销售商品制单表.ID = dbo.销售商品制单明细表.表单ID)UNION(SELECT 销售退出明细表.ID, 销售退出明细表.单据ID, 销售退出汇总表.单据编号,       销售退出汇总表.单位ID, 销售退出汇总表.日期, 销售退出汇总表.业务员ID,       销售退出汇总表.部门ID, 销售退出明细表.商品ID, 销售退出明细表.库房ID,       - (1 * 销售退出明细表.数量) AS 数量, 销售退出明细表.单价,       - (1 * 销售退出明细表.金额) AS 金额, 销售退出明细表.毛利,       销售退出明细表.未付款金额, 销售退出明细表.已付款金额,       销售退出明细表.库存成本价, 销售退出汇总表.BeActiveFROM 销售退出汇总表 INNER JOIN      销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[毛利视图]AS(SELECT     销售商品制单明细表.ID, 销售商品制单明细表.表单ID, 销售商品制单表.单据编号,销售商品制单表.单位ID, 销售商品制单明细表.商品ID, 销售商品制单明细表.库房ID, 销售商品制单表.日期,                       销售商品制单表.部门ID, 销售商品制单表.业务员ID, 销售商品制单表.操作员ID, 销售商品制单明细表.数量, 销售商品制单明细表.单价,                       销售商品制单明细表.库存成本价, 销售商品制单明细表.实计金额 AS 金额, 销售商品制单明细表.毛利, 销售商品制单表.BeActiveFROM         销售商品制单表 INNER JOIN                      销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID)UNION(SELECT     销售退出明细表.ID, 销售退出明细表.单据ID, 销售退出汇总表.单据编号,  销售退出汇总表.单位ID,销售退出明细表.商品ID, 销售退出明细表.库房ID, 销售退出汇总表.日期,                         销售退出汇总表.部门ID, 销售退出汇总表.业务员ID, 销售退出汇总表.操作员ID, - (1.0 * 销售退出明细表.数量) AS 数量, 销售退出明细表.单价,                         销售退出明细表.库存成本价, - (1.0 * 销售退出明细表.实计金额) AS 实计金额, - (1.0 * (销售退出明细表.实计金额 - 销售退出明细表.数量 * 销售退出明细表.库存成本价))                         AS 毛利, 销售退出汇总表.BeActive FROM         销售退出明细表 INNER JOIN                        销售退出汇总表 ON 销售退出明细表.单据ID = 销售退出汇总表.ID)UNION(SELECT     TOP 200 销售退补差价明细表.ID, 销售退补差价明细表.单据ID, 销售退补差价汇总表.单据编号, 销售退补差价汇总表.单位ID,销售退补差价明细表.商品ID, 0 AS 库房ID, 销售退补差价汇总表.日期,                         销售退补差价汇总表.部门ID, 销售退补差价汇总表.业务员ID, 销售退补差价汇总表.操作员ID, 0, 销售退补差价明细表.差价, 0 AS 库存成本价,                         销售退补差价明细表.金额, 销售退补差价明细表.金额 AS 毛利, 销售退补差价明细表.BeActive FROM         销售退补差价明细表 INNER JOIN                        销售退补差价汇总表 ON 销售退补差价明细表.单据ID = 销售退补差价汇总表.ID)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[收款汇总视图]AS(SELECT 单位ID, 单据编号, 日期, 价税合计, 已付款金额, 未付款金额, ID, 业务员ID, 操作员ID, 备注 , BeActive FROM dbo.销售商品制单表WHERE (BeActive = 1))UNION(SELECT 单位ID, 单据编号, 日期, 价税合计*-1, 已付款金额*-1, 未付款金额*-1, ID, 业务员ID, 操作员ID, 备注 , BeActive FROM 销售退出汇总表WHERE (BeActive = 1))UNION(SELECT 单位ID, 单据编号, 日期, 价税合计, 已付款金额, 未付款金额, ID, 业务员ID, 操作员ID, 备注 , BeActive FROM 销售退补差价汇总表WHERE (BeActive = 1))";
                sqlComm.ExecuteNonQuery();


                
                sqlComm.CommandText = "ALTER TABLE [dbo].[商品表] ADD  CONSTRAINT [DF_商品表_应付金额]  DEFAULT (0) FOR [应付金额]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[商品表] ADD  CONSTRAINT [DF_商品表_已付金额]  DEFAULT (0) FOR [已付金额]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[商品表] ADD  CONSTRAINT [DF_商品表_应收金额]  DEFAULT (0) FOR [应收金额]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[商品表] ADD  CONSTRAINT [DF_商品表_已收金额]  DEFAULT (0) FOR [已收金额]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[销售出库汇总表] ADD  CONSTRAINT [DF_销售出库汇总表_销售ID]  DEFAULT (0) FOR [销售ID]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "ALTER TABLE [dbo].[销售合同表] ADD  CONSTRAINT [DF_销售合同表_退货标记]  DEFAULT (0) FOR [退货标记]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[销售合同表] ADD  CONSTRAINT [DF_销售合同表_退货标记]  DEFAULT (0) FOR [执行标记]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[销售退出汇总表] ADD  CONSTRAINT [DF_销售退出汇总表_付款标记]  DEFAULT (0) FOR [付款标记]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[销售退出汇总表] ADD  CONSTRAINT [DF_销售退出汇总表_未付款金额]  DEFAULT (0) FOR [未付款金额]";
                sqlComm.ExecuteNonQuery();


                sqlComm.CommandText = "ALTER TABLE [dbo].[销售退出汇总表] ADD  CONSTRAINT [DF_销售退出汇总表_已付款金额]  DEFAULT (0) FOR [已付款金额]";
                sqlComm.ExecuteNonQuery();

                //缺省值
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CG', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'XS', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CC', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'AKP', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'ADH', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'ATH', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'ATB', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'AYF', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'BKP', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'BCK', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'ZXG', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'BTH', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'BTB', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'BYS', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CPD', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CCK', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'ZCC', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CG', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CZZ', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CCS', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CBS', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CTZ', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO 表单计数表(时间, 关键词, 计数) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'ETJ', 1)";
                sqlComm.ExecuteNonQuery();


                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "";
                sqlComm.ExecuteNonQuery();

                //MessageBox.Show("数据库创建成功，请用sa重新登录系统" , "数据库", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dSet.Tables["数据库信息"].Rows[0][0] = textBoxServer.Text;
                dSet.Tables["数据库信息"].Rows[0][1] = textBoxUser.Text;

                if (checkBoxRember.Checked) //记住密码
                    dSet.Tables["数据库信息"].Rows[0][2] = textBoxPassword.Text;
                else
                    dSet.Tables["数据库信息"].Rows[0][2] = "";

                dSet.Tables["数据库信息"].Rows[0][3] = textBoxDatabase.Text;
                dSet.WriteXml(dFileName);
                

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库创建失败：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strConn = "";
            }
            finally
            {
                sqlConn.Close();
                this.Dispose();
            }
         

        }
    }
}