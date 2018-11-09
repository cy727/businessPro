using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Xml;


namespace business
{
    class ClassChangeSkins
    {
        public static Sunisoft.IrisSkin.SkinEngine skinEngineSel = null;
        private static System.Data.DataSet dSetSkin = new DataSet();
        public int i=1;

        public void AddSkinMenu(ToolStripMenuItem toolMenu)
        {
            int i;
            string dFileName = "";


            //初始化皮肤信息
            dFileName = Directory.GetCurrentDirectory() + "\\skin.xml";
            if (File.Exists(dFileName)) //存在文件
            {
                dSetSkin.ReadXml(dFileName);
            }

            //初始化皮肤菜单
            if (dSetSkin.Tables.Contains("皮肤")) //存在皮肤文件
            {
                for (i = 0; i < dSetSkin.Tables["皮肤"].Rows.Count; i++)
                {
                    ToolStripMenuItem mItem = new ToolStripMenuItem();
                    mItem.Text = dSetSkin.Tables["皮肤"].Rows[i][0].ToString();
                    if (dSetSkin.Tables["皮肤"].Rows[i][1].ToString() == "1") //缺省皮肤
                    {
                        mItem.Checked = true;
                        //更换皮肤
                        if (dSetSkin.Tables["皮肤"].Rows[i][2].ToString() != "")
                        {
                            dFileName = Directory.GetCurrentDirectory() + "\\SKINS\\" + dSetSkin.Tables["皮肤"].Rows[i][2].ToString();
                            if (File.Exists(dFileName)) //存在文件
                            {
                                ChangeSkin(dFileName);
                            }

                        }

                    }
                    mItem.Click += new EventHandler(mItem_Click);
                    toolMenu.DropDownItems.Add(mItem);
                }
            }
        }

        static void mItem_Click(object sender, EventArgs e)
        {

            int i;
            string dFileName = "";

            ToolStripMenuItem mItem=(ToolStripMenuItem)sender;
            ToolStripMenuItem mP = (ToolStripMenuItem)mItem.OwnerItem;

            foreach (ToolStripMenuItem mI in mP.DropDownItems)
            {
                mI.Checked = false;
            }
            mItem.Checked = true;

            if (dSetSkin.Tables.Contains("皮肤")) //存在皮肤文件
            {
                for (i = 0; i < dSetSkin.Tables["皮肤"].Rows.Count; i++)
                {
                    if (dSetSkin.Tables["皮肤"].Rows[i][0].ToString() == mItem.Text) //得到点击菜单
                    {
                        dSetSkin.Tables["皮肤"].Rows[i][1] = "1";
                        //调用皮肤
                        if (dSetSkin.Tables["皮肤"].Rows[i][2].ToString() != "")
                        {
                            dFileName = Directory.GetCurrentDirectory() + "\\SKINS\\" + dSetSkin.Tables["皮肤"].Rows[i][2].ToString();
                            if (File.Exists(dFileName)) //存在文件
                            {
                                ChangeSkin(dFileName);
                            }

                        }
                        else
                        {
                            RemoveSkin();
                        }
                    }
                    else //非点击菜单
                    {
                        dSetSkin.Tables["皮肤"].Rows[i][1] = "0";
                    }


                }
            }

        }
        public static void ChangeSkin(string skinName)
        {
            System.Reflection.Assembly thisDll = System.Reflection.Assembly.GetExecutingAssembly();
            if (skinEngineSel == null)
            {
                skinEngineSel = new Sunisoft.IrisSkin.SkinEngine(Application.OpenForms[0]);
                skinEngineSel.SkinFile = skinName;
                skinEngineSel.Active = true;
                for (int i = 0; i < Application.OpenForms.Count; i++)
                {
                    skinEngineSel.AddForm(Application.OpenForms[i]);
                }

            }
            else
            {
                skinEngineSel.SkinFile = skinName;
                skinEngineSel.Active = true;
            }
        }

        public static void RemoveSkin()
        {
            if (skinEngineSel == null)
            {
                return;
            }
            else
            {
                skinEngineSel.Active = false;
            }
        }

        public void SveSkins()
        {
            string dFileName = "";

            //保存皮肤
            if (dSetSkin.Tables.Contains("皮肤")) //存在皮肤文件
            {
                dFileName = Directory.GetCurrentDirectory() + "\\skin.xml";
                if (File.Exists(dFileName)) //存在文件
                {
                    dSetSkin.Tables["皮肤"].WriteXml(dFileName);
                }

            }
        }




    }
}
