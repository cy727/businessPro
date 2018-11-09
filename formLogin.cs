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

            if (File.Exists(dFileName)) //存在文件
            {
                dSet.ReadXml(dFileName);
            }
            else  //建立文件
            {
                dSet.Tables.Add("登录信息");

                dSet.Tables["登录信息"].Columns.Add("用户名", System.Type.GetType("System.String"));
                dSet.Tables["登录信息"].Columns.Add("密码", System.Type.GetType("System.String"));
                dSet.Tables["登录信息"].Columns.Add("自动登录", System.Type.GetType("System.String"));

                string[] strDRow ={ "", "", "0" };
                dSet.Tables["登录信息"].Rows.Add(strDRow);
            }

            textBoxUser.Text = dSet.Tables["登录信息"].Rows[0][0].ToString();
            textBoxPass.Text = dSet.Tables["登录信息"].Rows[0][1].ToString();

            if (dSet.Tables["登录信息"].Rows[0][2].ToString() == "1")
                checkBoxLogin.Checked = true;
            else
                checkBoxLogin.Checked = false; ;

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (textBoxUser.Text.Trim() == "")
            {
                MessageBox.Show("用户名不能为空");
                return;
            }

            if (checkBoxLogin.Checked) //自动登录必须记住密码
                checkBoxRemember.Checked = true;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员姓名, 岗位表.权限, 岗位表.岗位名称, 职员表.PASSWORD, 职员表.部门ID, 职员表.登录状态 FROM 职员表 LEFT OUTER JOIN 岗位表 ON 职员表.岗位ID = 岗位表.ID WHERE (职员表.职员编号 = '" + textBoxUser.Text.Trim() + "') AND (职员表.BeActive = 1)";

            string sTemp = "";
            sqldr = sqlComm.ExecuteReader();
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
                string strPass = sqldr.GetValue(4).ToString().Trim();

                if (sqldr.GetValue(5).ToString() == "") //无部门
                    intUserBM = 0;
                else
                    intUserBM = Int32.Parse(sqldr.GetValue(5).ToString());

                sqldr.Close();
                sqlConn.Close();

                if (textBoxPass.Text.Trim().ToUpper() == strPass.Trim().ToUpper())
                {

                    this.Close();



                    dSet.Tables["登录信息"].Rows[0][0] = textBoxUser.Text;
                    if (checkBoxRemember.Checked) //记住密码
                        dSet.Tables["登录信息"].Rows[0][1] = textBoxPass.Text;
                    else
                        dSet.Tables["登录信息"].Rows[0][1] = "";

                    if (checkBoxLogin.Checked) //自动登录
                        dSet.Tables["登录信息"].Rows[0][2] = "1";
                    else
                        dSet.Tables["登录信息"].Rows[0][2] = "0";
                    dSet.WriteXml(dFileName);
                }
                else
                {
                    MessageBox.Show("用户登录密码错误！", "登录错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sqlConn.Close();
                }
            }
            else
            {
                MessageBox.Show("用户登录错误！没有相应职员", "登录错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlConn.Close();
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}