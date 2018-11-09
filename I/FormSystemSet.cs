using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSystemSet : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;


        private string sGSMC="";
        private string sGSDZ = "";
        private string sGSDH = "";
        private string sGSCZ = "";
        private string sGSYB = "";
        private string sGSZH = "";
        private string sGSKHYH = "";
        private string sGSSH = "";


        
        public FormSystemSet()
        {
            InitializeComponent();
        }

        private void FormSystemSet_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            try
            {
                sqlConn.Open();
                sqlComm.CommandText = "SELECT 公司名, 地址, 电话, 传真, 税号, 开户银行, 帐号, 邮政编码, 开始时间, 负责人 FROM 系统参数表";
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    textBoxGSMC.Text = sqldr.GetValue(0).ToString();
                    textBoxGSDZ.Text = sqldr.GetValue(1).ToString();
                    textBoxGSDH.Text = sqldr.GetValue(2).ToString();
                    textBoxGSCZ.Text = sqldr.GetValue(3).ToString();
                    textBoxSH.Text = sqldr.GetValue(4).ToString();
                    textBoxKHYH.Text = sqldr.GetValue(5).ToString();
                    textBoxZH.Text = sqldr.GetValue(6).ToString();
                    textBoxYZBM.Text = sqldr.GetValue(7).ToString();

                    dateTimePickerQYSJ.Value = Convert.ToDateTime(sqldr.GetValue(8).ToString());
                    textBoxFZR.Text = sqldr.GetValue(9).ToString();

                    break;
                }
                sqldr.Close();

                sqlComm.CommandText = "SELECT 公司宣传, 质量目标1, 质量目标2, 质量目标3, 质量目标4, 管理员权限, 总经理权限, 职员权限, 经理权限, 业务员权限 FROM 系统参数表";
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    textBoxGSXC.Text = sqldr.GetValue(0).ToString();
                    textBoxM1.Text = sqldr.GetValue(1).ToString();
                    textBoxM2.Text = sqldr.GetValue(2).ToString();
                    textBoxM3.Text = sqldr.GetValue(3).ToString();
                    textBoxM4.Text = sqldr.GetValue(4).ToString();

                    numericUpDownGLY.Value = decimal.Parse(sqldr.GetValue(5).ToString());
                    numericUpDownZJL.Value = decimal.Parse(sqldr.GetValue(6).ToString());
                    numericUpDownZY.Value = decimal.Parse(sqldr.GetValue(7).ToString());
                    numericUpDownJL.Value = decimal.Parse(sqldr.GetValue(8).ToString());
                    numericUpDownYWY.Value = decimal.Parse(sqldr.GetValue(9).ToString());
                    break;
                }

            }
            catch
            {
            }
            finally
            {
                sqlConn.Close();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            sqlConn.Open();
            try
            {
                sqlComm.CommandText = "UPDATE 系统参数表 SET 公司名 = N'" + textBoxGSMC.Text + "', 地址 = N'" + textBoxGSDZ.Text + "', 电话 = N'" + textBoxGSDH.Text + "', 传真 = N'" + textBoxGSCZ.Text + "', 税号 = N'" + textBoxSH.Text + "', 开户银行 = N'" + textBoxKHYH.Text + "', 帐号 = N'" + textBoxZH.Text + "', 邮政编码 = N'" + textBoxYZBM.Text + "', 开始时间 = '" + dateTimePickerQYSJ.Value.ToShortDateString() + "', 负责人=N'" + textBoxFZR.Text.Trim() + "'";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 系统参数表 SET 公司宣传 = N'" + textBoxGSXC.Text + "', 质量目标1 = N'" + textBoxM1.Text + "', 质量目标2 = N'" + textBoxM2.Text + "', 质量目标3 = N'" + textBoxM3.Text + "', 质量目标4 = N'" + textBoxM4.Text + "', 管理员权限 = " + numericUpDownGLY.Value.ToString("f0") + ", 总经理权限 = " + numericUpDownZJL.Value.ToString("f0") + ", 职员权限 = " + numericUpDownZY.Value.ToString("f0") + ", 经理权限 = " + numericUpDownJL.Value.ToString("f0") + ", 业务员权限 = " + numericUpDownYWY.Value.ToString("f0") + "";
                sqlComm.ExecuteNonQuery();


                MessageBox.Show("系统参数修改完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            catch
            {
                //MessageBox.Show("系统参数修改错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                sqlConn.Close();
                this.Close();
            }

            
            
        }

        private void textBoxM_TextChanged(object sender, EventArgs e)
        {
            
            TextBox tb=(TextBox)sender;

            if (tb.Text.Length > 4)
                tb.Text = tb.Text.Substring(0, 4);

                
            

        }
    }
}