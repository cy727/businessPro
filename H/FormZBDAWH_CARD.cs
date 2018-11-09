using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormZBDAWH_CARD : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";

        private ClassGetInformation cGetInformation;
        public int iStyle = 0;
        public DataTable dt;

        
        public FormZBDAWH_CARD()
        {
            InitializeComponent();
        }

        private void FormZBDAWH_CARD_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            dataGridViewDA.DataSource = dt;
            dataGridViewDA.AllowUserToDeleteRows = false;
            dataGridViewDA.Columns[0].Visible = false;

            switch (iStyle)
            {
                case 0://增加
                    btnAccept.Text = "增加";
                    dataGridViewDA.AllowUserToAddRows = true;
                    break;
                case 1://修改
                    btnAccept.Text = "修改";
                    dataGridViewDA.AllowUserToAddRows = false;
                    break;
                default:
                    break;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            int i;
            int i1 = 0, i2 = 0;
            string strDateSYS = "";
            System.Data.SqlClient.SqlTransaction sqlta;

            if (!countAmount())
            {
                MessageBox.Show("输入类型错误", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            switch (iStyle)
            {
                case 0://增加
                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {

                       for (i = 0; i < dataGridViewDA.Rows.Count; i++)
                        {
                            if (dataGridViewDA.Rows[i].IsNewRow)
                                continue;

                            i1 = 0;

                            if (dataGridViewDA.Rows[i].Cells[4].Value.ToString() != "")
                                i1 = Convert.ToInt32(dataGridViewDA.Rows[i].Cells[4].Value);

                            sqlComm.CommandText = "INSERT INTO 账簿表 (账簿编号, 账簿名称, 助记码, 是否可支付, 提示信息, BeActive) VALUES (N'" + dataGridViewDA.Rows[i].Cells[1].Value.ToString() + "', N'" + dataGridViewDA.Rows[i].Cells[2].Value.ToString() + "', N'" + dataGridViewDA.Rows[i].Cells[3].Value.ToString() + "', " + i1.ToString() + ", N'" + dataGridViewDA.Rows[i].Cells[5].Value.ToString() + "',1)";
                            sqlComm.ExecuteNonQuery();
                        }


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
                    MessageBox.Show("增加成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    break;
                case 1://修改

                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {

                        for (i = 0; i < dataGridViewDA.Rows.Count; i++)
                        {
                            if (dataGridViewDA.Rows[i].IsNewRow)
                                continue;

                            i1 = 0;


                            if (dataGridViewDA.Rows[i].Cells[4].Value.ToString() != "")
                                i1 = Convert.ToInt32(dataGridViewDA.Rows[i].Cells[4].Value);


                            sqlComm.CommandText = "UPDATE 账簿表 SET 账簿编号 = N'" + dataGridViewDA.Rows[i].Cells[1].Value.ToString() + "', 账簿名称 = N'" + dataGridViewDA.Rows[i].Cells[2].Value.ToString() + "', 助记码 = N'" + dataGridViewDA.Rows[i].Cells[3].Value.ToString() + "', 是否可支付 = " + i1.ToString() + ", 提示信息 = N'" + dataGridViewDA.Rows[i].Cells[5].Value.ToString() + "' WHERE (ID = " + dataGridViewDA.Rows[i].Cells[0].Value.ToString() + ")";
                            sqlComm.ExecuteNonQuery();
                        }


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
                    MessageBox.Show("修改成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    break;
                default:
                    break;
            }
 
        }

        private void dataGridViewDA_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("输入类型错误", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool countAmount()
        {
            bool bCheck = true;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDA);

            for (int i = 0; i < dataGridViewDA.Rows.Count; i++)
            {
                if (dataGridViewDA.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewDA.Rows[i].Cells[1].Value.ToString() == "")
                {
                    dataGridViewDA.Rows[i].Cells[1].ErrorText = "输入账簿编号";
                    bCheck = false;
                }
                if (dataGridViewDA.Rows[i].Cells[2].Value.ToString() == "")
                {
                    dataGridViewDA.Rows[i].Cells[2].ErrorText = "输入账簿名称";
                    bCheck = false;
                }
            }

            return bCheck;
        }
    }
}