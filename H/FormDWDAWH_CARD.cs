using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormDWDAWH_CARD : Form
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
        public int iSelect = 0;

        public FormDWDAWH_CARD()
        {
            InitializeComponent();
        }

        private void FormDWDAWH_CARD_Load(object sender, EventArgs e)
        {

            if (dt.Rows.Count < 1 && iStyle==1)
            {
                this.Close();
                return;
            }


            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            switch (iStyle)
            {
                case 0://增加
                    btnAccept.Text = "增加";
                    break;
                case 1://修改
                    btnAccept.Text = "修改";
                    break;
                default:
                    break;
            }

            sqlConn.Open();
            //银行
            sqlComm.CommandText = "SELECT DISTINCT 开户银行 FROM 单位表 WHERE (开户银行 <> N'') AND (开户银行 IS NOT NULL) ORDER BY 开户银行";
            if (dSet.Tables.Contains("开户银行")) dSet.Tables.Remove("开户银行");
            sqlDA.Fill(dSet, "开户银行");

            comboBoxKHYH.DataSource = dSet.Tables["开户银行"];
            comboBoxKHYH.DisplayMember = "开户银行";
            comboBoxKHYH.Text = "";


            //初始化地区
            comboBoxDQMC.SelectedIndexChanged -= comboBoxDQMC_SelectedIndexChanged;
            sqlComm.CommandText = "SELECT 地区, 编号 FROM 地区表 ORDER BY 地区";
            if (dSet.Tables.Contains("地区名称")) dSet.Tables.Remove("地区名称");
            sqlDA.Fill(dSet, "地区名称");
            comboBoxDQMC.DataSource = dSet.Tables["地区名称"];
            comboBoxDQMC.DisplayMember = "地区";
            comboBoxDQMC.ValueMember = "编号";
            comboBoxDQMC.SelectedIndexChanged += comboBoxDQMC_SelectedIndexChanged;


            //行业
            sqlComm.CommandText = "SELECT DISTINCT 行业名称 FROM 单位表 WHERE (行业名称 <> N'') AND (行业名称 IS NOT NULL) ORDER BY 行业名称";
            if (dSet.Tables.Contains("行业名称")) dSet.Tables.Remove("行业名称");
            sqlDA.Fill(dSet, "行业名称");

            comboBoxHYMC.DataSource = dSet.Tables["行业名称"];
            comboBoxHYMC.DisplayMember = "行业名称";
            comboBoxHYMC.Text = "";

            //业务员
            sqlComm.CommandText = "SELECT ID, 职员编号, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";


            //部门
            sqlComm.CommandText = "SELECT ID, 部门编号, 部门名称 FROM 部门表 WHERE (BeActive = 1)";

            if (dSet.Tables.Contains("部门表")) dSet.Tables.Remove("部门表");
            sqlDA.Fill(dSet, "部门表");
            comboBoxBM.DataSource = dSet.Tables["部门表"];
            comboBoxBM.DisplayMember = "部门名称";
            comboBoxBM.ValueMember = "ID";

            //到站
            sqlComm.CommandText = "SELECT 地区 FROM 地区表 ORDER BY 地区";
            if (dSet.Tables.Contains("到站名称")) dSet.Tables.Remove("到站名称");
            sqlDA.Fill(dSet, "到站名称");
            comboBoxDZMC.DataSource = dSet.Tables["到站名称"];
            comboBoxDZMC.DisplayMember = "地区";


            sqlConn.Close();

            if (iStyle == 1) //修改
            {
                //ID, 单位编号, 单位名称, 助记码, 是否进货, 是否销售, 税号, 电话, 开户银行, 银行账号, 联系人, 地址, 地区名称, 行业名称, 传真, 邮编, 备注, 登录日期, 联系地址, 收货人, 业务员, 到站名称
                textBoxDWBH.Text = dt.Rows[0][1].ToString();
                textBoxDWMC.Text = dt.Rows[0][2].ToString();
                textBoxZJM.Text = dt.Rows[0][3].ToString();
                if (dt.Rows[0][4].ToString() == "0")
                    checkBoxSFJH.Checked = false;
                else
                    checkBoxSFJH.Checked = true;

                if (dt.Rows[0][5].ToString() == "0")
                    checkBoxSFXS.Checked = false;
                else
                    checkBoxSFXS.Checked = true;

                textBoxSH.Text = dt.Rows[0][6].ToString();
                textBoxDH.Text = dt.Rows[0][7].ToString();
                comboBoxKHYH.Text = dt.Rows[0][8].ToString();
                textBoxYHZH.Text = dt.Rows[0][9].ToString();
                textBoxLXR.Text = dt.Rows[0][10].ToString();
                textBoxDZ.Text = dt.Rows[0][11].ToString();
                comboBoxDQMC.Text = dt.Rows[0][12].ToString();
                comboBoxHYMC.Text = dt.Rows[0][13].ToString();
                textBoxCZ.Text = dt.Rows[0][14].ToString();
                textBoxYB.Text = dt.Rows[0][15].ToString();
                textBoxBZ.Text = dt.Rows[0][16].ToString();
                textBoxKPDH.Text = dt.Rows[0][23].ToString();
                textBoxSHDH.Text = dt.Rows[0][24].ToString();
                try
                {
                    dateTimePickerDLRQ.Value = DateTime.Parse(dt.Rows[0][17].ToString());
                }
                catch
                {
                    dateTimePickerDLRQ.Value = DateTime.Now;
                }
                textBoxLXDZ.Text = dt.Rows[0][18].ToString();
                textBoxSHR.Text = dt.Rows[0][19].ToString();
                comboBoxYWY.Text = dt.Rows[0][20].ToString().Trim();
                comboBoxDZMC.Text = dt.Rows[0][21].ToString();

                if (dt.Rows[0][22].ToString() != "")
                {
                    comboBoxBM.SelectedValue = int.Parse(dt.Rows[0][22].ToString());
                }

            }
            



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iSelect = 0;
            this.Close();
        }


        private bool countAmount()
        {
            bool bCheck = true;

            if (textBoxDWBH.ToString() == "")
            {
                MessageBox.Show("输入类型错误,请输入单位编号", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bCheck = false;
                return bCheck;
            }

            if (textBoxDWMC.ToString() == "")
            {
                MessageBox.Show("输入类型错误,请输入单位名称", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bCheck = false;
            }
            return bCheck;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            int i1 = 0, i2 = 0;
            string strDateSYS = "";
            System.Data.SqlClient.SqlTransaction sqlta;

            if (!countAmount())
            {
               return;
            }
            

            switch (iStyle)
            {
                case 0://增加
                    sqlConn.Open();

                    //查重
                    if (textBoxDWBH.Text.Trim() == "")
                    {
                        MessageBox.Show("请输入单位编号");
                        sqlConn.Close();
                        break;
                    }
                    sqlComm.CommandText = "SELECT ID, 单位名称 FROM 单位表 WHERE (单位编号 = '" + textBoxDWBH.Text.Trim() + "')";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        MessageBox.Show("单位编号" + textBoxDWBH.Text.Trim() + "重复，名称为："+sqldr.GetValue(1).ToString());
                        sqldr.Close();
                        sqlConn.Close();
                        break;
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT ID, 单位编号 FROM 单位表 WHERE (单位名称 = '" + textBoxDWMC.Text.Trim() + "')";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        if (MessageBox.Show("单位名称" + textBoxDWMC.Text.Trim() + "重复，编号为：" + sqldr.GetValue(1).ToString() + "，是否继续？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        {
                            sqldr.Close();
                            sqlConn.Close();
                            break;
                        }
                    }
                    sqldr.Close();



                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {

                        //得到表单号
                        //得到服务器日期
                        sqlComm.CommandText = "SELECT GETDATE() AS 日期";
                        sqldr = sqlComm.ExecuteReader();

                        while (sqldr.Read())
                        {
                            strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                        }
                        sqldr.Close();

                        if (checkBoxSFJH.Checked)
                            i1 = 1;
                        else
                            i1 = 0;
                       

                        if (checkBoxSFXS.Checked)
                            i2 = 1;
                        else
                            i2 = 0;

                        sqlComm.CommandText = "INSERT INTO 单位表 (单位编号, 单位名称, 助记码, 是否进货, 是否销售, 税号, 电话, 开户银行, 银行账号, 联系人, 地址, 地区名称, 行业名称, 传真, 邮编, 备注, 登录日期, 联系地址, 应付账款, 应收账款, BeActive, 收货人, 业务员, 到站名称, 部门ID,开票电话,收货电话) VALUES ('" + textBoxDWBH.Text.Trim() + "', N'" + textBoxDWMC.Text.Trim() + "', '" + textBoxZJM.Text.Trim() + "', " + i1.ToString() + ", " + i2.ToString() + ", N'" + textBoxSH.Text.Trim() + "', '" + textBoxDH.Text.Trim() + "', N'" + comboBoxKHYH.Text.Trim() + "', '" + textBoxYHZH.Text.Trim() + "', N'" + textBoxLXR.Text.Trim() + "', N'" + textBoxDZ.Text.Trim() + "', N'" + comboBoxDQMC.Text.Trim() + "', N'" + comboBoxHYMC.Text.Trim() + "', N'" + textBoxCZ.Text.Trim() + "', '" + textBoxYB.Text.Trim() + "', N'" + textBoxBZ.Text.Trim() + "', '" + strDateSYS + "', N'" + textBoxLXDZ.Text.Trim() + "', 0, 0, 1, N'" + textBoxSHR.Text.Trim() + "',N'" + comboBoxYWY.Text + "',N'" + comboBoxDZMC.Text.Trim() + "', " + comboBoxBM.SelectedValue.ToString() + ",N'" + textBoxKPDH.Text.Trim() + "',N'" + textBoxSHDH.Text.Trim() + "')";
                        sqlComm.ExecuteNonQuery();

                        sqlComm.CommandText = "SELECT @@IDENTITY";
                        sqldr = sqlComm.ExecuteReader();
                        sqldr.Read();
                        iSelect = Convert.ToInt32(sqldr.GetValue(0).ToString());
                        sqldr.Close();


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
                    //查重
                    if (textBoxDWBH.Text.Trim() == "")
                    {
                        MessageBox.Show("请输入单位编号");
                        sqlConn.Close();
                        break;
                    }
                    iSelect = Convert.ToInt32(dt.Rows[0][0].ToString());
                    sqlComm.CommandText = "SELECT ID, 单位名称 FROM 单位表 WHERE (单位编号 = '" + textBoxDWBH.Text.Trim() + "' AND ID <> "+iSelect.ToString()+")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        MessageBox.Show("单位编号" + textBoxDWBH.Text.Trim() + "重复，名称为：" + sqldr.GetValue(1).ToString());
                        sqldr.Close();
                        sqlConn.Close();
                        break;
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT ID, 单位编号 FROM 单位表 WHERE (单位名称 = '" + textBoxDWMC.Text.Trim() + "' AND ID <> "+iSelect.ToString()+")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        if (MessageBox.Show("单位名称" + textBoxDWMC.Text.Trim() + "重复，编号为：" + sqldr.GetValue(1).ToString() + "，是否继续？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        {
                            sqldr.Close();
                            sqlConn.Close();
                            break;
                        }
                    }
                    sqldr.Close();


                    //使用状态
                    sqlComm.CommandText = "SELECT 单位表.单位名称, 单据汇总视图.单据编号 FROM 单位表 INNER JOIN 单据汇总视图 ON 单位表.ID = 单据汇总视图.单位ID WHERE (单据汇总视图.BeActive = 1) AND (单位表.ID = " + iSelect.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        if(textBoxDWMC.Text.Trim() != sqldr.GetValue(0).ToString())
                            MessageBox.Show("该单位已有单据保存，不可更改单位名称：" + sqldr.GetValue(0).ToString() + "。单据编号（示例）：" + sqldr.GetValue(1).ToString(), "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBoxDWMC.Text = sqldr.GetValue(0).ToString();
                    }
                    sqldr.Close();


                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {

                        //得到表单号
                        //得到服务器日期
                        sqlComm.CommandText = "SELECT GETDATE() AS 日期";
                        sqldr = sqlComm.ExecuteReader();

                        while (sqldr.Read())
                        {
                            strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                        }
                        sqldr.Close();

                        if (checkBoxSFJH.Checked)
                            i1 = 1;
                        else
                            i1 = 0;


                        if (checkBoxSFXS.Checked)
                            i2 = 1;
                        else
                            i2 = 0;

                        iSelect = Convert.ToInt32(dt.Rows[0][0].ToString());
                        sqlComm.CommandText = "UPDATE 单位表 SET 单位编号 = '" + textBoxDWBH.Text.Trim() + "', 单位名称 = N'" + textBoxDWMC.Text.Trim() + "', 助记码 = '" + textBoxZJM.Text.Trim() + "', 是否进货 = " + i1.ToString() + ", 是否销售 = " + i2.ToString() + ", 税号 = N'" + textBoxSH.Text.Trim() + "', 电话 = '" + textBoxDH.Text.Trim() + "', 开户银行 = N'" + comboBoxKHYH.Text.Trim() + "', 银行账号 = '" + textBoxYHZH.Text.Trim() + "', 联系人 = N'" + textBoxLXR.Text.Trim() + "', 地址 = N'" + textBoxDZ.Text.Trim() + "', 地区名称 = N'" + comboBoxDQMC.Text.Trim() + "', 行业名称 = N'" + comboBoxHYMC.Text.Trim() + "', 传真 = N'" + textBoxCZ.Text.Trim() + "', 邮编 = '" + textBoxYB.Text.Trim() + "', 备注 = N'" + textBoxBZ.Text.Trim() + "', 登录日期 = '" + dateTimePickerDLRQ.Value.ToShortDateString() + "', 联系地址 = N'" + textBoxLXDZ.Text.Trim() + "',收货人=N'" + textBoxSHR.Text.Trim() + "', 业务员=N'" + comboBoxYWY.Text.Trim() + "', 到站名称=N'" + comboBoxDZMC.Text.Trim() + "', 部门ID = " + comboBoxBM.SelectedValue.ToString() + ", 开票电话=N'" + textBoxKPDH.Text.Trim() + "', 收货电话=N'" + textBoxSHDH.Text.Trim() + "' WHERE (ID = " + iSelect + ")";
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
                    MessageBox.Show("修改成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    break;
                default:
                    break;
            }
        }


        private void textBoxDWMC_TextChanged(object sender, EventArgs e)
        {
            textBoxZJM.Text = cGetInformation.convertPYSM(textBoxDWMC.Text);
        }

        private void comboBoxDQMC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDQMC.SelectedValue.ToString() == "")
                return;

            if (iStyle == 1) //修改模式
                return;

            int iMaxDWNo = 1;
            string sT="";

            sqlConn.Open();

            //得到最终编码
            sqlComm.CommandText = "SELECT MAX(单位编号) FROM 单位表 WHERE (单位编号 LIKE '"+comboBoxDQMC.SelectedValue.ToString().Trim()+"%')";
            sqldr = sqlComm.ExecuteReader();

            if (sqldr.HasRows)
            {
                sqldr.Read();
                sT = sqldr.GetValue(0).ToString().Trim();

                if (sT.Length < 4)
                    sT = "";
                else
                    sT = sT.Substring(4,sT.Length-4);
                try
                {
                    iMaxDWNo = Convert.ToInt32(sT);
                    iMaxDWNo++;
                }
                catch
                {
                    iMaxDWNo = 1;
                }

            }
            sqldr.Close();

            textBoxDWBH.Text = comboBoxDQMC.SelectedValue.ToString().Trim() + string.Format("{0:D4}", iMaxDWNo);


            sqlConn.Close();
        }

        private void textBoxDWBH_Validating(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex rExpression = new System.Text.RegularExpressions.Regex(@"^\d{8}$");

            textBoxDWBH.Text = textBoxDWBH.Text.Trim();
            if (rExpression.IsMatch(textBoxDWBH.Text) || textBoxDWBH.Text == "")
            {
                this.errorProviderM.Clear();
            }
            else
            {
                this.errorProviderM.SetError(this.textBoxDWBH, "输入正确的单位编码，八位数字 例如：01000001");
                e.Cancel = true;
            }
        }

        private void comboBoxDQMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i;
            bool bSelect=false;

            if (e.KeyChar == (char)Keys.Return)
            {
                for (i = 0; i < dSet.Tables["地区名称"].Rows.Count; i++)
                {
                    if (dSet.Tables["地区名称"].Rows[i][0].ToString() == comboBoxDQMC.Text)
                    {
                        comboBoxDQMC.SelectedIndex = i;
                        bSelect = true;
                        break;
                    }
                }
                if (comboBoxDQMC.SelectedIndex < 0)
                    comboBoxDQMC.SelectedIndex = 0;
                if (!bSelect)
                {
                    comboBoxDQMC.Text = dSet.Tables["地区名称"].Rows[comboBoxDQMC.SelectedIndex][0].ToString();
                }
                comboBoxDQMC_SelectedIndexChanged(null,null);

            }
        }



   }
}