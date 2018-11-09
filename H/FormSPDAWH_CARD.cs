using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPDAWH_CARD : Form
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
        private int iClass = 0;
        public int iSelect = 0;
        
        public FormSPDAWH_CARD()
        {
            InitializeComponent();
        }

        private void FormSPDAWH_CARD_Load(object sender, EventArgs e)
        {
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
            initCommTree();

            if (iStyle == 1) //修改
            {
                textBoxSPBH.TextChanged -= textBoxSPBH_TextChanged;
                //商品表.ID, 商品表.商品编号, 商品表.商品名称, 商品表.助记码, 商品表.最小计量单位, 商品表.进价, 商品表.批发价, 商品表.登录日期, 商品表.商品规格, 商品表.库存上限, 商品表.库存下限, 商品表.合理库存上限, 商品表.合理库存下限, 商品分类表.分类名称, 商品表.分类编号 AS 分类ID, 商品分类表.分类编号
                textBoxSPBH.Text = dt.Rows[0][1].ToString();
                textBoxSPMC.Text = dt.Rows[0][2].ToString();
                textBoxZJM.Text = dt.Rows[0][3].ToString();
                textBoxZXJLDW.Text = dt.Rows[0][4].ToString();
                textBoxSPGG.Text = dt.Rows[0][8].ToString();

                try
                {
                    dateTimePickerDLRQ.Value = DateTime.Parse(dt.Rows[0][7].ToString());
                }
                catch
                {
                    dateTimePickerDLRQ.Value = DateTime.Now;
                }

                try
                {
                    iClass = Convert.ToInt32(dt.Rows[0][14].ToString());

                    numericUpDownJJ.Value = Convert.ToDecimal(dt.Rows[0][5].ToString());
                    numericUpDownPFJ.Value = Convert.ToDecimal(dt.Rows[0][6].ToString());

                    numericUpDownKCSX.Value = Convert.ToDecimal(dt.Rows[0][9].ToString());
                    numericUpDownKCXX.Value = Convert.ToDecimal(dt.Rows[0][10].ToString());

                    numericUpDownHLSX.Value = Convert.ToDecimal(dt.Rows[0][11].ToString());
                    numericUpDownHLSX.Value = Convert.ToDecimal(dt.Rows[0][12].ToString());
                }
                catch
                {
                    
                }

                if (iClass != 0)
                {
                    TreeNode nodeTemp = FindTreeNodeByDepth(this.treeViewComm.Nodes, iClass);
                    this.treeViewComm.SelectedNode = nodeTemp;
                }

                textBoxSPBH.TextChanged += textBoxSPBH_TextChanged;


            }
            textBoxSPMC.TextChanged += textBoxSPMC_TextChanged;
        }
        private void initCommTree()
        {
            string strTemp;
            int iTemp;
            TreeNode nodeTemp;

            TreeNode RootNode = new TreeNode("所有类型", 0, 1);
            int iTagRoot = 0;
            RootNode.Tag = iTagRoot;
            this.treeViewComm.Nodes.Add(RootNode);

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM 商品分类表 WHERE BeActive = 1 ORDER BY 上级分类, 分类编号";
            if (dSet.Tables.Contains("商品分类表")) dSet.Tables.Remove("商品分类表");
            sqlDA.Fill(dSet, "商品分类表");
            for (int i = 0; i < dSet.Tables["商品分类表"].Rows.Count; i++)
            {
                int iTag;

                if (dSet.Tables["商品分类表"].Rows[i][3].ToString() == "")
                    continue;
                strTemp = dSet.Tables["商品分类表"].Rows[i][3].ToString();
                //得到上级TAG
                iTemp = strTemp.LastIndexOf(',');
                if (iTemp != -1)
                    strTemp = strTemp.Substring(iTemp + 1);

                nodeTemp = FindTreeNodeByDepth(this.treeViewComm.Nodes, Int32.Parse(strTemp));
                TreeNode nT = new TreeNode(dSet.Tables["商品分类表"].Rows[i][2].ToString(), 0, 1);
                iTemp = Int32.Parse(dSet.Tables["商品分类表"].Rows[i][0].ToString());
                nT.Tag = iTemp;
                nodeTemp.Nodes.Add(nT);

            }
            sqlConn.Close();
            RootNode.Expand();

        }
        private TreeNode FindTreeNodeByDepth(TreeNodeCollection p_treeNodes, int p_i)
        {
            TreeNode treeNodeReturn = null;
            int iValue;

            foreach (TreeNode node in p_treeNodes)
            {
                //取当前节点键   
                iValue = (int)node.Tag;

                //否则根据值比   
                if (iValue == p_i)
                    treeNodeReturn = node;

                //找到即退出   
                if (treeNodeReturn != null)
                    break;
                else
                {
                    //深度优先查询   
                    if (node.Nodes.Count > 0)
                    {
                        treeNodeReturn = FindTreeNodeByDepth(node.Nodes, p_i);
                    }
                }

            }

            return treeNodeReturn;
        } 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iSelect = 0;
            this.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            
            int i;
            int i1 = 0, i2 = 0;
            string strDateSYS = "";
            System.Data.SqlClient.SqlTransaction sqlta;

            if (textBoxSPBH.Text.Trim()=="")
            {
                MessageBox.Show("输入类型错误，请输入商品编号", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBoxSPMC.Text.Trim() == "")
            {
                MessageBox.Show("输入类型错误，请输入商品名称", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (iClass == 0)
            {
                MessageBox.Show("输入类型错误，请选择商品类型", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            switch (iStyle)
            {
                case 0://增加
                    sqlConn.Open();

                    //查重
                    if (textBoxSPBH.Text.Trim() == "")
                    {
                        MessageBox.Show("请输入商品编号");
                        sqlConn.Close();
                        break;
                    }
                    sqlComm.CommandText = "SELECT ID, 商品名称 FROM 商品表 WHERE (商品编号 = '" + textBoxSPBH.Text.Trim() + "')";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        MessageBox.Show("商品编号" + textBoxSPBH.Text.Trim() + "重复，名称为：" + sqldr.GetValue(1).ToString());
                        sqldr.Close();
                        sqlConn.Close();
                        break;
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT ID, 商品编号 FROM 商品表 WHERE (商品名称 = '" + textBoxSPMC.Text.Trim() + "')";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        if (MessageBox.Show("商品名称" + textBoxSPMC.Text.Trim() + "重复，编号为：" + sqldr.GetValue(1).ToString() + "，是否继续？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
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

                        //得到服务器日期
                        sqlComm.CommandText = "SELECT GETDATE() AS 日期";
                        sqldr = sqlComm.ExecuteReader();

                        while (sqldr.Read())
                        {
                            strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                        }
                        sqldr.Close();


                        sqlComm.CommandText = "INSERT INTO 商品表 (商品编号, 商品名称, 助记码, 最小计量单位, 进价, 含税进价, 批发价, 含税批发价, 库存数量, 库存成本价, 库存金额, 库存件数, 最高进价, 最低进价, 最终进价, 结转数量, 结转件数, 结转金额, 结转单价, 登录日期, 库存上限, 库存下限, 合理库存上限, 合理库存下限, 组装商品, beactive, 应付金额, 已付金额, 应收金额, 已收金额, 分类编号, 商品规格) VALUES (N'" + textBoxSPBH.Text.Trim() + "', N'" + textBoxSPMC.Text.Trim() + "', N'" + textBoxZJM.Text.Trim() + "', N'" + textBoxZXJLDW.Text.Trim() + "', " + numericUpDownJJ.Value.ToString() + ", " + numericUpDownJJ.Value.ToString() + ", " + numericUpDownPFJ.Value.ToString() + ", " + numericUpDownPFJ.Value.ToString() + ", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '" + dateTimePickerDLRQ.Value.ToShortDateString() + "', " + numericUpDownKCSX.Value.ToString() + ", " + numericUpDownKCXX.Value.ToString() + ", " + numericUpDownHLSX.Value.ToString() + ", " + numericUpDownHLXX.Value.ToString() + ", 0, 1, 0, 0, 0, 0, " + iClass.ToString() + ", N'" + textBoxSPGG.Text.Trim() + "')";
                        sqlComm.ExecuteNonQuery();

                        sqlComm.CommandText = "SELECT @@IDENTITY";
                        sqldr = sqlComm.ExecuteReader();
                        sqldr.Read();
                        iSelect = Convert.ToInt32(sqldr.GetValue(0).ToString());
                        sqldr.Close();

                        //增加库存
                        sqlComm.CommandText = "SELECT 库房ID FROM 商品分类表 WHERE (ID = " + iClass.ToString() + ")";
                        sqldr = sqlComm.ExecuteReader();
                        sqldr.Read();
                        string sKF = sqldr.GetValue(0).ToString();
                        sqldr.Close();

                        if(sKF != "")
                        {
                            sqlComm.CommandText = "INSERT INTO 库存表 (库房ID, 商品ID, 库存数量, 库存金额, 库存成本价, 核算成本价, 库存上限, 库存下限, 合理库存上限, 合理库存下限, 应付金额, 已付金额, 应收金额, 已收金额, BeActive) VALUES (" + sKF + ", " + iSelect.ToString() + ", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)";
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
                    MessageBox.Show("增加成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    break;
                case 1://修改

                    sqlConn.Open();

                    //查重
                    if (textBoxSPBH.Text.Trim() == "")
                    {
                        MessageBox.Show("请输入商品编号");
                        sqlConn.Close();
                        break;
                    }
                    iSelect = Convert.ToInt32(dt.Rows[0][0].ToString());

                    sqlComm.CommandText = "SELECT ID, 商品名称 FROM 商品表 WHERE (商品编号 = '" + textBoxSPBH.Text.Trim() + "' AND ID <> " + iSelect.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        MessageBox.Show("商品编号" + textBoxSPBH.Text.Trim() + "重复，名称为：" + sqldr.GetValue(1).ToString());
                        sqldr.Close();
                        sqlConn.Close();
                        break;
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT ID, 商品编号 FROM 商品表 WHERE (商品名称 = '" + textBoxSPMC.Text.Trim() + "' AND ID <> " + iSelect.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        if (MessageBox.Show("商品名称" + textBoxSPMC.Text.Trim() + "重复，编号为：" + sqldr.GetValue(1).ToString() + "，是否继续？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        {
                            sqldr.Close();
                            sqlConn.Close();
                            break;
                        }
                    }
                    sqldr.Close();

                    //使用状态
                    sqlComm.CommandText = "SELECT DISTINCT 商品表.商品名称 FROM 单据明细汇总视图 INNER JOIN 商品表 ON 单据明细汇总视图.商品ID = 商品表.ID WHERE (单据明细汇总视图.BeActive = 1) AND (单据明细汇总视图.商品ID = " + iSelect.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        if (textBoxSPMC.Text.Trim() != sqldr.GetValue(0).ToString())
                            MessageBox.Show("该商品已有单据保存，不可更改商品名称：" + sqldr.GetValue(0).ToString() + "。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBoxSPMC.Text = sqldr.GetValue(0).ToString();
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

                        iSelect = Convert.ToInt32(dt.Rows[0][0].ToString());
                        sqlComm.CommandText = "UPDATE 商品表 SET 商品编号 = N'" + textBoxSPBH.Text.Trim() + "', 商品名称 = N'" + textBoxSPMC.Text.Trim() + "', 助记码 = N'" + textBoxZJM.Text.Trim() + "', 最小计量单位 = N'" + textBoxZXJLDW.Text.Trim() + "', 进价 = " + numericUpDownJJ.Value.ToString() + ", 含税进价 = " + numericUpDownJJ.Value.ToString() + ", 批发价 = " + numericUpDownPFJ.Value.ToString() + ", 含税批发价 = " + numericUpDownPFJ.Value.ToString() + ", 登录日期 = '" + dateTimePickerDLRQ.Value.ToShortDateString() + "', 库存上限 = " + numericUpDownKCSX.Value.ToString() + ", 库存下限 = " + numericUpDownKCXX.Value.ToString() + ", 合理库存上限 = " + numericUpDownHLSX.Value.ToString() + ", 合理库存下限 = " + numericUpDownHLXX.Value.ToString() + ", 分类编号 = " + iClass.ToString() + ", 商品规格 = N'"+textBoxSPGG.Text.Trim()+"' WHERE (ID = " + dt.Rows[0][0].ToString() + ")";
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

        private void textBoxSPMC_TextChanged(object sender, EventArgs e)
        {
            int ii = textBoxSPMC.Text.Trim().IndexOf('-');

            if (ii == -1 || ii == textBoxSPMC.Text.Trim().Length - 1)
            {
                textBoxZJM.Text = cGetInformation.convertPYSM(textBoxSPMC.Text);
                return;
            }

            int i1, i2, i3, i4;
            if (ii < 3)
            {
                i1 = 0;
                i2 = ii;
            }
            else
            {
                i1 = ii - 3;
                i2 = 3;
            }

            i3 = ii + 1;
            if (ii + 4 > textBoxSPMC.Text.Trim().Length)
            {
                i4 = textBoxSPMC.Text.Trim().Length - ii - 1;
            }
            else
            {
                i4 = 3;
            }

            string ss = textBoxSPMC.Text.Trim().Substring(i1, i2) + textBoxSPMC.Text.Trim().Substring(i3, i4);
            textBoxZJM.Text = cGetInformation.convertPYSM(ss);

                
        }

        private void treeViewComm_AfterSelect(object sender, TreeViewEventArgs e)
        {
            iClass = (int)e.Node.Tag;
        }

        private void textBoxSPBH_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSPBH.Text.Trim().Length < 2)
                return;

            int iNode = 0;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ID FROM 商品分类表 WHERE (BeActive = 1) AND (分类编号 = '" + textBoxSPBH.Text.Substring(0,2)+ "')";
            sqldr = sqlComm.ExecuteReader();

            if (sqldr.HasRows)
            {
                sqldr.Read();
                iNode = int.Parse(sqldr.GetValue(0).ToString());
            }
            sqlConn.Close();

            if (iNode == 0)
                return;

            TreeNode nodeTemp = FindTreeNodeByDepth(this.treeViewComm.Nodes, iNode);
            this.treeViewComm.SelectedNode = nodeTemp;



        }


    }
}