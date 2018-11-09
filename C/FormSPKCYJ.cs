using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPKCYJ : Form
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

        private int intKFID = 0;

        private ClassGetInformation cGetInformation;

        private bool isSaved = false;
        public int LIMITACCESS = 18;
        
        public FormSPKCYJ()
        {
            InitializeComponent();
        }

        private void FormSPKCYJ_Load(object sender, EventArgs e)
        {
            int i;

            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);


            sqlConn.Open();
            sqlComm.CommandText = "SELECT 公司宣传, 质量目标1, 质量目标2, 质量目标3, 质量目标4, 管理员权限, 总经理权限, 职员权限, 经理权限, 业务员权限 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {

                try
                {
                    LIMITACCESS = int.Parse(sqldr.GetValue(6).ToString());
                }
                catch
                {
                    LIMITACCESS = 18;
                }
            }
            sqldr.Close();
            sqlConn.Close();
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            comboBoxStyle.SelectedIndex = 0;

            //initDataView();
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAll.Checked) //总库房
            {
                intKFID = 0;
                textBoxKFBH.Text = "";
                textBoxKFMC.Text = "";
            }
            else
            {
                if (intKFID == 0)
                {
                    intKFID = 0;
                    textBoxKFBH.Text = "";
                    textBoxKFMC.Text = "";
                    checkBoxAll.CheckedChanged -= checkBoxAll_CheckedChanged;
                    checkBoxAll.Checked = true;
                    checkBoxAll.CheckedChanged += checkBoxAll_CheckedChanged;
                }
            }
            //initDataView();
        }

        private void textBoxKFBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                intKFID = cGetInformation.iKFNumber;
                textBoxKFBH.Text = cGetInformation.strKFCode;
                textBoxKFMC.Text = cGetInformation.strKFName;
            }
            //initDataView();
            if (intKFID == 0)
            {
                intKFID = 0;
                textBoxKFBH.Text = "";
                textBoxKFMC.Text = "";
                checkBoxAll.CheckedChanged -= checkBoxAll_CheckedChanged;
                checkBoxAll.Checked = true;
                checkBoxAll.CheckedChanged += checkBoxAll_CheckedChanged;
            }
            else
            {
                checkBoxAll.CheckedChanged -= checkBoxAll_CheckedChanged;
                checkBoxAll.Checked = false;
                checkBoxAll.CheckedChanged += checkBoxAll_CheckedChanged;
            }
        }

        private void textBoxKFBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(10, textBoxKFBH.Text.Trim()) == 0) //失败
                {
                    intKFID = 0;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                }
                //initDataView();
                if (intKFID == 0)
                {
                    intKFID = 0;
                    textBoxKFBH.Text = "";
                    textBoxKFMC.Text = "";
                    checkBoxAll.CheckedChanged -= checkBoxAll_CheckedChanged;
                    checkBoxAll.Checked = true;
                    checkBoxAll.CheckedChanged += checkBoxAll_CheckedChanged;
                }
                else
                {
                    checkBoxAll.CheckedChanged -= checkBoxAll_CheckedChanged;
                    checkBoxAll.Checked = false;
                    checkBoxAll.CheckedChanged += checkBoxAll_CheckedChanged;
                    btnSelect.Focus();
                }
            }
        }

        private void textBoxKFMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFMC.Text.Trim()) == 0) //失败
                {
                    intKFID = 0;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                }
                //initDataView();
                if (intKFID == 0)
                {
                    intKFID = 0;
                    textBoxKFBH.Text = "";
                    textBoxKFMC.Text = "";
                    checkBoxAll.CheckedChanged -= checkBoxAll_CheckedChanged;
                    checkBoxAll.Checked = true;
                    checkBoxAll.CheckedChanged += checkBoxAll_CheckedChanged;
                }
                else
                {
                    checkBoxAll.CheckedChanged -= checkBoxAll_CheckedChanged;
                    checkBoxAll.Checked = false;
                    checkBoxAll.CheckedChanged += checkBoxAll_CheckedChanged;
                }
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            sqlConn.Open();

            switch (comboBoxStyle.SelectedIndex)
            {
                case 0: //超出上限
                    if (intKFID == 0) //总库存
                        sqlComm.CommandText = "SELECT 商品名称, 商品编号, 商品规格, 库存数量, 库存金额, 库存上限, 合理库存上限, 合理库存下限, 库存下限 FROM 商品表 WHERE (beactive = 1) AND (库存数量 > 库存上限) AND (组装商品 = 0)";
                    else //分库存
                        sqlComm.CommandText = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库存表.库存数量, 库存表.库存金额,  库存表.库存上限, 库存表.合理库存上限, 库存表.合理库存下限, 库存表.库存下限 FROM 库存表 INNER JOIN 商品表 ON 库存表.商品ID = 商品表.ID WHERE (库存表.库房ID = " + intKFID.ToString() + ") AND (商品表.beactive = 1) AND (库存表.库存数量 > 库存表.库存上限) AND (商品表.组装商品 = 0)";
                    break;

                case 1: //临近上限
                    if (intKFID == 0) //总库存
                        sqlComm.CommandText = "SELECT 商品名称, 商品编号, 商品规格, 库存数量, 库存金额, 库存上限, 合理库存上限, 合理库存下限, 库存下限 FROM 商品表 WHERE (beactive = 1) AND (库存数量 <= 库存上限) AND (库存数量 > 合理库存上限) AND (组装商品 = 0) ";
                    else //分库存
                        sqlComm.CommandText = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库存表.库存数量, 库存表.库存金额, 库存表.库存上限, 库存表.合理库存上限, 库存表.合理库存下限, 库存表.库存下限 FROM 库存表 INNER JOIN 商品表 ON 库存表.商品ID = 商品表.ID WHERE (库存表.库房ID = " + intKFID.ToString() + ") AND (商品表.beactive = 1) AND (库存表.库存数量 <= 库存表.库存上限) AND (库存表.库存数量 > 库存表.合理库存上限) AND (商品表.组装商品 = 0)";
                    break;

                case 2: //低于下限
                    if (intKFID == 0) //总库存
                        sqlComm.CommandText = "SELECT 商品名称, 商品编号, 商品规格, 库存数量, 库存金额, 库存上限, 合理库存上限, 合理库存下限, 库存下限 FROM 商品表 WHERE (beactive = 1) AND (库存数量 < 库存下限) AND (组装商品 = 0)";
                    else //分库存
                        sqlComm.CommandText = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库存表.库存数量, 库存表.库存金额, 库存表.库存上限, 库存表.合理库存上限, 库存表.合理库存下限, 库存表.库存下限 FROM 库存表 INNER JOIN 商品表 ON 库存表.商品ID = 商品表.ID WHERE (库存表.库房ID = " + intKFID.ToString() + ") AND (商品表.beactive = 1) AND (库存表.库存数量 < 库存表.库存下限) AND (商品表.组装商品 = 0)";
                    break;

                case 3: //临近下限
                    if (intKFID == 0) //总库存
                        sqlComm.CommandText = "SELECT 商品名称, 商品编号, 商品规格, 库存数量, 库存金额, 库存上限, 合理库存上限, 合理库存下限, 库存下限 FROM 商品表 WHERE (beactive = 1) AND (库存数量 >= 库存下限) AND (库存数量 < 合理库存下限) AND (组装商品 = 0) ";
                    else //分库存
                        sqlComm.CommandText = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库存表.库存数量, 库存表.库存金额, 库存表.库存上限, 库存表.合理库存上限, 库存表.合理库存下限, 库存表.库存下限 FROM 库存表 INNER JOIN 商品表 ON 库存表.商品ID = 商品表.ID WHERE (库存表.库房ID = " + intKFID.ToString() + ") AND (商品表.beactive = 1) AND (库存表.库存数量 >= 库存表.库存下限) AND (库存表.库存数量 < 库存表.合理库存下限) AND (商品表.组装商品 = 0)";
                    break;

                case 5: //负库存
                    if (intKFID == 0) //总库存
                        sqlComm.CommandText = "SELECT 商品名称, 商品编号, 商品规格, 库存数量, 库存金额, 库存上限, 合理库存上限, 合理库存下限, 库存下限 FROM 商品表 WHERE (beactive = 1) AND (库存数量 < 0) AND (组装商品 = 0)";
                    else //分库存
                        sqlComm.CommandText = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库存表.库存数量, 库存表.库存金额, 库存表.库存上限, 库存表.合理库存上限, 库存表.合理库存下限, 库存表.库存下限 FROM 库存表 INNER JOIN 商品表 ON 库存表.商品ID = 商品表.ID WHERE (库存表.库房ID = " + intKFID.ToString() + ") AND (商品表.beactive = 1) AND (库存表.库存数量 < 0) AND (商品表.组装商品 = 0)";
                    break;


                case 4: //合理库存
                    if (intKFID == 0) //总库存
                        sqlComm.CommandText = "SELECT 商品名称, 商品编号, 商品规格, 库存数量, 库存金额, 库存上限, 合理库存上限, 合理库存下限, 库存下限 FROM 商品表 WHERE (beactive = 1) AND (库存数量 >= 合理库存下限) AND (库存数量 <= 合理库存上限)  AND (组装商品 = 0)";
                    else //分库存
                        sqlComm.CommandText = "SELECT 商品表.商品名称, 商品表.商品编号, 商品表.商品规格, 库存表.库存数量, 库存表.库存金额, 库存表.库存上限, 库存表.合理库存上限, 库存表.合理库存下限, 库存表.库存下限 FROM 库存表 INNER JOIN 商品表 ON 库存表.商品ID = 商品表.ID WHERE (库存表.库房ID = " + intKFID.ToString() + ") AND (商品表.beactive = 1) AND (库存表.库存数量 >= 库存表.合理库存下限) AND (库存表.库存数量 <= 库存表.合理库存上限) AND (商品表.组装商品 = 0)";
                    break;

                default:
                    MessageBox.Show("请选择商品库存预警的内容", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    sqlConn.Close();
                    return;


            }

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            dataGridViewDJMX.DataSource = dSet.Tables["商品表"];

            if (intUserLimit < LIMITACCESS)
                dataGridViewDJMX.Columns[4].Visible = false;

            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            sqlConn.Close();
            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.Rows.Count.ToString();
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "库存商品预警;日期：" + labelZDRQ.Text + ";"+comboBoxStyle.Text+";操作员：" + labelCZY.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "库存商品预警;日期：" + labelZDRQ.Text + ";" + comboBoxStyle.Text + ";操作员：" + labelCZY.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }
    }
}