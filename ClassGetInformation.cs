using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace business
{
    class ClassGetInformation
    {
        public string strConn = "";

        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public int iCompanyNumber = 0;
        public string strCompanyName = "";
        public string strCompanyCode = "";
        public string strCompanySH = "";
        public decimal dCompanyYFZK = 0;
        public decimal dCompanyYSZK = 0;
        public string sCompanyYWY = "";
        public int iBMID = 0;

        public int iVersion = 1;

        public ClassGetInformation(string strConnectionString)
        {
            strConn = strConnectionString;
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
        }

        public void initClassGetInformation()
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
        }

        //iStyle:1,���й�Ӧ��λ, 2,��������λ, 10,���������빩Ӧ��λ, 20,��������������λ,11,���е�λ��Ź�Ӧ��λ, 21,���е�λ�������λ,  100,���е�λ,110,���������뵥λ,,120,���е�λ��ŵ�λ,1000,���е�λ��1100,���е�λ��ŵ�λ, 1200,���������뵥λ,
        //����: �ɹ�1, ʧ��0
        public int getCompanyInformation(int iStyle, string strZJM)
        {
            strZJM = strZJM.ToUpper();
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID FROM ��λ�� WHERE (�Ƿ���� = 1) AND (BeActive = 1)";
                    break;
                case 2:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID  FROM ��λ�� WHERE (�Ƿ����� = 1) AND (BeActive = 1)";
                    break;
                case 10:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID FROM ��λ�� WHERE (�Ƿ���� = 1) AND (BeActive = 1) AND (������ LIKE '%" + strZJM + "%') OR (��λ��� LIKE '" + strZJM + "%') ";
                    break;
                case 11:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID FROM ��λ�� WHERE (�Ƿ���� = 1) AND (��λ��� LIKE '%" + strZJM + "%') AND (BeActive = 1)";
                    break;
                case 12:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID FROM ��λ�� WHERE (�Ƿ���� = 1) AND ((������ LIKE '%" + strZJM + "%') OR (��λ���� LIKE '%" + strZJM + "%')) AND (BeActive = 1)";
                    break;
                case 20:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID FROM ��λ�� WHERE (�Ƿ����� = 1)  AND (BeActive = 1) AND (������ LIKE '%" + strZJM + "%') OR (��λ��� LIKE '" + strZJM + "%')";
                    break;
                case 21:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID FROM ��λ�� WHERE (�Ƿ����� = 1) AND (��λ��� LIKE '%" + strZJM + "%') AND (BeActive = 1)";
                    break;
                case 22:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID FROM ��λ�� WHERE (�Ƿ����� = 1) AND ((������ LIKE '%" + strZJM + "%') OR (��λ���� LIKE '%" + strZJM + "%')) AND (BeActive = 1)";
                    break;
                case 100:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID FROM ��λ�� WHERE (BeActive = 1) AND (BeActive = 1)";
                    break;
                case 110:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա FROM ��λ�� WHERE (BeActive = 1) AND (������ LIKE '%" + strZJM + "%')OR (��λ��� LIKE '" + strZJM + "%')"; 
                    break;
                case 121:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID  FROM ��λ�� WHERE (BeActive = 1) AND (��λ��� LIKE '%" + strZJM + "%') AND (BeActive = 1)";
                    break;
                case 120:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID  FROM ��λ�� WHERE (BeActive = 1) AND ((������ LIKE '%" + strZJM + "%') OR (��λ���� LIKE '%" + strZJM + "%')) AND (BeActive = 1)";
                    break;
                case 1000:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID  FROM ��λ�� WHERE (BeActive = 1)";
                    break;
                case 1100:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID  FROM ��λ�� WHERE (��λ��� LIKE '%" + strZJM + "%') AND (BeActive = 1)"; 
                    break;
                case 1200:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID  FROM ��λ�� WHERE (������ LIKE '%" + strZJM + "%') OR (��λ���� LIKE '%" + strZJM + "%') AND (BeActive = 1) ";
                    break;
                case 1300:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID  FROM ��λ�� WHERE (������ LIKE '%" + strZJM + "%') OR (��λ���� LIKE '%" + strZJM + "%') AND (BeActive = 1)";
                    break;
                case 41000: //ɾ����λ
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID  FROM ��λ�� WHERE (BeActive <> 1)";
                    break;
                case 41200://ɾ����ŵ�λ
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ����, ˰��, Ӧ���˿�, Ӧ���˿�, ҵ��Ա, ����ID  FROM ��λ�� WHERE ((������ LIKE '%" + strZJM + "%') OR (��λ���� LIKE '%" + strZJM + "%')) AND (BeActive <> 1) ";
                    break;
                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("��λ��")) dSet.Tables.Remove("��λ��");
            sqlDA.Fill(dSet, "��λ��");

            if (dSet.Tables["��λ��"].Rows.Count < 1) //û�е�λ
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["��λ��"].Rows.Count == 1) //ֻ��һ����λ
            {
                iCompanyNumber = Int32.Parse(dSet.Tables["��λ��"].Rows[0][0].ToString());
                strCompanyName = dSet.Tables["��λ��"].Rows[0][2].ToString();
                strCompanyCode = dSet.Tables["��λ��"].Rows[0][1].ToString();
                strCompanySH = dSet.Tables["��λ��"].Rows[0][3].ToString();
                if (dSet.Tables["��λ��"].Rows[0][5].ToString() != "")
                    dCompanyYSZK = Convert.ToDecimal(dSet.Tables["��λ��"].Rows[0][5].ToString());
                if (dSet.Tables["��λ��"].Rows[0][4].ToString() != "")
                    dCompanyYFZK = Convert.ToDecimal(dSet.Tables["��λ��"].Rows[0][4].ToString());

                sCompanyYWY = dSet.Tables["��λ��"].Rows[0][6].ToString();
                try
                {
                    iBMID = int.Parse(dSet.Tables["��λ��"].Rows[0][7].ToString());
                }
                catch
                {
                    iBMID = 0;
                }
                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //�����λ
            FormSelectComPany frmSelectComPany = new FormSelectComPany();
            frmSelectComPany.strConn = strConn;
            frmSelectComPany.strSelectText = sqlComm.CommandText;
            frmSelectComPany.ShowDialog();

            if (frmSelectComPany.iCompanyNumber == 0)
                return 0;
            else
            {
                iCompanyNumber = frmSelectComPany.iCompanyNumber;
                strCompanyName = frmSelectComPany.strCompanyName;
                strCompanyCode = frmSelectComPany.strCompanyCode;
                strCompanySH = frmSelectComPany.strCompanySH;
                dCompanyYSZK = frmSelectComPany.dCompanyYSZK;
                dCompanyYFZK = frmSelectComPany.dCompanyYFZK;
                sCompanyYWY = frmSelectComPany.sCompanyYWY;
                iBMID = frmSelectComPany.iBMID;

                sqlConn.Close();
                return 1;
            }
        }

        public int iClassNumber = 0;
        public string strClassName = "";
        public string strClassCode = "";
        //iStyle:1,�������, 10,�������������, 20,���б�����
        //����: �ɹ�1, ʧ��0
        public int getClassInformation(int iStyle, string strZJM)
        {
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ����� FROM ��Ʒ����� WHERE BeActive = 1 ";
                    break;
                case 10:
                    sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ����� FROM ��Ʒ����� WHERE (������ LIKE '%" + strZJM + "%') AND  BeActive = 1";
                    break;
                case 20:
                    sqlComm.CommandText = "SELECT ID, ������, ��������, �ϼ����� FROM ��Ʒ����� WHERE (�������� LIKE '%" + strZJM + "%') AND  BeActive = 1";
                    break;

                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("��Ʒ�����")) dSet.Tables.Remove("��Ʒ�����");
            sqlDA.Fill(dSet, "��Ʒ�����");

            if (dSet.Tables["��Ʒ�����"].Rows.Count < 1) //û����Ʒ�����
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["��Ʒ�����"].Rows.Count == 1) //ֻ��һ����Ʒ�����
            {
                iClassNumber = Int32.Parse(dSet.Tables["��Ʒ�����"].Rows[0][0].ToString());
                strClassName = dSet.Tables["��Ʒ�����"].Rows[0][2].ToString();
                strClassCode = dSet.Tables["��Ʒ�����"].Rows[0][1].ToString();

                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //�����Ʒ�����
            FormSelectClass frmSelectClass = new FormSelectClass();
            frmSelectClass.strConn = strConn;
            frmSelectClass.strSelectText = sqlComm.CommandText;
            frmSelectClass.ShowDialog();

            if (frmSelectClass.iClassNumber == 0)
                return 0;
            else
            {
                iClassNumber = frmSelectClass.iClassNumber;
                strClassName = frmSelectClass.strClassName;
                strClassCode = frmSelectClass.strClassCode;
                sqlConn.Close();
                return 1;
            }

        }


        //
        public int intUnderClassNumber = 0;
        public int[] intUnderClass=new int[100];
        public int intUpClassNumber = 0;

        //�õ������¼�����
        public void getUnderClassInformation(int iClassNumber)
        {
            intUnderClassNumber = 0;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ID FROM ��Ʒ����� WHERE (�ϼ����� = '0," + iClassNumber.ToString() + "')";

            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                if (intUnderClassNumber >= 100)
                    break;
                intUnderClass[intUnderClassNumber]=int.Parse(sqldr.GetValue(0).ToString());
                intUnderClassNumber++;
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT �ϼ����� FROM ��Ʒ����� WHERE (ID = " + iClassNumber.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            string sTem="";
            while (sqldr.Read())
            {
                sTem=sqldr.GetValue(0).ToString();
            }
            if (sTem.LastIndexOf(',') == 0)
            {
                try
                {
                    intUpClassNumber = int.Parse(sTem);
                }
                catch
                {
                    intUpClassNumber = 0;
                }
            }
            else
            {
                sTem = sTem.Substring(sTem.LastIndexOf(',')+1, sTem.Length - sTem.LastIndexOf(',')-1);
                try
                {
                    intUpClassNumber = int.Parse(sTem);
                }
                catch
                {
                    intUpClassNumber = 0;
                }
            }
            sqlConn.Close();
        }


        public int iCommNumber = 0;
        public string strCommName = "";
        public string strCommCode = "";
        public string strCommCount = "";
        public string strCommGG = "";
        public decimal decCommKCCBJ;
        public decimal decCommHSCBJ;
        public decimal decCommZZJJ;
        public decimal decCommJJ=0;
        public decimal decCommPFJ=0;
        public decimal decCommZGJJ = 0;
        public decimal decCommZDJJ = 0;
        public decimal decCommKCSL=0;


        //iStyle:1,������Ʒ, 10,������������Ʒ, 20,���б����Ʒ, 30, ģ����ѯ, 40, ����ID��Ʒ��101�����������Ʒ��102�����������������Ʒ
        //����: �ɹ�1, ʧ��0
        public int getCommInformation(int iStyle, string strZJM)
        {
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ��Ʒ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, CONVERT(numeric(8, 2), ROUND(��Ʒ��.���ɱ���, 2)) AS ���ɱ���, CONVERT(numeric(8, 2), ROUND(��Ʒ��.����ɱ���, 2)) AS ����ɱ���, CONVERT(numeric(8, 2), ROUND(��Ʒ��.���ս���, 2)) AS ���ս���, ��Ʒ��.������, ��Ʒ�����.�ϼ�����, ��Ʒ��.����, ��Ʒ��.������, ��Ʒ��.��߽���, ��Ʒ��.��ͽ���, ��Ʒ��.������� FROM ��Ʒ�� LEFT OUTER JOIN ��Ʒ����� ON ��Ʒ��.������ = ��Ʒ�����.ID WHERE (��Ʒ��.beactive = 1) ORDER BY ��Ʒ��.��Ʒ���";
                    break;
                case 10:
                    //ͬ����Ʒ

                    sqlComm.CommandText = "SELECT ��Ʒ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, CONVERT(numeric(8, 2), ROUND(��Ʒ��.���ɱ���, 2)) AS ���ɱ���, CONVERT(numeric(8, 2), ROUND(��Ʒ��.����ɱ���, 2)) AS ����ɱ���, CONVERT(numeric(8, 2), ROUND(��Ʒ��.���ս���, 2)) AS ���ս���, ��Ʒ��.������, ��Ʒ�����.�ϼ�����, ��Ʒ��.����, ��Ʒ��.������, ��Ʒ��.��߽���, ��Ʒ��.��ͽ���, ��Ʒ��.�������  FROM ��Ʒ�� LEFT OUTER JOIN ��Ʒ����� ON ��Ʒ��.������ = ��Ʒ�����.ID WHERE (��Ʒ��.beactive = 1) AND ((��Ʒ��.������ LIKE '%" + strZJM + "%') OR (��Ʒ��.��Ʒ���� LIKE '%" + strZJM + "%')) ORDER BY ��Ʒ��.��Ʒ���";
                    break;
                case 20:
                    sqlComm.CommandText = "SELECT ��Ʒ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, CONVERT(numeric(8, 2), ROUND(��Ʒ��.���ɱ���, 2)) AS ���ɱ���, CONVERT(numeric(8, 2), ROUND(��Ʒ��.����ɱ���, 2)) AS ����ɱ���, CONVERT(numeric(8, 2), ROUND(��Ʒ��.���ս���, 2)) AS ���ս���, ��Ʒ��.������, ��Ʒ�����.�ϼ�����, ��Ʒ��.����, ��Ʒ��.������, ��Ʒ��.��߽���, ��Ʒ��.��ͽ���, ��Ʒ��.������� FROM ��Ʒ�� LEFT OUTER JOIN ��Ʒ����� ON ��Ʒ��.������ = ��Ʒ�����.ID WHERE (��Ʒ��.beactive = 1) AND (��Ʒ��.��Ʒ��� LIKE '%" + strZJM + "%') ORDER BY ��Ʒ��.��Ʒ���";
                    break;
                case 40:
                    sqlComm.CommandText = "SELECT ��Ʒ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, CONVERT(numeric(8, 2), ROUND(��Ʒ��.���ɱ���, 2)) AS ���ɱ���, CONVERT(numeric(8, 2), ROUND(��Ʒ��.����ɱ���, 2)) AS ����ɱ���, CONVERT(numeric(8, 2), ROUND(��Ʒ��.���ս���, 2)) AS ���ս���, ��Ʒ��.������, ��Ʒ�����.�ϼ�����, ��Ʒ��.����, ��Ʒ��.������, ��Ʒ��.��߽���, ��Ʒ��.��ͽ���, ��Ʒ��.������� FROM ��Ʒ�� LEFT OUTER JOIN ��Ʒ����� ON ��Ʒ��.������ = ��Ʒ�����.ID WHERE (��Ʒ��.beactive = 1) AND (��Ʒ��.ID = " + strZJM + ") ORDER BY ��Ʒ��.��Ʒ���";
                    break;
                case 101:
                    sqlComm.CommandText = "SELECT ��Ʒ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, CONVERT(numeric(8, 2), ROUND(��Ʒ��.���ɱ���, 2)) AS ���ɱ���, CONVERT(numeric(8, 2), ROUND(��Ʒ��.����ɱ���, 2)) AS ����ɱ���, CONVERT(numeric(8, 2), ROUND(��Ʒ��.���ս���, 2)) AS ���ս���, ��Ʒ��.������, ��Ʒ�����.�ϼ�����, ��Ʒ��.����, ��Ʒ��.������, ��Ʒ��.��߽���, ��Ʒ��.��ͽ���, ��Ʒ��.������� FROM ��Ʒ�� LEFT OUTER JOIN ��Ʒ����� ON ��Ʒ��.������ = ��Ʒ�����.ID WHERE (��Ʒ��.beactive = 1) AND (��Ʒ��.��װ��Ʒ = 1) ORDER BY ��Ʒ��.��Ʒ���";
                    break;
                case 102:
                    sqlComm.CommandText = "SELECT ��Ʒ��.ID, ��Ʒ��.��Ʒ����, ��Ʒ��.��Ʒ���, ��Ʒ��.��Ʒ���, ��Ʒ��.��С������λ AS ��λ, CONVERT(numeric(8, 2), ROUND(��Ʒ��.���ɱ���, 2)) AS ���ɱ���, CONVERT(numeric(8, 2), ROUND(��Ʒ��.����ɱ���, 2)) AS ����ɱ���, CONVERT(numeric(8, 2), ROUND(��Ʒ��.���ս���, 2)) AS ���ս���, ��Ʒ��.������, ��Ʒ�����.�ϼ�����, ��Ʒ��.����, ��Ʒ��.������, ��Ʒ��.��߽���, ��Ʒ��.��ͽ���, ��Ʒ��.������� FROM ��Ʒ�� LEFT OUTER JOIN ��Ʒ����� ON ��Ʒ��.������ = ��Ʒ�����.ID WHERE (��Ʒ��.beactive = 1) AND (��Ʒ��.��װ��Ʒ = 1) AND ((��Ʒ��.������ LIKE '%" + strZJM + "%') OR (��Ʒ��.��Ʒ���� LIKE '%" + strZJM + "%')) ORDER BY ��Ʒ��.��Ʒ���";
                    break;
                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("��Ʒ��")) dSet.Tables.Remove("��Ʒ��");
            sqlDA.Fill(dSet, "��Ʒ��");

            if (dSet.Tables["��Ʒ��"].Rows.Count < 1) //û����Ʒ
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["��Ʒ��"].Rows.Count == 1) //ֻ��һ����Ʒ
            {
                iCommNumber = Int32.Parse(dSet.Tables["��Ʒ��"].Rows[0][0].ToString());
                strCommName = dSet.Tables["��Ʒ��"].Rows[0][1].ToString();
                strCommCode = dSet.Tables["��Ʒ��"].Rows[0][2].ToString();
                strCommGG = dSet.Tables["��Ʒ��"].Rows[0][3].ToString();
                strCommCount = dSet.Tables["��Ʒ��"].Rows[0][4].ToString();

                if (dSet.Tables["��Ʒ��"].Rows[0][5].ToString().Trim() != "")
                    decCommKCCBJ = Decimal.Parse(dSet.Tables["��Ʒ��"].Rows[0][5].ToString().Trim());
                else
                    decCommKCCBJ = 0;

                if (dSet.Tables["��Ʒ��"].Rows[0][6].ToString().Trim() != "")
                    decCommHSCBJ = Decimal.Parse(dSet.Tables["��Ʒ��"].Rows[0][6].ToString().Trim());
                else
                    decCommHSCBJ = 0;

                if (dSet.Tables["��Ʒ��"].Rows[0][7].ToString().Trim() != "")
                    decCommZZJJ = Decimal.Parse(dSet.Tables["��Ʒ��"].Rows[0][7].ToString().Trim());
                else
                    decCommZZJJ = 0;

                if (dSet.Tables["��Ʒ��"].Rows[0][10].ToString().Trim() != "")
                    decCommJJ = Decimal.Parse(dSet.Tables["��Ʒ��"].Rows[0][10].ToString().Trim());
                else
                    decCommJJ = 0;

                if (dSet.Tables["��Ʒ��"].Rows[0][11].ToString().Trim() != "")
                    decCommPFJ = Decimal.Parse(dSet.Tables["��Ʒ��"].Rows[0][11].ToString().Trim());
                else
                    decCommPFJ = 0;

                if (dSet.Tables["��Ʒ��"].Rows[0][12].ToString().Trim() != "")
                    decCommZGJJ = Decimal.Parse(dSet.Tables["��Ʒ��"].Rows[0][12].ToString().Trim());
                else
                    decCommZGJJ = 0;

                if (dSet.Tables["��Ʒ��"].Rows[0][13].ToString().Trim() != "")
                    decCommZDJJ = Decimal.Parse(dSet.Tables["��Ʒ��"].Rows[0][13].ToString().Trim());
                else
                    decCommZDJJ = 0;

                if (dSet.Tables["��Ʒ��"].Rows[0][14].ToString().Trim() != "")
                    decCommKCSL = Decimal.Parse(dSet.Tables["��Ʒ��"].Rows[0][14].ToString().Trim());
                else
                    decCommKCSL = 0;

                
                sqlConn.Close();
                getCommKF();
                return 1;
            }

            sqlConn.Close();
            //�����Ʒ
            FormSelectCommodities frmSelectCommodities = new FormSelectCommodities();
            frmSelectCommodities.strConn = strConn;
            frmSelectCommodities.strSelectText = sqlComm.CommandText;
            frmSelectCommodities.ShowDialog();

            if (frmSelectCommodities.iCommNumber == -1)
                return -1;

            if (frmSelectCommodities.iCommNumber== 0)
                return 0;
            else
            {
                iCommNumber = frmSelectCommodities.iCommNumber;
                strCommName = frmSelectCommodities.strCommName;
                strCommCode = frmSelectCommodities.strCommCode;
                strCommGG = frmSelectCommodities.strCommGG;
                strCommCount = frmSelectCommodities.strCommCount;
                decCommKCCBJ = frmSelectCommodities.decCommKCCBJ;
                decCommHSCBJ = frmSelectCommodities.decCommHSCBJ;
                decCommZZJJ = frmSelectCommodities.decCommZZJJ;
                decCommJJ = frmSelectCommodities.decCommJJ;
                decCommPFJ = frmSelectCommodities.decCommPFJ;
                decCommZGJJ = frmSelectCommodities.decCommZGJJ;
                decCommZDJJ = frmSelectCommodities.decCommZDJJ;
                decCommKCSL = frmSelectCommodities.decCommKCSL;

                getCommKF();
                sqlConn.Close();

                return 1;
            }

        }

        //�õ���Ʒȱʡ�ⷿ
        public void getCommKF()
        {
            iKFNumber = 0;
            strKFName = "";
            strKFCode = "";

            if (iCommNumber == 0)
                return;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʒ�����.�ⷿID, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ���� FROM ��Ʒ�� INNER JOIN ��Ʒ����� ON ��Ʒ��.������ = ��Ʒ�����.ID INNER JOIN �ⷿ�� ON ��Ʒ�����.�ⷿID = �ⷿ��.ID WHERE (��Ʒ��.ID = " + iCommNumber .ToString()+ ")";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                iKFNumber = Convert.ToInt32(sqldr.GetValue(0).ToString());
                strKFCode = sqldr.GetValue(1).ToString();
                strKFName = sqldr.GetValue(2).ToString();
            }
            sqldr.Close();


            sqlConn.Close();
        }

        public int iKFNumber = 0;
        public string strKFName = "";
        public string strKFCode = "";

        //iStyle:1,���пⷿ, 10,������пⷿ 20������������ⷿ
        //����: �ɹ�1, ʧ��0
        public int getKFInformation(int iStyle, string strZJM)
        {
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ID, �ⷿ���, �ⷿ���� FROM �ⷿ�� WHERE (BeActive = 1) ORDER BY �ⷿ���";
                    break;
                case 10:
                    sqlComm.CommandText = "SELECT ID, �ⷿ���, �ⷿ���� FROM �ⷿ�� WHERE (BeActive = 1) AND (�ⷿ��� LIKE '%" + strZJM + "%') ORDER BY �ⷿ���";
                    break;
                case 20:
                    sqlComm.CommandText = "SELECT ID, �ⷿ���, �ⷿ���� FROM �ⷿ�� WHERE (BeActive = 1) AND ((������ LIKE '%" + strZJM + "%') OR �ⷿ���� LIKE N'%" + strZJM + "%') ORDER BY �ⷿ���";
                    break;
                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("�ⷿ��")) dSet.Tables.Remove("�ⷿ��");
            sqlDA.Fill(dSet, "�ⷿ��");

            if (dSet.Tables["�ⷿ��"].Rows.Count < 1) //û�пⷿ
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["�ⷿ��"].Rows.Count == 1) //ֻ��һ����Ʒ
            {
                iKFNumber = Int32.Parse(dSet.Tables["�ⷿ��"].Rows[0][0].ToString());
                strKFName = dSet.Tables["�ⷿ��"].Rows[0][2].ToString();
                strKFCode = dSet.Tables["�ⷿ��"].Rows[0][1].ToString();

                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //����ⷿ
            FormSelectKF frmSelectKF = new FormSelectKF();
            frmSelectKF.strConn = strConn;
            frmSelectKF.strSelectText = sqlComm.CommandText;
            frmSelectKF.ShowDialog();

            if (frmSelectKF.iKFNumber == 0)
                return 0;
            else
            {
                iKFNumber = frmSelectKF.iKFNumber;
                strKFName = frmSelectKF.strKFName;
                strKFCode = frmSelectKF.strKFCode;
                sqlConn.Close();
                return 1;
            }

        }

        public int iZBNumber = 0;
        public string strZBName = "";
        public string strZBCode = "";
        public decimal dZBKL = 100;

        //iStyle:1,����֧���˲�, 10,�������֧���˲� 11��������������֧���˲�
        //����: �ɹ�1, ʧ��0
        public int getZBInformation(int iStyle, string strZJM)
        {
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ID, �˲����, �˲�����, ���� FROM �˲��� WHERE (BeActive = 1) AND (�Ƿ��֧�� = 1)";
                    break;
                case 10:
                    sqlComm.CommandText = "SELECT ID, �˲����, �˲�����, ���� FROM �˲��� WHERE (BeActive = 1) AND (�Ƿ��֧�� = 1) AND (�˲���� LIKE N'%"+strZJM+"%')";
                    break;
                case 11:
                    sqlComm.CommandText = "SELECT ID, �˲����, �˲�����, ���� FROM �˲��� WHERE (BeActive = 1) AND (�Ƿ��֧�� = 1) AND ((������ LIKE '%" + strZJM + "%') OR (�˲����� LIKE '%" + strZJM + "%'))";
                    break;
                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("�˲���")) dSet.Tables.Remove("�˲���");
            sqlDA.Fill(dSet, "�˲���");

            if (dSet.Tables["�˲���"].Rows.Count < 1) //û���˲�
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["�˲���"].Rows.Count == 1) //ֻ��һ���˲�
            {
                iZBNumber = Int32.Parse(dSet.Tables["�˲���"].Rows[0][0].ToString());
                strZBName = dSet.Tables["�˲���"].Rows[0][2].ToString();
                strZBCode = dSet.Tables["�˲���"].Rows[0][1].ToString();
                dZBKL = Convert.ToDecimal(dSet.Tables["�˲���"].Rows[0][3].ToString());

                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //����˲�
            FormSelectZB frmSelectZB = new FormSelectZB();
            frmSelectZB.strConn = strConn;
            frmSelectZB.strSelectText = sqlComm.CommandText;
            frmSelectZB.ShowDialog();

            if (frmSelectZB.iZBNumber == 0)
                return 0;
            else
            {
                iZBNumber = frmSelectZB.iZBNumber;
                strZBName = frmSelectZB.strZBName;
                strZBCode = frmSelectZB.strZBCode;
                dZBKL = frmSelectZB.dZBKL;
                sqlConn.Close();
                return 1;
            }

        }

        public int iBMNumber = 0;
        public string strBMName = "";
        public string strBMCode = "";

        //iStyle:1,���в���
        //����: �ɹ�1, ʧ��0
        public int getBMInformation(int iStyle, string strZJM)
        {
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ID, ���ű��, �������� FROM ���ű� WHERE (BeActive = 1)";
                    break;
                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("���ű�")) dSet.Tables.Remove("���ű�");
            sqlDA.Fill(dSet, "���ű�");

            if (dSet.Tables["���ű�"].Rows.Count < 1) //û�в���
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["���ű�"].Rows.Count == 1) //ֻ��һ������
            {
                iBMNumber = Int32.Parse(dSet.Tables["���ű�"].Rows[0][0].ToString());
                strBMName = dSet.Tables["���ű�"].Rows[0][2].ToString();
                strBMCode = dSet.Tables["���ű�"].Rows[0][1].ToString();

                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //���
            FormSelectBM frmSelectBM = new FormSelectBM();
            frmSelectBM.strConn = strConn;
            frmSelectBM.strSelectText = sqlComm.CommandText;
            frmSelectBM.ShowDialog();

            if (frmSelectBM.iBMNumber == 0)
                return 0;
            else
            {
                iBMNumber = frmSelectBM.iBMNumber;
                strBMName = frmSelectBM.strBMName;
                strBMCode = frmSelectBM.strBMCode;
                sqlConn.Close();
                return 1;
            }

        }

        public int iGWNumber = 0;
        public string strGWName = "";
        public string strGWCode = "";

        //iStyle:1,���и�λ
        //����: �ɹ�1, ʧ��0
        public int getGWInformation(int iStyle, string strZJM)
        {
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ID, ��λ���, ��λ���� FROM ��λ��";
                    break;
                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("��λ��")) dSet.Tables.Remove("��λ��");
            sqlDA.Fill(dSet, "��λ��");

            if (dSet.Tables["��λ��"].Rows.Count < 1) //û�в���
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["��λ��"].Rows.Count == 1) //ֻ��һ������
            {
                iGWNumber = Int32.Parse(dSet.Tables["��λ��"].Rows[0][0].ToString());
                strGWName = dSet.Tables["��λ��"].Rows[0][2].ToString();
                strGWCode = dSet.Tables["��λ��"].Rows[0][1].ToString();

                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //���
            FormSelectGW frmSelectGW = new FormSelectGW();
            frmSelectGW.strConn = strConn;
            frmSelectGW.strSelectText = sqlComm.CommandText;
            frmSelectGW.ShowDialog();

            if (frmSelectGW.iGWNumber == 0)
                return 0;
            else
            {
                iGWNumber = frmSelectGW.iGWNumber;
                strGWName = frmSelectGW.strGWName;
                strGWCode = frmSelectGW.strGWCode;
                sqlConn.Close();
                return 1;
            }

        }




        //�õ���д���
        public string changeDAXIE(string sIn)
        {
            string s = double.Parse(sIn).ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");//d + "\n" +
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            string t=Regex.Replace(d, ".", delegate(Match m) { return "��Ԫ����Ҽ��������½��ƾ��տտտտտտշֽ�ʰ��Ǫ�����׾������"[m.Value[0] - '-'].ToString(); });

            if (t == "")
                t = "��";

            return t+"��";

        }

        //�õ����ݼ����������󷵻�""
        public string strSYSDATATIME = "";
                        //�õ�����������
        public void getSystemDateTime()
        {
            sqlConn.Open();
            sqlComm.CommandText = "SELECT GETDATE() AS ����";
            sqldr = sqlComm.ExecuteReader();

            while (sqldr.Read())
            {
                strSYSDATATIME = Convert.ToDateTime(sqldr.GetValue(0).ToString()).ToShortDateString();
            }
            sqlConn.Close();
        }

        public string getBillNumber(string strKey)
        {
            string strDateSYS="",strCount="";


            if (strKey == "")
                return "";

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
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
                strSYSDATATIME = strDateSYS;
                sqldr.Close();

                //�õ�����
                sqlComm.CommandText = "SELECT ʱ�� FROM �������� WHERE (ʱ�� = CONVERT(DATETIME, '"+strDateSYS+" 00:00:00', 102))";
                sqldr = sqlComm.ExecuteReader();

                if (sqldr.HasRows)
                    sqldr.Close();
                else //������ʱ�䲻�Ǻ�
                {
                    sqldr.Close();
                    //�������ڼ�������
                    sqlComm.CommandText = "UPDATE �������� SET ʱ�� = '"+strDateSYS+"', ���� = 1";
                    sqlComm.ExecuteNonQuery();
                }

                //�õ�������
                sqlComm.CommandText = "SELECT ���� FROM �������� WHERE (�ؼ��� = N'" + strKey + "')";
                sqldr = sqlComm.ExecuteReader();
                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    strCount = sqldr.GetValue(0).ToString();
                    sqldr.Close();

                    //���Ӽ�����
                    sqlComm.CommandText = "UPDATE �������� SET ���� = ���� + 1 WHERE (�ؼ��� = N'" + strKey + "')";
                    sqlComm.ExecuteNonQuery();
                }
                else
                    sqldr.Close();


                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ݿ����" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                return "";
            }
            finally
            {
                sqlConn.Close();
            }

            if (strCount!="")
            {
                if (iVersion <= 0)
                {
                    if (int.Parse(strCount) > 2)
                    {
                        MessageBox.Show("Ԥ�����û�ÿ��ֻ����������", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        strCount = "";
                        return strCount;
                    }
                }
                strCount = string.Format("{0:D3}", Int32.Parse(strCount));
                strCount = strKey.ToUpper()+Convert.ToDateTime(strDateSYS).ToString("yyyyMMdd") + strCount;
             }
            return strCount;
        }


        public void ClearDataGridViewErrorText(DataGridView dvIn)
        {
            if (dvIn.CurrentCell == null)
                return;
            for (int i = 0; i < dvIn.ColumnCount; i++)
            {
                dvIn.Rows[dvIn.CurrentCell.RowIndex].Cells[i].ErrorText = String.Empty;
            }
        }

        public int iBillNumber = 0;
        public string strBillCode = "";
        public int iBillCNumber =0;
        public string strBillCCode="";
        public string strBillCName = "";
        public int iPeopleNumber = 0;
        public string sPeopleName = "";
        public int iBillBMID = 0;

        //iStyle:1,���й�����, 2,�������۵�, 10,������й�����, 20,����������۵�, 3,���н��ﵥ, 30,������н��ﵥ,4,���е��۵�, 40,������е��۵�,5
        //����: �ɹ�1, ʧ��0
        public int getBillInformation(int iStyle, string strZJM)
        {
            bool bDWMC = true;
            switch (iStyle)
            {
                case 1:
                    sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ��λ��.ID AS ��λID, ������Ʒ�Ƶ���.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���.��˰�ϼ� AS ���, ������Ʒ�Ƶ���.��ע, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID WHERE (������Ʒ�Ƶ���.����� = 0) AND (������Ʒ�Ƶ���.BeActive = 1) ORDER BY ������Ʒ�Ƶ���.���� DESC";
                    break;
                case 2:
                    sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ��λ��.ID AS ��λID, ������Ʒ�Ƶ���.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���.��˰�ϼ� AS ���, ������Ʒ�Ƶ���.��ע, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID WHERE (������Ʒ�Ƶ���.������ = 0) AND (������Ʒ�Ƶ���.BeActive = 1) ORDER BY ������Ʒ�Ƶ���.���� DESC";
                    break;

                case 6602:
                    sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ��λ��.ID AS ��λID, ������Ʒ�Ƶ���.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���.��˰�ϼ� AS ���, ������Ʒ�Ƶ���.��ע, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID WHERE (������Ʒ�Ƶ���.������ = 0) AND (������Ʒ�Ƶ���.BeActive = 1) ORDER BY ������Ʒ�Ƶ���.���� DESC";
                    break;
                case 211:
                    sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ��λ��.ID AS ��λID, ������Ʒ�Ƶ���.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���.��˰�ϼ� AS ���, ������Ʒ�Ƶ���.��ע, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) ORDER BY ������Ʒ�Ƶ���.���� DESC";
                    break;
                case 80211:
                    sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ��λ��.ID AS ��λID, ������Ʒ�Ƶ���.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���.��˰�ϼ� AS ���,  ������Ʒ�Ƶ���.��ע, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID INNER JOIN (SELECT     ��ID, SUM(����) AS ��Ʒ���� FROM ������Ʒ�Ƶ���ϸ�� GROUP BY ��ID) AS B ON ������Ʒ�Ƶ���.ID = B.��ID LEFT OUTER JOIN (SELECT     COUNT(*) AS ��������, ���ݱ�� FROM ��Ʒ����� GROUP BY ���ݱ��) AS A ON ������Ʒ�Ƶ���.���ݱ�� = A.���ݱ�� WHERE     (������Ʒ�Ƶ���.BeActive = 1) AND (A.�������� IS NULL OR A.�������� < B.��Ʒ����) ORDER BY ������Ʒ�Ƶ���.���� DESC";
                    break;

                case 3:
                    sqlComm.CommandText = "SELECT ���������ܱ�.ID, ��λ��.ID AS ��λID, ���������ܱ�.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.����, ���������ܱ�.��˰�ϼ� AS ���, ���������ܱ�.��ע, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���������ܱ�.BeActive = 1) ORDER BY ���������ܱ�.���� DESC";
                    break;
                case 80003:
                    sqlComm.CommandText = "SELECT ���������ܱ�.ID, ��λ��.ID AS ��λID, ���������ܱ�.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.����, ���������ܱ�.��˰�ϼ� AS ���, ���������ܱ�.��ע, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN (SELECT     ��ID, SUM(����) AS ��Ʒ���� FROM ���������ϸ�� GROUP BY ��ID) AS B ON ���������ܱ�.ID = B.��ID LEFT OUTER JOIN (SELECT     COUNT(*) AS ��������, ���ݱ�� FROM ��Ʒ����� GROUP BY ���ݱ��) AS A ON ���������ܱ�.���ݱ�� = A.���ݱ�� WHERE (���������ܱ�.BeActive = 1) AND (A.�������� IS NULL OR A.�������� < B.��Ʒ����) ORDER BY ���������ܱ�.���� DESC";
                    break;
                case 10:
                    sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ��λ��.ID AS ��λID, ������Ʒ�Ƶ���.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���.��˰�ϼ� AS ���, ������Ʒ�Ƶ���.��ע, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID WHERE (������Ʒ�Ƶ���.����� = 0) AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���ݱ�� LIKE '%" + strZJM + "%') ORDER BY ������Ʒ�Ƶ���.���� DESC";
                    break;
                case 20:
                    sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ��λ��.ID AS ��λID, ������Ʒ�Ƶ���.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���.��˰�ϼ� AS ���, ������Ʒ�Ƶ���.��ע, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID WHERE (������Ʒ�Ƶ���.������ = 0) AND (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���ݱ�� LIKE '%" + strZJM + "%') ORDER BY ������Ʒ�Ƶ���.���� DESC";
                    break;
                case 2011:
                    sqlComm.CommandText = "SELECT ������Ʒ�Ƶ���.ID, ��λ��.ID AS ��λID, ������Ʒ�Ƶ���.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ������Ʒ�Ƶ���.����, ������Ʒ�Ƶ���.��˰�ϼ� AS ���, ������Ʒ�Ƶ���.��ע, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ������Ʒ�Ƶ��� INNER JOIN ��λ�� ON ������Ʒ�Ƶ���.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ������Ʒ�Ƶ���.ҵ��ԱID = ְԱ��.ID WHERE (������Ʒ�Ƶ���.BeActive = 1) AND (������Ʒ�Ƶ���.���ݱ�� LIKE '%" + strZJM + "%') ORDER BY ������Ʒ�Ƶ���.���� DESC";
                    break;
                case 30:
                    sqlComm.CommandText = "SELECT ���������ܱ�.ID, ��λ��.ID AS ��λID, ���������ܱ�.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.����, ���������ܱ�.��˰�ϼ� AS ���, ���������ܱ�.��ע, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���ݱ�� LIKE '%" + strZJM + "%') ORDER BY ���������ܱ�.���� DESC";
                    break;
                case 31:
                    sqlComm.CommandText = "SELECT ���������ܱ�.ID, ��λ��.ID AS ��λID, ���������ܱ�.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.����, ���������ܱ�.��˰�ϼ� AS ���, ���������ܱ�.��ע, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���ݱ�� LIKE '%" + strZJM + "%') AND (���������ܱ�.��ֵ���ID IS NULL) ORDER BY ���������ܱ�.���� DESC";
                    break;
                case 32:
                    sqlComm.CommandText = "SELECT ���������ܱ�.ID, ��λ��.ID AS ��λID, ���������ܱ�.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.����, ���������ܱ�.��˰�ϼ� AS ���, ���������ܱ�.��ע, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.��ֵ���ID IS NULL) ORDER BY ���������ܱ�.���� DESC";
                    break;

                case 4:
                    sqlComm.CommandText = "SELECT ����֪ͨ�����ܱ�.ID, ����֪ͨ�����ܱ�.ID AS Expr1,����֪ͨ�����ܱ�.���ݱ��, [ְԱ��_1].ְԱ���� AS ҵ��Ա, [ְԱ��_1].ְԱ���� AS ����Ա, ����֪ͨ�����ܱ�.����, ����֪ͨ�����ܱ�.ִ�б��, ����֪ͨ�����ܱ�.��ע, [ְԱ��_2].ID AS ҵ��ԱID, [ְԱ��_2].ְԱ���� AS ҵ��Ա,[ְԱ��_1].����ID  FROM ����֪ͨ�����ܱ� INNER JOIN ְԱ�� [ְԱ��_1] ON ����֪ͨ�����ܱ�.����ԱID = [ְԱ��_1].ID INNER JOIN ְԱ�� [ְԱ��_2] ON ����֪ͨ�����ܱ�.ҵ��ԱID = [ְԱ��_2].ID WHERE (����֪ͨ�����ܱ�.BeActive <> 0) AND (����֪ͨ�����ܱ�.ִ�б�� = 0) ORDER BY ����֪ͨ�����ܱ�.���� DESC";
                    break;

                case 40:
                    sqlComm.CommandText = "SELECT ����֪ͨ�����ܱ�.ID, ����֪ͨ�����ܱ�.ID AS Expr1,����֪ͨ�����ܱ�.���ݱ��, [ְԱ��_1].ְԱ���� AS ҵ��Ա, [ְԱ��_1].ְԱ���� AS ����Ա, ����֪ͨ�����ܱ�.����, ����֪ͨ�����ܱ�.ִ�б��, ����֪ͨ�����ܱ�.��ע, [ְԱ��_2].ID AS ҵ��ԱID, [ְԱ��_2].ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ����֪ͨ�����ܱ� INNER JOIN ְԱ�� [ְԱ��_1] ON ����֪ͨ�����ܱ�.����ԱID = [ְԱ��_1].ID INNER JOIN ְԱ�� [ְԱ��_2] ON ����֪ͨ�����ܱ�.ҵ��ԱID = [ְԱ��_2].ID WHERE (����֪ͨ�����ܱ�.BeActive <> 0) AND (����֪ͨ�����ܱ�.ִ�б�� = 0) AND (����֪ͨ�����ܱ�.���ݱ�� LIKE '%" + strZJM + "%') ORDER BY ����֪ͨ�����ܱ�.���� DESC";
                    break;

                case 50: //������ͬ,δִ��
                    sqlComm.CommandText = "SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.������λID, �ɹ���ͬ��.��ͬ��� AS ���ݱ��, ��λ��.��λ���, ��λ��.��λ����, �ɹ���ͬ��.ǩ��ʱ�� AS ����, �ɹ���ͬ��.���, ��λ��.��������, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (�ɹ���ͬ��.BeActive = 1) AND (�ɹ���ͬ��.�˻���� = 0) AND (�ɹ���ͬ��.ִ�б�� = 0) ORDER BY �ɹ���ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 51:  //�ѽ��������˻�������ͬ
                    sqlComm.CommandText = "SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.������λID, �ɹ���ͬ��.��ͬ��� AS ���ݱ��, ��λ��.��λ���, ��λ��.��λ����, �ɹ���ͬ��.ǩ��ʱ�� AS ����, �ɹ���ͬ��.���, ��λ��.��������, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (�ɹ���ͬ��.BeActive = 1) AND (�ɹ���ͬ��.�˻���� = 0) AND (�ɹ���ͬ��.ִ�б�� = 1) ORDER BY �ɹ���ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 52:  //���ۺ�ͬ
                    sqlComm.CommandText = "SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.������λID, ���ۺ�ͬ��.��ͬ��� AS ���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ���ۺ�ͬ��.ǩ��ʱ�� AS ����, ���ۺ�ͬ��.���, ��λ��.��������, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (���ۺ�ͬ��.BeActive = 1) AND (���ۺ�ͬ��.�˻���� = 0) AND (���ۺ�ͬ��.ִ�б�� = 0) ORDER BY ���ۺ�ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 53:  //�ѳ��������˻����ۺ�ͬ
                    sqlComm.CommandText = "SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.������λID, ���ۺ�ͬ��.��ͬ��� AS ���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ���ۺ�ͬ��.ǩ��ʱ�� AS ����, ���ۺ�ͬ��.���, ��λ��.��������, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (���ۺ�ͬ��.BeActive = 1) AND (���ۺ�ͬ��.�˻���� = 0) AND (���ۺ�ͬ��.ִ�б�� = 1) ORDER BY ���ۺ�ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 54://���к�ͬ
                    sqlComm.CommandText = "(SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.������λID, �ɹ���ͬ��.��ͬ��� AS ���ݱ��, ��λ��.��λ���, ��λ��.��λ����, �ɹ���ͬ��.ǩ��ʱ�� AS ����, �ɹ���ͬ��.���, ��λ��.��������, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (�ɹ���ͬ��.BeActive = 1) ORDER BY �ɹ���ͬ��.ǩ��ʱ�� DESC) UNION (SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.������λID, ���ۺ�ͬ��.��ͬ��� AS ���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ���ۺ�ͬ��.ǩ��ʱ�� AS ����, ���ۺ�ͬ��.���, ��λ��.��������, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (���ۺ�ͬ��.BeActive = 1) ORDER BY ���ۺ�ͬ��.ǩ��ʱ�� DESC)";
                    break;

                case 510: //���������ͬ
                    sqlComm.CommandText = "SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.������λID, �ɹ���ͬ��.��ͬ��� AS ���ݱ��, ��λ��.��λ���, ��λ��.��λ����, �ɹ���ͬ��.ǩ��ʱ�� AS ����, �ɹ���ͬ��.���, ��λ��.��������, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (�ɹ���ͬ��.BeActive = 1) AND (�ɹ���ͬ��.��ͬ��� LIKE '%" + strZJM + "%') AND (�ɹ���ͬ��.�˻���� = 0) AND (�ɹ���ͬ��.ִ�б�� = 0) ORDER BY �ɹ���ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 511:  //�����ѽ��������˻�������ͬ
                    sqlComm.CommandText = "SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.������λID, �ɹ���ͬ��.��ͬ���, ��λ��.��λ���, ��λ��.��λ����, �ɹ���ͬ��.ǩ��ʱ�� AS ����, �ɹ���ͬ��.���, ��λ��.��������, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (�ɹ���ͬ��.BeActive = 1) AND (�ɹ���ͬ��.�˻���� = 0) AND (�ɹ���ͬ��.��ͬ��� LIKE '%" + strZJM + "%') AND (�ɹ���ͬ��.ִ�б�� = 1)  ORDER BY �ɹ���ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 512:  //�������ۺ�ͬ
                    sqlComm.CommandText = "SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.������λID, ���ۺ�ͬ��.��ͬ��� AS ���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ���ۺ�ͬ��.ǩ��ʱ�� AS ����, ���ۺ�ͬ��.���, ��λ��.��������, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (���ۺ�ͬ��.BeActive = 1) AND (���ۺ�ͬ��.��ͬ��� LIKE '%" + strZJM + "%') AND (���ۺ�ͬ��.�˻���� = 0)  AND (���ۺ�ͬ��.ִ�б�� = 0)  ORDER BY ���ۺ�ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 513:  //�����ѳ��������˻����ۺ�ͬ
                    sqlComm.CommandText = "SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.������λID, ���ۺ�ͬ��.��ͬ��� AS ���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ���ۺ�ͬ��.ǩ��ʱ�� AS ����, ���ۺ�ͬ��.���, ��λ��.��������, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (���ۺ�ͬ��.BeActive = 1) AND (���ۺ�ͬ��.�˻���� = 0) AND (���ۺ�ͬ��.��ͬ��� LIKE '%" + strZJM + "%') AND (���ۺ�ͬ��.ִ�б�� = 1)  ORDER BY ���ۺ�ͬ��.ǩ��ʱ�� DESC";
                    break;
                case 514://�������к�ͬ
                    sqlComm.CommandText = "(SELECT ���ۺ�ͬ��.ID, ���ۺ�ͬ��.������λID, ���ۺ�ͬ��.��ͬ��� AS ���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ���ۺ�ͬ��.ǩ��ʱ�� AS ����, ���ۺ�ͬ��.���, ��λ��.��������, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���ۺ�ͬ�� INNER JOIN ��λ�� ON ���ۺ�ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON ���ۺ�ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (���ۺ�ͬ��.BeActive = 1)  AND (���ۺ�ͬ��.��ͬ��� LIKE '%" + strZJM + "%')) UNION (SELECT �ɹ���ͬ��.ID, �ɹ���ͬ��.������λID, �ɹ���ͬ��.��ͬ��� AS ���ݱ��, ��λ��.��λ���, ��λ��.��λ����, �ɹ���ͬ��.ǩ��ʱ�� AS ����, �ɹ���ͬ��.���, ��λ��.��������, ְԱ��.ID AS ְԱID, ְԱ��.ְԱ���� AS ҵ��Ա FROM �ɹ���ͬ�� INNER JOIN ��λ�� ON �ɹ���ͬ��.������λID = ��λ��.ID INNER JOIN ְԱ�� ON �ɹ���ͬ��.ҵ��ԱID = ְԱ��.ID WHERE (�ɹ���ͬ��.BeActive = 1) AND (�ɹ���ͬ��.��ͬ��� LIKE '%" + strZJM + "%')) ORDER BY ���� DESC";
                    break;

                case 61://���н�����ⵥ
                    sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.��λID, ���������ܱ�.���ݱ��,��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.����, ���������ܱ�.��˰�ϼ� AS ���, ���������ܱ�.��ע, ���������ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���������ܱ�.BeActive = 1) ORDER BY ���������ܱ�.���� DESC";
                    break;

                case 80061://���н�����ⵥ
                    sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.��λID, ���������ܱ�.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.����, ���������ܱ�.��˰�ϼ� AS ���,  ���������ܱ�.��ע, ���������ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN (SELECT     ����ID, SUM(����) AS ��Ʒ���� FROM ���������ϸ�� GROUP BY ����ID) AS B ON ���������ܱ�.ID = B.����ID LEFT OUTER JOIN (SELECT     COUNT(*) AS ��������, ���ݱ�� FROM ��Ʒ����� GROUP BY ���ݱ��) AS A ON ���������ܱ�.���ݱ�� = A.���ݱ�� WHERE     (���������ܱ�.BeActive = 1) AND (A.�������� IS NULL OR A.�������� < B.��Ʒ����) ORDER BY ���������ܱ�.���� DESC";
                    break;
                case 611://������н�����ⵥ
                    sqlComm.CommandText = "SELECT ���������ܱ�.ID, ���������ܱ�.��λID, ���������ܱ�.���ݱ��,��λ��.��λ���, ��λ��.��λ����, ���������ܱ�.����, ���������ܱ�.��˰�ϼ� AS ���, ���������ܱ�.��ע, ���������ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���������ܱ� INNER JOIN ��λ�� ON ���������ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���������ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���������ܱ�.BeActive = 1) AND (���������ܱ�.���ݱ�� LIKE '%" + strZJM + "%') ORDER BY ���������ܱ�.���� DESC";
                    break;
                case 62://�����˻ص�
                    sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.��λID, �����˳����ܱ�.���ݱ��,��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.����, �����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.��ע, �����˳����ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �����˳����ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (�����˳����ܱ�.BeActive = 1) ORDER BY �����˳����ܱ�.���� DESC";
                    break;
                case 80062://�����˻ص�
                    sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.��λID, �����˳����ܱ�.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.����, �����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.��ע, �����˳����ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �����˳����ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN (SELECT     COUNT(*) AS ��������, ���ݱ�� FROM ��Ʒ����� GROUP BY ���ݱ��) AS A ON �����˳����ܱ�.���ݱ�� = A.���ݱ�� LEFT OUTER JOIN (SELECT ����ID, SUM(����) AS ��Ʒ���� FROM �����˳���ϸ�� GROUP BY ����ID) AS B ON �����˳����ܱ�.ID = B.����ID WHERE (�����˳����ܱ�.BeActive = 1) AND (A.�������� IS NULL OR A.�������� < B.��Ʒ����) ORDER BY �����˳����ܱ�.���� DESC";
                    break;
                case 621://��������˻ص�
                    sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.��λID, �����˳����ܱ�.���ݱ��,��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.����, �����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.��ע, �����˳����ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �����˳����ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���ݱ�� LIKE '%" + strZJM + "%') ORDER BY �����˳����ܱ�.���� DESC";
                    break;
                case 63://���п�浥
                    sqlComm.CommandText = "SELECT ����̵���ܱ�.ID, ����̵���ܱ�.�ⷿID, ����̵���ܱ�.���ݱ��, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ����̵���ܱ�.�̵�ʱ�� AS ����, ����̵���ܱ�.�����ϼ�, ����̵���ܱ�.���ϼ�, ����̵���ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, 0  FROM ����̵���ܱ� INNER JOIN �ⷿ�� ON ����̵���ܱ�.�ⷿID = �ⷿ��.ID INNER JOIN ְԱ�� ON ����̵���ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (����̵���ܱ�.BeActive = 1) ORDER BY ����̵���ܱ�.�̵�ʱ�� DESC";
                    bDWMC = false;
                    break;
                case 80063://���п�浥
                    sqlComm.CommandText = "SELECT ����̵���ܱ�.ID, ����̵���ܱ�.�ⷿID, ����̵���ܱ�.���ݱ��, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ����̵���ܱ�.�̵�ʱ�� AS ����,  ����̵���ܱ�.�����ϼ�, ����̵���ܱ�.���ϼ�, ����̵���ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, 0 AS Expr1, B.��Ʒ����, A.�������� FROM ����̵���ܱ� INNER JOIN �ⷿ�� ON ����̵���ܱ�.�ⷿID = �ⷿ��.ID INNER JOIN ְԱ�� ON ����̵���ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN (SELECT     ����ID, SUM(��������) AS ��Ʒ���� FROM ����̵���ϸ�� GROUP BY ����ID) AS B ON ����̵���ܱ�.ID = B.����ID LEFT OUTER JOIN (SELECT COUNT(*) AS ��������, ���ݱ�� FROM ��Ʒ����� GROUP BY ���ݱ��) AS A ON ����̵���ܱ�.���ݱ�� = A.���ݱ�� WHERE (����̵���ܱ�.BeActive = 1) AND (A.�������� IS NULL OR A.�������� < B.��Ʒ����) AND (B.��Ʒ���� <> 0) ORDER BY ����̵���ܱ�.�̵�ʱ�� DESC";
                    bDWMC = false;
                    break;
                case 631://������п�浥
                    sqlComm.CommandText = "SELECT ����̵���ܱ�.ID, ����̵���ܱ�.�ⷿID, ����̵���ܱ�.���ݱ��, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ����̵���ܱ�.�̵�ʱ�� AS ����, ����̵���ܱ�.�����ϼ�, ����̵���ܱ�.���ϼ�, ����̵���ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, 0  FROM ����̵���ܱ� INNER JOIN �ⷿ�� ON ����̵���ܱ�.�ⷿID = �ⷿ��.ID INNER JOIN ְԱ�� ON ����̵���ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (����̵���ܱ�.BeActive = 1) AND (����̵���ܱ�.���ݱ�� LIKE '%" + strZJM + "%') ORDER BY ����̵���ܱ�.�̵�ʱ�� DESC";
                    bDWMC = false;
                    break;
                case 64://���б���
                    sqlComm.CommandText = "SELECT ��汨����ܱ�.ID, ��汨����ܱ�.�ⷿID, ��汨����ܱ�.���ݱ��, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ��汨����ܱ�.����, ��汨����ܱ�.���������ϼ�, ��汨����ܱ�.������ϼ�, ��汨����ܱ�.ҵ��ԱID, ְԱ��.ְԱ����, 0  FROM ��汨����ܱ� INNER JOIN �ⷿ�� ON ��汨����ܱ�.�ⷿID = �ⷿ��.ID INNER JOIN ְԱ�� ON ��汨����ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (��汨����ܱ�.BeActive = 1) ORDER BY ��汨����ܱ�.���� DESC";
                    bDWMC = false;
                    break;
                case 80064://
                    sqlComm.CommandText = "SELECT ��汨����ܱ�.ID, ��汨����ܱ�.�ⷿID, ��汨����ܱ�.���ݱ��, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ��汨����ܱ�.����, ��汨����ܱ�.���������ϼ�, ��汨����ܱ�.������ϼ�, ��汨����ܱ�.ҵ��ԱID, ְԱ��.ְԱ����, 0 AS Expr1, B.��Ʒ����, A.�������� FROM ��汨����ܱ� INNER JOIN �ⷿ�� ON ��汨����ܱ�.�ⷿID = �ⷿ��.ID INNER JOIN ְԱ�� ON ��汨����ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN (SELECT     ����ID, SUM(��������) AS ��Ʒ���� FROM ��汨����ϸ�� GROUP BY ����ID) AS B ON ��汨����ܱ�.ID = B.����ID LEFT OUTER JOIN (SELECT COUNT(*) AS ��������, ���ݱ�� FROM ��Ʒ����� GROUP BY ���ݱ��) AS A ON ��汨����ܱ�.���ݱ�� = A.���ݱ�� WHERE (��汨����ܱ�.BeActive = 1) AND (A.�������� IS NULL OR A.�������� < B.��Ʒ����) ORDER BY ��汨����ܱ�.���� DESC";
                    bDWMC = false;
                    break;

                case 641://������б���
                    sqlComm.CommandText = "SELECT ��汨����ܱ�.ID, ��汨����ܱ�.�ⷿID, ��汨����ܱ�.���ݱ��, �ⷿ��.�ⷿ���, �ⷿ��.�ⷿ����, ��汨����ܱ�.����, ��汨����ܱ�.���������ϼ�, ��汨����ܱ�.������ϼ�, ��汨����ܱ�.ҵ��ԱID, ְԱ��.ְԱ����, 0  FROM ��汨����ܱ� INNER JOIN �ⷿ�� ON ��汨����ܱ�.�ⷿID = �ⷿ��.ID INNER JOIN ְԱ�� ON ��汨����ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (��汨����ܱ�.BeActive = 1) AND (��汨����ܱ�.���ݱ�� LIKE '%" + strZJM + "%')  ORDER BY ��汨����ܱ�.���� DESC";
                    bDWMC = false;
                    break;
                case 65://�������۳��ⵥ
                    sqlComm.CommandText = "SELECT ���۳�����ܱ�.ID, ���۳�����ܱ�.��λID, ���۳�����ܱ�.���ݱ��,��λ��.��λ���, ��λ��.��λ����, ���۳�����ܱ�.����, ���۳�����ܱ�.��˰�ϼ� AS ���, ���۳�����ܱ�.��ע, ���۳�����ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���۳�����ܱ� INNER JOIN ��λ�� ON ���۳�����ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���۳�����ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���۳�����ܱ�.BeActive = 1) ORDER BY ���۳�����ܱ�.���� DESC";
                    break;
                case 651://����������۳��ⵥ
                    sqlComm.CommandText = "SELECT ���۳�����ܱ�.ID, ���۳�����ܱ�.��λID, ���۳�����ܱ�.���ݱ��,��λ��.��λ���, ��λ��.��λ����, ���۳�����ܱ�.����, ���۳�����ܱ�.��˰�ϼ� AS ���, ���۳�����ܱ�.��ע, ���۳�����ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM ���۳�����ܱ� INNER JOIN ��λ�� ON ���۳�����ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON ���۳�����ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (���۳�����ܱ�.BeActive = 1)  AND (���۳�����ܱ�.���ݱ�� LIKE '%" + strZJM + "%') ORDER BY ���۳�����ܱ�.���� DESC";
                    break;
                case 66://���������˻ص�
                    sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.��λID, �����˳����ܱ�.���ݱ��,��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.����, �����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.��ע, �����˳����ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �����˳����ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (�����˳����ܱ�.BeActive = 1) ORDER BY �����˳����ܱ�.���� DESC";
                    break;
                case 80066://���������˻ص�
                    sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.��λID, �����˳����ܱ�.���ݱ��, ��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.����, �����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.��ע, �����˳����ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �����˳����ܱ�.ҵ��ԱID = ְԱ��.ID INNER JOIN (SELECT     COUNT(*) AS ��������, ���ݱ�� FROM ��Ʒ����� GROUP BY ���ݱ��) AS A ON �����˳����ܱ�.���ݱ�� = A.���ݱ�� LEFT OUTER JOIN (SELECT     ����ID, SUM(����) AS ��Ʒ���� FROM �����˳���ϸ�� GROUP BY ����ID) AS B ON �����˳����ܱ�.ID = B.����ID WHERE (�����˳����ܱ�.BeActive = 1) AND (A.�������� IS NULL OR A.�������� < B.��Ʒ����) ORDER BY �����˳����ܱ�.���� DESC";
                    break;
                case 661://������������˻ص�
                    sqlComm.CommandText = "SELECT �����˳����ܱ�.ID, �����˳����ܱ�.��λID, �����˳����ܱ�.���ݱ��,��λ��.��λ���, ��λ��.��λ����, �����˳����ܱ�.����, �����˳����ܱ�.��˰�ϼ�, �����˳����ܱ�.��ע, �����˳����ܱ�.ҵ��ԱID, ְԱ��.ְԱ���� AS ҵ��Ա, ��λ��.����ID  FROM �����˳����ܱ� INNER JOIN ��λ�� ON �����˳����ܱ�.��λID = ��λ��.ID INNER JOIN ְԱ�� ON �����˳����ܱ�.ҵ��ԱID = ְԱ��.ID WHERE (�����˳����ܱ�.BeActive = 1) AND (�����˳����ܱ�.���ݱ�� LIKE '%" + strZJM + "%') ORDER BY �����˳����ܱ�.���� DESC";
                    break;

                default:
                    return 0;
            }

            sqlConn.Open();
            if (dSet.Tables.Contains("���ݱ�")) dSet.Tables.Remove("���ݱ�");
            sqlDA.Fill(dSet, "���ݱ�");

            if (dSet.Tables["���ݱ�"].Rows.Count < 1) //û�е���
            {
                sqlConn.Close();
                return 0;
            }

            if (dSet.Tables["���ݱ�"].Rows.Count == 1) //ֻ��һ������
            {
                iBillNumber = Int32.Parse(dSet.Tables["���ݱ�"].Rows[0][0].ToString());
                strBillCode = dSet.Tables["���ݱ�"].Rows[0][2].ToString();
                iBillCNumber = Int32.Parse(dSet.Tables["���ݱ�"].Rows[0][1].ToString());
                strBillCCode = dSet.Tables["���ݱ�"].Rows[0][3].ToString();
                strBillCName = dSet.Tables["���ݱ�"].Rows[0][4].ToString();

                try
                {
                    iPeopleNumber = int.Parse(dSet.Tables["���ݱ�"].Rows[0][8].ToString());
                }
                catch
                {
                    iPeopleNumber = 0;
                }
                sPeopleName = dSet.Tables["���ݱ�"].Rows[0][9].ToString();

                try
                {
                    iBillBMID = int.Parse(dSet.Tables["���ݱ�"].Rows[0][10].ToString());
                }
                catch
                {
                    iBillBMID = 0;
                }



                sqlConn.Close();
                return 1;
            }

            sqlConn.Close();
            //�������
            FormSelectBill frmSelectBill = new  FormSelectBill();
            frmSelectBill.strConn = strConn;
            frmSelectBill.strSelectText = sqlComm.CommandText;
            frmSelectBill.bShowDW = bDWMC;

            frmSelectBill.ShowDialog();

            if (frmSelectBill.iBillNumber == 0)
                return 0;
            else
            {
                iBillNumber = frmSelectBill.iBillNumber;
                strBillCode = frmSelectBill.strBillCode;
                iBillCNumber = frmSelectBill.iBillCNumber;
                strBillCCode = frmSelectBill.strBillCCode;
                strBillCName = frmSelectBill.strBillCName;
                iPeopleNumber = frmSelectBill.iPeopleNumber;
                sPeopleName = frmSelectBill.sPeopleName;
                iBillBMID = frmSelectBill.iBillBMID;
                sqlConn.Close();
                return 1;
            }

        }


        //������ɱ���
        public decimal countKCCBJ(decimal dKCL, decimal dKCJE, decimal dKCLIN, decimal dKCJEIN)
        {
            decimal dTemp=0;

            dTemp = dKCL + dKCLIN;
            if (dTemp == 0)
            {
                if (dKCL == 0)
                    return 0;
                else
                    return dKCJE / dKCL;
            }

            //����
            return (dKCJE + dKCJEIN) / (dTemp);
        }

        //�õ������
        public decimal dZKCL= 0;
        public decimal dKCL = 0;
        public decimal dKCJE = 0;
        public void getKCL(int intCommNumber,int intKFNumber)
        {

            dZBKL = 0; dKCL = 0;
            if (intCommNumber == 0)
                return;

            sqlConn.Open();
            sqlComm.CommandText = "SELECT �������, ����� FROM ���� WHERE (�ⷿID = " + intKFNumber.ToString() + ") AND (��ƷID = " + intCommNumber.ToString() + ")";
            sqldr=sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dKCL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
            }
            sqldr.Close();

            sqlComm.CommandText = "SELECT �������, ����� FROM ��Ʒ�� WHERE (ID = " + intCommNumber.ToString() + ")";
            sqldr = sqlComm.ExecuteReader();
            while (sqldr.Read())
            {
                dZKCL = Convert.ToDecimal(sqldr.GetValue(0).ToString());
                dKCJE = Convert.ToDecimal(sqldr.GetValue(1).ToString());
            }
            sqlConn.Close();
        }

        //private static int[] pyvalue = new int[]{-20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,-20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,-19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,-19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,-19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,-19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,-18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,-18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183,-18181,-18012,-17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,-17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,-17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,-16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,-16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,-16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,-15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,-15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,-15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,-15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,-14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,-14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,-14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,-14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,-14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,-13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,-13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,-13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,-13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,-12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,-12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,-11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,-11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,-10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,-10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254};
        //private static string[] pystr = new string[]{"a","ai","an","ang","ao","ba","bai","ban","bang","bao","bei","ben","beng","bi","bian","biao","bie","bin","bing","bo","bu","ca","cai","can","cang","cao","ce","ceng","cha","chai","chan","chang","chao","che","chen","cheng","chi","chong","chou","chu","chuai","chuan","chuang","chui","chun","chuo","ci","cong","cou","cu","cuan","cui","cun","cuo","da","dai","dan","dang","dao","de","deng","di","dian","diao","die","ding","diu","dong","dou","du","duan","dui","dun","duo","e","en","er","fa","fan","fang","fei","fen","feng","fo","fou","fu","ga","gai","gan","gang","gao","ge","gei","gen","geng","gong","gou","gu","gua","guai","guan","guang","gui","gun","guo","ha","hai","han","hang","hao","he","hei","hen","heng","hong","hou","hu","hua","huai","huan","huang","hui","hun","huo","ji","jia","jian","jiang","jiao","jie","jin","jing","jiong","jiu","ju","juan","jue","jun","ka","kai","kan","kang","kao","ke","ken","keng","kong","kou","ku","kua","kuai","kuan","kuang","kui","kun","kuo","la","lai","lan","lang","lao","le","lei","leng","li","lia","lian","liang","liao","lie","lin","ling","liu","long","lou","lu","lv","luan","lue","lun","luo","ma","mai","man","mang","mao","me","mei","men","meng","mi","mian","miao","mie","min","ming","miu","mo","mou","mu","na","nai","nan","nang","nao","ne","nei","nen","neng","ni","nian","niang","niao","nie","nin","ning","niu","nong","nu","nv","nuan","nue","nuo","o","ou","pa","pai","pan","pang","pao","pei","pen","peng","pi","pian","piao","pie","pin","ping","po","pu","qi","qia","qian","qiang","qiao","qie","qin","qing","qiong","qiu","qu","quan","que","qun","ran","rang","rao","re","ren","reng","ri","rong","rou","ru","ruan","rui","run","ruo","sa","sai","san","sang","sao","se","sen","seng","sha","shai","shan","shang","shao","she","shen","sheng","shi","shou","shu","shua","shuai","shuan","shuang","shui","shun","shuo","si","song","sou","su","suan","sui","sun","suo","ta","tai","tan","tang","tao","te","teng","ti","tian","tiao","tie","ting","tong","tou","tu","tuan","tui","tun","tuo","wa","wai","wan","wang","wei","wen","weng","wo","wu","xi","xia","xian","xiang","xiao","xie","xin","xing","xiong","xiu","xu","xuan","xue","xun","ya","yan","yang","yao","ye","yi","yin","ying","yo","yong","you","yu","yuan","yue","yun","za","zai","zan","zang","zao","ze","zei","zen","zeng","zha","zhai","zhan","zhang","zhao","zhe","zhen","zheng","zhi","zhong","zhou","zhu","zhua","zhuai","zhuan","zhuang","zhui","zhun","zhuo","zi","zong","zou","zu","zuan","zui","zun","zuo"};
        private static int[] pyvalue=new int[]{-20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,-20032,-20026, 
-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,-19756,-19751,-19746,-19741,-19739,-19728, 
-19725,-19715,-19540,-19531,-19525,-19515,-19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263, 
-19261,-19249,-19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,-19003,-18996, 
-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,-18731,-18722,-18710,-18697,-18696,-18526, 
-18518,-18501,-18490,-18478,-18463,-18448,-18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, 
-18181,-18012,-17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,-17733,-17730, 
-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,-17468,-17454,-17433,-17427,-17417,-17202, 
-17185,-16983,-16970,-16942,-16915,-16733,-16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459, 
-16452,-16448,-16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,-16212,-16205, 
-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,-15933,-15920,-15915,-15903,-15889,-15878, 
-15707,-15701,-15681,-15667,-15661,-15659,-15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416, 
-15408,-15394,-15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,-15149,-15144, 
-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,-14941,-14937,-14933,-14930,-14929,-14928, 
-14926,-14922,-14921,-14914,-14908,-14902,-14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668, 
-14663,-14654,-14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,-14170,-14159, 
-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,-14109,-14099,-14097,-14094,-14092,-14090, 
-14087,-14083,-13917,-13914,-13910,-13907,-13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658, 
-13611,-13601,-13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,-13340,-13329, 
-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,-13068,-13063,-13060,-12888,-12875,-12871, 
-12860,-12858,-12852,-12849,-12838,-12831,-12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346, 
-12320,-12300,-12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,-11781,-11604, 
-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,-11055,-11052,-11045,-11041,-11038,-11024, 
-11020,-11019,-11018,-11014,-10838,-10832,-10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331, 
-10329,-10328,-10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254}; 
private static string[] pystr=new string[]{"a","ai","an","ang","ao","ba","bai","ban","bang","bao","bei","ben","beng","bi","bian","biao", 
"bie","bin","bing","bo","bu","ca","cai","can","cang","cao","ce","ceng","cha","chai","chan","chang","chao","che","chen", 
"cheng","chi","chong","chou","chu","chuai","chuan","chuang","chui","chun","chuo","ci","cong","cou","cu","cuan","cui", 
"cun","cuo","da","dai","dan","dang","dao","de","deng","di","dian","diao","die","ding","diu","dong","dou","du","duan", 
"dui","dun","duo","e","en","er","fa","fan","fang","fei","fen","feng","fo","fou","fu","ga","gai","gan","gang","gao", 
"ge","gei","gen","geng","gong","gou","gu","gua","guai","guan","guang","gui","gun","guo","ha","hai","han","hang", 
"hao","he","hei","hen","heng","hong","hou","hu","hua","huai","huan","huang","hui","hun","huo","ji","jia","jian", 
"jiang","jiao","jie","jin","jing","jiong","jiu","ju","juan","jue","jun","ka","kai","kan","kang","kao","ke","ken", 
"keng","kong","kou","ku","kua","kuai","kuan","kuang","kui","kun","kuo","la","lai","lan","lang","lao","le","lei", 
"leng","li","lia","lian","liang","liao","lie","lin","ling","liu","long","lou","lu","lv","luan","lue","lun","luo", 
"ma","mai","man","mang","mao","me","mei","men","meng","mi","mian","miao","mie","min","ming","miu","mo","mou","mu", 
"na","nai","nan","nang","nao","ne","nei","nen","neng","ni","nian","niang","niao","nie","nin","ning","niu","nong", 
"nu","nv","nuan","nue","nuo","o","ou","pa","pai","pan","pang","pao","pei","pen","peng","pi","pian","piao","pie", 
"pin","ping","po","pu","qi","qia","qian","qiang","qiao","qie","qin","qing","qiong","qiu","qu","quan","que","qun", 
"ran","rang","rao","re","ren","reng","ri","rong","rou","ru","ruan","rui","run","ruo","sa","sai","san","sang", 
"sao","se","sen","seng","sha","shai","shan","shang","shao","she","shen","sheng","shi","shou","shu","shua", 
"shuai","shuan","shuang","shui","shun","shuo","si","song","sou","su","suan","sui","sun","suo","ta","tai", 
"tan","tang","tao","te","teng","ti","tian","tiao","tie","ting","tong","tou","tu","tuan","tui","tun","tuo", 
"wa","wai","wan","wang","wei","wen","weng","wo","wu","xi","xia","xian","xiang","xiao","xie","xin","xing", 
"xiong","xiu","xu","xuan","xue","xun","ya","yan","yang","yao","ye","yi","yin","ying","yo","yong","you", 
"yu","yuan","yue","yun","za","zai","zan","zang","zao","ze","zei","zen","zeng","zha","zhai","zhan","zhang", 
"zhao","zhe","zhen","zheng","zhi","zhong","zhou","zhu","zhua","zhuai","zhuan","zhuang","zhui","zhun","zhuo", 
"zi","zong","zou","zu","zuan","zui","zun","zuo"}; 

        //��ĸ
        public string convertPYSM(string chrstr)
        {
            byte[] array = new byte[2];
            string returnstr = "";
            int chrasc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] nowchar = chrstr.ToCharArray();
            for (int j = 0; j < nowchar.Length; j++)
            {
                array = System.Text.Encoding.Default.GetBytes(nowchar[j].ToString());
                if (array.Length < 2) //���ֽ�
                {
                    returnstr += nowchar[j].ToString();
                    continue;
                }

                i1 = (short)(array[0]);
                i2 = (short)(array[1]);
                chrasc = i1 * 256 + i2 - 65536;

                if (chrasc < -20319 || chrasc > -10247)
                { // ��֪�����ַ�
                    if (chrasc == -4445) //�
                    {
                        returnstr += "R";
                    }

                    continue;
                }

                if (chrasc > 0 && chrasc < 160)
                {
                    returnstr += nowchar[j];
                }
                else
                {
                        for (int i = (pyvalue.Length - 1); i >= 0; i--)
                        {
                            if (pyvalue[i] <= chrasc)
                            {
                                returnstr += pystr[i].Substring(0,1);
                                break;
                            }
                        }
                }
            }
            return returnstr.ToUpper();
        }

        //ȫ��
        public string convertPY(string chrstr)
        {
            byte[] array = new byte[2];
            string returnstr = "";
            int chrasc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] nowchar = chrstr.ToCharArray();
            for (int j = 0; j < nowchar.Length; j++)
            {
                array = System.Text.Encoding.Default.GetBytes(nowchar[j].ToString());
                if (array.Length < 2) //���ֽ�
                {
                    returnstr += nowchar[j].ToString();
                    continue;
                }
                i1 = (short)(array[0]);
                i2 = (short)(array[1]);
                chrasc = i1 * 256 + i2 - 65536;
                if (chrasc > 0 && chrasc < 160)
                {
                    returnstr += nowchar[j];
                }
                else
                {
                    for (int i = (pyvalue.Length - 1); i >= 0; i--)
                    {
                        if (pyvalue[i] <= chrasc)
                        {
                            returnstr += pystr[i];
                            break;
                        }
                    }
                }
            }
            return returnstr;
        }



    }
}
