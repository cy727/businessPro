using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormZWDAWH : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";

        public FormZWDAWH()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormZWDAWH_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            initDataView();
        }

        private void initDataView()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ID, 岗位名称, 岗位编号, 权限 FROM 岗位表";
            if (dSet.Tables.Contains("岗位表")) dSet.Tables.Remove("岗位表");
            sqlDA.Fill(dSet, "岗位表");
            sqlConn.Close();

            dataGridViewLB.DataSource = dSet.Tables["岗位表"];

            dataGridViewLB.Columns[0].Visible = false;
            dataGridViewLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewLB.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void dataGridViewLB_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewLB.RowCount < 1)
                return;

            if (dataGridViewLB.SelectedRows.Count < 1)
                return;

            textBoxBH.Text = dataGridViewLB.SelectedRows[0].Cells[2].Value.ToString();
            textBoxMC.Text = dataGridViewLB.SelectedRows[0].Cells[1].Value.ToString();
            try
            {
                numericUpDownJB.Value = Convert.ToDecimal(dataGridViewLB.SelectedRows[0].Cells[3].Value.ToString());
            }
            catch
            {
                numericUpDownJB.Value = Convert.ToDecimal("1");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;

            if (textBoxBH.Text.Trim() == "")
            {
                MessageBox.Show("输入类型错误，请输入职位编号", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBoxMC.Text.Trim() == "")
            {
                MessageBox.Show("输入类型错误，请输入职位名称", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                sqlComm.CommandText = "INSERT INTO 岗位表 (岗位名称, 岗位编号, 权限) VALUES (N'" + textBoxMC.Text.Trim() + "', '" + textBoxBH.Text.Trim() + "', "+numericUpDownJB.Value.ToString()+")";
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
            MessageBox.Show("增加成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;

            if (dataGridViewLB.RowCount < 1)
                return;

            if (dataGridViewLB.SelectedRows.Count < 1)
                return;


            if (textBoxBH.Text.Trim() == "")
            {
                MessageBox.Show("输入类型错误，请输入职位编号", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBoxMC.Text.Trim() == "")
            {
                MessageBox.Show("输入类型错误，请输入职位名称", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString() == "1")
            {
                MessageBox.Show("管理员岗位不可修改", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                sqlComm.CommandText = "UPDATE 岗位表 SET 岗位名称 = N'" + textBoxMC.Text.Trim() + "', 岗位编号 = '" + textBoxBH.Text.Trim() + "', 权限 = " + numericUpDownJB.Value.ToString() + " WHERE (ID = " + dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString() + ")";
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
            MessageBox.Show("修改职位成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;

            if (dataGridViewLB.RowCount < 1)
                return;

            if (dataGridViewLB.SelectedRows.Count < 1)
                return;

            if (dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString() == "1")
            {
                MessageBox.Show("管理员岗位不可删除", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (MessageBox.Show("是否删除职位？该过程不可回退", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
            {
                return;
            }


            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                sqlComm.CommandText = "DELETE FROM 岗位表 WHERE (ID = " + dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 模块权限表 WHERE (岗位ID = " + dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE 职员表 SET 岗位ID = 0 WHERE (岗位ID = " + dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString() + ")";
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
            MessageBox.Show("删除职位成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();

        }
    }
}
