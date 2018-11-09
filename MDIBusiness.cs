using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Net;



namespace business
{
    public partial class MDIBusiness : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();
        private static ClassChangeSkins SkinClass = new ClassChangeSkins();

        private string strConn = "";

        private int intUserID = 0;
        private int intUserLimit = 0;
        private string strUserLimit = "";
        private string strUserName = "";
        private int intUserBM = 0;

        private string strDataBaseAddr = "";
        private string strDataBaseUser = "";
        private string strDataBasePass = "";
        private string strDataBaseName = "";

        private int childFormNumber = 0;

        private int iConstLimit = 18;
        
        //软件锁
        private int iVersion = 0;
        ClassSenseLock cSenseLock = new ClassSenseLock();

        public MDIBusiness()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            Form childForm = new Form();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childForm.MdiParent = this;
            childForm.Text = "窗口" + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                // TODO: 在此处添加打开文件的代码。
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
                // TODO: 在此处添加代码，将窗体的当前内容保存到一个文件中。
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: 使用 System.Windows.Forms.Clipboard 将所选的文本或图像插入到剪贴板
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: 使用 System.Windows.Forms.Clipboard 将所选的文本或图像插入到剪贴板
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: 使用 System.Windows.Forms.Clipboard.GetText() 或 System.Windows.Forms.GetData 从剪贴板中检索信息。
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formAbout frmAbout = new formAbout();
            frmAbout.ShowDialog();
        }

        private void MDIBusiness_Load(object sender, EventArgs e)
        {
            string dFileName = "";

            iVersion = 1;
            //检验狗
            /*
            if (cSenseLock.checkSenseLock() == 0)
            {
                iVersion = 1;
            }
            else
            {
                iVersion = 0;
                MessageBox.Show("软件锁读取错误，请设置软件锁，软件变为预览版", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            */


            //初始化皮肤信息
            SkinClass.AddSkinMenu(SkinKToolStripMenuItem);

            //时间显示
            timerClock.Start();
            //timerClock.Stop();

            //数据库设置
            dFileName = Directory.GetCurrentDirectory() + "\\appcon.xml";
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            if (File.Exists(dFileName)) //存在文件
            {
                dSet.ReadXml(dFileName);
                strConn = "workstation id=CY;packet size=4096;user id=" + dSet.Tables["数据库信息"].Rows[0][1].ToString() + ";password=" + dSet.Tables["数据库信息"].Rows[0][2].ToString() + ";data source=\"" + dSet.Tables["数据库信息"].Rows[0][0].ToString() + "\";;initial catalog=" + dSet.Tables["数据库信息"].Rows[0][3].ToString();

                strDataBaseAddr = dSet.Tables["数据库信息"].Rows[0][0].ToString();
                strDataBaseUser = dSet.Tables["数据库信息"].Rows[0][1].ToString();
                strDataBasePass = dSet.Tables["数据库信息"].Rows[0][2].ToString();
                strDataBaseName = dSet.Tables["数据库信息"].Rows[0][3].ToString();

                sqlConn.ConnectionString = strConn;
                try
                {
                    sqlConn.Open();
                    sqlDA.SelectCommand = sqlComm;

                    sqlComm.CommandText = "SELECT 公司名 FROM 系统参数表";
                    sqldr = sqlComm.ExecuteReader();
                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        this.Text = "商业管理（进销存）系统：" + sqldr.GetValue(0).ToString();
                        sqldr.Close();
                        if (iVersion == 0)
                            this.Text += " - 预览版";
                    }
                }
                catch (System.Data.SqlClient.SqlException err)
                {

                    bool isCreateDatabase = true;
                    if (MessageBox.Show("数据库连接错误，是否需要创建数据库？" + err.Message.ToString(), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        isCreateDatabase = false;

                    strConn = "";
                    formDatabaseSet frmDatabaseSet = new formDatabaseSet();
                    if (isCreateDatabase)
                        frmDatabaseSet.intMode = 1;

                    frmDatabaseSet.ShowDialog(this);
                    if (frmDatabaseSet.strConn != "")
                    {
                        strConn = frmDatabaseSet.strConn;
                        //初始化窗口
                        sqlConn.ConnectionString = strConn;


                        sqlComm.CommandText = "SELECT 公司名 FROM 系统参数表";
                        sqlConn.Open();
                        sqldr = sqlComm.ExecuteReader();
                        if (sqldr.HasRows)
                        {
                            sqldr.Read();
                            this.Text = "商业管理（进销存）系统：" + sqldr.GetValue(0).ToString();
                            sqldr.Close();
                            if (iVersion == 0)
                                this.Text += " - 预览版";
                        }
                        sqlConn.Close();
                    }
                    else
                    {
                        this.Close();
                        return;
                    }
                }
                finally
                {
                    sqlConn.Close();
                }

            }
            else  //不存在文件
            {
                formDatabaseSet frmDatabaseSet = new formDatabaseSet();
                frmDatabaseSet.ShowDialog(this);
                if (frmDatabaseSet.strConn != "")
                {
                    strConn = frmDatabaseSet.strConn;
                    //初始化窗口
                    sqlConn.ConnectionString = strConn;

                    sqlComm.CommandText = "SELECT 公司名 FROM 系统参数表";
                    sqlConn.Open();
                    sqldr = sqlComm.ExecuteReader();
                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        this.Text = "商业管理（进销存）系统：" + sqldr.GetValue(0).ToString();
                        sqldr.Close();
                        if (iVersion == 0)
                            this.Text += " - 预览版";
                    }
                    sqlConn.Close();
                }
                else
                {
                    this.Close();
                    return;
                }
            }

            //用户登录
            dSet.Tables.Clear();
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            dFileName = Directory.GetCurrentDirectory() + "\\login.xml";
            if (File.Exists(dFileName)) //存在文件
            {
                dSet.ReadXml(dFileName);

                if (dSet.Tables["登录信息"].Rows[0][2].ToString() == "1") //自动登录
                {
                    sqlConn.Open();
                    sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员姓名, 岗位表.权限, 岗位表.岗位名称, 职员表.PASSWORD, 职员表.部门ID, 职员表.登录状态 FROM 职员表 LEFT OUTER JOIN 岗位表 ON 职员表.岗位ID = 岗位表.ID WHERE (职员表.职员编号 = '" + dSet.Tables["登录信息"].Rows[0][0].ToString() + "') AND (职员表.PASSWORD = '" + dSet.Tables["登录信息"].Rows[0][1].ToString() + "') AND (职员表.BeActive = 1)";

                   // sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员姓名, 岗位表.权限, 岗位表.岗位名称D, 职员表.部门ID, 职员表.登录状态 FROM 职员表 LEFT OUTER JOIN 岗位表 ON 职员表.岗位ID = 岗位表.ID WHERE (职员表.职员编号 = '" + dSet.Tables["登录信息"].Rows[0][0].ToString() + "') AND (职员表.PASSWORD = '" + dSet.Tables["登录信息"].Rows[0][1].ToString() + "') AND (职员表.BeActive = 1)";
                    strDataBaseAddr = dSet.Tables["数据库信息"].Rows[0][0].ToString();
                    strDataBaseUser = dSet.Tables["数据库信息"].Rows[0][1].ToString();
                    strDataBasePass = dSet.Tables["数据库信息"].Rows[0][2].ToString();
                    strDataBaseName = dSet.Tables["数据库信息"].Rows[0][3].ToString();


                    sqldr = sqlComm.ExecuteReader();
                    string sTemp="";
                    if (sqldr.HasRows)
                    {
                        sqldr.Read();

                        if (sqldr.GetValue(6).ToString() != "") //已登录
                        {
                            if (sqldr.GetValue(6).ToString() == "1")
                                sTemp = sqldr.GetValue(1).ToString().Trim();
                            else
                                sTemp = sqldr.GetValue(1).ToString().Trim() + "-" + sqldr.GetValue(6).ToString().Trim();

                            if (Int32.Parse(sqldr.GetValue(2).ToString()) < 18)
                            {

                                MessageBox.Show("用户（" + sTemp + "）已登录系统，请与管理员联系！", "登录错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                sqldr.Close();
                                sqlConn.Close();

                                this.Close();
                                return;
                            }
                            else
                            {
                                if (MessageBox.Show("用户（" + sTemp + "）已登录系统，是否强行登录？", "登录错误", MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes)
                                {
                                    sqldr.Close();
                                    sqlConn.Close();

                                    this.Close();
                                    return;
                                }

                            }
                        }
                        intUserID = Int32.Parse(sqldr.GetValue(0).ToString());
                        strUserLimit = sqldr.GetValue(3).ToString().Trim();
                        if (sqldr.GetValue(2).ToString() == "") //无岗位
                            intUserLimit = 0;
                        else
                            intUserLimit = Int32.Parse(sqldr.GetValue(2).ToString());
                        strUserName = sqldr.GetValue(1).ToString().Trim();

                        if (sqldr.GetValue(5).ToString() == "") //无部门
                            intUserBM = 0;
                        else
                            intUserBM = Int32.Parse(sqldr.GetValue(5).ToString());

                        sqldr.Close();
                        sqlConn.Close();

                    }
                    else
                    {
                        MessageBox.Show("用户登录错误！", "登录错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sqlConn.Close();
                        formLogin frmLogin = new formLogin();
                        frmLogin.strConn = strConn;
                        frmLogin.ShowDialog();

                        if (frmLogin.intUserID != 0) //登录
                        {
                            intUserID = frmLogin.intUserID;
                            intUserLimit = frmLogin.intUserLimit;
                            strUserLimit = frmLogin.strUserLimit;
                            strUserName = frmLogin.strUserName;
                            intUserBM = frmLogin.intUserBM;
                        }
                        else //取消
                        {
                            this.Close();
                            return;
                        }

                    }
                }
                else //不自动登录
                {
                    formLogin frmLogin = new formLogin();
                    frmLogin.strConn = strConn;
                    frmLogin.ShowDialog();

                    if (frmLogin.intUserID != 0) //登录
                    {
                        intUserID = frmLogin.intUserID;
                        intUserLimit = frmLogin.intUserLimit;
                        strUserLimit = frmLogin.strUserLimit;
                        strUserName = frmLogin.strUserName;
                        intUserBM = frmLogin.intUserBM;
                    }
                    else //取消
                    {
                        this.Close();
                        return;
                    }
                }
            }
            else //不存在文件
            {
                formLogin frmLogin = new formLogin();
                frmLogin.strConn = strConn;
                frmLogin.ShowDialog();

                if (frmLogin.intUserID != 0) //登录
                {
                    intUserID = frmLogin.intUserID;
                    intUserLimit = frmLogin.intUserLimit;
                    strUserLimit = frmLogin.strUserLimit;
                    strUserName = frmLogin.strUserName;
                    intUserBM = frmLogin.intUserBM;
                }
                else //取消
                {
                    this.Close();
                    return;
                }
            }

            
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] IpAddr = ipEntry.AddressList; 
            string strIP = "1";

            for (int i = 0; i < IpAddr.Length; i++)
            {
                if (IpAddr[i].ToString().Length <= 16)
                {
                    strIP = IpAddr[i].ToString();
                    break;
                }
            } 

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 公司宣传, 质量目标1, 质量目标2, 质量目标3, 质量目标4, 管理员权限, 总经理权限, 职员权限, 经理权限, 业务员权限 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                try
                {
                    iConstLimit = int.Parse(sqldr.GetValue(6).ToString());
                }
                catch
                {
                    iConstLimit = 18;
                }
            }
            sqldr.Close();

            sqlComm.CommandText = "UPDATE 职员表 SET 登录状态 = N'"+strIP+"' WHERE (ID = " + intUserID.ToString() + ")";
            sqlComm.ExecuteNonQuery();


            sqlConn.Close();
            initStatusBar();


            //权限
            UserLimitAccess();


            /*
            switch (intUserLimit)
            {
                case 20: //管理员
                    购进业务FToolStripMenuItem.Visible = true;//I
                    销售业务ToolStripMenuItem.Visible = true;//O
                    库管业务ToolStripMenuItem.Visible = true;//S
                    其他业务TToolStripMenuItem.Visible = true;//T
                    业务账目HToolStripMenuItem.Visible = true;//H
                    决策分析AToolStripMenuItem.Visible = true;//A
                    查询系统CToolStripMenuItem.Visible = true;//R
                    资料管理DToolStripMenuItem.Visible = true;//D
                    系统维护YToolStripMenuItem.Visible = true;//Y

                    购进合同ToolStripMenuItem.Visible = true;//IA
                    购进ToolStripMenuItem.Visible = true;//IB
                    toolStripButtonA.Visible = true;//IB
                    商品验货入库CToolStripMenuItem.Visible = true;//IC
                    进货退出制单DToolStripMenuItem.Visible = true;//ID
                    进货退补价单据EToolStripMenuItem.Visible = true;//IE
                    应付账款结算FToolStripMenuItem.Visible = true;//IF

                    销售合同制单AToolStripMenuItem.Visible = true;//OA
                    销售出库制单BToolStripMenuItem.Visible = true;//OB
                    toolStripButtonB.Visible = true;//OB
                    销售出库校对CToolStripMenuItem.Visible = true;//OC
                    修改送货信息DToolStripMenuItem.Visible = true;//OD
                    销售退回制单EToolStripMenuItem.Visible = true;//OE
                    销售退补价单据FToolStripMenuItem.Visible = true;//OF
                    应收账款ToolStripMenuItem.Visible = true;//OG

                    库存商品盘点AToolStripMenuItem.Visible = true;//SA
                    toolStripButtonC.Visible = true;//SA
                    借物业务管理BToolStripMenuItem.Visible = true;//SB
                    库存预警维护DToolStripMenuItem.Visible = true;//SD
                    商品库存预警EToolStripMenuItem.Visible = true;//SE
                    库存商品报损GToolStripMenuItem.Visible = true;//SG

                    商品账目处理AToolStripMenuItem.Visible = true;//TA
                    商品价格管理BToolStripMenuItem.Visible = true;//TB
                    错误单据处理CToolStripMenuItem.Visible = true;//TC
                    发票管理DToolStripMenuItem.Visible = true;//TD
                    商品条码管理EToolStripMenuItem.Visible = true;//TE

                    业务数据结转AToolStripMenuItem.Visible = true;//HA
                    商品账页查询BToolStripMenuItem.Visible = true;//HB
                    商品结存报告CToolStripMenuItem.Visible = true;//HC
                    单据状态报告DToolStripMenuItem.Visible = true;//HD
                    业务单位往来EToolStripMenuItem.Visible = true;//HE

                    商品单品分析AToolStripMenuItem.Visible = true;//AA
                    toolStripButtonD.Visible = true;//AA
                    库存商品分析BToolStripMenuItem.Visible = true;//AB
                    应收应付分析CToolStripMenuItem.Visible = true;//AC
                    客户销售分析DToolStripMenuItem.Visible = true;//AD
                    批发销售分析EToolStripMenuItem.Visible = true;//AE
                    经营历程分析FToolStripMenuItem.Visible = true;//AF
                    排行综合分析GToolStripMenuItem.Visible = true;//AG
                    应收款项分析HToolStripMenuItem.Visible = true;//AH
                    应付款项分析IToolStripMenuItem.Visible = true;//AI
                    客户购销分析GToolStripMenuItem.Visible = true;//AG

                    单据再现AToolStripMenuItem.Visible = true;//RA
                    toolStripButtonR.Visible = true;//RA
                    删除资料查询BToolStripMenuItem.Visible = true;//RB
                    购进业务查询CToolStripMenuItem.Visible = true;//RC
                    销售业务查询DToolStripMenuItem.Visible = true;//RD
                    库存业务查询EToolStripMenuItem.Visible = true;//RE
                    商品资料查询FToolStripMenuItem.Visible = true;//RF
                    借物业务查询GToolStripMenuItem.Visible = true;//RG
                    结转单据查询HToolStripMenuItem.Visible = true;//RH
                    基本信息查询IToolStripMenuItem.Visible = true;//RI

                    基本资料维护AToolStripMenuItem.Visible = true;//DA
                    商品资料定义BToolStripMenuItem.Visible = true;//DB

                    数据备份恢复AToolStripMenuItem.Visible = true;//YA
                    单据日志查询BToolStripMenuItem.Visible = true;//YB
                    数据库设置CToolStripMenuItem.Visible = true;//YC
                    更改密码DToolStripMenuItem.Visible = true;//YD
                    退出系统XToolStripMenuItem.Visible = true;//YX
                    系统参数设置SToolStripMenuItem.Visible = true;//YS
                    用户注销LToolStripMenuItem.Visible = true;//YL

                    数据整理ToolStripMenuItem.Visible = true;//EEE1
                    初始化ToolStripMenuItem.Visible = true;//EEE2
                    数据清除ToolStripMenuItem.Visible = true;//EEE3

                    break;
                case 18: //总经理
                    购进业务FToolStripMenuItem.Visible = true;//I
                    销售业务ToolStripMenuItem.Visible = true;//O
                    库管业务ToolStripMenuItem.Visible = true;//S
                    其他业务TToolStripMenuItem.Visible = true;//T
                    业务账目HToolStripMenuItem.Visible = true;//H
                    决策分析AToolStripMenuItem.Visible = true;//A
                    查询系统CToolStripMenuItem.Visible = true;//R
                    资料管理DToolStripMenuItem.Visible = true;//D
                    系统维护YToolStripMenuItem.Visible = true;//Y

                    购进合同ToolStripMenuItem.Visible = true;//IA
                    购进ToolStripMenuItem.Visible = true;//IB
                    toolStripButtonA.Visible = true;//IB
                    商品验货入库CToolStripMenuItem.Visible = true;//IC
                    进货退出制单DToolStripMenuItem.Visible = true;//ID
                    进货退补价单据EToolStripMenuItem.Visible = true;//IE
                    应付账款结算FToolStripMenuItem.Visible = true;//IF

                    销售合同制单AToolStripMenuItem.Visible = true;//OA
                    销售出库制单BToolStripMenuItem.Visible = true;//OB
                    toolStripButtonB.Visible = true;//OB
                    销售出库校对CToolStripMenuItem.Visible = true;//OC
                    修改送货信息DToolStripMenuItem.Visible = true;//OD
                    销售退回制单EToolStripMenuItem.Visible = true;//OE
                    销售退补价单据FToolStripMenuItem.Visible = true;//OF
                    应收账款ToolStripMenuItem.Visible = true;//OG

                    库存商品盘点AToolStripMenuItem.Visible = true;//SA
                    toolStripButtonC.Visible = true;//SA
                    借物业务管理BToolStripMenuItem.Visible = true;//SB
                    库存预警维护DToolStripMenuItem.Visible = true;//SD
                    商品库存预警EToolStripMenuItem.Visible = true;//SE
                    库存商品报损GToolStripMenuItem.Visible = true;//SG

                    商品账目处理AToolStripMenuItem.Visible = true;//TA
                    商品价格管理BToolStripMenuItem.Visible = true;//TB
                    错误单据处理CToolStripMenuItem.Visible = true;//TC
                    发票管理DToolStripMenuItem.Visible = true;//TD
                    商品条码管理EToolStripMenuItem.Visible = true;//TE

                    业务数据结转AToolStripMenuItem.Visible = true;//HA
                    商品账页查询BToolStripMenuItem.Visible = true;//HB
                    商品结存报告CToolStripMenuItem.Visible = true;//HC
                    单据状态报告DToolStripMenuItem.Visible = true;//HD
                    业务单位往来EToolStripMenuItem.Visible = true;//HE

                    商品单品分析AToolStripMenuItem.Visible = true;//AA
                    toolStripButtonD.Visible = true;//AA
                    库存商品分析BToolStripMenuItem.Visible = true;//AB
                    应收应付分析CToolStripMenuItem.Visible = true;//AC
                    客户销售分析DToolStripMenuItem.Visible = true;//AD
                    批发销售分析EToolStripMenuItem.Visible = true;//AE
                    经营历程分析FToolStripMenuItem.Visible = true;//AF
                    排行综合分析GToolStripMenuItem.Visible = true;//AG
                    应收款项分析HToolStripMenuItem.Visible = true;//AH
                    应付款项分析IToolStripMenuItem.Visible = true;//AI
                    客户购销分析GToolStripMenuItem.Visible = true;//AG

                    单据再现AToolStripMenuItem.Visible = true;//RA
                    toolStripButtonR.Visible = true;//RA
                    删除资料查询BToolStripMenuItem.Visible = true;//RB
                    购进业务查询CToolStripMenuItem.Visible = true;//RC
                    销售业务查询DToolStripMenuItem.Visible = true;//RD
                    库存业务查询EToolStripMenuItem.Visible = true;//RE
                    商品资料查询FToolStripMenuItem.Visible = true;//RF
                    借物业务查询GToolStripMenuItem.Visible = true;//RG
                    结转单据查询HToolStripMenuItem.Visible = true;//RH
                    基本信息查询IToolStripMenuItem.Visible = true;//RI

                    基本资料维护AToolStripMenuItem.Visible = true;//DA
                    商品资料定义BToolStripMenuItem.Visible = true;//DB

                    数据备份恢复AToolStripMenuItem.Visible = true;//YA
                    单据日志查询BToolStripMenuItem.Visible = true;//YB
                    数据库设置CToolStripMenuItem.Visible = true;//YC
                    更改密码DToolStripMenuItem.Visible = true;//YD
                    退出系统XToolStripMenuItem.Visible = true;//YX
                    系统参数设置SToolStripMenuItem.Visible = true;//YS
                    用户注销LToolStripMenuItem.Visible = true;//YL

                    数据整理ToolStripMenuItem.Visible = true;//EEE1
                    初始化ToolStripMenuItem.Visible = true;//EEE2
                    数据清除ToolStripMenuItem.Visible = true;//EEE3

                    break;
                case 16: // 经理
                    购进业务FToolStripMenuItem.Visible = true;//I
                    销售业务ToolStripMenuItem.Visible = true;//O
                    库管业务ToolStripMenuItem.Visible = true;//S
                    其他业务TToolStripMenuItem.Visible = true;//T
                    业务账目HToolStripMenuItem.Visible = true;//H
                    决策分析AToolStripMenuItem.Visible = true;//A
                    查询系统CToolStripMenuItem.Visible = true;//R
                    资料管理DToolStripMenuItem.Visible = true;//D
                    系统维护YToolStripMenuItem.Visible = true;//Y

                    购进合同ToolStripMenuItem.Visible = true;//IA
                    购进ToolStripMenuItem.Visible = true;//IB
                    toolStripButtonA.Visible = true;//IB
                    商品验货入库CToolStripMenuItem.Visible = true;//IC
                    进货退出制单DToolStripMenuItem.Visible = true;//ID
                    进货退补价单据EToolStripMenuItem.Visible = true;//IE
                    应付账款结算FToolStripMenuItem.Visible = false;//IF

                    销售合同制单AToolStripMenuItem.Visible = true;//OA
                    销售出库制单BToolStripMenuItem.Visible = true;//OB
                    toolStripButtonB.Visible = true;//OB
                    销售出库校对CToolStripMenuItem.Visible = true;//OC
                    修改送货信息DToolStripMenuItem.Visible = true;//OD
                    销售退回制单EToolStripMenuItem.Visible = true;//OE
                    销售退补价单据FToolStripMenuItem.Visible = true;//OF
                    应收账款ToolStripMenuItem.Visible = false;//OG

                    库存商品盘点AToolStripMenuItem.Visible = false;//SA
                    toolStripButtonC.Visible = false;//SA
                    借物业务管理BToolStripMenuItem.Visible = true;//SB
                    库存预警维护DToolStripMenuItem.Visible = true;//SD
                    商品库存预警EToolStripMenuItem.Visible = true;//SE
                    库存商品报损GToolStripMenuItem.Visible = true;//SG

                    商品账目处理AToolStripMenuItem.Visible = false;//TA
                    商品价格管理BToolStripMenuItem.Visible = true;//TB
                    错误单据处理CToolStripMenuItem.Visible = true;//TC
                    发票管理DToolStripMenuItem.Visible = true;//TD
                    商品条码管理EToolStripMenuItem.Visible = true;//TE

                    业务数据结转AToolStripMenuItem.Visible = false;//HA
                    商品账页查询BToolStripMenuItem.Visible = true;//HB
                    商品结存报告CToolStripMenuItem.Visible = true;//HC
                    单据状态报告DToolStripMenuItem.Visible = true;//HD
                    业务单位往来EToolStripMenuItem.Visible = true;//HE

                    商品单品分析AToolStripMenuItem.Visible = true;//AA
                    toolStripButtonD.Visible = true;//AA
                    库存商品分析BToolStripMenuItem.Visible = true;//AB
                    应收应付分析CToolStripMenuItem.Visible = true;//AC
                    客户销售分析DToolStripMenuItem.Visible = true;//AD
                    批发销售分析EToolStripMenuItem.Visible = true;//AE
                    经营历程分析FToolStripMenuItem.Visible = true;//AF
                    排行综合分析GToolStripMenuItem.Visible = true;//AG
                    应收款项分析HToolStripMenuItem.Visible = true;//AH
                    应付款项分析IToolStripMenuItem.Visible = true;//AI
                    客户购销分析GToolStripMenuItem.Visible = true;//AG

                    单据再现AToolStripMenuItem.Visible = true;//RA
                    toolStripButtonR.Visible = true;//RA
                    删除资料查询BToolStripMenuItem.Visible = true;//RB
                    购进业务查询CToolStripMenuItem.Visible = true;//RC
                    销售业务查询DToolStripMenuItem.Visible = true;//RD
                    库存业务查询EToolStripMenuItem.Visible = true;//RE
                    商品资料查询FToolStripMenuItem.Visible = true;//RF
                    借物业务查询GToolStripMenuItem.Visible = true;//RG
                    结转单据查询HToolStripMenuItem.Visible = true;//RH
                    基本信息查询IToolStripMenuItem.Visible = true;//RI

                    基本资料维护AToolStripMenuItem.Visible = true;//DA
                    商品资料定义BToolStripMenuItem.Visible = true;//DB

                    数据备份恢复AToolStripMenuItem.Visible = true;//YA
                    单据日志查询BToolStripMenuItem.Visible = true;//YB
                    数据库设置CToolStripMenuItem.Visible = true;//YC
                    更改密码DToolStripMenuItem.Visible = true;//YD
                    退出系统XToolStripMenuItem.Visible = true;//YX
                    系统参数设置SToolStripMenuItem.Visible = true;//YS
                    用户注销LToolStripMenuItem.Visible = true;//YL

                    数据整理ToolStripMenuItem.Visible = false;//EEE1
                    初始化ToolStripMenuItem.Visible = false;//EEE2
                    数据清除ToolStripMenuItem.Visible = false;//EEE3

                    break;
                case 10: //财务1
                    购进业务FToolStripMenuItem.Visible = true;//I
                    销售业务ToolStripMenuItem.Visible = true;//O
                    库管业务ToolStripMenuItem.Visible = false;//S
                    其他业务TToolStripMenuItem.Visible = true;//T
                    业务账目HToolStripMenuItem.Visible = true;//H
                    决策分析AToolStripMenuItem.Visible = true;//A
                    查询系统CToolStripMenuItem.Visible = true;//R
                    资料管理DToolStripMenuItem.Visible = true;//D
                    系统维护YToolStripMenuItem.Visible = true;//Y

                    购进合同ToolStripMenuItem.Visible = true;//IA
                    购进ToolStripMenuItem.Visible = true;//IB
                    toolStripButtonA.Visible = true;//IB
                    商品验货入库CToolStripMenuItem.Visible = true;//IC
                    进货退出制单DToolStripMenuItem.Visible = true;//ID
                    进货退补价单据EToolStripMenuItem.Visible = true;//IE
                    应付账款结算FToolStripMenuItem.Visible = true;//IF

                    销售合同制单AToolStripMenuItem.Visible = true;//OA
                    销售出库制单BToolStripMenuItem.Visible = true;//OB
                    toolStripButtonB.Visible = true;//OB
                    销售出库校对CToolStripMenuItem.Visible = true;//OC
                    修改送货信息DToolStripMenuItem.Visible = true;//OD
                    销售退回制单EToolStripMenuItem.Visible = true;//OE
                    销售退补价单据FToolStripMenuItem.Visible = true;//OF
                    应收账款ToolStripMenuItem.Visible = true;//OG

                    库存商品盘点AToolStripMenuItem.Visible = false;//SA
                    toolStripButtonC.Visible = false;//SA
                    借物业务管理BToolStripMenuItem.Visible = false;//SB
                    库存预警维护DToolStripMenuItem.Visible = false;//SD
                    商品库存预警EToolStripMenuItem.Visible = false;//SE
                    库存商品报损GToolStripMenuItem.Visible = false;//SG

                    商品账目处理AToolStripMenuItem.Visible = true;//TA
                    商品价格管理BToolStripMenuItem.Visible = true;//TB
                    错误单据处理CToolStripMenuItem.Visible = true;//TC
                    发票管理DToolStripMenuItem.Visible = true;//TD
                    商品条码管理EToolStripMenuItem.Visible = true;//TE

                    业务数据结转AToolStripMenuItem.Visible = true;//HA
                    商品账页查询BToolStripMenuItem.Visible = true;//HB
                    商品结存报告CToolStripMenuItem.Visible = true;//HC
                    单据状态报告DToolStripMenuItem.Visible = true;//HD
                    业务单位往来EToolStripMenuItem.Visible = true;//HE

                    商品单品分析AToolStripMenuItem.Visible = true;//AA
                    toolStripButtonD.Visible = true;//AA
                    库存商品分析BToolStripMenuItem.Visible = false;//AB
                    应收应付分析CToolStripMenuItem.Visible = false;//AC
                    客户销售分析DToolStripMenuItem.Visible = false;//AD
                    批发销售分析EToolStripMenuItem.Visible = false;//AE
                    经营历程分析FToolStripMenuItem.Visible = false;//AF
                    排行综合分析GToolStripMenuItem.Visible = false;//AG
                    应收款项分析HToolStripMenuItem.Visible = false;//AH
                    应付款项分析IToolStripMenuItem.Visible = false;//AI
                    客户购销分析GToolStripMenuItem.Visible = false;//AG

                    单据再现AToolStripMenuItem.Visible = true;//RA
                    toolStripButtonR.Visible = true;//RA
                    删除资料查询BToolStripMenuItem.Visible = true;//RB
                    购进业务查询CToolStripMenuItem.Visible = true;//RC
                    销售业务查询DToolStripMenuItem.Visible = true;//RD
                    库存业务查询EToolStripMenuItem.Visible = true;//RE
                    商品资料查询FToolStripMenuItem.Visible = true;//RF
                    借物业务查询GToolStripMenuItem.Visible = true;//RG
                    结转单据查询HToolStripMenuItem.Visible = true;//RH
                    基本信息查询IToolStripMenuItem.Visible = true;//RI

                    基本资料维护AToolStripMenuItem.Visible = true;//DA
                    商品资料定义BToolStripMenuItem.Visible = false;//DB

                    数据备份恢复AToolStripMenuItem.Visible = false;//YA
                    单据日志查询BToolStripMenuItem.Visible = false;//YB
                    数据库设置CToolStripMenuItem.Visible = true;//YC
                    更改密码DToolStripMenuItem.Visible = true;//YD
                    退出系统XToolStripMenuItem.Visible = true;//YX
                    系统参数设置SToolStripMenuItem.Visible = true;//YS
                    用户注销LToolStripMenuItem.Visible = true;//YL

                    数据整理ToolStripMenuItem.Visible = false;//EEE1
                    初始化ToolStripMenuItem.Visible = false;//EEE2
                    数据清除ToolStripMenuItem.Visible = false;//EEE3

                    break;
                case 6: //库管
                    购进业务FToolStripMenuItem.Visible = false;//I
                    销售业务ToolStripMenuItem.Visible = false;//O
                    库管业务ToolStripMenuItem.Visible = true;//S
                    其他业务TToolStripMenuItem.Visible = true;//T
                    业务账目HToolStripMenuItem.Visible = false;//H
                    决策分析AToolStripMenuItem.Visible = false;//A
                    查询系统CToolStripMenuItem.Visible = true;//R
                    资料管理DToolStripMenuItem.Visible = false;//D
                    系统维护YToolStripMenuItem.Visible = true;//Y

                    购进合同ToolStripMenuItem.Visible = false;//IA
                    购进ToolStripMenuItem.Visible = false;//IB
                    toolStripButtonA.Visible = false;//IB
                    商品验货入库CToolStripMenuItem.Visible = false;//IC
                    进货退出制单DToolStripMenuItem.Visible = false;//ID
                    进货退补价单据EToolStripMenuItem.Visible = false;//IE
                    应付账款结算FToolStripMenuItem.Visible = false;//IF

                    销售合同制单AToolStripMenuItem.Visible = false;//OA
                    销售出库制单BToolStripMenuItem.Visible = false;//OB
                    toolStripButtonB.Visible = false;//OB
                    销售出库校对CToolStripMenuItem.Visible = false;//OC
                    修改送货信息DToolStripMenuItem.Visible = false;//OD
                    销售退回制单EToolStripMenuItem.Visible = false;//OE
                    销售退补价单据FToolStripMenuItem.Visible = false;//OF
                    应收账款ToolStripMenuItem.Visible = false;//OG

                    库存商品盘点AToolStripMenuItem.Visible = true;//SA
                    toolStripButtonC.Visible = true;//SA
                    借物业务管理BToolStripMenuItem.Visible = false;//SB
                    库存预警维护DToolStripMenuItem.Visible = true;//SD
                    商品库存预警EToolStripMenuItem.Visible = true;//SE
                    库存商品报损GToolStripMenuItem.Visible = false;//SG

                    商品账目处理AToolStripMenuItem.Visible = false;//TA
                    商品价格管理BToolStripMenuItem.Visible = false;//TB
                    错误单据处理CToolStripMenuItem.Visible = false;//TC
                    发票管理DToolStripMenuItem.Visible = false;//TD
                    商品条码管理EToolStripMenuItem.Visible = true;//TE

                    业务数据结转AToolStripMenuItem.Visible = false;//HA
                    商品账页查询BToolStripMenuItem.Visible = false;//HB
                    商品结存报告CToolStripMenuItem.Visible = false;//HC
                    单据状态报告DToolStripMenuItem.Visible = false;//HD
                    业务单位往来EToolStripMenuItem.Visible = false;//HE

                    商品单品分析AToolStripMenuItem.Visible = false;//AA
                    toolStripButtonD.Visible = false;//AA
                    库存商品分析BToolStripMenuItem.Visible = false;//AB
                    应收应付分析CToolStripMenuItem.Visible = false;//AC
                    客户销售分析DToolStripMenuItem.Visible = false;//AD
                    批发销售分析EToolStripMenuItem.Visible = false;//AE
                    经营历程分析FToolStripMenuItem.Visible = false;//AF
                    排行综合分析GToolStripMenuItem.Visible = false;//AG
                    应收款项分析HToolStripMenuItem.Visible = false;//AH
                    应付款项分析IToolStripMenuItem.Visible = false;//AI
                    客户购销分析GToolStripMenuItem.Visible = false;//AG

                    单据再现AToolStripMenuItem.Visible = true;//RA
                    toolStripButtonR.Visible = false;//RA
                    删除资料查询BToolStripMenuItem.Visible = false;//RB
                    购进业务查询CToolStripMenuItem.Visible = false;//RC
                    销售业务查询DToolStripMenuItem.Visible = true;//RD
                    库存业务查询EToolStripMenuItem.Visible = true;//RE
                    商品资料查询FToolStripMenuItem.Visible = false;//RF
                    借物业务查询GToolStripMenuItem.Visible = false;//RG
                    结转单据查询HToolStripMenuItem.Visible = false;//RH
                    基本信息查询IToolStripMenuItem.Visible = true;//RI

                    基本资料维护AToolStripMenuItem.Visible = true;//DA
                    商品资料定义BToolStripMenuItem.Visible = false;//DB

                    数据备份恢复AToolStripMenuItem.Visible = false;//YA
                    单据日志查询BToolStripMenuItem.Visible = false;//YB
                    数据库设置CToolStripMenuItem.Visible = true;//YC
                    更改密码DToolStripMenuItem.Visible = true;//YD
                    退出系统XToolStripMenuItem.Visible = true;//YX
                    系统参数设置SToolStripMenuItem.Visible = true;//YS
                    用户注销LToolStripMenuItem.Visible = true;//YL

                    数据整理ToolStripMenuItem.Visible = false;//EEE1
                    初始化ToolStripMenuItem.Visible = false;//EEE2
                    数据清除ToolStripMenuItem.Visible = false;//EEE3

                    库存查询CToolStripMenuItem.Visible = true;
                    商品综合查询DToolStripMenuItem.Visible = false;
                    当期商品出入库CToolStripMenuItem.Visible = false;
                    库存所有商品查询BToolStripMenuItem.Visible = true;
                    商品盘点历史查询EToolStripMenuItem.Visible = false;
                    商品报损历史查询FToolStripMenuItem.Visible = false;
                    商品拆装历史查询HToolStripMenuItem.Visible = false;

                    break;
                case 5: //职员1
                    购进业务FToolStripMenuItem.Visible = true;//I
                    销售业务ToolStripMenuItem.Visible = true;//O
                    库管业务ToolStripMenuItem.Visible = true;//S
                    其他业务TToolStripMenuItem.Visible = true;//T
                    业务账目HToolStripMenuItem.Visible = false;//H
                    决策分析AToolStripMenuItem.Visible = false;//A
                    查询系统CToolStripMenuItem.Visible = true;//R
                    资料管理DToolStripMenuItem.Visible = false;//D
                    系统维护YToolStripMenuItem.Visible = true;//Y

                    购进合同ToolStripMenuItem.Visible = true;//IA
                    购进ToolStripMenuItem.Visible = false;//IB
                    toolStripButtonA.Visible = false;//IB
                    商品验货入库CToolStripMenuItem.Visible = false;//IC
                    进货退出制单DToolStripMenuItem.Visible = false;//ID
                    进货退补价单据EToolStripMenuItem.Visible = false;//IE
                    应付账款结算FToolStripMenuItem.Visible = false;//IF

                    销售合同制单AToolStripMenuItem.Visible = true;//OA
                    销售出库制单BToolStripMenuItem.Visible = false;//OB
                    toolStripButtonB.Visible = false;//OB
                    销售出库校对CToolStripMenuItem.Visible = false;//OC
                    修改送货信息DToolStripMenuItem.Visible = false;//OD
                    销售退回制单EToolStripMenuItem.Visible = false;//OE
                    销售退补价单据FToolStripMenuItem.Visible = false;//OF
                    应收账款ToolStripMenuItem.Visible = false;//OG

                    库存商品盘点AToolStripMenuItem.Visible = false;//SA
                    toolStripButtonC.Visible = false;//SA
                    借物业务管理BToolStripMenuItem.Visible = false;//SB
                    库存预警维护DToolStripMenuItem.Visible = false;//SD
                    商品库存预警EToolStripMenuItem.Visible = true;//SE
                    库存商品报损GToolStripMenuItem.Visible = false;//SG

                    商品账目处理AToolStripMenuItem.Visible = false;//TA
                    商品价格管理BToolStripMenuItem.Visible = false;//TB
                    错误单据处理CToolStripMenuItem.Visible = false;//TC
                    发票管理DToolStripMenuItem.Visible = true;//TD
                    开具发票AToolStripMenuItem.Visible = false;//TD
                    发票查询BToolStripMenuItem.Visible = true;//TD
                    发票作废CToolStripMenuItem.Visible = false;//TD
                    商品条码管理EToolStripMenuItem.Visible = false;//TE

                    业务数据结转AToolStripMenuItem.Visible = false;//HA
                    商品账页查询BToolStripMenuItem.Visible = false;//HB
                    商品结存报告CToolStripMenuItem.Visible = false;//HC
                    单据状态报告DToolStripMenuItem.Visible = false;//HD
                    业务单位往来EToolStripMenuItem.Visible = false;//HE

                    商品单品分析AToolStripMenuItem.Visible = false;//AA
                    toolStripButtonD.Visible = false;//AA
                    库存商品分析BToolStripMenuItem.Visible = false;//AB
                    应收应付分析CToolStripMenuItem.Visible = false;//AC
                    客户销售分析DToolStripMenuItem.Visible = false;//AD
                    批发销售分析EToolStripMenuItem.Visible = false;//AE
                    经营历程分析FToolStripMenuItem.Visible = false;//AF
                    排行综合分析GToolStripMenuItem.Visible = false;//AG
                    应收款项分析HToolStripMenuItem.Visible = false;//AH
                    应付款项分析IToolStripMenuItem.Visible = false;//AI
                    客户购销分析GToolStripMenuItem.Visible = false;//AG

                    单据再现AToolStripMenuItem.Visible = true;//RA
                    toolStripButtonR.Visible = false;//RA
                    删除资料查询BToolStripMenuItem.Visible = false;//RB
                    购进业务查询CToolStripMenuItem.Visible = false;//RC
                    销售业务查询DToolStripMenuItem.Visible = true;//RD
                    库存业务查询EToolStripMenuItem.Visible = true;//RE
                    商品资料查询FToolStripMenuItem.Visible = false;//RF
                    借物业务查询GToolStripMenuItem.Visible = false;//RG
                    结转单据查询HToolStripMenuItem.Visible = false;//RH
                    基本信息查询IToolStripMenuItem.Visible = true;//RI

                    基本资料维护AToolStripMenuItem.Visible = false;//DA
                    商品资料定义BToolStripMenuItem.Visible = false;//DB

                    数据备份恢复AToolStripMenuItem.Visible = false;//YA
                    单据日志查询BToolStripMenuItem.Visible = false;//YB
                    数据库设置CToolStripMenuItem.Visible = true;//YC
                    更改密码DToolStripMenuItem.Visible = true;//YD
                    退出系统XToolStripMenuItem.Visible = true;//YX
                    系统参数设置SToolStripMenuItem.Visible = true;//YS
                    用户注销LToolStripMenuItem.Visible = true;//YL

                    数据整理ToolStripMenuItem.Visible = false;//EEE1
                    初始化ToolStripMenuItem.Visible = false;//EEE2
                    数据清除ToolStripMenuItem.Visible = false;//EEE3

                    库存查询CToolStripMenuItem.Visible = true;
                    商品综合查询DToolStripMenuItem.Visible = false;
                    当期商品出入库CToolStripMenuItem.Visible = false;
                    库存所有商品查询BToolStripMenuItem.Visible = true;
                    商品盘点历史查询EToolStripMenuItem.Visible = false;
                    商品报损历史查询FToolStripMenuItem.Visible = false;
                    商品拆装历史查询HToolStripMenuItem.Visible = false;
                    break;
                case 9: //财务2
                    购进业务FToolStripMenuItem.Visible = true;//I
                    销售业务ToolStripMenuItem.Visible = true;//O
                    库管业务ToolStripMenuItem.Visible = false;//S
                    其他业务TToolStripMenuItem.Visible = true;//T
                    业务账目HToolStripMenuItem.Visible = false;//H
                    决策分析AToolStripMenuItem.Visible = false;//A
                    查询系统CToolStripMenuItem.Visible = true;//R
                    资料管理DToolStripMenuItem.Visible = true;//D
                    系统维护YToolStripMenuItem.Visible = true;//Y

                    购进合同ToolStripMenuItem.Visible = true;//IA
                    购进ToolStripMenuItem.Visible = true;//IB
                    toolStripButtonA.Visible = true;//IB
                    商品验货入库CToolStripMenuItem.Visible = true;//IC
                    进货退出制单DToolStripMenuItem.Visible = true;//ID
                    进货退补价单据EToolStripMenuItem.Visible = true;//IE
                    应付账款结算FToolStripMenuItem.Visible = true;//IF

                    销售合同制单AToolStripMenuItem.Visible = true;//OA
                    销售出库制单BToolStripMenuItem.Visible = true;//OB
                    toolStripButtonB.Visible = true;//OB
                    销售出库校对CToolStripMenuItem.Visible = true;//OC
                    修改送货信息DToolStripMenuItem.Visible = true;//OD
                    销售退回制单EToolStripMenuItem.Visible = true;//OE
                    销售退补价单据FToolStripMenuItem.Visible = true;//OF
                    应收账款ToolStripMenuItem.Visible = true;//OG

                    库存商品盘点AToolStripMenuItem.Visible = false;//SA
                    toolStripButtonC.Visible = false;//SA
                    借物业务管理BToolStripMenuItem.Visible = false;//SB
                    库存预警维护DToolStripMenuItem.Visible = false;//SD
                    商品库存预警EToolStripMenuItem.Visible = false;//SE
                    库存商品报损GToolStripMenuItem.Visible = false;//SG

                    商品账目处理AToolStripMenuItem.Visible = false;//TA
                    商品价格管理BToolStripMenuItem.Visible = false;//TB
                    错误单据处理CToolStripMenuItem.Visible = true;//TC
                    发票管理DToolStripMenuItem.Visible = true;//TD
                    商品条码管理EToolStripMenuItem.Visible = false;//TE

                    业务数据结转AToolStripMenuItem.Visible = false;//HA
                    商品账页查询BToolStripMenuItem.Visible = false;//HB
                    商品结存报告CToolStripMenuItem.Visible = false;//HC
                    单据状态报告DToolStripMenuItem.Visible = false;//HD
                    业务单位往来EToolStripMenuItem.Visible = false;//HE

                    商品单品分析AToolStripMenuItem.Visible = false;//AA
                    toolStripButtonD.Visible = false;//AA
                    库存商品分析BToolStripMenuItem.Visible = false;//AB
                    应收应付分析CToolStripMenuItem.Visible = false;//AC
                    客户销售分析DToolStripMenuItem.Visible = false;//AD
                    批发销售分析EToolStripMenuItem.Visible = false;//AE
                    经营历程分析FToolStripMenuItem.Visible = false;//AF
                    排行综合分析GToolStripMenuItem.Visible = false;//AG
                    应收款项分析HToolStripMenuItem.Visible = false;//AH
                    应付款项分析IToolStripMenuItem.Visible = false;//AI
                    客户购销分析GToolStripMenuItem.Visible = false;//AG

                    单据再现AToolStripMenuItem.Visible = true;//RA
                    toolStripButtonR.Visible = true;//RA
                    删除资料查询BToolStripMenuItem.Visible = true;//RB
                    购进业务查询CToolStripMenuItem.Visible = true;//RC
                    销售业务查询DToolStripMenuItem.Visible = true;//RD
                    库存业务查询EToolStripMenuItem.Visible = true;//RE
                    商品资料查询FToolStripMenuItem.Visible = true;//RF
                    借物业务查询GToolStripMenuItem.Visible = true;//RG
                    结转单据查询HToolStripMenuItem.Visible = true;//RH
                    基本信息查询IToolStripMenuItem.Visible = true;//RI

                    基本资料维护AToolStripMenuItem.Visible = true;//DA
                    商品资料定义BToolStripMenuItem.Visible = true;//DB

                    数据备份恢复AToolStripMenuItem.Visible = false;//YA
                    单据日志查询BToolStripMenuItem.Visible = false;//YB
                    数据库设置CToolStripMenuItem.Visible = true;//YC
                    更改密码DToolStripMenuItem.Visible = true;//YD
                    退出系统XToolStripMenuItem.Visible = true;//YX
                    系统参数设置SToolStripMenuItem.Visible = true;//YS
                    用户注销LToolStripMenuItem.Visible = true;//YL

                    数据整理ToolStripMenuItem.Visible = false;//EEE1
                    初始化ToolStripMenuItem.Visible = false;//EEE2
                    数据清除ToolStripMenuItem.Visible = false;//EEE3

                    break;
                case 8: //财务3
                    购进业务FToolStripMenuItem.Visible = true;//I
                    销售业务ToolStripMenuItem.Visible = true;//O
                    库管业务ToolStripMenuItem.Visible = true;//S
                    其他业务TToolStripMenuItem.Visible = true;//T
                    业务账目HToolStripMenuItem.Visible = false;//H
                    决策分析AToolStripMenuItem.Visible = false;//A
                    查询系统CToolStripMenuItem.Visible = true;//R
                    资料管理DToolStripMenuItem.Visible = true;//D
                    系统维护YToolStripMenuItem.Visible = true;//Y

                    购进合同ToolStripMenuItem.Visible = true;//IA
                    购进ToolStripMenuItem.Visible = true;//IB
                    toolStripButtonA.Visible = true;//IB
                    商品验货入库CToolStripMenuItem.Visible = true;//IC
                    进货退出制单DToolStripMenuItem.Visible = true;//ID
                    进货退补价单据EToolStripMenuItem.Visible = true;//IE
                    应付账款结算FToolStripMenuItem.Visible = true;//IF

                    销售合同制单AToolStripMenuItem.Visible = true;//OA
                    销售出库制单BToolStripMenuItem.Visible = true;//OB
                    toolStripButtonB.Visible = true;//OB
                    销售出库校对CToolStripMenuItem.Visible = true;//OC
                    修改送货信息DToolStripMenuItem.Visible = true;//OD
                    销售退回制单EToolStripMenuItem.Visible = true;//OE
                    销售退补价单据FToolStripMenuItem.Visible = true;//OF
                    应收账款ToolStripMenuItem.Visible = true;//OG

                    库存商品盘点AToolStripMenuItem.Visible = false;//SA
                    toolStripButtonC.Visible = false;//SA
                    借物业务管理BToolStripMenuItem.Visible = true;//SB
                    库存预警维护DToolStripMenuItem.Visible = false;//SD
                    商品库存预警EToolStripMenuItem.Visible = false;//SE
                    库存商品报损GToolStripMenuItem.Visible = false;//SG

                    商品账目处理AToolStripMenuItem.Visible = false;//TA
                    商品价格管理BToolStripMenuItem.Visible = false;//TB
                    错误单据处理CToolStripMenuItem.Visible = true;//TC
                    发票管理DToolStripMenuItem.Visible = true;//TD
                    商品条码管理EToolStripMenuItem.Visible = true;//TE

                    业务数据结转AToolStripMenuItem.Visible = false;//HA
                    商品账页查询BToolStripMenuItem.Visible = false;//HB
                    商品结存报告CToolStripMenuItem.Visible = false;//HC
                    单据状态报告DToolStripMenuItem.Visible = false;//HD
                    业务单位往来EToolStripMenuItem.Visible = false;//HE

                    商品单品分析AToolStripMenuItem.Visible = false;//AA
                    toolStripButtonD.Visible = false;//AA
                    库存商品分析BToolStripMenuItem.Visible = false;//AB
                    应收应付分析CToolStripMenuItem.Visible = false;//AC
                    客户销售分析DToolStripMenuItem.Visible = false;//AD
                    批发销售分析EToolStripMenuItem.Visible = false;//AE
                    经营历程分析FToolStripMenuItem.Visible = false;//AF
                    排行综合分析GToolStripMenuItem.Visible = false;//AG
                    应收款项分析HToolStripMenuItem.Visible = false;//AH
                    应付款项分析IToolStripMenuItem.Visible = false;//AI
                    客户购销分析GToolStripMenuItem.Visible = false;//AG

                    单据再现AToolStripMenuItem.Visible = true;//RA
                    toolStripButtonR.Visible = true;//RA
                    删除资料查询BToolStripMenuItem.Visible = false;//RB
                    购进业务查询CToolStripMenuItem.Visible = false;//RC
                    销售业务查询DToolStripMenuItem.Visible = true;//RD
                    库存业务查询EToolStripMenuItem.Visible = true;//RE
                    商品资料查询FToolStripMenuItem.Visible = false;//RF
                    借物业务查询GToolStripMenuItem.Visible = false;//RG
                    结转单据查询HToolStripMenuItem.Visible = false;//RH
                    基本信息查询IToolStripMenuItem.Visible = true;//RI

                    基本资料维护AToolStripMenuItem.Visible = true;//DA
                    商品资料定义BToolStripMenuItem.Visible = true;//DB

                    数据备份恢复AToolStripMenuItem.Visible = false;//YA
                    单据日志查询BToolStripMenuItem.Visible = false;//YB
                    数据库设置CToolStripMenuItem.Visible = true;//YC
                    更改密码DToolStripMenuItem.Visible = true;//YD
                    退出系统XToolStripMenuItem.Visible = true;//YX
                    系统参数设置SToolStripMenuItem.Visible = true;//YS
                    用户注销LToolStripMenuItem.Visible = true;//YL

                    数据整理ToolStripMenuItem.Visible = false;//EEE1
                    初始化ToolStripMenuItem.Visible = false;//EEE2
                    数据清除ToolStripMenuItem.Visible = false;//EEE3

                    库存查询CToolStripMenuItem.Visible = true;
                    商品综合查询DToolStripMenuItem.Visible = false;
                    当期商品出入库CToolStripMenuItem.Visible = false;
                    库存所有商品查询BToolStripMenuItem.Visible = true;
                    商品盘点历史查询EToolStripMenuItem.Visible = false;
                    商品报损历史查询FToolStripMenuItem.Visible = false;
                    商品拆装历史查询HToolStripMenuItem.Visible = false;

                    break;
                case 4: //职员2
                    购进业务FToolStripMenuItem.Visible = true;//I
                    销售业务ToolStripMenuItem.Visible = true;//O
                    库管业务ToolStripMenuItem.Visible = true;//S
                    其他业务TToolStripMenuItem.Visible = true;//T
                    业务账目HToolStripMenuItem.Visible = false;//H
                    决策分析AToolStripMenuItem.Visible = true;//A
                    查询系统CToolStripMenuItem.Visible = true;//R
                    资料管理DToolStripMenuItem.Visible = true;//D
                    系统维护YToolStripMenuItem.Visible = true;//Y

                    购进合同ToolStripMenuItem.Visible = true;//IA
                    购进ToolStripMenuItem.Visible = false;//IB
                    toolStripButtonA.Visible = false;//IB
                    商品验货入库CToolStripMenuItem.Visible = false;//IC
                    进货退出制单DToolStripMenuItem.Visible = false;//ID
                    进货退补价单据EToolStripMenuItem.Visible = false;//IE
                    应付账款结算FToolStripMenuItem.Visible = false;//IF

                    销售合同制单AToolStripMenuItem.Visible = true;//OA
                    销售出库制单BToolStripMenuItem.Visible = false;//OB
                    toolStripButtonB.Visible = false;//OB
                    销售出库校对CToolStripMenuItem.Visible = false;//OC
                    修改送货信息DToolStripMenuItem.Visible = false;//OD
                    销售退回制单EToolStripMenuItem.Visible = false;//OE
                    销售退补价单据FToolStripMenuItem.Visible = false;//OF
                    应收账款ToolStripMenuItem.Visible = false;//OG

                    库存商品盘点AToolStripMenuItem.Visible = false;//SA
                    toolStripButtonC.Visible = false;//SA
                    借物业务管理BToolStripMenuItem.Visible = true;//SB
                    库存预警维护DToolStripMenuItem.Visible = false;//SD
                    商品库存预警EToolStripMenuItem.Visible = true;//SE
                    库存商品报损GToolStripMenuItem.Visible = false;//SG

                    商品账目处理AToolStripMenuItem.Visible = false;//TA
                    商品价格管理BToolStripMenuItem.Visible = false;//TB
                    错误单据处理CToolStripMenuItem.Visible = true;//TC
                    发票管理DToolStripMenuItem.Visible = true;//TD
                    开具发票AToolStripMenuItem.Visible = false;//TD
                    发票查询BToolStripMenuItem.Visible = true;//TD
                    发票作废CToolStripMenuItem.Visible = false;//TD
                    商品条码管理EToolStripMenuItem.Visible = false;//TE

                    业务数据结转AToolStripMenuItem.Visible = false;//HA
                    商品账页查询BToolStripMenuItem.Visible = false;//HB
                    商品结存报告CToolStripMenuItem.Visible = false;//HC
                    单据状态报告DToolStripMenuItem.Visible = false;//HD
                    业务单位往来EToolStripMenuItem.Visible = false;//HE

                    商品单品分析AToolStripMenuItem.Visible = true;//AA
                    toolStripButtonD.Visible = true;//AA
                    库存商品分析BToolStripMenuItem.Visible = false;//AB
                    应收应付分析CToolStripMenuItem.Visible = false;//AC
                    客户销售分析DToolStripMenuItem.Visible = false;//AD
                    批发销售分析EToolStripMenuItem.Visible = false;//AE
                    经营历程分析FToolStripMenuItem.Visible = false;//AF
                    排行综合分析GToolStripMenuItem.Visible = false;//AG
                    应收款项分析HToolStripMenuItem.Visible = false;//AH
                    应付款项分析IToolStripMenuItem.Visible = false;//AI
                    客户购销分析GToolStripMenuItem.Visible = false;//AG

                    单据再现AToolStripMenuItem.Visible = true;//RA
                    toolStripButtonR.Visible = false;//RA
                    删除资料查询BToolStripMenuItem.Visible = false;//RB
                    购进业务查询CToolStripMenuItem.Visible = false;//RC
                    销售业务查询DToolStripMenuItem.Visible = true;//RD
                    库存业务查询EToolStripMenuItem.Visible = true;//RE
                    商品资料查询FToolStripMenuItem.Visible = false;//RF
                    借物业务查询GToolStripMenuItem.Visible = false;//RG
                    结转单据查询HToolStripMenuItem.Visible = false;//RH
                    基本信息查询IToolStripMenuItem.Visible = true;//RI

                    基本资料维护AToolStripMenuItem.Visible = true;//DA
                    单位档案维护AToolStripMenuItem.Visible = true;
                    往来单位余额登记BToolStripMenuItem.Visible = false;
                    账簿档案维护CToolStripMenuItem.Visible = false;
                    库房档案维护DToolStripMenuItem.Visible = false;
                    部门档案维护EToolStripMenuItem.Visible = false;
                    职员档案维护GToolStripMenuItem.Visible = false;
                    地区档案维护ToolStripMenuItem.Visible = false;
                    职位档案维护ToolStripMenuItem.Visible = false;

                    商品资料定义BToolStripMenuItem.Visible = false;//DB

                    数据备份恢复AToolStripMenuItem.Visible = false;//YA
                    单据日志查询BToolStripMenuItem.Visible = false;//YB
                    数据库设置CToolStripMenuItem.Visible = true;//YC
                    更改密码DToolStripMenuItem.Visible = true;//YD
                    退出系统XToolStripMenuItem.Visible = true;//YX
                    系统参数设置SToolStripMenuItem.Visible = true;//YS
                    用户注销LToolStripMenuItem.Visible = true;//YL

                    数据整理ToolStripMenuItem.Visible = false;//EEE1
                    初始化ToolStripMenuItem.Visible = false;//EEE2
                    数据清除ToolStripMenuItem.Visible = false;//EEE3

                    库存查询CToolStripMenuItem.Visible = true;
                    商品综合查询DToolStripMenuItem.Visible = false;
                    当期商品出入库CToolStripMenuItem.Visible = false;
                    库存所有商品查询BToolStripMenuItem.Visible = true;
                    商品盘点历史查询EToolStripMenuItem.Visible = false;
                    商品报损历史查询FToolStripMenuItem.Visible = false;
                    商品拆装历史查询HToolStripMenuItem.Visible = false;


                    break;
                default:


                    break;


            }
             * */


        }

        private void UserLimitAccess()
        {

            int intGWID = 0, iMax = 0, i,j;
            if (intUserLimit >= 20) //管理员没有权限限制
                return;



            if (intUserID == 0)
                return;

            string sTemp;
            		
            ToolStripItem[] cs1 = menuStrip.Items.Find("准备盘点表AToolStripMenuItem", true);

            sqlConn.Open();

            //得到岗位ID
            sqlComm.CommandText = "SELECT 岗位ID FROM 职员表 WHERE (ID = "+intUserID.ToString()+")";
            sqldr=sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                try
                {
                    intGWID = int.Parse(sqldr.GetValue(0).ToString());
                }
                catch
                {
                }
            }
            sqldr.Close();

            //得到最大级别
            sqlComm.CommandText = "SELECT MAX(模块级别) FROM 模块表";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                try
                {
                    iMax = int.Parse(sqldr.GetValue(0).ToString());
                }
                catch
                {
                }
            }
            sqldr.Close();


            sqlComm.CommandText = "SELECT 模块表.ID, 模块表.模块名称, 模块表.模块代码, 模块表.模块指针, 模块表.上级ID, A.权限, 模块表.模块级别 FROM 模块表 LEFT OUTER JOIN (SELECT 岗位ID, 模块ID, 权限 FROM 模块权限表 WHERE (岗位ID = " + intGWID .ToString()+ ")) AS A ON 模块表.ID = A.模块ID";

            if (dSet.Tables.Contains("模块权限表")) dSet.Tables.Remove("模块权限表");
            sqlDA.Fill(dSet, "模块权限表");

            sqlConn.Close();

            for (i = 1; i <= iMax; i++)
            {
                DataRow[] dtTemp;
                dtTemp = dSet.Tables["模块权限表"].Select("模块级别=" +i.ToString());

                for (j = 0; j < dtTemp.Length; j++)
                {
                    if (dtTemp[j][5].ToString() != "1") //没有权限声明，查看子菜单
                    {
                        DataRow[] dtTemp1;
                        sTemp = "模块代码 LIKE '" + dtTemp[j][2].ToString() + "%' AND 权限=1";
                        dtTemp1 = dSet.Tables["模块权限表"].Select(sTemp);

                        if (dtTemp1.Length < 1) //没有子菜单
                        {
                            ToolStripItem[] cs = menuStrip.Items.Find(dtTemp[j][3].ToString(), true);
                            if (cs.Length > 0)
                            {
                                cs[0].Visible = false;
                                switch (dtTemp[j][3].ToString())
                                {
                                    case "购进ToolStripMenuItem":
                                        toolStripButtonA.Visible = false;
                                        break;
                                    case "销售出库制单BToolStripMenuItem":
                                        toolStripButtonB.Visible = false;
                                        break;
                                    case "准备盘点表AToolStripMenuItem":
                                        toolStripButtonC.Visible = false;
                                        break;

                                    case "商品单品分析AToolStripMenuItem":
                                        toolStripButtonD.Visible = false;
                                        break;
                                    case "单据再现AToolStripMenuItem":
                                        toolStripButtonR.Visible = false;
                                        break;
                                }


                            }
                            else
                                sTemp = "";
                        }
                    }
                }
            }

            初始化ToolStripMenuItem.Visible = false;
            数据清除ToolStripMenuItem.Visible = false;

            系统维护YToolStripMenuItem.Visible = true;

            //toolStripButtonA.Visible = 购进ToolStripMenuItem.Visible;
            //toolStripButtonB.Visible = 销售出库制单BToolStripMenuItem.Visible;
            //toolStripButtonC.Visible = 准备盘点表AToolStripMenuItem.Visible;
            //toolStripButtonD.Visible = 商品单品分析AToolStripMenuItem.Visible;
            //toolStripButtonR.Visible = 单据再现AToolStripMenuItem.Visible;




        }

        private void initStatusBar()
        {
            toolStripStatusLabel.Text = "操作员："+strUserName;
            if (strUserLimit != "")
                toolStripStatusLabel.Text = toolStripStatusLabel.Text + "（"+strUserLimit+"）";
            toolStripStatusLabel.Text = toolStripStatusLabel.Text + ",欢迎登录商业管理进销存系统.....";
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabelTime.Text = "当前系统时间为：" + System.DateTime.Now.ToString("F");
        }

        //采购合同
        public void 购进合同ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormCGHT childFormCGHT = new FormCGHT();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormCGHT.MdiParent = this;
            childFormCGHT.strConn = strConn;

            childFormCGHT.intUserID = intUserID;
            childFormCGHT.intUserLimit = intUserLimit;
            childFormCGHT.strUserLimit = strUserLimit;
            childFormCGHT.strUserName = strUserName;
            childFormCGHT.intUserBM = intUserBM;

            childFormCGHT.iVersion = iVersion;

            childFormCGHT.Show();
        }

        //购进商品制单
        public void 购进ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormGJSPZD childFormGJSPZD = new FormGJSPZD();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormGJSPZD.MdiParent = this;

            childFormGJSPZD.strConn = strConn;

            childFormGJSPZD.iVersion = iVersion;

            if (intUserLimit < iConstLimit)
            {
                childFormGJSPZD.printToolStripButton.Visible = false;
                childFormGJSPZD.printPreviewToolStripButton.Visible = false;
            }

            childFormGJSPZD.intUserID = intUserID;
            childFormGJSPZD.intUserLimit = intUserLimit;
            childFormGJSPZD.strUserLimit = strUserLimit;
            childFormGJSPZD.strUserName = strUserName;
            childFormGJSPZD.intUserBM = intUserBM;

            childFormGJSPZD.iVersion = iVersion;

            childFormGJSPZD.Show();
        }

        public void 商品验货入库CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormJHRKYHD childFormJHRKYHD = new FormJHRKYHD();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormJHRKYHD.MdiParent = this;

            childFormJHRKYHD.strConn = strConn;

            if (intUserLimit < iConstLimit)
            {
                childFormJHRKYHD.printToolStripButton.Visible = false;
                childFormJHRKYHD.printPreviewToolStripButton.Visible = false;
            }

            childFormJHRKYHD.intUserID = intUserID;
            childFormJHRKYHD.intUserLimit = intUserLimit;
            childFormJHRKYHD.strUserLimit = strUserLimit;
            childFormJHRKYHD.strUserName = strUserName;
            childFormJHRKYHD.intUserBM = intUserBM;

            childFormJHRKYHD.Show();
        }

        private void 进货退出制单DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormJHTCZD childFormJHTCZD = new FormJHTCZD();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormJHTCZD.MdiParent = this;

            childFormJHTCZD.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormJHTCZD.printToolStripButton.Visible = false;
                childFormJHTCZD.printPreviewToolStripButton.Visible = false;
            }

            childFormJHTCZD.intUserID = intUserID;
            childFormJHTCZD.intUserLimit = intUserLimit;
            childFormJHTCZD.strUserLimit = strUserLimit;
            childFormJHTCZD.strUserName = strUserName;
            childFormJHTCZD.intUserBM = intUserBM;
            childFormJHTCZD.Show();
        }

        public void 进货退补价单据EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormJHTBJDJ childFormJHTBJDJ = new FormJHTBJDJ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormJHTBJDJ.MdiParent = this;

            childFormJHTBJDJ.strConn = strConn;

            if (intUserLimit < iConstLimit)
            {
                childFormJHTBJDJ.printToolStripButton.Visible = false;
                childFormJHTBJDJ.printPreviewToolStripButton.Visible = false;
            }

            childFormJHTBJDJ.intUserID = intUserID;
            childFormJHTBJDJ.intUserLimit = intUserLimit;
            childFormJHTBJDJ.strUserLimit = strUserLimit;
            childFormJHTBJDJ.strUserName = strUserName;
            childFormJHTBJDJ.intUserBM = intUserBM;

            childFormJHTBJDJ.Show();
        }

        public void 应付账款结算FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormYFZKJS childFormYFZKJS = new FormYFZKJS();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormYFZKJS.MdiParent = this;

            childFormYFZKJS.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormYFZKJS.printToolStripButton.Visible = false;
                childFormYFZKJS.printPreviewToolStripButton.Visible = false;
            }

            childFormYFZKJS.intUserID = intUserID;
            childFormYFZKJS.intUserLimit = intUserLimit;
            childFormYFZKJS.strUserLimit = strUserLimit;
            childFormYFZKJS.strUserName = strUserName;
            childFormYFZKJS.intUserBM = intUserBM;
            childFormYFZKJS.Show();
        }

        public void 应收账款ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormYSZKJS childFormYSZKJS = new FormYSZKJS();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormYSZKJS.MdiParent = this;

            childFormYSZKJS.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormYSZKJS.printToolStripButton.Visible = false;
                childFormYSZKJS.printPreviewToolStripButton.Visible = false;
            }

            childFormYSZKJS.intUserID = intUserID;
            childFormYSZKJS.intUserLimit = intUserLimit;
            childFormYSZKJS.strUserLimit = strUserLimit;
            childFormYSZKJS.strUserName = strUserName;
            childFormYSZKJS.intUserBM = intUserBM;
            childFormYSZKJS.Show();
        }

        //销售合同制单
        public void 销售合同制单AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXSHT childFormXSHT = new FormXSHT();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXSHT.MdiParent = this;
            childFormXSHT.strConn = strConn;

            childFormXSHT.intUserID = intUserID;
            childFormXSHT.intUserLimit = intUserLimit;
            childFormXSHT.strUserLimit = strUserLimit;
            childFormXSHT.strUserName = strUserName;
            childFormXSHT.intUserBM = intUserBM;

            childFormXSHT.iVersion = iVersion;

            childFormXSHT.Show();
        }

        public void 销售出库制单BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXSCKZD childFormXSCKZD = new FormXSCKZD();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXSCKZD.MdiParent = this;
            childFormXSCKZD.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXSCKZD.printToolStripButton.Visible = false;
                childFormXSCKZD.printPreviewToolStripButton.Visible = false;
            }

            childFormXSCKZD.intUserID = intUserID;
            childFormXSCKZD.intUserLimit = intUserLimit;
            childFormXSCKZD.strUserLimit = strUserLimit;
            childFormXSCKZD.strUserName = strUserName;
            childFormXSCKZD.intUserBM = intUserBM;

            childFormXSCKZD.iVersion = iVersion;

            childFormXSCKZD.Show();
        }

        public void 销售出库校对CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXSCKJD childFormXSCKJD = new FormXSCKJD();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXSCKJD.MdiParent = this;
            childFormXSCKJD.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXSCKJD.printToolStripButton.Visible = false;
                childFormXSCKJD.printPreviewToolStripButton.Visible = false;
            }

            childFormXSCKJD.intUserID = intUserID;
            childFormXSCKJD.intUserLimit = intUserLimit;
            childFormXSCKJD.strUserLimit = strUserLimit;
            childFormXSCKJD.strUserName = strUserName;
            childFormXSCKJD.intUserBM = intUserBM;

            childFormXSCKJD.Show();
        }

        public void 修改送货信息DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXGSHXX childFormXGSHXX = new FormXGSHXX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXGSHXX.MdiParent = this;
            childFormXGSHXX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXGSHXX.printToolStripButton.Visible = false;
                childFormXGSHXX.printPreviewToolStripButton.Visible = false;
            }

            childFormXGSHXX.intUserID = intUserID;
            childFormXGSHXX.intUserLimit = intUserLimit;
            childFormXGSHXX.strUserLimit = strUserLimit;
            childFormXGSHXX.strUserName = strUserName;
            childFormXGSHXX.intUserBM = intUserBM;

            childFormXGSHXX.Show();
        }

        public void 销售退回制单EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXSTHZD childFormXSTHZD = new FormXSTHZD();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXSTHZD.MdiParent = this;
            childFormXSTHZD.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXSTHZD.printToolStripButton.Visible = false;
                childFormXSTHZD.printPreviewToolStripButton.Visible = false;
            }

            childFormXSTHZD.intUserID = intUserID;
            childFormXSTHZD.intUserLimit = intUserLimit;
            childFormXSTHZD.strUserLimit = strUserLimit;
            childFormXSTHZD.strUserName = strUserName;
            childFormXSTHZD.intUserBM = intUserBM;

            childFormXSTHZD.Show();
        }

        public void 销售退补价单据FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXSTBJDJ childFormXSTBJDJ = new FormXSTBJDJ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXSTBJDJ.MdiParent = this;

            childFormXSTBJDJ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXSTBJDJ.printToolStripButton.Visible = false;
                childFormXSTBJDJ.printPreviewToolStripButton.Visible = false;
            }

            childFormXSTBJDJ.intUserID = intUserID;
            childFormXSTBJDJ.intUserLimit = intUserLimit;
            childFormXSTBJDJ.strUserLimit = strUserLimit;
            childFormXSTBJDJ.strUserName = strUserName;
            childFormXSTBJDJ.intUserBM = intUserBM;
            childFormXSTBJDJ.Show();
        }

        public void 准备盘点表AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKCSPPD1 childFormKCSPPD1 = new FormKCSPPD1();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKCSPPD1.MdiParent = this;

            childFormKCSPPD1.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKCSPPD1.printToolStripButton.Visible = false;
                childFormKCSPPD1.printPreviewToolStripButton.Visible = false;
            }

            childFormKCSPPD1.intUserID = intUserID;
            childFormKCSPPD1.intUserLimit = intUserLimit;
            childFormKCSPPD1.strUserLimit = strUserLimit;
            childFormKCSPPD1.strUserName = strUserName;
            childFormKCSPPD1.intUserBM = intUserBM;

            childFormKCSPPD1.Show();
        }

        public void 实盘数据登录CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKCSPPD2 childFormKCSPPD2 = new FormKCSPPD2();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKCSPPD2.MdiParent = this;

            childFormKCSPPD2.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKCSPPD2.printToolStripButton.Visible = false;
                childFormKCSPPD2.printPreviewToolStripButton.Visible = false;
            }

            childFormKCSPPD2.intUserID = intUserID;
            childFormKCSPPD2.intUserLimit = intUserLimit;
            childFormKCSPPD2.strUserLimit = strUserLimit;
            childFormKCSPPD2.strUserName = strUserName;
            childFormKCSPPD2.intUserBM = intUserBM;
            childFormKCSPPD2.Show();
        }

        public void 借物出库制单AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKCJWCKDJ childFormKCJWCKDJ = new FormKCJWCKDJ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKCJWCKDJ.MdiParent = this;

            childFormKCJWCKDJ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKCJWCKDJ.printToolStripButton.Visible = false;
                childFormKCJWCKDJ.printPreviewToolStripButton.Visible = false;
            }

            childFormKCJWCKDJ.intUserID = intUserID;
            childFormKCJWCKDJ.intUserLimit = intUserLimit;
            childFormKCJWCKDJ.strUserLimit = strUserLimit;
            childFormKCJWCKDJ.strUserName = strUserName;
            childFormKCJWCKDJ.intUserBM = intUserBM;
            childFormKCJWCKDJ.Show();
        }

        public void 修改借物单据BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXGJWDXX childFormXGJWDXX = new FormXGJWDXX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXGJWDXX.MdiParent = this;

            childFormXGJWDXX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXGJWDXX.printToolStripButton.Visible = false;
                childFormXGJWDXX.printPreviewToolStripButton.Visible = false;
            }

            childFormXGJWDXX.intUserID = intUserID;
            childFormXGJWDXX.intUserLimit = intUserLimit;
            childFormXGJWDXX.strUserLimit = strUserLimit;
            childFormXGJWDXX.strUserName = strUserName;
            childFormXGJWDXX.intUserBM = intUserBM;
            childFormXGJWDXX.Show();
        }

        private void 商品组装AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKCSPZZ childFormKCSPZZ = new FormKCSPZZ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKCSPZZ.MdiParent = this;

            childFormKCSPZZ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKCSPZZ.printToolStripButton.Visible = false;
                childFormKCSPZZ.printPreviewToolStripButton.Visible = false;
            }

            childFormKCSPZZ.intUserID = intUserID;
            childFormKCSPZZ.intUserLimit = intUserLimit;
            childFormKCSPZZ.strUserLimit = strUserLimit;
            childFormKCSPZZ.strUserName = strUserName;
            childFormKCSPZZ.intUserBM = intUserBM;

            childFormKCSPZZ.Show();
        }

        public void 商品拆散BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKCSPCS childFormKCSPCS = new FormKCSPCS();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKCSPCS.MdiParent = this;

            childFormKCSPCS.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKCSPCS.printToolStripButton.Visible = false;
                childFormKCSPCS.printPreviewToolStripButton.Visible = false;
            }

            childFormKCSPCS.intUserID = intUserID;
            childFormKCSPCS.intUserLimit = intUserLimit;
            childFormKCSPCS.strUserLimit = strUserLimit;
            childFormKCSPCS.strUserName = strUserName;
            childFormKCSPCS.intUserBM = intUserBM;

            childFormKCSPCS.Show();
        }

        public void 库存预警维护DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKCSXXWH childFormKCSXXWH = new FormKCSXXWH();

            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKCSXXWH.MdiParent = this;

            childFormKCSXXWH.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKCSXXWH.printToolStripButton.Visible = false;
                childFormKCSXXWH.printPreviewToolStripButton.Visible = false;
            }

            childFormKCSXXWH.intUserID = intUserID;
            childFormKCSXXWH.intUserLimit = intUserLimit;
            childFormKCSXXWH.strUserLimit = strUserLimit;
            childFormKCSXXWH.strUserName = strUserName;
            childFormKCSXXWH.intUserBM = intUserBM;

            childFormKCSXXWH.Show();

        }

        public void 商品库存预警EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPKCYJ childFormSPKCYJ = new FormSPKCYJ();

            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPKCYJ.MdiParent = this;

            childFormSPKCYJ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPKCYJ.printToolStripButton.Visible = false;
                childFormSPKCYJ.printPreviewToolStripButton.Visible = false;
            }

            childFormSPKCYJ.intUserID = intUserID;
            childFormSPKCYJ.intUserLimit = intUserLimit;
            childFormSPKCYJ.strUserLimit = strUserLimit;
            childFormSPKCYJ.strUserName = strUserName;
            childFormSPKCYJ.intUserBM = intUserBM;

            childFormSPKCYJ.Show();
        }

        public void 库存商品报损GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKCSPBSCL childFormFormKCSPBSCL = new FormKCSPBSCL();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormFormKCSPBSCL.MdiParent = this;

            childFormFormKCSPBSCL.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormFormKCSPBSCL.printToolStripButton.Visible = false;
                childFormFormKCSPBSCL.printPreviewToolStripButton.Visible = false;
            }

            childFormFormKCSPBSCL.intUserID = intUserID;
            childFormFormKCSPBSCL.intUserLimit = intUserLimit;
            childFormFormKCSPBSCL.strUserLimit = strUserLimit;
            childFormFormKCSPBSCL.strUserName = strUserName;
            childFormFormKCSPBSCL.intUserBM = intUserBM;

            childFormFormKCSPBSCL.Show();
        }

        public void 商品账目处理AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKCSPZMCL childFormKCSPZMCL = new FormKCSPZMCL();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKCSPZMCL.MdiParent = this;

            childFormKCSPZMCL.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKCSPZMCL.printToolStripButton.Visible = false;
                childFormKCSPZMCL.printPreviewToolStripButton.Visible = false;
            }

            childFormKCSPZMCL.intUserID = intUserID;
            childFormKCSPZMCL.intUserLimit = intUserLimit;
            childFormKCSPZMCL.strUserLimit = strUserLimit;
            childFormKCSPZMCL.strUserName = strUserName;
            childFormKCSPZMCL.intUserBM = intUserBM;

            childFormKCSPZMCL.Show();
        }

        public void 制作调价通知单AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormTJTZD1 childFormTJTZD1 = new FormTJTZD1();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormTJTZD1.MdiParent = this;

            childFormTJTZD1.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormTJTZD1.printToolStripButton.Visible = false;
                childFormTJTZD1.printPreviewToolStripButton.Visible = false;
            }

            childFormTJTZD1.intUserID = intUserID;
            childFormTJTZD1.intUserLimit = intUserLimit;
            childFormTJTZD1.strUserLimit = strUserLimit;
            childFormTJTZD1.strUserName = strUserName;
            childFormTJTZD1.intUserBM = intUserBM;

            childFormTJTZD1.Show();
        }

        public void 执行调价通知单BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormTJTZD2 childFormTJTZD2 = new FormTJTZD2();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormTJTZD2.MdiParent = this;

            childFormTJTZD2.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormTJTZD2.printToolStripButton.Visible = false;
                childFormTJTZD2.printPreviewToolStripButton.Visible = false;
            }

            childFormTJTZD2.intUserID = intUserID;
            childFormTJTZD2.intUserLimit = intUserLimit;
            childFormTJTZD2.strUserLimit = strUserLimit;
            childFormTJTZD2.strUserName = strUserName;
            childFormTJTZD2.intUserBM = intUserBM;

            childFormTJTZD2.Show();
        }

        public void 进销单据冲红ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormCWDJCL childFormCWDJCL = new FormCWDJCL();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormCWDJCL.MdiParent = this;

            childFormCWDJCL.strConn = strConn;
            childFormCWDJCL.iStyle = 0;
            if (intUserLimit < iConstLimit)
            {
                childFormCWDJCL.printToolStripButton.Visible = false;
                childFormCWDJCL.printPreviewToolStripButton.Visible = false;
            }


            childFormCWDJCL.intUserID = intUserID;
            childFormCWDJCL.intUserLimit = intUserLimit;
            childFormCWDJCL.strUserLimit = strUserLimit;
            childFormCWDJCL.strUserName = strUserName;
            childFormCWDJCL.intUserBM = intUserBM;

            childFormCWDJCL.Show();
        }

        public void 进销单据修改BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormCWDJCL childFormCWDJCL = new FormCWDJCL();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormCWDJCL.MdiParent = this;

            childFormCWDJCL.strConn = strConn;
            childFormCWDJCL.iStyle = 1;
            if (intUserLimit < iConstLimit)
            {
                childFormCWDJCL.printToolStripButton.Visible = false;
                childFormCWDJCL.printPreviewToolStripButton.Visible = false;
            }


            childFormCWDJCL.intUserID = intUserID;
            childFormCWDJCL.intUserLimit = intUserLimit;
            childFormCWDJCL.strUserLimit = strUserLimit;
            childFormCWDJCL.strUserName = strUserName;
            childFormCWDJCL.intUserBM = intUserBM;

            childFormCWDJCL.Show();
        }

        public void 开具发票AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormFPKJ childFormFPKJ = new FormFPKJ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormFPKJ.MdiParent = this;

            childFormFPKJ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormFPKJ.printToolStripButton.Visible = false;
                childFormFPKJ.printPreviewToolStripButton.Visible = false;
            }

            childFormFPKJ.intUserID = intUserID;
            childFormFPKJ.intUserLimit = intUserLimit;
            childFormFPKJ.strUserLimit = strUserLimit;
            childFormFPKJ.strUserName = strUserName;
            childFormFPKJ.intUserBM = intUserBM;

            childFormFPKJ.Show();
        }

        public void 发票查询BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormFPCX childFormFPCX = new FormFPCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormFPCX.MdiParent = this;

            childFormFPCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormFPCX.printToolStripButton.Visible = false;
                childFormFPCX.printPreviewToolStripButton.Visible = false;
            }

            childFormFPCX.intUserID = intUserID;
            childFormFPCX.intUserLimit = intUserLimit;
            childFormFPCX.strUserLimit = strUserLimit;
            childFormFPCX.strUserName = strUserName;
            childFormFPCX.intUserBM = intUserBM;

            childFormFPCX.Show();
        }

        private void 发票作废CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormFPZF childFormFPZF = new FormFPZF();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormFPZF.MdiParent = this;

            childFormFPZF.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormFPZF.printToolStripButton.Visible = false;
                childFormFPZF.printPreviewToolStripButton.Visible = false;
            }

            childFormFPZF.intUserID = intUserID;
            childFormFPZF.intUserLimit = intUserLimit;
            childFormFPZF.strUserLimit = strUserLimit;
            childFormFPZF.strUserName = strUserName;
            childFormFPZF.intUserBM = intUserBM;

            childFormFPZF.Show();
        }

        public void 业务数据结转AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormYWSJJZ childFormYWSJJZ = new FormYWSJJZ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormYWSJJZ.MdiParent = this;

            childFormYWSJJZ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormYWSJJZ.printToolStripButton.Visible = false;
                childFormYWSJJZ.printPreviewToolStripButton.Visible = false;
            }

            childFormYWSJJZ.intUserID = intUserID;
            childFormYWSJJZ.intUserLimit = intUserLimit;
            childFormYWSJJZ.strUserLimit = strUserLimit;
            childFormYWSJJZ.strUserName = strUserName;
            childFormYWSJJZ.intUserBM = intUserBM;

            childFormYWSJJZ.Show();
        }

        public void 商品总账查询AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPZZZY childFormFormSPZZZY = new FormSPZZZY();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormFormSPZZZY.MdiParent = this;

            childFormFormSPZZZY.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormFormSPZZZY.printToolStripButton.Visible = false;
                childFormFormSPZZZY.printPreviewToolStripButton.Visible = false;
            }

            childFormFormSPZZZY.intUserID = intUserID;
            childFormFormSPZZZY.intUserLimit = intUserLimit;
            childFormFormSPZZZY.strUserLimit = strUserLimit;
            childFormFormSPZZZY.strUserName = strUserName;
            childFormFormSPZZZY.intUserBM = intUserBM;

            childFormFormSPZZZY.Show();
        }

        public void 库房商品查询BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKFSPZY childFormKFSPZY = new FormKFSPZY();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKFSPZY.MdiParent = this;

            childFormKFSPZY.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKFSPZY.printToolStripButton.Visible = false;
                childFormKFSPZY.printPreviewToolStripButton.Visible = false;
            }

            childFormKFSPZY.intUserID = intUserID;
            childFormKFSPZY.intUserLimit = intUserLimit;
            childFormKFSPZY.strUserLimit = strUserLimit;
            childFormKFSPZY.strUserName = strUserName;
            childFormKFSPZY.intUserBM = intUserBM;

            childFormKFSPZY.Show();
        }

        public void 商品总结存账DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPZJC childFormSPZJC = new FormSPZJC();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPZJC.MdiParent = this;

            childFormSPZJC.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPZJC.printToolStripButton.Visible = false;
                childFormSPZJC.printPreviewToolStripButton.Visible = false;
            }

            childFormSPZJC.intUserID = intUserID;
            childFormSPZJC.intUserLimit = intUserLimit;
            childFormSPZJC.strUserLimit = strUserLimit;
            childFormSPZJC.strUserName = strUserName;
            childFormSPZJC.intUserBM = intUserBM;

            childFormSPZJC.Show();
        }

        public void 库房商品结存账BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKFSPJCZ childFormKFSPJCZ = new FormKFSPJCZ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKFSPJCZ.MdiParent = this;

            childFormKFSPJCZ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKFSPJCZ.printToolStripButton.Visible = false;
                childFormKFSPJCZ.printPreviewToolStripButton.Visible = false;
            }

            childFormKFSPJCZ.intUserID = intUserID;
            childFormKFSPJCZ.intUserLimit = intUserLimit;
            childFormKFSPJCZ.strUserLimit = strUserLimit;
            childFormKFSPJCZ.strUserName = strUserName;
            childFormKFSPJCZ.intUserBM = intUserBM;

            childFormKFSPJCZ.Show();
        }

        public void 进货单据平行账AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            // 创建此子窗体的一个新实例。
            FormJHDJPXZ childFormJHDJPXZ = new FormJHDJPXZ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormJHDJPXZ.MdiParent = this;

            childFormJHDJPXZ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormJHDJPXZ.printToolStripButton.Visible = false;
                childFormJHDJPXZ.printPreviewToolStripButton.Visible = false;
            }

            childFormJHDJPXZ.intUserID = intUserID;
            childFormJHDJPXZ.intUserLimit = intUserLimit;
            childFormJHDJPXZ.strUserLimit = strUserLimit;
            childFormJHDJPXZ.strUserName = strUserName;
            childFormJHDJPXZ.intUserBM = intUserBM;

            childFormJHDJPXZ.Show();
        }

        public void 购进单位往来账AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormGJDWWLZ childFormGJDWWLZ = new FormGJDWWLZ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormGJDWWLZ.MdiParent = this;

            childFormGJDWWLZ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormGJDWWLZ.printToolStripButton.Visible = false;
                childFormGJDWWLZ.printPreviewToolStripButton.Visible = false;
            }

            childFormGJDWWLZ.intUserID = intUserID;
            childFormGJDWWLZ.intUserLimit = intUserLimit;
            childFormGJDWWLZ.strUserLimit = strUserLimit;
            childFormGJDWWLZ.strUserName = strUserName;
            childFormGJDWWLZ.intUserBM = intUserBM;

            childFormGJDWWLZ.Show();
        }

        public void 销售单位往来账BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXWDWWLZ childFormXWDWWLZ = new FormXWDWWLZ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXWDWWLZ.MdiParent = this;

            childFormXWDWWLZ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXWDWWLZ.printToolStripButton.Visible = false;
                childFormXWDWWLZ.printPreviewToolStripButton.Visible = false;
            }

            childFormXWDWWLZ.intUserID = intUserID;
            childFormXWDWWLZ.intUserLimit = intUserLimit;
            childFormXWDWWLZ.strUserLimit = strUserLimit;
            childFormXWDWWLZ.strUserName = strUserName;
            childFormXWDWWLZ.intUserBM = intUserBM;

            childFormXWDWWLZ.Show();
        }

        public void 业务单位综合余额账CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormYWDWZHYEZ childFormYWDWZHYEZ = new FormYWDWZHYEZ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormYWDWZHYEZ.MdiParent = this;

            childFormYWDWZHYEZ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormYWDWZHYEZ.printToolStripButton.Visible = false;
                childFormYWDWZHYEZ.printPreviewToolStripButton.Visible = false;
            }

            childFormYWDWZHYEZ.intUserID = intUserID;
            childFormYWDWZHYEZ.intUserLimit = intUserLimit;
            childFormYWDWZHYEZ.strUserLimit = strUserLimit;
            childFormYWDWZHYEZ.strUserName = strUserName;
            childFormYWDWZHYEZ.intUserBM = intUserBM;

            childFormYWDWZHYEZ.Show();
        }

        public void 商品单品分析AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPDPFX childFormSPDPFX = new FormSPDPFX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPDPFX.MdiParent = this;

            childFormSPDPFX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPDPFX.printToolStripButton.Visible = false;
                childFormSPDPFX.printPreviewToolStripButton.Visible = false;
            }

            childFormSPDPFX.intUserID = intUserID;
            childFormSPDPFX.intUserLimit = intUserLimit;
            childFormSPDPFX.strUserLimit = strUserLimit;
            childFormSPDPFX.strUserName = strUserName;
            childFormSPDPFX.intUserBM = intUserBM;

            childFormSPDPFX.Show();
        }

        public void 商品库存压占分析AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPKCYZFX childFormSPKCYZFX = new FormSPKCYZFX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPKCYZFX.MdiParent = this;

            childFormSPKCYZFX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPKCYZFX.printToolStripButton.Visible = false;
                childFormSPKCYZFX.printPreviewToolStripButton.Visible = false;
            }

            childFormSPKCYZFX.intUserID = intUserID;
            childFormSPKCYZFX.intUserLimit = intUserLimit;
            childFormSPKCYZFX.strUserLimit = strUserLimit;
            childFormSPKCYZFX.strUserName = strUserName;
            childFormSPKCYZFX.intUserBM = intUserBM;

            childFormSPKCYZFX.Show();
        }

        public void 商品库存结构分析BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPKCJGFX childFormSPKCJGFX = new FormSPKCJGFX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPKCJGFX.MdiParent = this;

            childFormSPKCJGFX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPKCJGFX.printToolStripButton.Visible = false;
                childFormSPKCJGFX.printPreviewToolStripButton.Visible = false;
            }

            childFormSPKCJGFX.intUserID = intUserID;
            childFormSPKCJGFX.intUserLimit = intUserLimit;
            childFormSPKCJGFX.strUserLimit = strUserLimit;
            childFormSPKCJGFX.strUserName = strUserName;
            childFormSPKCJGFX.intUserBM = intUserBM;

            childFormSPKCJGFX.Show();
        }

        public void 商品销售回款分析AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPXSHKFX childFormSPXSHKFX = new FormSPXSHKFX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPXSHKFX.MdiParent = this;

            childFormSPXSHKFX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPXSHKFX.printToolStripButton.Visible = false;
                childFormSPXSHKFX.printPreviewToolStripButton.Visible = false;
            }

            childFormSPXSHKFX.intUserID = intUserID;
            childFormSPXSHKFX.intUserLimit = intUserLimit;
            childFormSPXSHKFX.strUserLimit = strUserLimit;
            childFormSPXSHKFX.strUserName = strUserName;
            childFormSPXSHKFX.intUserBM = intUserBM;

            childFormSPXSHKFX.Show();
        }

        public void 销品批发价格分析AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXPPFJGFX childFormXPPFJGFX = new FormXPPFJGFX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXPPFJGFX.MdiParent = this;

            childFormXPPFJGFX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXPPFJGFX.printToolStripButton.Visible = false;
                childFormXPPFJGFX.printPreviewToolStripButton.Visible = false;
            }

            childFormXPPFJGFX.intUserID = intUserID;
            childFormXPPFJGFX.intUserLimit = intUserLimit;
            childFormXPPFJGFX.strUserLimit = strUserLimit;
            childFormXPPFJGFX.strUserName = strUserName;
            childFormXPPFJGFX.intUserBM = intUserBM;

            childFormXPPFJGFX.Show();
        }

        public void 批发销售状态表AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXSZTFB childFormXSZTFB = new FormXSZTFB();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXSZTFB.MdiParent = this;

            childFormXSZTFB.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXSZTFB.printToolStripButton.Visible = false;
                childFormXSZTFB.printPreviewToolStripButton.Visible = false;
            }

            childFormXSZTFB.intUserID = intUserID;
            childFormXSZTFB.intUserLimit = intUserLimit;
            childFormXSZTFB.strUserLimit = strUserLimit;
            childFormXSZTFB.strUserName = strUserName;
            childFormXSZTFB.intUserBM = intUserBM;

            childFormXSZTFB.Show();
        }

        public void 批发销售日报表BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormPFXSRBB childFormPFXSRBB = new FormPFXSRBB();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormPFXSRBB.MdiParent = this;

            childFormPFXSRBB.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormPFXSRBB.printToolStripButton.Visible = false;
                childFormPFXSRBB.printPreviewToolStripButton.Visible = false;
            }

            childFormPFXSRBB.intUserID = intUserID;
            childFormPFXSRBB.intUserLimit = intUserLimit;
            childFormPFXSRBB.strUserLimit = strUserLimit;
            childFormPFXSRBB.strUserName = strUserName;
            childFormPFXSRBB.intUserBM = intUserBM;

            childFormPFXSRBB.Show();
        }

        public void 本期经营概况AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormBQJYGK childFormBQJYGK = new FormBQJYGK();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormBQJYGK.MdiParent = this;

            childFormBQJYGK.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormBQJYGK.printToolStripButton.Visible = false;
                childFormBQJYGK.printPreviewToolStripButton.Visible = false;
            }

            childFormBQJYGK.intUserID = intUserID;
            childFormBQJYGK.intUserLimit = intUserLimit;
            childFormBQJYGK.strUserLimit = strUserLimit;
            childFormBQJYGK.strUserName = strUserName;
            childFormBQJYGK.intUserBM = intUserBM;

            childFormBQJYGK.Show();
        }

        public void 总体经营概况BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormJYZK cildFormJYZK = new FormJYZK();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            cildFormJYZK.MdiParent = this;

            cildFormJYZK.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                cildFormJYZK.printToolStripButton.Visible = false;
                cildFormJYZK.printPreviewToolStripButton.Visible = false;
            }

            cildFormJYZK.intUserID = intUserID;
            cildFormJYZK.intUserLimit = intUserLimit;
            cildFormJYZK.strUserLimit = strUserLimit;
            cildFormJYZK.strUserName = strUserName;
            cildFormJYZK.intUserBM = intUserBM;

            cildFormJYZK.Show();
        }

        public void 销售同期比较CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormTQXSBJFX cildFormTQXSBJFX = new FormTQXSBJFX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            cildFormTQXSBJFX.MdiParent = this;

            cildFormTQXSBJFX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                cildFormTQXSBJFX.printToolStripButton.Visible = false;
                cildFormTQXSBJFX.printPreviewToolStripButton.Visible = false;
            }

            cildFormTQXSBJFX.intUserID = intUserID;
            cildFormTQXSBJFX.intUserLimit = intUserLimit;
            cildFormTQXSBJFX.strUserLimit = strUserLimit;
            cildFormTQXSBJFX.strUserName = strUserName;
            cildFormTQXSBJFX.intUserBM = intUserBM;

            cildFormTQXSBJFX.Show();
        }

        public void 排行综合分析GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPXSPHFX childFormSPXSPHFX = new FormSPXSPHFX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPXSPHFX.MdiParent = this;

            childFormSPXSPHFX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPXSPHFX.printToolStripButton.Visible = false;
                childFormSPXSPHFX.printPreviewToolStripButton.Visible = false;
            }

            childFormSPXSPHFX.intUserID = intUserID;
            childFormSPXSPHFX.intUserLimit = intUserLimit;
            childFormSPXSPHFX.strUserLimit = strUserLimit;
            childFormSPXSPHFX.strUserName = strUserName;
            childFormSPXSPHFX.intUserBM = intUserBM;

            childFormSPXSPHFX.Show();
        }

        public void 应收款项分析HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormYSKFX childFormYSKFX = new FormYSKFX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormYSKFX.MdiParent = this;

            childFormYSKFX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormYSKFX.printToolStripButton.Visible = false;
                childFormYSKFX.printPreviewToolStripButton.Visible = false;
            }


            childFormYSKFX.intUserID = intUserID;
            childFormYSKFX.intUserLimit = intUserLimit;
            childFormYSKFX.strUserLimit = strUserLimit;
            childFormYSKFX.strUserName = strUserName;
            childFormYSKFX.intUserBM = intUserBM;

            childFormYSKFX.Show();
        }

        public void 应付款项分析IToolStripMenuItem_Click(object sender, EventArgs e)
        {
             // 创建此子窗体的一个新实例。
            FormYFKFX childFormYFKFX = new FormYFKFX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormYFKFX.MdiParent = this;

            childFormYFKFX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormYFKFX.printToolStripButton.Visible = false;
                childFormYFKFX.printPreviewToolStripButton.Visible = false;
            }

            childFormYFKFX.intUserID = intUserID;
            childFormYFKFX.intUserLimit = intUserLimit;
            childFormYFKFX.strUserLimit = strUserLimit;
            childFormYFKFX.strUserName = strUserName;
            childFormYFKFX.intUserBM = intUserBM;

            childFormYFKFX.Show();
        }

        public void 单据再现AToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // 创建此子窗体的一个新实例。
            FormDJZX childFormDJZX = new FormDJZX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormDJZX.MdiParent = this;

            childFormDJZX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormDJZX.printToolStripButton.Visible = false;
                childFormDJZX.printPreviewToolStripButton.Visible = false;
            }

            childFormDJZX.intUserID = intUserID;
            childFormDJZX.intUserLimit = intUserLimit;
            childFormDJZX.strUserLimit = strUserLimit;
            childFormDJZX.strUserName = strUserName;
            childFormDJZX.intUserBM = intUserBM;

            childFormDJZX.Show();
        }

        public void 删除商品查询AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSCSPCX childFormSCSPCX = new FormSCSPCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSCSPCX.MdiParent = this;

            childFormSCSPCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSCSPCX.printToolStripButton.Visible = false;
                childFormSCSPCX.printPreviewToolStripButton.Visible = false;
            }

            childFormSCSPCX.intUserID = intUserID;
            childFormSCSPCX.intUserLimit = intUserLimit;
            childFormSCSPCX.strUserLimit = strUserLimit;
            childFormSCSPCX.strUserName = strUserName;
            childFormSCSPCX.intUserBM = intUserBM;

            childFormSCSPCX.Show();
        }

        public void 删除单位查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSCDWCX childFormSCDWCX = new FormSCDWCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSCDWCX.MdiParent = this;

            childFormSCDWCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSCDWCX.printToolStripButton.Visible = false;
                childFormSCDWCX.printPreviewToolStripButton.Visible = false;
            }

            childFormSCDWCX.intUserID = intUserID;
            childFormSCDWCX.intUserLimit = intUserLimit;
            childFormSCDWCX.strUserLimit = strUserLimit;
            childFormSCDWCX.strUserName = strUserName;
            childFormSCDWCX.intUserBM = intUserBM;

            childFormSCDWCX.Show();
        }

        private void 删除库房查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSCKFCX childFormSCKFCX = new FormSCKFCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSCKFCX.MdiParent = this;

            childFormSCKFCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSCKFCX.printToolStripButton.Visible = false;
                childFormSCKFCX.printPreviewToolStripButton.Visible = false;
            }

            childFormSCKFCX.intUserID = intUserID;
            childFormSCKFCX.intUserLimit = intUserLimit;
            childFormSCKFCX.strUserLimit = strUserLimit;
            childFormSCKFCX.strUserName = strUserName;
            childFormSCKFCX.intUserBM = intUserBM;

            childFormSCKFCX.Show();
        }



        public void 购进业务查询AToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormGJYWCX childFormGJYWCX = new FormGJYWCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormGJYWCX.MdiParent = this;

            childFormGJYWCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormGJYWCX.printToolStripButton.Visible = false;
                childFormGJYWCX.printPreviewToolStripButton.Visible = false;
            }

            childFormGJYWCX.intUserID = intUserID;
            childFormGJYWCX.intUserLimit = intUserLimit;
            childFormGJYWCX.strUserLimit = strUserLimit;
            childFormGJYWCX.strUserName = strUserName;
            childFormGJYWCX.intUserBM = intUserBM;

            childFormGJYWCX.Show();
        }

        public void 购进退补查询BToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormGJTBJCX childFormGJTBJCX = new FormGJTBJCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormGJTBJCX.MdiParent = this;

            childFormGJTBJCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormGJTBJCX.printToolStripButton.Visible = false;
                childFormGJTBJCX.printPreviewToolStripButton.Visible = false;
            }

            childFormGJTBJCX.intUserID = intUserID;
            childFormGJTBJCX.intUserLimit = intUserLimit;
            childFormGJTBJCX.strUserLimit = strUserLimit;
            childFormGJTBJCX.strUserName = strUserName;
            childFormGJTBJCX.intUserBM = intUserBM;

            childFormGJTBJCX.Show();
        }

        private void 购进退出查询CToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormGJTCCX childFormGJTCCX = new FormGJTCCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormGJTCCX.MdiParent = this;

            childFormGJTCCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormGJTCCX.printToolStripButton.Visible = false;
                childFormGJTCCX.printPreviewToolStripButton.Visible = false;
            }

            childFormGJTCCX.intUserID = intUserID;
            childFormGJTCCX.intUserLimit = intUserLimit;
            childFormGJTCCX.strUserLimit = strUserLimit;
            childFormGJTCCX.strUserName = strUserName;
            childFormGJTCCX.intUserBM = intUserBM;

            childFormGJTCCX.Show();
        }

        private void 购进付款查询DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormGJFKCX childFormGJFKCX = new FormGJFKCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormGJFKCX.MdiParent = this;

            childFormGJFKCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormGJFKCX.printToolStripButton.Visible = false;
                childFormGJFKCX.printPreviewToolStripButton.Visible = false;
            }
            childFormGJFKCX.intUserID = intUserID;
            childFormGJFKCX.intUserLimit = intUserLimit;
            childFormGJFKCX.strUserLimit = strUserLimit;
            childFormGJFKCX.strUserName = strUserName;
            childFormGJFKCX.intUserBM = intUserBM;

            childFormGJFKCX.Show();
        }

        public void 单位购进查询EToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormDWGJCX childFormDWGJCX = new FormDWGJCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormDWGJCX.MdiParent = this;

            childFormDWGJCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormDWGJCX.printToolStripButton.Visible = false;
                childFormDWGJCX.printPreviewToolStripButton.Visible = false;
            }

            childFormDWGJCX.intUserID = intUserID;
            childFormDWGJCX.intUserLimit = intUserLimit;
            childFormDWGJCX.strUserLimit = strUserLimit;
            childFormDWGJCX.strUserName = strUserName;
            childFormDWGJCX.intUserBM = intUserBM;

            childFormDWGJCX.Show();
        }

        public void 购进赠品查询FToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormGJZPCX childFormGJZPCX = new FormGJZPCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormGJZPCX.MdiParent = this;

            childFormGJZPCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormGJZPCX.printToolStripButton.Visible = false;
                childFormGJZPCX.printPreviewToolStripButton.Visible = false;
            }

            childFormGJZPCX.intUserID = intUserID;
            childFormGJZPCX.intUserLimit = intUserLimit;
            childFormGJZPCX.strUserLimit = strUserLimit;
            childFormGJZPCX.strUserName = strUserName;
            childFormGJZPCX.intUserBM = intUserBM;

            childFormGJZPCX.Show();
        }

        private void 进货分类查询GToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormJHFLCX childFormJHFLCXX = new FormJHFLCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormJHFLCXX.MdiParent = this;

            childFormJHFLCXX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormJHFLCXX.printToolStripButton.Visible = false;
                childFormJHFLCXX.printPreviewToolStripButton.Visible = false;
            }

            childFormJHFLCXX.intUserID = intUserID;
            childFormJHFLCXX.intUserLimit = intUserLimit;
            childFormJHFLCXX.strUserLimit = strUserLimit;
            childFormJHFLCXX.strUserName = strUserName;
            childFormJHFLCXX.intUserBM = intUserBM;

            childFormJHFLCXX.Show();
        }

        private void 销售业务查询AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXSYWCX childFormXSYWCX = new FormXSYWCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXSYWCX.MdiParent = this;

            childFormXSYWCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXSYWCX.printToolStripButton.Visible = false;
                childFormXSYWCX.printPreviewToolStripButton.Visible = false;
            }

            childFormXSYWCX.intUserID = intUserID;
            childFormXSYWCX.intUserLimit = intUserLimit;
            childFormXSYWCX.strUserLimit = strUserLimit;
            childFormXSYWCX.strUserName = strUserName;
            childFormXSYWCX.intUserBM = intUserBM;

            childFormXSYWCX.Show();
        }

        public void 销售退补查询BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXSTBJCX childFormXSTBJCX = new FormXSTBJCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXSTBJCX.MdiParent = this;

            childFormXSTBJCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXSTBJCX.printToolStripButton.Visible = false;
                childFormXSTBJCX.printPreviewToolStripButton.Visible = false;
            }

            childFormXSTBJCX.intUserID = intUserID;
            childFormXSTBJCX.intUserLimit = intUserLimit;
            childFormXSTBJCX.strUserLimit = strUserLimit;
            childFormXSTBJCX.strUserName = strUserName;
            childFormXSTBJCX.intUserBM = intUserBM;

            childFormXSTBJCX.Show();
        }

        public void 销售退回查询CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXSTHCX childFormXSTHCX = new FormXSTHCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXSTHCX.MdiParent = this;

            childFormXSTHCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXSTHCX.printToolStripButton.Visible = false;
                childFormXSTHCX.printPreviewToolStripButton.Visible = false;
            }

            childFormXSTHCX.intUserID = intUserID;
            childFormXSTHCX.intUserLimit = intUserLimit;
            childFormXSTHCX.strUserLimit = strUserLimit;
            childFormXSTHCX.strUserName = strUserName;
            childFormXSTHCX.intUserBM = intUserBM;

            childFormXSTHCX.Show();
        }

        public void 销售收款查询DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXSSKCX childFormXSSKCX = new FormXSSKCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXSSKCX.MdiParent = this;

            childFormXSSKCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXSSKCX.printToolStripButton.Visible = false;
                childFormXSSKCX.printPreviewToolStripButton.Visible = false;
            }

            childFormXSSKCX.intUserID = intUserID;
            childFormXSSKCX.intUserLimit = intUserLimit;
            childFormXSSKCX.strUserLimit = strUserLimit;
            childFormXSSKCX.strUserName = strUserName;
            childFormXSSKCX.intUserBM = intUserBM;

            childFormXSSKCX.Show();
        }

        public void 销售赠品查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXSZPCX childFormXSZPCX = new FormXSZPCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXSZPCX.MdiParent = this;

            childFormXSZPCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXSZPCX.printToolStripButton.Visible = false;
                childFormXSZPCX.printPreviewToolStripButton.Visible = false;
            }

            childFormXSZPCX.intUserID = intUserID;
            childFormXSZPCX.intUserLimit = intUserLimit;
            childFormXSZPCX.strUserLimit = strUserLimit;
            childFormXSZPCX.strUserName = strUserName;
            childFormXSZPCX.intUserBM = intUserBM;

            childFormXSZPCX.Show();
        }

        public void 销售分类查询FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXSFLCX childFormXSFLCX = new FormXSFLCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXSFLCX.MdiParent = this;

            childFormXSFLCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXSFLCX.printToolStripButton.Visible = false;
                childFormXSFLCX.printPreviewToolStripButton.Visible = false;
            }

            childFormXSFLCX.intUserID = intUserID;
            childFormXSFLCX.intUserLimit = intUserLimit;
            childFormXSFLCX.strUserLimit = strUserLimit;
            childFormXSFLCX.strUserName = strUserName;
            childFormXSFLCX.intUserBM = intUserBM;

            childFormXSFLCX.Show();
        }

        public void 库存所有商品查询BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKCSPCX childFormKCSPCX = new FormKCSPCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKCSPCX.MdiParent = this;

            childFormKCSPCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKCSPCX.printToolStripButton.Visible = false;
                childFormKCSPCX.printPreviewToolStripButton.Visible = false;
            }

            childFormKCSPCX.intUserID = intUserID;
            childFormKCSPCX.intUserLimit = intUserLimit;
            childFormKCSPCX.strUserLimit = strUserLimit;
            childFormKCSPCX.strUserName = strUserName;
            childFormKCSPCX.intUserBM = intUserBM;

            childFormKCSPCX.Show();
        }

        public void 库存查询CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKCCX childFormKCCX = new FormKCCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKCCX.MdiParent = this;

            childFormKCCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKCCX.printToolStripButton.Visible = false;
                childFormKCCX.printPreviewToolStripButton.Visible = false;
            }

            childFormKCCX.intUserID = intUserID;
            childFormKCCX.intUserLimit = intUserLimit;
            childFormKCCX.strUserLimit = strUserLimit;
            childFormKCCX.strUserName = strUserName;
            childFormKCCX.intUserBM = intUserBM;

            childFormKCCX.Show();
        }

        public void 当期商品出入库CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormDQSPCRKHZCX childFormDQSPCRKHZCX = new FormDQSPCRKHZCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormDQSPCRKHZCX.MdiParent = this;

            childFormDQSPCRKHZCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormDQSPCRKHZCX.printToolStripButton.Visible = false;
                childFormDQSPCRKHZCX.printPreviewToolStripButton.Visible = false;
            }


            childFormDQSPCRKHZCX.intUserID = intUserID;
            childFormDQSPCRKHZCX.intUserLimit = intUserLimit;
            childFormDQSPCRKHZCX.strUserLimit = strUserLimit;
            childFormDQSPCRKHZCX.strUserName = strUserName;
            childFormDQSPCRKHZCX.intUserBM = intUserBM;

            childFormDQSPCRKHZCX.Show();
        }

        public void 商品综合查询DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPZHCX childFormSPZHCX = new FormSPZHCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPZHCX.MdiParent = this;

            childFormSPZHCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPZHCX.printToolStripButton.Visible = false;
                childFormSPZHCX.printPreviewToolStripButton.Visible = false;
            }

            childFormSPZHCX.intUserID = intUserID;
            childFormSPZHCX.intUserLimit = intUserLimit;
            childFormSPZHCX.strUserLimit = strUserLimit;
            childFormSPZHCX.strUserName = strUserName;
            childFormSPZHCX.intUserBM = intUserBM;

            childFormSPZHCX.Show();
        }

        public void 商品盘点历史查询EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPPDLSJL childFormSPPDLSJL = new FormSPPDLSJL();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPPDLSJL.MdiParent = this;

            childFormSPPDLSJL.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPPDLSJL.printToolStripButton.Visible = false;
                childFormSPPDLSJL.printPreviewToolStripButton.Visible = false;
            }


            childFormSPPDLSJL.intUserID = intUserID;
            childFormSPPDLSJL.intUserLimit = intUserLimit;
            childFormSPPDLSJL.strUserLimit = strUserLimit;
            childFormSPPDLSJL.strUserName = strUserName;
            childFormSPPDLSJL.intUserBM = intUserBM;

            childFormSPPDLSJL.Show();
        }

        private void 商品报损历史查询FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPBSLSJL childFormSPBSLSJL = new FormSPBSLSJL();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPBSLSJL.MdiParent = this;

            childFormSPBSLSJL.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPBSLSJL.printToolStripButton.Visible = false;
                childFormSPBSLSJL.printPreviewToolStripButton.Visible = false;
            }

            childFormSPBSLSJL.intUserID = intUserID;
            childFormSPBSLSJL.intUserLimit = intUserLimit;
            childFormSPBSLSJL.strUserLimit = strUserLimit;
            childFormSPBSLSJL.strUserName = strUserName;
            childFormSPBSLSJL.intUserBM = intUserBM;

            childFormSPBSLSJL.Show();
        }

        public void 商品拆装历史查询HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPZZCX childFormSPZZCX = new FormSPZZCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPZZCX.MdiParent = this;

            childFormSPZZCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPZZCX.printToolStripButton.Visible = false;
                childFormSPZZCX.printPreviewToolStripButton.Visible = false;
            }

            childFormSPZZCX.intUserID = intUserID;
            childFormSPZZCX.intUserLimit = intUserLimit;
            childFormSPZZCX.strUserLimit = strUserLimit;
            childFormSPZZCX.strUserName = strUserName;
            childFormSPZZCX.intUserBM = intUserBM;

            childFormSPZZCX.Show();
        }

        public void 商品调价查询AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPTJCX childFormSPTJCX = new FormSPTJCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPTJCX.MdiParent = this;

            childFormSPTJCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPTJCX.printToolStripButton.Visible = false;
                childFormSPTJCX.printPreviewToolStripButton.Visible = false;
            }
            childFormSPTJCX.intUserID = intUserID;
            childFormSPTJCX.intUserLimit = intUserLimit;
            childFormSPTJCX.strUserLimit = strUserLimit;
            childFormSPTJCX.strUserName = strUserName;
            childFormSPTJCX.intUserBM = intUserBM;

            childFormSPTJCX.Show();
        }

        public void 商品资料查询BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPZLCX childFormSPZLCX = new FormSPZLCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPZLCX.MdiParent = this;

            childFormSPZLCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPZLCX.printToolStripButton.Visible = false;
                childFormSPZLCX.printPreviewToolStripButton.Visible = false;
            }
            childFormSPZLCX.intUserID = intUserID;
            childFormSPZLCX.intUserLimit = intUserLimit;
            childFormSPZLCX.strUserLimit = strUserLimit;
            childFormSPZLCX.strUserName = strUserName;
            childFormSPZLCX.intUserBM = intUserBM;

            childFormSPZLCX.Show();

        }

        public void 借物出库查询AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormJWCKCX childFormJWCKCX = new FormJWCKCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormJWCKCX.MdiParent = this;

            childFormJWCKCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormJWCKCX.printToolStripButton.Visible = false;
                childFormJWCKCX.printPreviewToolStripButton.Visible = false;
            }
            childFormJWCKCX.intUserID = intUserID;
            childFormJWCKCX.intUserLimit = intUserLimit;
            childFormJWCKCX.strUserLimit = strUserLimit;
            childFormJWCKCX.strUserName = strUserName;
            childFormJWCKCX.intUserBM = intUserBM;

            childFormJWCKCX.Show();

        }

        public void 借物查询BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormJWCX childFormJWCX = new FormJWCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormJWCX.MdiParent = this;

            childFormJWCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormJWCX.printToolStripButton.Visible = false;
                childFormJWCX.printPreviewToolStripButton.Visible = false;
            }
            childFormJWCX.intUserID = intUserID;
            childFormJWCX.intUserLimit = intUserLimit;
            childFormJWCX.strUserLimit = strUserLimit;
            childFormJWCX.strUserName = strUserName;
            childFormJWCX.intUserBM = intUserBM;

            childFormJWCX.Show();

        }

        public void 结转单据查询HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormJZSJCX childFormJZSJCX = new FormJZSJCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormJZSJCX.MdiParent = this;

            childFormJZSJCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormJZSJCX.printToolStripButton.Visible = false;
                childFormJZSJCX.printPreviewToolStripButton.Visible = false;
            }
            childFormJZSJCX.intUserID = intUserID;
            childFormJZSJCX.intUserLimit = intUserLimit;
            childFormJZSJCX.strUserLimit = strUserLimit;
            childFormJZSJCX.strUserName = strUserName;
            childFormJZSJCX.intUserBM = intUserBM;

            childFormJZSJCX.Show();

        }

        public void 供销单位查询AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormDWZLCX childFormDWZLCX = new FormDWZLCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormDWZLCX.MdiParent = this;

            childFormDWZLCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormDWZLCX.printToolStripButton.Visible = false;
                childFormDWZLCX.printPreviewToolStripButton.Visible = false;
            }

            childFormDWZLCX.intUserID = intUserID;
            childFormDWZLCX.intUserLimit = intUserLimit;
            childFormDWZLCX.strUserLimit = strUserLimit;
            childFormDWZLCX.strUserName = strUserName;
            childFormDWZLCX.intUserBM = intUserBM;

            childFormDWZLCX.Show();

        }

        private void 库房信息查询BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKFZLCX childFormKFZLCX = new FormKFZLCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKFZLCX.MdiParent = this;

            childFormKFZLCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKFZLCX.printToolStripButton.Visible = false;
                childFormKFZLCX.printPreviewToolStripButton.Visible = false;
            }
            childFormKFZLCX.intUserID = intUserID;
            childFormKFZLCX.intUserLimit = intUserLimit;
            childFormKFZLCX.strUserLimit = strUserLimit;
            childFormKFZLCX.strUserName = strUserName;
            childFormKFZLCX.intUserBM = intUserBM;

            childFormKFZLCX.Show();

        }

        public void 部门资料查询CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormBMZLCX childFormBMZLCX = new FormBMZLCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormBMZLCX.MdiParent = this;

            childFormBMZLCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormBMZLCX.printToolStripButton.Visible = false;
                childFormBMZLCX.printPreviewToolStripButton.Visible = false;
            }
            childFormBMZLCX.intUserID = intUserID;
            childFormBMZLCX.intUserLimit = intUserLimit;
            childFormBMZLCX.strUserLimit = strUserLimit;
            childFormBMZLCX.strUserName = strUserName;
            childFormBMZLCX.intUserBM = intUserBM;

            childFormBMZLCX.Show();

        }

        public void 单位档案维护AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormDWDAWH childFormDWDAWH = new FormDWDAWH();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormDWDAWH.MdiParent = this;

            childFormDWDAWH.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormDWDAWH.printToolStripButton.Visible = false;
                childFormDWDAWH.printPreviewToolStripButton.Visible = false;
            }
            childFormDWDAWH.intUserID = intUserID;
            childFormDWDAWH.intUserLimit = intUserLimit;
            childFormDWDAWH.strUserLimit = strUserLimit;
            childFormDWDAWH.strUserName = strUserName;
            childFormDWDAWH.intUserBM = intUserBM;

            childFormDWDAWH.Show();
        }

        public void 往来单位余额登记BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormWLDWYEDJ childFormWLDWYEDJ = new FormWLDWYEDJ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormWLDWYEDJ.MdiParent = this;

            childFormWLDWYEDJ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormWLDWYEDJ.printToolStripButton.Visible = false;
                childFormWLDWYEDJ.printPreviewToolStripButton.Visible = false;
            }
            childFormWLDWYEDJ.intUserID = intUserID;
            childFormWLDWYEDJ.intUserLimit = intUserLimit;
            childFormWLDWYEDJ.strUserLimit = strUserLimit;
            childFormWLDWYEDJ.strUserName = strUserName;
            childFormWLDWYEDJ.intUserBM = intUserBM;

            childFormWLDWYEDJ.Show();
        }

        public void 账簿档案维护CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormZBDAWH childFormZBDAWH = new FormZBDAWH();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormZBDAWH.MdiParent = this;

            childFormZBDAWH.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormZBDAWH.printToolStripButton.Visible = false;
                childFormZBDAWH.printPreviewToolStripButton.Visible = false;
            }
            childFormZBDAWH.intUserID = intUserID;
            childFormZBDAWH.intUserLimit = intUserLimit;
            childFormZBDAWH.strUserLimit = strUserLimit;
            childFormZBDAWH.strUserName = strUserName;
            childFormZBDAWH.intUserBM = intUserBM;

            childFormZBDAWH.Show();
        }

        public void 库房档案维护DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKFDAWH childFormKFDAWH = new FormKFDAWH();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKFDAWH.MdiParent = this;

            childFormKFDAWH.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKFDAWH.printToolStripButton.Visible = false;
                childFormKFDAWH.printPreviewToolStripButton.Visible = false;
            }
            childFormKFDAWH.intUserID = intUserID;
            childFormKFDAWH.intUserLimit = intUserLimit;
            childFormKFDAWH.strUserLimit = strUserLimit;
            childFormKFDAWH.strUserName = strUserName;
            childFormKFDAWH.intUserBM = intUserBM;

            childFormKFDAWH.Show();

        }

        public void 部门档案维护EToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FormBMDAWH frmBMDAWH = new FormBMDAWH();
            frmBMDAWH.strConn = strConn;

            frmBMDAWH.ShowDialog();

        }

        public void 职员档案维护GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (intUserLimit < 5)
            {
                MessageBox.Show("您的权限不足");
                return;
            }
            FormZYDAWHCARD frmZYDAWH = new FormZYDAWHCARD();
            frmZYDAWH.strConn = strConn;

            frmZYDAWH.ShowDialog();
        }

        public void 商品分类维护AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPFLWH childFormSPFLWH = new FormSPFLWH();


            childFormSPFLWH.strConn = strConn;

            childFormSPFLWH.intUserID = intUserID;
            childFormSPFLWH.intUserLimit = intUserLimit;
            childFormSPFLWH.strUserLimit = strUserLimit;
            childFormSPFLWH.strUserName = strUserName;
            childFormSPFLWH.intUserBM = intUserBM;

            childFormSPFLWH.ShowDialog();
        }

        private void 商品资料维护BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSPDAWH childFormSPDAWH = new FormSPDAWH();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPDAWH.MdiParent = this;

            childFormSPDAWH.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSPDAWH.printToolStripButton.Visible = false;
                childFormSPDAWH.printPreviewToolStripButton.Visible = false;
            }
            childFormSPDAWH.intUserID = intUserID;
            childFormSPDAWH.intUserLimit = intUserLimit;
            childFormSPDAWH.strUserLimit = strUserLimit;
            childFormSPDAWH.strUserName = strUserName;
            childFormSPDAWH.intUserBM = intUserBM;

            childFormSPDAWH.iVersion = iVersion;
            childFormSPDAWH.Show();
        }

        private void 库房下发产品CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXFSPZL childFormXFSPZL = new FormXFSPZL();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXFSPZL.MdiParent = this;

            childFormXFSPZL.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXFSPZL.printToolStripButton.Visible = false;
                childFormXFSPZL.printPreviewToolStripButton.Visible = false;
            }
            childFormXFSPZL.intUserID = intUserID;
            childFormXFSPZL.intUserLimit = intUserLimit;
            childFormXFSPZL.strUserLimit = strUserLimit;
            childFormXFSPZL.strUserName = strUserName;
            childFormXFSPZL.intUserBM = intUserBM;

            childFormXFSPZL.Show();
        }

        public void 数据备份AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDataBaseBackUP frmDataBaseBackUP = new FormDataBaseBackUP();


            frmDataBaseBackUP.strConn = strConn;

            frmDataBaseBackUP.strDataBaseAddr = strDataBaseAddr;
            frmDataBaseBackUP.strDataBaseName = strDataBaseName;
            frmDataBaseBackUP.strDataBasePass = strDataBasePass;
            frmDataBaseBackUP.strDataBaseUser = strDataBaseUser;

            frmDataBaseBackUP.intUserID = intUserID;
            frmDataBaseBackUP.intUserLimit = intUserLimit;
            frmDataBaseBackUP.strUserLimit = strUserLimit;
            frmDataBaseBackUP.strUserName = strUserName;
            frmDataBaseBackUP.intUserBM = intUserBM;

            frmDataBaseBackUP.ShowDialog();
        }

        public void 数据恢复BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDataBaseReStore frmDataBaseReStore = new FormDataBaseReStore();

            frmDataBaseReStore.strConn = strConn;

            frmDataBaseReStore.strDataBaseAddr = strDataBaseAddr;
            frmDataBaseReStore.strDataBaseName = strDataBaseName;
            frmDataBaseReStore.strDataBasePass = strDataBasePass;
            frmDataBaseReStore.strDataBaseUser = strDataBaseUser;

            frmDataBaseReStore.intUserID = intUserID;
            frmDataBaseReStore.intUserLimit = intUserLimit;
            frmDataBaseReStore.strUserLimit = strUserLimit;
            frmDataBaseReStore.strUserName = strUserName;
            frmDataBaseReStore.intUserBM = intUserBM;

            frmDataBaseReStore.ShowDialog();
        }

        public void 单据日志查询BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormRZCX childFormRZCX = new FormRZCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormRZCX.MdiParent = this;

            childFormRZCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormRZCX.printToolStripButton.Visible = false;
                childFormRZCX.printPreviewToolStripButton.Visible = false;
            }
            childFormRZCX.intUserID = intUserID;
            childFormRZCX.intUserLimit = intUserLimit;
            childFormRZCX.strUserLimit = strUserLimit;
            childFormRZCX.strUserName = strUserName;
            childFormRZCX.intUserBM = intUserBM;

            childFormRZCX.Show();
        }

        public void helpToolStripButton_Click(object sender, EventArgs e)
        {
            indexToolStripMenuItem_Click(null, null);
        }

        private void toolStripButtonA_Click(object sender, EventArgs e)
        {
            购进ToolStripMenuItem_Click(null, null);
        }

        private void toolStripButtonB_Click(object sender, EventArgs e)
        {
            销售出库制单BToolStripMenuItem_Click(null, null);
        }

        private void toolStripButtonC_Click(object sender, EventArgs e)
        {
            准备盘点表AToolStripMenuItem_Click(null,null);
        }

        private void toolStripButtonD_Click(object sender, EventArgs e)
        {
            商品单品分析AToolStripMenuItem_Click(null, null);
        }

        private void indexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "business.chm");   
        }

        private void 入库商品条码管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormTMRKGL childFormTMRKGL = new FormTMRKGL();
            childFormTMRKGL.MdiParent = this;
            childFormTMRKGL.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormTMRKGL.printToolStripButton.Visible = false;
                childFormTMRKGL.printPreviewToolStripButton.Visible = false;
            }
            childFormTMRKGL.intUserID = intUserID;
            childFormTMRKGL.intUserLimit = intUserLimit;
            childFormTMRKGL.strUserLimit = strUserLimit;
            childFormTMRKGL.strUserName = strUserName;
            childFormTMRKGL.intUserBM = intUserBM;

            childFormTMRKGL.Show();
        }

        public void 出库商品条码管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            
            FormTMCKGL childFormTMCKGL = new FormTMCKGL();
            childFormTMCKGL.MdiParent = this;
            childFormTMCKGL.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormTMCKGL.printToolStripButton.Visible = false;
                childFormTMCKGL.printPreviewToolStripButton.Visible = false;
            }
            childFormTMCKGL.intUserID = intUserID;
            childFormTMCKGL.intUserLimit = intUserLimit;
            childFormTMCKGL.strUserLimit = strUserLimit;
            childFormTMCKGL.strUserName = strUserName;
            childFormTMCKGL.intUserBM = intUserBM;

            childFormTMCKGL.Show();
        }

        public void 条码商品查询CToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FormTMCX childFormTMCX = new FormTMCX();
            childFormTMCX.MdiParent = this;
            childFormTMCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormTMCX.printToolStripButton.Visible = false;
                childFormTMCX.printPreviewToolStripButton.Visible = false;
            }
            childFormTMCX.intUserID = intUserID;
            childFormTMCX.intUserLimit = intUserLimit;
            childFormTMCX.strUserLimit = strUserLimit;
            childFormTMCX.strUserName = strUserName;
            childFormTMCX.intUserBM = intUserBM;

            childFormTMCX.Show();
        }

        public void 数据库设置CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formDatabaseSet frmDatabaseSet = new formDatabaseSet();
            frmDatabaseSet.intMode = 0;//测试模式

            if (frmDatabaseSet.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                strConn = frmDatabaseSet.strConn;
                if (strConn == "") //连接错误
                    return;

                //初始化窗口
                sqlConn.ConnectionString = strConn;
                sqlComm.Connection = sqlConn;
                sqlDA.SelectCommand = sqlComm;


            }
        }

        public void 退出系统XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void 更改密码DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormZYChangePass frmZYChangePass = new FormZYChangePass();
            frmZYChangePass.strConn = strConn;
            frmZYChangePass.iZYID= intUserID;
            frmZYChangePass.strZYName= strUserName;

            frmZYChangePass.ShowDialog();
        }

        public void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "bussiness.chm");   
        }

        public void 系统参数设置SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSystemSet frmSystemSet = new FormSystemSet();
            frmSystemSet.strConn = strConn;
            frmSystemSet.intUserID= intUserID;
            frmSystemSet.strUserName= strUserName;

            frmSystemSet.ShowDialog();

            sqlComm.CommandText = "SELECT 公司名 FROM 系统参数表";
            sqlConn.Open();
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                this.Text = "商业管理（进销存）系统：" + sqldr.GetValue(0).ToString();
                sqldr.Close();
                if (iVersion == 0)
                    this.Text += " - 预览版";
            }
            sqlConn.Close();
        }

        private void toolStripButtonR_Click(object sender, EventArgs e)
        {
            单据再现AToolStripMenuItem_Click(null, null);
        }

        public void 用户注销LToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }

            formLogin frmLogin = new formLogin();
            frmLogin.strConn = strConn;
            frmLogin.ShowDialog();

            if (frmLogin.intUserID != 0) //登录
            {
                intUserID = frmLogin.intUserID;
                intUserLimit = frmLogin.intUserLimit;
                strUserLimit = frmLogin.strUserLimit;
                strUserName = frmLogin.strUserName;
                intUserBM = frmLogin.intUserBM;
            }
            else //取消
            {
                //this.Close();
                return;
            }
            initStatusBar();
        }


        private void MDIBusiness_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                sqlConn.Open();
                sqlComm.CommandText = "UPDATE 职员表 SET 登录状态 = NULL WHERE (ID = " + intUserID.ToString() + ")";
                sqlComm.ExecuteNonQuery();
                sqlConn.Close();

                cSenseLock.freeSenseLock();
            }
            catch
            {
            }
            SkinClass.SveSkins();
        }

        private void 地区档案维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FormDQDAWH frmDQDAWH = new FormDQDAWH();
            frmDQDAWH.strConn = strConn;

            frmDQDAWH.ShowDialog();
        }

        private void 职位档案维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormZWDAWH frmZWDAWH = new FormZWDAWH();
            frmZWDAWH.strConn = strConn;

            frmZWDAWH.ShowDialog();
        }

        private void 数据整理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("删除所有财务数据,该过程不可恢复,是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            sqlConn.Open();
            try
            {
                sqlComm.CommandText = "UPDATE 单位表 SET 应付账款 = 0, 应收账款 = 0";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 商品表 SET 应付金额 = 0, 已付金额 = 0, 应收金额 = 0, 已收金额 = 0";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 库存表 SET 应付金额 = 0, 已付金额 = 0, 应收金额 = 0, 已收金额 = 0";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算付款勾兑表";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算付款明细表";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算付款汇总表";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算收款勾兑表";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算收款汇总表";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 购进退补差价明细表 SET 未付款金额 = 0, 已付款金额 = 金额, 未付款数量 = 0, 已付款数量 = 补价数量";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 购进退补差价汇总表 SET 付款标记 = 1, 未付款金额 = 0, 已付款金额 = 价税合计, 结清时间 = '2010-5-31'";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 进货入库明细表 SET 未付款金额 = 0, 已付款金额 = 实计金额, 未付款数量 = 0, 已付款数量 = 数量 ";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 进货入库汇总表 SET 未付款金额 = 0, 已付款金额 = 价税合计, 付款标记 = 1, 结清时间 = '2010-5-31'";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 进货退出明细表 SET 未付款金额 = 0, 已付款金额 = 实计金额, 未付款数量 = 0, 已付款数量 = 数量";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 进货退出汇总表 SET 未付款金额 = 0, 已付款金额 = 价税合计, 付款标记 = 1, 结清时间 = '2010-5-1'";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 销售出库明细表 SET 未付款金额 = 0, 已付款金额 = 实计金额, 未付款数量 = 0, 已付款数量 = 数量";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 销售出库汇总表 SET 未付款金额 = 0, 已付款金额 = 价税合计, 付款标记 = 1, 结清时间 = '2010-5-31'";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 销售商品制单明细表 SET 未付款金额 = 0, 已付款金额 = 实计金额, 未付款数量 = 0, 已付款数量 = 数量";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 销售商品制单表 SET 付款标记 = 1, 未付款金额 = 0, 已付款金额 = 价税合计, 结清时间 = '2010-5-31'";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 销售退出明细表 SET 未付款金额 = 0, 已付款金额 = 实计金额, 未付款数量 = 0, 已付款数量 = 数量";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 销售退出汇总表 SET 未付款金额 = 0, 已付款金额 = 价税合计, 结清时间 = '2010-5-31', 付款标记 = 1";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 销售退补差价明细表 SET 未付款金额 = 0, 已付款金额 = 金额, 未付款数量 = 0, 已付款数量 = 补价数量";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 销售退补差价汇总表 SET 付款标记 = 1, 未付款金额 = 0, 已付款金额 = 价税合计, 结清时间 = '2010-5-31'";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 发票明细表";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 发票汇总表";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 进货入库汇总表 SET 发票号=N'ＤＥＬ'";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 销售商品制单表 SET 发票号=N'ＤＥＬ'";
                sqlComm.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                sqlConn.Close();
            }
            MessageBox.Show("所有财务数据已经删除");

        }

        private void 初始化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlDataAdapter sqlDA1 = new System.Data.SqlClient.SqlDataAdapter();
            System.Data.DataSet dSet1 = new DataSet();
            sqlDA1.SelectCommand = sqlComm;

            int i;
            string sTemp;

            sqlConn.Open();
            try
            {
                sqlComm.CommandText = "UPDATE 进货入库汇总表 SET 发票号 = NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 购进商品制单表 SET 发票号 = NULL";
                sqlComm.ExecuteNonQuery();


                sqlComm.CommandText = "SELECT 发票汇总表.ID, 发票汇总表.发票号, 发票汇总表.发票类型, 发票明细表.冲抵ID, 发票明细表.单据ID, 发票明细表.单据编号, 发票明细表.冲抵编号 FROM 发票汇总表 INNER JOIN 发票明细表 ON 发票汇总表.ID = 发票明细表.发票ID WHERE (发票汇总表.BeActive = 1) AND (发票明细表.单据编号 LIKE N'ADH%') AND (发票汇总表.发票类型 = 0)";


                if (dSet1.Tables.Contains("发票表")) dSet.Tables.Remove("发票表");
                sqlDA1.Fill(dSet1, "发票表");

                for (i = 0; i < dSet1.Tables["发票表"].Rows.Count; i++)
                {
                    sTemp = dSet1.Tables["发票表"].Rows[i][5].ToString().Substring(0, 3);

                    if (sTemp == "ADH")
                    {
                        sqlComm.CommandText = "UPDATE 进货入库汇总表 SET 发票号 = N'" + dSet1.Tables["发票表"].Rows[i][1].ToString() + "' WHERE (单据编号 = N'" + dSet1.Tables["发票表"].Rows[i][5].ToString() + "')";
                        sqlComm.ExecuteNonQuery();

                        sqlComm.CommandText = "UPDATE 购进商品制单表 SET 发票号 = N'" + dSet1.Tables["发票表"].Rows[i][1].ToString() + "' WHERE (单据编号 = N'" + dSet1.Tables["发票表"].Rows[i][6].ToString() + "')";
                        sqlComm.ExecuteNonQuery();

                    }
                }

                /*
                sqlComm.CommandText = "alter table 销售退出汇总表 alter column 发票号 varchar(200)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "alter table 销售退出汇总表 alter column 支票号 varchar(200)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 销售退补差价汇总表 alter column 发票号 varchar(200)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "alter table 销售退补差价汇总表 alter column 支票号 varchar(200)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 销售出库汇总表 alter column 发票号 varchar(200)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "alter table 销售出库汇总表 alter column 支票号 varchar(200)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 进货退出汇总表 alter column 发票号 varchar(200)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "alter table 进货退出汇总表 alter column 支票号 varchar(200)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 进货入库汇总表 alter column 发票号 varchar(200)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "alter table 进货入库汇总表 alter column 支票号 varchar(200)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 购进退补差价汇总表 alter column 发票号 varchar(200)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "alter table 购进退补差价汇总表 alter column 支票号 varchar(200)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 购进商品制单表 alter column 发票号 varchar(200)";
                sqlComm.ExecuteNonQuery();
                 * */

                /*
                sqlComm.CommandText = "alter table 采购合同表 add [冲红时间] [smalldatetime] NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 购进商品制单表 add [冲红时间] [smalldatetime] NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 购进退补差价汇总表 add [冲红时间] [smalldatetime] NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 结算付款汇总表 add [冲红时间] [smalldatetime] NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 结算收款汇总表 add [冲红时间] [smalldatetime] NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 借物出库汇总表 add [冲红时间] [smalldatetime] NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 进货入库汇总表 add [冲红时间] [smalldatetime] NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 进货退出汇总表 add [冲红时间] [smalldatetime] NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 销售出库汇总表 add [冲红时间] [smalldatetime] NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 销售合同表 add [冲红时间] [smalldatetime] NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 销售商品制单表 add [冲红时间] [smalldatetime] NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 销售退补差价汇总表 add [冲红时间] [smalldatetime] NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 销售退出汇总表 add [冲红时间] [smalldatetime] NULL";
                sqlComm.ExecuteNonQuery();
                /*
                sqlComm.CommandText = "DELETE FROM 库存表 WHERE (商品ID = 1) AND (库房ID <> 1)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结转往来汇总表 WHERE (单位ID = 0)";
                sqlComm.ExecuteNonQuery();
                 */
                /*
                sqlComm.CommandText = "alter table 库存盘点明细表 add [库房ID] [int] NULL";
                sqlComm.ExecuteNonQuery();

;
                sqlComm.CommandText = "UPDATE 库存盘点明细表 SET 库房ID =(SELECT 库存盘点汇总表.库房ID FROM 库存盘点汇总表 INNER JOIN 库存盘点明细表 AS 库存盘点明细表_1 ON 库存盘点汇总表.ID = 库存盘点明细表_1.单据ID WHERE (库存盘点明细表.ID = 库存盘点明细表_1.ID))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 进货入库明细表 add [原单据明细ID] [int] NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table  销售出库明细表 add [原单据明细ID] [int] NULL";
                sqlComm.ExecuteNonQuery();
                */


                /*
                //sqlComm.CommandText = "alter table 库存报损明细表 add [原库存数量] [decimal](18, 0) NULL ";
                sqlComm.CommandText = "alter table 单位表 alter column 传真 varchar(50)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "alter table 单位表  add [开票电话] varchar(50) NULL";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 商品表 SET 库存金额 = 库存数量 * 库存成本价";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 库存表 SET 库存金额 = 库存数量 * 库存成本价";
                sqlComm.ExecuteNonQuery();
                */

            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                sqlConn.Close();
            }
            MessageBox.Show("数据库更新完毕");
        }

        private void 合同条码查询DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTMHTCX childFormTMHTCX = new FormTMHTCX();
            childFormTMHTCX.MdiParent = this;
            childFormTMHTCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormTMHTCX.printToolStripButton.Visible = false;
                childFormTMHTCX.printPreviewToolStripButton.Visible = false;
            }
            childFormTMHTCX.intUserID = intUserID;
            childFormTMHTCX.intUserLimit = intUserLimit;
            childFormTMHTCX.strUserLimit = strUserLimit;
            childFormTMHTCX.strUserName = strUserName;
            childFormTMHTCX.intUserBM = intUserBM;

            childFormTMHTCX.Show();
        }

        private void 月平均销售ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormYJXSCX childFormYJXSCX = new FormYJXSCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormYJXSCX.MdiParent = this;

            childFormYJXSCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormYJXSCX.printToolStripButton.Visible = false;
                childFormYJXSCX.printPreviewToolStripButton.Visible = false;
            }
            childFormYJXSCX.intUserID = intUserID;
            childFormYJXSCX.intUserLimit = intUserLimit;
            childFormYJXSCX.strUserLimit = strUserLimit;
            childFormYJXSCX.strUserName = strUserName;
            childFormYJXSCX.intUserBM = intUserBM;

            childFormYJXSCX.Show();
        }

        private void 数据清理CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDataClear frmDataClear = new FormDataClear();
            frmDataClear.strConn = strConn;
            //frmDataClear.intUserID = intUserID;
            //frmDataClear.strUserName = strUserName;

            frmDataClear.ShowDialog();

        }

        private void 单据备注修改CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormCWDJCL childFormCWDJCL = new FormCWDJCL();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormCWDJCL.MdiParent = this;

            childFormCWDJCL.strConn = strConn;
            childFormCWDJCL.iStyle = 2;
            if (intUserLimit < iConstLimit)
            {
                childFormCWDJCL.printToolStripButton.Visible = false;
                childFormCWDJCL.printPreviewToolStripButton.Visible = false;
            }

            childFormCWDJCL.intUserID = intUserID;
            childFormCWDJCL.intUserLimit = intUserLimit;
            childFormCWDJCL.strUserLimit = strUserLimit;
            childFormCWDJCL.strUserName = strUserName;
            childFormCWDJCL.intUserBM = intUserBM;

            childFormCWDJCL.Show();
        }

        private void 销售购进单位往来帐ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormXSGJDWWLZ childFormXSGJDWWLZ = new FormXSGJDWWLZ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormXSGJDWWLZ.MdiParent = this;

            childFormXSGJDWWLZ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormXSGJDWWLZ.printToolStripButton.Visible = false;
                childFormXSGJDWWLZ.printPreviewToolStripButton.Visible = false;
            }
            childFormXSGJDWWLZ.intUserID = intUserID;
            childFormXSGJDWWLZ.intUserLimit = intUserLimit;
            childFormXSGJDWWLZ.strUserLimit = strUserLimit;
            childFormXSGJDWWLZ.strUserName = strUserName;
            childFormXSGJDWWLZ.intUserBM = intUserBM;

            childFormXSGJDWWLZ.Show();
        }

        private void 客户购销分析GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormKHGXFX childFormKHGXFX = new FormKHGXFX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormKHGXFX.MdiParent = this;

            childFormKHGXFX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormKHGXFX.printToolStripButton.Visible = false;
                childFormKHGXFX.printPreviewToolStripButton.Visible = false;
            }
            childFormKHGXFX.intUserID = intUserID;
            childFormKHGXFX.intUserLimit = intUserLimit;
            childFormKHGXFX.strUserLimit = strUserLimit;
            childFormKHGXFX.strUserName = strUserName;
            childFormKHGXFX.intUserBM = intUserBM;

            childFormKHGXFX.Show();
        }

        private void 删除单位单据DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormSCDWDJ childFormSCDWDJ = new FormSCDWDJ();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSCDWDJ.MdiParent = this;

            childFormSCDWDJ.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormSCDWDJ.printToolStripButton.Visible = false;
                childFormSCDWDJ.printPreviewToolStripButton.Visible = false;
            }
            childFormSCDWDJ.intUserID = intUserID;
            childFormSCDWDJ.intUserLimit = intUserLimit;
            childFormSCDWDJ.strUserLimit = strUserLimit;
            childFormSCDWDJ.strUserName = strUserName;
            childFormSCDWDJ.intUserBM = intUserBM;

            childFormSCDWDJ.Show();
        }

        private void 商品五日库存分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormWRKCFX childFormWRKCFX = new FormWRKCFX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormWRKCFX.MdiParent = this;

            childFormWRKCFX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormWRKCFX.printToolStripButton.Visible = false;
                childFormWRKCFX.printPreviewToolStripButton.Visible = false;
            }
            childFormWRKCFX.intUserID = intUserID;
            childFormWRKCFX.intUserLimit = intUserLimit;
            childFormWRKCFX.strUserLimit = strUserLimit;
            childFormWRKCFX.strUserName = strUserName;
            childFormWRKCFX.intUserBM = intUserBM;

            childFormWRKCFX.Show();
        }

        private void 商品五日销售分析DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormWRXSFX childFormWRXSFX = new FormWRXSFX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormWRXSFX.MdiParent = this;

            childFormWRXSFX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormWRXSFX.printToolStripButton.Visible = false;
                childFormWRXSFX.printPreviewToolStripButton.Visible = false;
            }
            childFormWRXSFX.intUserID = intUserID;
            childFormWRXSFX.intUserLimit = intUserLimit;
            childFormWRXSFX.strUserLimit = strUserLimit;
            childFormWRXSFX.strUserName = strUserName;
            childFormWRXSFX.intUserBM = intUserBM;

            childFormWRXSFX.Show();
        }

        private void 冲红单据查询EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建此子窗体的一个新实例。
            FormCHDJCX childFormCHDJCX = new FormCHDJCX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormCHDJCX.MdiParent = this;

            childFormCHDJCX.strConn = strConn;
            if (intUserLimit < iConstLimit)
            {
                childFormCHDJCX.printToolStripButton.Visible = false;
                childFormCHDJCX.printPreviewToolStripButton.Visible = false;
            }
            childFormCHDJCX.intUserID = intUserID;
            childFormCHDJCX.intUserLimit = intUserLimit;
            childFormCHDJCX.strUserLimit = strUserLimit;
            childFormCHDJCX.strUserName = strUserName;
            childFormCHDJCX.intUserBM = intUserBM;

            childFormCHDJCX.Show();
        }

        private void 数据清除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否删除所有单据？这个过程不可恢复。", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                return;
            }
            sqlConn.Open();

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;

            try
            {
                sqlComm.CommandText = "DELETE FROM 借物信息修改表";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "dbcc checkident(借物信息修改表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 借物出库明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(借物出库明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 借物出库汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(借物出库汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 单位历史账表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(单位历史账表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 发票明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(发票明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 发票汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(发票汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 商品历史账表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(商品历史账表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 商品库房历史账表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(商品库房历史账表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 商品条码表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(商品条码表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 库存报损明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(库存报损明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 库存报损汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(库存报损汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 库存盘点明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(库存盘点明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 库存盘点汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(库存盘点汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 日志表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(日志表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算付款明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(结算付款明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算付款汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(结算付款汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算付款勾兑表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(结算付款勾兑表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算收款明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(结算收款明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算收款汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(结算收款汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算收款勾兑表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(结算收款勾兑表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 调价通知单明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(调价通知单明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 调价通知单汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(调价通知单汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 购进商品制单明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(购进商品制单明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 购进商品制单表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(购进商品制单表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 购进退补差价明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(购进退补差价明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 购进退补差价汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(购进退补差价汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 进货入库明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(进货入库明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 进货入库汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(进货入库汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 进货退出明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(进货退出明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 进货退出汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(进货退出汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 送货信息修改表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(送货信息修改表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 采购合同明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(采购合同明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 采购合同表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(采购合同表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售出库明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(销售出库明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售出库汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(销售出库汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售合同明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(销售合同明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售合同表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(销售合同表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售商品制单明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(销售商品制单明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售商品制单表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(销售商品制单表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售退出明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(销售退出明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售退出汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(销售退出汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售退补差价明细表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(销售退补差价明细表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售退补差价汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(销售退补差价汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 备注修改记录表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(备注修改记录表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结转汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(结转汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结转进销存汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(结转进销存汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();


                sqlComm.CommandText = "DELETE FROM 结转库房汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(结转库房汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结转往来汇总表";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "dbcc checkident(结转往来汇总表,reseed,0)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 商品表 SET 库存数量 = 0, 库存成本价 = 0, 库存金额 = 0, 应付金额 = 0, 已付金额 = 0, 应收金额 = 0, 已收金额 = 0";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 库存表 SET 库存数量 = 0, 库存金额 = 0, 库存成本价 = 0, 核算成本价 = 0, 应付金额 = 0, 已付金额 = 0, 应收金额 = 0, 已收金额 = 0";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 单位表 SET 应付账款 = 0, 应收账款 = 0";
                sqlComm.ExecuteNonQuery();

                sqlta.Commit();
                MessageBox.Show("删除完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        }

        private void 职位权限管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAccessLimit frmAccessLimit = new FormAccessLimit();
            frmAccessLimit.strConn = strConn;

            frmAccessLimit.ShowDialog();
        }





    }
}
