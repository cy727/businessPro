using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormZYDAWH_CARD : Form
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
        
        public FormZYDAWH_CARD()
        {
            InitializeComponent();
        }

        private void FormZYDAWH_CARD_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            dataGridViewDA.DataSource = dt;
            dataGridViewDA.AllowUserToDeleteRows = false;
            dataGridViewDA.Columns[0].Visible = false;
            dataGridViewDA.Columns[11].Visible = false;
            dataGridViewDA.Columns[12].Visible = false;

            dataGridViewDA.Columns[4].ReadOnly = false;
            dataGridViewDA.Columns[5].ReadOnly = false;

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
                    dataGridViewDA.Rows[i].Cells[1].ErrorText = "输入职员编号";
                    bCheck = false;
                }
                if (dataGridViewDA.Rows[i].Cells[2].Value.ToString() == "")
                {
                    dataGridViewDA.Rows[i].Cells[2].ErrorText = "输入职员姓名";
                    bCheck = false;
                }

                if (dataGridViewDA.Rows[i].Cells[11].Value.ToString() == "")
                    dataGridViewDA.Rows[i].Cells[11].Value = 0 ;

                if (dataGridViewDA.Rows[i].Cells[12].Value.ToString() == "")
                    dataGridViewDA.Rows[i].Cells[12].Value = 0;

                if (dataGridViewDA.Rows[i].Cells[11].Value.ToString() == "0")
                {
                    dataGridViewDA.Rows[i].Cells[4].ErrorText = "输入职员岗位";
                    bCheck = false;
                }

                if (dataGridViewDA.Rows[i].Cells[12].Value.ToString() == "0")
                {
                    dataGridViewDA.Rows[i].Cells[5].ErrorText = "输入职员部门";
                    bCheck = false;
                }
            }

            return bCheck;
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
                            i2 = 0;

                            if (dataGridViewDA.Rows[i].Cells[6].Value.ToString() != "")
                                i1 = Convert.ToInt32(dataGridViewDA.Rows[i].Cells[6].Value);

                            if (dataGridViewDA.Rows[i].Cells[7].Value.ToString() != "")
                                i2 = Convert.ToInt32(dataGridViewDA.Rows[i].Cells[7].Value);

                            sqlComm.CommandText = "INSERT INTO 职员表 (职员编号, 职员姓名, 性别, 岗位ID, 部门ID, 是否操作员, 是否业务员, 身份证号, 职员电话, 家庭地址, BeActive) VALUES ('" + dataGridViewDA.Rows[i].Cells[1].Value.ToString() + "', N'" + dataGridViewDA.Rows[i].Cells[2].Value.ToString() + "', N'" + dataGridViewDA.Rows[i].Cells[3].Value.ToString() + "', " + dataGridViewDA.Rows[i].Cells[11].Value.ToString() + ", " + dataGridViewDA.Rows[i].Cells[12].Value.ToString() + ", " + i1.ToString() + ", " + i2.ToString() + ", '" + dataGridViewDA.Rows[i].Cells[8].Value.ToString() + "', '" + dataGridViewDA.Rows[i].Cells[9].Value.ToString() + "', N'" + dataGridViewDA.Rows[i].Cells[10].Value.ToString() + "', 1)";
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
                            i2 = 0;

                            if (dataGridViewDA.Rows[i].Cells[6].Value.ToString() != "")
                                i1 = Convert.ToInt32(dataGridViewDA.Rows[i].Cells[6].Value);

                            if (dataGridViewDA.Rows[i].Cells[7].Value.ToString() != "")
                                i2 = Convert.ToInt32(dataGridViewDA.Rows[i].Cells[7].Value);

                            sqlComm.CommandText = "UPDATE 职员表 SET 职员编号 = '" + dataGridViewDA.Rows[i].Cells[1].Value.ToString() + "', 职员姓名 = N'" + dataGridViewDA.Rows[i].Cells[2].Value.ToString() + "', 性别 = N'" + dataGridViewDA.Rows[i].Cells[3].Value.ToString() + "', 岗位ID = " + dataGridViewDA.Rows[i].Cells[11].Value.ToString() + ", 部门ID = " + dataGridViewDA.Rows[i].Cells[12].Value.ToString() + ", 是否操作员 = " + i1.ToString() + ", 是否业务员 = " + i2.ToString() + ", 身份证号 = '" + dataGridViewDA.Rows[i].Cells[8].Value.ToString() + "', 职员电话 = '" + dataGridViewDA.Rows[i].Cells[9].Value.ToString() + "', 家庭地址 = N'" + dataGridViewDA.Rows[i].Cells[10].Value.ToString() + "' WHERE (ID = " + dataGridViewDA.Rows[i].Cells[0].Value.ToString() + ")";
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

        private void dataGridViewDA_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 4) //岗位
            {
                if (cGetInformation.getGWInformation(1, "") == 0) //失败
                {
                    return;
                }
                else
                {
                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDA);
                    dataGridViewDA.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iGWNumber;
                    dataGridViewDA.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strGWName;
                    dataGridViewDA.EndEdit();
                }
            }

            if (e.ColumnIndex == 5) //部门
            {
                if (cGetInformation.getBMInformation(1, "") == 0) //失败
                {
                    return;
                }
                else
                {
                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDA);
                    dataGridViewDA.Rows[e.RowIndex].Cells[12].Value = cGetInformation.iBMNumber;
                    dataGridViewDA.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strBMName;
                    dataGridViewDA.EndEdit();
                }
            }
            

        }
    }
}