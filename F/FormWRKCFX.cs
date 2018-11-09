using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormWRKCFX : Form
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

        private int intClassID = 0;
        private int intCommID = 0;

        private string strDT0 = "", strDT1 = "", strDT2 = "", strDT3 = "";
        private ClassGetInformation cGetInformation;

        private const int NUMDAYS = 5;

        private DateTime[] Days = new DateTime[NUMDAYS];

        public FormWRKCFX()
        {
            InitializeComponent();
        }

        private void FormWRKCFX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;

            int i = 1;
            DateTime dtTemp = DateTime.Now;
            //得到开始时间
            Days[0] = dtTemp;
            while (true)
            {
                if (i >= NUMDAYS)
                    break;
                dtTemp=dtTemp.AddDays(-1);
                if ((int)dtTemp.DayOfWeek != 0 || (int)dtTemp.DayOfWeek != 6) //非周末
                {
                    Days[i] = dtTemp;
                    i++;
                }

            }


            cGetInformation.getSystemDateTime();
            strDT0 = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT0).ToString("yyyy年M月dd日");
            labelCZY.Text = strUserName;
        }

        private void checkBoxALL_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxALL.Checked)
            {
                intClassID = 0;
            }
        }

        private void textBoxSPLB_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getClassInformation(1, "") == 0) //失败
            {
                return;
            }
            else
            {
                intClassID = cGetInformation.iClassNumber;
                textBoxSPLB.Text = cGetInformation.strClassName;
                checkBoxALL.Checked = false;

            }
        }

        private void textBoxSPLB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getClassInformation(10, textBoxSPLB.Text) == 0) //失败
                {
                    textBoxSPLB.Text = "";
                    intClassID = 0;
                    checkBoxALL.Checked = true;

                }
                else
                {
                    intClassID = cGetInformation.iClassNumber;
                    textBoxSPLB.Text = cGetInformation.strClassName;
                    checkBoxALL.Checked = false;
                }
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

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i,j;


            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                cGetInformation.getUnderClassInformation(intClassID);
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品表.ID, 商品表.商品编号, 商品表.商品名称, 商品表.库存数量 FROM 商品表 WHERE (商品表.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0)
            {
                sqlComm.CommandText += " AND (商品表.ID = " + intCommID.ToString() + ")";
            }
            if (!checkBoxALL.Checked && intClassID != 0) //分类
            {
                sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                for (i = 0; i < cGetInformation.intUnderClassNumber; i++)
                    sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[i].ToString() + ")";
                sqlComm.CommandText += ") ";
            }
            sqlComm.CommandText += " ORDER BY 商品表.ID";

            if (dSet.Tables.Contains("商品表")) dSet.Tables.Remove("商品表");
            sqlDA.Fill(dSet, "商品表");

            for (i = 1; i < NUMDAYS-1; i++)
            {
                //sqlComm.CommandText = "SELECT 商品表.ID, A.总结存数量 FROM 商品表 LEFT OUTER JOIN (SELECT 商品ID, 总结存数量 FROM 商品历史账表 WHERE (ID IN (SELECT MAX(ID) AS ID FROM 商品历史账表 AS 商品历史账表_1 WHERE      (日期 >= CONVERT(DATETIME, '1999-01-01 00:00:00', 102)) AND (日期 =< CONVERT(DATETIME, '2010-01-01 23:59:59', 102)) AND (总结存数量 IS NOT NULL) AND (BeActive = 1) GROUP BY 商品ID))) AS A ON 商品表.ID = A.商品ID WHERE (商品表.beactive = 1) ORDER BY 商品表.ID";

                sqlComm.CommandText = "SELECT 商品表.ID, A.总结存数量 FROM 商品表 LEFT OUTER JOIN (SELECT 商品ID, 总结存数量 FROM 商品历史账表 WHERE (ID IN (SELECT MAX(ID) AS ID FROM 商品历史账表 AS 商品历史账表_1 WHERE      (日期 >= CONVERT(DATETIME, '" + Days[i].Year.ToString() + "-" + Days[i].Month.ToString() + "-" + Days[i].Day.ToString() + " 00:00:00', 102)) AND (日期 <= CONVERT(DATETIME, '" + Days[i].Year.ToString() + "-" + Days[i].Month.ToString() + "-" + Days[i].Day.ToString() + " 23:59:59', 102)) AND (总结存数量 IS NOT NULL) AND (BeActive = 1) GROUP BY 商品ID))) AS A ON 商品表.ID = A.商品ID WHERE (商品表.beactive = 1) ";

                if (!checkBoxALLSP.Checked && intCommID != 0)
                {
                    sqlComm.CommandText += " AND (商品表.ID = " + intCommID.ToString() + ")";
                }
                if (!checkBoxALL.Checked && intClassID != 0) //分类
                {
                    sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                    for (j = 0; j < cGetInformation.intUnderClassNumber; j++)
                        sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[j].ToString() + ")";
                    sqlComm.CommandText += ") ";
                }
                sqlComm.CommandText += " ORDER BY 商品表.ID";


                if (dSet.Tables.Contains("商品表" + i.ToString())) dSet.Tables.Remove("商品表" + i.ToString());
                sqlDA.Fill(dSet, "商品表" + i.ToString());

            }

            //最前一天
            sqlComm.CommandText = "SELECT 商品表.ID, A.总结存数量 FROM 商品表 LEFT OUTER JOIN (SELECT 商品ID, 总结存数量 FROM 商品历史账表 WHERE (ID IN (SELECT MAX(ID) AS ID FROM 商品历史账表 AS 商品历史账表_1 WHERE      (日期 <= CONVERT(DATETIME, '" + Days[NUMDAYS-1].Year.ToString() + "-" + Days[NUMDAYS-1].Month.ToString() + "-" + Days[NUMDAYS-1].Day.ToString() + " 00:00:00', 102)) AND (总结存数量 IS NOT NULL) AND (BeActive = 1) GROUP BY 商品ID))) AS A ON 商品表.ID = A.商品ID WHERE (商品表.beactive = 1) ";

                if (!checkBoxALLSP.Checked && intCommID != 0)
                {
                    sqlComm.CommandText += " AND (商品表.ID = " + intCommID.ToString() + ")";
                }
                if (!checkBoxALL.Checked && intClassID != 0) //分类
                {
                    sqlComm.CommandText += " AND ((商品表.分类编号 = " + intClassID.ToString() + ")";
                    for (j = 0; j < cGetInformation.intUnderClassNumber; j++)
                        sqlComm.CommandText += " OR (商品表.分类编号 = " + cGetInformation.intUnderClass[j].ToString() + ")";
                    sqlComm.CommandText += ") ";
                }
                sqlComm.CommandText += " ORDER BY 商品表.ID";


                if (dSet.Tables.Contains("商品表" + (NUMDAYS-1).ToString())) dSet.Tables.Remove("商品表" + (NUMDAYS-1).ToString());
                sqlDA.Fill(dSet, "商品表" + (NUMDAYS-1).ToString());


            //dTable.Columns.Add("分类编号", System.Type.GetType("System.Int32"));
            sqlConn.Close();

            DataTable dTable = new DataTable();
            dTable.Columns.Add("商品编号", System.Type.GetType("System.String"));
            dTable.Columns.Add("商品名称", System.Type.GetType("System.String"));

            for (i = 0; i < NUMDAYS; i++)
            {
                dTable.Columns.Add(Days[i].ToShortDateString(), System.Type.GetType("System.Decimal"));
            }

            for (i = 0; i < dSet.Tables["商品表"].Rows.Count; i++)
            {
                object[] oTemp = new object[NUMDAYS+2];

                oTemp[0] = dSet.Tables["商品表"].Rows[i][1].ToString();
                oTemp[1] = dSet.Tables["商品表"].Rows[i][2].ToString();

                if (dSet.Tables["商品表"].Rows[i][3].ToString() == "")
                {
                    oTemp[2] = 0;
                }
                else
                {
                    oTemp[2] = decimal.Parse(dSet.Tables["商品表"].Rows[i][3].ToString());
                }

                //最前一天
                if (dSet.Tables["商品表"+(NUMDAYS-1).ToString()].Rows[i][1].ToString() == "")
                {
                    oTemp[NUMDAYS+1] = 0;
                }
                else
                {
                    oTemp[NUMDAYS+1] = decimal.Parse(dSet.Tables["商品表"+(NUMDAYS-1).ToString()].Rows[i][1].ToString());
                }

                
                for(j = NUMDAYS - 1; j >= 2; j--)
                {
                    if (dSet.Tables["商品表" + j.ToString()].Rows[i][1].ToString() == "")
                    {
                        oTemp[j + 1] = oTemp[j+2];
                    }
                    else
                    {
                        oTemp[j + 1] = decimal.Parse(dSet.Tables["商品表" + j.ToString()].Rows[i][1].ToString());
                    }

                }
                 
                dTable.Rows.Add(oTemp);

            }
            dataGridViewDJMX.DataSource = dTable;

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {

        }
    }
}
