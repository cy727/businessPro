using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormZYDAWHCARD : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;


        private ClassGetInformation cGetInformation;

        
        public FormZYDAWHCARD()
        {
            InitializeComponent();
        }

        private void FormZYDAWHCARD_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            sqlConn.Open();

            sqlComm.CommandText = "SELECT ID, 岗位名称 FROM 岗位表";

            if (dSet.Tables.Contains("岗位表")) dSet.Tables.Remove("岗位表");
            sqlDA.Fill(dSet, "岗位表");

            comboBoxZW.DataSource = dSet.Tables["岗位表"];
            comboBoxZW.DisplayMember = "岗位名称";
            comboBoxZW.ValueMember = "ID";

            sqlComm.CommandText = "SELECT ID, 部门名称 FROM 部门表";

            if (dSet.Tables.Contains("部门表")) dSet.Tables.Remove("部门表");
            sqlDA.Fill(dSet, "部门表");

            comboBoxBM.DataSource = dSet.Tables["部门表"];
            comboBoxBM.DisplayMember = "部门名称";
            comboBoxBM.ValueMember = "ID";


            sqlConn.Close();
            initDataView();
        }

        private void initDataView()
        {
            //初始化列表
            sqlConn.Open();

            sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员编号, 职员表.职员姓名, 职员表.性别, 岗位表.岗位名称, 部门表.部门名称, 职员表.是否操作员, 职员表.是否业务员, 职员表.身份证号, 职员表.职员电话, 职员表.家庭地址, 职员表.岗位ID, 职员表.部门ID FROM 职员表 LEFT JOIN 岗位表 ON 职员表.岗位ID = 岗位表.ID INNER JOIN 部门表 ON 职员表.部门ID = 部门表.ID WHERE (职员表.BeActive = 1)  ORDER BY 职员表.职员编号";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");

            dataGridViewLB.DataSource = dSet.Tables["职员表"];
            dataGridViewLB.Columns[0].Visible = false;
            dataGridViewLB.Columns[11].Visible = false;
            dataGridViewLB.Columns[12].Visible = false;

            sqlConn.Close();


        }

        private void dataGridViewLB_SelectionChanged(object sender, EventArgs e)
        {
            int i;

            if (dataGridViewLB.RowCount < 1)
                return;

            if (dataGridViewLB.SelectedRows.Count < 1)
                return;

            textBoxBH.Text = dataGridViewLB.SelectedRows[0].Cells[1].Value.ToString();
            textBoxXM.Text = dataGridViewLB.SelectedRows[0].Cells[2].Value.ToString();
            comboBoxXB.Text = dataGridViewLB.SelectedRows[0].Cells[3].Value.ToString();
            comboBoxZW.Text = dataGridViewLB.SelectedRows[0].Cells[4].Value.ToString();
            comboBoxBM.Text = dataGridViewLB.SelectedRows[0].Cells[5].Value.ToString();
            textBoxSFZH.Text = dataGridViewLB.SelectedRows[0].Cells[8].Value.ToString();
            textBoxZYDH.Text = dataGridViewLB.SelectedRows[0].Cells[9].Value.ToString();
            textBoxJTDZ.Text = dataGridViewLB.SelectedRows[0].Cells[10].Value.ToString();

            try
            {
                i = Convert.ToInt32(dataGridViewLB.SelectedRows[0].Cells[6].Value);
            }
            catch
            {
                i = 0;
            }
            if (i == 1)
                checkBoxCZY.Checked = true;
            else
                checkBoxCZY.Checked = false;

            try
            {
                i = Convert.ToInt32(dataGridViewLB.SelectedRows[0].Cells[7].Value);
            }
            catch
            {
                i = 0;
            }
            if (i == 1)
                checkBoxYWY.Checked = true;
            else
                checkBoxYWY.Checked = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;

            if (textBoxBH.Text.Trim() == "")
            {
                MessageBox.Show("输入类型错误，请输入职员编号", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBoxXM.Text.Trim() == "")
            {
                MessageBox.Show("输入类型错误，请输入职员姓名", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                sqlComm.CommandText = "INSERT INTO 职员表 (职员编号, 职员姓名, 性别, 岗位ID, 部门ID, 是否操作员, 是否业务员, 身份证号, 职员电话, 家庭地址, BeActive, PASSWORD) VALUES ('" + textBoxBH.Text.Trim() + "', N'" + textBoxXM.Text.Trim() + "', N'" + comboBoxXB.Text.Trim() + "', " + comboBoxZW.SelectedValue.ToString() + "," + comboBoxBM.SelectedValue.ToString() + ", " + Convert.ToSingle(checkBoxCZY.Checked).ToString() + ", " + Convert.ToSingle(checkBoxYWY.Checked).ToString() + ", '" + textBoxSFZH.Text.Trim() + "', '" + textBoxZYDH.Text.Trim() + "', N'" + textBoxJTDZ.Text.Trim() + "', 1, N'')";
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
            MessageBox.Show("增加职员成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewLB.RowCount < 1)
                return;

            if (dataGridViewLB.SelectedRows.Count < 1)
                return;

            System.Data.SqlClient.SqlTransaction sqlta;

            if (textBoxBH.Text.Trim() == "")
            {
                MessageBox.Show("输入类型错误，请输入职员编号", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBoxXM.Text.Trim() == "")
            {
                MessageBox.Show("输入类型错误，请输入职员姓名", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                sqlComm.CommandText = "UPDATE 职员表 SET 职员编号 = '" + textBoxBH.Text.Trim() + "', 职员姓名 = N'" + textBoxXM.Text.Trim() + "', 性别 = N'" + comboBoxXB.Text.Trim() + "', 岗位ID = " + comboBoxZW.SelectedValue.ToString() + ", 部门ID = " + comboBoxBM.SelectedValue.ToString() + ", 是否操作员 = " + Convert.ToSingle(checkBoxCZY.Checked).ToString() + ", 是否业务员 = " + Convert.ToSingle(checkBoxYWY.Checked).ToString() + ", 身份证号 = '" + textBoxSFZH.Text.Trim() + "', 职员电话 = '" + textBoxZYDH.Text.Trim() + "', 家庭地址 = N'" + textBoxJTDZ.Text.Trim() + "' WHERE (ID = " + dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString() + ")";
                
                
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
            MessageBox.Show("修改职员成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlTransaction sqlta;

            if (dataGridViewLB.RowCount < 1)
                return;

            if (dataGridViewLB.SelectedRows.Count < 1)
                return;


            if (MessageBox.Show("是否删除职员？该过程不可回退", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
            {
                return;
            }


            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                sqlComm.CommandText = "UPDATE 职员表 SET BeActive = 0 WHERE (ID = " + dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString() + ")";
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
            MessageBox.Show("删除职员成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();
        }

        private void 清空用户密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否将所选用户密码清空？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
            {
                return;
            }
            System.Data.SqlClient.SqlTransaction sqlta;
            int i;
            
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                for (i = 0; i < dataGridViewLB.SelectedRows.Count; i++)
                {
                    if (dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString() == "")
                        continue;
                    sqlComm.CommandText = "UPDATE 职员表 SET PASSWORD = N'' WHERE (ID = " + dataGridViewLB.SelectedRows[i].Cells[0].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                sqlta.Commit();
                MessageBox.Show("密码清空完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            
        }

        private void 重置用户登录状态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否将所选用户状态设置为未登录？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
            {
                return;
            }
            System.Data.SqlClient.SqlTransaction sqlta;
            int i;

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                for (i = 0; i < dataGridViewLB.SelectedRows.Count; i++)
                {
                    if (dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString() == "")
                        continue;
                    sqlComm.CommandText = "UPDATE 职员表 SET 登录状态 = NULL WHERE (ID = " + dataGridViewLB.SelectedRows[i].Cells[0].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }

                sqlta.Commit();
                MessageBox.Show("状态修正完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }


    }
}
