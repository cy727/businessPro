using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormWLDWYEDJ : Form
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
        
        public FormWLDWYEDJ()
        {
            InitializeComponent();
        }

        private void FormWLDWYEDJ_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            this.Top = 1;
            this.Left = 1;

            initDataView();
        }

        private void initDataView()
        {
            //初始化列表
            sqlConn.Open();

            sqlComm.CommandText = "SELECT ID, 单位编号, 单位名称, 应付账款, 应收账款, 应付账款 AS 增加应付账款, 应收账款  AS 增加应收账款 FROM 单位表 WHERE (BeActive = 1)  ORDER BY 单位编号";

            if (dSet.Tables.Contains("单位表")) dSet.Tables.Remove("单位表");
            sqlDA.Fill(dSet, "单位表");
            sqlConn.Close();

            dataGridViewDJMX.DataSource = dSet.Tables["单位表"];
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[1].ReadOnly = true;
            dataGridViewDJMX.Columns[2].ReadOnly = true;
            dataGridViewDJMX.Columns[3].ReadOnly = true;
            dataGridViewDJMX.Columns[4].ReadOnly = true;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            for (int i = 0; i < dataGridViewDJMX.RowCount; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;
                if (dataGridViewDJMX.Rows[i].Cells[3].Value.ToString()=="")
                    dataGridViewDJMX.Rows[i].Cells[3].Value = 0;
                if (dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[4].Value = 0;
                dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                dataGridViewDJMX.Rows[i].Cells[6].Value = 0;
            }
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void toolStripButtonEDIT_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("是否进行修改？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            int i;
            System.Data.SqlClient.SqlTransaction sqlta;
            decimal dt1 = 0, dt2 = 0;

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    if (dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() == "")
                        dataGridViewDJMX.Rows[i].Cells[5].Value = 0;
                    if (dataGridViewDJMX.Rows[i].Cells[6].Value.ToString() == "")
                        dataGridViewDJMX.Rows[i].Cells[6].Value = 0;

                    dt1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                    if (dt1 != 0)
                    {
                        dt1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[3].Value);
                        sqlComm.CommandText = "UPDATE 单位表 SET 应付账款 = "+dt1.ToString()+" WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                        sqlComm.ExecuteNonQuery();
                    }

                    dt2 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);
                    if (dt2 != 0)
                    {
                        dt2 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);
                        sqlComm.CommandText = "UPDATE 单位表 SET 应收账款 = " + dt2.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                        sqlComm.ExecuteNonQuery();
                    }


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
            MessageBox.Show("登记完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initDataView();

        }

        private void dataGridViewDJMX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("输入类型错误", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");

            string strT = "往来单位余额登记;当前日期：" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            strDT = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");

            string strT = "往来单位余额登记;当前日期：" + strDT;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false);
        }
    }
}