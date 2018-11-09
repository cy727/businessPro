using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPDAWH_CARD : Form
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
        private int iClass = 0;
        public int iSelect = 0;
        
        public FormSPDAWH_CARD()
        {
            InitializeComponent();
        }

        private void FormSPDAWH_CARD_Load(object sender, EventArgs e)
        {
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
            initCommTree();

            if (iStyle == 1) //�޸�
            {
                textBoxSPBH.TextChanged -= textBoxSPBH_TextChanged;
                //��Ʒ��.ID, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ����, ��Ʒ��.������, ��Ʒ��.��С������λ, ��Ʒ��.����, ��Ʒ��.������, ��Ʒ��.��¼����, ��Ʒ��.��Ʒ���, ��Ʒ��.�������, ��Ʒ��.�������, ��Ʒ��.����������, ��Ʒ��.����������, ��Ʒ�����.��������, ��Ʒ��.������ AS ����ID, ��Ʒ�����.������
                textBoxSPBH.Text = dt.Rows[0][1].ToString();
                textBoxSPMC.Text = dt.Rows[0][2].ToString();
                textBoxZJM.Text = dt.Rows[0][3].ToString();
                textBoxZXJLDW.Text = dt.Rows[0][4].ToString();
                textBoxSPGG.Text = dt.Rows[0][8].ToString();

                try
                {
                    dateTimePickerDLRQ.Value = DateTime.Parse(dt.Rows[0][7].ToString());
                }
                catch
                {
                    dateTimePickerDLRQ.Value = DateTime.Now;
                }

                try
                {
                    iClass = Convert.ToInt32(dt.Rows[0][14].ToString());

                    numericUpDownJJ.Value = Convert.ToDecimal(dt.Rows[0][5].ToString());
                    numericUpDownPFJ.Value = Convert.ToDecimal(dt.Rows[0][6].ToString());

                    numericUpDownKCSX.Value = Convert.ToDecimal(dt.Rows[0][9].ToString());
                    numericUpDownKCXX.Value = Convert.ToDecimal(dt.Rows[0][10].ToString());

                    numericUpDownHLSX.Value = Convert.ToDecimal(dt.Rows[0][11].ToString());
                    numericUpDownHLSX.Value = Convert.ToDecimal(dt.Rows[0][12].ToString());
                }
                catch
                {
                    
                }

                if (iClass != 0)
                {
                    TreeNode nodeTemp = FindTreeNodeByDepth(this.treeViewComm.Nodes, iClass);
                    this.treeViewComm.SelectedNode = nodeTemp;
                }

                textBoxSPBH.TextChanged += textBoxSPBH_TextChanged;


            }
            textBoxSPMC.TextChanged += textBoxSPMC_TextChanged;
        }
        private void initCommTree()
        {
            string strTemp;
            int iTemp;
            TreeNode nodeTemp;

            TreeNode RootNode = new TreeNode("��������", 0, 1);
            int iTagRoot = 0;
            RootNode.Tag = iTagRoot;
            this.treeViewComm.Nodes.Add(RootNode);

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ����� FROM ��Ʒ����� WHERE BeActive = 1 ORDER BY �ϼ�����, ������";
            if (dSet.Tables.Contains("��Ʒ�����")) dSet.Tables.Remove("��Ʒ�����");
            sqlDA.Fill(dSet, "��Ʒ�����");
            for (int i = 0; i < dSet.Tables["��Ʒ�����"].Rows.Count; i++)
            {
                int iTag;

                if (dSet.Tables["��Ʒ�����"].Rows[i][3].ToString() == "")
                    continue;
                strTemp = dSet.Tables["��Ʒ�����"].Rows[i][3].ToString();
                //�õ��ϼ�TAG
                iTemp = strTemp.LastIndexOf(',');
                if (iTemp != -1)
                    strTemp = strTemp.Substring(iTemp + 1);

                nodeTemp = FindTreeNodeByDepth(this.treeViewComm.Nodes, Int32.Parse(strTemp));
                TreeNode nT = new TreeNode(dSet.Tables["��Ʒ�����"].Rows[i][2].ToString(), 0, 1);
                iTemp = Int32.Parse(dSet.Tables["��Ʒ�����"].Rows[i][0].ToString());
                nT.Tag = iTemp;
                nodeTemp.Nodes.Add(nT);

            }
            sqlConn.Close();
            RootNode.Expand();

        }
        private TreeNode FindTreeNodeByDepth(TreeNodeCollection p_treeNodes, int p_i)
        {
            TreeNode treeNodeReturn = null;
            int iValue;

            foreach (TreeNode node in p_treeNodes)
            {
                //ȡ��ǰ�ڵ��   
                iValue = (int)node.Tag;

                //�������ֵ��   
                if (iValue == p_i)
                    treeNodeReturn = node;

                //�ҵ����˳�   
                if (treeNodeReturn != null)
                    break;
                else
                {
                    //������Ȳ�ѯ   
                    if (node.Nodes.Count > 0)
                    {
                        treeNodeReturn = FindTreeNodeByDepth(node.Nodes, p_i);
                    }
                }

            }

            return treeNodeReturn;
        } 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iSelect = 0;
            this.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            
            int i;
            int i1 = 0, i2 = 0;
            string strDateSYS = "";
            System.Data.SqlClient.SqlTransaction sqlta;

            if (textBoxSPBH.Text.Trim()=="")
            {
                MessageBox.Show("�������ʹ�����������Ʒ���", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBoxSPMC.Text.Trim() == "")
            {
                MessageBox.Show("�������ʹ�����������Ʒ����", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (iClass == 0)
            {
                MessageBox.Show("�������ʹ�����ѡ����Ʒ����", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            switch (iStyle)
            {
                case 0://����
                    sqlConn.Open();

                    //����
                    if (textBoxSPBH.Text.Trim() == "")
                    {
                        MessageBox.Show("��������Ʒ���");
                        sqlConn.Close();
                        break;
                    }
                    sqlComm.CommandText = "SELECT ID, ��Ʒ���� FROM ��Ʒ�� WHERE (��Ʒ��� = '" + textBoxSPBH.Text.Trim() + "')";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        MessageBox.Show("��Ʒ���" + textBoxSPBH.Text.Trim() + "�ظ�������Ϊ��" + sqldr.GetValue(1).ToString());
                        sqldr.Close();
                        sqlConn.Close();
                        break;
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT ID, ��Ʒ��� FROM ��Ʒ�� WHERE (��Ʒ���� = '" + textBoxSPMC.Text.Trim() + "')";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        if (MessageBox.Show("��Ʒ����" + textBoxSPMC.Text.Trim() + "�ظ������Ϊ��" + sqldr.GetValue(1).ToString() + "���Ƿ������", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
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

                        //�õ�����������
                        sqlComm.CommandText = "SELECT GETDATE() AS ����";
                        sqldr = sqlComm.ExecuteReader();

                        while (sqldr.Read())
                        {
                            strDateSYS = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
                        }
                        sqldr.Close();


                        sqlComm.CommandText = "INSERT INTO ��Ʒ�� (��Ʒ���, ��Ʒ����, ������, ��С������λ, ����, ��˰����, ������, ��˰������, �������, ���ɱ���, �����, ������, ��߽���, ��ͽ���, ���ս���, ��ת����, ��ת����, ��ת���, ��ת����, ��¼����, �������, �������, ����������, ����������, ��װ��Ʒ, beactive, Ӧ�����, �Ѹ����, Ӧ�ս��, ���ս��, ������, ��Ʒ���) VALUES (N'" + textBoxSPBH.Text.Trim() + "', N'" + textBoxSPMC.Text.Trim() + "', N'" + textBoxZJM.Text.Trim() + "', N'" + textBoxZXJLDW.Text.Trim() + "', " + numericUpDownJJ.Value.ToString() + ", " + numericUpDownJJ.Value.ToString() + ", " + numericUpDownPFJ.Value.ToString() + ", " + numericUpDownPFJ.Value.ToString() + ", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '" + dateTimePickerDLRQ.Value.ToShortDateString() + "', " + numericUpDownKCSX.Value.ToString() + ", " + numericUpDownKCXX.Value.ToString() + ", " + numericUpDownHLSX.Value.ToString() + ", " + numericUpDownHLXX.Value.ToString() + ", 0, 1, 0, 0, 0, 0, " + iClass.ToString() + ", N'" + textBoxSPGG.Text.Trim() + "')";
                        sqlComm.ExecuteNonQuery();

                        sqlComm.CommandText = "SELECT @@IDENTITY";
                        sqldr = sqlComm.ExecuteReader();
                        sqldr.Read();
                        iSelect = Convert.ToInt32(sqldr.GetValue(0).ToString());
                        sqldr.Close();

                        //���ӿ��
                        sqlComm.CommandText = "SELECT �ⷿID FROM ��Ʒ����� WHERE (ID = " + iClass.ToString() + ")";
                        sqldr = sqlComm.ExecuteReader();
                        sqldr.Read();
                        string sKF = sqldr.GetValue(0).ToString();
                        sqldr.Close();

                        if(sKF != "")
                        {
                            sqlComm.CommandText = "INSERT INTO ���� (�ⷿID, ��ƷID, �������, �����, ���ɱ���, ����ɱ���, �������, �������, ����������, ����������, Ӧ�����, �Ѹ����, Ӧ�ս��, ���ս��, BeActive) VALUES (" + sKF + ", " + iSelect.ToString() + ", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)";
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
                    MessageBox.Show("���ӳɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    break;
                case 1://�޸�

                    sqlConn.Open();

                    //����
                    if (textBoxSPBH.Text.Trim() == "")
                    {
                        MessageBox.Show("��������Ʒ���");
                        sqlConn.Close();
                        break;
                    }
                    iSelect = Convert.ToInt32(dt.Rows[0][0].ToString());

                    sqlComm.CommandText = "SELECT ID, ��Ʒ���� FROM ��Ʒ�� WHERE (��Ʒ��� = '" + textBoxSPBH.Text.Trim() + "' AND ID <> " + iSelect.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        MessageBox.Show("��Ʒ���" + textBoxSPBH.Text.Trim() + "�ظ�������Ϊ��" + sqldr.GetValue(1).ToString());
                        sqldr.Close();
                        sqlConn.Close();
                        break;
                    }
                    sqldr.Close();

                    sqlComm.CommandText = "SELECT ID, ��Ʒ��� FROM ��Ʒ�� WHERE (��Ʒ���� = '" + textBoxSPMC.Text.Trim() + "' AND ID <> " + iSelect.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        if (MessageBox.Show("��Ʒ����" + textBoxSPMC.Text.Trim() + "�ظ������Ϊ��" + sqldr.GetValue(1).ToString() + "���Ƿ������", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        {
                            sqldr.Close();
                            sqlConn.Close();
                            break;
                        }
                    }
                    sqldr.Close();

                    //ʹ��״̬
                    sqlComm.CommandText = "SELECT DISTINCT ��Ʒ��.��Ʒ���� FROM ������ϸ������ͼ INNER JOIN ��Ʒ�� ON ������ϸ������ͼ.��ƷID = ��Ʒ��.ID WHERE (������ϸ������ͼ.BeActive = 1) AND (������ϸ������ͼ.��ƷID = " + iSelect.ToString() + ")";
                    sqldr = sqlComm.ExecuteReader();

                    if (sqldr.HasRows)
                    {
                        sqldr.Read();
                        if (textBoxSPMC.Text.Trim() != sqldr.GetValue(0).ToString())
                            MessageBox.Show("����Ʒ���е��ݱ��棬���ɸ�����Ʒ���ƣ�" + sqldr.GetValue(0).ToString() + "��", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBoxSPMC.Text = sqldr.GetValue(0).ToString();
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

                        iSelect = Convert.ToInt32(dt.Rows[0][0].ToString());
                        sqlComm.CommandText = "UPDATE ��Ʒ�� SET ��Ʒ��� = N'" + textBoxSPBH.Text.Trim() + "', ��Ʒ���� = N'" + textBoxSPMC.Text.Trim() + "', ������ = N'" + textBoxZJM.Text.Trim() + "', ��С������λ = N'" + textBoxZXJLDW.Text.Trim() + "', ���� = " + numericUpDownJJ.Value.ToString() + ", ��˰���� = " + numericUpDownJJ.Value.ToString() + ", ������ = " + numericUpDownPFJ.Value.ToString() + ", ��˰������ = " + numericUpDownPFJ.Value.ToString() + ", ��¼���� = '" + dateTimePickerDLRQ.Value.ToShortDateString() + "', ������� = " + numericUpDownKCSX.Value.ToString() + ", ������� = " + numericUpDownKCXX.Value.ToString() + ", ���������� = " + numericUpDownHLSX.Value.ToString() + ", ���������� = " + numericUpDownHLXX.Value.ToString() + ", ������ = " + iClass.ToString() + ", ��Ʒ��� = N'"+textBoxSPGG.Text.Trim()+"' WHERE (ID = " + dt.Rows[0][0].ToString() + ")";
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

        private void textBoxSPMC_TextChanged(object sender, EventArgs e)
        {
            int ii = textBoxSPMC.Text.Trim().IndexOf('-');

            if (ii == -1 || ii == textBoxSPMC.Text.Trim().Length - 1)
            {
                textBoxZJM.Text = cGetInformation.convertPYSM(textBoxSPMC.Text);
                return;
            }

            int i1, i2, i3, i4;
            if (ii < 3)
            {
                i1 = 0;
                i2 = ii;
            }
            else
            {
                i1 = ii - 3;
                i2 = 3;
            }

            i3 = ii + 1;
            if (ii + 4 > textBoxSPMC.Text.Trim().Length)
            {
                i4 = textBoxSPMC.Text.Trim().Length - ii - 1;
            }
            else
            {
                i4 = 3;
            }

            string ss = textBoxSPMC.Text.Trim().Substring(i1, i2) + textBoxSPMC.Text.Trim().Substring(i3, i4);
            textBoxZJM.Text = cGetInformation.convertPYSM(ss);

                
        }

        private void treeViewComm_AfterSelect(object sender, TreeViewEventArgs e)
        {
            iClass = (int)e.Node.Tag;
        }

        private void textBoxSPBH_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSPBH.Text.Trim().Length < 2)
                return;

            int iNode = 0;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ID FROM ��Ʒ����� WHERE (BeActive = 1) AND (������ = '" + textBoxSPBH.Text.Substring(0,2)+ "')";
            sqldr = sqlComm.ExecuteReader();

            if (sqldr.HasRows)
            {
                sqldr.Read();
                iNode = int.Parse(sqldr.GetValue(0).ToString());
            }
            sqlConn.Close();

            if (iNode == 0)
                return;

            TreeNode nodeTemp = FindTreeNodeByDepth(this.treeViewComm.Nodes, iNode);
            this.treeViewComm.SelectedNode = nodeTemp;



        }


    }
}