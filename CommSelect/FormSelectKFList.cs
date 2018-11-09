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
    public partial class FormSelectKFList : Form
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

        public ArrayList alKF = new ArrayList();
        public FormSelectKFList()
        {
            InitializeComponent();
        }

        private void FormSelectKFList_Load(object sender, EventArgs e)
        {

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            initCommLIST();
        }

        private void initCommLIST()
        {
            string strTemp;
            int iTemp, j;

            checkedListBoxKF.Items.Clear();
            alKF.Clear();

            sqlComm.CommandText = "SELECT ID, 库房名称 FROM 库房表 WHERE (BeActive = 1)";

            sqlConn.Open();

            if (dSet.Tables.Contains("库房表")) dSet.Tables.Remove("库房表");
            sqlDA.Fill(dSet, "库房表");
            sqlConn.Close();
            for (int i = 0; i < dSet.Tables["库房表"].Rows.Count; i++)
            {

                checkedListBoxKF.Items.Add(dSet.Tables["库房表"].Rows[i][1].ToString(), true);
                alKF.Add(dSet.Tables["库房表"].Rows[i][0].ToString());
            }

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            bSEL = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bSEL = false;
            this.Close();
        }

        private void buttonCLEAR_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxKF.Items.Count; i++)
            {
                checkedListBoxKF.SetItemChecked(i, false);
            }
        }
    }
}
