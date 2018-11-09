using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormDataBaseReStore : Form
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

        private ClassGetInformation cGetInformation;
        public FormDataBaseReStore()
        {
            InitializeComponent();
        }

        private void FormDataBaseReStore_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            /*
            if (textBoxLJ.Text == "")
            {
                MessageBox.Show("请确定数据库备份地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SQLDMO.Restore restore = new SQLDMO.RestoreClass();
            SQLDMO.SQLServer server = new SQLDMO.SQLServerClass();
            server.Connect(strDataBaseAddr, strDataBaseUser, strDataBasePass);

            //KILL DataBase Process
            sqlConn.ConnectionString = strConn;
            sqlConn.Open();
            sqlComm.CommandText = "use master Select spid FROM sysprocesses ,sysdatabases Where sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='"+strDataBaseName+"'";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                server.KillProcess(Convert.ToInt32(sqldr[0].ToString()));
            }
            sqldr.Close();
            sqlConn.Close();

            try
            {
                restore.Action = 0;
                SQLDMO.RestoreSink_PercentCompleteEventHandler pceh = new SQLDMO.RestoreSink_PercentCompleteEventHandler(Step2);
                restore.PercentComplete += pceh;
                restore.Database = strDataBaseName;
                restore.Files = @textBoxLJ.Text;
                restore.ReplaceDatabase = true;
                restore.SQLRestore(server);
                MessageBox.Show("数据库恢复成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                server.DisConnect();
            }
                         * */
        }
        private void Step2(string message, int percent)
        {
            toolStripProgressBar1.Value = percent;
        }


    }
}