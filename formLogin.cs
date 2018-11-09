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
    public partial class formLogin : Form
    {
        public string strConn = "";
        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private string dFileName = "";


        private System.Data.SqlClient.SqlDataReader sqldr;

        public formLogin()
        {
            InitializeComponent();
        }

        private void formLogin_Load(object sender, EventArgs e)
        {
            sqlComm.Connection = sqlConn;
            sqlConn.ConnectionString = strConn;

            if (strConn == "") return;
            dFileName = Directory.GetCurrentDirectory() + "\\login.xml";

            if (File.Exists(dFileName)) //�����ļ�
            {
                dSet.ReadXml(dFileName);
            }
            else  //�����ļ�
            {
                dSet.Tables.Add("��¼��Ϣ");

                dSet.Tables["��¼��Ϣ"].Columns.Add("�û���", System.Type.GetType("System.String"));
                dSet.Tables["��¼��Ϣ"].Columns.Add("����", System.Type.GetType("System.String"));
                dSet.Tables["��¼��Ϣ"].Columns.Add("�Զ���¼", System.Type.GetType("System.String"));

                string[] strDRow ={ "", "", "0" };
                dSet.Tables["��¼��Ϣ"].Rows.Add(strDRow);
            }

            textBoxUser.Text = dSet.Tables["��¼��Ϣ"].Rows[0][0].ToString();
            textBoxPass.Text = dSet.Tables["��¼��Ϣ"].Rows[0][1].ToString();

            if (dSet.Tables["��¼��Ϣ"].Rows[0][2].ToString() == "1")
                checkBoxLogin.Checked = true;
            else
                checkBoxLogin.Checked = false; ;

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (textBoxUser.Text.Trim() == "")
            {
                MessageBox.Show("�û�������Ϊ��");
                return;
            }

            if (checkBoxLogin.Checked) //�Զ���¼�����ס����
                checkBoxRemember.Checked = true;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ְԱ��.ID, ְԱ��.ְԱ����, ��λ��.Ȩ��, ��λ��.��λ����, ְԱ��.PASSWORD, ְԱ��.����ID, ְԱ��.��¼״̬ FROM ְԱ�� LEFT OUTER JOIN ��λ�� ON ְԱ��.��λID = ��λ��.ID WHERE (ְԱ��.ְԱ��� = '" + textBoxUser.Text.Trim() + "') AND (ְԱ��.BeActive = 1)";

            string sTemp = "";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                if (sqldr.GetValue(6).ToString() != "") //�ѵ�¼
                {
                    if (sqldr.GetValue(6).ToString() == "1")
                        sTemp = sqldr.GetValue(1).ToString().Trim();
                    else
                        sTemp = sqldr.GetValue(1).ToString().Trim() + "-" + sqldr.GetValue(6).ToString().Trim();

                    if (Int32.Parse(sqldr.GetValue(2).ToString()) < 18)
                    {
                        MessageBox.Show("�û���" + sTemp + "���ѵ�¼ϵͳ���������Ա��ϵ��", "��¼����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sqldr.Close();
                        sqlConn.Close();

                        this.Close();
                        return;
                    }
                    else
                    {
                        if (MessageBox.Show("�û���" + sTemp + "���ѵ�¼ϵͳ���Ƿ�ǿ�е�¼��", "��¼����", MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes)
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

                if (sqldr.GetValue(2).ToString() == "") //�޸�λ
                    intUserLimit = 0;
                else
                    intUserLimit = Int32.Parse(sqldr.GetValue(2).ToString());

                strUserName = sqldr.GetValue(1).ToString().Trim();
                string strPass = sqldr.GetValue(4).ToString().Trim();

                if (sqldr.GetValue(5).ToString() == "") //�޲���
                    intUserBM = 0;
                else
                    intUserBM = Int32.Parse(sqldr.GetValue(5).ToString());

                sqldr.Close();
                sqlConn.Close();

                if (textBoxPass.Text.Trim().ToUpper() == strPass.Trim().ToUpper())
                {

                    this.Close();



                    dSet.Tables["��¼��Ϣ"].Rows[0][0] = textBoxUser.Text;
                    if (checkBoxRemember.Checked) //��ס����
                        dSet.Tables["��¼��Ϣ"].Rows[0][1] = textBoxPass.Text;
                    else
                        dSet.Tables["��¼��Ϣ"].Rows[0][1] = "";

                    if (checkBoxLogin.Checked) //�Զ���¼
                        dSet.Tables["��¼��Ϣ"].Rows[0][2] = "1";
                    else
                        dSet.Tables["��¼��Ϣ"].Rows[0][2] = "0";
                    dSet.WriteXml(dFileName);
                }
                else
                {
                    MessageBox.Show("�û���¼�������", "��¼����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sqlConn.Close();
                }
            }
            else
            {
                MessageBox.Show("�û���¼����û����ӦְԱ", "��¼����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlConn.Close();
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}