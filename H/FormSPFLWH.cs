using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSPFLWH : Form
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


        private ClassGetInformation cGetInformation;

        private int iUPClass = 0;
        private string strUPClass = "0";
        private int intKFID = 0;

        private int iSelect = 0;
        
        public FormSPFLWH()
        {
            InitializeComponent();
        }

        private void FormSPFLWH_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            initCommTree();
        }
        private void initCommTree()
        {
            string strTemp;
            int iTemp;
            TreeNode nodeTemp;

            this.treeViewComm.Nodes.Clear();


            TreeNode RootNode = new TreeNode("��������", 0, 1);
            int iTagRoot = 0;
            RootNode.Tag = iTagRoot;
            this.treeViewComm.Nodes.Add(RootNode);

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʒ�����.ID, ��Ʒ�����.������, ��Ʒ�����.��������, ��Ʒ�����.�ϼ�����, ��Ʒ�����.�ⷿID, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ���� FROM ��Ʒ����� LEFT OUTER JOIN �ⷿ�� ON ��Ʒ�����.�ⷿID = �ⷿ��.ID WHERE (��Ʒ�����.BeActive = 1) ORDER BY ��Ʒ�����.�ϼ�����, ��Ʒ�����.������";
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
            RootNode.ExpandAll();

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
            this.Close();
        }

        private void treeViewComm_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DataRow[] drTemp;
            string strTemp = "";

            iSelect = (int)e.Node.Tag;
            if (iSelect != 0)  //
            {
                //�õ��ϼ�����
                drTemp = dSet.Tables["��Ʒ�����"].Select("ID=" + iSelect.ToString());
                if (drTemp.Length < 1) //û�д������
                    return;

                textBoxFLBH.Text = drTemp[0][1].ToString();
                textBoxFLMC.Text = drTemp[0][2].ToString();

                if (drTemp[0][4].ToString() == "")
                    intKFID = 0;
                else
                    intKFID = Convert.ToInt32(drTemp[0][4].ToString());
                textBoxKFBH.Text = drTemp[0][5].ToString();
                textBoxKFMC.Text = drTemp[0][6].ToString();

                //�õ��ϼ�TAG
                strTemp = drTemp[0][3].ToString();
                int iTemp = strTemp.LastIndexOf(',');
                if (iTemp != -1)
                    strTemp = strTemp.Substring(iTemp + 1);

                sqlConn.Open();
                sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ�����  FROM ��Ʒ����� WHERE (ID = " + strTemp + ")";
                sqldr = sqlComm.ExecuteReader();

                iUPClass = 0;
                strUPClass = "0";
                textBoxSJFL.Text = "ȫ������";
                while (sqldr.Read())
                {
                    iUPClass = Convert.ToInt32(sqldr.GetValue(0).ToString());
                    strUPClass = sqldr.GetValue(3).ToString() + "," + sqldr.GetValue(0).ToString();
                    textBoxSJFL.Text = sqldr.GetValue(1).ToString() + ":" + sqldr.GetValue(2).ToString();
                }
                sqldr.Close();

                sqlConn.Close();


            }
            else
            {

            }
        }

        private void textBoxKFBH_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getKFInformation(1, "") == 0)
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

        private void textBoxSJFL_DoubleClick(object sender, EventArgs e)
        {
            if (cGetInformation.getClassInformation(1, "") == 0)
            {
                //return;
            }
            else
            {
                iUPClass = cGetInformation.iClassNumber;
                sqlConn.Open();
                sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ�����  FROM ��Ʒ����� WHERE (ID = " + iUPClass.ToString() + ")";
                sqldr = sqlComm.ExecuteReader();

                while (sqldr.Read())
                {
                    strUPClass = sqldr.GetValue(3).ToString() + "," + sqldr.GetValue(0).ToString();
                    textBoxSJFL.Text = sqldr.GetValue(1).ToString() + ":" + sqldr.GetValue(2).ToString();
                }
                sqldr.Close();

                sqlConn.Close();
            }
        }

        private void btnADD_Click(object sender, EventArgs e)
        {
            int i;
            string sTemp="NULL";
            System.Data.SqlClient.SqlTransaction sqlta;

            if (textBoxFLBH.Text.Trim() == "")
            {
                MessageBox.Show("�����������", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBoxFLMC.Text.Trim() == "")
            {
                MessageBox.Show("�������������", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                if (intKFID != 0)
                    sTemp = intKFID.ToString();

                sqlComm.CommandText = "INSERT INTO ��Ʒ����� (������, ��������, �ϼ�����, �ⷿID, BeActive) VALUES (N'" + textBoxFLBH.Text + "', N'" + textBoxFLMC.Text + "', N'" + strUPClass + "', " + sTemp + ", 1)";
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
            MessageBox.Show("���ӳɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initCommTree();


        }

        private void btnDEL_Click(object sender, EventArgs e)
        {

            System.Data.SqlClient.SqlTransaction sqlta;

            if (iSelect==0)
            {
                MessageBox.Show("��ѡ�����", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                sqlComm.CommandText = "UPDATE ��Ʒ����� SET BeActive = 0 WHERE (ID = "+iSelect.ToString()+")";
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
            MessageBox.Show("ɾ���ɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initCommTree();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i;
            string sTemp = "NULL";
            System.Data.SqlClient.SqlTransaction sqlta;


            if (iSelect == 0)
            {
                MessageBox.Show("��ѡ�����", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBoxFLBH.Text.Trim() == "")
            {
                MessageBox.Show("�����������", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (textBoxFLMC.Text.Trim() == "")
            {
                MessageBox.Show("�������������", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {
                if (intKFID != 0)
                    sTemp = intKFID.ToString();

                sqlComm.CommandText = "UPDATE ��Ʒ����� SET ������ = N'" + textBoxFLBH.Text + "', �������� = N'" + textBoxFLMC.Text + "', �ϼ����� = N'"+strUPClass+"', �ⷿID = "+sTemp+" WHERE (ID = "+iSelect.ToString()+")";
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
            initCommTree();

        }   

    }
}