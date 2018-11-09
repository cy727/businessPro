using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormTQXSBJFX : Form
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

        private int iJZID = 0;
        private string SDTY0 = "", SDTY1 = "", SDTY2 = "";//时间：年，月，期
        private string SDTM0 = "", SDTM1 = "", SDTM2 = "";
        private string SDTQ0 = "", SDTQ1 = "", SDTQ2 = "";
        private string SDTS0 = "";

        private ClassGetInformation cGetInformation;

        private int intCommID = 0;
        private int iCompanyID = 0;

        private int[] iCount = { 0, 0, 0 };


        
        public FormTQXSBJFX()
        {
            InitializeComponent();
        }

        private void FormTQXSBJFX_Load(object sender, EventArgs e)
        {
            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;

            //得到时间

            //年时间
            SDTY0 = DateTime.Parse(strDT).AddYears(-1).Year.ToString() + "-1-1";
            SDTY1 = DateTime.Parse(strDT).Year.ToString() + "-1-1";
            SDTY2 = DateTime.Parse(strDT).AddDays(1).ToShortDateString();

            //月时间
            SDTM0 = DateTime.Parse(strDT).AddMonths(-1).Year.ToString() + "-"+DateTime.Parse(strDT).AddMonths(-1).Month.ToString()+"-1";
            SDTM1 = DateTime.Parse(strDT).Year.ToString() + "-" + DateTime.Parse(strDT).Month.ToString() + "-1";
            SDTM2 = DateTime.Parse(strDT).AddDays(1).ToShortDateString();


            //期时间
            //得到上次结转
            //得到开始时间
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 开始时间 FROM 系统参数表";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                SDTS0 = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT 结算时间,ID FROM 结转汇总表 ORDER BY 结算时间 DESC";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                try
                {
                    sqldr.Read();
                    iJZID = Convert.ToInt32(sqldr.GetValue(1).ToString());
                    SDTQ1 = Convert.ToDateTime(sqldr.GetValue(0).ToString()).AddDays(1).ToShortDateString();

                    sqldr.Read();
                    iJZID = Convert.ToInt32(sqldr.GetValue(1).ToString());
                    SDTQ0 = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();

                }
                catch
                {
                }
            }
            sqldr.Close();
            if (SDTQ1 == "")
                SDTQ1 = SDTS0;

            if (SDTQ0 == "")
                SDTQ0 = SDTS0;


            SDTQ2 = DateTime.Parse(strDT).AddDays(1).ToShortDateString();

            

            //初始化员工列表
            sqlComm.CommandText = "SELECT ID, 职员姓名 FROM 职员表 WHERE (beactive = 1)";

            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            DataRow drTemp = dSet.Tables["职员表"].NewRow();
            drTemp[0] = 0;
            drTemp[1] = "全部";
            dSet.Tables["职员表"].Rows.Add(drTemp);


            comboBoxYWY.DataSource = dSet.Tables["职员表"];
            comboBoxYWY.DisplayMember = "职员姓名";
            comboBoxYWY.ValueMember = "ID";
            comboBoxYWY.Text = strUserName;
            comboBoxYWY.SelectedValue = 0;

            sqlConn.Close();

            toolStripButtonGD_Click(null, null);

        }

        private void adjustDataView()
        {
            int i, j;
            decimal[] dTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                for (j = 2; j < dSet.Tables["商品表"].Columns.Count; j++)
                {
                    if (dSet.Tables["商品表"].Rows[i][j].ToString() == "")
                        dSet.Tables["商品表"].Rows[i][j] = 0;
                }
                dSet.Tables["商品表"].Rows[i][12] = decimal.Parse(dSet.Tables["商品表"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][4].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][10].ToString());
                dSet.Tables["商品表"].Rows[i][13] = decimal.Parse(dSet.Tables["商品表"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][5].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][11].ToString());

                dSet.Tables["商品表"].Rows[i][14] = decimal.Parse(dSet.Tables["商品表"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][6].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][4].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][8].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][10].ToString());
                dSet.Tables["商品表"].Rows[i][15] = decimal.Parse(dSet.Tables["商品表"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][7].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][5].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][9].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][11].ToString());

                dSet.Tables["商品表"].Rows[i][26] = decimal.Parse(dSet.Tables["商品表"].Rows[i][16].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][18].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][24].ToString());
                dSet.Tables["商品表"].Rows[i][27] = decimal.Parse(dSet.Tables["商品表"].Rows[i][17].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][19].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][25].ToString());

                dSet.Tables["商品表"].Rows[i][28] = decimal.Parse(dSet.Tables["商品表"].Rows[i][16].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][20].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][18].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][22].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][24].ToString());
                dSet.Tables["商品表"].Rows[i][29] = decimal.Parse(dSet.Tables["商品表"].Rows[i][17].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][21].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][19].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][23].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][25].ToString());


                dSet.Tables["商品表"].Rows[i][40] = decimal.Parse(dSet.Tables["商品表"].Rows[i][30].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][32].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][38].ToString());
                dSet.Tables["商品表"].Rows[i][41] = decimal.Parse(dSet.Tables["商品表"].Rows[i][31].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][33].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][39].ToString());

                dSet.Tables["商品表"].Rows[i][42] = decimal.Parse(dSet.Tables["商品表"].Rows[i][30].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][34].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][32].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][36].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][38].ToString());
                dSet.Tables["商品表"].Rows[i][43] = decimal.Parse(dSet.Tables["商品表"].Rows[i][31].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][35].ToString()) - decimal.Parse(dSet.Tables["商品表"].Rows[i][33].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][37].ToString()) + decimal.Parse(dSet.Tables["商品表"].Rows[i][39].ToString());



                for (j = 2; j < dSet.Tables["商品表"].Columns.Count; j++)
                {
                    dTemp[j-2] += decimal.Parse(dSet.Tables["商品表"].Rows[i][j].ToString());
                }
            }

            DataRow drT1 = dSet.Tables["商品表"].NewRow();
            drT1[1] = "合计";
            for (j = 2; j < dSet.Tables["商品表"].Columns.Count; j++)
            {
                drT1[j] = dTemp[j - 2];
            }
            dSet.Tables["商品表"].Rows.Add(drT1);

            for (j = 2; j < dSet.Tables["单位表"].Columns.Count; j++)
            {
                dTemp[j - 2] = 0;
            }

            for (i = 0; i < dSet.Tables["单位表"].Rows.Count; i++)
            {
                for (j = 2; j < dSet.Tables["单位表"].Columns.Count; j++)
                {
                    if (dSet.Tables["单位表"].Rows[i][j].ToString() == "")
                        dSet.Tables["单位表"].Rows[i][j] = 0;
                }
                dSet.Tables["单位表"].Rows[i][12] = decimal.Parse(dSet.Tables["单位表"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][4].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][10].ToString());
                dSet.Tables["单位表"].Rows[i][13] = decimal.Parse(dSet.Tables["单位表"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][5].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][11].ToString());

                dSet.Tables["单位表"].Rows[i][14] = decimal.Parse(dSet.Tables["单位表"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][6].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][4].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][8].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][10].ToString());
                dSet.Tables["单位表"].Rows[i][15] = decimal.Parse(dSet.Tables["单位表"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][7].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][5].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][9].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][11].ToString());

                dSet.Tables["单位表"].Rows[i][26] = decimal.Parse(dSet.Tables["单位表"].Rows[i][16].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][18].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][24].ToString());
                dSet.Tables["单位表"].Rows[i][27] = decimal.Parse(dSet.Tables["单位表"].Rows[i][17].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][19].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][25].ToString());

                dSet.Tables["单位表"].Rows[i][28] = decimal.Parse(dSet.Tables["单位表"].Rows[i][16].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][20].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][18].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][22].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][24].ToString());
                dSet.Tables["单位表"].Rows[i][29] = decimal.Parse(dSet.Tables["单位表"].Rows[i][17].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][21].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][19].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][23].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][25].ToString());


                dSet.Tables["单位表"].Rows[i][40] = decimal.Parse(dSet.Tables["单位表"].Rows[i][30].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][32].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][38].ToString());
                dSet.Tables["单位表"].Rows[i][41] = decimal.Parse(dSet.Tables["单位表"].Rows[i][31].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][33].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][39].ToString());

                dSet.Tables["单位表"].Rows[i][42] = decimal.Parse(dSet.Tables["单位表"].Rows[i][30].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][34].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][32].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][36].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][38].ToString());
                dSet.Tables["单位表"].Rows[i][43] = decimal.Parse(dSet.Tables["单位表"].Rows[i][31].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][35].ToString()) - decimal.Parse(dSet.Tables["单位表"].Rows[i][33].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][37].ToString()) + decimal.Parse(dSet.Tables["单位表"].Rows[i][39].ToString());



                for (j = 2; j < dSet.Tables["单位表"].Columns.Count; j++)
                {
                    dTemp[j - 2] += decimal.Parse(dSet.Tables["单位表"].Rows[i][j].ToString());
                }
            }

            DataRow drT2 = dSet.Tables["单位表"].NewRow();
            drT2[1] = "合计";
            for (j = 2; j < dSet.Tables["单位表"].Columns.Count; j++)
            {
                drT2[j] = dTemp[j - 2];
            }
            dSet.Tables["单位表"].Rows.Add(drT2);



            for (j = 2; j < dSet.Tables["职员表"].Columns.Count; j++)
            {
                dTemp[j - 2] = 0;
            }

            for (i = 0; i < dSet.Tables["职员表"].Rows.Count; i++)
            {
                for (j = 2; j < dSet.Tables["职员表"].Columns.Count; j++)
                {
                    if (dSet.Tables["职员表"].Rows[i][j].ToString() == "")
                        dSet.Tables["职员表"].Rows[i][j] = 0;
                }
                dSet.Tables["职员表"].Rows[i][12] = decimal.Parse(dSet.Tables["职员表"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][4].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][10].ToString());
                dSet.Tables["职员表"].Rows[i][13] = decimal.Parse(dSet.Tables["职员表"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][5].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][11].ToString());

                dSet.Tables["职员表"].Rows[i][14] = decimal.Parse(dSet.Tables["职员表"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][6].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][4].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][8].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][10].ToString());
                dSet.Tables["职员表"].Rows[i][15] = decimal.Parse(dSet.Tables["职员表"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][7].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][5].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][9].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][11].ToString());

                dSet.Tables["职员表"].Rows[i][26] = decimal.Parse(dSet.Tables["职员表"].Rows[i][16].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][18].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][24].ToString());
                dSet.Tables["职员表"].Rows[i][27] = decimal.Parse(dSet.Tables["职员表"].Rows[i][17].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][19].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][25].ToString());

                dSet.Tables["职员表"].Rows[i][28] = decimal.Parse(dSet.Tables["职员表"].Rows[i][16].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][20].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][18].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][22].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][24].ToString());
                dSet.Tables["职员表"].Rows[i][29] = decimal.Parse(dSet.Tables["职员表"].Rows[i][17].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][21].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][19].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][23].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][25].ToString());


                dSet.Tables["职员表"].Rows[i][40] = decimal.Parse(dSet.Tables["职员表"].Rows[i][30].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][32].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][38].ToString());
                dSet.Tables["职员表"].Rows[i][41] = decimal.Parse(dSet.Tables["职员表"].Rows[i][31].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][33].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][39].ToString());

                dSet.Tables["职员表"].Rows[i][42] = decimal.Parse(dSet.Tables["职员表"].Rows[i][30].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][34].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][32].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][36].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][38].ToString());
                dSet.Tables["职员表"].Rows[i][43] = decimal.Parse(dSet.Tables["职员表"].Rows[i][31].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][35].ToString()) - decimal.Parse(dSet.Tables["职员表"].Rows[i][33].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][37].ToString()) + decimal.Parse(dSet.Tables["职员表"].Rows[i][39].ToString());



                for (j = 2; j < dSet.Tables["职员表"].Columns.Count; j++)
                {
                    dTemp[j - 2] += decimal.Parse(dSet.Tables["职员表"].Rows[i][j].ToString());
                }
            }

            DataRow drT3 = dSet.Tables["职员表"].NewRow();
            drT3[1] = "合计";
            for (j = 2; j < dSet.Tables["职员表"].Columns.Count; j++)
            {
                drT3[j] = dTemp[j - 2];
            }
            dSet.Tables["职员表"].Rows.Add(drT3);




        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "同期销售比较分析（商品比较）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewSPBJ, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "同期销售比较分析（客户比较）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewKHBJ, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "同期销售比较分析（业务员比较）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewYWYBJ, strT, true, intUserLimit);
                    break;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "同期销售比较分析（商品比较）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewSPBJ, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "同期销售比较分析（客户比较）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewKHBJ, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "同期销售比较分析（业务员比较）;当前日期：" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewYWYBJ, strT, false, intUserLimit);
                    break;
            }
        }

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                intCommID = cGetInformation.iCommNumber;
                textBoxSPBH.Text = cGetInformation.strCommCode;
                textBoxSPMC.Text = cGetInformation.strCommName;
            }
        }

        private void textBoxSPBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                }

            }
        }

        private void textBoxSPMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    intCommID = cGetInformation.iCommNumber;
                    textBoxSPBH.Text = cGetInformation.strCommCode;
                    textBoxSPMC.Text = cGetInformation.strCommName;
                }

            }
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                iCompanyID = cGetInformation.iCompanyNumber;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
            }
        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1100, textBoxDWBH.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    iCompanyID = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                }

            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1300, textBoxDWMC.Text) == 0) //失败
                {
                    return;
                }
                else
                {
                    iCompanyID = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                }

            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabelMXJLS.Text = iCount[tabControl1.SelectedIndex].ToString();
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            string strA = "", strB = "", strC = "", strD = ""; //上月
            string strA1 = "", strB1 = "", strC1 = "", strD1 = "";//本月
            string strA2 = "", strB2 = "", strC2 = "", strD2 = "";//上年
            string strA3 = "", strB3 = "", strC3 = "", strD3 = "";//本年
            string strA4 = "", strB4 = "", strC4 = "", strD4 = "";//上期
            string strA5 = "", strB5 = "", strC5 = "", strD5 = "";//本期

            int i;

            strA = "SELECT 销售商品制单明细表.商品ID, SUM(销售商品制单明细表.实计金额) AS 上月销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 上月销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA += " GROUP BY 销售商品制单明细表.商品ID";

            strA1 = "SELECT 销售商品制单明细表.商品ID, SUM(销售商品制单明细表.实计金额) AS 本月销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 本月销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA1 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA1 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA1 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA1 += " GROUP BY 销售商品制单明细表.商品ID";


            strA2 = "SELECT 销售商品制单明细表.商品ID, SUM(销售商品制单明细表.实计金额) AS 上年销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 上年销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA2 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA2 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA2 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA2 += " GROUP BY 销售商品制单明细表.商品ID";

            strA3 = "SELECT 销售商品制单明细表.商品ID, SUM(销售商品制单明细表.实计金额) AS 本年销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 本年销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA3 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA3 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA3 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA3 += " GROUP BY 销售商品制单明细表.商品ID";

            strA4 = "SELECT 销售商品制单明细表.商品ID, SUM(销售商品制单明细表.实计金额) AS 上期销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 上期销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA4 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA4 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA4 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA4 += " GROUP BY 销售商品制单明细表.商品ID";

            strA5 = "SELECT 销售商品制单明细表.商品ID, SUM(销售商品制单明细表.实计金额) AS 本期销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 本期销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA5 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA5 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA5 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA5 += " GROUP BY 销售商品制单明细表.商品ID";

            strB = "SELECT 销售退出明细表.商品ID, SUM(销售退出明细表.实计金额) AS 上月退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 上月退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB += " GROUP BY 销售退出明细表.商品ID";

            strB1 = "SELECT 销售退出明细表.商品ID, SUM(销售退出明细表.实计金额) AS 本月退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 本月退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB1 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB1 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB1 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB1 += " GROUP BY 销售退出明细表.商品ID";

            strB2 = "SELECT 销售退出明细表.商品ID, SUM(销售退出明细表.实计金额) AS 上年退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 上年退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB2 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB2 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB2 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB2 += " GROUP BY 销售退出明细表.商品ID";

            strB3 = "SELECT 销售退出明细表.商品ID, SUM(销售退出明细表.实计金额) AS 本年退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 本年退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB3 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB3 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB3 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB3 += " GROUP BY 销售退出明细表.商品ID";

            strB4 = "SELECT 销售退出明细表.商品ID, SUM(销售退出明细表.实计金额) AS 上期退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 上期退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB4 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB4 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB4 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB4 += " GROUP BY 销售退出明细表.商品ID";

            strB5 = "SELECT 销售退出明细表.商品ID, SUM(销售退出明细表.实计金额) AS 本期退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 本期退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB5 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB5 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB5 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB5 += " GROUP BY 销售退出明细表.商品ID";


            strC = "SELECT  销售退补差价明细表.商品ID, SUM(销售退补差价明细表.金额) AS 上月退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY 销售退补差价明细表.商品ID";

            strC1 = "SELECT  销售退补差价明细表.商品ID, SUM(销售退补差价明细表.金额) AS 本月退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC1 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC1 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC1 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC1 += " GROUP BY 销售退补差价明细表.商品ID";

            strC2 = "SELECT  销售退补差价明细表.商品ID, SUM(销售退补差价明细表.金额) AS 上年退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC2 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC2 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC2 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC2 += " GROUP BY 销售退补差价明细表.商品ID";

            strC3 = "SELECT  销售退补差价明细表.商品ID, SUM(销售退补差价明细表.金额) AS 本年退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC3 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC3 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC3 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC3 += " GROUP BY 销售退补差价明细表.商品ID";

            strC4 = "SELECT  销售退补差价明细表.商品ID, SUM(销售退补差价明细表.金额) AS 上期退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC4 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC4 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC4 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC4 += " GROUP BY 销售退补差价明细表.商品ID";

            strC5 = "SELECT  销售退补差价明细表.商品ID, SUM(销售退补差价明细表.金额) AS 本期退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC5 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC5 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC5 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC5 += " GROUP BY 销售退补差价明细表.商品ID";


            sqlComm.CommandText = "SELECT 商品表.商品名称,商品表.商品规格,本月销售表.本月销售金额, 上月销售表.上月销售金额, 本月退出表.本月退出金额, 上月退出表.上月退出金额, 本月销售表.本月销售成本, 上月销售表.上月销售成本, 本月退出表.本月退出成本,上月退出表.上月退出成本,本月退补表.本月退补金额,上月退补表.上月退补金额,0.00 AS 本月实销金额, 0.00 AS 上月实销金额, 0.00 AS 本月毛利,0.00 AS 上月毛利, 本年销售表.本年销售金额, 上年销售表.上年销售金额, 本年退出表.本年退出金额, 上年退出表.上年退出金额, 本年销售表.本年销售成本, 上年销售表.上年销售成本, 本年退出表.本年退出成本,上年退出表.上年退出成本,本年退补表.本年退补金额,上年退补表.上年退补金额,0.00 AS 本年实销金额, 0.00 AS 上年实销金额, 0.00 AS 本年毛利,0.00 AS 上年毛利, 本期销售表.本期销售金额, 上期销售表.上期销售金额, 本期退出表.本期退出金额, 上期退出表.上期退出金额, 本期销售表.本期销售成本, 上期销售表.上期销售成本, 本期退出表.本期退出成本,上期退出表.上期退出成本,本期退补表.本期退补金额,上期退补表.上期退补金额,0.00 AS 本期实销金额, 0.00 AS 上期实销金额, 0.00 AS 本期毛利,0.00 AS 上期毛利 FROM 商品表 LEFT OUTER JOIN (" + strA + ") 上月销售表 ON 上月销售表.商品ID = 商品表.ID LEFT OUTER JOIN (" + strA1 + ") 本月销售表 ON 本月销售表.商品ID = 商品表.ID LEFT OUTER JOIN (" + strA2 + ") 上年销售表 ON 上年销售表.商品ID = 商品表.ID  LEFT OUTER JOIN (" + strA3 + ") 本年销售表 ON 本年销售表.商品ID = 商品表.ID  LEFT OUTER JOIN (" + strA4 + ") 上期销售表 ON 上期销售表.商品ID = 商品表.ID  LEFT OUTER JOIN (" + strA5 + ") 本期销售表 ON 本期销售表.商品ID = 商品表.ID  LEFT OUTER JOIN (" + strB + ") 上月退出表 ON 上月退出表.商品ID = 商品表.ID  LEFT OUTER JOIN (" + strB1 + ") 本月退出表 ON 本月退出表.商品ID = 商品表.ID LEFT OUTER JOIN (" + strB2 + ") 上年退出表 ON 上年退出表.商品ID = 商品表.ID LEFT OUTER JOIN (" + strB3 + ") 本年退出表 ON 本年退出表.商品ID = 商品表.ID LEFT OUTER JOIN (" + strB4 + ") 上期退出表 ON 上期退出表.商品ID = 商品表.ID LEFT OUTER JOIN (" + strB5 + ") 本期退出表 ON 本期退出表.商品ID = 商品表.ID LEFT OUTER JOIN (" + strC + ") 上月退补表 ON 上月退补表.商品ID = 商品表.ID LEFT OUTER JOIN (" + strC1 + ") 本月退补表 ON 本月退补表.商品ID = 商品表.ID LEFT OUTER JOIN (" + strC2 + ") 上年退补表 ON 上年退补表.商品ID = 商品表.ID LEFT OUTER JOIN (" + strC3 + ") 本年退补表 ON 本年退补表.商品ID = 商品表.ID LEFT OUTER JOIN (" + strC4 + ") 上期退补表 ON 上期退补表.商品ID = 商品表.ID LEFT OUTER JOIN (" + strC5 + ") 本期退补表 ON 本期退补表.商品ID = 商品表.ID WHERE (商品表.组装商品 = 0)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                sqlComm.CommandText += " AND (商品表.ID = " + intCommID.ToString() + ") ";
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");
            iCount[0] = dSet.Tables["商品表"].Rows.Count;


            //**********单位ID 销售商品制单表.单位ID
            strA = "SELECT 销售商品制单表.单位ID, SUM(销售商品制单明细表.实计金额) AS 上月销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 上月销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA += " GROUP BY 销售商品制单表.单位ID";

            strA1 = "SELECT 销售商品制单表.单位ID, SUM(销售商品制单明细表.实计金额) AS 本月销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 本月销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA1 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA1 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA1 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA1 += " GROUP BY 销售商品制单表.单位ID";


            strA2 = "SELECT 销售商品制单表.单位ID, SUM(销售商品制单明细表.实计金额) AS 上年销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 上年销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA2 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA2 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA2 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA2 += " GROUP BY 销售商品制单表.单位ID";

            strA3 = "SELECT 销售商品制单表.单位ID, SUM(销售商品制单明细表.实计金额) AS 本年销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 本年销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA3 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA3 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA3 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA3 += " GROUP BY 销售商品制单表.单位ID";

            strA4 = "SELECT 销售商品制单表.单位ID, SUM(销售商品制单明细表.实计金额) AS 上期销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 上期销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA4 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA4 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA4 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA4 += " GROUP BY 销售商品制单表.单位ID";

            strA5 = "SELECT 销售商品制单表.单位ID, SUM(销售商品制单明细表.实计金额) AS 本期销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 本期销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA5 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA5 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA5 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA5 += " GROUP BY 销售商品制单表.单位ID";

            strB = "SELECT 销售退出汇总表.单位ID, SUM(销售退出明细表.实计金额) AS 上月退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 上月退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB += " GROUP BY 销售退出汇总表.单位ID";

            strB1 = "SELECT 销售退出汇总表.单位ID, SUM(销售退出明细表.实计金额) AS 本月退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 本月退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB1 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB1 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB1 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB1 += " GROUP BY 销售退出汇总表.单位ID";

            strB2 = "SELECT 销售退出汇总表.单位ID, SUM(销售退出明细表.实计金额) AS 上年退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 上年退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB2 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB2 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB2 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB2 += " GROUP BY 销售退出汇总表.单位ID";

            strB3 = "SELECT 销售退出汇总表.单位ID, SUM(销售退出明细表.实计金额) AS 本年退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 本年退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB3 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB3 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB3 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB3 += " GROUP BY 销售退出汇总表.单位ID";

            strB4 = "SELECT 销售退出汇总表.单位ID, SUM(销售退出明细表.实计金额) AS 上期退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 上期退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB4 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB4 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB4 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB4 += " GROUP BY 销售退出汇总表.单位ID";

            strB5 = "SELECT 销售退出汇总表.单位ID, SUM(销售退出明细表.实计金额) AS 本期退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 本期退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB5 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB5 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB5 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB5 += " GROUP BY 销售退出汇总表.单位ID";


            strC = "SELECT  销售退补差价汇总表.单位ID, SUM(销售退补差价明细表.金额) AS 上月退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY 销售退补差价汇总表.单位ID";

            strC1 = "SELECT  销售退补差价汇总表.单位ID, SUM(销售退补差价明细表.金额) AS 本月退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC1 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC1 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC1 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC1 += " GROUP BY 销售退补差价汇总表.单位ID";

            strC2 = "SELECT  销售退补差价汇总表.单位ID, SUM(销售退补差价明细表.金额) AS 上年退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC2 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC2 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC2 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC2 += " GROUP BY 销售退补差价汇总表.单位ID";

            strC3 = "SELECT  销售退补差价汇总表.单位ID, SUM(销售退补差价明细表.金额) AS 本年退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC3 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC3 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC3 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC3 += " GROUP BY 销售退补差价汇总表.单位ID";

            strC4 = "SELECT  销售退补差价汇总表.单位ID, SUM(销售退补差价明细表.金额) AS 上期退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC4 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC4 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC4 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC4 += " GROUP BY 销售退补差价汇总表.单位ID";

            strC5 = "SELECT  销售退补差价汇总表.单位ID, SUM(销售退补差价明细表.金额) AS 本期退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC5 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC5 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC5 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC5 += " GROUP BY 销售退补差价汇总表.单位ID";


            sqlComm.CommandText = "SELECT 单位表.单位编号,单位表.单位名称,本月销售表.本月销售金额, 上月销售表.上月销售金额, 本月退出表.本月退出金额, 上月退出表.上月退出金额, 本月销售表.本月销售成本, 上月销售表.上月销售成本, 本月退出表.本月退出成本,上月退出表.上月退出成本,本月退补表.本月退补金额,上月退补表.上月退补金额,0.00 AS 本月实销金额, 0.00 AS 上月实销金额, 0.00 AS 本月毛利,0.00 AS 上月毛利, 本年销售表.本年销售金额, 上年销售表.上年销售金额, 本年退出表.本年退出金额, 上年退出表.上年退出金额, 本年销售表.本年销售成本, 上年销售表.上年销售成本, 本年退出表.本年退出成本,上年退出表.上年退出成本,本年退补表.本年退补金额,上年退补表.上年退补金额,0.00 AS 本年实销金额, 0.00 AS 上年实销金额, 0.00 AS 本年毛利,0.00 AS 上年毛利, 本期销售表.本期销售金额, 上期销售表.上期销售金额, 本期退出表.本期退出金额, 上期退出表.上期退出金额, 本期销售表.本期销售成本, 上期销售表.上期销售成本, 本期退出表.本期退出成本,上期退出表.上期退出成本,本期退补表.本期退补金额,上期退补表.上期退补金额,0.00 AS 本期实销金额, 0.00 AS 上期实销金额, 0.00 AS 本期毛利,0.00 AS 上期毛利 FROM 单位表 LEFT OUTER JOIN (" + strA + ") 上月销售表 ON 上月销售表.单位ID = 单位表.ID LEFT OUTER JOIN (" + strA1 + ") 本月销售表 ON 本月销售表.单位ID = 单位表.ID LEFT OUTER JOIN (" + strA2 + ") 上年销售表 ON 上年销售表.单位ID = 单位表.ID  LEFT OUTER JOIN (" + strA3 + ") 本年销售表 ON 本年销售表.单位ID = 单位表.ID  LEFT OUTER JOIN (" + strA4 + ") 上期销售表 ON 上期销售表.单位ID = 单位表.ID  LEFT OUTER JOIN (" + strA5 + ") 本期销售表 ON 本期销售表.单位ID = 单位表.ID  LEFT OUTER JOIN (" + strB + ") 上月退出表 ON 上月退出表.单位ID = 单位表.ID  LEFT OUTER JOIN (" + strB1 + ") 本月退出表 ON 本月退出表.单位ID = 单位表.ID LEFT OUTER JOIN (" + strB2 + ") 上年退出表 ON 上年退出表.单位ID = 单位表.ID LEFT OUTER JOIN (" + strB3 + ") 本年退出表 ON 本年退出表.单位ID = 单位表.ID LEFT OUTER JOIN (" + strB4 + ") 上期退出表 ON 上期退出表.单位ID = 单位表.ID LEFT OUTER JOIN (" + strB5 + ") 本期退出表 ON 本期退出表.单位ID = 单位表.ID LEFT OUTER JOIN (" + strC + ") 上月退补表 ON 上月退补表.单位ID = 单位表.ID LEFT OUTER JOIN (" + strC1 + ") 本月退补表 ON 本月退补表.单位ID = 单位表.ID LEFT OUTER JOIN (" + strC2 + ") 上年退补表 ON 上年退补表.单位ID = 单位表.ID LEFT OUTER JOIN (" + strC3 + ") 本年退补表 ON 本年退补表.单位ID = 单位表.ID LEFT OUTER JOIN (" + strC4 + ") 上期退补表 ON 上期退补表.单位ID = 单位表.ID LEFT OUTER JOIN (" + strC5 + ") 本期退补表 ON 本期退补表.单位ID = 单位表.ID";

            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                sqlComm.CommandText += " WHERE (单位表.ID=" + iCompanyID.ToString() + ") ";
            }

            if (dSet.Tables.Contains("单位表")) dSet.Tables.Remove("单位表");
            sqlDA.Fill(dSet, "单位表");
            iCount[1] = dSet.Tables["单位表"].Rows.Count;

            //**********业务员ID 销售商品制单表.业务员ID
            strA = "SELECT 销售商品制单表.业务员ID, SUM(销售商品制单明细表.实计金额) AS 上月销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 上月销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA += " GROUP BY 销售商品制单表.业务员ID";

            strA1 = "SELECT 销售商品制单表.业务员ID, SUM(销售商品制单明细表.实计金额) AS 本月销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 本月销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA1 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA1 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA1 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA1 += " GROUP BY 销售商品制单表.业务员ID";


            strA2 = "SELECT 销售商品制单表.业务员ID, SUM(销售商品制单明细表.实计金额) AS 上年销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 上年销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA2 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA2 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA2 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA2 += " GROUP BY 销售商品制单表.业务员ID";

            strA3 = "SELECT 销售商品制单表.业务员ID, SUM(销售商品制单明细表.实计金额) AS 本年销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 本年销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA3 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA3 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA3 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA3 += " GROUP BY 销售商品制单表.业务员ID";

            strA4 = "SELECT 销售商品制单表.业务员ID, SUM(销售商品制单明细表.实计金额) AS 上期销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 上期销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA4 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA4 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA4 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA4 += " GROUP BY 销售商品制单表.业务员ID";

            strA5 = "SELECT 销售商品制单表.业务员ID, SUM(销售商品制单明细表.实计金额) AS 本期销售金额, SUM(销售商品制单明细表.数量 * 销售商品制单明细表.库存成本价) AS 本期销售成本 FROM 销售商品制单表 INNER JOIN 销售商品制单明细表 ON 销售商品制单表.ID = 销售商品制单明细表.表单ID INNER JOIN 单位表 ON 销售商品制单表.单位ID = 单位表.ID INNER JOIN 商品表 ON 销售商品制单明细表.商品ID = 商品表.ID WHERE (销售商品制单表.日期 >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售商品制单表.日期 < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (销售商品制单表.BeActive=1) AND (单位表.BeActive = 1) AND (商品表.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strA5 += " AND (销售商品制单明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA5 += " AND (销售商品制单表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA5 += " AND (销售商品制单表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA5 += " GROUP BY 销售商品制单表.业务员ID";

            strB = "SELECT 销售退出汇总表.业务员ID, SUM(销售退出明细表.实计金额) AS 上月退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 上月退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB += " GROUP BY 销售退出汇总表.业务员ID";

            strB1 = "SELECT 销售退出汇总表.业务员ID, SUM(销售退出明细表.实计金额) AS 本月退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 本月退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB1 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB1 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB1 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB1 += " GROUP BY 销售退出汇总表.业务员ID";

            strB2 = "SELECT 销售退出汇总表.业务员ID, SUM(销售退出明细表.实计金额) AS 上年退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 上年退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB2 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB2 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB2 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB2 += " GROUP BY 销售退出汇总表.业务员ID";

            strB3 = "SELECT 销售退出汇总表.业务员ID, SUM(销售退出明细表.实计金额) AS 本年退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 本年退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB3 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB3 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB3 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB3 += " GROUP BY 销售退出汇总表.业务员ID";

            strB4 = "SELECT 销售退出汇总表.业务员ID, SUM(销售退出明细表.实计金额) AS 上期退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 上期退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB4 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB4 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB4 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB4 += " GROUP BY 销售退出汇总表.业务员ID";

            strB5 = "SELECT 销售退出汇总表.业务员ID, SUM(销售退出明细表.实计金额) AS 本期退出金额, SUM(销售退出明细表.数量 * 销售退出明细表.库存成本价) AS 本期退出成本 FROM 销售退出汇总表 INNER JOIN 销售退出明细表 ON 销售退出汇总表.ID = 销售退出明细表.单据ID INNER JOIN 商品表 ON 销售退出明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退出汇总表.单位ID = 单位表.ID WHERE (销售退出汇总表.日期 >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售退出汇总表.日期 < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (销售退出汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strB5 += " AND (销售退出明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB5 += " AND (销售退出汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB5 += " AND (销售退出汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB5 += " GROUP BY 销售退出汇总表.业务员ID";

            strC = "SELECT  销售退补差价汇总表.业务员ID, SUM(销售退补差价明细表.金额) AS 上月退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY 销售退补差价汇总表.业务员ID";

            strC1 = "SELECT  销售退补差价汇总表.业务员ID, SUM(销售退补差价明细表.金额) AS 本月退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC1 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC1 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC1 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC1 += " GROUP BY 销售退补差价汇总表.业务员ID";

            strC2 = "SELECT  销售退补差价汇总表.业务员ID, SUM(销售退补差价明细表.金额) AS 上年退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC2 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC2 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC2 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC2 += " GROUP BY 销售退补差价汇总表.业务员ID";

            strC3 = "SELECT  销售退补差价汇总表.业务员ID, SUM(销售退补差价明细表.金额) AS 本年退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC3 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC3 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC3 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC3 += " GROUP BY 销售退补差价汇总表.业务员ID";

            strC4 = "SELECT  销售退补差价汇总表.业务员ID, SUM(销售退补差价明细表.金额) AS 上期退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC4 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC4 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC4 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC4 += " GROUP BY 销售退补差价汇总表.业务员ID";

            strC5 = "SELECT  销售退补差价汇总表.业务员ID, SUM(销售退补差价明细表.金额) AS 本期退补金额 FROM 销售退补差价汇总表 INNER JOIN 销售退补差价明细表 ON 销售退补差价汇总表.ID = 销售退补差价明细表.单据ID INNER JOIN 商品表 ON 销售退补差价明细表.商品ID = 商品表.ID INNER JOIN 单位表 ON 销售退补差价汇总表.单位ID = 单位表.ID WHERE (销售退补差价汇总表.日期 >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (销售退补差价汇总表.日期 < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (销售退补差价汇总表.BeActive=1) AND (商品表.beactive = 1) AND (单位表.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //有商品
            {
                strC5 += " AND (销售退补差价明细表.商品ID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC5 += " AND (销售退补差价汇总表.单位ID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC5 += " AND (销售退补差价汇总表.业务员ID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC5 += " GROUP BY 销售退补差价汇总表.业务员ID";

            sqlComm.CommandText = "SELECT 职员表.职员编号,职员表.职员姓名,本月销售表.本月销售金额, 上月销售表.上月销售金额, 本月退出表.本月退出金额, 上月退出表.上月退出金额, 本月销售表.本月销售成本, 上月销售表.上月销售成本, 本月退出表.本月退出成本,上月退出表.上月退出成本,本月退补表.本月退补金额,上月退补表.上月退补金额,0.00 AS 本月实销金额, 0.00 AS 上月实销金额, 0.00 AS 本月毛利,0.00 AS 上月毛利, 本年销售表.本年销售金额, 上年销售表.上年销售金额, 本年退出表.本年退出金额, 上年退出表.上年退出金额, 本年销售表.本年销售成本, 上年销售表.上年销售成本, 本年退出表.本年退出成本,上年退出表.上年退出成本,本年退补表.本年退补金额,上年退补表.上年退补金额,0.00 AS 本年实销金额, 0.00 AS 上年实销金额, 0.00 AS 本年毛利,0.00 AS 上年毛利, 本期销售表.本期销售金额, 上期销售表.上期销售金额, 本期退出表.本期退出金额, 上期退出表.上期退出金额, 本期销售表.本期销售成本, 上期销售表.上期销售成本, 本期退出表.本期退出成本,上期退出表.上期退出成本,本期退补表.本期退补金额,上期退补表.上期退补金额,0.00 AS 本期实销金额, 0.00 AS 上期实销金额, 0.00 AS 本期毛利,0.00 AS 上期毛利 FROM 职员表 LEFT OUTER JOIN (" + strA + ") 上月销售表 ON 上月销售表.业务员ID = 职员表.ID LEFT OUTER JOIN (" + strA1 + ") 本月销售表 ON 本月销售表.业务员ID = 职员表.ID LEFT OUTER JOIN (" + strA2 + ") 上年销售表 ON 上年销售表.业务员ID = 职员表.ID  LEFT OUTER JOIN (" + strA3 + ") 本年销售表 ON 本年销售表.业务员ID = 职员表.ID  LEFT OUTER JOIN (" + strA4 + ") 上期销售表 ON 上期销售表.业务员ID = 职员表.ID  LEFT OUTER JOIN (" + strA5 + ") 本期销售表 ON 本期销售表.业务员ID = 职员表.ID  LEFT OUTER JOIN (" + strB + ") 上月退出表 ON 上月退出表.业务员ID = 职员表.ID  LEFT OUTER JOIN (" + strB1 + ") 本月退出表 ON 本月退出表.业务员ID = 职员表.ID LEFT OUTER JOIN (" + strB2 + ") 上年退出表 ON 上年退出表.业务员ID = 职员表.ID LEFT OUTER JOIN (" + strB3 + ") 本年退出表 ON 本年退出表.业务员ID = 职员表.ID LEFT OUTER JOIN (" + strB4 + ") 上期退出表 ON 上期退出表.业务员ID = 职员表.ID LEFT OUTER JOIN (" + strB5 + ") 本期退出表 ON 本期退出表.业务员ID = 职员表.ID LEFT OUTER JOIN (" + strC + ") 上月退补表 ON 上月退补表.业务员ID = 职员表.ID LEFT OUTER JOIN (" + strC1 + ") 本月退补表 ON 本月退补表.业务员ID = 职员表.ID LEFT OUTER JOIN (" + strC2 + ") 上年退补表 ON 上年退补表.业务员ID = 职员表.ID LEFT OUTER JOIN (" + strC3 + ") 本年退补表 ON 本年退补表.业务员ID = 职员表.ID LEFT OUTER JOIN (" + strC4 + ") 上期退补表 ON 上期退补表.业务员ID = 职员表.ID LEFT OUTER JOIN (" + strC5 + ") 本期退补表 ON 本期退补表.业务员ID = 职员表.ID";

            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " WHERE (职员表.ID =" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }


            if (dSet.Tables.Contains("职员表")) dSet.Tables.Remove("职员表");
            sqlDA.Fill(dSet, "职员表");
            iCount[2] = dSet.Tables["职员表"].Rows.Count;


            sqlConn.Close();
            adjustDataView();
            dataGridViewSPBJ.DataSource = dSet.Tables["商品表"];
            for (i = 0; i < dataGridViewSPBJ.Columns.Count; i++)
            {
                dataGridViewSPBJ.Columns[i].DefaultCellStyle.Format = "f2";
            }


            dataGridViewKHBJ.DataSource = dSet.Tables["单位表"];

            for (i = 0; i < dataGridViewKHBJ.Columns.Count; i++)
            {
                dataGridViewKHBJ.Columns[i].DefaultCellStyle.Format = "f2";
            }

            dataGridViewYWYBJ.DataSource = dSet.Tables["职员表"];
            for (i = 0; i < dataGridViewYWYBJ.Columns.Count; i++)
            {
                dataGridViewYWYBJ.Columns[i].DefaultCellStyle.Format = "f2";
            }
            tabControl1_SelectedIndexChanged(null, null);

        }




    }
}