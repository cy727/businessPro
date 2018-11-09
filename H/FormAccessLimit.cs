using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormAccessLimit : Form
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

        public FormAccessLimit()
        {
            InitializeComponent();
        }

        private void FormAccessLimit_Load(object sender, EventArgs e)
        {
            int i;


            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            sqlConn.Open();

            sqlComm.CommandText = "SELECT ID, 岗位名称, 权限 FROM 岗位表 WHERE (ID <> 1) ORDER BY 权限 DESC";

            if (dSet.Tables.Contains("岗位表")) dSet.Tables.Remove("岗位表");
            sqlDA.Fill(dSet, "岗位表");

            sqlComm.CommandText = "SELECT ID, 模块名称, 模块代码, 权限显示 FROM 模块表 WHERE (权限显示 = 1)";

            if (dSet.Tables.Contains("模块表")) dSet.Tables.Remove("模块表");
            sqlDA.Fill(dSet, "模块表");

            sqlComm.CommandText = "SELECT ID, 岗位ID, 模块ID, 权限 FROM 模块权限表";
            if (dSet.Tables.Contains("模块权限表")) dSet.Tables.Remove("模块权限表");
            sqlDA.Fill(dSet, "模块权限表");

            sqlConn.Close();
            dataGridViewGW.DataSource = dSet.Tables["岗位表"];

            dataGridViewGW.Columns[0].Visible = false;
            dataGridViewGW.Columns[2].Visible = false;

            dataGridViewMK.DataSource = dSet.Tables["模块表"];
            dataGridViewMK.Columns[0].Visible = false;

            dataGridViewMK.Columns[1].ReadOnly = true;
            dataGridViewMK.Columns[2].ReadOnly = true;
            //dataGridViewMK.Columns[3].ReadOnly = true;

            dataGridViewGW_SelectionChanged(null,null);
        }

        private void dataGridViewGW_SelectionChanged(object sender, EventArgs e)
        {
            int i;
            if (dataGridViewGW.RowCount < 1)
                return;

            if (dataGridViewGW.SelectedRows.Count < 1)
                return;

            sqlConn.Open();
            
            for(i=0;i<dataGridViewMK.RowCount;i++)
            {
                DataRow[] dtTemp;
                dtTemp = dSet.Tables["模块权限表"].Select("岗位ID="+dataGridViewGW.SelectedRows[0].Cells[0].Value.ToString()+" AND 模块ID="+dataGridViewMK.Rows[i].Cells[0].Value.ToString());

                if(dtTemp.Length<1)
                    dSet.Tables["模块表"].Rows[i][3]=0;
                else
                    dSet.Tables["模块表"].Rows[i][3]=1;
            }

            sqlConn.Close();
        }

        private void toolStripButtonDEL_Click(object sender, EventArgs e)
        {
            int i;

            for (i = 0; i < dataGridViewMK.SelectedRows.Count; i++)
            {
                dataGridViewMK.SelectedRows[i].Cells[3].Value = 0;
            }
        }

        private void toolStripButtonEDIT_Click(object sender, EventArgs e)
        {
            int i;

            for (i = 0; i < dataGridViewMK.SelectedRows.Count; i++)
            {
                dataGridViewMK.SelectedRows[i].Cells[3].Value = 1;
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i;

            toolStrip.Focus();

            if (dataGridViewGW.RowCount < 1)
                return;

            if (dataGridViewGW.SelectedRows.Count < 1)
                return;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();

            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                sqlComm.CommandText = "DELETE FROM 模块权限表 WHERE (岗位ID = "+dataGridViewGW.SelectedRows[0].Cells[0].Value.ToString()+")";
                sqlComm.ExecuteNonQuery();

                for (i = 0; i < dataGridViewMK.Rows.Count; i++)
                {
                    if (dataGridViewMK.Rows[i].Cells[3].Value.ToString() == "")
                        continue;

                    if (bool.Parse(dataGridViewMK.Rows[i].Cells[3].Value.ToString()))
                    {
                        sqlComm.CommandText = "INSERT INTO 模块权限表 (岗位ID, 模块ID, 权限) VALUES (" + dataGridViewGW.SelectedRows[0].Cells[0].Value.ToString() + ", " + dataGridViewMK.Rows[i].Cells[0].Value.ToString() + ", 1)";
                        sqlComm.ExecuteNonQuery();
                    }
                }
                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                saveToolStripButton.Enabled = true;
                return;
            }
            finally
            {
                sqlConn.Close();
            }

            MessageBox.Show(dataGridViewGW.SelectedRows[0].Cells[1].Value.ToString()+"权限设置完毕");

            sqlConn.Open();

            sqlComm.CommandText = "SELECT ID, 岗位ID, 模块ID, 权限 FROM 模块权限表";
            if (dSet.Tables.Contains("模块权限表")) dSet.Tables.Remove("模块权限表");
            sqlDA.Fill(dSet, "模块权限表");

            sqlConn.Close();

            
        }

        private void toolStripButtonPASS_Click(object sender, EventArgs e)
        {
            FormChangeLimitAccess frmChangeLimitAccess = new FormChangeLimitAccess();
            frmChangeLimitAccess.strConn = strConn;
            frmChangeLimitAccess.iZYID = intUserID;
            frmChangeLimitAccess.strZYName = strUserName;

            frmChangeLimitAccess.ShowDialog();
        }
    }
}
