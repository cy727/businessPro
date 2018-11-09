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
                sqlComm.CommandText = "SELECT ��˾��, ��ַ, �绰, ����, ˰��, ��������, �ʺ�, ��������, ��ʼʱ��, ������ FROM ϵͳ������";
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

                sqlComm.CommandText = "SELECT ��˾����, ����Ŀ��1, ����Ŀ��2, ����Ŀ��3, ����Ŀ��4, ����ԱȨ��, �ܾ���Ȩ��, ְԱȨ��, ����Ȩ��, ҵ��ԱȨ�� FROM ϵͳ������";
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
                sqlComm.CommandText = "UPDATE ϵͳ������ SET ��˾�� = N'" + textBoxGSMC.Text + "', ��ַ = N'" + textBoxGSDZ.Text + "', �绰 = N'" + textBoxGSDH.Text + "', ���� = N'" + textBoxGSCZ.Text + "', ˰�� = N'" + textBoxSH.Text + "', �������� = N'" + textBoxKHYH.Text + "', �ʺ� = N'" + textBoxZH.Text + "', �������� = N'" + textBoxYZBM.Text + "', ��ʼʱ�� = '" + dateTimePickerQYSJ.Value.ToShortDateString() + "', ������=N'" + textBoxFZR.Text.Trim() + "'";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE ϵͳ������ SET ��˾���� = N'" + textBoxGSXC.Text + "', ����Ŀ��1 = N'" + textBoxM1.Text + "', ����Ŀ��2 = N'" + textBoxM2.Text + "', ����Ŀ��3 = N'" + textBoxM3.Text + "', ����Ŀ��4 = N'" + textBoxM4.Text + "', ����ԱȨ�� = " + numericUpDownGLY.Value.ToString("f0") + ", �ܾ���Ȩ�� = " + numericUpDownZJL.Value.ToString("f0") + ", ְԱȨ�� = " + numericUpDownZY.Value.ToString("f0") + ", ����Ȩ�� = " + numericUpDownJL.Value.ToString("f0") + ", ҵ��ԱȨ�� = " + numericUpDownYWY.Value.ToString("f0") + "";
                sqlComm.ExecuteNonQuery();


                MessageBox.Show("ϵͳ�����޸����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            catch
            {
                //MessageBox.Show("ϵͳ�����޸Ĵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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