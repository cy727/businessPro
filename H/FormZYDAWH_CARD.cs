using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormZYDAWH_CARD : Form
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
        
        public FormZYDAWH_CARD()
        {
            InitializeComponent();
        }

        private void FormZYDAWH_CARD_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);

            dataGridViewDA.DataSource = dt;
            dataGridViewDA.AllowUserToDeleteRows = false;
            dataGridViewDA.Columns[0].Visible = false;
            dataGridViewDA.Columns[11].Visible = false;
            dataGridViewDA.Columns[12].Visible = false;

            dataGridViewDA.Columns[4].ReadOnly = false;
            dataGridViewDA.Columns[5].ReadOnly = false;

            switch (iStyle)
            {
                case 0://����
                    btnAccept.Text = "����";
                    dataGridViewDA.AllowUserToAddRows = true;
                    break;
                case 1://�޸�
                    btnAccept.Text = "�޸�";
                    dataGridViewDA.AllowUserToAddRows = false;
                    break;
                default:
                    break;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewDA_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("�������ʹ���", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private bool countAmount()
        {
            bool bCheck = true;

            cGetInformation.ClearDataGridViewErrorText(dataGridViewDA);

            for (int i = 0; i < dataGridViewDA.Rows.Count; i++)
            {
                if (dataGridViewDA.Rows[i].IsNewRow)
                    continue;

                if (dataGridViewDA.Rows[i].Cells[1].Value.ToString() == "")
                {
                    dataGridViewDA.Rows[i].Cells[1].ErrorText = "����ְԱ���";
                    bCheck = false;
                }
                if (dataGridViewDA.Rows[i].Cells[2].Value.ToString() == "")
                {
                    dataGridViewDA.Rows[i].Cells[2].ErrorText = "����ְԱ����";
                    bCheck = false;
                }

                if (dataGridViewDA.Rows[i].Cells[11].Value.ToString() == "")
                    dataGridViewDA.Rows[i].Cells[11].Value = 0 ;

                if (dataGridViewDA.Rows[i].Cells[12].Value.ToString() == "")
                    dataGridViewDA.Rows[i].Cells[12].Value = 0;

                if (dataGridViewDA.Rows[i].Cells[11].Value.ToString() == "0")
                {
                    dataGridViewDA.Rows[i].Cells[4].ErrorText = "����ְԱ��λ";
                    bCheck = false;
                }

                if (dataGridViewDA.Rows[i].Cells[12].Value.ToString() == "0")
                {
                    dataGridViewDA.Rows[i].Cells[5].ErrorText = "����ְԱ����";
                    bCheck = false;
                }
            }

            return bCheck;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            int i;
            int i1 = 0, i2 = 0;
            string strDateSYS = "";
            System.Data.SqlClient.SqlTransaction sqlta;

            if (!countAmount())
            {
                MessageBox.Show("�������ʹ���", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            switch (iStyle)
            {
                case 0://����
                    sqlConn.Open();
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {

                        for (i = 0; i < dataGridViewDA.Rows.Count; i++)
                        {
                            if (dataGridViewDA.Rows[i].IsNewRow)
                                continue;

                            i1 = 0;
                            i2 = 0;

                            if (dataGridViewDA.Rows[i].Cells[6].Value.ToString() != "")
                                i1 = Convert.ToInt32(dataGridViewDA.Rows[i].Cells[6].Value);

                            if (dataGridViewDA.Rows[i].Cells[7].Value.ToString() != "")
                                i2 = Convert.ToInt32(dataGridViewDA.Rows[i].Cells[7].Value);

                            sqlComm.CommandText = "INSERT INTO ְԱ�� (ְԱ���, ְԱ����, �Ա�, ��λID, ����ID, �Ƿ����Ա, �Ƿ�ҵ��Ա, ���֤��, ְԱ�绰, ��ͥ��ַ, BeActive) VALUES ('" + dataGridViewDA.Rows[i].Cells[1].Value.ToString() + "', N'" + dataGridViewDA.Rows[i].Cells[2].Value.ToString() + "', N'" + dataGridViewDA.Rows[i].Cells[3].Value.ToString() + "', " + dataGridViewDA.Rows[i].Cells[11].Value.ToString() + ", " + dataGridViewDA.Rows[i].Cells[12].Value.ToString() + ", " + i1.ToString() + ", " + i2.ToString() + ", '" + dataGridViewDA.Rows[i].Cells[8].Value.ToString() + "', '" + dataGridViewDA.Rows[i].Cells[9].Value.ToString() + "', N'" + dataGridViewDA.Rows[i].Cells[10].Value.ToString() + "', 1)";
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
                    sqlta = sqlConn.BeginTransaction();
                    sqlComm.Transaction = sqlta;
                    try
                    {



                        for (i = 0; i < dataGridViewDA.Rows.Count; i++)
                        {
                            if (dataGridViewDA.Rows[i].IsNewRow)
                                continue;

                            i1 = 0;
                            i2 = 0;

                            if (dataGridViewDA.Rows[i].Cells[6].Value.ToString() != "")
                                i1 = Convert.ToInt32(dataGridViewDA.Rows[i].Cells[6].Value);

                            if (dataGridViewDA.Rows[i].Cells[7].Value.ToString() != "")
                                i2 = Convert.ToInt32(dataGridViewDA.Rows[i].Cells[7].Value);

                            sqlComm.CommandText = "UPDATE ְԱ�� SET ְԱ��� = '" + dataGridViewDA.Rows[i].Cells[1].Value.ToString() + "', ְԱ���� = N'" + dataGridViewDA.Rows[i].Cells[2].Value.ToString() + "', �Ա� = N'" + dataGridViewDA.Rows[i].Cells[3].Value.ToString() + "', ��λID = " + dataGridViewDA.Rows[i].Cells[11].Value.ToString() + ", ����ID = " + dataGridViewDA.Rows[i].Cells[12].Value.ToString() + ", �Ƿ����Ա = " + i1.ToString() + ", �Ƿ�ҵ��Ա = " + i2.ToString() + ", ���֤�� = '" + dataGridViewDA.Rows[i].Cells[8].Value.ToString() + "', ְԱ�绰 = '" + dataGridViewDA.Rows[i].Cells[9].Value.ToString() + "', ��ͥ��ַ = N'" + dataGridViewDA.Rows[i].Cells[10].Value.ToString() + "' WHERE (ID = " + dataGridViewDA.Rows[i].Cells[0].Value.ToString() + ")";
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
                    MessageBox.Show("�޸ĳɹ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    break;
                default:
                    break;
            }

        }

        private void dataGridViewDA_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 4) //��λ
            {
                if (cGetInformation.getGWInformation(1, "") == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDA);
                    dataGridViewDA.Rows[e.RowIndex].Cells[11].Value = cGetInformation.iGWNumber;
                    dataGridViewDA.Rows[e.RowIndex].Cells[4].Value = cGetInformation.strGWName;
                    dataGridViewDA.EndEdit();
                }
            }

            if (e.ColumnIndex == 5) //����
            {
                if (cGetInformation.getBMInformation(1, "") == 0) //ʧ��
                {
                    return;
                }
                else
                {
                    cGetInformation.ClearDataGridViewErrorText(dataGridViewDA);
                    dataGridViewDA.Rows[e.RowIndex].Cells[12].Value = cGetInformation.iBMNumber;
                    dataGridViewDA.Rows[e.RowIndex].Cells[5].Value = cGetInformation.strBMName;
                    dataGridViewDA.EndEdit();
                }
            }
            

        }
    }
}