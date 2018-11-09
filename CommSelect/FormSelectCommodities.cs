using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormSelectCommodities : Form
    {
        public string strConn = "";
        public string strSelectText = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iCommNumber = 0;
        public string strCommName = "";
        public string strCommCode = "";
        public string strCommGG = "";
        public string strCommCount = "";
        public decimal decCommKCCBJ;
        public decimal decCommHSCBJ;
        public decimal decCommZZJJ;
        public decimal decCommJJ; 
        public decimal decCommPFJ;
        public decimal decCommZGJJ;
        public decimal decCommZDJJ;
        public decimal decCommKCSL;

        private DataView dvCommSelect;
        
        public FormSelectCommodities()
        {
            InitializeComponent();
        }

        private void FormSelectCommodities_Load(object sender, EventArgs e)
        {
            if (strSelectText == "")
            {
                this.Close();
            }

            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;

            //��ʼ���б�
            sqlComm.CommandText = strSelectText;
            sqlConn.Open();
            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");

            //dataGridViewComm.DataSource = dSet.Tables["��Ʒ��"];
            dvCommSelect = new DataView(dSet.Tables["��Ʒ��"]);
            dataGridViewComm.DataSource = dvCommSelect;
            sqlConn.Close();

            dataGridViewComm.Columns[0].Visible = false;
            dataGridViewComm.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[8].Visible = false;
            dataGridViewComm.Columns[9].Visible = false;

            dataGridViewComm.Columns[5].Visible = false;
            dataGridViewComm.Columns[6].Visible = false;
            dataGridViewComm.Columns[7].Visible = false;
            dataGridViewComm.Columns[10].Visible = false;
            //dataGridViewComm.Columns[11].Visible = false;
            dataGridViewComm.Columns[12].Visible = false;
            dataGridViewComm.Columns[13].Visible = false;
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
            sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ����� FROM ��Ʒ����� WHERE (BeActive = 1) ORDER BY �ϼ�����, ������";
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
                if(iTemp!=-1)
                    strTemp = strTemp.Substring(iTemp+1);

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
                iValue=(int)node.Tag;

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
            iCommNumber = -1;
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dataGridViewComm.SelectedRows.Count < 1)
            {
                iCommNumber = 0;
                this.Close();
                return; ;
            }

            iCommNumber = Int32.Parse(dataGridViewComm.SelectedRows[0].Cells[0].Value.ToString());
            strCommName = dataGridViewComm.SelectedRows[0].Cells[1].Value.ToString();
            strCommCode = dataGridViewComm.SelectedRows[0].Cells[2].Value.ToString();
            strCommGG = dataGridViewComm.SelectedRows[0].Cells[3].Value.ToString();
            strCommCount = dataGridViewComm.SelectedRows[0].Cells[4].Value.ToString();
            if(dataGridViewComm.SelectedRows[0].Cells[5].Value.ToString().Trim()=="")
                decCommKCCBJ=0;
            else
                decCommKCCBJ=Decimal.Parse(dataGridViewComm.SelectedRows[0].Cells[5].Value.ToString().Trim());

            if(dataGridViewComm.SelectedRows[0].Cells[6].Value.ToString().Trim()=="")
                decCommHSCBJ=0;
            else
                decCommHSCBJ=Decimal.Parse(dataGridViewComm.SelectedRows[0].Cells[6].Value.ToString().Trim());

            if(dataGridViewComm.SelectedRows[0].Cells[7].Value.ToString().Trim()=="")
                decCommZZJJ=0;
            else
                decCommZZJJ=Decimal.Parse(dataGridViewComm.SelectedRows[0].Cells[7].Value.ToString().Trim());

            if (dataGridViewComm.SelectedRows[0].Cells[10].Value.ToString().Trim() == "")
                decCommJJ = 0;
            else
                decCommJJ = Decimal.Parse(dataGridViewComm.SelectedRows[0].Cells[10].Value.ToString().Trim());

            if (dataGridViewComm.SelectedRows[0].Cells[11].Value.ToString().Trim() == "")
                decCommPFJ = 0;
            else
                decCommPFJ = Decimal.Parse(dataGridViewComm.SelectedRows[0].Cells[11].Value.ToString().Trim());

            if (dataGridViewComm.SelectedRows[0].Cells[12].Value.ToString().Trim() == "")
                decCommZGJJ = 0;
            else
                decCommZGJJ = Decimal.Parse(dataGridViewComm.SelectedRows[0].Cells[12].Value.ToString().Trim());

            if (dataGridViewComm.SelectedRows[0].Cells[13].Value.ToString().Trim() == "")
                decCommZDJJ = 0;
            else
                decCommZDJJ = Decimal.Parse(dataGridViewComm.SelectedRows[0].Cells[13].Value.ToString().Trim());

            if (dataGridViewComm.SelectedRows[0].Cells[14].Value.ToString().Trim() == "")
                decCommKCSL = 0;
            else
                decCommKCSL = Decimal.Parse(dataGridViewComm.SelectedRows[0].Cells[14].Value.ToString().Trim());

            this.Close();
        }

        private void dataGridViewComm_DoubleClick(object sender, EventArgs e)
        {
            btnSelect_Click(null, null);
        }

        private void treeViewComm_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int iSelect = 0;
            DataRow[] drTemp;
            string strTemp = "";

            

            iSelect = (int)e.Node.Tag;
            if (iSelect == 0)  //ѡȡȫ����Ʒ
            {
                dvCommSelect.RowFilter="";
            }
            else
            {
                //�õ��ϼ�����
                drTemp = dSet.Tables["��Ʒ�����"].Select("ID=" + iSelect.ToString());
                if (drTemp.Length < 1) //û�д������
                    return;
                strTemp = dSet.Tables["��Ʒ�����"].Rows[0][3].ToString().Trim();
                dvCommSelect.RowFilter = "������=" + iSelect.ToString() + " OR �ϼ����� = '" + strTemp + "," + iSelect.ToString() + "'";
            }

            dataGridViewComm.DataSource = dvCommSelect;
            
            dataGridViewComm.Columns[0].Visible = false;
            dataGridViewComm.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewComm.Columns[8].Visible = false;
            dataGridViewComm.Columns[9].Visible = false;
            
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            dvCommSelect.RowFilter = "";
            dvCommSelect.RowStateFilter = DataViewRowState.CurrentRows;
            dataGridViewComm.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (textBoxMC.Text.Trim() == "")
                return;

            dvCommSelect.RowStateFilter = DataViewRowState.CurrentRows;
            if (radioButtonAll.Checked)
            {
                dvCommSelect.RowFilter = "��Ʒ���� LIKE '%" + textBoxMC.Text.Trim().ToUpper() + "%'";
            }
            if (radioButtonF.Checked)
            {
                dvCommSelect.RowFilter = "��Ʒ���� LIKE '" + textBoxMC.Text.Trim().ToUpper() + "%'";
            }
            if (radioButtonE.Checked)
            {
                dvCommSelect.RowFilter = "��Ʒ���� LIKE '%" + textBoxMC.Text.Trim().ToUpper() + "'";
            }
            dataGridViewComm.Focus();
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            if (textBoxMC.Text.Trim() == "")
                return;

            int iRow = -1;
            string sTemp = "";

            for (int i = 0; i < dataGridViewComm.Rows.Count; i++)
            {
                if (radioButtonAll.Checked)  //ȫƥ��
                {
                    sTemp = dataGridViewComm.Rows[i].Cells[1].Value.ToString();
                    if (sTemp.IndexOf(textBoxMC.Text.Trim()) != -1)
                    {
                        iRow = i;
                        break;
                    }
                }

                if (radioButtonF.Checked) //ǰƥ��
                {
                    sTemp = dataGridViewComm.Rows[i].Cells[1].Value.ToString();
                    if (sTemp.IndexOf(textBoxMC.Text.Trim()) == 0)
                    {
                        iRow = i;
                        break;
                    }
                }

                if (radioButtonE.Checked) //��ƥ��
                {
                    sTemp = dataGridViewComm.Rows[i].Cells[1].Value.ToString().Trim();
                    if (sTemp.Length < textBoxMC.Text.Trim().Length)
                        break;

                    if (sTemp.LastIndexOf(textBoxMC.Text.Trim()) == sTemp.Length - textBoxMC.Text.Trim().Length)
                    {
                        iRow = i;
                        break;
                    }
                }
                dataGridViewComm.Focus();


            }


            if (iRow != -1)
            {
                //dataGridViewDWLB.Rows[iRow].Selected = false;
                dataGridViewComm.Rows[iRow].Selected = true;
                dataGridViewComm.FirstDisplayedScrollingRowIndex = iRow;
            }
            else
            {
                if (dataGridViewComm.Rows.Count > 0)
                {
                    dataGridViewComm.Rows[0].Selected = true;
                    dataGridViewComm.FirstDisplayedScrollingRowIndex = 0;
                }
            }

        }

        protected override bool ProcessCmdKey(ref   Message msg, Keys keyData)
        {
            if (keyData == Keys.F9)
            {
                btnAll_Click(null, null);
                return true;
            }
            if (keyData == Keys.F7)
            {
                btnSearch_Click(null, null);
                return true;
            }
            if (keyData == Keys.F8)
            {
                btnLocation_Click(null, null);
                return true;
            }
            if (keyData == Keys.Enter && dataGridViewComm.Focused)
            {
                //System.Windows.Forms.SendKeys.Send("{tab}");
                btnSelect_Click(null, null);//
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void textBoxMC_TextChanged(object sender, EventArgs e)
        {
            textBoxMC.Text = textBoxMC.Text.ToUpper();
        }



        private void textBoxMC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                btnSearch_Click(null, null);
            }
        }
    }
}