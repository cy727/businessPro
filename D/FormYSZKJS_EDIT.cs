using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormYSZKJS_EDIT : Form
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

        public int intDJID = 0;

        private int iSupplyCompany = 0;
        private int intHTH = 0;
        private decimal dDJSUM = 0;
        private bool isSaved = false;
        private int iYWY = 0;
        private int iBM = 0;

        private ClassGetInformation cGetInformation;


        public FormYSZKJS_EDIT()
        {
            InitializeComponent();
        }

        private void FormYSZKJS_EDIT_Load(object sender, EventArgs e)
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
            sqlComm.CommandText = "SELECT �����տ���ܱ�.���ݱ��, �����տ���ܱ�.����, ְԱ��.ְԱ����,[ְԱ��_1].ְԱ����, ��λ��.��λ���, ��λ��.��λ����, �����տ���ܱ�.��Ʊ��, �����տ���ܱ�.��Ʊ����, �����տ���ܱ�.��ע, �����տ���ܱ�.ʵ�ƽ��, �����տ���ܱ�.˰��, �����տ���ܱ�.��λID,�����տ���ܱ�.ҵ��ԱID,�����տ���ܱ�.����ID FROM �����տ���ܱ� INNER JOIN ְԱ�� ON �����տ���ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON �����տ���ܱ�.ҵ��ԱID = [ְԱ��_1].ID INNER JOIN ��λ�� ON �����տ���ܱ�.��λID = ��λ��.ID WHERE (�����տ���ܱ�.ID =  " + intDJID.ToString() + ") AND (�����տ���ܱ�.BeActive<>0)";
            sqldr = sqlComm.ExecuteReader();

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
                textBoxSH.Text = sqldr.GetValue(10).ToString();

                textBoxBZ.Text = sqldr.GetValue(8).ToString();
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

            sqlComm.CommandText = "SELECT �����տ���ϸ��.ID, �˲���.�˲����, �˲���.�˲�����, �����տ���ϸ��.ժҪ,�����տ���ϸ��.��Ӧ����, �˲���.����, �����տ���ϸ��.������, �����տ���ϸ��.֧Ʊ��, �����տ���ϸ��.��ע, �˲���.�˲�ID, �����տ���.���ұ��, �����տ���.���Ҽ�¼, �����տ���ϸ��.����ID FROM �˲��� INNER JOIN �����տ���ϸ�� ON �˲���.ID = �����տ���ϸ��.�˲�ID CROSS JOIN �����տ��� WHERE (�����տ���ϸ��.����ID = " + intDJID.ToString() + ")";

            if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
            sqlDA.Fill(dSet, "���ݱ�");
            dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];

            sqlConn.Close();


            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[9].Visible = false;
            dataGridViewDJMX.Columns[10].Visible = false;
            dataGridViewDJMX.Columns[11].Visible = false;
            dataGridViewDJMX.Columns[12].Visible = false;
            dataGridViewDJMX.Columns[6].ReadOnly = true;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.ShowCellErrors = true;
            dataGridViewDJMX.ReadOnly = true;
            dataGridViewDJMX.AllowUserToAddRows = false;
            dataGridViewDJMX.AllowUserToDeleteRows = false;


            countAmount();

        }
        //return true ��ȷ  false ����
        private bool countAmount()
        {
            decimal fSum = 0, fSum1 = 0;
            decimal fTemp, fTemp1;
            decimal fCount = 0, fCSum = 0;

            //this.dataGridViewDJMX.CellValidating -= dataGridViewDJMX_CellValidating;
            bool bCheck = true;


            for (int i = 0; i < dataGridViewDJMX.Rows.Count; i++)
            {
                if (dataGridViewDJMX.Rows[i].IsNewRow)
                    continue;

                //�ⷿID
                if (dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "" || dataGridViewDJMX.Rows[i].Cells[9].Value.ToString() == "0")
                {
                    bCheck = false;
                    dataGridViewDJMX.Rows[i].Cells[1].ErrorText = "�������˲����";
                    dataGridViewDJMX.Rows[i].Cells[2].ErrorText = "�������˲�������";
                    continue;
                }


                //��Ӧ�տ�
                if (dataGridViewDJMX.Rows[i].Cells[4].Value.ToString() == "")
                    dataGridViewDJMX.Rows[i].Cells[4].Value = 0;

                fTemp = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);


                //����
                if (dataGridViewDJMX.Rows[i].Cells[5].Value.ToString() == "")
                {
                    dataGridViewDJMX.Rows[i].Cells[5].Value = 100;
                }


                //ʵ�ƽ��
                fTemp1 = Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[5].Value);
                dataGridViewDJMX.Rows[i].Cells[6].Value = fTemp * fTemp1 / 100;


                fSum += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[4].Value);
                fSum1 += Convert.ToDecimal(dataGridViewDJMX.Rows[i].Cells[6].Value);

                fCount += 1;

            }
            //this.dataGridViewDJMX.CellValidating += dataGridViewDJMX_CellValidating;
            dataGridViewDJMX.EndEdit();

            labelJEHJ.Text = fSum.ToString();
            labelSJJE.Text = fSum1.ToString();

            labelDX.Text = cGetInformation.changeDAXIE(labelSJJE.Text);
            toolStripStatusLabelMXJLS.Text = fCount.ToString();

            return bCheck;


        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            //�������
            if (isSaved)
            {
                MessageBox.Show("Ӧ���˿���㵥�Ѿ����,���ݺ�Ϊ��" + labelDJBH.Text, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int i,j,k;
            string sTemp = "";
            decimal dKUL = 0, dKCCBJ = 0, dZGJJ = 0, dZDJJ = 0, dZZJJ = 0;
            decimal dKUL1 = 0, dKCCBJ1 = 0, dZGJJ1 = 0, dZDJJ1 = 0, dZZJJ1 = 0;
            decimal dKCJE = 0, dKCJE1 = 0, dYSYE = 0, dYSYE1 = 0;
            string strCommID, strKFID;

            string sBMID = "NULL";
            if (iBM != 0)
                sBMID = iBM.ToString();

            System.Data.SqlClient.SqlTransaction sqlta;

            string strDateSYS;
            cGetInformation.getSystemDateTime();
            strDateSYS = cGetInformation.strSYSDATATIME;
            
            sqlConn.Open();
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
            sqlComm.CommandText = "SELECT ���� from �����տ���ܱ� WHERE (ID = " + intDJID.ToString() + ")";
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
                sqlComm.CommandText = "UPDATE �����տ���ܱ� SET BeActive = 0 WHERE (ID = " + intDJID.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                sqlComm.CommandText = "UPDATE �����տ���ܱ� SET ���ʱ�� = '" + strDateSYS + "' WHERE (ID = " + intDJID.ToString() + ")";
                sqlComm.ExecuteNonQuery();



                //��λӦ����
                sqlComm.CommandText = "SELECT Ӧ���˿� FROM ��λ�� WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();

                dKCJE = 0;
                while (sqldr.Read())
                {
                    dKCJE = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                }
                sqldr.Close();
                dKCJE1 = dKCJE + Convert.ToDecimal(labelSJJE.Text);
                sqlComm.CommandText = "UPDATE ��λ�� SET Ӧ���˿� = " + dKCJE1.ToString() + " WHERE (ID = " + iSupplyCompany.ToString() + ")";
                sqlComm.ExecuteNonQuery();

                //��λ��ʷ��¼
                sqlComm.CommandText = "INSERT INTO ��λ��ʷ�˱� (��λID, ����, ���ݱ��, ժҪ, Ӧ�ս��, ҵ��ԱID, ��ֵ���, BeActive, ����ID) VALUES (" + iSupplyCompany.ToString() + ", '" + strDateSYS + "', N'" + labelDJBH.Text + "��', N'Ӧ���˿���㵥���', " + dKCJE1.ToString() + ", " + iYWY.ToString() + ", N'" + labelDJBH.Text + "', 1,"+sBMID+")";
                sqlComm.ExecuteNonQuery();

                //��ϸ����
                for (k = 0; k < dataGridViewDJMX.RowCount; k++)
                {
                    sqlComm.CommandText = "SELECT ID, ���ҷ�ʽ, ����ID, ���ݱ��, �Ѹ���, BeActive FROM �����տ�ұ� WHERE (���ҷ�ʽ = 1) AND (BeActive = 1) AND (����ID = " + dataGridViewDJMX.Rows[k].Cells[0].Value.ToString() + ")";

                    if (dSet.Tables.Contains("���ұ�")) dSet.Tables.Remove("���ұ�");
                    sqlDA.Fill(dSet, "���ұ�");

                    for (j = 0; j < dSet.Tables["���ұ�"].Rows.Count; j++)
                    {
                        //���˵���
                        sTemp = dSet.Tables["���ұ�"].Rows[j][3].ToString().Substring(0, 3);
                        strCommID = "0";
                        strKFID = "0";
                        switch (sTemp)
                        {
                            case "BKP":
                                sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ���ϸ�� SET δ������ = δ������ + " + dSet.Tables["���ұ�"].Rows[j][4].ToString() + ", �Ѹ����� = �Ѹ����� - " + dSet.Tables["���ұ�"].Rows[j][4].ToString() + " WHERE (ID = " + dSet.Tables["���ұ�"].Rows[j][2].ToString() + ")";
                                sqlComm.ExecuteNonQuery();

                                sqlComm.CommandText = "UPDATE ������Ʒ�Ƶ��� SET δ������ = δ������ + " + dSet.Tables["���ұ�"].Rows[j][4].ToString() + ", �Ѹ����� =  �Ѹ����� - " + dSet.Tables["���ұ�"].Rows[j][4].ToString() + " WHERE (���ݱ�� = N'" + dSet.Tables["���ұ�"].Rows[j][3].ToString() + "')";
                                sqlComm.ExecuteNonQuery();

                                sqlComm.CommandText = "SELECT ��ƷID, �ⷿID FROM ������Ʒ�Ƶ���ϸ�� WHERE (ID = " + dSet.Tables["���ұ�"].Rows[j][2].ToString() + ")";
                                sqldr = sqlComm.ExecuteReader();
                                while (sqldr.Read())
                                {
                                    strCommID = sqldr.GetValue(0).ToString();
                                    strKFID = sqldr.GetValue(1).ToString();
                                    break;
                                }
                                sqldr.Close();

                                break;

                            case "BTH":
                                sqlComm.CommandText = "UPDATE �����˳���ϸ�� SET δ������ = δ������ - (" + dSet.Tables["���ұ�"].Rows[j][4].ToString() + "), �Ѹ����� = �Ѹ����� - (-1*" + dSet.Tables["���ұ�"].Rows[j][4].ToString() + ") WHERE (ID = " + dSet.Tables["���ұ�"].Rows[j][2].ToString() + ")";
                                sqlComm.ExecuteNonQuery();

                                sqlComm.CommandText = "UPDATE �����˳����ܱ� SET δ������ = δ������ - (" + dSet.Tables["���ұ�"].Rows[j][4].ToString() + "), �Ѹ����� =  �Ѹ����� - (-1*" + dSet.Tables["���ұ�"].Rows[j][4].ToString() + ") WHERE (���ݱ�� = N'" + dSet.Tables["���ұ�"].Rows[j][3].ToString() + "')";
                                sqlComm.ExecuteNonQuery();

                                sqlComm.CommandText = "SELECT ��ƷID, �ⷿID FROM �����˳���ϸ�� WHERE (ID = " + dSet.Tables["���ұ�"].Rows[j][2].ToString() + ")";
                                sqldr = sqlComm.ExecuteReader();
                                while (sqldr.Read())
                                {
                                    strCommID = sqldr.GetValue(0).ToString();
                                    strKFID = sqldr.GetValue(1).ToString();
                                    break;
                                }
                                sqldr.Close();

                               break;

                            case "BTB":
                               sqlComm.CommandText = "UPDATE �����˲������ϸ�� SET δ������ = δ������ + " + dSet.Tables["���ұ�"].Rows[j][4].ToString() + ", �Ѹ����� = �Ѹ����� - " + dSet.Tables["���ұ�"].Rows[j][4].ToString() + " WHERE (ID = " + dSet.Tables["���ұ�"].Rows[j][2].ToString() + ")";
                               sqlComm.ExecuteNonQuery();

                               sqlComm.CommandText = "UPDATE �����˲���ۻ��ܱ� SET δ������ = δ������ + " + dSet.Tables["���ұ�"].Rows[j][4].ToString() + ", �Ѹ����� =  �Ѹ����� - " + dSet.Tables["���ұ�"].Rows[j][4].ToString() + " WHERE (���ݱ�� = N'" + dSet.Tables["���ұ�"].Rows[j][3].ToString() + "')";
                                sqlComm.ExecuteNonQuery();


                               sqlComm.CommandText = "SELECT ��ƷID, �ⷿID FROM �����˲������ϸ�� WHERE (ID = " + dSet.Tables["���ұ�"].Rows[j][2].ToString() + ")";
                               sqldr = sqlComm.ExecuteReader();
                               while (sqldr.Read())
                               {
                                   strCommID = sqldr.GetValue(0).ToString();
                                   strKFID = sqldr.GetValue(1).ToString();
                                   break;
                               }
                               sqldr.Close();

                                break;

                        }
                        //
                        sqlComm.CommandText = "UPDATE �����տ�ұ� SET BeActive = 0 WHERE (ID = " + dSet.Tables["���ұ�"].Rows[j][0].ToString() + ")";
                        sqlComm.ExecuteNonQuery();

                        //�ܿ��
                        sqlComm.CommandText = "UPDATE ��Ʒ�� SET Ӧ�ս�� = Ӧ�ս�� +" + dSet.Tables["���ұ�"].Rows[j][4].ToString() + ", ���ս�� = ���ս�� -" + dSet.Tables["���ұ�"].Rows[j][4].ToString() + " WHERE (ID = " + strCommID + ")";
                        sqlComm.ExecuteNonQuery();

                        //�ֿ��
                        sqlComm.CommandText = "UPDATE ���� SET  Ӧ�ս�� = Ӧ�ս�� +" + dSet.Tables["���ұ�"].Rows[j][4].ToString() + ", ���ս�� = ���ս�� - " + dSet.Tables["���ұ�"].Rows[j][4].ToString() + " WHERE (�ⷿID = " + strKFID + ") AND (��ƷID = " + strCommID + ") AND (BeActive = 1)";
                        sqlComm.ExecuteNonQuery();


                      //
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

            //MessageBox.Show("Ӧ���˿���㵥���ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isSaved = true;
            if (MessageBox.Show("Ӧ���˿���㵥���ɹ����Ƿ�رյ��ݴ��ڣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }

        }

        private void FormYSZKJS_EDIT_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
            {
                return;
            }

            DialogResult dr = MessageBox.Show(this, "������δ���棬ȷ��Ҫ�˳���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "Ӧ���˿���㵥(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelJEHJ.Text + "(��д:" + labelDX.Text + ");����Ʊ�ţ�" + textBoxFPH.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "Ӧ���˿���㵥(���ݱ��:" + labelDJBH.Text + ");�Ƶ����ڣ�" + labelZDRQ.Text + ";ҵ����Ա��" + comboBoxYWY.Text + ";��λ���ƣ�" + textBoxDWMC.Text + ";��˰�ϼƣ�" + labelJEHJ.Text + "(��д:" + labelDX.Text + ");����Ʊ�ţ�" + textBoxFPH.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }

        private string getCompanyPay(int icompanyID)
        {
            string strPay = "0.00";

            sqlConn.Open();
            sqlComm.CommandText = "SELECT SUM(δ������) FROM �տ���ϸ��ͼ WHERE (��λID = " + icompanyID.ToString() + ")";

            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                try
                {
                    strPay = decimal.Parse(sqldr.GetValue(0).ToString()).ToString("f2");
                }
                catch
                {
                }
            }


            sqlConn.Close();

            return strPay;

        }
    }
}