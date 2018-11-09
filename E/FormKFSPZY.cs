using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormKFSPZY : Form
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

        private int intKFID = 0;
        private int intCommID = 0;

        private ClassGetInformation cGetInformation;
        public FormKFSPZY()
        {
            InitializeComponent();
        }

        private void FormKFSPZY_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            this.Top = 1;
            this.Left = 1;

            sqlConn.Open();
            //�õ���ʼʱ��
            sqlComm.CommandText = "SELECT ��ʼʱ�� FROM ϵͳ������";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dateTimePickerS.Value = Convert.ToDateTime(sqldr.GetValue(0).ToString());
            }
            sqldr.Close();
            sqlConn.Close();

            string strDT;
            cGetInformation.getSystemDateTime();
            strDT = cGetInformation.strSYSDATATIME;
            labelZDRQ.Text = Convert.ToDateTime(strDT).ToString("yyyy��M��dd��");
            labelCZY.Text = strUserName;

            comboBoxDJLB.SelectedIndex = 0;
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

                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;

                }

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

                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;

                }

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

                intKFID = cGetInformation.iKFNumber;
                textBoxKFBH.Text = cGetInformation.strKFCode;
                textBoxKFMC.Text = cGetInformation.strKFName;
            }
        }

        private void textBoxKFMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(20, textBoxKFMC.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                }

            }
        }

        private void textBoxKFBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (cGetInformation.getKFInformation(10, textBoxKFBH.Text) == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    intKFID = cGetInformation.iKFNumber;
                    textBoxKFBH.Text = cGetInformation.strKFCode;
                    textBoxKFMC.Text = cGetInformation.strKFName;
                }

            }
        }

        private void textBoxKFBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(1, "") == 0) //ʧ��
            {
                return;
            }
            else
            {
                intKFID = cGetInformation.iKFNumber;
                textBoxKFBH.Text = cGetInformation.strKFCode;
                textBoxKFMC.Text = cGetInformation.strKFName;
            }
        }

        private void toolStripButtonGD_Click(object sender, EventArgs e)
        {
            int i, j;
            decimal dsum = 0, dsum1 = 0, dsum2 = 0; ;
            //�������
            if (intCommID == 0)
            {
                MessageBox.Show("��ѡ��Ҫ��ѯ����Ʒ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (intKFID == 0)
            {
                MessageBox.Show("��ѡ��Ҫ��ѯ�Ŀⷿ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            sqlConn.Open();
            //sqlComm.CommandText = "SELECT ����, ���ݱ��, ժҪ, �������, ��ⵥ��, �����, ��������, ������, �˳�����, �˳����, �˻�����, �˻ؽ��, ��������, ������, �ⷿ�������, �ⷿ�����, ���۽��, ë�� FROM ��Ʒ�ⷿ��ʷ�˱� WHERE (��ƷID = " + intCommID.ToString() + ") AND (BeActive = 1) AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�ⷿID = " + intKFID.ToString() + ") ORDER BY ����";
            sqlComm.CommandText = "SELECT ���ݱ��, ����, ����, ʵ�ƽ��, ë��, '' AS ˵��, ��ID  FROM ������ϸ������ͼ WHERE (BeActive = 1) AND (��ƷID = " + intCommID.ToString() + ") AND (���� >= CONVERT(DATETIME, '" + dateTimePickerS.Value.ToShortDateString() + " 00:00:00', 102)) AND (�ⷿID = " + intKFID.ToString() + ")";
            switch (comboBoxDJLB.SelectedIndex)
            {
                case 1:
                    sqlComm.CommandText += " AND ���ݱ��  LIKE N'%AKP%' ";
                    break;
                case 2:
                    sqlComm.CommandText += " AND ���ݱ��  LIKE N'%ADH%' ";
                    break;
                case 3:
                    sqlComm.CommandText += " AND ���ݱ��  LIKE N'%ATH%' ";
                    break;
                case 4:
                    sqlComm.CommandText += " AND ���ݱ��  LIKE N'%ATB%' ";
                    break;
                case 5:
                    sqlComm.CommandText += " AND ���ݱ��  LIKE N'%BKP%' ";
                    break;
                case 6:
                    sqlComm.CommandText += " AND ���ݱ��  LIKE N'%BCK%' ";
                    break;
                case 7:
                    sqlComm.CommandText += " AND ���ݱ��  LIKE N'%BTH%' ";
                    break;
                case 8:
                    sqlComm.CommandText += " AND ���ݱ��  LIKE N'%BTB%' ";
                    break;
                case 9:
                    sqlComm.CommandText += " AND ���ݱ��  LIKE N'%CCK%' AND ���� > 0 ";
                    break;
                case 10:
                    sqlComm.CommandText += " AND ���ݱ��  LIKE N'%CCK%' AND ���� < 0 ";
                    break;
            }
            sqlComm.CommandText += " ORDER BY ����";

            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");
            dataGridViewDJMX.DataSource = dSet.Tables["��Ʒ��"];
            dataGridViewDJMX.Columns[6].Visible = false;

            /*
            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
                for (j = 3; j < dSet.Tables["��Ʒ��"].Columns.Count; j++)
                {
                    if (dSet.Tables["��Ʒ��"].Rows[i][j].ToString() == "")
                        dSet.Tables["��Ʒ��"].Rows[i][j] = 0;

                }
             */
            string stemp = "";
            for (i = 0; i < dSet.Tables["��Ʒ��"].Rows.Count; i++)
            {
                if (dSet.Tables["��Ʒ��"].Rows[i][4].ToString() != "")
                    dsum += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][4].ToString());
                if (dSet.Tables["��Ʒ��"].Rows[i][1].ToString() != "")
                    dsum1 += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][1].ToString());
                if (dSet.Tables["��Ʒ��"].Rows[i][3].ToString() != "")
                    dsum2 += decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][3].ToString());

                stemp = dSet.Tables["��Ʒ��"].Rows[i][0].ToString().Substring(0, 3);
                switch (stemp)
                {
                    case "AKP":
                        dSet.Tables["��Ʒ��"].Rows[i][5] = "������Ʒ��Ʊ";
                        break;
                    case "ADH":
                        dSet.Tables["��Ʒ��"].Rows[i][5] = "������Ʒ���";
                        break;
                    case "ATH":
                        dSet.Tables["��Ʒ��"].Rows[i][5] = "������Ʒ�˳�";
                        break;
                    case "ATB":
                        dSet.Tables["��Ʒ��"].Rows[i][5] = "������Ʒ�˲����";
                        break;
                    case "BKP":
                        dSet.Tables["��Ʒ��"].Rows[i][5] = "������Ʒ����";
                        break;
                    case "BCK":
                        dSet.Tables["��Ʒ��"].Rows[i][5] = "������Ʒ����У��";
                        break;
                    case "BTH":
                        dSet.Tables["��Ʒ��"].Rows[i][5] = "������Ʒ�˻�";
                        break;
                    case "BTB":
                        dSet.Tables["��Ʒ��"].Rows[i][5] = "������Ʒ�˲����";
                        break;
                    case "CCK":
                        dSet.Tables["��Ʒ��"].Rows[i][5] = "�������";
                        sqlComm.CommandText = "SELECT ������, ��ֵ���ID, ���ݱ�� FROM ���������ܱ� WHERE (���ݱ�� = N'" + dSet.Tables["��Ʒ��"].Rows[i][0].ToString() + "')";
                        sqldr = sqlComm.ExecuteReader();

                        if(!sqldr.HasRows)
                        {
                            if (decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][1].ToString()) > 0)
                                dSet.Tables["��Ʒ��"].Rows[i][5] = "�������";
                            else
                                dSet.Tables["��Ʒ��"].Rows[i][5] = "�������";
                            sqldr.Close();
                            break;
                        }
                        sqldr.Read();
                        try
                        {
                            if (decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][1].ToString()) > 0)
                            {
                                if (sqldr.GetValue(1).ToString() == "-1")
                                    dSet.Tables["��Ʒ��"].Rows[i][5] = "�����ֵ������⣩";
                                else
                                {
                                    if (sqldr.GetValue(1).ToString() == "")
                                        dSet.Tables["��Ʒ��"].Rows[i][5] = "������ⵥ��δ��֣�";
                                    else
                                        dSet.Tables["��Ʒ��"].Rows[i][5] = "������ⵥ���ѳ�֣�";
                                }

                            }
                            else
                            {
                                if (sqldr.GetValue(1).ToString() == "-1")
                                    dSet.Tables["��Ʒ��"].Rows[i][5] = "�����ֵ�����⣩";
                                else
                                {
                                    if (sqldr.GetValue(1).ToString() == "")
                                        dSet.Tables["��Ʒ��"].Rows[i][5] = "������ⵥ��δ��֣�";
                                    else
                                        dSet.Tables["��Ʒ��"].Rows[i][5] = "������ⵥ���ѳ�֣�";
                                }
                            }


                        }
                        catch
                        {
                            if (decimal.Parse(dSet.Tables["��Ʒ��"].Rows[i][1].ToString())>0)
                                dSet.Tables["��Ʒ��"].Rows[i][5] = "�������";
                            else
                                dSet.Tables["��Ʒ��"].Rows[i][5] = "�������";
                        }
                        sqldr.Close();
                        break;
                    default:
                        break;


                }
            }

            dataGridViewDJMX.Columns[1].DefaultCellStyle.Format = "f0";
            dataGridViewDJMX.Columns[2].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[3].DefaultCellStyle.Format = "f2";
            dataGridViewDJMX.Columns[4].DefaultCellStyle.Format = "f2";


            for (i = 0; i < dataGridViewDJMX.ColumnCount; i++)
            {
                dataGridViewDJMX.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            toolStripStatusLabelMXJLS.Text = dataGridViewDJMX.RowCount.ToString();
            if (comboBoxDJLB.SelectedIndex == 0)
                toolStripStatusLabelMXJLS.Text += " ë��:" + dsum.ToString("f2");
            else
                toolStripStatusLabelMXJLS.Text += " ����:" + dsum1.ToString("f0") + " ���:" + dsum2.ToString("f2");
            sqlConn.Close();

        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "�ⷿ��Ʒ��ҳ(��Ʒ:" + textBoxSPMC.Text + ");��ǰ���ڣ�" + labelZDRQ.Text + ";�ⷿ��"+textBoxKFMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, true, intUserLimit);

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "�ⷿ��Ʒ��ҳ(��Ʒ:" + textBoxSPMC.Text + ");��ǰ���ڣ�" + labelZDRQ.Text + ";�ⷿ��" + textBoxKFMC.Text;
            PrintDGV.Print_DataGridView(dataGridViewDJMX, strT, false, intUserLimit);
        }


        private void dataGridViewDJMX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewDJMX.RowCount < 1)
                return;

            if (dataGridViewDJMX.SelectedRows.Count < 1)
                return;

            string sTemp = "", sTemp1 = "";

            if (e == null)
            {
                sTemp = dataGridViewDJMX.SelectedRows[0].Cells[0].Value.ToString().ToUpper();
                sTemp1 = dataGridViewDJMX.SelectedRows[0].Cells[6].Value.ToString();
            }
            else
            {
                sTemp = dataGridViewDJMX.Rows[e.RowIndex].Cells[0].Value.ToString().ToUpper();
                sTemp1 = dataGridViewDJMX.Rows[e.RowIndex].Cells[6].Value.ToString();
            }

            //if(e.RowIndex<0)
            //    return;

            //if (dataGridViewDJMX.Rows[e.RowIndex].IsNewRow)
            //    return;


            switch (sTemp.Substring(0, 2))
            {
                case "CG":
                    // �������Ӵ����һ����ʵ����
                    FormCGHT childFormCGHT = new FormCGHT();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormCGHT.MdiParent = this.MdiParent;

                    childFormCGHT.strConn = strConn;
                    childFormCGHT.iDJID = Convert.ToInt32(sTemp1);
                    childFormCGHT.isSaved = true;

                    childFormCGHT.intUserID = intUserID;
                    childFormCGHT.intUserLimit = intUserLimit;
                    childFormCGHT.strUserLimit = strUserLimit;
                    childFormCGHT.strUserName = strUserName;
                    childFormCGHT.Show();
                    break;
                case "XS":
                    // �������Ӵ����һ����ʵ����
                    FormXSHT childFormXSHT = new FormXSHT();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSHT.MdiParent = this.MdiParent;

                    childFormXSHT.strConn = strConn;
                    childFormXSHT.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSHT.isSaved = true;

                    childFormXSHT.intUserID = intUserID;
                    childFormXSHT.intUserLimit = intUserLimit;
                    childFormXSHT.strUserLimit = strUserLimit;
                    childFormXSHT.strUserName = strUserName;
                    childFormXSHT.Show();
                    break;
            }

            switch (sTemp.Substring(0, 3))
            {
                case "AKP":
                    // �������Ӵ����һ����ʵ����
                    FormGJSPZD childFormGJSPZD = new FormGJSPZD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormGJSPZD.MdiParent = this.MdiParent;

                    childFormGJSPZD.strConn = strConn;
                    childFormGJSPZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormGJSPZD.isSaved = true;

                    childFormGJSPZD.intUserID = intUserID;
                    childFormGJSPZD.intUserLimit = intUserLimit;
                    childFormGJSPZD.strUserLimit = strUserLimit;
                    childFormGJSPZD.strUserName = strUserName;
                    childFormGJSPZD.Show();
                    break;

                case "ADH":
                    // �������Ӵ����һ����ʵ����
                    FormJHRKYHD childFormJHRKYHD = new FormJHRKYHD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormJHRKYHD.MdiParent = this.MdiParent;

                    childFormJHRKYHD.strConn = strConn;
                    childFormJHRKYHD.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHRKYHD.isSaved = true;

                    childFormJHRKYHD.intUserID = intUserID;
                    childFormJHRKYHD.intUserLimit = intUserLimit;
                    childFormJHRKYHD.strUserLimit = strUserLimit;
                    childFormJHRKYHD.strUserName = strUserName;
                    childFormJHRKYHD.Show();
                    break;

                case "ATH":
                    // �������Ӵ����һ����ʵ����
                    FormJHTCZD childFormJHTCZD = new FormJHTCZD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormJHTCZD.MdiParent = this.MdiParent;

                    childFormJHTCZD.strConn = strConn;
                    childFormJHTCZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHTCZD.isSaved = true;

                    childFormJHTCZD.intUserID = intUserID;
                    childFormJHTCZD.intUserLimit = intUserLimit;
                    childFormJHTCZD.strUserLimit = strUserLimit;
                    childFormJHTCZD.strUserName = strUserName;
                    childFormJHTCZD.Show();
                    break;

                case "ATB":
                    // �������Ӵ����һ����ʵ����
                    FormJHTBJDJ childFormJHTBJDJ = new FormJHTBJDJ();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormJHTBJDJ.MdiParent = this.MdiParent;

                    childFormJHTBJDJ.strConn = strConn;
                    childFormJHTBJDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormJHTBJDJ.isSaved = true;

                    childFormJHTBJDJ.intUserID = intUserID;
                    childFormJHTBJDJ.intUserLimit = intUserLimit;
                    childFormJHTBJDJ.strUserLimit = strUserLimit;
                    childFormJHTBJDJ.strUserName = strUserName;
                    childFormJHTBJDJ.Show();
                    break;

                case "AYF":
                    // �������Ӵ����һ����ʵ����
                    FormYFZKJS childFormYFZKJS = new FormYFZKJS();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormYFZKJS.MdiParent = this.MdiParent;

                    childFormYFZKJS.strConn = strConn;
                    childFormYFZKJS.iDJID = Convert.ToInt32(sTemp1);
                    childFormYFZKJS.isSaved = true;

                    childFormYFZKJS.intUserID = intUserID;
                    childFormYFZKJS.intUserLimit = intUserLimit;
                    childFormYFZKJS.strUserLimit = strUserLimit;
                    childFormYFZKJS.strUserName = strUserName;
                    childFormYFZKJS.Show();
                    break;

                case "BKP":
                    // �������Ӵ����һ����ʵ����
                    FormXSCKZD childFormXSCKZD = new FormXSCKZD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSCKZD.MdiParent = this.MdiParent;

                    childFormXSCKZD.strConn = strConn;
                    childFormXSCKZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSCKZD.isSaved = true;

                    childFormXSCKZD.intUserID = intUserID;
                    childFormXSCKZD.intUserLimit = intUserLimit;
                    childFormXSCKZD.strUserLimit = strUserLimit;
                    childFormXSCKZD.strUserName = strUserName;
                    childFormXSCKZD.Show();
                    break;

                case "BCK":
                    // �������Ӵ����һ����ʵ����
                    FormXSCKJD childFormXSCKJD = new FormXSCKJD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSCKJD.MdiParent = this.MdiParent;

                    childFormXSCKJD.strConn = strConn;
                    childFormXSCKJD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSCKJD.isSaved = true;

                    childFormXSCKJD.intUserID = intUserID;
                    childFormXSCKJD.intUserLimit = intUserLimit;
                    childFormXSCKJD.strUserLimit = strUserLimit;
                    childFormXSCKJD.strUserName = strUserName;
                    childFormXSCKJD.Show();
                    break;

                case "BTH":
                    // �������Ӵ����һ����ʵ����
                    FormXSTHZD childFormXSTHZD = new FormXSTHZD();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSTHZD.MdiParent = this.MdiParent;

                    childFormXSTHZD.strConn = strConn;
                    childFormXSTHZD.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSTHZD.isSaved = true;

                    childFormXSTHZD.intUserID = intUserID;
                    childFormXSTHZD.intUserLimit = intUserLimit;
                    childFormXSTHZD.strUserLimit = strUserLimit;
                    childFormXSTHZD.strUserName = strUserName;
                    childFormXSTHZD.Show();
                    break;

                case "BTB":
                    // �������Ӵ����һ����ʵ����
                    FormXSTBJDJ childFormXSTBJDJ = new FormXSTBJDJ();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormXSTBJDJ.MdiParent = this.MdiParent;

                    childFormXSTBJDJ.strConn = strConn;
                    childFormXSTBJDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormXSTBJDJ.isSaved = true;

                    childFormXSTBJDJ.intUserID = intUserID;
                    childFormXSTBJDJ.intUserLimit = intUserLimit;
                    childFormXSTBJDJ.strUserLimit = strUserLimit;
                    childFormXSTBJDJ.strUserName = strUserName;
                    childFormXSTBJDJ.Show();
                    break;

                case "BYS":
                    // �������Ӵ����һ����ʵ����
                    FormYSZKJS childFormYSZKJS = new FormYSZKJS();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormYSZKJS.MdiParent = this.MdiParent;

                    childFormYSZKJS.strConn = strConn;
                    childFormYSZKJS.iDJID = Convert.ToInt32(sTemp1);
                    childFormYSZKJS.isSaved = true;

                    childFormYSZKJS.intUserID = intUserID;
                    childFormYSZKJS.intUserLimit = intUserLimit;
                    childFormYSZKJS.strUserLimit = strUserLimit;
                    childFormYSZKJS.strUserName = strUserName;
                    childFormYSZKJS.Show();
                    break;

                case "CPD":
                    // �������Ӵ����һ����ʵ����
                    FormKCSPPD2 childFormKCSPPD2 = new FormKCSPPD2();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormKCSPPD2.MdiParent = this.MdiParent;

                    childFormKCSPPD2.strConn = strConn;
                    childFormKCSPPD2.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCSPPD2.isSaved = true;

                    childFormKCSPPD2.intUserID = intUserID;
                    childFormKCSPPD2.intUserLimit = intUserLimit;
                    childFormKCSPPD2.strUserLimit = strUserLimit;
                    childFormKCSPPD2.strUserName = strUserName;
                    childFormKCSPPD2.Show();
                    break;

                case "CBS":
                    // �������Ӵ����һ����ʵ����
                    FormKCSPBSCL childFormKCSPBSCL = new FormKCSPBSCL();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormKCSPBSCL.MdiParent = this.MdiParent;

                    childFormKCSPBSCL.strConn = strConn;
                    childFormKCSPBSCL.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCSPBSCL.isSaved = true;

                    childFormKCSPBSCL.intUserID = intUserID;
                    childFormKCSPBSCL.intUserLimit = intUserLimit;
                    childFormKCSPBSCL.strUserLimit = strUserLimit;
                    childFormKCSPBSCL.strUserName = strUserName;
                    childFormKCSPBSCL.Show();
                    break;

                case "CCK":
                    // �������Ӵ����һ����ʵ����
                    FormKCJWCKDJ childFormKCJWCKDJ = new FormKCJWCKDJ();
                    // ����ʾ�ô���ǰʹ���Ϊ�� MDI ������Ӵ��塣
                    childFormKCJWCKDJ.MdiParent = this.MdiParent;

                    childFormKCJWCKDJ.strConn = strConn;
                    childFormKCJWCKDJ.iDJID = Convert.ToInt32(sTemp1);
                    childFormKCJWCKDJ.isSaved = true;

                    childFormKCJWCKDJ.intUserID = intUserID;
                    childFormKCJWCKDJ.intUserLimit = intUserLimit;
                    childFormKCJWCKDJ.strUserLimit = strUserLimit;
                    childFormKCJWCKDJ.strUserName = strUserName;
                    childFormKCJWCKDJ.Show();
                    break;
            }


        }


    }
}