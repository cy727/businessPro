using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace business
{
    public partial class FormKCCX : Form
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


        private int intCommID = 0;
        private int intKFID=0;

        bool bDPFX = false;

        private ArrayList alFL=new ArrayList();

        private ClassGetInformation cGetInformation;

        public int LIMITACCESS = 18;

        public FormKCCX()
        {
            InitializeComponent();
        }

        private void FormKCCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

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
            
        }

        private void textBoxKFBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(10, "") == 0)
            {
                //return;
            }
            else
            {
                intKFID = cGetInformation.iKFNumber;
                textBoxKFMC.Text = cGetInformation.strKFName;
                textBoxKFBH.Text = cGetInformation.strKFCode;
            }
        }

        private void textBoxKFBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(10, textBoxKFBH.Text) == 0)
                {
                    //return;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                }
            }
        }

        private void textBoxKFMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFMC.Text) == 0)
                {
                    //return;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                }
            }
        }

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0)
            {
                //return;
            }
            else
            {
                intCommID = cGetInformation.iCommNumber;
                textBoxSPMC.Text = cGetInformation.strCommName;
                textBoxSPBH.Text = cGetInformation.strCommCode;
            }
        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0)
                {
                    //return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                }
            }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0)
                {
                    //return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                }
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            bDPFX = true;
            int i,j;

            sqlConn.Open();
           // sqlComm.CommandText = "SELECT 商品表.ID, 库房表.库房名称, 商品表.商品名称, 库存表.库存数量, 库存表.库存成本价,库存表.库存金额 FROM 库存表 INNER JOIN 库房表 ON 库存表.库房ID = 库房表.ID INNER JOIN 商品表 ON 库存表.商品ID = 商品表.ID WHERE (库房表.BeActive = 1)";
            sqlComm.CommandText = "SELECT 商品表.ID, 库房表.库房名称, 商品表.商品名称, 商品表.商品规格, 库存表.库存数量, 库存表.库存成本价,(库存表.库存数量*库存表.库存成本价) AS 库存金额, 商品表.进价 AS 进价, 商品表.批发价 FROM 库存表 INNER JOIN 库房表 ON 库存表.库房ID = 库房表.ID INNER JOIN 商品表 ON 库存表.商品ID = 商品表.ID WHERE (库房表.BeActive = 1) AND (商品表.beactive = 1) AND (库存表.BeActive = 1) ";

            if (intCommID != 0)
                sqlComm.CommandText += " AND (商品表.ID = "+intCommID.ToString()+") ";
            if (intKFID != 0)
                sqlComm.CommandText += "AND (库房表.ID = "+intKFID.ToString()+") ";

            switch (comboBoxC.SelectedIndex.ToString())
            {
                case "1":
                    sqlComm.CommandText += " AND (库存表.库存数量 <= " + numericUpDownC.Value.ToString() + ") ";
                    break;
                case "2":
                    sqlComm.CommandText += " AND (库存表.库存数量 >= " + numericUpDownC.Value.ToString() + ") ";
                    break;
                default:
                    break;
            }

            if (alFL.Count > 0)
            {
                for (i = 0; i < alFL.Count; i++)
                {
                    cGetInformation.getUnderClassInformation(int.Parse(alFL[i].ToString()));
                    if(i==0)
                        sqlComm.CommandText += " AND ((商品表.分类编号 = " + alFL[i].ToString() + ")";
                    else
                        sqlComm.CommandText += " OR (商品表.分类编号 = " + alFL[i].ToString() + ")";
                    for (j = 0; j < cGetInformation.intUnderClassNumber; j++)
                        sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[j].ToString() + ")";

                }
                sqlComm.CommandText += ")";
            }

            sqlComm.CommandText += " ORDER BY 商品表.分类编号";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");

            sqlConn.Close();

            decimal dTemp = 0, dTemp1 = 0; ;
            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表"].Rows[i][4].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][4] = 0;
                if (dSet.Tables["商品表"].Rows[i][6].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][6] = 0;
                dTemp += Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][6]);
                dTemp1 += Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][4]);

                //dSet.Tables["商品表"].Rows[i][0] = i + 1;

            }
            
            labelKCSLHJ.Text = dTemp1.ToString("f0");

            dataGridView1.DataSource = dSet.Tables["商品表"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[7].DefaultCellStyle.Format = "f2";
            dataGridView1.Columns[8].DefaultCellStyle.Format = "f2"; 
            //dataGridView1.Columns[0].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //权限控制
            if (intUserLimit >= LIMITACCESS)
            {
                labelKCJEHJ.Text = dTemp.ToString("f2");

            }
            else
            {
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                labelKCJEHJ.Visible = false;
            }


            toolStripStatusLabel1.Text = "记录数 "+dSet.Tables["商品表"].Rows.Count.ToString();

        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            bDPFX = false;

            int i,j;
            bool bMX = true;

            if (MessageBox.Show("是否包含明细？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 库房表.库房名称, 商品表.商品名称, 库存表.库存数量, 库存表.库存成本价,(库存表.库存数量*库存表.库存成本价) AS 库存金额, 商品表.分类编号 FROM 库存表 INNER JOIN 库房表 ON 库存表.库房ID = 库房表.ID INNER JOIN 商品表 ON 库存表.商品ID = 商品表.ID WHERE (库房表.BeActive = 1) AND (商品表.beactive = 1) AND (库存表.BeActive = 1) ";

            if (intCommID != 0)
                sqlComm.CommandText += " AND (商品表.ID = " + intCommID.ToString() + ")";
            if (intKFID != 0)
                sqlComm.CommandText += "AND (库房表.ID = " + intKFID.ToString() + ")";

            switch (comboBoxC.SelectedIndex.ToString())
            {
                case "1":
                    sqlComm.CommandText += " AND (库存表.库存数量 <= " + numericUpDownC.Value.ToString() + ") ";
                    break;
                case "2":
                    sqlComm.CommandText += " AND (库存表.库存数量 >= " + numericUpDownC.Value.ToString() + ") ";
                    break;
                default:
                    break;
            }
            if (alFL.Count > 0)
            {
                for (i = 0; i < alFL.Count; i++)
                {
                    cGetInformation.getUnderClassInformation(int.Parse(alFL[i].ToString()));
                    if (i == 0)
                        sqlComm.CommandText += " AND ((商品表.分类编号 = " + alFL[i].ToString() + ")";
                    else
                        sqlComm.CommandText += " OR (商品表.分类编号 = " + alFL[i].ToString() + ")";
                    for (j = 0; j < cGetInformation.intUnderClassNumber; j++)
                        sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[j].ToString() + ")";

                }
                sqlComm.CommandText += ")";
            }

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");

            sqlConn.Close();

            decimal dTemp = 0, dTemp1 = 0; ;
            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                if (dSet.Tables["商品表"].Rows[i][2].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][2] = 0;
                if (dSet.Tables["商品表"].Rows[i][4].ToString() == "")
                    dSet.Tables["商品表"].Rows[i][4] = 0;
                dTemp += Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][4]);
                dTemp1 += Convert.ToDecimal(dSet.Tables["商品表"].Rows[i][2]);

            }
            //labelKCJEHJ.Text = dTemp.ToString("f2");
            labelKCSLHJ.Text = dTemp1.ToString("f0");

            dataGridView1.DataSource = dSet.Tables["商品表"];

            int k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[5];
            decimal[] dSum1 = new decimal[5];

            for (t = 0; t < dSum1.Length; t++)
                dSum1[t] = 0;


            sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM 商品分类表 ORDER BY 上级分类";
            if (dSet.Tables.Contains("商品分类表")) dSet.Tables.Remove("商品分类表");
            sqlDA.Fill(dSet, "商品分类表");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("库房名称", System.Type.GetType("System.String"));
            dTable.Columns.Add("商品名称", System.Type.GetType("System.String"));
            dTable.Columns.Add("库存数量", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("库存成本价", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("库存金额", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("进价", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("批发价", System.Type.GetType("System.Decimal"));


            DataRow[] dtC = dSet.Tables["商品分类表"].Select("上级分类 = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[7];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];

                for (t = 2; t < oTemp.Length; t++)
                    oTemp[t] = 0;


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["商品分类表"].Select("上级分类 = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[7];
                    oTemp1[0] = dtC1[j][1];
                    oTemp1[1] = "　　" + dtC1[j][2];
                    //oTemp1[8] = dtC1[j][0];
                    //oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;
                    for (t = 2; t < oTemp1.Length; t++)
                        oTemp1[t] = 0;

                    dTable.Rows.Add(oTemp1);
                    iRow1 = dTable.Rows.Count - 1;

                    DataRow[] dtC2 = dSet.Tables["商品表"].Select("分类编号 = " + dtC1[j][0]);

                    for (t = 0; t < dSum.Length; t++)
                        dSum[t] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {

                        for (t = 0; t < dSum.Length; t++)
                        {
                            if (t == 0 || t == 2)
                                dSum[t] += Convert.ToDecimal(dtC2[k][t + 2].ToString());
                        }


                        if (bMX)
                        {
                            object[] oTemp2 = new object[4];
                            for (t = 0; t < oTemp2.Length; t++)
                                oTemp2[t] = dtC2[k][t];
                            oTemp2[1] = "　　　　" + dtC2[k][1];

                            dTable.Rows.Add(oTemp2);
                        }
                    }

                    for (t = 2; t < dSum.Length + 2; t++)
                        dTable.Rows[iRow1][t] = dSum[t - 2];


                    for (t = 0; t < dSum1.Length; t++)
                        dSum1[t] += dSum[t];


                    for (t = 2; t < dSum.Length + 2; t++)
                        dTable.Rows[iRow0][t] = Convert.ToDecimal(dTable.Rows[iRow0][t]) + Convert.ToDecimal(dTable.Rows[iRow1][t]);
                }


            }



            dataGridView1.DataSource = dTable;
            toolStripStatusLabel1.Text = "";
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            //权限控制
            if (intUserLimit >= LIMITACCESS)
            {
                labelKCJEHJ.Text = dTemp.ToString("f2");
            }
            else
            {
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                labelKCJEHJ.Visible = false;
            }
 
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "库存查询;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, true,intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "库存查询;当前日期：" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, false,intUserLimit);
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                //添加行号 
                SolidBrush v_SolidBrush = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor);
                int v_LineNo = 0;
                v_LineNo = e.RowIndex + 1;
                string v_Line = v_LineNo.ToString();
                e.Graphics.DrawString(v_Line, e.InheritedRowStyle.Font, v_SolidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加行号时发生错误，错误信息：" + ex.Message, "操作失败");
            }
        }

        private void toolStripButtonACompany_Click(object sender, EventArgs e)
        {
            intKFID = 0;
            textBoxKFBH.Text = "";
            textBoxKFMC.Text = "";
        }

        private void toolStripButtonASP_Click(object sender, EventArgs e)
        {
            intCommID = 0;
            textBoxSPBH.Text = "";
            textBoxSPMC.Text = "";
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!bDPFX)
                return;

            if (dataGridView1.SelectedRows.Count < 1)
                return;

            if (dataGridView1.SelectedRows[0].Cells[0].Value.ToString()=="")
                return;

            // 创建此子窗体的一个新实例。
            FormSPDPFX childFormSPDPFX = new FormSPDPFX();
            // 在显示该窗体前使其成为此 MDI 窗体的子窗体。
            childFormSPDPFX.MdiParent = this.MdiParent;

            childFormSPDPFX.strConn = strConn;

            childFormSPDPFX.intUserID = intUserID;
            childFormSPDPFX.intUserLimit = intUserLimit;
            childFormSPDPFX.strUserLimit = strUserLimit;
            childFormSPDPFX.strUserName = strUserName;
            childFormSPDPFX.intCommID = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

            childFormSPDPFX.Show();
        }

        private void textBoxSPLB_DoubleClick(object sender, EventArgs e)
        {
            FormSelectClassList frmSelectClassList = new FormSelectClassList();
            frmSelectClassList.strConn = strConn;
            frmSelectClassList.ShowDialog();

            if (frmSelectClassList.bSEL)
            {
                textBoxSPLB.Text = "";
                alFL.Clear();
                for (int i = 0; i < frmSelectClassList.checkedListBoxFL.Items.Count; i++)
                {
                    if (frmSelectClassList.checkedListBoxFL.GetItemChecked(i))
                    {
                        alFL.Add(frmSelectClassList.alFL[i]);
                        textBoxSPLB.Text += " " + frmSelectClassList.checkedListBoxFL.Items[i];
                    }

                }
            }

        }



    }




}