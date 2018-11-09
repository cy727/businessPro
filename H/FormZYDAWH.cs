using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormZYDAWH : Form
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
        
        public FormZYDAWH()
        {
            InitializeComponent();
        }

        private void FormZYDAWH_Load(object sender, EventArgs e)
        {
            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            initDataView();
        }

        private void initDataView()
        {
            //初始化列表
            sqlConn.Open();

            sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员编号, 职员表.职员姓名, 职员表.性别, 岗位表.岗位名称, 部门表.部门名称, 职员表.是否操作员, 职员表.是否业务员, 职员表.身份证号, 职员表.职员电话, 职员表.家庭地址, 职员表.岗位ID, 职员表.部门ID FROM 职员表 LEFT JOIN 岗位表 ON 职员表.岗位ID = 岗位表.ID INNER JOIN 部门表 ON 职员表.部门ID = 部门表.ID WHERE (职员表.BeActive = 1)  ORDER BY 职员表.职员编号";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");

            sqlComm.CommandText = "SELECT 职员表.ID, 职员表.职员编号, 职员表.职员姓名, 职员表.性别, 岗位表.岗位名称, 部门表.部门名称, 职员表.是否操作员, 职员表.是否业务员, 职员表.身份证号, 职员表.职员电话, 职员表.家庭地址, 职员表.岗位ID, 职员表.部门ID FROM 职员表 LEFT JOIN 岗位表 ON 职员表.岗位ID = 岗位表.ID INNER JOIN 部门表 ON 职员表.部门ID = 部门表.ID WHERE (职员表.BeActive = 1) AND (职员表.ID = 0) ORDER BY 职员表.职员编号";

            if (dSet.Tables.Contains("职员表1")) dSet.Tables.Remove("职员表1");
            sqlDA.Fill(dSet, "职员表1");

            dataGridViewDJMX.DataSource = dSet.Tables["职员表"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[12].Visible = false;

            sqlConn.Close();


        }

        private void ToolStripButtonADD_Click(object sender, EventArgs e)
        {
            dSet.Tables["职员表1"].Clear();
            DataTable dt = dSet.Tables["职员表1"];

            FormZYDAWH_CARD frmZYDAWH_CARD = new FormZYDAWH_CARD();
            frmZYDAWH_CARD.strConn = strConn;
            frmZYDAWH_CARD.dt = dt;
            frmZYDAWH_CARD.iStyle = 0;

            frmZYDAWH_CARD.ShowDialog();
            initDataView();
        }

        private void toolStripButtonEDIT_Click(object sender, EventArgs e)
        {
            object[] oT = new object[dataGridViewDJMX.ColumnCount];

            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("请选择要修改的职员", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dSet.Tables["职员表1"].Clear();
            DataTable dt = dSet.Tables["职员表1"];

            for (int i = dataGridViewDJMX.SelectedRows.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < oT.Length; j++)
                    oT[j] = dataGridViewDJMX.SelectedRows[i].Cells[j].Value;
                dt.Rows.Add(oT);
            }

            FormZYDAWH_CARD frmZYDAWH_CARD = new FormZYDAWH_CARD();
            frmZYDAWH_CARD.strConn = strConn;
            frmZYDAWH_CARD.dt = dt;
            frmZYDAWH_CARD.iStyle = 1;

            frmZYDAWH_CARD.ShowDialog();
            initDataView();
        }

        private void toolStripButtonDEL_Click(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("请选择要删除的职员", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (MessageBox.Show("是否删除所选内容？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            int i;
            System.Data.SqlClient.SqlTransaction sqlta;

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                for (i = 0; i < dataGridViewDJMX.SelectedRows.Count; i++)
                {
                    if (dataGridViewDJMX.SelectedRows[i].IsNewRow)
                        continue;

                    sqlComm.CommandText = "UPDATE 职员表 SET BeActive = 0 WHERE (ID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
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
            MessageBox.Show("删除完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void toolStripButtonPASS_Click(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("请选择要修改密码的职员", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FormZYChangePass frmZYChangePass = new FormZYChangePass();
            frmZYChangePass.strConn = strConn;
            frmZYChangePass.iZYID = Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString());
            frmZYChangePass.strZYName= dataGridViewDJMX.SelectedRows[0].Cells[2].Value.ToString();

            frmZYChangePass.ShowDialog();


        }

        private void dataGridViewDJMX_DoubleClick(object sender, EventArgs e)
        {
            toolStripButtonEDIT_Click(null,null);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");

            string strT = "职员档案维护;当前日期：" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");

            string strT = "职员档案维护;当前日期：" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}