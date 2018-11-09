using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormJHRKYHD_EDIT : Form
    {

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";
        public int intDJID = 0;

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private int iSupplyCompany = 0;
        private int intHTH = 0;

        private bool isSaved = false;
        private int iYWY = 0;

        private ClassGetInformation cGetInformation;
        private bool bCheck = true;
        private int iBM = 0;


        public FormJHRKYHD_EDIT()
        {
            InitializeComponent();
        }

        private void FormJHRKYHD_EDIT_Load(object sender, EventArgs e)
        {
            int i;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
            this.Top = 1;
            this.Left = 1;


            if (intDJID == 0)
                return;

            this.Text += ":���ݳ��";

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ���������ܱ�.���ݱ��, ���������ܱ�.����, [ְԱ��_1].ְԱ���� AS ����Ա, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.��Ʊ��, ���������ܱ�.֧Ʊ��, ���������ܱ�.��ͬ��, ���������ܱ�.��˰�ϼ�, ���������ܱ�.��ע, ��λ��.ID,���������ܱ�.ҵ��ԱID, ���������ܱ�.����ID  FROM ���������ܱ� INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ���������ܱ�.����ԱID = [ְԱ��_1].ID INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID WHERE (���������ܱ�.ID = " + intDJID.ToString() + ") AND (���������ܱ�.BeActive<>0)";
            sqldr=sqlComm.ExecuteReader();

            if (!sqldr.HasRows)
            {
                isSaved = true;
                sqldr.Close();
                sqlConn.Close();
                return;
            }
            while (sqldr.Read())
            {
                if (sqldr.GetValue(13).ToString() != "")
                {
                    try
                    {
                        iBM = int.Parse(sqldr.GetValue(13).ToString());
                    }
                    catch
                    {
                        iBM = 0;
                    }

                }

                labelZDRQ.Text = Convert.ToDateTime(sqldr.GetValue(1).ToString()).ToString("yyyy��M��dd��");
                labelDJBH.Text = sqldr.GetValue(0).ToString();
                labelCZY.Text = sqldr.GetValue(2).ToString();
                comboBoxYWY.Text = sqldr.GetValue(3).ToString();
                textBoxDWBH.Text = sqldr.GetValue(4).ToString();
                textBoxDWMC.Text = sqldr.GetValue(5).ToString();
                textBoxFPH.Text = sqldr.GetValue(6).ToString();
                textBoxZPH.Text = sqldr.GetValue(7).ToString();
                textBoxHTH.Text = sqldr.GetValue(8).ToString();
                textBoxBZ.Text = sqldr.GetValue(10).ToString();
                iSupplyCompany = Convert.ToInt32(sqldr.GetValue(11).ToString());
                iYWY = Convert.ToInt32(sqldr.GetValue(12).ToString());
            }

            sqldr.Close();


            if (iBM != 0)
            {
                sqlComm.CommandText = "SELECT �������� FROM ���ű� WHERE (ID = " + iBM.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();
                while (sqldr.Read())
                {
                    comboBoxBM.Items.Add(sqldr.GetValue(0).ToString());
                    comboBoxBM.Text = sqldr.GetValue(0).ToString();
                    break;
                }
                sqldr.Close();
            }


            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���ϸ�����.����, ���������ܱ�.���ݱ��, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ���������ϸ��.����, ���������ϸ��.����, ���������ϸ��.���, ���������ϸ��.����, ���������ϸ��.ʵ�ƽ��, ��Ʒ��.�������, ���������ϸ��.��ƷID, ���������ϸ��.�ⷿID, ���������ϸ��.ID, ���������ϸ��.��Ʒ, ���������ܱ�.ID AS Expr1, ���������ϸ��.ԭ������ϸID, ���������ϸ��.ԭ����ID FROM ���������ϸ�� INNER JOIN  ��Ʒ�� ON ���������ϸ��.��ƷID = ��Ʒ��.ID INNER JOIN �ⷿ�� ON ���������ϸ��.�ⷿID = �ⷿ��.ID INNER JOIN ���������ܱ� ON ���������ϸ��.����ID = ���������ܱ�.ID CROSS JOIN ������Ʒ�Ƶ���ϸ����� WHERE (���������ϸ��.����ID = " + intDJID.ToString() + ")";

            if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
            sqlDA.Fill(dSet, "���ݱ�");
            dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];

            sqlConn.Close();

            
            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[8].Visible = false;
            dataGridViewDJMX.Columns[13].Visible = false;
            dataGridViewDJMX.Columns[14].Visible = false;
            dataGridViewDJMX.Columns[15].Visible = false;
            dataGridViewDJMX.Columns[16].Visible = false;
            dataGridViewDJMX.Columns[18].Visible = false;
            dataGridViewDJMX.Columns[19].Visible = false;
            dataGridViewDJMX.Columns[20].Visible = false;
            
            dataGridViewDJMX.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


            countAmount();

        }

        private void countAmount()
        {
            decimal fSum = 0, fSum1 = 0;
            decimal fTemp, fTemp1;
            decimal fCount = 0, fCSum = 0;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;


                //����

                if (dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() == "")
                    fTemp = 0;
                else
                    fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                fCSum += fTemp;

                //����
                if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "")
                    fTemp1 = 0;
                else
                    fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);

                //���
                dataGridViewDJMX.Rows[i].Cells[10].Value = Math.Round(fTemp * fTemp1, 2);

                //����
                if (dataGridViewDJMX.Rows[i].Cells[11].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[11].Value = 100;
                }

                //ʵ�ƽ��
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                dataGridViewDJMX.Rows[i].Cells[12].Value = fTemp1 * Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[11].Value.ToString()) / 100;

                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[10].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[12].Value);

                fCount += 1;

            }
            labelSLHJ.Text = fCSum.ToString("f0");
            labelJEHJ.Text = fSum.ToString();
            labelSJJE.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();


        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            int i,j;
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE=0,dKCJE1=0,dYSYE=0,dYSYE1=0;

            
            //�������
            if (isSaved)
            {
                MessageBox.Show("�������������Ѿ����,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string strDateSYS;
            cGetInformation.getSystemDateTime();
            strDateSYS = cGetInformation.strSYSDATATIME;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            //�����
            sqlComm.CommandText = "SELECT ���㸶����ܱ�.���ݱ�� FROM ���㸶��ұ� INNER JOIN ���㸶����ܱ� ON ���㸶��ұ�.����ID = ���㸶����ܱ�.ID WHERE (���㸶��ұ�.���ݱ�� = N'" + labelDJBH.Text + "') AND (���㸶��ұ�.BeActive = 1)";
            sqldr = sqlComm.ExecuteReader();
            if (sqldr.HasRows)
            {
                while (sqldr.Read())
                {
                    MessageBox.Show("���в��񹴶Ҽ�¼,���ݺ�Ϊ��" + sqldr.GetValue(0).ToString(), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                }
                sqldr.Close();
                sqlConn.Close();
                return;
            }
            sqldr.Close();

            //��Ʊ��¼
            sqlComm.CommandText = "SELECT ��Ʊ��, ID FROM ���������ܱ� WHERE (��Ʊ�� IS NOT NULL) AND (��Ʊ�� NOT LIKE N'����Ʊ%') AND (ID = " + intDJID.ToString() + ") AND (��Ʊ�� NOT LIKE N'�ֽ𲻿�Ʊ%')";
            sqldr = sqlComm.ExecuteReader();
            bool b=false;
            if (sqldr.HasRows)
            {
                while (sqldr.Read())
                {
                    if (sqldr.GetValue(0).ToString().Trim() != "")
                    {
                        MessageBox.Show("���з�Ʊ��¼,��Ʊ��Ϊ��" + sqldr.GetValue(0).ToString(), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        b = true;
                    }
                    break;
                }
                if (b)
                {
                    sqldr.Close();
                    sqlConn.Close();
                    return;
                }
            }
           sqldr.Close();

           //�õ��ϴν�תʱ��
           string sSCJZSJ = "";
           sqlComm.CommandText = "SELECT ����ʱ��,ID FROM ��ת���ܱ� ORDER BY ����ʱ�� DESC";
           sqldr = sqlComm.ExecuteReader();
           if (sqldr.HasRows)
           {
               sqldr.Read();
               sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
           }
           sqldr.Close();

           if (sSCJZSJ == "") //û�н���
           {
               sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
               sqldr = sqlComm.ExecuteReader();
               while (sqldr.Read())
               {
                   sSCJZSJ = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
               }
               sqldr.Close();
           }

           //�õ��Ƶ�����
           string strDate1 = "";
           sqlComm.CommandText = "SELECT ���� from ���������ܱ� WHERE (ID = " + intDJID.ToString() + ")";
           sqldr = sqlComm.ExecuteReader();

           while (sqldr.Read())
           {
               strDate1 = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
           }
           sqldr.Close();

           if (DateTime.Parse(strDate1) <= DateTime.Parse(sSCJZSJ)) //��ת���¼
           {
               if (MessageBox.Show("�Ƶ�������ת���¼��" + sSCJZSJ + "���Ƿ�ǿ�г�죿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
               {
                   sqlConn.Close();
                   return;
               }
           }
            


            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                //������
                string sBMID = "NULL";
                if (iBM != 0)
                    sBMID = iBM.ToString();

                sqlComm.CommandText = "UPDATE ���������ܱ� SET BeActive = 0 WHERE (ID = "+intDJID.ToString()+")";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE ���������ܱ� SET ���ʱ�� = '" + strDateSYS + "' WHERE (ID = " + intDJID.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //��λӦ����
                sqlComm.CommandText = "SELECT Ӧ���˿� FROM ��λ�� WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqldr=sqlComm.ExecuteReader();

                dKCJE = 0;
                while (sqldr.Read())
                {
                    dKCJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                }
                sqldr.Close();
                dKCJE1 = dKCJE - Convert.ToDecimal(labelSJJE.Text);
                sqlComm.CommandText = "UPDATE ��λ�� SET Ӧ���˿� = " + dKCJE1.ToString() + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //��λ��ʷ��¼
                sqlComm.CommandText = "INSERT INTO ��λ��ʷ�˱� (��λID, ����, ���ݱ��, ժҪ, �������, Ӧ�����, �������, ҵ��ԱID, ��ֵ���, BeActive, ����ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + labelDJBH.Text + "��', N'���������������', -" + labelSJJE.Text.ToString() + ", " + dKCJE1.ToString() + ", 1, " + iYWY.ToString() + ", N'" + labelDJBH.Text + "', 1,"+sBMID+")";
                sqlComm.ExecuteNonQuery();

                //δ�����ָ�
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ���ϸ�� SET δ�������� =δ��������+" + dataGridViewDJMX.Rows[i].Cells[8].Value.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[19].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();
                }



                //���
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {
                    if (dataGridViewDJMX.Rows[i].IsNewRow)
                        continue;

                    //�ܿ����
                    dKUL1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[8].Value);
                    dKCJE1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[12].Value);
                    dKCCBJ1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[9].Value);
                    dYSYE1 = -dKCJE1;

                    //�ܿ����
                    sqlComm.CommandText = "SELECT �������, ���ɱ���,�����, Ӧ����� FROM ��Ʒ�� WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                    }
                    sqldr.Close();

                    //���
                    dYSYE += dYSYE1;

                    dKUL -= dKUL1;
                    //dKCJE -= dKCJE1;
                    dKCJE = dKUL * dKCCBJ;

                    sqlComm.CommandText = "UPDATE ��Ʒ�� SET ������� = " + dKUL.ToString() + ", ���ɱ��� = " + dKCCBJ.ToString() + ", �����=" + dKCJE.ToString() + ", Ӧ�����=" + dYSYE.ToString() + " WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                    //������ʷ��¼
                    sqlComm.CommandText = "INSERT INTO ��Ʒ��ʷ�˱� (����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, �������, ��ⵥ��, �����, �ܽ������, �ܽ����, Ӧ�����, BeActive, ����ID) VALUES ('" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'���������������', -" + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", -" + dKCJE1 + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1,"+sBMID+")";
                    sqlComm.ExecuteNonQuery();



                    //�ֿ�����
                    sqlComm.CommandText = "SELECT �������, �����, ���ɱ���, ����� ,Ӧ����� FROM ���� WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ") AND (BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();
                    while (sqldr.Read())
                    {
                        dKUL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                        dKCCBJ = Convert.ToDecimal(sqldr.GetValue(2).ToString());
                        dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
                        dYSYE = Convert.ToDecimal(sqldr.GetValue(3).ToString());
                    }
                    sqldr.Close();
                    dKUL -= dKUL1;
                    //dKCJE -= dKCJE1;
                    dKCJE = dKUL * dKCCBJ;

                    //���
                    dYSYE += dYSYE1;

                    sqlComm.CommandText = "UPDATE ���� SET ������� = " + dKUL.ToString() + ", ���ɱ��� = " + dKCCBJ.ToString() + ",�����=" + dKCJE.ToString() + ", Ӧ�����=" + dYSYE.ToString() + " WHERE (�ⷿID = " + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ") AND (��ƷID = " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ") AND (BeActive = 1)";
                    sqlComm.ExecuteNonQuery();

                    //�ⷿ����ʷ��¼
                    sqlComm.CommandText = "INSERT INTO ��Ʒ�ⷿ��ʷ�˱� (�ⷿID, ����, ��ƷID, ��λID, ҵ��ԱID, ���ݱ��, ժҪ, �������, ��ⵥ��, �����, �ⷿ�������, �ⷿ�����, Ӧ�����, BeActive, ����ID) VALUES (" + dataGridViewDJMX.Rows[i].Cells[15].Value.ToString() + ",'" + strDateSYS + "', " + dataGridViewDJMX.Rows[i].Cells[14].Value.ToString() + ", " + iSupplyCompany.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "��', N'���������������', -" + dKUL1.ToString() + ", " + dZZJJ1.ToString() + ", -" + dKCJE1.ToString() + ", " + dKUL.ToString() + ", " + dKCJE.ToString() + ", " + dYSYE.ToString() + ", 1,"+sBMID+")";
                    sqlComm.ExecuteNonQuery();

                }


                //����
                sqlComm.CommandText = "DELETE FROM ��Ʒ����� WHERE (���ݱ�� = N'" + labelDJBH.Text + "')";
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

            checkRKView();

            //MessageBox.Show("���������������ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isSaved = true;

            if (MessageBox.Show("���������������ɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }

        }

        //���������
        private void checkRKView()
        {
            int i;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                for (i = 0; i < dataGridViewDJMX.Rows.Count; i++)
                {

                    //������־
                    if (dataGridViewDJMX.Rows[i].Cells[0].Value.ToString() == "")
                    {
                        dataGridViewDJMX.Rows[i].Cells[0].Value = 0;
                    }

                    sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���ϸ��.ID FROM ������Ʒ�Ƶ���ϸ�� INNER JOIN ������Ʒ�Ƶ��� ON ������Ʒ�Ƶ���ϸ��.��ID = ������Ʒ�Ƶ���.ID WHERE (������Ʒ�Ƶ���ϸ��.δ�������� <> 0) AND (������Ʒ�Ƶ���ϸ��.��ID = " + dataGridViewDJMX.Rows[i].Cells[20].Value.ToString() + ") AND (������Ʒ�Ƶ���.BeActive = 1)";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows) //����δ������ϸ
                    {
                        sqldr.Close();
                        sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET ����� = 0 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[20].Value.ToString() + ")";
                        sqlComm.ExecuteNonQuery();
                    }
                    else
                    {
                        sqldr.Close();
                        sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET ����� = 1 WHERE (ID = " + dataGridViewDJMX.Rows[i].Cells[20].Value.ToString() + ")";
                        sqlComm.ExecuteNonQuery();
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
        }

        private void FormJHRKYHD_EDIT_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
                return;

            DialogResult dr = MessageBox.Show(this, "�����޸���δ���棬ȷ��Ҫ�˳���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
                    
            string strT = "������������(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelSJJE.Text + "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {

            string strT = "������������(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelSJJE.Text + "(��д:" + labelDX.Text + ")";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

    }
}