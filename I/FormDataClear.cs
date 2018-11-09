using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormDataClear : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";

        public FormDataClear()
        {
            InitializeComponent();
        }

        private void FormDataClear_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            dateTimePickerEND.Value = System.DateTime.Now.AddYears(-2);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否删除"+dateTimePickerEND.Value.ToLongDateString()+"以前的所有单据？这个过程可能引起以往转结结果不正常，并不可恢复。", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                return;
            }
            sqlConn.Open();

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;

            try
            {
                sqlComm.CommandText = "DELETE FROM 借物信息修改表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 借物出库明细表 WHERE (表单ID IN (SELECT ID FROM 借物出库汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 借物出库汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 单位历史账表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 发票明细表 WHERE (发票ID IN (SELECT ID FROM 发票汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 发票汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 商品历史账表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 商品库房历史账表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 商品条码表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 库存报损明细表 WHERE (单据ID IN (SELECT ID FROM 库存报损汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 库存报损汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 库存盘点明细表 WHERE (单据ID IN (SELECT ID FROM 库存盘点汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 库存盘点汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 日志表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算付款明细表 WHERE (单据ID IN (SELECT ID FROM 结算付款汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算付款汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算收款明细表 WHERE (单据ID IN (SELECT ID FROM 结算收款汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 结算收款汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 调价通知单明细表 WHERE (单据ID IN (SELECT ID FROM 调价通知单汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 调价通知单汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 购进商品制单明细表 WHERE (表单ID IN (SELECT ID FROM 购进商品制单表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 购进商品制单表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 购进退补差价明细表 WHERE (单据ID IN (SELECT ID FROM 购进退补差价汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 购进退补差价汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 进货入库明细表 WHERE (单据ID IN (SELECT ID FROM 进货入库汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 进货入库汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 进货退出明细表 WHERE (单据ID IN (SELECT ID FROM 进货退出汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 进货退出汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 送货信息修改表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 采购合同明细表 WHERE (采购合同ID IN (SELECT ID FROM 采购合同表 WHERE (签订时间 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 采购合同表 WHERE (签订时间 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售出库明细表 WHERE (单据ID IN (SELECT ID FROM 销售出库汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售出库汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售合同明细表 WHERE (销售合同ID IN (SELECT ID FROM 销售合同表 WHERE (签订时间 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售合同表 WHERE (签订时间 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售商品制单明细表 WHERE (表单ID IN (SELECT ID FROM 销售商品制单表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售商品制单表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售退出明细表 WHERE (单据ID IN (SELECT ID FROM 销售退出汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售退出汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售退补差价明细表 WHERE (单据ID IN (SELECT ID FROM 销售退补差价汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))))";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "DELETE FROM 销售退补差价汇总表 WHERE (日期 < CONVERT(DATETIME, '" + dateTimePickerEND.Value.ToShortDateString() + " 00:00:00', 102))";
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
            MessageBox.Show("删除完毕", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}
