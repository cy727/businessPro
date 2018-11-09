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
        private string SDTY0 = "", SDTY1 = "", SDTY2 = "";//ʱ�䣺�꣬�£���
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
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            //�õ�ʱ��

            //��ʱ��
            SDTY0 = DateTime.Parse(strDT).AddYears(-1).Year.ToString() + "-1-1";
            SDTY1 = DateTime.Parse(strDT).Year.ToString() + "-1-1";
            SDTY2 = DateTime.Parse(strDT).AddDays(1).ToShortDateString();

            //��ʱ��
            SDTM0 = DateTime.Parse(strDT).AddMonths(-1).Year.ToString() + "-"+DateTime.Parse(strDT).AddMonths(-1).Month.ToString()+"-1";
            SDTM1 = DateTime.Parse(strDT).Year.ToString() + "-" + DateTime.Parse(strDT).Month.ToString() + "-1";
            SDTM2 = DateTime.Parse(strDT).AddDays(1).ToShortDateString();


            //��ʱ��
            //�õ��ϴν�ת
            //�õ���ʼʱ��
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                SDTS0 = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT ����ʱ��,ID FROM ��ת���ܱ� ORDER BY ����ʱ�� DESC";
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

            

            //��ʼ��Ա���б�
            sqlComm.CommandText = "SELECT ID, ְԱ���� FROM ְԱ�� WHERE (beactive = 1)";

            if (dSet.Tables.Contains("ְԱ��")) dSet.Tables.Remove("ְԱ��");
            sqlDA.Fill(dSet, "ְԱ��");
            DataRow drTemp = dSet.Tables["ְԱ��"].NewRow();
            drTemp[0] = 0;
            drTemp[1] = "ȫ��";
            dSet.Tables["ְԱ��"].Rows.Add(drTemp);


            comboBoxYWY.DataSource = dSet.Tables["ְԱ��"];
            comboBoxYWY.DisplayMember = "ְԱ����";
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

            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                for (j = 2; j < dSet.Tables["��Ʒ��"].Columns.Count; j++)
                {
                    if (dSet.Tables["��Ʒ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["��Ʒ��"].Rows[i][j] = 0;
                }
                dSet.Tables["��Ʒ��"].Rows[i][12] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][4].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][10].ToString());
                dSet.Tables["��Ʒ��"].Rows[i][13] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][5].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][11].ToString());

                dSet.Tables["��Ʒ��"].Rows[i][14] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][6].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][4].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][8].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][10].ToString());
                dSet.Tables["��Ʒ��"].Rows[i][15] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][7].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][5].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][9].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][11].ToString());

                dSet.Tables["��Ʒ��"].Rows[i][26] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][16].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][18].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][24].ToString());
                dSet.Tables["��Ʒ��"].Rows[i][27] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][17].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][19].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][25].ToString());

                dSet.Tables["��Ʒ��"].Rows[i][28] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][16].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][20].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][18].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][22].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][24].ToString());
                dSet.Tables["��Ʒ��"].Rows[i][29] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][17].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][21].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][19].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][23].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][25].ToString());


                dSet.Tables["��Ʒ��"].Rows[i][40] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][30].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][32].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][38].ToString());
                dSet.Tables["��Ʒ��"].Rows[i][41] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][31].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][33].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][39].ToString());

                dSet.Tables["��Ʒ��"].Rows[i][42] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][30].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][34].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][32].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][36].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][38].ToString());
                dSet.Tables["��Ʒ��"].Rows[i][43] = decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][31].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][35].ToString()) - decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][33].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][37].ToString()) + decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][39].ToString());



                for (j = 2; j < dSet.Tables["��Ʒ��"].Columns.Count; j++)
                {
                    dTemp[j-2] += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][j].ToString());
                }
            }

            DataRow drT1 = dSet.Tables["��Ʒ��"].NewRow();
            drT1[1] = "�ϼ�";
            for (j = 2; j < dSet.Tables["��Ʒ��"].Columns.Count; j++)
            {
                drT1[j] = dTemp[j - 2];
            }
            dSet.Tables["��Ʒ��"].Rows.Add(drT1);

            for (j = 2; j < dSet.Tables["��λ��"].Columns.Count; j++)
            {
                dTemp[j - 2] = 0;
            }

            for (i = 0; i < dSet.Tables["��λ��"].Rows.Count; i++)
            {
                for (j = 2; j < dSet.Tables["��λ��"].Columns.Count; j++)
                {
                    if (dSet.Tables["��λ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["��λ��"].Rows[i][j] = 0;
                }
                dSet.Tables["��λ��"].Rows[i][12] = decimal.Parse(dSet.Tables["��λ��"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][4].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][10].ToString());
                dSet.Tables["��λ��"].Rows[i][13] = decimal.Parse(dSet.Tables["��λ��"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][5].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][11].ToString());

                dSet.Tables["��λ��"].Rows[i][14] = decimal.Parse(dSet.Tables["��λ��"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][6].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][4].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][8].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][10].ToString());
                dSet.Tables["��λ��"].Rows[i][15] = decimal.Parse(dSet.Tables["��λ��"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][7].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][5].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][9].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][11].ToString());

                dSet.Tables["��λ��"].Rows[i][26] = decimal.Parse(dSet.Tables["��λ��"].Rows[i][16].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][18].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][24].ToString());
                dSet.Tables["��λ��"].Rows[i][27] = decimal.Parse(dSet.Tables["��λ��"].Rows[i][17].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][19].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][25].ToString());

                dSet.Tables["��λ��"].Rows[i][28] = decimal.Parse(dSet.Tables["��λ��"].Rows[i][16].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][20].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][18].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][22].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][24].ToString());
                dSet.Tables["��λ��"].Rows[i][29] = decimal.Parse(dSet.Tables["��λ��"].Rows[i][17].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][21].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][19].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][23].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][25].ToString());


                dSet.Tables["��λ��"].Rows[i][40] = decimal.Parse(dSet.Tables["��λ��"].Rows[i][30].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][32].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][38].ToString());
                dSet.Tables["��λ��"].Rows[i][41] = decimal.Parse(dSet.Tables["��λ��"].Rows[i][31].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][33].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][39].ToString());

                dSet.Tables["��λ��"].Rows[i][42] = decimal.Parse(dSet.Tables["��λ��"].Rows[i][30].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][34].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][32].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][36].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][38].ToString());
                dSet.Tables["��λ��"].Rows[i][43] = decimal.Parse(dSet.Tables["��λ��"].Rows[i][31].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][35].ToString()) - decimal.Parse(dSet.Tables["��λ��"].Rows[i][33].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][37].ToString()) + decimal.Parse(dSet.Tables["��λ��"].Rows[i][39].ToString());



                for (j = 2; j < dSet.Tables["��λ��"].Columns.Count; j++)
                {
                    dTemp[j - 2] += decimal.Parse(dSet.Tables["��λ��"].Rows[i][j].ToString());
                }
            }

            DataRow drT2 = dSet.Tables["��λ��"].NewRow();
            drT2[1] = "�ϼ�";
            for (j = 2; j < dSet.Tables["��λ��"].Columns.Count; j++)
            {
                drT2[j] = dTemp[j - 2];
            }
            dSet.Tables["��λ��"].Rows.Add(drT2);



            for (j = 2; j < dSet.Tables["ְԱ��"].Columns.Count; j++)
            {
                dTemp[j - 2] = 0;
            }

            for (i = 0; i < dSet.Tables["ְԱ��"].Rows.Count; i++)
            {
                for (j = 2; j < dSet.Tables["ְԱ��"].Columns.Count; j++)
                {
                    if (dSet.Tables["ְԱ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["ְԱ��"].Rows[i][j] = 0;
                }
                dSet.Tables["ְԱ��"].Rows[i][12] = decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][4].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][10].ToString());
                dSet.Tables["ְԱ��"].Rows[i][13] = decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][5].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][11].ToString());

                dSet.Tables["ְԱ��"].Rows[i][14] = decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][2].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][6].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][4].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][8].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][10].ToString());
                dSet.Tables["ְԱ��"].Rows[i][15] = decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][3].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][7].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][5].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][9].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][11].ToString());

                dSet.Tables["ְԱ��"].Rows[i][26] = decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][16].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][18].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][24].ToString());
                dSet.Tables["ְԱ��"].Rows[i][27] = decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][17].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][19].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][25].ToString());

                dSet.Tables["ְԱ��"].Rows[i][28] = decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][16].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][20].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][18].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][22].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][24].ToString());
                dSet.Tables["ְԱ��"].Rows[i][29] = decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][17].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][21].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][19].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][23].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][25].ToString());


                dSet.Tables["ְԱ��"].Rows[i][40] = decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][30].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][32].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][38].ToString());
                dSet.Tables["ְԱ��"].Rows[i][41] = decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][31].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][33].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][39].ToString());

                dSet.Tables["ְԱ��"].Rows[i][42] = decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][30].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][34].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][32].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][36].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][38].ToString());
                dSet.Tables["ְԱ��"].Rows[i][43] = decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][31].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][35].ToString()) - decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][33].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][37].ToString()) + decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][39].ToString());



                for (j = 2; j < dSet.Tables["ְԱ��"].Columns.Count; j++)
                {
                    dTemp[j - 2] += decimal.Parse(dSet.Tables["ְԱ��"].Rows[i][j].ToString());
                }
            }

            DataRow drT3 = dSet.Tables["ְԱ��"].NewRow();
            drT3[1] = "�ϼ�";
            for (j = 2; j < dSet.Tables["ְԱ��"].Columns.Count; j++)
            {
                drT3[j] = dTemp[j - 2];
            }
            dSet.Tables["ְԱ��"].Rows.Add(drT3);




        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    strT = "ͬ�����۱ȽϷ�������Ʒ�Ƚϣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewSPBJ, strT, true, intUserLimit);
                    break;
                case 1:
                    strT = "ͬ�����۱ȽϷ������ͻ��Ƚϣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewKHBJ, strT, true, intUserLimit);
                    break;
                case 2:
                    strT = "ͬ�����۱ȽϷ�����ҵ��Ա�Ƚϣ�;��ǰ���ڣ�" + labelZDRQ.Text;
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
                    strT = "ͬ�����۱ȽϷ�������Ʒ�Ƚϣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewSPBJ, strT, false, intUserLimit);
                    break;
                case 1:
                    strT = "ͬ�����۱ȽϷ������ͻ��Ƚϣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewKHBJ, strT, false, intUserLimit);
                    break;
                case 2:
                    strT = "ͬ�����۱ȽϷ�����ҵ��Ա�Ƚϣ�;��ǰ���ڣ�" + labelZDRQ.Text;
                    PrintDGV.Print_DataGridView(dataGridViewYWYBJ, strT, false, intUserLimit);
                    break;
            }
        }

        private void textBoxSPBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCommInformation(1, "") == 0) //ʧ��
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
                if (cGetInformation.getCommInformation(20, textBoxSPBH.Text) == 0) //ʧ��
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
                if (cGetInformation.getCommInformation(10, textBoxSPMC.Text) == 0) //ʧ��
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
            if (cGetInformation.getCompanyInformation(1, "") == 0) //ʧ��
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
                if (cGetInformation.getCompanyInformation(1100, textBoxDWBH.Text) == 0) //ʧ��
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
                if (cGetInformation.getCompanyInformation(1300, textBoxDWMC.Text) == 0) //ʧ��
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
            string strA = "", strB = "", strC = "", strD = ""; //����
            string strA1 = "", strB1 = "", strC1 = "", strD1 = "";//����
            string strA2 = "", strB2 = "", strC2 = "", strD2 = "";//����
            string strA3 = "", strB3 = "", strC3 = "", strD3 = "";//����
            string strA4 = "", strB4 = "", strC4 = "", strD4 = "";//����
            string strA5 = "", strB5 = "", strC5 = "", strD5 = "";//����

            int i;

            strA = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

            strA1 = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA1 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA1 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA1 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA1 += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";


            strA2 = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA2 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA2 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA2 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA2 += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

            strA3 = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA3 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA3 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA3 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA3 += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

            strA4 = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA4 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA4 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA4 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA4 += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

            strA5 = "SELECT ������Ʒ�Ƶ���ϸ��.��ƷID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA5 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA5 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA5 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA5 += " GROUP BY ������Ʒ�Ƶ���ϸ��.��ƷID";

            strB = "SELECT �����˳���ϸ��.��ƷID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB += " GROUP BY �����˳���ϸ��.��ƷID";

            strB1 = "SELECT �����˳���ϸ��.��ƷID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB1 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB1 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB1 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB1 += " GROUP BY �����˳���ϸ��.��ƷID";

            strB2 = "SELECT �����˳���ϸ��.��ƷID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB2 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB2 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB2 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB2 += " GROUP BY �����˳���ϸ��.��ƷID";

            strB3 = "SELECT �����˳���ϸ��.��ƷID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB3 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB3 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB3 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB3 += " GROUP BY �����˳���ϸ��.��ƷID";

            strB4 = "SELECT �����˳���ϸ��.��ƷID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB4 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB4 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB4 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB4 += " GROUP BY �����˳���ϸ��.��ƷID";

            strB5 = "SELECT �����˳���ϸ��.��ƷID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB5 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB5 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB5 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB5 += " GROUP BY �����˳���ϸ��.��ƷID";


            strC = "SELECT  �����˲������ϸ��.��ƷID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY �����˲������ϸ��.��ƷID";

            strC1 = "SELECT  �����˲������ϸ��.��ƷID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC1 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC1 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC1 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC1 += " GROUP BY �����˲������ϸ��.��ƷID";

            strC2 = "SELECT  �����˲������ϸ��.��ƷID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC2 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC2 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC2 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC2 += " GROUP BY �����˲������ϸ��.��ƷID";

            strC3 = "SELECT  �����˲������ϸ��.��ƷID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC3 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC3 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC3 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC3 += " GROUP BY �����˲������ϸ��.��ƷID";

            strC4 = "SELECT  �����˲������ϸ��.��ƷID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC4 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC4 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC4 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC4 += " GROUP BY �����˲������ϸ��.��ƷID";

            strC5 = "SELECT  �����˲������ϸ��.��ƷID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC5 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC5 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC5 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC5 += " GROUP BY �����˲������ϸ��.��ƷID";


            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����,��Ʒ��.��Ʒ���,�������۱�.�������۽��, �������۱�.�������۽��, �����˳���.�����˳����, �����˳���.�����˳����, �������۱�.�������۳ɱ�, �������۱�.�������۳ɱ�, �����˳���.�����˳��ɱ�,�����˳���.�����˳��ɱ�,�����˲���.�����˲����,�����˲���.�����˲����,0.00 AS ����ʵ�����, 0.00 AS ����ʵ�����, 0.00 AS ����ë��,0.00 AS ����ë��, �������۱�.�������۽��, �������۱�.�������۽��, �����˳���.�����˳����, �����˳���.�����˳����, �������۱�.�������۳ɱ�, �������۱�.�������۳ɱ�, �����˳���.�����˳��ɱ�,�����˳���.�����˳��ɱ�,�����˲���.�����˲����,�����˲���.�����˲����,0.00 AS ����ʵ�����, 0.00 AS ����ʵ�����, 0.00 AS ����ë��,0.00 AS ����ë��, �������۱�.�������۽��, �������۱�.�������۽��, �����˳���.�����˳����, �����˳���.�����˳����, �������۱�.�������۳ɱ�, �������۱�.�������۳ɱ�, �����˳���.�����˳��ɱ�,�����˳���.�����˳��ɱ�,�����˲���.�����˲����,�����˲���.�����˲����,0.00 AS ����ʵ�����, 0.00 AS ����ʵ�����, 0.00 AS ����ë��,0.00 AS ����ë�� FROM ��Ʒ�� LEFT OUTER JOIN (" + strA + ") �������۱� ON �������۱�.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN (" + strA1 + ") �������۱� ON �������۱�.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN (" + strA2 + ") �������۱� ON �������۱�.��ƷID = ��Ʒ��.ID  LEFT OUTER JOIN (" + strA3 + ") �������۱� ON �������۱�.��ƷID = ��Ʒ��.ID  LEFT OUTER JOIN (" + strA4 + ") �������۱� ON �������۱�.��ƷID = ��Ʒ��.ID  LEFT OUTER JOIN (" + strA5 + ") �������۱� ON �������۱�.��ƷID = ��Ʒ��.ID  LEFT OUTER JOIN (" + strB + ") �����˳��� ON �����˳���.��ƷID = ��Ʒ��.ID  LEFT OUTER JOIN (" + strB1 + ") �����˳��� ON �����˳���.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN (" + strB2 + ") �����˳��� ON �����˳���.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN (" + strB3 + ") �����˳��� ON �����˳���.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN (" + strB4 + ") �����˳��� ON �����˳���.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN (" + strB5 + ") �����˳��� ON �����˳���.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN (" + strC + ") �����˲��� ON �����˲���.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN (" + strC1 + ") �����˲��� ON �����˲���.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN (" + strC2 + ") �����˲��� ON �����˲���.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN (" + strC3 + ") �����˲��� ON �����˲���.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN (" + strC4 + ") �����˲��� ON �����˲���.��ƷID = ��Ʒ��.ID LEFT OUTER JOIN (" + strC5 + ") �����˲��� ON �����˲���.��ƷID = ��Ʒ��.ID WHERE (��Ʒ��.��װ��Ʒ = 0)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                sqlComm.CommandText += " AND (��Ʒ��.ID = " + intCommID.ToString() + ") ";
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            iCount[0] = dSet.Tables["��Ʒ��"].Rows.Count;


            //**********��λID ������Ʒ�Ƶ���.��λID
            strA = "SELECT ������Ʒ�Ƶ���.��λID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA += " GROUP BY ������Ʒ�Ƶ���.��λID";

            strA1 = "SELECT ������Ʒ�Ƶ���.��λID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA1 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA1 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA1 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA1 += " GROUP BY ������Ʒ�Ƶ���.��λID";


            strA2 = "SELECT ������Ʒ�Ƶ���.��λID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA2 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA2 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA2 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA2 += " GROUP BY ������Ʒ�Ƶ���.��λID";

            strA3 = "SELECT ������Ʒ�Ƶ���.��λID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA3 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA3 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA3 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA3 += " GROUP BY ������Ʒ�Ƶ���.��λID";

            strA4 = "SELECT ������Ʒ�Ƶ���.��λID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA4 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA4 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA4 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA4 += " GROUP BY ������Ʒ�Ƶ���.��λID";

            strA5 = "SELECT ������Ʒ�Ƶ���.��λID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA5 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA5 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA5 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA5 += " GROUP BY ������Ʒ�Ƶ���.��λID";

            strB = "SELECT �����˳����ܱ�.��λID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB += " GROUP BY �����˳����ܱ�.��λID";

            strB1 = "SELECT �����˳����ܱ�.��λID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB1 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB1 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB1 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB1 += " GROUP BY �����˳����ܱ�.��λID";

            strB2 = "SELECT �����˳����ܱ�.��λID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB2 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB2 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB2 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB2 += " GROUP BY �����˳����ܱ�.��λID";

            strB3 = "SELECT �����˳����ܱ�.��λID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB3 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB3 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB3 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB3 += " GROUP BY �����˳����ܱ�.��λID";

            strB4 = "SELECT �����˳����ܱ�.��λID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB4 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB4 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB4 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB4 += " GROUP BY �����˳����ܱ�.��λID";

            strB5 = "SELECT �����˳����ܱ�.��λID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB5 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB5 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB5 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB5 += " GROUP BY �����˳����ܱ�.��λID";


            strC = "SELECT  �����˲���ۻ��ܱ�.��λID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY �����˲���ۻ��ܱ�.��λID";

            strC1 = "SELECT  �����˲���ۻ��ܱ�.��λID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC1 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC1 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC1 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC1 += " GROUP BY �����˲���ۻ��ܱ�.��λID";

            strC2 = "SELECT  �����˲���ۻ��ܱ�.��λID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC2 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC2 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC2 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC2 += " GROUP BY �����˲���ۻ��ܱ�.��λID";

            strC3 = "SELECT  �����˲���ۻ��ܱ�.��λID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC3 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC3 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC3 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC3 += " GROUP BY �����˲���ۻ��ܱ�.��λID";

            strC4 = "SELECT  �����˲���ۻ��ܱ�.��λID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC4 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC4 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC4 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC4 += " GROUP BY �����˲���ۻ��ܱ�.��λID";

            strC5 = "SELECT  �����˲���ۻ��ܱ�.��λID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC5 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC5 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC5 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC5 += " GROUP BY �����˲���ۻ��ܱ�.��λID";


            sqlComm.CommandText = "SELECT ��λ��.��λ���,��λ��.��λ����,�������۱�.�������۽��, �������۱�.�������۽��, �����˳���.�����˳����, �����˳���.�����˳����, �������۱�.�������۳ɱ�, �������۱�.�������۳ɱ�, �����˳���.�����˳��ɱ�,�����˳���.�����˳��ɱ�,�����˲���.�����˲����,�����˲���.�����˲����,0.00 AS ����ʵ�����, 0.00 AS ����ʵ�����, 0.00 AS ����ë��,0.00 AS ����ë��, �������۱�.�������۽��, �������۱�.�������۽��, �����˳���.�����˳����, �����˳���.�����˳����, �������۱�.�������۳ɱ�, �������۱�.�������۳ɱ�, �����˳���.�����˳��ɱ�,�����˳���.�����˳��ɱ�,�����˲���.�����˲����,�����˲���.�����˲����,0.00 AS ����ʵ�����, 0.00 AS ����ʵ�����, 0.00 AS ����ë��,0.00 AS ����ë��, �������۱�.�������۽��, �������۱�.�������۽��, �����˳���.�����˳����, �����˳���.�����˳����, �������۱�.�������۳ɱ�, �������۱�.�������۳ɱ�, �����˳���.�����˳��ɱ�,�����˳���.�����˳��ɱ�,�����˲���.�����˲����,�����˲���.�����˲����,0.00 AS ����ʵ�����, 0.00 AS ����ʵ�����, 0.00 AS ����ë��,0.00 AS ����ë�� FROM ��λ�� LEFT OUTER JOIN (" + strA + ") �������۱� ON �������۱�.��λID = ��λ��.ID LEFT OUTER JOIN (" + strA1 + ") �������۱� ON �������۱�.��λID = ��λ��.ID LEFT OUTER JOIN (" + strA2 + ") �������۱� ON �������۱�.��λID = ��λ��.ID  LEFT OUTER JOIN (" + strA3 + ") �������۱� ON �������۱�.��λID = ��λ��.ID  LEFT OUTER JOIN (" + strA4 + ") �������۱� ON �������۱�.��λID = ��λ��.ID  LEFT OUTER JOIN (" + strA5 + ") �������۱� ON �������۱�.��λID = ��λ��.ID  LEFT OUTER JOIN (" + strB + ") �����˳��� ON �����˳���.��λID = ��λ��.ID  LEFT OUTER JOIN (" + strB1 + ") �����˳��� ON �����˳���.��λID = ��λ��.ID LEFT OUTER JOIN (" + strB2 + ") �����˳��� ON �����˳���.��λID = ��λ��.ID LEFT OUTER JOIN (" + strB3 + ") �����˳��� ON �����˳���.��λID = ��λ��.ID LEFT OUTER JOIN (" + strB4 + ") �����˳��� ON �����˳���.��λID = ��λ��.ID LEFT OUTER JOIN (" + strB5 + ") �����˳��� ON �����˳���.��λID = ��λ��.ID LEFT OUTER JOIN (" + strC + ") �����˲��� ON �����˲���.��λID = ��λ��.ID LEFT OUTER JOIN (" + strC1 + ") �����˲��� ON �����˲���.��λID = ��λ��.ID LEFT OUTER JOIN (" + strC2 + ") �����˲��� ON �����˲���.��λID = ��λ��.ID LEFT OUTER JOIN (" + strC3 + ") �����˲��� ON �����˲���.��λID = ��λ��.ID LEFT OUTER JOIN (" + strC4 + ") �����˲��� ON �����˲���.��λID = ��λ��.ID LEFT OUTER JOIN (" + strC5 + ") �����˲��� ON �����˲���.��λID = ��λ��.ID";

            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                sqlComm.CommandText += " WHERE (��λ��.ID=" + iCompanyID.ToString() + ") ";
            }

            if (dSet.Tables.Contains("��λ��")) dSet.Tables.Remove("��λ��");
            sqlDA.Fill(dSet, "��λ��");
            iCount[1] = dSet.Tables["��λ��"].Rows.Count;

            //**********ҵ��ԱID ������Ʒ�Ƶ���.ҵ��ԱID
            strA = "SELECT ������Ʒ�Ƶ���.ҵ��ԱID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA += " GROUP BY ������Ʒ�Ƶ���.ҵ��ԱID";

            strA1 = "SELECT ������Ʒ�Ƶ���.ҵ��ԱID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA1 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA1 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA1 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA1 += " GROUP BY ������Ʒ�Ƶ���.ҵ��ԱID";


            strA2 = "SELECT ������Ʒ�Ƶ���.ҵ��ԱID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA2 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA2 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA2 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA2 += " GROUP BY ������Ʒ�Ƶ���.ҵ��ԱID";

            strA3 = "SELECT ������Ʒ�Ƶ���.ҵ��ԱID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA3 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA3 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA3 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA3 += " GROUP BY ������Ʒ�Ƶ���.ҵ��ԱID";

            strA4 = "SELECT ������Ʒ�Ƶ���.ҵ��ԱID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA4 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA4 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA4 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA4 += " GROUP BY ������Ʒ�Ƶ���.ҵ��ԱID";

            strA5 = "SELECT ������Ʒ�Ƶ���.ҵ��ԱID, SUM(������Ʒ�Ƶ���ϸ��.ʵ�ƽ��) AS �������۽��, SUM(������Ʒ�Ƶ���ϸ��.���� * ������Ʒ�Ƶ���ϸ��.���ɱ���) AS �������۳ɱ� FROM ������Ʒ�Ƶ��� INNER JOIN ������Ʒ�Ƶ���ϸ�� ON ������Ʒ�Ƶ���.ID = ������Ʒ�Ƶ���ϸ��.��ID INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ��Ʒ�� ON ������Ʒ�Ƶ���ϸ��.��ƷID = ��Ʒ��.ID WHERE (������Ʒ�Ƶ���.���� >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.���� < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (������Ʒ�Ƶ���.BeActive=1) AND (��λ��.BeActive = 1) AND (��Ʒ��.beactive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strA5 += " AND (������Ʒ�Ƶ���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strA5 += " AND (������Ʒ�Ƶ���.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strA5 += " AND (������Ʒ�Ƶ���.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }
            strA5 += " GROUP BY ������Ʒ�Ƶ���.ҵ��ԱID";

            strB = "SELECT �����˳����ܱ�.ҵ��ԱID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB += " GROUP BY �����˳����ܱ�.ҵ��ԱID";

            strB1 = "SELECT �����˳����ܱ�.ҵ��ԱID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB1 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB1 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB1 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB1 += " GROUP BY �����˳����ܱ�.ҵ��ԱID";

            strB2 = "SELECT �����˳����ܱ�.ҵ��ԱID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB2 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB2 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB2 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB2 += " GROUP BY �����˳����ܱ�.ҵ��ԱID";

            strB3 = "SELECT �����˳����ܱ�.ҵ��ԱID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";

            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB3 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB3 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB3 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB3 += " GROUP BY �����˳����ܱ�.ҵ��ԱID";

            strB4 = "SELECT �����˳����ܱ�.ҵ��ԱID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB4 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB4 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB4 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB4 += " GROUP BY �����˳����ܱ�.ҵ��ԱID";

            strB5 = "SELECT �����˳����ܱ�.ҵ��ԱID, SUM(�����˳���ϸ��.ʵ�ƽ��) AS �����˳����, SUM(�����˳���ϸ��.���� * �����˳���ϸ��.���ɱ���) AS �����˳��ɱ� FROM �����˳����ܱ� INNER JOIN �����˳���ϸ�� ON �����˳����ܱ�.ID = �����˳���ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID WHERE (�����˳����ܱ�.���� >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (�����˳����ܱ�.���� < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (�����˳����ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strB5 += " AND (�����˳���ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strB5 += " AND (�����˳����ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strB5 += " AND (�����˳����ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strB5 += " GROUP BY �����˳����ܱ�.ҵ��ԱID";

            strC = "SELECT  �����˲���ۻ��ܱ�.ҵ��ԱID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTM0 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC += " GROUP BY �����˲���ۻ��ܱ�.ҵ��ԱID";

            strC1 = "SELECT  �����˲���ۻ��ܱ�.ҵ��ԱID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTM1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTM2 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC1 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC1 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC1 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC1 += " GROUP BY �����˲���ۻ��ܱ�.ҵ��ԱID";

            strC2 = "SELECT  �����˲���ۻ��ܱ�.ҵ��ԱID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTY0 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC2 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC2 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC2 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC2 += " GROUP BY �����˲���ۻ��ܱ�.ҵ��ԱID";

            strC3 = "SELECT  �����˲���ۻ��ܱ�.ҵ��ԱID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTY1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTY2 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC3 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC3 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC3 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC3 += " GROUP BY �����˲���ۻ��ܱ�.ҵ��ԱID";

            strC4 = "SELECT  �����˲���ۻ��ܱ�.ҵ��ԱID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTQ0 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC4 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC4 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC4 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC4 += " GROUP BY �����˲���ۻ��ܱ�.ҵ��ԱID";

            strC5 = "SELECT  �����˲���ۻ��ܱ�.ҵ��ԱID, SUM(�����˲������ϸ��.���) AS �����˲���� FROM �����˲���ۻ��ܱ� INNER JOIN �����˲������ϸ�� ON �����˲���ۻ��ܱ�.ID = �����˲������ϸ��.����ID INNER JOIN ��Ʒ�� ON �����˲������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��λ�� ON �����˲���ۻ��ܱ�.��λID = ��λ��.ID WHERE (�����˲���ۻ��ܱ�.���� >= CONVERT(DATETIME, '" + SDTQ1 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.���� < CONVERT(DATETIME, '" + SDTQ2 + " 00:00:00', 102)) AND (�����˲���ۻ��ܱ�.BeActive=1) AND (��Ʒ��.beactive = 1) AND (��λ��.BeActive = 1)";
            if (!checkBoxALLSP.Checked && intCommID != 0) //����Ʒ
            {
                strC5 += " AND (�����˲������ϸ��.��ƷID = " + intCommID.ToString() + ") ";
            }
            if (!checkBoxALLDW.Checked && iCompanyID != 0)
            {
                strC5 += " AND (�����˲���ۻ��ܱ�.��λID=" + iCompanyID.ToString() + ") ";
            }
            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                strC5 += " AND (�����˲���ۻ��ܱ�.ҵ��ԱID=" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }

            strC5 += " GROUP BY �����˲���ۻ��ܱ�.ҵ��ԱID";

            sqlComm.CommandText = "SELECT ְԱ��.ְԱ���,ְԱ��.ְԱ����,�������۱�.�������۽��, �������۱�.�������۽��, �����˳���.�����˳����, �����˳���.�����˳����, �������۱�.�������۳ɱ�, �������۱�.�������۳ɱ�, �����˳���.�����˳��ɱ�,�����˳���.�����˳��ɱ�,�����˲���.�����˲����,�����˲���.�����˲����,0.00 AS ����ʵ�����, 0.00 AS ����ʵ�����, 0.00 AS ����ë��,0.00 AS ����ë��, �������۱�.�������۽��, �������۱�.�������۽��, �����˳���.�����˳����, �����˳���.�����˳����, �������۱�.�������۳ɱ�, �������۱�.�������۳ɱ�, �����˳���.�����˳��ɱ�,�����˳���.�����˳��ɱ�,�����˲���.�����˲����,�����˲���.�����˲����,0.00 AS ����ʵ�����, 0.00 AS ����ʵ�����, 0.00 AS ����ë��,0.00 AS ����ë��, �������۱�.�������۽��, �������۱�.�������۽��, �����˳���.�����˳����, �����˳���.�����˳����, �������۱�.�������۳ɱ�, �������۱�.�������۳ɱ�, �����˳���.�����˳��ɱ�,�����˳���.�����˳��ɱ�,�����˲���.�����˲����,�����˲���.�����˲����,0.00 AS ����ʵ�����, 0.00 AS ����ʵ�����, 0.00 AS ����ë��,0.00 AS ����ë�� FROM ְԱ�� LEFT OUTER JOIN (" + strA + ") �������۱� ON �������۱�.ҵ��ԱID = ְԱ��.ID LEFT OUTER JOIN (" + strA1 + ") �������۱� ON �������۱�.ҵ��ԱID = ְԱ��.ID LEFT OUTER JOIN (" + strA2 + ") �������۱� ON �������۱�.ҵ��ԱID = ְԱ��.ID  LEFT OUTER JOIN (" + strA3 + ") �������۱� ON �������۱�.ҵ��ԱID = ְԱ��.ID  LEFT OUTER JOIN (" + strA4 + ") �������۱� ON �������۱�.ҵ��ԱID = ְԱ��.ID  LEFT OUTER JOIN (" + strA5 + ") �������۱� ON �������۱�.ҵ��ԱID = ְԱ��.ID  LEFT OUTER JOIN (" + strB + ") �����˳��� ON �����˳���.ҵ��ԱID = ְԱ��.ID  LEFT OUTER JOIN (" + strB1 + ") �����˳��� ON �����˳���.ҵ��ԱID = ְԱ��.ID LEFT OUTER JOIN (" + strB2 + ") �����˳��� ON �����˳���.ҵ��ԱID = ְԱ��.ID LEFT OUTER JOIN (" + strB3 + ") �����˳��� ON �����˳���.ҵ��ԱID = ְԱ��.ID LEFT OUTER JOIN (" + strB4 + ") �����˳��� ON �����˳���.ҵ��ԱID = ְԱ��.ID LEFT OUTER JOIN (" + strB5 + ") �����˳��� ON �����˳���.ҵ��ԱID = ְԱ��.ID LEFT OUTER JOIN (" + strC + ") �����˲��� ON �����˲���.ҵ��ԱID = ְԱ��.ID LEFT OUTER JOIN (" + strC1 + ") �����˲��� ON �����˲���.ҵ��ԱID = ְԱ��.ID LEFT OUTER JOIN (" + strC2 + ") �����˲��� ON �����˲���.ҵ��ԱID = ְԱ��.ID LEFT OUTER JOIN (" + strC3 + ") �����˲��� ON �����˲���.ҵ��ԱID = ְԱ��.ID LEFT OUTER JOIN (" + strC4 + ") �����˲��� ON �����˲���.ҵ��ԱID = ְԱ��.ID LEFT OUTER JOIN (" + strC5 + ") �����˲��� ON �����˲���.ҵ��ԱID = ְԱ��.ID";

            if (comboBoxYWY.SelectedValue.ToString() != "0")
            {
                sqlComm.CommandText += " WHERE (ְԱ��.ID =" + comboBoxYWY.SelectedValue.ToString() + ") ";
            }


            if (dSet.Tables.Contains("ְԱ��")) dSet.Tables.Remove("ְԱ��");
            sqlDA.Fill(dSet, "ְԱ��");
            iCount[2] = dSet.Tables["ְԱ��"].Rows.Count;


            sqlConn.Close();
            adjustDataView();
            dataGridViewSPBJ.DataSource = dSet.Tables["��Ʒ��"];
            for (i = 0; i < dataGridViewSPBJ.Columns.Count; i++)
            {
                dataGridViewSPBJ.Columns[i].DefaultCellStyle.Format = "f2";
            }


            dataGridViewKHBJ.DataSource = dSet.Tables["��λ��"];

            for (i = 0; i < dataGridViewKHBJ.Columns.Count; i++)
            {
                dataGridViewKHBJ.Columns[i].DefaultCellStyle.Format = "f2";
            }

            dataGridViewYWYBJ.DataSource = dSet.Tables["ְԱ��"];
            for (i = 0; i < dataGridViewYWYBJ.Columns.Count; i++)
            {
                dataGridViewYWYBJ.Columns[i].DefaultCellStyle.Format = "f2";
            }
            tabControl1_SelectedIndexChanged(null, null);

        }




    }
}