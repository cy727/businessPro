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
    public partial class FormSelectClassList : Form
    {
        public string strConn = "";
        public string strSelectText = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iClassNumber = 0;
        public string strClassName = "";
        public string strClassCode = "";

        public bool bSEL = false;

        public ArrayList alFL=new ArrayList();

        public FormSelectClassList()
        {
            InitializeComponent();
        }

        private void FormSelectClassList_Load(object sender, EventArgs e)
        {

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            initCommLIST();
        }

        private void initCommLIST()
        {
            string strTemp;
            int iTemp,j;

            checkedListBoxFL.Items.Clear();
            alFL.Clear();

            sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM  商品分类表 WHERE (BeActive = 1) AND (上级分类 NOT LIKE N'%,%')ORDER BY 上级分类, 分类编号";

            sqlConn.Open();
            
            if (dSet.Tables.Contains("商品分类表")) dSet.Tables.Remove("商品分类表");
            sqlDA.Fill(dSet, "商品分类表");

            sqlComm.CommandText = "SELECT ID, 分类编号, 分类名称, 上级分类 FROM 商品分类表 WHERE (BeActive = 1) AND (上级分类 LIKE N'%,%') ORDER BY 上级分类, 分类编号 DESC";
            if (dSet.Tables.Contains("商品分类表1")) dSet.Tables.Remove("商品分类表1");
            sqlDA.Fill(dSet, "商品分类表1");

            sqlConn.Close();
            for (int i = 0; i < dSet.Tables["商品分类表"].Rows.Count; i++)
            {
                if (dSet.Tables["商品分类表"].Rows[i][3].ToString() == "")
                    continue;

                checkedListBoxFL.Items.Add(dSet.Tables["商品分类表"].Rows[i][1].ToString() + " " + dSet.Tables["商品分类表"].Rows[i][2].ToString(),true);
                alFL.Add(dSet.Tables["商品分类表"].Rows[i][0].ToString());
            }

            for (int i = 0; i < dSet.Tables["商品分类表1"].Rows.Count; i++)
            {

                strTemp = dSet.Tables["商品分类表1"].Rows[i][3].ToString();
                //得到上级TAG
                iTemp = strTemp.LastIndexOf(',');
                if (iTemp == -1)
                {
                    continue;
                }
                else //分类
                {
                    strTemp = strTemp.Substring(iTemp + 1);
                    for (j = 0; j < alFL.Count; j++)
                    {
                        if (alFL[j].ToString() == strTemp)
                        {
                            checkedListBoxFL.Items.Insert(j+1,"  "+dSet.Tables["商品分类表1"].Rows[i][1].ToString() + " " + dSet.Tables["商品分类表1"].Rows[i][2].ToString());
                            alFL.Insert(j+1,dSet.Tables["商品分类表1"].Rows[i][0].ToString());
                            break;
                        }
                    }
                }
            }
            




        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bSEL = false;
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            bSEL = true;
            this.Close();
        }

        private void buttonCLEAR_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxFL.Items.Count; i++)
            {
                checkedListBoxFL.SetItemChecked(i,false);
            }
        }
    }
}
