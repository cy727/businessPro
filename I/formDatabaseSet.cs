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
                MessageBox.Show("���ݿ����Ӵ����������Ա��ϵ");
                strConn = "";
                return;

            }

            MessageBox.Show("���ݿ���������");
            sqlConn.Close();

            dSet.Tables["���ݿ���Ϣ"].Rows[0][0] = textBoxServer.Text;
            dSet.Tables["���ݿ���Ϣ"].Rows[0][1] = textBoxUser.Text;

            if(checkBoxRember.Checked) //��ס����
                dSet.Tables["���ݿ���Ϣ"].Rows[0][2] = textBoxPassword.Text;
            else
                dSet.Tables["���ݿ���Ϣ"].Rows[0][2] = "";

            dSet.Tables["���ݿ���Ϣ"].Rows[0][3] = textBoxDatabase.Text;
            dSet.WriteXml(dFileName);


            this.Close();

        }

        private void formDatabaseSet_Load(object sender, EventArgs e)
        {
            if (intMode == 0)//����
            {
                btnTest.Visible = true;
                btnCreate.Visible = false;
                this.Text = "���ݿ�����";
            }
            else //����
            {
                btnTest.Visible = false;
                btnCreate.Visible = true;
                this.Text = "�������ݿ�";
            }

            sqlComm.Connection = sqlConn;

            if(File.Exists(dFileName)) //�����ļ�
            {
                dSet.ReadXml(dFileName);
            }
            else  //�����ļ�
            {
                dSet.Tables.Add("���ݿ���Ϣ");

                dSet.Tables["���ݿ���Ϣ"].Columns.Add("��������ַ", System.Type.GetType("System.String"));
                dSet.Tables["���ݿ���Ϣ"].Columns.Add("�û���", System.Type.GetType("System.String"));
                dSet.Tables["���ݿ���Ϣ"].Columns.Add("����", System.Type.GetType("System.String"));
                dSet.Tables["���ݿ���Ϣ"].Columns.Add("���ݿ�", System.Type.GetType("System.String"));

                string[]  strDRow ={ "","","",""};
                dSet.Tables["���ݿ���Ϣ"].Rows.Add(strDRow);
            }

            textBoxServer.Text = dSet.Tables["���ݿ���Ϣ"].Rows[0][0].ToString();
            textBoxUser.Text = dSet.Tables["���ݿ���Ϣ"].Rows[0][1].ToString();
            textBoxPassword.Text = dSet.Tables["���ݿ���Ϣ"].Rows[0][2].ToString();
            textBoxDatabase.Text = dSet.Tables["���ݿ���Ϣ"].Rows[0][3].ToString();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("��������в������ݿⴴ��", "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;

            
            strConn = "packet size=4096;user id=" + textBoxUser.Text.Trim().ToLower() + ";password=" + textBoxPassword.Text.Trim().ToLower() + ";data source=\"" + textBoxServer.Text.Trim() + "\";initial catalog=;Integrated Security=True";

            try
            {
                sqlConn.ConnectionString = strConn;
                sqlConn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ݿⴴ��ʧ�ܣ�" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                sqlComm.CommandText = "CREATE TABLE [dbo].[����̵���ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[����ID] [int] NULL,[��ƷID] [int] NULL,[�������] [decimal](18, 0) NULL,[�����] [decimal](14, 2) NULL,[ʵ������] [decimal](18, 0) NULL,[��ע] [nvarchar](200) NULL,[�̵��־] [int] NULL,[��������] [decimal](18, 0) NULL,[������] [decimal](18, 0) NULL,[�ⷿID] [int] NULL, CONSTRAINT [PK_����̵���ϸ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[����̵���ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,[����] [smalldatetime] NULL,[ҵ��ԱID] [int] NULL,[����ԱID] [int] NULL,[�̵���] [bit] NULL,[��ע] [nvarchar](200) NULL,[�ⷿID] [int] NULL,[��ƷID] [int] NULL,[����ID] [int] NULL,[�����ϼ�] [decimal](18, 0) NULL,[���ϼ�] [decimal](14, 2) NULL,[���������ϼ�] [decimal](18, 0) NULL,[������ϼ�] [decimal](14, 2) NULL,	[BeActive] [bit] NULL,[�̵�ʱ��] [smalldatetime] NULL,CONSTRAINT [PK_����̵���ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[����̵㶨���]([��ע] [nvarchar](200) NULL,[��¼ѡ��] [bit] NULL,[��������] [decimal](18, 0) NULL,[������] [decimal](14, 2) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[����]([ID] [int] IDENTITY(1,1) NOT NULL,[�ⷿID] [int] NULL,[��ƷID] [int] NULL,[�������] [decimal](10, 0) NOT NULL,[�����] [decimal](12, 2) NOT NULL,[���ɱ���] [decimal](14, 2) NOT NULL,[����ɱ���] [decimal](14, 2) NULL,[�������] [decimal](12, 0) NOT NULL,[�������] [decimal](12, 0) NOT NULL,[����������] [decimal](12, 0) NOT NULL,[����������] [decimal](12, 0) NOT NULL,[Ӧ�����] [decimal](18, 2) NOT NULL,[�Ѹ����] [decimal](18, 2) NOT NULL,[Ӧ�ս��] [decimal](14, 2) NOT NULL,[���ս��] [decimal](14, 2) NOT NULL,[BeActive] [bit] NULL,CONSTRAINT [PK_����] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��汨����ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[����ID] [int] NULL,[��ƷID] [int] NULL,[��������] [decimal](18, 0) NULL,[������] [decimal](14, 2) NULL,[�ɱ�����] [decimal](14, 2) NULL,[��ע] [nvarchar](200) NULL,[ԭ�������] [decimal](18, 0) NULL,CONSTRAINT [PK_��汨����ϸ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��汨����ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,[����] [smalldatetime] NULL,[ҵ��ԱID] [int] NULL,[����ԱID] [int] NULL,[��ע] [nvarchar](200) NULL,[�ⷿID] [int] NULL,[��ƷID] [int] NULL,[����ID] [int] NULL,[���������ϼ�] [decimal](18, 0) NULL,[������ϼ�] [decimal](14, 2) NULL,[BeActive] [bit] NULL,CONSTRAINT [PK_��汨����ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����˳���ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[����ID] [int] NOT NULL,[��ƷID] [int] NOT NULL,[�ⷿID] [int] NOT NULL,[ԭ����ID] [int] NULL,[��λ] [char](4) NULL,[���] [int] NULL,[����] [decimal](12, 2) NULL,[����] [decimal](10, 2) NULL,[��˰��] [decimal](14, 3) NULL,[����] [decimal](12, 3) NULL,[���] [decimal](14, 2) NULL,[˰��] [decimal](7, 2) NULL,[˰��] [decimal](14, 2) NULL,[����] [decimal](7, 2) NULL,[���ۼ�] [decimal](14, 2) NULL,[ë��] [decimal](14, 2) NULL,[��Ʒ] [bit] NULL,[���۽��] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[ʵ�ƽ��] [decimal](14, 2) NULL,[������] [decimal](18, 0) NULL,[BeActive] [bit] NULL,[δ������] [decimal](14, 2) NULL,[�Ѹ�����] [decimal](14, 2) NULL,[δ��������] [decimal](14, 2) NULL,[�Ѹ�������] [decimal](14, 2) NULL,CONSTRAINT [PK_�����˳���ϸ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����˳����ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[��λID] [int] NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,[����] [smalldatetime] NULL,[��Ʊ��] [varchar](200) NULL,[֧Ʊ��] [varchar](200) NULL,[��ͬ��] [nvarchar](20) NULL,[��˰�ϼ�] [decimal](14, 2) NULL,[ҵ��ԱID] [int] NULL,[BeActive] [bit] NULL,[����ԱID] [int] NULL,[��ע] [nvarchar](200) NULL,[δ������] [decimal](14, 2) NULL,[�Ѹ�����] [decimal](14, 2) NULL,[������] [bit] NULL,[����ʱ��] [smalldatetime] NULL,[����ID] [int] NULL,[���ʱ��] [smalldatetime] NULL,CONSTRAINT [PK_�����˳����ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���������ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[����ID] [int] NOT NULL,[��ƷID] [int] NOT NULL,[�ⷿID] [int] NOT NULL,[ԭ����ID] [int] NULL,[��λ] [char](4) NULL,[���] [int] NULL,[����] [decimal](12, 2) NULL,[����] [decimal](10, 2) NULL,[��˰��] [decimal](14, 3) NULL,[����] [decimal](12, 3) NULL,[���] [decimal](14, 2) NULL,[˰��] [decimal](7, 2) NULL,[˰��] [decimal](14, 2) NULL,[����] [decimal](7, 2) NULL,[���ۼ�] [decimal](14, 2) NULL,[ë��] [decimal](14, 2) NULL,[��Ʒ] [bit] NULL,[���۽��] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[ʵ�ƽ��] [decimal](14, 2) NULL,[BeActive] [bit] NULL,[δ������] [decimal](14, 2) NULL,[�Ѹ�����] [decimal](14, 2) NULL,[δ��������] [decimal](14, 2) NULL,[�Ѹ�������] [decimal](14, 2) NULL,[ԭ������ϸID] [int] NULL,CONSTRAINT [PK_���������ϸ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���������ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[��λID] [int] NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,[����] [smalldatetime] NULL,[��Ʊ��] [varchar](200) NULL,[֧Ʊ��] [varchar](200) NULL,[��ͬ��] [nvarchar](20) NULL,[��˰�ϼ�] [decimal](14, 2) NULL,[ҵ��ԱID] [int] NULL,[����ԱID] [int] NULL,[δ������] [decimal](14, 2) NULL,[�Ѹ�����] [decimal](14, 2) NULL,[������] [bit] NULL,[��ע] [varchar](200) NULL,[BeActive] [bit] NULL,[����ID] [int] NULL,[����ʱ��] [smalldatetime] NULL,[����ID] [int] NULL,	[���ʱ��] [smalldatetime] NULL,CONSTRAINT [PK_���������ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[������Ϣ�޸ı�]([ID] [int] IDENTITY(1,1) NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,[�޸ĵ���ID] [int] NOT NULL,[����] [smalldatetime] NULL,[����ID] [int] NULL,[ҵ��ԱID] [int] NULL,[����ԱID] [int] NULL,[ԭ��ע] [nvarchar](50) NULL,[ԭ��ϵ�绰] [nvarchar](50) NULL,[ԭ��ϵ��] [nvarchar](20) NULL,[ԭ�ջ���] [nvarchar](20) NULL,[ԭ��վ] [nvarchar](20) NULL,[ԭ���䷽ʽ] [nvarchar](50) NULL,[ԭ��ϸ��ַ] [nvarchar](100) NULL,[ԭ��������] [nvarchar](100) NULL,[ԭ����] [nvarchar](30) NULL,[ԭ��������] [nvarchar](6) NULL,[BeActive] [bit] NULL,[��ע] [nvarchar](50) NULL,[��ϵ�绰] [nvarchar](50) NULL,[��ϵ��] [nvarchar](20) NULL,[�ջ���] [nvarchar](20) NULL,[��վ] [nvarchar](20) NULL,[���䷽ʽ] [nvarchar](50) NULL,[��ϸ��ַ] [nvarchar](100) NULL,[��������] [nvarchar](100) NULL,[����] [nvarchar](30) NULL,[��������] [nvarchar](6) NULL,CONSTRAINT [PK_������Ϣ�޸ı�] RIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���������ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[��ID] [int] NOT NULL,[��ƷID] [int] NOT NULL,[�ⷿID] [int] NULL,[������] [smalldatetime] NULL,[����] [decimal](18, 0) NULL,[����] [decimal](12, 3) NULL,[���] [decimal](14, 2) NULL,[���ɱ���] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[��ע] [nvarchar](200) NULL,[BeActive] [bit] NULL,CONSTRAINT [PK_���������ϸ��] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���������ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,[��λID] [int] NOT NULL,[����] [smalldatetime] NULL,[���] [decimal](14, 2) NULL,[˰��] [decimal](14, 2) NULL,[����ID] [int] NULL,[ҵ��ԱID] [int] NULL,[����ԱID] [int] NULL,[���ʽ] [int] NULL,[��Ʊ��] [nvarchar](12) NULL,[��ͬID] [int] NULL,[��ע] [nvarchar](50) NULL,[��˰�ϼ�] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[������] [bit] NULL,[BeActive] [bit] NULL,[��ϵ�绰] [nvarchar](50) NULL,[��ϵ��] [nvarchar](20) NULL,[�ջ���] [nvarchar](20) NULL,[��վ] [nvarchar](20) NULL,[���䷽ʽ] [nvarchar](50) NULL,[��ϸ��ַ] [nvarchar](100) NULL,[��������] [nvarchar](100) NULL,[����] [nvarchar](30) NULL,[��������] [nvarchar](6) NULL,[������] [bit] NULL,[δ������] [decimal](14, 2) NULL,[�Ѹ�����] [decimal](14, 2) NULL,[��ֵ���ID] [int] NULL,[���ʱ��] [smalldatetime] NULL,CONSTRAINT [PK_���������ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��ת�������ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[��תID] [int] NULL,[��λID] [int] NULL,[Ӧ�����] [decimal](14, 2) NULL,[Ӧ�����] [decimal](14, 2) NULL,CONSTRAINT [PK_��ת�������ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��ת�ⷿ���ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[��תID] [int] NULL,[��ƷID] [int] NULL,[�ⷿID] [int] NULL,[��ת����] [decimal](18, 0) NULL,[��ת����] [decimal](14, 2) NULL,[��ת���] [decimal](14, 2) NULL,[�������] [decimal](18, 0) NULL,[�����] [decimal](18, 0) NULL,[�����������] [decimal](18, 0) NULL,[���������] [decimal](18, 0) NULL,[��������] [decimal](18, 0) NULL,[������] [decimal](18, 0) NULL,[��������] [decimal](18, 0) NULL,[���۽��] [decimal](18, 0) NULL,[����ë��] [decimal](18, 0) NULL,[����ë����] [decimal](18, 0) NULL,CONSTRAINT [PK_��ת�ⷿ���ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��ת��������ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[��תID] [int] NULL,[��ƷID] [int] NULL,[��ת����] [decimal](18, 0) NULL,[��ת����] [decimal](14, 2) NULL,[��ת���] [decimal](14, 2) NULL,[�������] [decimal](18, 0) NULL,[�����] [decimal](18, 0) NULL,[�����������] [decimal](18, 0) NULL,[���������] [decimal](18, 0) NULL,[��������] [decimal](18, 0) NULL,[������] [decimal](18, 0) NULL,[��������] [decimal](18, 0) NULL,[���۽��] [decimal](18, 0) NULL,[����ë��] [decimal](18, 0) NULL,[����ë����] [decimal](18, 0) NULL,CONSTRAINT [PK_��ת��������ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��ת���ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[����ʱ��] [smalldatetime] NULL,[����ԱID] [int] NULL,CONSTRAINT [PK_��ת���ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����տ���ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[����ID] [int] NOT NULL,[�˲�ID] [int] NULL,[֧Ʊ��] [nvarchar](100) NULL,[��Ӧ����] [decimal](14, 2) NULL,[����] [decimal](7, 2) NULL,[������] [decimal](14, 2) NULL,[ժҪ] [nvarchar](50) NULL,[����] [smalldatetime] NULL,[��ע] [nvarchar](200) NULL,CONSTRAINT [PK_�����տ���ϸ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����տ���ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,[ԭ����ID] [int] NULL,[��λID] [int] NULL,[˰��] [nvarchar](50) NULL,[ҵ��ԱID] [int] NULL,[����ԱID] [int] NULL,[����] [smalldatetime] NULL,[��Ʊ��] [nvarchar](12) NULL,[��Ʊ����] [smalldatetime] NULL,[��ע] [nvarchar](200) NULL,[BeActive] [bit] NULL,[ʵ�ƽ��] [decimal](14, 2) NULL,[����ID] [int] NULL,[��ע2] [varchar](200) NULL,[���ʱ��] [smalldatetime] NULL,CONSTRAINT [PK_�����տ���ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����տ�ұ�]([ID] [int] IDENTITY(1,1) NOT NULL,[����ID] [int] NULL,[���ҷ�ʽ] [int] NULL,[����ID] [int] NULL,[���ݱ��] [nvarchar](50) NULL,[�Ѹ���] [float] NULL,[BeActive] [int] NULL,CONSTRAINT [PK_�����տ�ұ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����տ���]([����] [bit] NOT NULL,[���ұ��] [int] NULL,[���Ҽ�¼] [int] NULL,[��֧�����] [decimal](14, 2) NULL,[����������] [decimal](18, 0) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���㸶����ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[����ID] [int] NOT NULL,[�˲�ID] [int] NULL,[֧Ʊ��] [nvarchar](100) NULL,[��Ӧ����] [decimal](14, 2) NULL,[����] [decimal](7, 2) NULL,[������] [decimal](14, 2) NULL,[ժҪ] [nvarchar](50) NULL,[����] [smalldatetime] NULL,[��ע] [nvarchar](200) NULL,CONSTRAINT [PK_���㸶����ϸ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���㸶����ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,[ԭ����ID] [int] NULL,[��λID] [int] NULL,[˰��] [nvarchar](50) NULL,[ҵ��ԱID] [int] NULL,[����ԱID] [int] NULL,[����] [smalldatetime] NULL,[��Ʊ��] [nvarchar](12) NULL,[��Ʊ����] [smalldatetime] NULL,[��ע] [nvarchar](200) NULL,[BeActive] [bit] NULL,[ʵ�ƽ��] [decimal](14, 2) NULL,[����ID] [int] NULL,[��ע2] varchar](200) NULL,	[���ʱ��] [smalldatetime] NULL, CONSTRAINT [PK_���㸶����ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���㸶��ұ�]([ID] [int] IDENTITY(1,1) NOT NULL,[����ID] [int] NULL,[���ҷ�ʽ] [int] NULL,[����ID] [int] NULL,[���ݱ��] [nvarchar](50) NULL,[�Ѹ���] [float] NULL,[BeActive] [int] NULL,CONSTRAINT [PK_���㸶��ұ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���㸶����]([����] [bit] NOT NULL,[���ұ��] [int] NULL,[���Ҽ�¼] [int] NULL,[��֧�����] [decimal](14, 2) NULL,[����������] [decimal](18, 0) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���㷽ʽ��]([ID] [int] IDENTITY(1,1) NOT NULL,[djsid] [char](11) NOT NULL,[������] [char](11) NOT NULL,[��������] [char](20) NULL,[������] [char](20) NULL,[����] [char](2) NULL,[�����Ƽ�] [decimal](12, 6) NOT NULL,[��Ҫ���ý�] [char](2) NULL,[�Ƿ��֧��] [char](2) NULL,[���Ź���] [char](2) NULL,[����] [decimal](7, 2) NOT NULL,[�������] [char](2) NULL,[ʹ�÷�ʽ] [char](4) NULL,[��ʾ��Ϣ] [char](20) NULL,[����] [char](2) NULL,[֤���Ź���] [char](2) NULL,[beactive] [char](2) NULL,CONSTRAINT [PK_���㷽ʽ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����˲������ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,	[����ID] [int] NOT NULL,[��ƷID] [int] NOT NULL,[�ⷿID] [int] NOT NULL,[��������] [decimal](12, 2) NULL,[���] [decimal](12, 3) NULL,[���] [decimal](14, 2) NULL,[BeActive] [bit] NULL,[δ������] [decimal](14, 2) NULL,[�Ѹ�����] [decimal](14, 2) NULL,[δ��������] [decimal](14, 2) NULL,[�Ѹ�������] [decimal](14, 2) NULL,CONSTRAINT [PK_�����˲������ϸ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����˲���ۻ��ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,	[��λID] [int] NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,[����] [smalldatetime] NULL,[��Ʊ��] [varchar](200) NULL,[֧Ʊ��] [varchar](200) NULL,[��ͬ��] [nvarchar](20) NULL,[��˰�ϼ�] [decimal](14, 2) NULL,[ҵ��ԱID] [int] NULL,	[����ԱID] [int] NULL,[BeActive] [bit] NULL,[������] [bit] NULL,[��ע] [nvarchar](200) NULL,[δ������] [decimal](14, 2) NULL,[�Ѹ�����] [decimal](14, 2) NULL,[����ʱ��] [smalldatetime] NULL,[����ID] [int] NULL,[���ʱ��] [smalldatetime] NULL,CONSTRAINT [PK_�����˲���ۻ��ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[������Ʒ�Ƶ���ϸ�����]([ID] [int] IDENTITY(1,1) NOT NULL,[��ID] [int] NOT NULL,[��ƷID] [int] NOT NULL,[�ⷿID] [int] NOT NULL,[����] [smalldatetime] NULL,[������] [smalldatetime] NULL,[��λ] [nvarchar](10) NULL,[���] [int] NULL,[����] [decimal](12, 2) NULL,[��˰��] [decimal](14, 3) NULL,[����] [decimal](12, 3) NULL,[���] [decimal](14, 2) NULL,[˰��] [decimal](7, 2) NULL,[˰��] [decimal](14, 2) NULL,[����] [decimal](7, 2) NULL,[ʵ�ƽ��] [decimal](14, 2) NULL,[���ۼ�] [decimal](14, 2) NULL,[ë��] [decimal](14, 2) NULL,[��Ʒ] [bit] NULL,[������] [decimal](14, 2) NULL,[����] [bit] NULL,[�������] [decimal](18, 0) NULL,[�ⷿ���] [char](10) NULL,[�ⷿ����] [nvarchar](50) NULL,[ͳ�Ʊ�־] [int] NULL,[����] [bit] NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[������Ʒ�Ƶ���ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[��ID] [int] NOT NULL,[��ƷID] [int] NOT NULL,[�ⷿID] [int] NULL,[����] [smalldatetime] NULL,[������] [smalldatetime] NULL,[��λ] [nvarchar](10) NULL,[���] [int] NULL,[����] [decimal](12, 2) NULL,[��˰��] [decimal](14, 3) NULL,[����] [decimal](12, 3) NULL,[���] [decimal](14, 2) NULL,[˰��] [decimal](7, 2) NULL,[˰��] [decimal](14, 2) NULL,[����] [decimal](7, 2) NULL,[ʵ�ƽ��] [decimal](14, 2) NULL,[���ۼ�] [decimal](14, 2) NULL,[ë��] [decimal](14, 2) NULL,[��Ʒ] [bit] NULL,[������] [decimal](14, 2) NULL,[δ��������] [decimal](18, 0) NULL,[�ѵ�������] [decimal](18, 0) NULL, CONSTRAINT [PK_������Ʒ�Ƶ���ϸ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[������Ʒ�Ƶ���]([ID] [int] IDENTITY(1,1) NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,[��λID] [int] NOT NULL,[����] [smalldatetime] NULL,[���] [decimal](14, 2) NULL,[˰��] [decimal](14, 2) NULL,[����ID] [int] NULL,[ҵ��ԱID] [int] NULL,[����ԱID] [int] NULL,[���ʽ] [nvarchar](20) NULL,[��Ʊ��] [varchar](200) NULL,[��ͬID] [int] NULL,[��ע] [nvarchar](50) NULL,[��˰�ϼ�] [decimal](14, 2) NULL,[�����] [bit] NULL,[BeActive] [bit] NULL,[���ʱ��] [smalldatetime] NULL, CONSTRAINT [PK_������Ʒ�Ƶ���] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��λ��]([ID] [int] IDENTITY(1,1) NOT NULL,[dgwid] [char](11) NULL,[��λ���] [char](10) NOT NULL,[��λ����] [nvarchar](50) NULL,[������] [char](20) NULL,[��ע] [nvarchar](200) NULL,[Ȩ��] [int] NOT NULL,CONSTRAINT [PK_��λ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��Ʊ��ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[��ƱID] [int] NOT NULL,[���ID] [int] NULL,[����ID] [int] NULL,[���ݱ��] [nvarchar](30) NULL,[ԭ��Ʊ�ܶ�] [decimal](18, 0) NULL,[��Ʊ�ܶ�] [decimal](14, 2) NULL,[������ʽ] [nvarchar](50) NULL,[����] [nvarchar](50) NULL,[��ƷID] [int] NULL,[��ע1] [varchar](200) NULL,[��ע2] [varchar](200) NULL,[��ֱ��] [varchar](30) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��Ʊ���ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[��Ʊ��] [nvarchar](200) NULL,[��λID] [int] NULL,[��ע] [nvarchar](200) NULL,[������ʽ] [nvarchar](50) NULL,[����] [nvarchar](50) NULL,[����ԱID] [int] NULL,[ԭ��Ʊ���] [decimal](14, 2) NULL,[��Ʊ�ܶ�] [decimal](18, 2) NULL,[BeActive] [bit] NULL,[��Ʊ����] [int] NULL,[����] [smalldatetime] NULL,[��������] [smalldatetime] NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��Ʊ�����]([ѡ��] [bit] NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[����֪ͨ����ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[����ID] [int] NULL,[��ƷID] [int] NULL,[ԭ����] [decimal](14, 2) NULL,[����] [decimal](14, 2) NULL,[ԭ������] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,CONSTRAINT [PK_����֪ͨ����ϸ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[����֪ͨ�����ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,[����] [smalldatetime] NULL,[ҵ��ԱID] [int] NULL,[����ԱID] [int] NULL,[��ע] [nvarchar](200) NULL,[BeActive] [bit] NULL,[ִ�б��] [bit] NULL,[ִ��ʱ��] [smalldatetime] NULL,CONSTRAINT [PK_����֪ͨ�����ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[������]([ID] [int] IDENTITY(1,1) NOT NULL,[����] [nvarchar](50) NULL,[���] [nchar](10) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��λ��ʷ�˱�]([ID] [int] IDENTITY(1,1) NOT NULL,[��λID] [int] NULL,[����] [smalldatetime] NULL,[���ݱ��] [nvarchar](50) NULL,[ժҪ] [nvarchar](50) NULL,[����δ�����] [decimal](14, 2) NULL,[�������] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[Ӧ�����] [decimal](14, 2) NULL,[�������] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[Ӧ�ս��] [decimal](14, 2) NULL,[�������] [bit] NULL,[���۱��] [bit] NULL,[����ID] [int] NULL,[ҵ��ԱID] [int] NULL,[��ֵ���] [nvarchar](50) NULL,[BeActive] [bit] NULL, CONSTRAINT [PK_��λ��ʷ�˱�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��λ��]([ID] [int] IDENTITY(1,1) NOT NULL,[ddwid] [char](11) NULL,[��λ���] [char](10) NULL,[��λ����] [nvarchar](50) NULL,[������] [char](20) NULL,[�Ƿ����] [bit] NULL,[�Ƿ�����] [bit] NULL,	[�Ƿ����] [bit] NULL,[�Ƿ���] [bit] NULL,[˰��] [nchar](20) NULL,[�绰] [nvarchar](50) NULL,	[��������] [nvarchar](50) NULL,[�����˺�] [char](30) NULL,[��ϵ��] [nvarchar](50) NULL,[����] [decimal](7, 2) NULL,	[��ַ] [nvarchar](100) NULL,[��������] [nvarchar](50) NULL,[��ҵ����] [nvarchar](50) NULL,[�ͻ��ȼ�] [int] NULL,[����] [varchar](50) NULL,[�ʱ�] [char](6) NULL,[��ע] [ntext] NULL,[��¼����] [smalldatetime] NULL,[ҵ��Ա] [nvarchar](20) NULL,[�ջ���] [nvarchar](20) NULL,[��ϵ��ַ] [nvarchar](60) NULL,[Ӧ���˿�] [decimal](18, 2) NULL,[Ӧ���˿�] [decimal](18, 2) NULL,[BeActive] [bit] NULL,[��վ����] [nvarchar](50) NULL,[����ID] [int] NULL,[��Ʊ�绰] [varchar](50) NULL,[�ջ��绰] [varchar](50) NULL,CONSTRAINT [PK_��λ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��Ʒ�����]([ID] [int] IDENTITY(1,1) NOT NULL,[����] [nvarchar](40) NULL,[��ƷID] [int] NULL,[�ⷿID] [int] NULL,[���ݱ��] [nvarchar](30) NULL,[ժҪ] [nvarchar](50) NULL,[����] [smalldatetime] NULL,[�������] [bit] NULL,[����ԱID] [int] NULL,[������ϸID] [int] NULL,CONSTRAINT [PK_��Ʒ�����] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��Ʒ��ʷ�˱�]([ID] [int] IDENTITY(1,1) NOT NULL,[����] [smalldatetime] NULL,[��ƷID] [int] NULL,[��λID] [int] NULL,[����ID] [int] NULL,[ҵ��ԱID] [int] NULL,[ԭ���ݱ��] [nvarchar](30) NULL,[���ݱ��] [nvarchar](30) NULL,[ժҪ] [nvarchar](50) NULL,[��������] [decimal](18, 0) NULL,[��������] [decimal](14, 2) NULL,[�������] [decimal](14, 2) NULL,[�������] [decimal](18, 0) NULL,[��ⵥ��] [decimal](14, 2) NULL,[�����] [decimal](14, 2) NULL,[��������] [decimal](18, 0) NULL,[���۵���] [decimal](14, 2) NULL,[���۽��] [decimal](14, 2) NULL,[��������] [decimal](18, 0) NULL,[���ⵥ��] [decimal](14, 2) NULL,[������] [decimal](18, 0) NULL,[�ܽ������] [decimal](18, 0) NULL,[�ܽ����] [decimal](14, 2) NULL,[ë��] [decimal](18, 0) NULL,[��������] [decimal](18, 0) NULL,[���㵥��] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[�˳�����] [decimal](18, 0) NULL,[�˳�����] [decimal](14, 2) NULL,[�˳����] [decimal](14, 2) NULL,[�˻�����] [decimal](18, 0) NULL,[�˻ص���] [decimal](14, 2) NULL,[�˻ؽ��] [decimal](14, 2) NULL,[��Ʊ���] [decimal](14, 2) NULL,[Ӧ�����] [decimal](18, 0) NULL,[δ�����] [decimal](18, 0) NULL,[�Ѹ����] [decimal](18, 0) NULL,[Ӧ�ս��] [decimal](14, 2) NULL,[δ�ս��] [decimal](18, 0) NULL,[���ս��] [decimal](14, 2) NULL,[�����˲�������] [decimal](18, 0) NULL,[�����˲��۵���] [decimal](14, 2) NULL,[�����˲��۽��] [decimal](14, 2) NULL,[��������] [decimal](18, 0) NULL,[���𵥼�] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[��������] [decimal](18, 0) NULL,[���𵥼�] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[��������] [decimal](18, 0) NULL,[���ﵥ��] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[BeActive] [bit] NULL,[�����˲�������] [decimal](18, 0) NULL,[�����˲��۵���] [decimal](14, 2) NULL,[�����˲��۽��] [decimal](14, 2) NULL,[��װ����] [decimal](18, 0) NULL,[��װ����] [decimal](14, 2) NULL,[��װ���] [decimal](14, 2) NULL,CONSTRAINT [PK_��Ʒ��ʷ�˱�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��Ʒ�ⷿ��ʷ�˱�]([ID] [int] IDENTITY(1,1) NOT NULL,[����] [smalldatetime] NULL,[��ƷID] [int] NULL,[��λID] [int] NULL,[�ⷿID] [int] NULL,[����ID] [int] NULL,[ҵ��ԱID] [int] NULL,[ԭ���ݱ��] [nvarchar](30) NULL,[���ݱ��] [nvarchar](30) NULL,[ժҪ] [nvarchar](50) NULL,[��������] [decimal](18, 0) NULL,[��������] [decimal](14, 2) NULL,[�������] [decimal](14, 2) NULL,[�������] [decimal](18, 0) NULL,[��ⵥ��] [decimal](14, 2) NULL,[�����] [decimal](14, 2) NULL,[��������] [decimal](18, 0) NULL,[���۵���] [decimal](14, 2) NULL,[���۽��] [decimal](14, 2) NULL,[��������] [decimal](18, 0) NULL,[���ⵥ��] [decimal](14, 2) NULL,[������] [decimal](18, 0) NULL,[�ⷿ�������] [decimal](18, 0) NULL,[�ⷿ�����] [decimal](14, 2) NULL,[ë��] [decimal](18, 0) NULL,[��������] [decimal](18, 0) NULL,[���㵥��] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[�˳�����] [decimal](18, 0) NULL,[�˳�����] [decimal](14, 2) NULL,[�˳����] [decimal](14, 2) NULL,[�˻�����] [decimal](18, 0) NULL,[�˻ص���] [decimal](14, 2) NULL,[�˻ؽ��] [decimal](14, 2) NULL,[��Ʊ���] [decimal](14, 2) NULL,[Ӧ�����] [decimal](18, 0) NULL,[δ�����] [decimal](18, 0) NULL,[�Ѹ����] [decimal](18, 0) NULL,[Ӧ�ս��] [decimal](14, 2) NULL,[δ�ս��] [decimal](18, 0) NULL,[���ս��] [decimal](14, 2) NULL,[�����˲�������] [decimal](18, 0) NULL,[�����˲��۵���] [decimal](14, 2) NULL,[�����˲��۽��] [decimal](14, 2) NULL,[��������] [decimal](18, 0) NULL,[���𵥼�] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[��������] [decimal](18, 0) NULL,[���𵥼�] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[��������] [decimal](18, 0) NULL,[���ﵥ��] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[BeActive] [bit] NULL,[�����˲�������] [decimal](18, 0) NULL,[�����˲��۵���] [decimal](14, 2) NULL,[�����˲��۽��] [decimal](14, 2) NULL,[��װ����] [decimal](18, 0) NULL,[��װ����] [decimal](14, 2) NULL,[��װ���] [decimal](14, 2) NULL,CONSTRAINT [PK_��Ʒ�ⷿ��ʷ�˱�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��Ʒ�����]([ID] [int] IDENTITY(1,1) NOT NULL,[������] [nvarchar](20) NOT NULL,[��������] [nvarchar](50) NULL,[�ϼ�����] [nvarchar](50) NULL,[�ⷿID] [int] NULL,[BeActive] [bit] NULL,CONSTRAINT [PK_��Ʒ�����] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��Ʒ��](	[ID] [int] IDENTITY(1,1) NOT NULL,	[dspid] [char](11) NULL,	[��Ʒ���] [nvarchar](30) NULL,	[��Ʒ����] [nvarchar](50) NULL,	[������] [nvarchar](50) NULL,	[������λ] [nvarchar](4) NULL,	[�������] [int] NULL,	[��С������λ] [nvarchar](4) NULL,	[����˰��] [decimal](12, 2) NULL,	[����˰��] [decimal](12, 2) NULL,	[���ۼ�] [decimal](14, 2) NULL,	[����] [decimal](14, 2) NULL,	[��˰����] [decimal](14, 2) NULL,	[������] [decimal](14, 2) NULL,	[��˰������] [decimal](14, 2) NULL,	[�������] [decimal](12, 0) NOT NULL,	[���ɱ���] [decimal](14, 2) NOT NULL,	[�����] [decimal](12, 2) NOT NULL,	[������] [decimal](10, 0) NULL,	[����ɱ���] [decimal](14, 2) NULL,	[��߽���] [decimal](12, 3) NOT NULL,	[��ͽ���] [decimal](12, 3) NOT NULL,	[���ս���] [decimal](12, 3) NOT NULL,	[��ת����] [decimal](12, 0) NULL,	[��ת����] [decimal](10, 0) NULL,	[��ת���] [decimal](12, 2) NOT NULL,	[��ת����] [decimal](14, 2) NOT NULL,	[��¼����] [smalldatetime] NULL,	[��ɱ���] [decimal](12, 2) NULL,	[����������] [int] NULL,	[�����ڷ�ʽ] [nvarchar](8) NULL,	[��Ʒ����] [nvarchar](50) NULL,	[��Ʒ���] [nvarchar](20) NULL,	[����] [char](20) NULL,	[��ע] [ntext] NULL,	[�������] [decimal](12, 0) NOT NULL,	[�������] [decimal](12, 0) NOT NULL,	[����������] [decimal](12, 0) NOT NULL,	[����������] [decimal](12, 0) NOT NULL,	[����] [nvarchar](8) NULL,	[Ԥ������] [int] NULL,	[�Ƿ��ؼ���Ʒ] [bit] NULL,	[�Ƿ��Ա��Ʒ] [bit] NULL,	[��Ա�ؼ�] [decimal](12, 3) NULL,	[���۷�ʽ] [int] NULL,	[�޶�������] [decimal](14, 2) NULL,	[��װ����] [int] NULL,	[��Ʒ����] [nvarchar](50) NULL,	[������] [int] NULL,	[��װ��Ʒ] [bit] NULL,	[beactive] [bit] NULL,	[Ӧ�����] [decimal](18, 2) NOT NULL,	[�Ѹ����] [decimal](18, 2) NOT NULL,	[Ӧ�ս��] [decimal](14, 2) NOT NULL,	[���ս��] [decimal](14, 2) NOT NULL, CONSTRAINT [PK_��Ʒ��] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��ע�޸ļ�¼��](	[ID] [int] IDENTITY(1,1) NOT NULL,	[����] [smalldatetime] NULL,	[����ԱID] [int] NULL,	[����] [nvarchar](30) NULL,	[ԭ��ע] [nvarchar](200) NULL,	[��ע] [nvarchar](200) NULL,	[ԭ��ע] [nvarchar](200) NULL,	[��ע] [nvarchar](200) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[������Ʒ�Ƶ���ϸ��](	[ID] [int] IDENTITY(1,1) NOT NULL,	[��ID] [int] NOT NULL,	[��ƷID] [int] NOT NULL,	[�ⷿID] [int] NULL,	[��ͬID] [int] NULL,	[����] [smalldatetime] NULL,	[������] [smalldatetime] NULL,	[��λ] [nvarchar](10) NULL,	[���] [int] NULL,	[����] [decimal](12, 2) NULL,	[��˰��] [decimal](14, 3) NULL,	[����] [decimal](12, 3) NULL,	[���] [decimal](14, 2) NULL,	[˰��] [decimal](7, 2) NULL,	[˰��] [decimal](14, 2) NULL,	[����] [decimal](7, 2) NULL,	[ʵ�ƽ��] [decimal](14, 2) NULL,	[���ۼ�] [decimal](14, 2) NULL,	[ë��] [decimal](14, 2) NULL,	[��Ʒ] [bit] NULL,	[������] [decimal](14, 2) NULL,	[δ��������] [decimal](18, 0) NULL,	[�ѳ�������] [decimal](18, 0) NULL,	[BeActive] [bit] NULL,	[У�Ա�־] [bit] NULL,	[δ������] [decimal](14, 2) NULL,	[�Ѹ�����] [decimal](14, 2) NULL,	[δ��������] [decimal](14, 2) NULL,	[�Ѹ�������] [decimal](14, 2) NULL,	[���ɱ���] [decimal](14, 2) NULL, CONSTRAINT [PK_������Ʒ�Ƶ���ϸ��] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[������Ʒ�Ƶ���](	[ID] [int] IDENTITY(1,1) NOT NULL,	[���ݱ��] [nvarchar](20) NOT NULL,	[��λID] [int] NOT NULL,	[����] [smalldatetime] NULL,	[���] [decimal](14, 2) NULL,	[˰��] [decimal](14, 2) NULL,	[����ID] [int] NULL,	[ҵ��ԱID] [int] NULL,	[����ԱID] [int] NULL,	[���ʽ] [int] NULL,	[��Ʊ��] [nvarchar](200) NULL,	[��ͬID] [int] NULL,	[��ע] [nvarchar](50) NULL,	[��˰�ϼ�] [decimal](14, 2) NULL,	[������] [bit] NULL,	[BeActive] [bit] NULL,	[��ϵ�绰] [nvarchar](50) NULL,	[��ϵ��] [nvarchar](20) NULL,	[�ջ���] [nvarchar](20) NULL,	[��վ] [nvarchar](20) NULL,	[���䷽ʽ] [nvarchar](50) NULL,	[��ϸ��ַ] [nvarchar](100) NULL,	[��������] [nvarchar](100) NULL,	[����] [nvarchar](30) NULL,	[��������] [nvarchar](6) NULL,	[������] [bit] NULL,	[δ������] [decimal](14, 2) NULL,	[�Ѹ�����] [decimal](14, 2) NULL,	[����ʱ��] [smalldatetime] NULL,	[���ʱ��] [smalldatetime] NULL, CONSTRAINT [PK_������Ʒ�Ƶ���] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[������Ʒ�����](	[ID] [int] NOT NULL,	[���ɱ���] [decimal](14, 2) NULL,	[�ɱ����] [decimal](14, 2) NULL,	[ͳ�Ʊ�־] [int] NULL,	[����] [bit] NULL, CONSTRAINT [PK_������Ʒ�����] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���ۺ�ͬ��ϸ��](	[ID] [int] IDENTITY(1,1) NOT NULL,	[���ۺ�ͬID] [int] NOT NULL,	[��ƷID] [int] NULL,	[����] [int] NULL,	[����] [decimal](18, 2) NULL,	[�ܼ�] [decimal](18, 2) NULL,	[��ע] [ntext] NULL, CONSTRAINT [PK_���ۺ�ͬ��ϸ��] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���ۺ�ͬ��](	[ID] [int] IDENTITY(1,1) NOT NULL,	[��ͬ���] [nvarchar](20) NOT NULL,	[������λID] [int] NULL,	[ҵ��ԱID] [int] NULL,	[����ԱID] [int] NULL,	[ǩ���ص�] [nvarchar](50) NULL,	[ǩ��ʱ��] [smalldatetime] NULL,	[����Ҫ��] [nvarchar](50) NULL,	[��������] [nvarchar](50) NULL,	[��������] [nvarchar](50) NULL,	[�����Ʒ] [nvarchar](50) NULL,	[���䷽ʽ] [nvarchar](50) NULL,	[������] [nvarchar](50) NULL,	[���óе�] [nvarchar](10) NULL,	[�����ص�] [nvarchar](50) NULL,	[�ֻ�����ʱ��] [nvarchar](50) NULL,	[�ֻ����ʽ] [nvarchar](50) NULL,	[�ڻ�����ʱ��] [nvarchar](50) NULL,	[Ԥ�����] [nvarchar](50) NULL,	[���Ӧ�����] [nvarchar](50) NULL,	[�ڻ����ʽ] [nvarchar](50) NULL,	[ΥԼ����] [nvarchar](50) NULL,	[�ٲ�ίԱ��] [nvarchar](50) NULL,	[����Լ������] [nvarchar](50) NULL,	[��ͬ��Ч��] [nvarchar](50) NULL,	[BeActive] [bit] NULL,	[�˻����] [bit] NULL,	[ִ�б��] [bit] NULL,	[���] [decimal](14, 2) NULL,	[������λ����] [nvarchar](50) NULL,	[����˰��] [nchar](50) NULL,	[�����绰] [nvarchar](50) NULL,	[������������] [nvarchar](50) NULL,	[���������˺�] [char](50) NULL,	[������ϵ��] [nvarchar](50) NULL,	[������ַ] [nvarchar](100) NULL,	[��������] [nvarchar](50) NULL,	[�����ʱ�] [char](6) NULL,	[�跽��λ����] [nvarchar](50) NULL,	[�跽˰��] [nchar](50) NULL,	[�跽�绰] [nvarchar](50) NULL,	[�跽��������] [nvarchar](50) NULL,	[�跽�����˺�] [char](50) NULL,	[�跽��ϵ��] [nvarchar](50) NULL,	[�跽��ַ] [nvarchar](100) NULL,	[�跽����] [nvarchar](50) NULL,	[�跽�ʱ�] [char](6) NULL,	[����ID] [int] NULL,	[���ʱ��] [smalldatetime] NULL, CONSTRAINT [PK_���ۺ�ͬ��] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���۳�����ϸ��](	[ID] [int] IDENTITY(1,1) NOT NULL,	[����ID] [int] NOT NULL,	[��ƷID] [int] NOT NULL,	[�ⷿID] [int] NOT NULL,	[ԭ����ID] [int] NULL,	[��λ] [char](4) NULL,	[���] [int] NULL,	[����] [decimal](12, 2) NULL,	[����] [decimal](10, 2) NULL,	[��˰��] [decimal](14, 3) NULL,	[����] [decimal](12, 3) NULL,	[���] [decimal](14, 2) NULL,	[˰��] [decimal](7, 2) NULL,	[˰��] [decimal](14, 2) NULL,	[����] [decimal](7, 2) NULL,	[���ۼ�] [decimal](14, 2) NULL,	[ë��] [decimal](14, 2) NULL,	[��Ʒ] [bit] NULL,	[���ɱ���] [decimal](14, 2) NULL,	[������] [decimal](14, 2) NULL,	[ʵ�ƽ��] [decimal](14, 2) NULL,	[BeActive] [bit] NULL,	[δ������] [decimal](14, 2) NULL,	[�Ѹ�����] [decimal](14, 2) NULL,	[δ��������] [decimal](14, 2) NULL,	[�Ѹ�������] [decimal](14, 2) NULL,	[ԭ������ϸID] [int] NULL, CONSTRAINT [PK_���۳�����ϸ��] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���۳�����ܱ�](	[ID] [int] IDENTITY(1,1) NOT NULL,	[��λID] [int] NOT NULL,	[���ݱ��] [nvarchar](20) NOT NULL,	[����] [smalldatetime] NULL,	[��Ʊ��] [varchar](200) NULL,	[֧Ʊ��] [varchar](200) NULL,	[��ͬ��] [nvarchar](20) NULL,	[��˰�ϼ�] [decimal](14, 2) NULL,	[ҵ��ԱID] [int] NULL,	[����ԱID] [int] NULL,	[δ������] [decimal](14, 2) NULL,	[�Ѹ�����] [decimal](14, 2) NULL,	[������] [bit] NULL,	[��ע] [nvarchar](200) NULL,	[BeActive] [bit] NULL,	[����ʱ��] [smalldatetime] NULL,	[����ID] [int] NULL,	[����ID] [int] NULL,	[���ʱ��] [smalldatetime] NULL, CONSTRAINT [PK_���۳�����ܱ�] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[ϵͳ������](	[ID] [int] NOT NULL,	[��˾��] [nvarchar](100) NULL,	[��ַ] [nvarchar](100) NULL,	[�绰] [nvarchar](50) NULL,	[����] [nvarchar](50) NULL,	[˰��] [nvarchar](50) NULL,	[��������] [nvarchar](100) NULL,	[�ʺ�] [nvarchar](100) NULL,	[��������] [nvarchar](50) NULL,	[��ʼʱ��] [smalldatetime] NULL,	[������] [nvarchar](100) NULL,	[��˾����] [nvarchar](50) NULL,	[����Ŀ��] [nvarchar](50) NULL,	[����Ŀ��] [nvarchar](50) NULL,	[����Ŀ��] [nvarchar](50) NULL,	[����Ŀ��] [nvarchar](50) NULL,	[����ԱȨ��] [int] NULL,	[�ܾ���Ȩ��] [int] NULL,	[ְԱȨ��] [int] NULL,	[����Ȩ��] [int] NULL,	[ҵ��ԱȨ��] [int] NULL, CONSTRAINT [PK_ϵͳ������] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�ͻ���Ϣ�޸ı�](	[ID] [int] IDENTITY(1,1) NOT NULL,	[���ݱ��] [nvarchar](20) NOT NULL,	[�޸ĵ���ID] [int] NOT NULL,	[����] [smalldatetime] NULL,	[����ID] [int] NULL,	[ҵ��ԱID] [int] NULL,	[����ԱID] [int] NULL,	[ԭ��ע] [nvarchar](50) NULL,	[ԭ��ϵ�绰] [nvarchar](50) NULL,	[ԭ��ϵ��] [nvarchar](20) NULL,	[ԭ�ջ���] [nvarchar](20) NULL,	[ԭ��վ] [nvarchar](20) NULL,	[ԭ���䷽ʽ] [nvarchar](50) NULL,	[ԭ��ϸ��ַ] [nvarchar](100) NULL,	[ԭ��������] [nvarchar](100) NULL,	[ԭ����] [nvarchar](30) NULL,	[ԭ��������] [nvarchar](6) NULL,	[BeActive] [bit] NULL,	[��ע] [nvarchar](50) NULL,	[��ϵ�绰] [nvarchar](50) NULL,	[��ϵ��] [nvarchar](20) NULL,	[�ջ���] [nvarchar](20) NULL,	[��վ] [nvarchar](20) NULL,	[���䷽ʽ] [nvarchar](50) NULL,	[��ϸ��ַ] [nvarchar](100) NULL,	[��������] [nvarchar](100) NULL,	[����] [nvarchar](30) NULL,	[��������] [nvarchar](6) NULL, CONSTRAINT [PK_�ͻ���Ϣ�޸ı�] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�ɹ���ͬ��ϸ��](	[ID] [int] IDENTITY(1,1) NOT NULL,	[�ɹ���ͬID] [int] NOT NULL,	[��ƷID] [int] NULL,	[����] [int] NULL,	[����] [decimal](18, 2) NULL,	[�ܼ�] [decimal](18, 2) NULL,	[��ע] [ntext] NULL, CONSTRAINT [PK_�ɹ���ͬ��ϸ��] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�ɹ���ͬ�����](	[ID] [int] IDENTITY(1,1) NOT NULL,	[�ɹ���ͬID] [int] NULL,	[��ƷID] [int] NULL,	[����] [int] NULL,	[����] [decimal](18, 2) NULL,	[�ܼ�] [decimal](18, 2) NULL,	[��ע] [ntext] NULL, CONSTRAINT [PK_�ɹ���ͬ�����] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�ɹ���ͬ��](	[ID] [int] IDENTITY(1,1) NOT NULL,	[��ͬ���] [nvarchar](20) NOT NULL,	[������λID] [int] NULL,	[ҵ��ԱID] [int] NULL,	[����ԱID] [int] NULL,	[ǩ���ص�] [nvarchar](50) NULL,	[ǩ��ʱ��] [smalldatetime] NULL,	[����Ҫ��] [nvarchar](50) NULL,	[��������] [nvarchar](50) NULL,	[��������] [nvarchar](50) NULL,	[�����Ʒ] [nvarchar](50) NULL,	[���䷽ʽ] [nvarchar](50) NULL,	[������] [nvarchar](50) NULL,	[���óе�] [nvarchar](10) NULL,	[�����ص�] [nvarchar](50) NULL,	[�ֻ�����ʱ��] [nvarchar](50) NULL,	[�ֻ����ʽ] [nvarchar](50) NULL,	[�ڻ�����ʱ��] [nvarchar](50) NULL,	[Ԥ�����] [nvarchar](50) NULL,	[���Ӧ�����] [nvarchar](50) NULL,	[�ڻ����ʽ] [nvarchar](50) NULL,	[ΥԼ����] [nvarchar](50) NULL,	[�ٲ�ίԱ��] [nvarchar](50) NULL,	[����Լ������] [nvarchar](50) NULL,	[��ͬ��Ч��] [nvarchar](50) NULL,	[BeActive] [bit] NULL,	[�˻����] [bit] NULL,	[ִ�б��] [bit] NULL,	[���] [decimal](14, 2) NULL,	[������λ����] [nvarchar](50) NULL,	[����˰��] [nchar](50) NULL,	[�����绰] [nvarchar](50) NULL,	[������������] [nvarchar](50) NULL,	[���������˺�] [char](50) NULL,	[������ϵ��] [nvarchar](50) NULL,	[������ַ] [nvarchar](100) NULL,	[��������] [nvarchar](20) NULL,	[�����ʱ�] [char](6) NULL,	[�跽��λ����] [nvarchar](50) NULL,	[�跽˰��] [nchar](50) NULL,	[�跽�绰] [nvarchar](50) NULL,	[�跽��������] [nvarchar](50) NULL,	[�跽�����˺�] [char](50) NULL,	[�跽��ϵ��] [nvarchar](50) NULL,	[�跽��ַ] [nvarchar](100) NULL,	[�跽����] [nvarchar](50) NULL,	[�跽�ʱ�] [char](6) NULL,	[����ID] [int] NULL,	[���ʱ��] [smalldatetime] NULL, CONSTRAINT [PK_�ɹ���ͬ��] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[���ű�](	[ID] [int] IDENTITY(1,1) NOT NULL,	[dbmid] [char](11) NULL,	[���ű��] [nvarchar](50) NULL,	[��������] [nvarchar](50) NULL,	[������] [char](20) NULL,	[����ְ��] [nvarchar](200) NULL,	[�Ƿ�����] [bit] NULL,	[�Ƿ�����] [bit] NULL,	[�Ƿ�����] [bit] NULL,	[BeActive] [bit] NULL, CONSTRAINT [PK_���ű�] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��������](	[ID] [int] IDENTITY(1,1) NOT NULL,	[ʱ��] [smalldatetime] NULL,	[�ؼ���] [nvarchar](50) NULL,	[����] [int] NULL, CONSTRAINT [PK_��������] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[ְԱ��](	[ID] [int] IDENTITY(1,1) NOT NULL,	[dzyid] [char](11) NULL,	[ְԱ���] [char](8) NOT NULL,	[ְԱ����] [nvarchar](20) NULL,	[PASSWORD] [nvarchar](20) NOT NULL,	[��������] [datetime] NULL,	[�Ļ��̶�] [nvarchar](30) NULL,	[���֤��] [char](18) NULL,	[������] [char](20) NULL,	[ְԱְ��] [nvarchar](20) NULL,	[ְԱְ��] [nvarchar](20) NULL,	[ְԱרҵ] [nvarchar](20) NULL,	[�Ա�] [nvarchar](4) NULL,	[��λID] [int] NULL,	[������] [int] NULL,	[�Ƿ����Ա] [bit] NULL,	[�Ƿ�ҵ��Ա] [bit] NULL,	[�Ƿ�����Ա] [bit] NULL,	[�Ƿ��տ�Ա] [bit] NULL,	[ְԱ�绰] [char](40) NULL,	[��ͥ��ַ] [nvarchar](60) NULL,	[��ҵʱ��] [datetime] NULL,	[��ҵѧУ] [nvarchar](50) NULL,	[��ע] [ntext] NULL,	[BeActive] [bit] NULL,	[����ID] [int] NULL,	[��¼״̬] [varchar](50) NULL, CONSTRAINT [PK_ְԱ��] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[֧����ʽ��](	[ID] [int] IDENTITY(1,1) NOT NULL,	[֧����ʽ] [nvarchar](12) NULL, CONSTRAINT [PK_֧����ʽ��] PRIMARY KEY CLUSTERED (	[ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����Ʒ��Ŀ������ܱ�](	[ID] [int] IDENTITY(1,1) NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,	[����] [smalldatetime] NULL,[ҵ��ԱID] [int] NULL,	[����ԱID] [int] NULL,[��ע] [nvarchar](200) NULL,[BeActive] [bit] NULL, CONSTRAINT [PK_�����Ʒ��Ŀ������ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����Ʒ��ɢ���ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[���ݱ��] [nvarchar](30) NULL,[����] [smalldatetime] NULL,[��װ����ID] [int] NULL,[��ɢ����] [decimal](18, 0) NULL,[��ע] [nvarchar](200) NULL,[BeActive] [bit] NULL,[����ԱID] [int] NULL,[ҵ��ԱID] [int] NULL, CONSTRAINT [PK_�����Ʒ��ɢ���ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�˲���]([ID] [int] IDENTITY(1,1) NOT NULL,[�˲�ID] [nvarchar](11) NULL,[�˲����] [nvarchar](11) NOT NULL,[�˲�����] [nvarchar](20) NULL,[������] [nvarchar](20) NULL,[����] [nvarchar](2) NULL,[�Ƿ��֧��] [bit] NULL,[����] [decimal](7, 2) NOT NULL,[ʹ�÷�ʽ] [nvarchar](4) NULL,[��ʾ��Ϣ] [nvarchar](20) NULL,[����] [bit] NULL,[BeActive] [bit] NULL, CONSTRAINT [PK_�˲���] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����˳���ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[����ID] [int] NOT NULL,[��ƷID] [int] NOT NULL,[�ⷿID] [int] NOT NULL,[ԭ����ID] [int] NULL,[��λ] [char](4) NULL,[���] [int] NULL,[����] [decimal](12, 2) NULL,[����] [decimal](10, 2) NULL,[��˰��] [decimal](14, 3) NULL,[����] [decimal](12, 3) NULL,[���] [decimal](14, 2) NULL,[˰��] [decimal](7, 2) NULL,[˰��] [decimal](14, 2) NULL,[����] [decimal](7, 2) NULL,[���ۼ�] [decimal](14, 2) NULL,[ë��] [decimal](14, 2) NULL,[��Ʒ] [bit] NULL,[���۽��] [decimal](14, 2) NULL,[������] [decimal](14, 2) NULL,[ʵ�ƽ��] [decimal](14, 2) NULL,[δ������] [decimal](18, 0) NULL,[�Ѹ�����] [decimal](14, 2) NULL,[δ��������] [decimal](14, 2) NULL,[�Ѹ�������] [decimal](14, 2) NULL,[BeActive] [bit] NULL,[���ɱ���] [decimal](14, 2) NULL, CONSTRAINT [PK_�����˳���ϸ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[Ȩ�����]([Ȩ����] [nvarchar](50) NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[ģ���]([ID] [int] IDENTITY(1,1) NOT NULL,[ģ������] [nvarchar](200) NULL,[ģ�����] [nvarchar](20) NULL,[ģ��ָ��] [nvarchar](100) NULL,[�ϼ�ID] [int] NULL,[Ȩ����ʾ] [bit] NULL,[ģ�鼶��] [int] NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����˳����ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[��λID] [int] NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,[����] [smalldatetime] NULL,[��Ʊ��] [varchar](200) NULL,[֧Ʊ��] [varchar](200) NULL,[��ͬ��] [nvarchar](20) NULL,[��˰�ϼ�] [decimal](14, 2) NULL,[ҵ��ԱID] [int] NULL,[BeActive] [bit] NULL,[������] [bit] NULL,[����ԱID] [int] NULL,[��ע] [nvarchar](200) NULL,[δ������] [decimal](14, 2) NULL,[�Ѹ�����] [decimal](14, 2) NULL,[����ʱ��] [smalldatetime] NULL,[����ID] [int] NULL,[���ʱ��] [smalldatetime] NULL, CONSTRAINT [PK_�����˳����ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[��־��]([ID] [int] IDENTITY(1,1) NOT NULL,[����] [smalldatetime] NULL,[����ԱID] [int] NULL,[ժҪ] [nvarchar](200) NULL, CONSTRAINT [PK_��־��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[ģ��Ȩ�ޱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[��λID] [int] NULL,[ģ��ID] [int] NULL,[Ȩ��] [bit] NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����˲������ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[����ID] [int] NOT NULL,[��ƷID] [int] NOT NULL,[�ⷿID] [int] NOT NULL,[��������] [decimal](12, 2) NULL,[���] [decimal](12, 3) NULL,[���] [decimal](14, 2) NULL,[BeActive] [bit] NULL,[δ������] [decimal](14, 2) NULL,[�Ѹ�����] [decimal](14, 2) NULL,[δ��������] [decimal](14, 2) NULL,[�Ѹ�������] [decimal](14, 2) NULL, CONSTRAINT [PK_�����˲������ϸ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�ⷿ��]([ID] [int] IDENTITY(1,1) NOT NULL,[dkfid] [char](11) NULL,[�ⷿ���] [nvarchar](30) NOT NULL,[�ⷿ����] [nvarchar](50) NULL,[������] [nvarchar](20) NULL,[�������] [nvarchar](20) NULL,[�������] [int] NULL,[�Ƿ�ⷿ] [bit] NULL,[�Ƿ�����] [bit] NULL,[�Ƿ����] [bit] NULL,[�Ƿ�ֵ�] [bit] NULL,[���] [nvarchar](20) NULL,[�Է���ʶ] [nvarchar](4) NULL,[BeActive] [bit] NULL, CONSTRAINT [PK_�ⷿ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����˲���ۻ��ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[��λID] [int] NOT NULL,[���ݱ��] [nvarchar](20) NOT NULL,[����] [smalldatetime] NULL,[��Ʊ��] [varchar](200) NULL,[֧Ʊ��] [varchar](200) NULL,[��ͬ��] [nvarchar](20) NULL,[��˰�ϼ�] [decimal](14, 2) NULL,[ҵ��ԱID] [int] NULL,[����ԱID] [int] NULL,[BeActive] [bit] NULL,[������] [bit] NULL,[��ע] [nvarchar](200) NULL,[δ������] [decimal](14, 2) NULL,[�Ѹ�����] [decimal](14, 2) NULL,[����ʱ��] [smalldatetime] NULL,[����ID] [int] NULL,[���ʱ��] [smalldatetime] NULL, CONSTRAINT [PK_�����˲���ۻ��ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����Ʒ��װ��ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[����ID] [int] NULL,[���ID] [int] NULL,[�ⷿID] [int] NULL,[�������] [decimal](18, 0) NULL,[�ɱ�����] [decimal](14, 2) NULL,[�ɱ����] [decimal](14, 2) NULL,[��ע] [nvarchar](50) NULL, CONSTRAINT [PK_�����Ʒ��װ��ϸ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����Ʒ��Ŀ������ϸ��]([ID] [int] IDENTITY(1,1) NOT NULL,[����ID] [int] NULL,[��ƷID] [int] NULL,[�ⷿID] [int] NULL,[ԭ���ɱ���] [decimal](18, 0) NULL,[ԭ�������] [decimal](14, 2) NULL,[ԭ�����] [decimal](18, 0) NULL,[���ɱ���] [decimal](18, 0) NULL,[�������] [decimal](14, 2) NULL,[�����] [decimal](18, 0) NULL, CONSTRAINT [PK_�����Ʒ��Ŀ������ϸ��] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����Ʒ��װ���ܱ�]([ID] [int] IDENTITY(1,1) NOT NULL,[���ݱ��] [nvarchar](30) NULL,[����] [smalldatetime] NULL,[��Ʒ�ⷿID] [int] NULL,[��ƷID] [int] NULL,[��Ʒ���] [nvarchar](30) NULL,[��Ʒ����] [nvarchar](50) NULL,[��Ʒ����] [decimal](18, 0) NULL,[��װ����] [decimal](14, 2) NULL,[��ע] [nvarchar](200) NULL,[BeActive] [bit] NULL,[����ԱID] [int] NULL,[ҵ��ԱID] [int] NULL, CONSTRAINT [PK_�����Ʒ��װ���ܱ�] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE TABLE [dbo].[�����Ʒ��װ�����]([�����] [decimal](18, 0) NULL,[ͳ�Ʊ�־] [smallint] NULL) ON [PRIMARY]";
                sqlComm.ExecuteNonQuery();

                //��ͼ
                sqlComm.CommandText = "CREATE VIEW [dbo].[�����ͼ]AS(SELECT     dbo.���������ܱ�.ID, dbo.���������ܱ�.����, dbo.���������ϸ��.��ƷID, dbo.���������ϸ��.�ⷿID, dbo.���������ϸ��.����, dbo.���������ϸ��.����,                       dbo.���������ϸ��.ʵ�ƽ�� AS ���, ���������ϸ��.ID AS ��ϸIDFROM         dbo.���������ϸ�� INNER JOIN                      dbo.���������ܱ� ON dbo.���������ϸ��.����ID = dbo.���������ܱ�.IDWHERE     (dbo.���������ܱ�.BeActive = 1))UNION(SELECT     ���������ܱ�.ID, ���������ܱ�.����, ���������ϸ��.��ƷID, ���������ϸ��.�ⷿID, ABS(���������ϸ��.����) AS ����, ���������ϸ��.����,                         ABS(���������ϸ��.������), ���������ϸ��.ID FROM         ���������ܱ� INNER JOIN                        ���������ϸ�� ON ���������ܱ�.ID = ���������ϸ��.��ID WHERE     (���������ܱ�.BeActive = 1) AND (���������ϸ��.���� < 0))UNION(SELECT     �����˳����ܱ�.ID, �����˳����ܱ�.����, �����˳���ϸ��.��ƷID, �����˳���ϸ��.�ⷿID, - (1 * �����˳���ϸ��.����) AS Expr1, �����˳���ϸ��.����,                         - (1 * �����˳���ϸ��.ʵ�ƽ��) AS Expr2, �����˳���ϸ��.ID FROM         �����˳����ܱ� INNER JOIN                        �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE     (�����˳����ܱ�.BeActive = 1))";
                sqlComm.ExecuteNonQuery();


                sqlComm.CommandText = "CREATE VIEW [dbo].[������ϸ������ͼ]AS(SELECT     ������Ʒ�Ƶ���ϸ��.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���ϸ��.��ID, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���ϸ��.��ƷID, ������Ʒ�Ƶ���ϸ��.�ⷿID,                       ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, 0 AS ë��, ������Ʒ�Ƶ���.BeActiveFROM         ������Ʒ�Ƶ��� INNER JOIN                      ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID)UNION(SELECT     ���������ϸ��.ID, ���������ܱ�.���ݱ��, ���������ϸ��.��ID, ���������ܱ�.����, ���������ϸ��.��ƷID, ���������ϸ��.�ⷿID,                         ���������ϸ��.����, ���������ϸ��.����, ���������ϸ��.������, 0, ���������ܱ�.BeActive FROM         ���������ϸ�� INNER JOIN                        ���������ܱ� ON ���������ϸ��.��ID = ���������ܱ�.ID)UNION(SELECT     �����˲������ϸ��.ID, �����˲���ۻ��ܱ�.���ݱ��, �����˲������ϸ��.����ID, �����˲���ۻ��ܱ�.����, �����˲������ϸ��.��ƷID,                         �����˲������ϸ��.�ⷿID, �����˲������ϸ��.��������, �����˲������ϸ��.���, �����˲������ϸ��.���, �����˲������ϸ��.���,                         �����˲���ۻ��ܱ�.BeActive FROM         �����˲������ϸ�� INNER JOIN                        �����˲���ۻ��ܱ� ON �����˲������ϸ��.����ID = �����˲���ۻ��ܱ�.ID)UNION(SELECT     ���������ϸ��.ID, ���������ܱ�.���ݱ��, ���������ϸ��.����ID, ���������ܱ�.����, ���������ϸ��.��ƷID, ���������ϸ��.�ⷿID,                         ���������ϸ��.����, ���������ϸ��.����, ���������ϸ��.ʵ�ƽ��, 0, ���������ܱ�.BeActive FROM         ���������ϸ�� INNER JOIN                        ���������ܱ� ON ���������ϸ��.����ID = ���������ܱ�.ID)UNION(SELECT     �����˳���ϸ��.ID, �����˳����ܱ�.���ݱ��, �����˳���ϸ��.����ID, �����˳����ܱ�.����, �����˳���ϸ��.��ƷID, �����˳���ϸ��.�ⷿID,                         �����˳���ϸ��.����, �����˳���ϸ��.����, �����˳���ϸ��.ʵ�ƽ��, 0, �����˳����ܱ�.BeActive FROM         �����˳���ϸ�� INNER JOIN                        �����˳����ܱ� ON �����˳���ϸ��.����ID = �����˳����ܱ�.ID)UNION(SELECT     ���۳�����ϸ��.ID, ���۳�����ܱ�.���ݱ��, ���۳�����ϸ��.����ID, ���۳�����ܱ�.����, ���۳�����ϸ��.��ƷID, ���۳�����ϸ��.�ⷿID,                         ���۳�����ϸ��.����, ���۳�����ϸ��.����, ���۳�����ϸ��.ʵ�ƽ��, 0, ���۳�����ܱ�.BeActive FROM         ���۳�����ϸ�� INNER JOIN                        ���۳�����ܱ� ON ���۳�����ϸ��.����ID = ���۳�����ܱ�.ID)UNION(SELECT     ������Ʒ�Ƶ���ϸ��.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���ϸ��.��ID, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���ϸ��.��ƷID, ������Ʒ�Ƶ���ϸ��.�ⷿID,                         ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.ë��, ������Ʒ�Ƶ���.BeActive FROM         ������Ʒ�Ƶ��� INNER JOIN                        ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID)UNION(SELECT     �����˳���ϸ��.ID, �����˳����ܱ�.���ݱ��, �����˳���ϸ��.����ID, �����˳����ܱ�.����, �����˳���ϸ��.��ƷID, �����˳���ϸ��.�ⷿID,                         �����˳���ϸ��.����, �����˳���ϸ��.����, �����˳���ϸ��.ʵ�ƽ��, - 1.0 * (�����˳���ϸ��.ʵ�ƽ�� - �����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���),                         �����˳����ܱ�.BeActive FROM         �����˳���ϸ�� INNER JOIN                        �����˳����ܱ� ON �����˳���ϸ��.����ID = �����˳����ܱ�.ID)UNION(SELECT     �����˲������ϸ��.ID, �����˲���ۻ��ܱ�.���ݱ��, �����˲������ϸ��.����ID, �����˲���ۻ��ܱ�.����, �����˲������ϸ��.��ƷID,                         �����˲������ϸ��.�ⷿID, �����˲������ϸ��.��������, �����˲������ϸ��.���, �����˲������ϸ��.���, �����˲������ϸ��.���,                         �����˲���ۻ��ܱ�.BeActive FROM         �����˲������ϸ�� INNER JOIN                        �����˲���ۻ��ܱ� ON �����˲������ϸ��.����ID = �����˲���ۻ��ܱ�.ID)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[������ϸ��ͼ]AS(SELECT dbo.���������ܱ�.��λID, dbo.���������ܱ�.���ݱ��,       dbo.���������ܱ�.����, dbo.��Ʒ��.��Ʒ���, dbo.��Ʒ��.��Ʒ����,       dbo.���������ϸ��.����, dbo.���������ϸ��.ʵ�ƽ��,       dbo.���������ϸ��.δ������, dbo.���������ϸ��.�Ѹ�����,       dbo.���������ϸ��.δ��������, dbo.���������ϸ��.�Ѹ�������,       dbo.���������ϸ��.����ID, dbo.���������ϸ��.ID, dbo.���������ϸ��.��ƷID,       dbo.��Ʒ��.������, dbo.���������ϸ��.�ⷿID, dbo.���������ܱ�.��ע , dbo.���������ܱ�.ҵ��ԱID FROM dbo.���������ܱ� INNER JOIN      dbo.���������ϸ�� ON       dbo.���������ܱ�.ID = dbo.���������ϸ��.����ID INNER JOIN      dbo.��Ʒ�� ON dbo.���������ϸ��.��ƷID = dbo.��Ʒ��.IDWHERE (dbo.���������ϸ��.BeActive = 1) AND (dbo.���������ܱ�.BeActive = 1))UNION(SELECT �����˳����ܱ�.��λID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����,       ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �����˳���ϸ��.����, �����˳���ϸ��.ʵ�ƽ��*-1,       �����˳���ϸ��.δ������*-1, �����˳���ϸ��.�Ѹ�����*-1,       �����˳���ϸ��.δ��������, �����˳���ϸ��.�Ѹ�������, �����˳���ϸ��.����ID,       �����˳���ϸ��.ID, �����˳���ϸ��.��ƷID, ��Ʒ��.������,       �����˳���ϸ��.�ⷿID, �����˳����ܱ�.��ע , �����˳����ܱ�.ҵ��ԱID FROM ��Ʒ�� INNER JOIN      �����˳���ϸ�� ON ��Ʒ��.ID = �����˳���ϸ��.��ƷID INNER JOIN      �����˳����ܱ� ON �����˳���ϸ��.����ID = �����˳����ܱ�.ID INNER JOIN      ��λ�� ON �����˳����ܱ�.��λID = ��λ��.IDWHERE (�����˳���ϸ��.BeActive = 1) AND (�����˳����ܱ�.BeActive = 1))UNION(SELECT �����˲���ۻ��ܱ�.��λID, �����˲���ۻ��ܱ�.���ݱ��,       �����˲���ۻ��ܱ�.����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����,       �����˲������ϸ��.�������� AS ����, �����˲������ϸ��.���,       �����˲������ϸ��.δ������, �����˲������ϸ��.�Ѹ�����,       �����˲������ϸ��.δ��������, �����˲������ϸ��.�Ѹ�������,       �����˲������ϸ��.����ID, �����˲������ϸ��.ID, �����˲������ϸ��.��ƷID,       ��Ʒ��.������, �����˲������ϸ��.�ⷿID, �����˲���ۻ��ܱ�.��ע , �����˲���ۻ��ܱ�.ҵ��ԱID  FROM �����˲������ϸ�� INNER JOIN      �����˲���ۻ��ܱ� ON       �����˲������ϸ��.����ID = �����˲���ۻ��ܱ�.ID INNER JOIN      ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN      ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.IDWHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲������ϸ��.BeActive = 1))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[���ݻ�����ͼ]AS(SELECT ID, ���ݱ��, ��λID, ����, ����ID, ҵ��ԱID, ����ԱID, ��˰�ϼ�, BeActiveFROM dbo.������Ʒ�Ƶ���)UNION(SELECT ID, ���ݱ��, ��λID, ����, ����ID, ҵ��ԱID, ����ԱID, ��˰�ϼ�, BeActiveFROM ���������ܱ�)UNION(SELECT ID, ���ݱ��, ��λID, ����, ����ID, ҵ��ԱID, ����ԱID, ��˰�ϼ�, BeActiveFROM �����˲���ۻ��ܱ�)UNION(SELECT ID, ���ݱ��, ��λID, ����, ����ID, ҵ��ԱID, ����ԱID, ��˰�ϼ�, BeActiveFROM ���������ܱ�)UNION(SELECT ID, ���ݱ��, ��λID, ����, ����ID, ҵ��ԱID, ����ԱID, ��˰�ϼ�, BeActiveFROM �����˳����ܱ�)UNION(SELECT ID, ���ݱ��, ��λID, ����, ����ID, ҵ��ԱID, ����ԱID, ��˰�ϼ�, BeActiveFROM ���۳�����ܱ�)UNION(SELECT ID, ���ݱ��, ��λID, ����, ����ID, ҵ��ԱID, ����ԱID, ��˰�ϼ�, BeActiveFROM ������Ʒ�Ƶ���)UNION(SELECT ID, ���ݱ��, ��λID, ����, ����ID, ҵ��ԱID, ����ԱID, ��˰�ϼ�, BeActiveFROM �����˳����ܱ�)UNION(SELECT ID, ���ݱ��, ��λID, ����, ����ID, ҵ��ԱID, ����ԱID, ��˰�ϼ�, BeActiveFROM �����˲���ۻ��ܱ�)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[���������ͼ]AS(SELECT ��λID, ���ݱ��, ����, ��˰�ϼ�, �Ѹ�����, δ������, ID, ҵ��ԱID, ����ԱID, ��ע, BeActive FROM dbo.���������ܱ�WHERE (BeActive = 1))UNION(SELECT ��λID, ���ݱ��, ����, ��˰�ϼ�*-1, �Ѹ�����*-1, δ������*-1, ID, ҵ��ԱID, ����ԱID, ��ע, BeActive  FROM �����˳����ܱ�WHERE (BeActive = 1))UNION(SELECT ��λID, ���ݱ��, ����, ��˰�ϼ�, �Ѹ�����, δ������, ID, ҵ��ԱID, ����ԱID, ��ע , BeActive FROM �����˲���ۻ��ܱ�WHERE (BeActive = 1))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[������ͼ]AS(SELECT     ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���ϸ��.��ƷID, ������Ʒ�Ƶ���ϸ��.�ⷿID, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����,                       ������Ʒ�Ƶ���ϸ��.ʵ�ƽ��, ������Ʒ�Ƶ���ϸ��.���ɱ���, ������Ʒ�Ƶ���ϸ��.ID AS ��ϸ��IDFROM         ������Ʒ�Ƶ��� INNER JOIN                      ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��IDWHERE     (������Ʒ�Ƶ���.BeActive = 1))UNION(SELECT     ���������ܱ�.ID, ���������ܱ�.����, ���������ϸ��.��ƷID, ���������ϸ��.�ⷿID, ABS(���������ϸ��.����) AS ����, ���������ϸ��.����,                         ���������ϸ��.������, ���������ϸ��.���ɱ���, ���������ϸ��.ID FROM         ���������ܱ� INNER JOIN                        ���������ϸ�� ON ���������ܱ�.ID = ���������ϸ��.��ID WHERE     (���������ܱ�.BeActive = 1) AND (���������ϸ��.���� > 0))UNION(SELECT     �����˳����ܱ�.ID, �����˳����ܱ�.����, �����˳���ϸ��.��ƷID, �����˳���ϸ��.�ⷿID, - (1 * �����˳���ϸ��.����) AS Expr1, �����˳���ϸ��.����,                         - (1 * �����˳���ϸ��.ʵ�ƽ��) AS Expr2, �����˳���ϸ��.���ɱ���, �����˳���ϸ��.ID FROM         �����˳����ܱ� INNER JOIN                        �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID WHERE     (�����˳����ܱ�.BeActive = 1))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[�տ���ϸ��ͼ]AS(SELECT dbo.������Ʒ�Ƶ���.��λID, dbo.������Ʒ�Ƶ���.���ݱ��,       dbo.������Ʒ�Ƶ���.����, dbo.��Ʒ��.��Ʒ���, dbo.��Ʒ��.��Ʒ����,       dbo.������Ʒ�Ƶ���ϸ��.����, dbo.������Ʒ�Ƶ���ϸ��.ʵ�ƽ��,       dbo.������Ʒ�Ƶ���ϸ��.δ������, dbo.������Ʒ�Ƶ���ϸ��.�Ѹ�����,       dbo.������Ʒ�Ƶ���ϸ��.δ��������, dbo.������Ʒ�Ƶ���ϸ��.�Ѹ�������,       dbo.������Ʒ�Ƶ���ϸ��.��ID AS ����ID, dbo.������Ʒ�Ƶ���ϸ��.ID,       dbo.������Ʒ�Ƶ���ϸ��.��ƷID, dbo.��Ʒ��.������,       dbo.������Ʒ�Ƶ���ϸ��.�ⷿID,  dbo.������Ʒ�Ƶ���.��ע,  dbo.������Ʒ�Ƶ���.ҵ��ԱIDFROM dbo.������Ʒ�Ƶ��� INNER JOIN      dbo.������Ʒ�Ƶ���ϸ�� ON       dbo.������Ʒ�Ƶ���.ID = dbo.������Ʒ�Ƶ���ϸ��.��ID INNER JOIN      dbo.��Ʒ�� ON dbo.������Ʒ�Ƶ���ϸ��.��ƷID = dbo.��Ʒ��.IDWHERE (dbo.������Ʒ�Ƶ���ϸ��.BeActive = 1) AND (dbo.������Ʒ�Ƶ���.BeActive = 1))UNION(SELECT �����˳����ܱ�.��λID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����,       ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, �����˳���ϸ��.����, �����˳���ϸ��.ʵ�ƽ��*-1,       �����˳���ϸ��.δ������*-1, �����˳���ϸ��.�Ѹ�����*-1,       �����˳���ϸ��.δ��������, �����˳���ϸ��.�Ѹ�������, �����˳���ϸ��.����ID,       �����˳���ϸ��.ID, �����˳���ϸ��.��ƷID, ��Ʒ��.������,       �����˳���ϸ��.�ⷿID, �����˳����ܱ�.��ע ,  dbo.�����˳����ܱ�.ҵ��ԱID FROM ��Ʒ�� INNER JOIN      �����˳���ϸ�� ON ��Ʒ��.ID = �����˳���ϸ��.��ƷID INNER JOIN      �����˳����ܱ� ON �����˳���ϸ��.����ID = �����˳����ܱ�.ID INNER JOIN      ��λ�� ON �����˳����ܱ�.��λID = ��λ��.IDWHERE (�����˳���ϸ��.BeActive = 1) AND (�����˳����ܱ�.BeActive = 1))UNION(SELECT �����˲���ۻ��ܱ�.��λID, �����˲���ۻ��ܱ�.���ݱ��,       �����˲���ۻ��ܱ�.����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����,       �����˲������ϸ��.�������� AS ����, �����˲������ϸ��.���,       �����˲������ϸ��.δ������, �����˲������ϸ��.�Ѹ�����,       �����˲������ϸ��.δ��������, �����˲������ϸ��.�Ѹ�������,       �����˲������ϸ��.����ID, �����˲������ϸ��.ID, �����˲������ϸ��.��ƷID,       ��Ʒ��.������, �����˲������ϸ��.�ⷿID,  �����˲���ۻ��ܱ�.��ע   ,  dbo.�����˲���ۻ��ܱ�.ҵ��ԱID FROM �����˲������ϸ�� INNER JOIN      �����˲���ۻ��ܱ� ON       �����˲������ϸ��.����ID = �����˲���ۻ��ܱ�.ID INNER JOIN      ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN      ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.IDWHERE (�����˲���ۻ��ܱ�.BeActive = 1) AND (�����˲������ϸ��.BeActive = 1))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[������ͼ]AS(SELECT dbo.������Ʒ�Ƶ���ϸ��.ID, dbo.������Ʒ�Ƶ���ϸ��.��ID,       dbo.������Ʒ�Ƶ���.���ݱ��, dbo.������Ʒ�Ƶ���.��λID,       dbo.������Ʒ�Ƶ���.����, dbo.������Ʒ�Ƶ���.ҵ��ԱID,       dbo.������Ʒ�Ƶ���.����ID, dbo.������Ʒ�Ƶ���ϸ��.��ƷID,       dbo.������Ʒ�Ƶ���ϸ��.�ⷿID, dbo.������Ʒ�Ƶ���ϸ��.����,       dbo.������Ʒ�Ƶ���ϸ��.����, dbo.������Ʒ�Ƶ���ϸ��.ʵ�ƽ��,       dbo.������Ʒ�Ƶ���ϸ��.ë��, dbo.������Ʒ�Ƶ���ϸ��.δ������,       dbo.������Ʒ�Ƶ���ϸ��.�Ѹ�����, dbo.������Ʒ�Ƶ���ϸ��.���ɱ���,       dbo.������Ʒ�Ƶ���.BeActiveFROM dbo.������Ʒ�Ƶ��� INNER JOIN      dbo.������Ʒ�Ƶ���ϸ�� ON       dbo.������Ʒ�Ƶ���.ID = dbo.������Ʒ�Ƶ���ϸ��.��ID)UNION(SELECT �����˳���ϸ��.ID, �����˳���ϸ��.����ID, �����˳����ܱ�.���ݱ��,       �����˳����ܱ�.��λID, �����˳����ܱ�.����, �����˳����ܱ�.ҵ��ԱID,       �����˳����ܱ�.����ID, �����˳���ϸ��.��ƷID, �����˳���ϸ��.�ⷿID,       - (1 * �����˳���ϸ��.����) AS ����, �����˳���ϸ��.����,       - (1 * �����˳���ϸ��.���) AS ���, �����˳���ϸ��.ë��,       �����˳���ϸ��.δ������, �����˳���ϸ��.�Ѹ�����,       �����˳���ϸ��.���ɱ���, �����˳����ܱ�.BeActiveFROM �����˳����ܱ� INNER JOIN      �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[ë����ͼ]AS(SELECT     ������Ʒ�Ƶ���ϸ��.ID, ������Ʒ�Ƶ���ϸ��.��ID, ������Ʒ�Ƶ���.���ݱ��,������Ʒ�Ƶ���.��λID, ������Ʒ�Ƶ���ϸ��.��ƷID, ������Ʒ�Ƶ���ϸ��.�ⷿID, ������Ʒ�Ƶ���.����,                       ������Ʒ�Ƶ���.����ID, ������Ʒ�Ƶ���.ҵ��ԱID, ������Ʒ�Ƶ���.����ԱID, ������Ʒ�Ƶ���ϸ��.����, ������Ʒ�Ƶ���ϸ��.����,                       ������Ʒ�Ƶ���ϸ��.���ɱ���, ������Ʒ�Ƶ���ϸ��.ʵ�ƽ�� AS ���, ������Ʒ�Ƶ���ϸ��.ë��, ������Ʒ�Ƶ���.BeActiveFROM         ������Ʒ�Ƶ��� INNER JOIN                      ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID)UNION(SELECT     �����˳���ϸ��.ID, �����˳���ϸ��.����ID, �����˳����ܱ�.���ݱ��,  �����˳����ܱ�.��λID,�����˳���ϸ��.��ƷID, �����˳���ϸ��.�ⷿID, �����˳����ܱ�.����,                         �����˳����ܱ�.����ID, �����˳����ܱ�.ҵ��ԱID, �����˳����ܱ�.����ԱID, - (1.0 * �����˳���ϸ��.����) AS ����, �����˳���ϸ��.����,                         �����˳���ϸ��.���ɱ���, - (1.0 * �����˳���ϸ��.ʵ�ƽ��) AS ʵ�ƽ��, - (1.0 * (�����˳���ϸ��.ʵ�ƽ�� - �����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���))                         AS ë��, �����˳����ܱ�.BeActive FROM         �����˳���ϸ�� INNER JOIN                        �����˳����ܱ� ON �����˳���ϸ��.����ID = �����˳����ܱ�.ID)UNION(SELECT     TOP 200 �����˲������ϸ��.ID, �����˲������ϸ��.����ID, �����˲���ۻ��ܱ�.���ݱ��, �����˲���ۻ��ܱ�.��λID,�����˲������ϸ��.��ƷID, 0 AS �ⷿID, �����˲���ۻ��ܱ�.����,                         �����˲���ۻ��ܱ�.����ID, �����˲���ۻ��ܱ�.ҵ��ԱID, �����˲���ۻ��ܱ�.����ԱID, 0, �����˲������ϸ��.���, 0 AS ���ɱ���,                         �����˲������ϸ��.���, �����˲������ϸ��.��� AS ë��, �����˲������ϸ��.BeActive FROM         �����˲������ϸ�� INNER JOIN                        �����˲���ۻ��ܱ� ON �����˲������ϸ��.����ID = �����˲���ۻ��ܱ�.ID)";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "CREATE VIEW [dbo].[�տ������ͼ]AS(SELECT ��λID, ���ݱ��, ����, ��˰�ϼ�, �Ѹ�����, δ������, ID, ҵ��ԱID, ����ԱID, ��ע , BeActive FROM dbo.������Ʒ�Ƶ���WHERE (BeActive = 1))UNION(SELECT ��λID, ���ݱ��, ����, ��˰�ϼ�*-1, �Ѹ�����*-1, δ������*-1, ID, ҵ��ԱID, ����ԱID, ��ע , BeActive FROM �����˳����ܱ�WHERE (BeActive = 1))UNION(SELECT ��λID, ���ݱ��, ����, ��˰�ϼ�, �Ѹ�����, δ������, ID, ҵ��ԱID, ����ԱID, ��ע , BeActive FROM �����˲���ۻ��ܱ�WHERE (BeActive = 1))";
                sqlComm.ExecuteNonQuery();


                
                sqlComm.CommandText = "ALTER TABLE [dbo].[��Ʒ��] ADD  CONSTRAINT [DF_��Ʒ��_Ӧ�����]  DEFAULT (0) FOR [Ӧ�����]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[��Ʒ��] ADD  CONSTRAINT [DF_��Ʒ��_�Ѹ����]  DEFAULT (0) FOR [�Ѹ����]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[��Ʒ��] ADD  CONSTRAINT [DF_��Ʒ��_Ӧ�ս��]  DEFAULT (0) FOR [Ӧ�ս��]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[��Ʒ��] ADD  CONSTRAINT [DF_��Ʒ��_���ս��]  DEFAULT (0) FOR [���ս��]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[���۳�����ܱ�] ADD  CONSTRAINT [DF_���۳�����ܱ�_����ID]  DEFAULT (0) FOR [����ID]";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "ALTER TABLE [dbo].[���ۺ�ͬ��] ADD  CONSTRAINT [DF_���ۺ�ͬ��_�˻����]  DEFAULT (0) FOR [�˻����]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[���ۺ�ͬ��] ADD  CONSTRAINT [DF_���ۺ�ͬ��_�˻����]  DEFAULT (0) FOR [ִ�б��]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[�����˳����ܱ�] ADD  CONSTRAINT [DF_�����˳����ܱ�_������]  DEFAULT (0) FOR [������]";
                sqlComm.ExecuteNonQuery();
                
                sqlComm.CommandText = "ALTER TABLE [dbo].[�����˳����ܱ�] ADD  CONSTRAINT [DF_�����˳����ܱ�_δ������]  DEFAULT (0) FOR [δ������]";
                sqlComm.ExecuteNonQuery();


                sqlComm.CommandText = "ALTER TABLE [dbo].[�����˳����ܱ�] ADD  CONSTRAINT [DF_�����˳����ܱ�_�Ѹ�����]  DEFAULT (0) FOR [�Ѹ�����]";
                sqlComm.ExecuteNonQuery();

                //ȱʡֵ
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CG', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'XS', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CC', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'AKP', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'ADH', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'ATH', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'ATB', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'AYF', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'BKP', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'BCK', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'ZXG', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'BTH', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'BTB', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'BYS', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CPD', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CCK', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'ZCC', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CG', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CZZ', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CCS', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CBS', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'CTZ', 1)";
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = "INSERT INTO ��������(ʱ��, �ؼ���, ����) VALUES(CONVERT(DATETIME, '1999-01-01 00:00:00', 102), N'ETJ', 1)";
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

                //MessageBox.Show("���ݿⴴ���ɹ�������sa���µ�¼ϵͳ" , "���ݿ�", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dSet.Tables["���ݿ���Ϣ"].Rows[0][0] = textBoxServer.Text;
                dSet.Tables["���ݿ���Ϣ"].Rows[0][1] = textBoxUser.Text;

                if (checkBoxRember.Checked) //��ס����
                    dSet.Tables["���ݿ���Ϣ"].Rows[0][2] = textBoxPassword.Text;
                else
                    dSet.Tables["���ݿ���Ϣ"].Rows[0][2] = "";

                dSet.Tables["���ݿ���Ϣ"].Rows[0][3] = textBoxDatabase.Text;
                dSet.WriteXml(dFileName);
                

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ݿⴴ��ʧ�ܣ�" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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