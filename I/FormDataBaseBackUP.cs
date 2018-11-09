using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormDataBaseBackUP : Form
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


        public string strDataBaseAddr = "";
        public string strDataBaseUser = "";
        public string strDataBasePass = "";
        public string strDataBaseName = "";
       
        public FormDataBaseBackUP()
        {
            InitializeComponent();
        }

        private void FormDataBaseBackUP_Load(object sender, EventArgs e)
        {

        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            /*
            if (textBoxLJ.Text == "")
            {
                MessageBox.Show("请确定数据库备份地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SQLDMO.Backup oBackup = new SQLDMO.BackupClass();
            SQLDMO.SQLServer oSQLServer = new SQLDMO.SQLServerClass();
            try
            {
                oSQLServer.LoginSecure = false;
                //下面设置登录sql服务器的ip,登录名,登录密码
                oSQLServer.Connect(strDataBaseAddr, strDataBaseUser, strDataBasePass);
                oBackup.Action = 0;
                //下面两句是显示进度条的状态
                SQLDMO.BackupSink_PercentCompleteEventHandler pceh = new SQLDMO.BackupSink_PercentCompleteEventHandler(Step2);
                oBackup.PercentComplete += pceh;
                //数据库名称:
                oBackup.Database = strDataBaseName;
                //备份的路径
                oBackup.Files = @textBoxLJ.Text;
                //备份的文件名
                oBackup.BackupSetName = "BUSINESS";
                oBackup.BackupSetDescription = "数据库备份";
                oBackup.Initialize = true;
                oBackup.SQLBackup(oSQLServer);
                MessageBox.Show("备份成功！", "提示");
            }
            catch
            {
                MessageBox.Show("备份失败！", "提示");
            }
            finally
            {
                oSQLServer.DisConnect();
            }
             * */
        }

        private void Step2(string message, int percent)
        {
            toolStripProgressBar1.Value = percent;
        }

    }
}