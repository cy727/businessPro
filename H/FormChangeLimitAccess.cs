using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormChangeLimitAccess : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;

        public string strConn = "";

        private ClassGetInformation cGetInformation;

        public int iZYID = 0;
        public string strZYName = "";

        public FormChangeLimitAccess()
        {
            InitializeComponent();
        }

        private void FormChangeLimitAccess_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 权限码 FROM 权限码表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                labelYMM.Text = sqldr.GetValue(0).ToString();
                break;
            }
            sqldr.Close();

            sqlConn.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (textBoxP1.Text != textBoxP2.Text)
            {
                MessageBox.Show("请确认授权密码", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            System.Data.SqlClient.SqlTransaction sqlta;

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                sqlComm.CommandText = "DELETE FROM 权限码表";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "INSERT INTO 权限码表 (权限码) VALUES (N'" + textBoxP1.Text.Trim().ToUpper() + "')";
                sqlComm.ExecuteNonQuery();

                sqlta.Commit();
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
            MessageBox.Show("授权密码修改完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
