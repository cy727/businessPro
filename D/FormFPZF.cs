using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormFPZF : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private int iFPH = 0;
        private string strFPH = "";

        private ClassGetInformation cGetInformation;

        public FormFPZF()
        {
            InitializeComponent();
        }


        private void FormFPZF_Load(object sender, EventArgs e)
        {
            int i;
            
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

            comboBoxStyle.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (textBoxFPH.Text == "")
            {
                textBoxFPH.Text = strFPH;
                return;
            }

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʊ���ܱ�.ID, ��Ʊ���ܱ�.��Ʊ��, ��λ��.��λ���, ��λ��.��λ����, ��Ʊ���ܱ�.��ע, ��Ʊ���ܱ�.������ʽ, ��Ʊ���ܱ�.����, ��Ʊ���ܱ�.��Ʊ����, ��Ʊ���ܱ�.����, ��Ʊ���ܱ�.ԭ��Ʊ���, ��Ʊ���ܱ�.��Ʊ�ܶ� FROM ��Ʊ���ܱ� INNER JOIN ��λ�� ON ��Ʊ���ܱ�.��λID = ��λ��.ID WHERE (��Ʊ���ܱ�.BeActive = 1) AND (��Ʊ���ܱ�.��Ʊ�� = N'" + textBoxFPH.Text + "')";
            if (dSet.Tables.Contains("��Ʊ��")) dSet.Tables.Remove("��Ʊ��");
            sqlDA.Fill(dSet, "��Ʊ��");

            if (dSet.Tables["��Ʊ��"].Rows.Count < 1) //û�з�Ʊ
            {
                textBoxFPH.Text = strFPH;
                sqlConn.Close();
                return;
            }


            if (dSet.Tables["��Ʊ��"].Rows.Count == 1) //ֻ��һ����Ʊ
            {
                iFPH = Int32.Parse(dSet.Tables["��Ʊ��"].Rows[0][0].ToString());
            }
            else //�����Ʊ
            {
                sqlComm.CommandText = "SELECT ��Ʊ���ܱ�.ID, ��Ʊ���ܱ�.��λID, ��Ʊ���ܱ�.��Ʊ��, ��λ��.��λ���, ��λ��.��λ����, ��Ʊ���ܱ�.����, ��Ʊ���ܱ�.��Ʊ�ܶ�, ��Ʊ���ܱ�.��ע, ��Ʊ���ܱ�.����ԱID, ְԱ��.ְԱ����, ��Ʊ���ܱ�.����ԱID AS ����Ա FROM ��Ʊ���ܱ� INNER JOIN ��λ�� ON ��Ʊ���ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ��Ʊ���ܱ�.����ԱID = ְԱ��.ID WHERE (��Ʊ���ܱ�.BeActive = 1) AND (��Ʊ���ܱ�.��Ʊ�� = N'" + textBoxFPH.Text + "')";
                FormSelectBill frmSelectBill = new FormSelectBill();
                frmSelectBill.strConn = strConn;
                frmSelectBill.strSelectText = sqlComm.CommandText;
                frmSelectBill.bShowDW = true;

                frmSelectBill.ShowDialog();

                if (frmSelectBill.iBillNumber == 0)
                {
                    sqlConn.Close();
                    return;
                }
                else
                {
                    iFPH = frmSelectBill.iBillNumber;
                }
            }

            sqlComm.CommandText = "SELECT ��Ʊ���ܱ�.ID, ��Ʊ���ܱ�.��Ʊ��, ��λ��.��λ���, ��λ��.��λ����, ��Ʊ���ܱ�.��ע, ��Ʊ���ܱ�.������ʽ, ��Ʊ���ܱ�.����, ��Ʊ���ܱ�.��Ʊ����, ��Ʊ���ܱ�.����, ��Ʊ���ܱ�.ԭ��Ʊ���, ��Ʊ���ܱ�.��Ʊ�ܶ� FROM ��Ʊ���ܱ� INNER JOIN ��λ�� ON ��Ʊ���ܱ�.��λID = ��λ��.ID WHERE (��Ʊ���ܱ�.BeActive = 1) AND (��Ʊ���ܱ�.ID = " + iFPH.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();

            if (!sqldr.HasRows)
            {
                textBoxFPH.Text = strFPH;
                sqldr.Close();
                sqlConn.Close();
                return;
            }

            while (sqldr.Read())
            {
                iFPH = Convert.ToInt32(sqldr.GetValue(0).ToString());
                strFPH = sqldr.GetValue(1).ToString();
                textBoxDWBH.Text = sqldr.GetValue(2).ToString();
                textBoxDWMC.Text = sqldr.GetValue(3).ToString();
                textBoxBZ.Text = sqldr.GetValue(4).ToString();
                comboBoxFHFS.Text = sqldr.GetValue(5).ToString();
                textBoxDH.Text=  sqldr.GetValue(6).ToString();
                comboBoxStyle.SelectedIndex = Convert.ToInt32(sqldr.GetValue(7).ToString());
                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(8).ToString()).ToString("yyyy��M��dd��");
                labelJEHJ.Text = sqldr.GetValue(9).ToString();
                labelSJJE.Text = sqldr.GetValue(10).ToString();
                labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            }
            sqldr.Close();

            switch (comboBoxStyle.SelectedIndex)
            {
                case 0:
                    //��ʼ����ϸ�б�
                    sqlComm.CommandText = "SELECT ����ID, ���ID, ���ݱ��, ��ֱ��, ԭ��Ʊ�ܶ�, ��Ʊ�ܶ�, ������ʽ, ����, ��ע1, ��ע2, ID FROM ��Ʊ��ϸ�� WHERE  (��Ʊ��ϸ��.��ƱID = " + iFPH.ToString() + ")";

                    if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");
                    sqlDA.Fill(dSet, "������ϸ��");
                    dataGridViewDJMX.DataSource = dSet.Tables["������ϸ��"];

                    dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[1].Visible = false;
                    dataGridViewDJMX.Columns[10].Visible = false;

                    dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                    break;
                case 1:
                    sqlComm.CommandText = "SELECT ����ID, ���ID, ���ݱ��, ��ֱ��, ԭ��Ʊ�ܶ�, ��Ʊ�ܶ�, ������ʽ, ����, ��ע1, ��ע2, ID FROM ��Ʊ��ϸ�� WHERE  (��Ʊ��ϸ��.��ƱID = " + iFPH.ToString() + ")";

                    if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");
                    sqlDA.Fill(dSet, "������ϸ��");
                    dataGridViewDJMX.DataSource = dSet.Tables["������ϸ��"];

                    dataGridViewDJMX.Columns[0].Visible = false;
                    dataGridViewDJMX.Columns[1].Visible = false;
                    dataGridViewDJMX.Columns[10].Visible = false;

                    dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    break;

            }

            sqlConn.Close();
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            string sTemp;

            if (iFPH == 0)
            {
                MessageBox.Show("��ѡ��Ҫ���ϵķ�Ʊ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("�Ƿ�����ѡ���ķ�Ʊ��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;


            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                //������
                sqlComm.CommandText = "UPDATE ��Ʊ���ܱ� SET BeActive = 0 WHERE (ID = "+iFPH.ToString()+")";
                sqlComm.ExecuteNonQuery();

                //��ϸ
                for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                   sTemp = dataGridViewDJMX.Rows[i].Cells[2].Value.ToString().Substring(0, 3);
                    switch (comboBoxStyle.SelectedIndex)
                    {
                        case 0:
                            switch (sTemp)
                            {
                                case "ADH":

                                    sqlComm.CommandText = "UPDATE ���������ܱ� SET ��Ʊ�� = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();

                                    sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET ��Ʊ�� = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[1].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                                case "ATH":

                                    sqlComm.CommandText = "UPDATE �����˳����ܱ� SET ��Ʊ�� = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                                case "ATB":

                                    sqlComm.CommandText = "UPDATE �����˲���ۻ��ܱ� SET ��Ʊ�� = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                                default:
                                    break;
                            }

                            break;

                        case 1:
                            switch (sTemp)
                            {
                                case "BKP":

                                    sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET ��Ʊ�� = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                                case "BTH":

                                    sqlComm.CommandText = "UPDATE �����˳����ܱ� SET ��Ʊ�� = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                                case "BTB":

                                    sqlComm.CommandText = "UPDATE �����˲���ۻ��ܱ� SET ��Ʊ�� = NULL WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() + ")";
                                    sqlComm.ExecuteNonQuery();
                                    break;
                                default:
                                    break;
                            }
                            break;

                    }


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



            MessageBox.Show("��Ʊ�ɹ��ϳ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            iFPH = 0;
            strFPH = "";
            textBoxDWBH.Text = "";
            textBoxDWMC.Text = "";
            textBoxBZ.Text = "";
            comboBoxFHFS.Text = "";
            textBoxDH.Text = "";
            comboBoxStyle.SelectedIndex = 0;
            labelZDRQ.Text = "";
            labelJEHJ.Text = "0";
            labelSJJE.Text = "0";
            labelDX.Text = "��";
            if (dSet.Tables.Contains("������ϸ��")) dSet.Tables.Remove("������ϸ��");



        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��Ʊ��ѯ;���ڣ�" + labelZDRQ.Text + ";��λ���ƣ�" + textBoxDWMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��Ʊ��ѯ;���ڣ�" + labelZDRQ.Text + ";��λ���ƣ�" + textBoxDWMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }
    }
}