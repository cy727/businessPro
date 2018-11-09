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
           // sqlComm.CommandText = "SELECT ��Ʒ��.ID, �ⷿ��.�ⷿ����, ��Ʒ��.��Ʒ����, ����.�������, ����.���ɱ���,����.����� FROM ���� INNER JOIN �ⷿ�� ON ����.�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON ����.��ƷID = ��Ʒ��.ID WHERE (�ⷿ��.BeActive = 1)";
            sqlComm.CommandText = "SELECT ��Ʒ��.ID, �ⷿ��.�ⷿ����, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ����.�������, ����.���ɱ���,(����.�������*����.���ɱ���) AS �����, ��Ʒ��.���� AS ����, ��Ʒ��.������ FROM ���� INNER JOIN �ⷿ�� ON ����.�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON ����.��ƷID = ��Ʒ��.ID WHERE (�ⷿ��.BeActive = 1) AND (��Ʒ��.beactive = 1) AND (����.BeActive = 1) ";

            if (intCommID != 0)
                sqlComm.CommandText += " AND (��Ʒ��.ID = "+intCommID.ToString()+") ";
            if (intKFID != 0)
                sqlComm.CommandText += "AND (�ⷿ��.ID = "+intKFID.ToString()+") ";

            switch (comboBoxC.SelectedIndex.ToString())
            {
                case "1":
                    sqlComm.CommandText += " AND (����.������� <= " + numericUpDownC.Value.ToString() + ") ";
                    break;
                case "2":
                    sqlComm.CommandText += " AND (����.������� >= " + numericUpDownC.Value.ToString() + ") ";
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
                        sqlComm.CommandText += " AND ((��Ʒ��.������ = " + alFL[i].ToString() + ")";
                    else
                        sqlComm.CommandText += " OR (��Ʒ��.������ = " + alFL[i].ToString() + ")";
                    for (j = 0; j < cGetInformation.intUnderClassNumber; j++)
                        sqlComm.CommandText += " OR (��Ʒ��.������ = " + cGetInformation.intUnderClass[j].ToString() + ")";

                }
                sqlComm.CommandText += ")";
            }

            sqlComm.CommandText += " ORDER BY ��Ʒ��.������";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");

            sqlConn.Close();

            decimal dTemp = 0, dTemp1 = 0; ;
            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��"].Rows[i][4].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][4] = 0;
                if (dSet.Tables["��Ʒ��"].Rows[i][6].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][6] = 0;
                dTemp += Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][6]);
                dTemp1 += Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][4]);

                //dSet.Tables["��Ʒ��"].Rows[i][0] = i + 1;

            }
            
            labelKCSLHJ.Text = dTemp1.ToString("f0");

            dataGridView1.DataSource = dSet.Tables["��Ʒ��"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].DefaultCellStyle.Format = "f0";
            dataGridView1.Columns[7].DefaultCellStyle.Format = "f2";
            dataGridView1.Columns[8].DefaultCellStyle.Format = "f2"; 
            //dataGridView1.Columns[0].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //Ȩ�޿���
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


            toolStripStatusLabel1.Text = "��¼�� "+dSet.Tables["��Ʒ��"].Rows.Count.ToString();

        }

        private void toolStripButtonHZ_Click(object sender, EventArgs e)
        {
            bDPFX = false;

            int i,j;
            bool bMX = true;

            if (MessageBox.Show("�Ƿ������ϸ��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                bMX = false;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT �ⷿ��.�ⷿ����, ��Ʒ��.��Ʒ����, ����.�������, ����.���ɱ���,(����.�������*����.���ɱ���) AS �����, ��Ʒ��.������ FROM ���� INNER JOIN �ⷿ�� ON ����.�ⷿID = �ⷿ��.ID INNER JOIN ��Ʒ�� ON ����.��ƷID = ��Ʒ��.ID WHERE (�ⷿ��.BeActive = 1) AND (��Ʒ��.beactive = 1) AND (����.BeActive = 1) ";

            if (intCommID != 0)
                sqlComm.CommandText += " AND (��Ʒ��.ID = " + intCommID.ToString() + ")";
            if (intKFID != 0)
                sqlComm.CommandText += "AND (�ⷿ��.ID = " + intKFID.ToString() + ")";

            switch (comboBoxC.SelectedIndex.ToString())
            {
                case "1":
                    sqlComm.CommandText += " AND (����.������� <= " + numericUpDownC.Value.ToString() + ") ";
                    break;
                case "2":
                    sqlComm.CommandText += " AND (����.������� >= " + numericUpDownC.Value.ToString() + ") ";
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

            decimal dTemp = 0, dTemp1 = 0; ;
            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��"].Rows[i][2].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][2] = 0;
                if (dSet.Tables["��Ʒ��"].Rows[i][4].ToString() == "")
                    dSet.Tables["��Ʒ��"].Rows[i][4] = 0;
                dTemp += Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][4]);
                dTemp1 += Convert.ToDecimal(dSet.Tables["��Ʒ��"].Rows[i][2]);

            }
            //labelKCJEHJ.Text = dTemp.ToString("f2");
            labelKCSLHJ.Text = dTemp1.ToString("f0");

            dataGridView1.DataSource = dSet.Tables["��Ʒ��"];

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
            dTable.Columns.Add("�ⷿ����", System.Type.GetType("System.String"));
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
                            if (t == 0 || t == 2)
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



            dataGridView1.DataSource = dTable;
            toolStripStatusLabel1.Text = "";
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            //Ȩ�޿���
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
            string strT = "����ѯ;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, true,intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "����ѯ;��ǰ���ڣ�" + labelZDRQ.Text;
            PrintDGV.Print_DataGridView(dataGridView1, strT, false,intUserLimit);
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                //����к� 
                SolidBrush v_SolidBrush = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor);
                int v_LineNo = 0;
                v_LineNo = e.RowIndex + 1;
                string v_Line = v_LineNo.ToString();
                e.Graphics.DrawString(v_Line, e.InheritedRowStyle.Font, v_SolidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
            }
            catch (Exception ex)
            {
                MessageBox.Show("����к�ʱ�������󣬴�����Ϣ��" + ex.Message, "����ʧ��");
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

            // �������Ӵ����һ����ʵ����
            FormSPDPFX childFormSPDPFX = new FormSPDPFX();
            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
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