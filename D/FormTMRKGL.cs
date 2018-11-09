using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormTMRKGL : Form
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

        private string strDJBH = "";
        private int intDJID = 0;
        private string sDJClass = "";

        private bool isSaved = false;
        private ClassGetInformation cGetInformation;


        public FormTMRKGL()
        {
            InitializeComponent();
        }

        private void FormTMRKGL_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            comboBoxLX.Items.Add("����");
            comboBoxLX.Items.Add("�˻�");
            comboBoxLX.Items.Add("�̵�");
            comboBoxLX.Items.Add("����");
            comboBoxLX.Items.Add("����");

            comboBoxLX.Text = "����";

        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (textBoxDJBH.Text.Trim() == "")  //ȫѡ
            {
                switch (comboBoxLX.Text)
                {
                    case "����":
                        if (!checkBoxW.Checked)
                        {
                            if (cGetInformation.getBillInformation(611, textBoxDJBH.Text.Trim()) == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        else
                        {
                            if (cGetInformation.getBillInformation(80061, "") == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        break;
                    case "�˻�":
                        if (!checkBoxW.Checked)
                        {
                            if (cGetInformation.getBillInformation(621, textBoxDJBH.Text.Trim()) == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        else
                        {
                            if (cGetInformation.getBillInformation(80062, "") == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        break;
                    case "�̵�":
                        if (!checkBoxW.Checked)
                        {
                            if (cGetInformation.getBillInformation(631, textBoxDJBH.Text.Trim()) == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        else
                        {
                            if (cGetInformation.getBillInformation(80063, "") == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        break;
                    case "����":
                        if (!checkBoxW.Checked)
                        {
                            if (cGetInformation.getBillInformation(641, textBoxDJBH.Text.Trim()) == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        else
                        {
                            if (cGetInformation.getBillInformation(80064, "") == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        break;
                    case "����":
                        if (!checkBoxW.Checked)
                        {
                            if (cGetInformation.getBillInformation(31, textBoxDJBH.Text.Trim()) == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        else
                        {
                            if (cGetInformation.getBillInformation(80003, "") == 0)
                            {
                                textBoxDJBH.Text = "";
                            }
                            else
                            {
                                textBoxDJBH.Text = cGetInformation.strBillCode;
                            }
                        }
                        break;
                    default:
                        return;

                }
            }
            else
            {
                if (textBoxDJBH.Text.Length < 3)
                    return;


                strDJBH = textBoxDJBH.Text.ToUpper();
                sDJClass = strDJBH.Substring(0, 3);

                switch (strDJBH.Substring(0, 3))
                {
                    case "ADH"://����
                        comboBoxLX.Text = "����";
                        if (cGetInformation.getBillInformation(611, strDJBH) == 0)
                        {
                            textBoxDJBH.Text = "";
                        }
                        else
                        {
                            textBoxDJBH.Text = cGetInformation.strBillCode;
                        }
                        break;
                    case "BTH"://�����˻�
                        comboBoxLX.Text = "�˻�";
                        if (cGetInformation.getBillInformation(621, strDJBH) == 0)
                        {
                            textBoxDJBH.Text = "";
                        }
                        else
                        {
                            textBoxDJBH.Text = cGetInformation.strBillCode;
                        }
                        break;
                    case "CPD": //�̵�
                        comboBoxLX.Text = "�̵�";
                        if (cGetInformation.getBillInformation(631, strDJBH) == 0)
                        {
                            textBoxDJBH.Text = "";
                        }
                        else
                        {
                            textBoxDJBH.Text = cGetInformation.strBillCode;
                        }
                        break;
                    case "CBS": //����
                        comboBoxLX.Text = "����";
                        if (cGetInformation.getBillInformation(641, strDJBH) == 0)
                        {
                            textBoxDJBH.Text = "";
                        }
                        else
                        {
                            textBoxDJBH.Text = cGetInformation.strBillCode;
                        }
                        break;
                    case "CCK": //����
                        comboBoxLX.Text = "����";
                        if (cGetInformation.getBillInformation(30, strDJBH) == 0)
                        {
                            textBoxDJBH.Text = "";
                        }
                        else
                        {
                            textBoxDJBH.Text = cGetInformation.strBillCode;
                        }
                        break;
                    default:
                        MessageBox.Show("û���ҵ��õ��ݣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;


                }

            }

            getBillInfoamtion();
            btnTMDQ_Click(null, null);
        }

         private void getBillInfoamtion()
         {
             if (textBoxDJBH.Text.Trim() == "")
                 return;

            strDJBH=textBoxDJBH.Text.Trim().ToUpper();
            sDJClass = strDJBH.Substring(0, 3);

            sqlConn.Open();
            dataGridViewDJMX.SelectionChanged -= dataGridViewDJMX_SelectionChanged;
            switch (strDJBH.Substring(0, 3))
            {
                case "ADH"://����
                    sqlComm.CommandText = "SELECT ���������ܱ�.���ݱ��, ��λ��.��λ����, ���������ܱ�.����, ְԱ��.ְԱ����, ���������ܱ�.ID, �ɹ���ͬ��.��ͬ��� FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID LEFT OUTER JOIN ������Ʒ�Ƶ��� ON ���������ܱ�.����ID = ������Ʒ�Ƶ���.ID LEFT OUTER JOIN �ɹ���ͬ�� ON ������Ʒ�Ƶ���.��ͬID = �ɹ���ͬ��.ID WHERE (���������ܱ�.���ݱ�� = N'"+ strDJBH +"') AND (���������ܱ�.BeActive = 1)";
                    sqldr=sqlComm.ExecuteReader();

                    if (!sqldr.HasRows)
                    {
                        sqldr.Close();
                        MessageBox.Show("û���ҵ��õ��ݣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        strDJBH = "";
                        break;
                    }

                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        labelDWMC.Text = sqldr.GetValue(1).ToString();
                        labelRQ.Text = Convert.ToDateTime(sqldr.GetValue(2).ToString()).ToString("yyyy��M��dd��");
                        labelYWY.Text = sqldr.GetValue(3).ToString();
                        intDJID = Convert.ToInt32(sqldr.GetValue(4).ToString());
                        labelHTBH.Text = sqldr.GetValue(5).ToString();
                        break;
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ����, ���������ϸ��.����, ���������ϸ��.��ƷID, ���������ϸ��.�ⷿID, ���������ϸ��.ID, ��Ʒ��.��Ʒ��� FROM ���������ϸ�� INNER JOIN ��Ʒ�� ON ���������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID WHERE (���������ϸ��.����ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");
                    sqlDA.Fill(dSet, "������ϸ��");
                    dataGridViewDJMX.DataSource = dSet.Tables["������ϸ��"];

                    dataGridViewDJMX.Columns[3].Visible = false;
                    dataGridViewDJMX.Columns[4].Visible = false;
                    dataGridViewDJMX.Columns[5].Visible = false;
                    dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";
                    break;

                case "BTH"://�����˻�
                    sqlComm.CommandText = "SELECT �����˳����ܱ�.���ݱ��, ��λ��.��λ����, �����˳����ܱ�.����,ְԱ��.ְԱ����,�����˳����ܱ�.ID FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �����˳����ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (�����˳����ܱ�.���ݱ�� = N'" + strDJBH + "') AND (�����˳����ܱ�.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (!sqldr.HasRows)
                    {
                        sqldr.Close();
                        MessageBox.Show("û���ҵ��õ��ݣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        strDJBH = "";
                        break;
                    }

                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        labelDWMC.Text = sqldr.GetValue(1).ToString();
                        labelRQ.Text = Convert.ToDateTime(sqldr.GetValue(2).ToString()).ToString("yyyy��M��dd��");
                        labelYWY.Text = sqldr.GetValue(3).ToString();
                        intDJID = Convert.ToInt32(sqldr.GetValue(4).ToString());
                        labelHTBH.Text = "";
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ����, �����˳���ϸ��.����, �����˳���ϸ��.��ƷID, �����˳���ϸ��.�ⷿID, �����˳���ϸ��.ID, ��Ʒ��.��Ʒ��� FROM �����˳���ϸ�� INNER JOIN ��Ʒ�� ON �����˳���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON �����˳���ϸ��.�ⷿID = �ⷿ��.ID WHERE (�����˳���ϸ��.����ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");
                    sqlDA.Fill(dSet, "������ϸ��");
                    dataGridViewDJMX.DataSource = dSet.Tables["������ϸ��"];

                    dataGridViewDJMX.Columns[3].Visible = false;
                    dataGridViewDJMX.Columns[4].Visible = false;
                    dataGridViewDJMX.Columns[5].Visible = false;

                    dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";
                    break;

                case "CPD": //�̵�
                    sqlComm.CommandText = "SELECT ����̵���ܱ�.���ݱ��, �ⷿ��.�ⷿ����, ����̵���ܱ�.����,ְԱ��.ְԱ����,����̵���ܱ�.ID FROM ����̵���ܱ� INNER JOIN �ⷿ�� ON ����̵���ܱ�.�ⷿID = �ⷿ��.ID INNER JOIN ְԱ�� ON ����̵���ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (����̵���ܱ�.���ݱ�� = N'" + strDJBH + "') AND (����̵���ܱ�.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (!sqldr.HasRows)
                    {
                        sqldr.Close();
                        MessageBox.Show("û���ҵ��õ��ݣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        strDJBH = "";
                        break;
                    }

                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        //labelDWMC.Text = sqldr.GetValue(1).ToString();
                        labelDWMC.Text = "";
                        labelRQ.Text = Convert.ToDateTime(sqldr.GetValue(2).ToString()).ToString("yyyy��M��dd��");
                        labelYWY.Text = sqldr.GetValue(3).ToString();
                        intDJID = Convert.ToInt32(sqldr.GetValue(4).ToString());
                        labelHTBH.Text = "";
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ����, ����̵���ϸ��.��������, ����̵���ϸ��.��ƷID, ����̵���ϸ��.�ⷿID, ����̵���ϸ��.ID, ��Ʒ��.��Ʒ��� FROM ����̵���ϸ�� INNER JOIN ��Ʒ�� ON ����̵���ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ����̵���ϸ��.�ⷿID = �ⷿ��.ID WHERE (����̵���ϸ��.����ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");
                    sqlDA.Fill(dSet, "������ϸ��");
                    dataGridViewDJMX.DataSource = dSet.Tables["������ϸ��"];

                    dataGridViewDJMX.Columns[3].Visible = false;
                    dataGridViewDJMX.Columns[4].Visible = false;
                    dataGridViewDJMX.Columns[5].Visible = false;

                    dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";

                    break;

                case "CBS": //����
                    sqlComm.CommandText = "SELECT ��汨����ܱ�.���ݱ��, �ⷿ��.�ⷿ����, ��汨����ܱ�.����,ְԱ��.ְԱ����,��汨����ܱ�.ID FROM ��汨����ܱ� INNER JOIN �ⷿ�� ON ��汨����ܱ�.�ⷿID = �ⷿ��.ID INNER JOIN ְԱ�� ON ��汨����ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (��汨����ܱ�.���ݱ�� = N'" + strDJBH + "') AND (��汨����ܱ�.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (!sqldr.HasRows)
                    {
                        sqldr.Close();
                        MessageBox.Show("û���ҵ��õ��ݣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        strDJBH = "";
                        break;
                    }

                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        //labelDWMC.Text = sqldr.GetValue(1).ToString();
                        labelDWMC.Text = "";
                        labelRQ.Text = Convert.ToDateTime(sqldr.GetValue(2).ToString()).ToString("yyyy��M��dd��");
                        labelYWY.Text = sqldr.GetValue(3).ToString();
                        intDJID = Convert.ToInt32(sqldr.GetValue(4).ToString());
                        labelHTBH.Text = "";
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ����, ��汨����ϸ��.��������, ��汨����ϸ��.��ƷID, ��汨����ܱ�.�ⷿID,��汨����ϸ��.ID, ��Ʒ��.��Ʒ��� FROM ��汨����ϸ�� INNER JOIN ��Ʒ�� ON ��汨����ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN ��汨����ܱ� ON ��汨����ϸ��.����ID = ��汨����ܱ�.ID INNER JOIN �ⷿ�� ON ��汨����ܱ�.�ⷿID = �ⷿ��.ID WHERE     (��汨����ϸ��.����ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");
                    sqlDA.Fill(dSet, "������ϸ��");
                    dataGridViewDJMX.DataSource = dSet.Tables["������ϸ��"];

                    dataGridViewDJMX.Columns[3].Visible = false;
                    dataGridViewDJMX.Columns[4].Visible = false;
                    dataGridViewDJMX.Columns[5].Visible = false;

                    dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";

                    break;

                case "CCK": //����
                    sqlComm.CommandText = "SELECT ���������ܱ�.���ݱ��, ��λ��.��λ����, ���������ܱ�.����,ְԱ��.ְԱ����,���������ܱ�.ID FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���������ܱ�.���ݱ�� = N'" + strDJBH + "') AND (���������ܱ�.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (!sqldr.HasRows)
                    {
                        sqldr.Close();
                        MessageBox.Show("û���ҵ��õ��ݣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        strDJBH = "";
                        break;
                    }

                    while (sqldr.Read())
                    {
                        labelDJBH.Text = sqldr.GetValue(0).ToString();
                        labelDWMC.Text = sqldr.GetValue(1).ToString();
                        labelRQ.Text = Convert.ToDateTime(sqldr.GetValue(2).ToString()).ToString("yyyy��M��dd��");
                        labelYWY.Text = sqldr.GetValue(3).ToString();
                        intDJID = Convert.ToInt32(sqldr.GetValue(4).ToString());
                        labelHTBH.Text = "";
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT ��Ʒ��.��Ʒ����, �ⷿ��.�ⷿ����, ���������ϸ��.����, ���������ϸ��.��ƷID, ���������ϸ��.�ⷿID,���������ϸ��.ID, ��Ʒ��.��Ʒ��� FROM ���������ϸ�� INNER JOIN ��Ʒ�� ON ���������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID WHERE (���������ϸ��.��ID = " + intDJID.ToString() + ")";

                    if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");
                    sqlDA.Fill(dSet, "������ϸ��");
                    dataGridViewDJMX.DataSource = dSet.Tables["������ϸ��"];

                    dataGridViewDJMX.Columns[3].Visible = false;
                    dataGridViewDJMX.Columns[4].Visible = false;
                    dataGridViewDJMX.Columns[5].Visible = false;

                    dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f0";

                    break;

                default:
                    sDJClass = "";
                    intDJID = 0;
                    textBoxDJBH.Text = "";
                    break;

            }
            sqlConn.Close();
            dataGridViewDJMX.SelectionChanged += dataGridViewDJMX_SelectionChanged;
            dataGridViewDJMX_SelectionChanged(null, null);
            //inittoolStripStatusLabelTS();
 
        }

         private void inittoolStripStatusLabelTS()
         {
             if (labelDJBH.Text == "")
             {
                 toolStripStatusLabelTS.Text = "";
                 return;
             }

             int iSum=0;
             for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
             {
                 try
                 {
                     iSum += (int)decimal.Parse(dataGridViewDJMX.Rows[i].Cells[2].Value.ToString());
                 }
                 catch(Exception e)
                 {
                 }
             }

            int iTM=0;
             sqlConn.Open();
            sqlComm.CommandText = "SELECT COUNT(*) FROM ��Ʒ����� WHERE (���ݱ�� = N'"+labelDJBH.Text+"') AND (������� = 0)";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                iTM=int.Parse(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            sqlConn.Close();

            toolStripStatusLabelTS.Text = "������"+iSum.ToString()+" ��ɨ¼����������"+iTM.ToString();
             

         }

         private void inittoolStripStatusLabelTS1()
         {
             if (dataGridViewDJMX.Rows.Count < 1 || dataGridViewDJMX.SelectedRows.Count<1)
             {
                 toolStripStatusLabelTS1.Text = "";
                 return;
             }

             int iSum = (int)decimal.Parse(dataGridViewDJMX.SelectedRows[0].Cells[2].Value.ToString());
             int iTM = dataGridViewTM.RowCount;

             if (iSum > iTM)
                 toolStripStatusLabelTS1.ForeColor = Color.Red;
             else
                 toolStripStatusLabelTS1.ForeColor = Color.Black;
             toolStripStatusLabelTS1.Text = "������" + iSum.ToString() + " ��ɨ¼����������" + iTM.ToString();
         }

        private void initTMView(int iSPID, int iKFID, int MXID)
        {
            if (strDJBH == "")
            {
                if (dSet.Tables.Contains("�����")) dSet.Tables.Remove("�����");
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ID, ����, ���� FROM ��Ʒ����� WHERE (���ݱ�� = N'" + strDJBH + "') AND (������ϸID=" + MXID.ToString() + ") ";

            if (iSPID != 0) //��Ʒ����
                sqlComm.CommandText += " AND (��ƷID = " + iSPID.ToString() + ")AND (�ⷿID = " + iKFID.ToString() + ")";
            sqlComm.CommandText += " ORDER BY ���� DESC";

            if (dSet.Tables.Contains("�����")) dSet.Tables.Remove("�����");
            sqlDA.Fill(dSet, "�����");
            dataGridViewTM.DataSource = dSet.Tables["�����"];
            sqlConn.Close();
            inittoolStripStatusLabelTS1();
            inittoolStripStatusLabelTS();
        }

        private void btnTMDQ_Click(object sender, EventArgs e)
        {
            this.textBoxTM.Focus();
            this.textBoxTM.SelectAll();
        }

        private void dataGridViewDJMX_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewDJMX.SelectedRows.Count < 1)
            {
                labelSPMC.Text = "";
                return;
            }

            labelSPMC.Text = dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString();
            
            if(dataGridViewDJMX.SelectedRows[0].Cells[3].Value.ToString()=="")
                dataGridViewDJMX.SelectedRows[0].Cells[3].Value=0;
            if (dataGridViewDJMX.SelectedRows[0].Cells[4].Value.ToString() == "")
                dataGridViewDJMX.SelectedRows[0].Cells[4].Value = 0;

            initTMView(Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[3].Value), Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[4].Value), Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[5].Value));
            dataGridViewTM.Columns[0].Visible = false;

            textBoxTM.Text = "";
            btnTMDQ_Click(null, null);

        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            initTMView(0, 0, 0);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewTM.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫɾ�������룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("�Ƿ�Ҫɾ��ѡ�������룿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                //��ϸ
                for (int i = 0; i < dataGridViewTM.SelectedRows.Count; i++)
                {

                    sqlComm.CommandText = "DELETE FROM ��Ʒ����� WHERE (ID = " + dataGridViewTM.SelectedRows[i].Cells[0].Value.ToString()+ ")";
                    sqlComm.ExecuteNonQuery();

                }

                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ݿ����" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                return;
            }
            finally
            {
                sqlConn.Close();
            }
            MessageBox.Show("ɾ��������ϣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dataGridViewDJMX_SelectionChanged(null, null);
        }

        private void textBoxTM_KeyPress(object sender, KeyPressEventArgs e)
        {
            string sZY="";
            int iCount = 0;

            if (e.KeyChar == (char)Keys.Return)
            {
                if (textBoxTM.Text == "")
                {
                    labelWARN.Text = "";
                    return;
                }

                if (dataGridViewDJMX.SelectedRows.Count < 1)
                {
                    MessageBox.Show("��ѡ�����������Ӧ����Ʒ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxTM.SelectAll();
                    return;
                }

                string strDT;
                cGetInformation.getSystemDateTime();
                strDT = cGetInformation.strSYSDATATIME;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                //�Ƿ�������¼
                /*
                sqlComm.CommandText = "SELECT ID, ���ݱ��, ժҪ, ���� FROM ��Ʒ����� WHERE (������� = 0) AND (���� = N'"+textBoxTM.Text.ToUpper()+"')";
                sqldr = sqlComm.ExecuteReader();

                if (sqldr.HasRows) //������¼
                {
                    if (sDJClass != "BTH") //���˻���Ʒ
                    {
                        sqldr.Read();
                        labelWARN.Text = "���и���������¼�����ݱ�ţ�"+sqldr.GetValue(1).ToString();
                        textBoxTM.SelectAll();
                        sqldr.Close();
                        sqlConn.Close();
                        return;
                    }
                }
                sqldr.Close();
                */
                sqlComm.CommandText = "SELECT ID, ���ݱ��, ժҪ, ����, ������� FROM ��Ʒ����� WHERE (���� = N'" + textBoxTM.Text.ToUpper() + "') ORDER BY ���� DESC, ID DESC";

                sqldr = sqlComm.ExecuteReader();

                if (sqldr.HasRows) //������¼
                {
                    sqldr.Read();
                    if (!bool.Parse(sqldr.GetValue(4).ToString()))
                    {
                        labelWARN.Text = "����������Ϊ����¼�����ݱ�ţ�" + sqldr.GetValue(1).ToString();
                        textBoxTM.SelectAll();
                        sqldr.Close();
                        sqlConn.Close();
                        return;
                    }
                }
                sqldr.Close();

                //����У��
                iCount = 0;
                sqlComm.CommandText = "SELECT COUNT(*) AS ���� FROM ��Ʒ����� WHERE (���ݱ�� = N'" + strDJBH + "') AND (������ϸID=" + dataGridViewDJMX.SelectedRows[0].Cells[5].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.SelectedRows[0].Cells[3].Value.ToString() + ") AND (������� = 0)";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    iCount = Convert.ToInt32(sqldr.GetValue(0).ToString());
                }
                sqldr.Close();

                //int iTemp = Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[2].Value);
                if (iCount >= Math.Abs(Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[2].Value)))
                {
                    labelWARN.Text = "������Ʒ������������";
                    textBoxTM.SelectAll();
                    sqldr.Close();
                    sqlConn.Close();
                    return;
                }


                //ժҪ
                switch (sDJClass)
                {
                    case "ADH"://�������
                        sZY = "��������Ƶ�";
                        break;
                    case "BTH"://�����˻�
                        sZY = "�����˻��Ƶ�";
                        break;

                    case "CPD": //�̵�
                        sZY = "��Ʒ�̵�";
                        break;

                    case "CBS": //����
                        sZY = "��Ʒ����";
                        break;
                    case "CCK": //����
                        sZY = "����";
                        break;
                }

                sqlComm.CommandText = "INSERT INTO ��Ʒ����� (����, ��ƷID, �ⷿID, ���ݱ��, ժҪ, ����, �������, ����ԱID, ������ϸID) VALUES (N'" + textBoxTM.Text + "', " + dataGridViewDJMX.SelectedRows[0].Cells[3].Value.ToString() + ", " + dataGridViewDJMX.SelectedRows[0].Cells[4].Value.ToString() + ", N'" + strDJBH + "', N'" + sZY + "', '" + strDT + "', 0, " + intUserID.ToString() + "," + dataGridViewDJMX.SelectedRows[0].Cells[5].Value.ToString() + ")";
                sqlComm.ExecuteNonQuery();


                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ݿ����" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                return;
            }
            finally
            {
                sqlConn.Close();
            }


                labelWARN.Text = "����¼��ɹ�";
                initTMView(Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[3].Value.ToString()), Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[4].Value.ToString()), Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[5].Value.ToString()));

                textBoxTM.SelectAll();
            }
        }

        private void textBoxTM_Enter(object sender, EventArgs e)
        {
            textBoxTM.SelectAll();
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            int i,j;
            if (dSet.Tables.Contains("�����ӡ��")) dSet.Tables.Remove("�����ӡ��");
            dSet.Tables.Add("�����ӡ��");
            dSet.Tables["�����ӡ��"].Columns.Add("��Ʒ����", System.Type.GetType("System.String"));
            string[] strDRow ={ "" };

            sqlConn.Open();
            for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                strDRow[0] = "��Ʒ:" + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString();
                dSet.Tables["�����ӡ��"].Rows.Add(strDRow);
                
                sqlComm.CommandText = "SELECT ���� FROM ��Ʒ����� WHERE (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[3].Value.ToString() + ") AND (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ") AND (���ݱ�� = N'"+strDJBH+"')";
                sqldr = sqlComm.ExecuteReader();
                j = 1;
                while (sqldr.Read())
                {
                    strDRow[0] = "��"+j.ToString()+":"+sqldr.GetValue(0).ToString();
                    dSet.Tables["�����ӡ��"].Rows.Add(strDRow);
                    j++;
                }
                sqldr.Close();
            }
            sqlConn.Close();

            dataGridViewPR.DataSource = dSet.Tables["�����ӡ��"];
            //dataGridViewPR.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            string strT = "��Ʒ�����;;";
            PrintDGV.Print_DataGridView(dataGridViewPR, strT, true, intUserLimit);

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            int i, j;
            if (dSet.Tables.Contains("�����ӡ��")) dSet.Tables.Remove("�����ӡ��");
            dSet.Tables.Add("�����ӡ��");
            dSet.Tables["�����ӡ��"].Columns.Add("��Ʒ����", System.Type.GetType("System.String"));
            string[] strDRow ={ "" };

            sqlConn.Open();
            for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                strDRow[0] = "��Ʒ:" + ":" + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString();
                dSet.Tables["�����ӡ��"].Rows.Add(strDRow);

                sqlComm.CommandText = "SELECT ���� FROM ��Ʒ����� WHERE (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[3].Value.ToString() + ") AND (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() + ") AND (���ݱ�� = N'" + strDJBH + "')";
                sqldr = sqlComm.ExecuteReader();
                j = 1;
                while (sqldr.Read())
                {
                    strDRow[0] = "��" + j.ToString() + ":" + sqldr.GetValue(0).ToString();
                    dSet.Tables["�����ӡ��"].Rows.Add(strDRow);
                    j++;
                }
                sqldr.Close();
            }
            sqlConn.Close();

            dataGridViewPR.DataSource = dSet.Tables["�����ӡ��"];
            string strT = "��Ʒ�����;;";
            PrintDGV.Print_DataGridView(dataGridViewPR, strT, false, intUserLimit);

        }

        private void textBoxDJBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                textBoxDJBH.Text = textBoxDJBH.Text.Trim().ToUpper();
                try
                {
                    int.Parse(textBoxDJBH.Text.Substring(0, 1));
                    switch (comboBoxLX.SelectedIndex)
                    {
                        case 0:
                            textBoxDJBH.Text = "ADH" + textBoxDJBH.Text;
                            break;
                        case 1:
                            textBoxDJBH.Text = "BTH" + textBoxDJBH.Text;
                            break;
                        case 2:
                            textBoxDJBH.Text = "CPD" + textBoxDJBH.Text;
                            break;
                        case 3:
                            textBoxDJBH.Text = "CBS" + textBoxDJBH.Text;
                            break;
                        case 4:
                            textBoxDJBH.Text = "CCK" + textBoxDJBH.Text;
                            break;


                    }
                }
                catch
                {
                }
                btnSelect_Click(null, null);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //return EnterToTab(ref   msg, keyData, true);
            int i;

            if (keyData == Keys.Up)
            {
                i = dataGridViewDJMX.SelectedRows[0].Index;

                if (i == 0)
                    i = dataGridViewDJMX.RowCount - 1;
                else
                    i--;

                dataGridViewDJMX.Rows[i].Selected = true;
                dataGridViewDJMX.FirstDisplayedScrollingRowIndex = i;
                return true;
            }

            if (keyData == Keys.Down)
            {
                i = dataGridViewDJMX.SelectedRows[0].Index;

                if (i == dataGridViewDJMX.RowCount - 1)
                    i = 0;
                else
                    i++;

                dataGridViewDJMX.Rows[i].Selected = true;
                dataGridViewDJMX.FirstDisplayedScrollingRowIndex = i;
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }



    }
}