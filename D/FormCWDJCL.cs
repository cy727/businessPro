using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormCWDJCL : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";
        public string strSelect = "";
        //0,��� 1,�޸�
        public int iStyle = 0;

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private int intDJID = 0;
        private int iSupplyCompany = 0;


        private ClassGetInformation cGetInformation;

        private bool isSaved = false;

        private int iConstLimit = 18; 
        
        public FormCWDJCL()
        {
            InitializeComponent();
        }

        private void FormCWDJCL_Load(object sender, EventArgs e)
        {

            this.Top = 1;
            this.Left = 1;

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            //�õ���ʼʱ��
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��˾����, ����Ŀ��1, ����Ŀ��2, ����Ŀ��3, ����Ŀ��4, ����ԱȨ��, �ܾ���Ȩ��, ְԱȨ��, ����Ȩ��, ҵ��ԱȨ�� FROM ϵͳ������";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {

                try
                {
                    iConstLimit = int.Parse(sqldr.GetValue(6).ToString());
                }
                catch
                {
                    iConstLimit = 18;
                }
            }
            sqldr.Close();

            //sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
            sqlComm.CommandText = "SELECT GETDATE()";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                //dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
                dateTimePicker1.Value = DateTime.Parse(Convert.ToDateTime(sqldr.GetValue(0).ToString()).Year.ToString() + "-" + Convert.ToDateTime(sqldr.GetValue(0).ToString()).Month.ToString() + "-1");

            }
            sqldr.Close();
            sqlConn.Close();

            switch (iStyle)
            {
                case 0: //���
                    comboBoxDJLB.Items.Add("������Ʒ��");
                    comboBoxDJLB.Items.Add("������ⵥ");
                    comboBoxDJLB.Items.Add("���۳��ⵥ");
                    comboBoxDJLB.Items.Add("����У�Ե�");
                    comboBoxDJLB.Items.Add("�����˳���");
                    comboBoxDJLB.Items.Add("�����˻ص�");
                    comboBoxDJLB.Items.Add("Ӧ���˿");
                    comboBoxDJLB.Items.Add("Ӧ���˿");
                    comboBoxDJLB.Items.Add("������ⵥ");
                    comboBoxDJLB.Items.Add("������ͬ");
                    comboBoxDJLB.Items.Add("���ۺ�ͬ");
                    comboBoxDJLB.SelectedIndex = 0;
                    this.Text += ":���";
                    btnEdit.Text = "���";
                    break;
                case 1://�޸�
                    //comboBoxDJLB.Items.Add("����������");
                    //comboBoxDJLB.Items.Add("���۳��ⵥ");
                    //comboBoxDJLB.Items.Add("�����˳���");
                    //comboBoxDJLB.Items.Add("�����˻ص�");
                    //comboBoxDJLB.Items.Add("������ⵥ");
                    comboBoxDJLB.Items.Add("������ͬ");
                    comboBoxDJLB.Items.Add("���ۺ�ͬ");

                    comboBoxDJLB.SelectedIndex = 0;
                    this.Text += ":�޸�";
                    btnEdit.Text = "�޸�";

                    break;
                case 2://��ע
                    comboBoxDJLB.Items.Add("������Ʒ�Ƶ�");
                    comboBoxDJLB.Items.Add("���۳��ⵥ");
                    comboBoxDJLB.Items.Add("�����˳���");
                    comboBoxDJLB.Items.Add("�����˻ص�");
                    comboBoxDJLB.Items.Add("Ӧ���˿");
                    comboBoxDJLB.Items.Add("Ӧ���˿");
                    comboBoxDJLB.Items.Add("������ⵥ");
                    comboBoxDJLB.Items.Add("������������");
                    comboBoxDJLB.Items.Add("���۳���У�Ե�");
                    //comboBoxDJLB.Items.Add("������ͬ");
                    //comboBoxDJLB.Items.Add("���ۺ�ͬ");

                    comboBoxDJLB.SelectedIndex = 0;
                    this.Text += ":��ע�޸�";
                    btnEdit.Text = "��ע�޸�";

                    break;
                default:
                    break;
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxDWBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getCompanyInformation(100, "") == 0)
            {
                //return;
            }
            else
            {
                iSupplyCompany = cGetInformation.iCompanyNumber;
                textBoxDWBH.Text = cGetInformation.strCompanyCode;
                textBoxDWMC.Text = cGetInformation.strCompanyName;
            }

        }

        private void textBoxDWBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1100, textBoxDWBH.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                }
            }
        }

        private void textBoxDWMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getCompanyInformation(1200, textBoxDWMC.Text.Trim()) == 0)
                {
                    iSupplyCompany = 0;
                }
                else
                {
                    iSupplyCompany = cGetInformation.iCompanyNumber;
                    textBoxDWBH.Text = cGetInformation.strCompanyCode;
                    textBoxDWMC.Text = cGetInformation.strCompanyName;
                }
            }

        }

        private void btnAccepy_Click(object sender, EventArgs e)
        {
            string strTemp = "";

            switch (iStyle)
            {
                case 0://���
                    switch (comboBoxDJLB.SelectedIndex)
                    {
                        case 0://������ⵥ
                            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ������Ʒ�Ƶ���.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ������Ʒ�Ƶ���.ҵ��ԱID = [ְԱ��_1].ID WHERE (������Ʒ�Ƶ���.BeActive = 1)";
                            strTemp = "������Ʒ�Ƶ���";
                            break;
                        case 1://������ⵥ
                            sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ���������ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ���������ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (���������ܱ�.BeActive = 1)";
                            strTemp = "���������ܱ�";
                            break;
                        case 2://���۳��ⵥ
                            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ������Ʒ�Ƶ���.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ������Ʒ�Ƶ���.ҵ��ԱID = [ְԱ��_1].ID WHERE (������Ʒ�Ƶ���.BeActive = 1)";
                            strTemp = "������Ʒ�Ƶ���";
                            break;
                        case 3://����У�Ե�
                            sqlComm.CommandText = "SELECT ���۳�����ܱ�.ID, ���۳�����ܱ�.���ݱ��, ���۳�����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ���۳�����ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM ���۳�����ܱ� INNER JOIN ��λ�� ON ���۳�����ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ���۳�����ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ���۳�����ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (���۳�����ܱ�.BeActive = 1)";
                            strTemp = "���۳�����ܱ�";
                            break;
                        case 4://�����˳���
                            sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON �����˳����ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON �����˳����ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (�����˳����ܱ�.BeActive = 1)";
                            strTemp = "�����˳����ܱ�";
                            break;
                        case 5://�����˻ص�
                            sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON �����˳����ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON �����˳����ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (�����˳����ܱ�.BeActive = 1)";
                            strTemp = "�����˳����ܱ�";
                            break;
                        case 6://Ӧ���˿
                            sqlComm.CommandText = "SELECT ���㸶����ܱ�.ID, ���㸶����ܱ�.���ݱ��, ���㸶����ܱ�.����,��λ��.��λ���, ��λ��.��λ����, ���㸶����ܱ�.ʵ�ƽ��, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM ���㸶����ܱ� INNER JOIN ��λ�� ON ���㸶����ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���㸶����ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ���㸶����ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (���㸶����ܱ�.BeActive = 1)";
                            strTemp = "���㸶����ܱ�";
                            break;
                        case 7://Ӧ���˿
                            sqlComm.CommandText = "SELECT �����տ���ܱ�.ID, �����տ���ܱ�.���ݱ��, �����տ���ܱ�.����,��λ��.��λ���, ��λ��.��λ����, �����տ���ܱ�.ʵ�ƽ��, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM �����տ���ܱ� INNER JOIN ��λ�� ON �����տ���ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �����տ���ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON �����տ���ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (�����տ���ܱ�.BeActive = 1)";
                            strTemp = "�����տ���ܱ�";
                            break;
                        case 8://������ⵥ
                            //sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ���������ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ���������ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.��ֵ���ID IS NULL)";
                            sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ���������ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ���������ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (���������ܱ�.BeActive = 1)";
                            strTemp = "���������ܱ�";
                            break;
                        case 9://������ͬ
                            sqlComm.CommandText = "SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.��ͬ���, �ɹ���ͬ��.ǩ��ʱ��, ��λ��.��λ���, ��λ��.��λ����, �ɹ���ͬ��.���, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON �ɹ���ͬ��.����ԱID = ����Ա.ID WHERE (�ɹ���ͬ��.BeActive = 1) AND (�ɹ���ͬ��.ִ�б�� = 0)";
                            strTemp = "�ɹ���ͬ��";
                            break;
                        case 10://���ۺ�ͬ
                            sqlComm.CommandText = "SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.��ͬ���, ���ۺ�ͬ��.ǩ��ʱ��, ��λ��.��λ���, ��λ��.��λ����, ���ۺ�ͬ��.���, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ���ۺ�ͬ��.����ԱID = ����Ա.ID WHERE (���ۺ�ͬ��.BeActive = 1) AND (���ۺ�ͬ��.ִ�б�� = 0)";
                            strTemp = "���ۺ�ͬ��";
                            break;

                    }
                    break;

                case 1: //�޸�
                    switch (comboBoxDJLB.SelectedIndex)
                    {
                            /*
                        case 0://����������
                            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ������Ʒ�Ƶ���.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ������Ʒ�Ƶ���.ҵ��ԱID = [ְԱ��_1].ID WHERE (������Ʒ�Ƶ���.BeActive = 1)";
                            strTemp = "������Ʒ�Ƶ���";
                            break;
                        case 1://���۳��ⵥ
                            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ������Ʒ�Ƶ���.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ������Ʒ�Ƶ���.ҵ��ԱID = [ְԱ��_1].ID WHERE (������Ʒ�Ƶ���.BeActive = 1)";
                            strTemp = "������Ʒ�Ƶ���";
                            break;
                        case 2://�����˳���
                            sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON �����˳����ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON �����˳����ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (�����˳����ܱ�.BeActive = 1)";
                            strTemp = "�����˳����ܱ�";
                            break;
                        case 3://�����˻ص�
                            sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON �����˳����ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON �����˳����ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (�����˳����ܱ�.BeActive = 1)";
                            strTemp = "�����˳����ܱ�";
                            break;
                        case 4://������ⵥ
                            sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ���������ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ���������ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (���������ܱ�.BeActive = 1)";
                            strTemp = "���������ܱ�";
                            break;
                             */
                        case 0://������ͬ
                            sqlComm.CommandText = "SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.��ͬ���, �ɹ���ͬ��.ǩ��ʱ��, ��λ��.��λ���, ��λ��.��λ����, �ɹ���ͬ��.���, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON �ɹ���ͬ��.����ԱID = ����Ա.ID WHERE (�ɹ���ͬ��.BeActive = 1) AND (�ɹ���ͬ��.ִ�б�� = 0)";
                            strTemp = "�ɹ���ͬ��";
                            break;

                        case 1://���ۺ�ͬ
                            sqlComm.CommandText = "SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.��ͬ���, ���ۺ�ͬ��.ǩ��ʱ��, ��λ��.��λ���, ��λ��.��λ����, ���ۺ�ͬ��.���, ְԱ��.ְԱ���� AS ҵ��Ա, ����Ա.ְԱ���� AS ����Ա FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID INNER JOIN ְԱ�� ����Ա ON ���ۺ�ͬ��.����ԱID = ����Ա.ID WHERE (���ۺ�ͬ��.BeActive = 1)AND (���ۺ�ͬ��.ִ�б�� = 0)";
                            strTemp = "���ۺ�ͬ��";
                            break;
                    }
                    break;

                case 2: //��ע
                    switch (comboBoxDJLB.SelectedIndex)
                    {
                        case 0://����������
                            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա, ������Ʒ�Ƶ���.��ע FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ������Ʒ�Ƶ���.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ������Ʒ�Ƶ���.ҵ��ԱID = [ְԱ��_1].ID WHERE (������Ʒ�Ƶ���.BeActive = 1)";
                            strTemp = "������Ʒ�Ƶ���";
                            break;
                        case 1://���۳��ⵥ
                            sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ������Ʒ�Ƶ���.���ݱ��, ������Ʒ�Ƶ���.����, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա, ������Ʒ�Ƶ���.��ע FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ������Ʒ�Ƶ���.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ������Ʒ�Ƶ���.ҵ��ԱID = [ְԱ��_1].ID WHERE (������Ʒ�Ƶ���.BeActive = 1)";
                            strTemp = "������Ʒ�Ƶ���";
                            break;
                        case 2://�����˳���
                            sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա, �����˳����ܱ�.��ע FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON �����˳����ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON �����˳����ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (�����˳����ܱ�.BeActive = 1)";
                            strTemp = "�����˳����ܱ�";
                            break;
                        case 3://�����˻ص�
                            sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.���ݱ��, �����˳����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա, �����˳����ܱ�.��ע FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON �����˳����ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON �����˳����ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (�����˳����ܱ�.BeActive = 1)";
                            strTemp = "�����˳����ܱ�";
                            break;
                        case 4://Ӧ���˿
                            sqlComm.CommandText = "SELECT ���㸶����ܱ�.ID, ���㸶����ܱ�.���ݱ��, ���㸶����ܱ�.����,��λ��.��λ���, ��λ��.��λ����, ���㸶����ܱ�.ʵ�ƽ��, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա, ���㸶����ܱ�.��ע, ���㸶����ܱ�.��ע2 FROM ���㸶����ܱ� INNER JOIN ��λ�� ON ���㸶����ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���㸶����ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ���㸶����ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (���㸶����ܱ�.BeActive = 1)";
                            strTemp = "���㸶����ܱ�";
                            break;
                        case 5://Ӧ���˿
                            sqlComm.CommandText = "SELECT �����տ���ܱ�.ID, �����տ���ܱ�.���ݱ��, �����տ���ܱ�.����,��λ��.��λ���, ��λ��.��λ����, �����տ���ܱ�.ʵ�ƽ��, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա, �����տ���ܱ�.��ע, �����տ���ܱ�.��ע2 FROM �����տ���ܱ� INNER JOIN ��λ�� ON �����տ���ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �����տ���ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON �����տ���ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (�����տ���ܱ�.BeActive = 1)";
                            strTemp = "�����տ���ܱ�";
                            break;
                        case 6://������ⵥ
                            sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա, ���������ܱ�.��ע FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ���������ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ���������ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (���������ܱ�.BeActive = 1)";
                            strTemp = "���������ܱ�";
                            break;
                        case 7://������������
                            sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.���ݱ��, ���������ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա, ���������ܱ�.��ע FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ���������ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ���������ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (���������ܱ�.BeActive = 1)";
                            strTemp = "���������ܱ�";
                            break;

                        case 8://���۳���У�Ե�
                            sqlComm.CommandText = "SELECT ���۳�����ܱ�.ID, ���۳�����ܱ�.���ݱ��, ���۳�����ܱ�.����, ��λ��.��λ���, ��λ��.��λ����, ���۳�����ܱ�.��˰�ϼ�, [ְԱ��_1].ְԱ���� AS ҵ��Ա, ְԱ��.ְԱ���� AS ����Ա, ���۳�����ܱ�.��ע FROM ���۳�����ܱ� INNER JOIN ��λ�� ON ���۳�����ܱ�.��λID = ��λ��.ID INNER JOIN  ְԱ�� ON ���۳�����ܱ�.����ԱID = ְԱ��.ID INNER JOIN ְԱ�� [ְԱ��_1] ON ���۳�����ܱ�.ҵ��ԱID = [ְԱ��_1].ID WHERE (���۳�����ܱ�.BeActive = 1)";
                            strTemp = "���۳�����ܱ�";
                            break;
                    }
                    break;
                default:
                    return;
            }

            if (iSupplyCompany != 0)
            {
                sqlComm.CommandText += " AND (��λ��.ID = "+iSupplyCompany.ToString()+")"; 
            }

            if (textBoxDJBH.Text!= "")
            {
                if ((iStyle == 0 && comboBoxDJLB.SelectedIndex >= 9) || (iStyle == 1 && comboBoxDJLB.SelectedIndex >= 0))
                    sqlComm.CommandText += " AND (" + strTemp + ".��ͬ��� LIKE N'%" + textBoxDJBH.Text + "%')";
                else
                    sqlComm.CommandText += " AND (" + strTemp + ".���ݱ�� LIKE N'%" + textBoxDJBH .Text+ "%')";
            }

            
            if (!checkBoxNo1.Checked)
            {
                if ((iStyle == 0 && comboBoxDJLB.SelectedIndex >= 9) || (iStyle == 1 && comboBoxDJLB.SelectedIndex >= 0))
                    sqlComm.CommandText += " AND (" + strTemp + ".ǩ��ʱ�� >= CONVERT(DATETIME, '" + dateTimePicker1.Value.ToShortDateString() + " 00:00:00', 102))";
                else
                    sqlComm.CommandText += " AND (" + strTemp + ".���� >= CONVERT(DATETIME, '"+dateTimePicker1.Value.ToShortDateString()+" 00:00:00', 102))";
            }

            if (!checkBoxNo2.Checked)
            {
                if ((iStyle == 0 && comboBoxDJLB.SelectedIndex >= 9) || (iStyle == 1 && comboBoxDJLB.SelectedIndex >= 0))
                    sqlComm.CommandText += " AND (" + strTemp + ".ǩ��ʱ�� <= CONVERT(DATETIME, '" + dateTimePicker2.Value.ToShortDateString() + " 00:00:00', 102))";
                else
                    sqlComm.CommandText += " AND (" + strTemp + ".���� <= CONVERT(DATETIME, '" + dateTimePicker2.Value.ToShortDateString() + " 00:00:00', 102))";

            }

            if ((iStyle == 0 && comboBoxDJLB.SelectedIndex >= 9) || (iStyle == 1 && comboBoxDJLB.SelectedIndex >= 0))
                sqlComm.CommandText += " ORDER BY  ǩ��ʱ�� DESC";
            else
                sqlComm.CommandText += " ORDER BY  ���� DESC";
            /*
            if (!checkBoxNo1.Checked)
            {
                    sqlComm.CommandText += " AND (" + strTemp + ".ǩ��ʱ�� >= CONVERT(DATETIME, '" + dateTimePicker1.Value.ToShortDateString() + " 00:00:00', 102))";
            }

            if (!checkBoxNo2.Checked)
            {
                    sqlComm.CommandText += " AND (" + strTemp + ".ǩ��ʱ�� <= CONVERT(DATETIME, '" + dateTimePicker2.Value.ToShortDateString() + " 23:59:59', 102))";
            }
            */

            sqlConn.Open();
            if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
            sqlDA.Fill(dSet, "���ݱ�");
            dataGridViewDJMX.DataSource = dSet.Tables["���ݱ�"];

            dataGridViewDJMX.Columns[0].Visible = false;
            dataGridViewDJMX.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewDJMX.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            sqlConn.Close();


            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.Rows.Count.ToString();
            dataGridViewDJMX.Focus();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
  
            //�������
            if (dataGridViewDJMX.SelectedRows.Count<1)
            {
                MessageBox.Show("��ѡ��Ҫ�����ĵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            intDJID = Convert.ToInt32(dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString());

            switch (iStyle)
            {
                case 0://���
                    switch (comboBoxDJLB.SelectedIndex)
                    {
                        case 0://����������
                            // �������Ӵ����һ����ʵ����
                            FormGJSPZD_EDIT childFormGJSPZD = new FormGJSPZD_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormGJSPZD.MdiParent = this.MdiParent;

                            childFormGJSPZD.strConn = strConn;
                            childFormGJSPZD.intDJID = intDJID;
                            childFormGJSPZD.intUserID = intUserID;
                            childFormGJSPZD.intUserLimit = intUserLimit;
                            childFormGJSPZD.strUserLimit = strUserLimit;
                            childFormGJSPZD.strUserName = strUserName;

                            if (intUserLimit < iConstLimit)
                            {
                                childFormGJSPZD.printToolStripButton.Visible = false;
                                childFormGJSPZD.printPreviewToolStripButton.Visible = false;
                            }
                            childFormGJSPZD.Show();
                            break;
                        case 1://������ⵥ

                            // �������Ӵ����һ����ʵ����
                            FormJHRKYHD_EDIT childFormJHRKYHD = new FormJHRKYHD_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormJHRKYHD.MdiParent = this.MdiParent;

                            childFormJHRKYHD.strConn = strConn;
                            childFormJHRKYHD.intDJID = intDJID;
                            if (intUserLimit < iConstLimit)
                            {
                                childFormJHRKYHD.printToolStripButton.Visible = false;
                                childFormJHRKYHD.printPreviewToolStripButton.Visible = false;
                            }

                            childFormJHRKYHD.intUserID = intUserID;
                            childFormJHRKYHD.intUserLimit = intUserLimit;
                            childFormJHRKYHD.strUserLimit = strUserLimit;
                            childFormJHRKYHD.strUserName = strUserName;
                            childFormJHRKYHD.Show();
                            break;
                        case 2://���۳��ⵥ
                            // �������Ӵ����һ����ʵ����
                            FormXSCKZD_EDIT childFormXSCKZD = new FormXSCKZD_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormXSCKZD.MdiParent = this.MdiParent;

                            childFormXSCKZD.strConn = strConn;
                            childFormXSCKZD.intDJID = intDJID;
                            if (intUserLimit < iConstLimit)
                            {
                                childFormXSCKZD.printToolStripButton.Visible = false;
                                childFormXSCKZD.printPreviewToolStripButton.Visible = false;
                            }

                            childFormXSCKZD.intUserID = intUserID;
                            childFormXSCKZD.intUserLimit = intUserLimit;
                            childFormXSCKZD.strUserLimit = strUserLimit;
                            childFormXSCKZD.strUserName = strUserName;
                            childFormXSCKZD.Show();
                            break;
                        case 3://����У�Ե�
                            // �������Ӵ����һ����ʵ����
                            FormXSCKJD_EDIT childFormXSCKJD = new FormXSCKJD_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormXSCKJD.MdiParent = this.MdiParent;

                            childFormXSCKJD.strConn = strConn;
                            childFormXSCKJD.intDJID = intDJID;
                            if (intUserLimit < iConstLimit)
                            {
                                childFormXSCKJD.printToolStripButton.Visible = false;
                                childFormXSCKJD.printPreviewToolStripButton.Visible = false;
                            }

                            childFormXSCKJD.intUserID = intUserID;
                            childFormXSCKJD.intUserLimit = intUserLimit;
                            childFormXSCKJD.strUserLimit = strUserLimit;
                            childFormXSCKJD.strUserName = strUserName;
                            childFormXSCKJD.Show();
                            break;
                        case 4://�����˳���
                            // �������Ӵ����һ����ʵ����
                            FormJHTCZD_EDIT childFormJHTCZD = new FormJHTCZD_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormJHTCZD.MdiParent = this.MdiParent;

                            childFormJHTCZD.strConn = strConn;
                            childFormJHTCZD.intDJID = intDJID;
                            if (intUserLimit < iConstLimit)
                            {
                                childFormJHTCZD.printToolStripButton.Visible = false;
                                childFormJHTCZD.printPreviewToolStripButton.Visible = false;
                            }

                            childFormJHTCZD.intUserID = intUserID;
                            childFormJHTCZD.intUserLimit = intUserLimit;
                            childFormJHTCZD.strUserLimit = strUserLimit;
                            childFormJHTCZD.strUserName = strUserName;
                            childFormJHTCZD.Show();
                            break;
                        case 5://�����˻ص�
                            // �������Ӵ����һ����ʵ����
                            FormXSTHZD_EDIT childFormXSTHZD = new FormXSTHZD_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormXSTHZD.MdiParent = this.MdiParent;

                            childFormXSTHZD.strConn = strConn;
                            childFormXSTHZD.intDJID = intDJID;
                            if (intUserLimit < iConstLimit)
                            {
                                childFormXSTHZD.printToolStripButton.Visible = false;
                                childFormXSTHZD.printPreviewToolStripButton.Visible = false;
                            }

                            childFormXSTHZD.intUserID = intUserID;
                            childFormXSTHZD.intUserLimit = intUserLimit;
                            childFormXSTHZD.strUserLimit = strUserLimit;
                            childFormXSTHZD.strUserName = strUserName;
                            childFormXSTHZD.Show();
                            break;
                        case 6://Ӧ���˿
                            // �������Ӵ����һ����ʵ����
                            FormYFZKJS_EDIT childFormYFZKJS = new FormYFZKJS_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormYFZKJS.MdiParent = this.MdiParent;

                            childFormYFZKJS.strConn = strConn;
                            childFormYFZKJS.intDJID = intDJID;
                            if (intUserLimit < iConstLimit)
                            {
                                childFormYFZKJS.printToolStripButton.Visible = false;
                                childFormYFZKJS.printPreviewToolStripButton.Visible = false;
                            }


                            childFormYFZKJS.intUserID = intUserID;
                            childFormYFZKJS.intUserLimit = intUserLimit;
                            childFormYFZKJS.strUserLimit = strUserLimit;
                            childFormYFZKJS.strUserName = strUserName;
                            childFormYFZKJS.Show();
                            break;
                        case 7://Ӧ���˿
                            // �������Ӵ����һ����ʵ����
                            FormYSZKJS_EDIT childFormYSZKJS = new FormYSZKJS_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormYSZKJS.MdiParent = this.MdiParent;

                            childFormYSZKJS.strConn = strConn;
                            childFormYSZKJS.intDJID = intDJID;
                            if (intUserLimit < iConstLimit)
                            {
                                childFormYSZKJS.printToolStripButton.Visible = false;
                                childFormYSZKJS.printPreviewToolStripButton.Visible = false;
                            }
                            childFormYSZKJS.intUserID = intUserID;
                            childFormYSZKJS.intUserLimit = intUserLimit;
                            childFormYSZKJS.strUserLimit = strUserLimit;
                            childFormYSZKJS.strUserName = strUserName;
                            childFormYSZKJS.Show();
                            break;
                        case 8://������ⵥ
                            // �������Ӵ����һ����ʵ����
                            FormKCJWCKDJ_EDIT childFormKCJWCKDJ = new FormKCJWCKDJ_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormKCJWCKDJ.MdiParent = this.MdiParent;

                            childFormKCJWCKDJ.strConn = strConn;
                            childFormKCJWCKDJ.intDJID = intDJID;
                            childFormKCJWCKDJ.iStyle = 0;
                            if (intUserLimit < iConstLimit)
                            {
                                childFormKCJWCKDJ.printToolStripButton.Visible = false;
                                childFormKCJWCKDJ.printPreviewToolStripButton.Visible = false;
                            }
                            childFormKCJWCKDJ.intUserID = intUserID;
                            childFormKCJWCKDJ.intUserLimit = intUserLimit;
                            childFormKCJWCKDJ.strUserLimit = strUserLimit;
                            childFormKCJWCKDJ.strUserName = strUserName;
                            childFormKCJWCKDJ.Show(); 
                            break;

                        case 9://�ɹ���ͬ
                            // �������Ӵ����һ����ʵ����
                            FormCGHT_EDIT childFormCGHT = new FormCGHT_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormCGHT.MdiParent = this.MdiParent;

                            childFormCGHT.strConn = strConn;
                            childFormCGHT.iDJID= intDJID;
                            childFormCGHT.iStyle = 0;

                            childFormCGHT.intUserID = intUserID;
                            childFormCGHT.intUserLimit = intUserLimit;
                            childFormCGHT.strUserLimit = strUserLimit;
                            childFormCGHT.strUserName = strUserName;
                            childFormCGHT.Show();
                            break;

                        case 10://���ۺ�ͬ
                            // �������Ӵ����һ����ʵ����
                            FormXSHT_EDIT childFormXSHT = new FormXSHT_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormXSHT.MdiParent = this.MdiParent;

                            childFormXSHT.strConn = strConn;
                            childFormXSHT.iDJID = intDJID;
                            childFormXSHT.iStyle = 0;

                            childFormXSHT.intUserID = intUserID;
                            childFormXSHT.intUserLimit = intUserLimit;
                            childFormXSHT.strUserLimit = strUserLimit;
                            childFormXSHT.strUserName = strUserName;
                            childFormXSHT.Show();
                            break;
                    }
                    break;

                case 1: //�޸�
                    switch (comboBoxDJLB.SelectedIndex)
                    {
                            /*
                        case 0://����������
                            // �������Ӵ����һ����ʵ����
                            FormGJSPZD_EDIT childFormGJSPZD = new FormGJSPZD_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormGJSPZD.MdiParent = this.MdiParent;

                            childFormGJSPZD.strConn = strConn;
                            childFormGJSPZD.intDJID = intDJID;
                            childFormGJSPZD.intUserID = intUserID;
                            childFormGJSPZD.intUserLimit = intUserLimit;
                            childFormGJSPZD.strUserLimit = strUserLimit;
                            childFormGJSPZD.strUserName = strUserName;
                            childFormGJSPZD.Show();
                            break;
                        case 1://���۳��ⵥ
                            // �������Ӵ����һ����ʵ����
                            FormXSCKZD_EDIT childFormXSCKZD = new FormXSCKZD_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormXSCKZD.MdiParent = this.MdiParent;

                            childFormXSCKZD.strConn = strConn;
                            childFormXSCKZD.intDJID = intDJID;
                            childFormXSCKZD.iStyle=1;

                            childFormXSCKZD.intUserID = intUserID;
                            childFormXSCKZD.intUserLimit = intUserLimit;
                            childFormXSCKZD.strUserLimit = strUserLimit;
                            childFormXSCKZD.strUserName = strUserName;
                            childFormXSCKZD.Show();
                            break;

                        case 2://�����˳���
                            // �������Ӵ����һ����ʵ����
                            FormJHTCZD_EDIT childFormJHTCZD = new FormJHTCZD_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormJHTCZD.MdiParent = this.MdiParent;

                            childFormJHTCZD.strConn = strConn;
                            childFormJHTCZD.intDJID = intDJID;
                            childFormJHTCZD.iStyle = 1;

                            childFormJHTCZD.intUserID = intUserID;
                            childFormJHTCZD.intUserLimit = intUserLimit;
                            childFormJHTCZD.strUserLimit = strUserLimit;
                            childFormJHTCZD.strUserName = strUserName;
                            childFormJHTCZD.Show();
                            break;

                        case 3://�����˻ص�
                            // �������Ӵ����һ����ʵ����
                            FormXSTHZD_EDIT childFormXSTHZD = new FormXSTHZD_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormXSTHZD.MdiParent = this.MdiParent;

                            childFormXSTHZD.strConn = strConn;
                            childFormXSTHZD.intDJID = intDJID;
                            childFormXSTHZD.iStyle = 1;

                            childFormXSTHZD.intUserID = intUserID;
                            childFormXSTHZD.intUserLimit = intUserLimit;
                            childFormXSTHZD.strUserLimit = strUserLimit;
                            childFormXSTHZD.strUserName = strUserName;
                            childFormXSTHZD.Show();
                            break;
                        case 4://������ⵥ
                            // �������Ӵ����һ����ʵ����
                            FormKCJWCKDJ_EDIT childFormKCJWCKDJ = new FormKCJWCKDJ_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormKCJWCKDJ.MdiParent = this.MdiParent;

                            childFormKCJWCKDJ.strConn = strConn;
                            childFormKCJWCKDJ.intDJID = intDJID;
                            childFormKCJWCKDJ.iStyle = 1;

                            childFormKCJWCKDJ.intUserID = intUserID;
                            childFormKCJWCKDJ.intUserLimit = intUserLimit;
                            childFormKCJWCKDJ.strUserLimit = strUserLimit;
                            childFormKCJWCKDJ.strUserName = strUserName;
                            childFormKCJWCKDJ.Show();                                
                            break;
                             * */
                        case 0://�ɹ���ͬ
                            // �������Ӵ����һ����ʵ����
                            FormCGHT_EDIT childFormCGHT = new FormCGHT_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormCGHT.MdiParent = this.MdiParent;

                            childFormCGHT.strConn = strConn;
                            childFormCGHT.iDJID = intDJID;
                            childFormCGHT.iStyle = 1;

                            childFormCGHT.intUserID = intUserID;
                            childFormCGHT.intUserLimit = intUserLimit;
                            childFormCGHT.strUserLimit = strUserLimit;
                            childFormCGHT.strUserName = strUserName;
                            childFormCGHT.Show();
                            break;

                        case 1://�ɹ���ͬ
                            // �������Ӵ����һ����ʵ����
                            FormXSHT_EDIT childFormXSHT = new FormXSHT_EDIT();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormXSHT.MdiParent = this.MdiParent;

                            childFormXSHT.strConn = strConn;
                            childFormXSHT.iDJID = intDJID;
                            childFormXSHT.iStyle = 1;

                            childFormXSHT.intUserID = intUserID;
                            childFormXSHT.intUserLimit = intUserLimit;
                            childFormXSHT.strUserLimit = strUserLimit;
                            childFormXSHT.strUserName = strUserName;
                            childFormXSHT.Show();
                            break;
                    }
                    break;
                case 2://��ע
                    switch (comboBoxDJLB.SelectedIndex)
                    {
                        case 0://������ⵥ

                            // �������Ӵ����һ����ʵ����
                            FormBZXG childFormBZXG = new FormBZXG();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormBZXG.MdiParent = this.MdiParent;

                            childFormBZXG.strConn = strConn;
                            childFormBZXG.iDJLX = 0;

                            childFormBZXG.intUserID = intUserID;
                            childFormBZXG.intDJID = intDJID;
                            childFormBZXG.intUserLimit = intUserLimit;
                            childFormBZXG.strUserLimit = strUserLimit;
                            childFormBZXG.strUserName = strUserName;
                            childFormBZXG.Show();
                            break;
                        case 1://���۳��ⵥ

                            // �������Ӵ����һ����ʵ����
                            FormBZXG childFormBZXG1 = new FormBZXG();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormBZXG1.MdiParent = this.MdiParent;

                            childFormBZXG1.strConn = strConn;
                            childFormBZXG1.iDJLX = 1;

                            childFormBZXG1.intUserID = intUserID;
                            childFormBZXG1.intDJID = intDJID;
                            childFormBZXG1.intUserLimit = intUserLimit;
                            childFormBZXG1.strUserLimit = strUserLimit;
                            childFormBZXG1.strUserName = strUserName;
                            childFormBZXG1.Show();
                            break;
                        case 2://�����˳���
                            // �������Ӵ����һ����ʵ����
                            FormBZXG childFormBZXG2 = new FormBZXG();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormBZXG2.MdiParent = this.MdiParent;

                            childFormBZXG2.strConn = strConn;
                            childFormBZXG2.iDJLX = 2;

                            childFormBZXG2.intUserID = intUserID;
                            childFormBZXG2.intDJID = intDJID;
                            childFormBZXG2.intUserLimit = intUserLimit;
                            childFormBZXG2.strUserLimit = strUserLimit;
                            childFormBZXG2.strUserName = strUserName;
                            childFormBZXG2.Show();
                            break;
                        case 3://�����˻ص�
                            // �������Ӵ����һ����ʵ����
                            FormBZXG childFormBZXG3 = new FormBZXG();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormBZXG3.MdiParent = this.MdiParent;

                            childFormBZXG3.strConn = strConn;
                            childFormBZXG3.iDJLX = 3;

                            childFormBZXG3.intUserID = intUserID;
                            childFormBZXG3.intDJID = intDJID;
                            childFormBZXG3.intUserLimit = intUserLimit;
                            childFormBZXG3.strUserLimit = strUserLimit;
                            childFormBZXG3.strUserName = strUserName;
                            childFormBZXG3.Show();
                            break;
                        case 4://Ӧ���˿
                            // �������Ӵ����һ����ʵ����
                            FormBZXG childFormBZXG4 = new FormBZXG();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormBZXG4.MdiParent = this.MdiParent;

                            childFormBZXG4.strConn = strConn;
                            childFormBZXG4.iDJLX = 4;

                            childFormBZXG4.intUserID = intUserID;
                            childFormBZXG4.intDJID = intDJID;
                            childFormBZXG4.intUserLimit = intUserLimit;
                            childFormBZXG4.strUserLimit = strUserLimit;
                            childFormBZXG4.strUserName = strUserName;
                            childFormBZXG4.Show(); ;
                            break;
                        case 5://Ӧ���˿
                            // �������Ӵ����һ����ʵ����
                            FormBZXG childFormBZXG5 = new FormBZXG();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormBZXG5.MdiParent = this.MdiParent;

                            childFormBZXG5.strConn = strConn;
                            childFormBZXG5.iDJLX = 5;

                            childFormBZXG5.intUserID = intUserID;
                            childFormBZXG5.intDJID = intDJID;
                            childFormBZXG5.intUserLimit = intUserLimit;
                            childFormBZXG5.strUserLimit = strUserLimit;
                            childFormBZXG5.strUserName = strUserName;
                            childFormBZXG5.Show();
                            break;
                        case 6://������ⵥ
                            // �������Ӵ����һ����ʵ����
                            FormBZXG childFormBZXG6 = new FormBZXG();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormBZXG6.MdiParent = this.MdiParent;

                            childFormBZXG6.strConn = strConn;
                            childFormBZXG6.iDJLX = 6;

                            childFormBZXG6.intUserID = intUserID;
                            childFormBZXG6.intDJID = intDJID;
                            childFormBZXG6.intUserLimit = intUserLimit;
                            childFormBZXG6.strUserLimit = strUserLimit;
                            childFormBZXG6.strUserName = strUserName;
                            childFormBZXG6.Show();
                            break;
                        case 7://������ⵥ
                            // �������Ӵ����һ����ʵ����
                            FormBZXG childFormBZXG7 = new FormBZXG();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormBZXG7.MdiParent = this.MdiParent;

                            childFormBZXG7.strConn = strConn;
                            childFormBZXG7.iDJLX = 7;

                            childFormBZXG7.intUserID = intUserID;
                            childFormBZXG7.intDJID = intDJID;
                            childFormBZXG7.intUserLimit = intUserLimit;
                            childFormBZXG7.strUserLimit = strUserLimit;
                            childFormBZXG7.strUserName = strUserName;
                            childFormBZXG7.Show();
                            break;
                        case 8://������ⵥ
                            // �������Ӵ����һ����ʵ����
                            FormBZXG childFormBZXG8 = new FormBZXG();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormBZXG8.MdiParent = this.MdiParent;

                            childFormBZXG8.strConn = strConn;
                            childFormBZXG8.iDJLX = 8;

                            childFormBZXG8.intUserID = intUserID;
                            childFormBZXG8.intDJID = intDJID;
                            childFormBZXG8.intUserLimit = intUserLimit;
                            childFormBZXG8.strUserLimit = strUserLimit;
                            childFormBZXG8.strUserName = strUserName;
                            childFormBZXG8.Show();
                            break;
                            /*
                        case 7://�ɹ���ͬ
                            // �������Ӵ����һ����ʵ����
                            FormBZXG childFormBZXG7 = new FormBZXG();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormBZXG7.MdiParent = this.MdiParent;

                            childFormBZXG7.strConn = strConn;
                            childFormBZXG7.iDJLX = 7;

                            childFormBZXG7.intUserID = intUserID;
                            childFormBZXG7.intDJID = intDJID;
                            childFormBZXG7.intUserLimit = intUserLimit;
                            childFormBZXG7.strUserLimit = strUserLimit;
                            childFormBZXG7.strUserName = strUserName;
                            childFormBZXG7.Show();
                            break;

                        case 8://���ۺ�ͬ
                            // �������Ӵ����һ����ʵ����
                            FormBZXG childFormBZXG8 = new FormBZXG();
                            // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                            childFormBZXG8.MdiParent = this.MdiParent;

                            childFormBZXG8.strConn = strConn;
                            childFormBZXG8.iDJLX = 8;

                            childFormBZXG8.intUserID = intUserID;
                            childFormBZXG8.intDJID = intDJID;
                            childFormBZXG8.intUserLimit = intUserLimit;
                            childFormBZXG8.strUserLimit = strUserLimit;
                            childFormBZXG8.strUserName = strUserName;
                            childFormBZXG8.Show();
                            break;
                          */
                    }
                    break;
                default:
                    return;
            }
        }

        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
             btnEdit_Click(null,null);
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "���󵥾ݴ���;";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true,intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "���󵥾ݴ���;";
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);

        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && dataGridViewDJMX.Focused)
            {
                //System.Windows.Forms.SendKeys.Send("{tab}");
                btnEdit_Click(null, null);//
                return true;
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void comboBoxDJLB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                btnAccepy.Focus();
            }
        }
    }
}