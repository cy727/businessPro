using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormDQDAWH : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";

        public FormDQDAWH()
        {
            InitializeComponent();
        }

        private void FormDQDAWH_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            initDataView();

        }

        private void initDataView()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ID, 地区, 编号 FROM 地区表 ORDER BY 编号";
            if (dSet.Tables.Contains("地区表")) dSet.Tables.Remove("地区表");
            sqlDA.Fill(dSet, "地区表");
            sqlConn.Close();

            dataGridViewDQLB.DataSource = dSet.Tables["地区表"];

            dataGridViewDQLB.Columns[0].Visible = false;
            dataGridViewDQLB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDQLB.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void dataGridViewDQLB_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewDQLB.RowCount < 1)
                return;

            if (dataGridViewDQLB.SelectedRows.Count < 1)
                return;

            textBoxDQBH.Text = dataGridViewDQLB.SelectedRows[0].Cells[2].Value.ToString();
            textBoxDQMC.Text = dataGridViewDQLB.SelectedRows[0].Cells[1].Value.ToString();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;

            if (textBoxDQBH.Text.Trim() == "")
            {
                MessageBox.Show("输入类型错误，请输入地区编号", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBoxDQMC.Text.Trim() == "")
            {
                MessageBox.Show("输入类型错误，请输入地区名称", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sqlConn.Open();

            //查重
            sqlComm.CommandText = "SELECT ID, 地区, 编号 FROM 地区表 WHERE (地区 = N'"+textBoxDQMC.Text.Trim()+"')";
            sqldr = sqlComm.ExecuteReader();

            if (sqldr.HasRows)
            {
                sqldr.Read();
                MessageBox.Show("地区：" + textBoxDQMC.Text.Trim() + " 已经存在.");
                sqldr.Close();
                sqlConn.Close();
                return;
            }
            sqldr.Close();


            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                sqlComm.CommandText = "INSERT INTO 地区表 (地区, 编号) VALUES (N'" + textBoxDQMC.Text.Trim() + "', N'" + textBoxDQBH.Text.Trim() + "')";
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
            MessageBox.Show("增加地区成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;

            if (dataGridViewDQLB.RowCount < 1)
                return;

            if (dataGridViewDQLB.SelectedRows.Count < 1)
                return;


            if (textBoxDQBH.Text.Trim() == "")
            {
                MessageBox.Show("输入类型错误，请输入地区编号", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBoxDQMC.Text.Trim() == "")
            {
                MessageBox.Show("输入类型错误，请输入地区名称", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sqlConn.Open();

            //查重
            sqlComm.CommandText = "SELECT ID, 地区, 编号 FROM 地区表 WHERE (地区 = N'" + textBoxDQMC.Text.Trim() + "') AND (ID <> " + dataGridViewDQLB.SelectedRows[0].Cells[0].Value.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();

            if (sqldr.HasRows)
            {
                sqldr.Read();
                MessageBox.Show("地区：" + textBoxDQMC.Text.Trim() + " 已经存在.");
                sqldr.Close();
                sqlConn.Close();
                return;
            }
            sqldr.Close();

            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                sqlComm.CommandText = "UPDATE 地区表 SET 地区 = N'" + textBoxDQMC.Text.Trim() + "', 编号 = N'" + textBoxDQBH.Text.Trim() + "'WHERE (ID = " + dataGridViewDQLB.SelectedRows[0].Cells[0].Value.ToString() + ")";
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
            MessageBox.Show("修改地区成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;

            if (dataGridViewDQLB.RowCount < 1)
                return;

            if (dataGridViewDQLB.SelectedRows.Count < 1)
                return;


            if (MessageBox.Show("是否删除地区？该过程不可回退", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information)!=DialogResult.Yes)
            {
                return;
            }


            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                sqlComm.CommandText = "DELETE FROM 地区表 WHERE (ID = " + dataGridViewDQLB.SelectedRows[0].Cells[0].Value.ToString() + ")";
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
            MessageBox.Show("删除地区成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();

        }

        private void textBoxDQBH_Validating(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex rExpression = new System.Text.RegularExpressions.Regex(@"^\d{4}$");

            textBoxDQBH.Text = textBoxDQBH.Text.Trim();
            if (rExpression.IsMatch(textBoxDQBH.Text) || textBoxDQBH.Text == "")
            {
                this.errorProviderM.Clear();
            }
            else
            {
                this.errorProviderM.SetError(this.textBoxDQBH, "输入正确的编码，四位数字，例如：0100");
                e.Cancel = true;
            }
        }
    }
}
