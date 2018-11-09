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
                MessageBox.Show("��ȷ�����ݿⱸ�ݵ�ַ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SQLDMO.Backup oBackup = new SQLDMO.BackupClass();
            SQLDMO.SQLServer oSQLServer = new SQLDMO.SQLServerClass();
            try
            {
                oSQLServer.LoginSecure = false;
                //�������õ�¼sql��������ip,��¼��,��¼����
                oSQLServer.Connect(strDataBaseAddr, strDataBaseUser, strDataBasePass);
                oBackup.Action = 0;
                //������������ʾ��������״̬
                SQLDMO.BackupSink_PercentCompleteEventHandler pceh = new SQLDMO.BackupSink_PercentCompleteEventHandler(Step2);
                oBackup.PercentComplete += pceh;
                //���ݿ�����:
                oBackup.Database = strDataBaseName;
                //���ݵ�·��
                oBackup.Files = @textBoxLJ.Text;
                //���ݵ��ļ���
                oBackup.BackupSetName = "BUSINESS";
                oBackup.BackupSetDescription = "���ݿⱸ��";
                oBackup.Initialize = true;
                oBackup.SQLBackup(oSQLServer);
                MessageBox.Show("���ݳɹ���", "��ʾ");
            }
            catch
            {
                MessageBox.Show("����ʧ�ܣ�", "��ʾ");
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