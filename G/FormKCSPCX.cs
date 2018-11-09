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
    public partial class FormKCSPCX : Form
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
        private int intKFID = 0;


        private ArrayList alFL = new ArrayList();
        private ClassGetInformation cGetInformation;

        public int LIMITACCESS = 18;
        
        public FormKCSPCX()
        {
            InitializeComponent();
        }

        private void FormKCSPCX_Load(object sender, EventArgs e)
        {
            this.Left = 1;
            this.Top = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��˾����, ����Ŀ��1, ����Ŀ��2, ����Ŀ��3, ����Ŀ��4, ����ԱȨ��, �ܾ���Ȩ��, ְԱȨ��, ����Ȩ��, ҵ��ԱȨ�� FROM ϵͳ������";
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


        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i,j;
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.�������, ��Ʒ��.���ɱ���, (��Ʒ��.�������*��Ʒ��.���ɱ���) AS �����, ��Ʒ��.���� AS ����, ��Ʒ��.������ FROM ��Ʒ�� LEFT OUTER JOIN ��Ʒ����� ON ��Ʒ��.������ = ��Ʒ�����.ID WHERE (��Ʒ��.beactive = 1)";


            if (intCommID != 0)
                sqlComm.CommandText += " AND (��Ʒ��.ID = " + intCommID.ToString() + ") ";

            switch (comboBoxC.SelectedIndex.ToString())
            {
                case "1":
                    sqlComm.CommandText += " AND (��Ʒ��.������� <= " + numericUpDownC.Value.ToString() + ") ";
                    break;
                case "2":
                    sqlComm.CommandText += " AND (��Ʒ��.������� >= " + numericUpDownC.Value.ToString() + ") ";
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
                        sqlComm.CommandText += " AND ((��Ʒ��.������ = " + alFL[i].ToString() + ")";
                    else
                        sqlComm.CommandText += " OR (��Ʒ��.������ = " + alFL[i].ToString() + ")";
                    for (j = 0; j < cGetInformation.intUnderClassNumber; j++)
                        sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[j].ToString() + ")";

                }
                sqlComm.CommandText += ")";
            }



            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");

            sqlConn.Close();

            decimal dTemp = 0, dTemp1 = 0;
            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��"].Rows[i][3].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][3] = 0;
                if (dSet.Tables["��Ʒ��"].Rows[i][5].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][5] = 0;
                dTemp += Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][5]);
                dTemp1 += Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][3]);

            }
            labelKCSLHJ.Text = dTemp1.ToString("f0");

            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];
            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";

            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.Rows.Count.ToString();

            //Ȩ�޿���
            if (intUserLimit >= LIMITACCESS)
            {
                labelKCJEHJ.Text = dTemp.ToString("f2");
            }
            else
            {
                dataGridViewDJMX.Columns[4].Visible = false;
                dataGridViewDJMX.Columns[5].Visible = false;
                dataGridViewDJMX.Columns[6].Visible = false;
                labelKCJEHJ.Visible = false;
            }


        }

  
        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            int i,j;
            bool bMX = true;

            if (MessageBox.Show("�Ƿ������ϸ��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����+'(���:'+��Ʒ��.��Ʒ���+')', ��Ʒ��.�������, ��Ʒ��.���ɱ���, (��Ʒ��.�������*��Ʒ��.���ɱ���) AS �����, ��Ʒ��.���ս��� AS ����, ��Ʒ��.������, ��Ʒ��.������ FROM ��Ʒ�� LEFT OUTER JOIN ��Ʒ����� ON ��Ʒ��.������ = ��Ʒ�����.ID WHERE (��Ʒ��.beactive = 1)";

            if (intCommID != 0)
                sqlComm.CommandText += " AND (��Ʒ��.ID = " + intCommID.ToString() + ") ";

            switch (comboBoxC.SelectedIndex.ToString())
            {
                case "1":
                    sqlComm.CommandText += " AND (��Ʒ��.������� <= " + numericUpDownC.Value.ToString() + ") ";
                    break;
                case "2":
                    sqlComm.CommandText += " AND (��Ʒ��.������� >= " + numericUpDownC.Value.ToString() + ") ";
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
                        sqlComm.CommandText += " AND ((��Ʒ��.������ = " + alFL[i].ToString() + ")";
                    else
                        sqlComm.CommandText += " OR (��Ʒ��.������ = " + alFL[i].ToString() + ")";
                    for (j = 0; j < cGetInformation.intUnderClassNumber; j++)
                        sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[j].ToString() + ")";

                }
                sqlComm.CommandText += ")";
            }

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");

            sqlConn.Close();

            decimal dTemp = 0, dTemp1 = 0;
            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��"].Rows[i][2].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][2] = 0;
                if (dSet.Tables["��Ʒ��"].Rows[i][4].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][4] = 0;
                dTemp += Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][4]);
                dTemp1 += Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][2]);

            }
            labelKCSLHJ.Text = dTemp.ToString("f0");

            int k, t;
            int iRow0, iRow1;
            decimal[] dSum = new decimal[5];
            decimal[] dSum1 = new decimal[5];

            for (t = 0; t < dSum1.Length; t++)
                dSum1[t] = 0;


            sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ����� FROM ��Ʒ����� ORDER BY �ϼ�����";
            if (dSet.Tables.Contains("��Ʒ�����")) dSet.Tables.Remove("��Ʒ�����");
            sqlDA.Fill(dSet, "��Ʒ�����");

            DataTable dTable = new DataTable();
            dTable.Columns.Add("��Ʒ���", System.Type.GetType("System.String"));
            dTable.Columns.Add("��Ʒ����", System.Type.GetType("System.String"));
            dTable.Columns.Add("�������", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("���ɱ���", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("�����", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("����", System.Type.GetType("System.Decimal"));
            dTable.Columns.Add("������", System.Type.GetType("System.Decimal"));


            DataRow[] dtC = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0'");
            for (i = 0; i < dtC.Length; i++)
            {
                object[] oTemp = new object[7];
                oTemp[0] = dtC[i][1];
                oTemp[1] = dtC[i][2];

                for (t = 2; t < oTemp.Length; t++)
                    oTemp[t] = 0;


                dTable.Rows.Add(oTemp);
                iRow0 = dTable.Rows.Count - 1;

                DataRow[] dtC1 = dSet.Tables["��Ʒ�����"].Select("�ϼ����� = '0," + dtC[i][0] + "'");
                for (j = 0; j < dtC1.Length; j++)
                {
                    object[] oTemp1 = new object[7];
                    oTemp1[0] = dtC1[j][1];
                    oTemp1[1] = "����" + dtC1[j][2];
                    //oTemp1[8] = dtC1[j][0];
                    //oTemp1[2] = 0; oTemp1[3] = 0; oTemp1[4] = 0; oTemp1[5] = 0; oTemp1[6] = 0; oTemp1[7] = 0;
                    for (t = 2; t < oTemp1.Length; t++)
                        oTemp1[t] = 0;

                    dTable.Rows.Add(oTemp1);
                    iRow1 = dTable.Rows.Count - 1;

                    DataRow[] dtC2 = dSet.Tables["��Ʒ��"].Select("������ = " + dtC1[j][0]);

                    for (t = 0; t < dSum.Length; t++)
                        dSum[t] = 0;
                    for (k = 0; k < dtC2.Length; k++)
                    {

                        for (t = 0; t < dSum.Length; t++)
                        {
                            if(t==0 || t==2)
                                dSum[t] += Convert.ToDecimal(dtC2[k][t + 2].ToString());
                        }


                        if (bMX)
                        {
                            object[] oTemp2 = new object[4];
                            for (t = 0; t < oTemp2.Length; t++)
                                oTemp2[t] = dtC2[k][t];
                            oTemp2[1] = "��������" + dtC2[k][1];

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

            object[] oTemp3 = new object[7];
            oTemp3[0] = "�ϼ�";
            oTemp3[1] = "";
            for (t = 2; t < oTemp3.Length; t++)
                oTemp3[t] = dSum1[t - 2];
            dTable.Rows.Add(oTemp3);


            dataGridViewDJMX.DataSource = dTable;
            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";

            toolStripStatusLabelMXJLS.Text = dSet.Tables["��Ʒ��"].Rows.Count.ToString();

            //Ȩ�޿���
            if (intUserLimit >= LIMITACCESS)
            {
                labelKCJEHJ.Text = dTemp.ToString("f2");
            }
            else
            {
                dataGridViewDJMX.Columns[3].Visible = false;
                dataGridViewDJMX.Columns[4].Visible = false;
                dataGridViewDJMX.Columns[5].Visible = false;
                labelKCJEHJ.Visible = false;
            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "�����Ʒ��ѯ;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true,intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "�����Ʒ��ѯ;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false,intUserLimit);
        }

        private void toolStripButtonASP_Click(object sender, EventArgs e)
        {
            intCommID = 0;
            textBoxSPBH.Text = "";
            textBoxSPMC.Text = "";
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

        private void toolStripButtonACompany_Click(object sender, EventArgs e)
        {
            alFL.Clear();
            textBoxSPLB.Text = "";
        }


 
    }
}