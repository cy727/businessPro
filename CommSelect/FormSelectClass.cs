using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSelectClass : Form
    {
        public string strConn = "";
        public string strSelectText = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iClassNumber = 0;
        public string strClassName = "";
        public string strClassCode = "";

        public FormSelectClass()
        {
            InitializeComponent();
        }

        private void FormSelectClass_Load(object sender, EventArgs e)
        {
            if (strSelectText == "")
            {
                this.Close();
            }

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            initCommTree();
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
            sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ����� FROM ��Ʒ����� WHERE (BeActive = 1) ORDER BY �ϼ�����, ������ ";
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

            this.treeViewComm.SelectedNode = RootNode;

            

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iClassNumber = 0;
            this.Close();
        }

        private void treeViewComm_AfterSelect(object sender, TreeViewEventArgs e)
        {
            iClassNumber = (int)e.Node.Tag;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (iClassNumber == 0)
                this.Close();

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ������, ��������, �ϼ����� FROM ��Ʒ����� WHERE (ID = " + iClassNumber.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                strClassCode = sqldr.GetValue(0).ToString();
                strClassName = sqldr.GetValue(1).ToString();
            }
            sqldr.Close();
            sqlConn.Close();

            this.Close();
        }

        private void treeViewComm_DoubleClick(object sender, EventArgs e)
        {
            btnSelect_Click(null, null);
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

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && treeViewComm.Focused)
            {
                //System.Windows.Forms.SendKeys.Send("{tab}");
                btnSelect_Click(null, null);//
                return true;
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}