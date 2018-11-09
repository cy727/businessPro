using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormDWDAWH_CARD : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";

        private ClassGetInformation cGetInformation;

        public int iStyle = 0;
        public DataTable dt;
        public int iSelect = 0;

        public FormDWDAWH_CARD()
        {
            InitializeComponent();
        }

        private void FormDWDAWH_CARD_Load(object sender, EventArgs e)
        {

            if (dt.Rows.Count < 1 && iStyle==1)
            {
                this.Close();
                return;
            }


            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            switch (iStyle)
            {
                case 0://����
                    btnAccept.Text = "����";
                    break;
                case 1://�޸�
                    btnAccept.Text = "�޸�";
                    break;
                default:
                    break;
            }

            sqlConn.Open();
            //����
            sqlComm.CommandText = "SELECT DISTINCT �������� FROM ��λ�� WHERE (�������� <> N'') AND (�������� IS NOT NULL) ORDER BY ��������";
            if (dSet.Tables.Contains("��������")) dSet.Tables.Remove("��������");
            sqlDA.Fill(dSet, "��������");

            comboBoxKHYH.DataSource = dSet.Tables["��������"];
            comboBoxKHYH.DisplayMember = "��������";
            comboBoxKHYH.Text = "";


            //��ʼ������
            comboBoxDQMC.SelectedIndexChanged -= comboBoxDQMC_SelectedIndexChanged;
            sqlComm.CommandText = "SELECT ����, ��� FROM ������ ORDER BY ����";
            if (dSet.Tables.Contains("��������")) dSet.Tables.Remove("��������");
            sqlDA.Fill(dSet, "��������");
            comboBoxDQMC.DataSource = dSet.Tables["��������"];
            comboBoxDQMC.DisplayMember = "����";
            comboBoxDQMC.ValueMember = "���";
            comboBoxDQMC.SelectedIndexChanged += comboBoxDQMC_SelectedIndexChanged;


            //��ҵ
            sqlComm.CommandText = "SELECT DISTINCT ��ҵ���� FROM ��λ�� WHERE (��ҵ���� <> N'') AND (��ҵ���� IS NOT NULL) ORDER BY ��ҵ����";
            if (dSet.Tables.Contains("��ҵ����")) dSet.Tables.Remove("��ҵ����");
            sqlDA.Fill(dSet, "��ҵ����");

            comboBoxHYMC.DataSource = dSet.Tables["��ҵ����"];
            comboBoxHYMC.DisplayMember = "��ҵ����";
            comboBoxHYMC.Text = "";

            //ҵ��Ա
            sqlComm.CommandText = "SELECT ID, ְԱ���, ְԱ���� FROM ְԱ�� WHERE (beactive = 1)";

            if (dSet.Tables.Contains("ְԱ��")) dSet.Tables.Remove("ְԱ��");
            sqlDA.Fill(dSet, "ְԱ��");
            comboBoxYWY.DataSource = dSet.Tables["ְԱ��"];
            comboBoxYWY.DisplayMember = "ְԱ����";
            comboBoxYWY.ValueMember = "ID";


            //����
            sqlComm.CommandText = "SELECT ID, ���ű��, �������� FROM ���ű� WHERE (BeActive = 1)";

            if (dSet.Tables.Contains("���ű�")) dSet.Tables.Remove("���ű�");
            sqlDA.Fill(dSet, "���ű�");
            comboBoxBM.DataSource = dSet.Tables["���ű�"];
            comboBoxBM.DisplayMember = "��������";
            comboBoxBM.ValueMember = "ID";

            //��վ
            sqlComm.CommandText = "SELECT ���� FROM ������ ORDER BY ����";
            if (dSet.Tables.Contains("��վ����")) dSet.Tables.Remove("��վ����");
            sqlDA.Fill(dSet, "��վ����");
            comboBoxDZMC.DataSource = dSet.Tables["��վ����"];
            comboBoxDZMC.DisplayMember = "����";


            sqlConn.Close();

            if (iStyle == 1) //�޸�
            {
                //ID, ��λ���, ��λ����, ������, �Ƿ����, �Ƿ�����, ˰��, �绰, ��������, �����˺�, ��ϵ��, ��ַ, ��������, ��ҵ����, ����, �ʱ�, ��ע, ��¼����, ��ϵ��ַ, �ջ���, ҵ��Ա, ��վ����
                textBoxDWBH.Text = dt.Rows[0][1].ToString();
                textBoxDWMC.Text = dt.Rows[0][2].ToString();
                textBoxZJM.Text = dt.Rows[0][3].ToString();
                if (dt.Rows[0][4].ToString() == "0")
                    checkBoxSFJH.Checked = false;
                else
                    checkBoxSFJH.Checked = true;

                if (dt.Rows[0][5].ToString() == "0")
                    checkBoxSFXS.Checked = false;
                else
                    checkBoxSFXS.Checked = true;

                textBoxSH.Text = dt.Rows[0][6].ToString();
                textBoxDH.Text = dt.Rows[0][7].ToString();
                comboBoxKHYH.Text = dt.Rows[0][8].ToString();
                textBoxYHZH.Text = dt.Rows[0][9].ToString();
                textBoxLXR.Text = dt.Rows[0][10].ToString();
                textBoxDZ.Text = dt.Rows[0][11].ToString();
                comboBoxDQMC.Text = dt.Rows[0][12].ToString();
                comboBoxHYMC.Text = dt.Rows[0][13].ToString();
                textBoxCZ.Text = dt.Rows[0][14].ToString();
                textBoxYB.Text = dt.Rows[0][15].ToString();
                textBoxBZ.Text = dt.Rows[0][16].ToString();
                textBoxKPDH.Text = dt.Rows[0][23].ToString();
                textBoxSHDH.Text = dt.Rows[0][24].ToString();
                try
                {
                    dateTimePickerDLRQ.Value = DateTime.Parse(dt.Rows[0][17].ToString());
                }
                catch
                {
                    dateTimePickerDLRQ.Value = DateTime.Now;
                }
                textBoxLXDZ.Text = dt.Rows[0][18].ToString();
                textBoxSHR.Text = dt.Rows[0][19].ToString();
                comboBoxYWY.Text = dt.Rows[0][20].ToString().Trim();
                comboBoxDZMC.Text = dt.Rows[0][21].ToString();

                if (dt.Rows[0][22].ToString() != "")
                {
                    comboBoxBM.SelectedValue = int.Parse(dt.Rows[0][22].ToString());
                }

            }
            



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iSelect = 0;
            this.Close();
        }


        private bool countAmount()
        {
            bool bCheck = true;

            if (textBoxDWBH.ToString() == "")
            {
                MessageBox.Show("�������ʹ���,�����뵥λ���", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bCheck = false;
                return bCheck;
            }

            if (textBoxDWMC.ToString() == "")
            {
                MessageBox.Show("�������ʹ���,�����뵥λ����", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bCheck = false;
            }
            return bCheck;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            int i1 = 0, i2 = 0;
            string strDateSYS = "";
            System.Data.SqlClient.SqlTransaction sqlta;

            if (!countAmount())
            {
               return;
            }
            

            switch (iStyle)
            {
                case 0://����
                    sqlConn.Open();

                    //����
                    if (textBoxDWBH.Text.Trim() == "")
                    {
                        MessageBox.Show("�����뵥λ���");
                        sqlConn.Close();
                        break;
                    }
                    sqlComm.CommandText = "SELECT ID, ��λ���� FROM ��λ�� WHERE (��λ��� = '" + textBoxDWBH.Text.Trim() + "')";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        MessageBox.Show("��λ���" + textBoxDWBH.Text.Trim() + "�ظ�������Ϊ��"+sqldr.GetValue(1).ToString());
                        sqldr.Close();
                        sqlConn.Close();
                        break;
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT ID, ��λ��� FROM ��λ�� WHERE (��λ���� = '" + textBoxDWMC.Text.Trim() + "')";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        if (MessageBox.Show("��λ����" + textBoxDWMC.Text.Trim() + "�ظ������Ϊ��" + sqldr.GetValue(1).ToString() + "���Ƿ������", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        {
                            sqldr.Close();
                            sqlConn.Close();
                            break;
                        }
                    }
                    sqldr.Close();



                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {

                        //�õ�����
                        //�õ�����������
                        sqlComm.CommandText = "SELECT GETDATE() AS ����";
                        sqldr = sqlComm.ExecuteReader();

                        while (sqldr.Read())
                        {
                            strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                        }
                        sqldr.Close();

                        if (checkBoxSFJH.Checked)
                            i1 = 1;
                        else
                            i1 = 0;
                       

                        if (checkBoxSFXS.Checked)
                            i2 = 1;
                        else
                            i2 = 0;

                        sqlComm.CommandText = "INSERT INTO ��λ�� (��λ���, ��λ����, ������, �Ƿ����, �Ƿ�����, ˰��, �绰, ��������, �����˺�, ��ϵ��, ��ַ, ��������, ��ҵ����, ����, �ʱ�, ��ע, ��¼����, ��ϵ��ַ, Ӧ���˿�, Ӧ���˿�, BeActive, �ջ���, ҵ��Ա, ��վ����, ����ID,��Ʊ�绰,�ջ��绰) VALUES ('" + textBoxDWBH.Text.Trim() + "', N'" + textBoxDWMC.Text.Trim() + "', '" + textBoxZJM.Text.Trim() + "', " + i1.ToString() + ", " + i2.ToString() + ", N'" + textBoxSH.Text.Trim() + "', '" + textBoxDH.Text.Trim() + "', N'" + comboBoxKHYH.Text.Trim() + "', '" + textBoxYHZH.Text.Trim() + "', N'" + textBoxLXR.Text.Trim() + "', N'" + textBoxDZ.Text.Trim() + "', N'" + comboBoxDQMC.Text.Trim() + "', N'" + comboBoxHYMC.Text.Trim() + "', N'" + textBoxCZ.Text.Trim() + "', '" + textBoxYB.Text.Trim() + "', N'" + textBoxBZ.Text.Trim() + "', '" + strDateSYS + "', N'" + textBoxLXDZ.Text.Trim() + "', 0, 0, 1, N'" + textBoxSHR.Text.Trim() + "',N'" + comboBoxYWY.Text + "',N'" + comboBoxDZMC.Text.Trim() + "', " + comboBoxBM.SelectedValue.ToString() + ",N'" + textBoxKPDH.Text.Trim() + "',N'" + textBoxSHDH.Text.Trim() + "')";
                        sqlComm.ExecuteNonQuery();

                        sqlComm.CommandText = "SELECT @@IDENTITY";
                        sqldr = sqlComm.ExecuteReader();
                        sqldr.Read();
                        iSelect = Convert.ToInt32(sqldr.GetValue(0).ToString());
                        sqldr.Close();


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
                    MessageBox.Show("���ӳɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    break;
                case 1://�޸�

                    sqlConn.Open();
                    //����
                    if (textBoxDWBH.Text.Trim() == "")
                    {
                        MessageBox.Show("�����뵥λ���");
                        sqlConn.Close();
                        break;
                    }
                    iSelect = Convert.ToInt32(dt.Rows[0][0].ToString());
                    sqlComm.CommandText = "SELECT ID, ��λ���� FROM ��λ�� WHERE (��λ��� = '" + textBoxDWBH.Text.Trim() + "' AND ID <> "+iSelect.ToString()+")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        MessageBox.Show("��λ���" + textBoxDWBH.Text.Trim() + "�ظ�������Ϊ��" + sqldr.GetValue(1).ToString());
                        sqldr.Close();
                        sqlConn.Close();
                        break;
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT ID, ��λ��� FROM ��λ�� WHERE (��λ���� = '" + textBoxDWMC.Text.Trim() + "' AND ID <> "+iSelect.ToString()+")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        if (MessageBox.Show("��λ����" + textBoxDWMC.Text.Trim() + "�ظ������Ϊ��" + sqldr.GetValue(1).ToString() + "���Ƿ������", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        {
                            sqldr.Close();
                            sqlConn.Close();
                            break;
                        }
                    }
                    sqldr.Close();


                    //ʹ��״̬
                    sqlComm.CommandText = "SELECT ��λ��.��λ����, ���ݻ�����ͼ.���ݱ�� FROM ��λ�� INNER JOIN ���ݻ�����ͼ ON ��λ��.ID = ���ݻ�����ͼ.��λID WHERE (���ݻ�����ͼ.BeActive = 1) AND (��λ��.ID = " + iSelect.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        if(textBoxDWMC.Text.Trim() != sqldr.GetValue(0).ToString())
                            MessageBox.Show("�õ�λ���е��ݱ��棬���ɸ��ĵ�λ���ƣ�" + sqldr.GetValue(0).ToString() + "�����ݱ�ţ�ʾ������" + sqldr.GetValue(1).ToString(), "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBoxDWMC.Text = sqldr.GetValue(0).ToString();
                    }
                    sqldr.Close();


                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {

                        //�õ�����
                        //�õ�����������
                        sqlComm.CommandText = "SELECT GETDATE() AS ����";
                        sqldr = sqlComm.ExecuteReader();

                        while (sqldr.Read())
                        {
                            strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                        }
                        sqldr.Close();

                        if (checkBoxSFJH.Checked)
                            i1 = 1;
                        else
                            i1 = 0;


                        if (checkBoxSFXS.Checked)
                            i2 = 1;
                        else
                            i2 = 0;

                        iSelect = Convert.ToInt32(dt.Rows[0][0].ToString());
                        sqlComm.CommandText = "UPDATE ��λ�� SET ��λ��� = '" + textBoxDWBH.Text.Trim() + "', ��λ���� = N'" + textBoxDWMC.Text.Trim() + "', ������ = '" + textBoxZJM.Text.Trim() + "', �Ƿ���� = " + i1.ToString() + ", �Ƿ����� = " + i2.ToString() + ", ˰�� = N'" + textBoxSH.Text.Trim() + "', �绰 = '" + textBoxDH.Text.Trim() + "', �������� = N'" + comboBoxKHYH.Text.Trim() + "', �����˺� = '" + textBoxYHZH.Text.Trim() + "', ��ϵ�� = N'" + textBoxLXR.Text.Trim() + "', ��ַ = N'" + textBoxDZ.Text.Trim() + "', �������� = N'" + comboBoxDQMC.Text.Trim() + "', ��ҵ���� = N'" + comboBoxHYMC.Text.Trim() + "', ���� = N'" + textBoxCZ.Text.Trim() + "', �ʱ� = '" + textBoxYB.Text.Trim() + "', ��ע = N'" + textBoxBZ.Text.Trim() + "', ��¼���� = '" + dateTimePickerDLRQ.Value.ToShortDateString() + "', ��ϵ��ַ = N'" + textBoxLXDZ.Text.Trim() + "',�ջ���=N'" + textBoxSHR.Text.Trim() + "', ҵ��Ա=N'" + comboBoxYWY.Text.Trim() + "', ��վ����=N'" + comboBoxDZMC.Text.Trim() + "', ����ID = " + comboBoxBM.SelectedValue.ToString() + ", ��Ʊ�绰=N'" + textBoxKPDH.Text.Trim() + "', �ջ��绰=N'" + textBoxSHDH.Text.Trim() + "' WHERE (ID = " + iSelect + ")";
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
                    MessageBox.Show("�޸ĳɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    break;
                default:
                    break;
            }
        }


        private void textBoxDWMC_TextChanged(object sender, EventArgs e)
        {
            textBoxZJM.Text = cGetInformation.convertPYSM(textBoxDWMC.Text);
        }

        private void comboBoxDQMC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDQMC.SelectedValue.ToString() == "")
                return;

            if (iStyle == 1) //�޸�ģʽ
                return;

            int iMaxDWNo = 1;
            string sT="";

            sqlConn.Open();

            //�õ����ձ���
            sqlComm.CommandText = "SELECT MAX(��λ���) FROM ��λ�� WHERE (��λ��� LIKE '"+comboBoxDQMC.SelectedValue.ToString().Trim()+"%')";
            sqldr = sqlComm.ExecuteReader();

            if (sqldr.HasRows)
            {
                sqldr.Read();
                sT = sqldr.GetValue(0).ToString().Trim();

                if (sT.Length < 4)
                    sT = "";
                else
                    sT = sT.Substring(4,sT.Length-4);
                try
                {
                    iMaxDWNo = Convert.ToInt32(sT);
                    iMaxDWNo++;
                }
                catch
                {
                    iMaxDWNo = 1;
                }

            }
            sqldr.Close();

            textBoxDWBH.Text = comboBoxDQMC.SelectedValue.ToString().Trim() + string.Format("{0:D4}", iMaxDWNo);


            sqlConn.Close();
        }

        private void textBoxDWBH_Validating(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex rExpression = new System.Text.RegularExpressions.Regex(@"^\d{8}$");

            textBoxDWBH.Text = textBoxDWBH.Text.Trim();
            if (rExpression.IsMatch(textBoxDWBH.Text) || textBoxDWBH.Text == "")
            {
                this.errorProviderM.Clear();
            }
            else
            {
                this.errorProviderM.SetError(this.textBoxDWBH, "������ȷ�ĵ�λ���룬��λ���� ���磺01000001");
                e.Cancel = true;
            }
        }

        private void comboBoxDQMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i;
            bool bSelect=false;

            if (e.KeyChar == (char)Keys.Return)
            {
                for (i = 0; i < dSet.Tables["��������"].Rows.Count; i++)
                {
                    if (dSet.Tables["��������"].Rows[i][0].ToString() == comboBoxDQMC.Text)
                    {
                        comboBoxDQMC.SelectedIndex = i;
                        bSelect = true;
                        break;
                    }
                }
                if (comboBoxDQMC.SelectedIndex < 0)
                    comboBoxDQMC.SelectedIndex = 0;
                if (!bSelect)
                {
                    comboBoxDQMC.Text = dSet.Tables["��������"].Rows[comboBoxDQMC.SelectedIndex][0].ToString();
                }
                comboBoxDQMC_SelectedIndexChanged(null,null);

            }
        }



   }
}