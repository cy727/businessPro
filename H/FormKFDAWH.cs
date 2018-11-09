using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKFDAWH : Form
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

        public FormKFDAWH()
        {
            InitializeComponent();
        }

        private void FormKFDAWH_Load(object sender, EventArgs e)
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

            sqlComm.CommandText = "SELECT ID, 库房编号, 库房名称, 助记码, 简称 FROM 库房表 WHERE (BeActive = 1)";

            if (dSet.Tables.Contains("库房表")) dSet.Tables.Remove("库房表");
            sqlDA.Fill(dSet, "库房表");

            sqlComm.CommandText = "SELECT ID, 库房编号, 库房名称, 助记码, 简称 FROM 库房表 WHERE (BeActive = 1) AND (ID = 0)";

            if (dSet.Tables.Contains("库房表1")) dSet.Tables.Remove("库房表1");
            sqlDA.Fill(dSet, "库房表1");

            dataGridViewDJMX.DataSource = dSet.Tables["库房表"];
            dataGridViewDJMX.Columns[0].Visible = false;

            sqlConn.Close();


        }

        private void ToolStripButtonADD_Click(object sender, EventArgs e)
        {
            dSet.Tables["库房表1"].Clear();
            DataTable dt = dSet.Tables["库房表1"];

            FormKFDAWH_CARD frmKFDAWH_CARD = new FormKFDAWH_CARD();
            frmKFDAWH_CARD.strConn = strConn;
            frmKFDAWH_CARD.dt = dt;
            frmKFDAWH_CARD.iStyle = 0;

            frmKFDAWH_CARD.ShowDialog();
            initDataView();
        }

        private void toolStripButtonEDIT_Click(object sender, EventArgs e)
        {
            object[] oT = new object[dataGridViewDJMX.ColumnCount];

            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("请选择要修改的库房", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dSet.Tables["库房表1"].Clear();
            DataTable dt = dSet.Tables["库房表1"];

            for (int i = dataGridViewDJMX.SelectedRows.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < oT.Length; j++)
                    oT[j] = dataGridViewDJMX.SelectedRows[i].Cells[j].Value;
                dt.Rows.Add(oT);
            }

            FormKFDAWH_CARD frmKFDAWH_CARD = new FormKFDAWH_CARD();
            frmKFDAWH_CARD.strConn = strConn;
            frmKFDAWH_CARD.dt = dt;
            frmKFDAWH_CARD.iStyle = 1;

            frmKFDAWH_CARD.ShowDialog();
            initDataView();
        }

        private void toolStripButtonDEL_Click(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                MessageBox.Show("请选择要删除的库房", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int i;
            sqlConn.Open();
            sqlComm.CommandText = "SELECT SUM(库存数量) FROM 库存表 ";
            for (i = 0; i < dataGridViewDJMX.SelectedRows.Count; i++)
            {
                if (dataGridViewDJMX.SelectedRows[i].IsNewRow)
                    continue;

                if (i == 0)
                    sqlComm.CommandText += " WHERE (库房ID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
                else
                    sqlComm.CommandText += " OR (库房ID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
            }
            sqldr = sqlComm.ExecuteReader();
            int iC=0;
            while (sqldr.Read())
            {
                if (sqldr.GetValue(0).ToString()!="")
                    iC = int.Parse(sqldr.GetValue(0).ToString());
                break;
            }
            sqldr.Close();

            if (iC == 0)
            {
                if (MessageBox.Show("是否删除所选内容？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    sqlConn.Close();
                    return;
                }
            }
            else
            {
                if (MessageBox.Show("是否删除所选内容？库房中尚有"+iC.ToString()+"个库存", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    sqlConn.Close();
                    return;
                }

            }

            
            System.Data.SqlClient.SqlTransaction sqlta;


            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                for (i = 0; i < dataGridViewDJMX.SelectedRows.Count; i++)
                {
                    if (dataGridViewDJMX.SelectedRows[i].IsNewRow)
                        continue;

                    sqlComm.CommandText = "UPDATE 库房表 SET BeActive = 0 WHERE (ID = " + dataGridViewDJMX.SelectedRows[i].Cells[0].Value.ToString() + ")";
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

        private void dataGridViewDJMX_DoubleClick(object sender, EventArgs e)
        {
            toolStripButtonEDIT_Click(null, null);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");

            string strT = "库房档案维护;当前日期：" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");

            string strT = "库房档案维护;当前日期：" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}