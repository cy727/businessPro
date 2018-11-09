namespace business
{
    partial class FormSPFLWH
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSPFLWH));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDEL = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxFLBH = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxKFMC = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxKFBH = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxSJFL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxFLMC = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.treeViewComm = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEDIT = new System.Windows.Forms.Button();
            this.btnADD = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(502, 363);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDEL
            // 
            this.btnDEL.Location = new System.Drawing.Point(235, 363);
            this.btnDEL.Name = "btnDEL";
            this.btnDEL.Size = new System.Drawing.Size(104, 25);
            this.btnDEL.TabIndex = 6;
            this.btnDEL.Text = "删除";
            this.btnDEL.UseVisualStyleBackColor = true;
            this.btnDEL.Click += new System.EventHandler(this.btnDEL_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxFLBH);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBoxKFMC);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxKFBH);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxSJFL);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBoxFLMC);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(235, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(345, 343);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "分类明细";
            // 
            // textBoxFLBH
            // 
            this.textBoxFLBH.Location = new System.Drawing.Point(89, 38);
            this.textBoxFLBH.Name = "textBoxFLBH";
            this.textBoxFLBH.Size = new System.Drawing.Size(99, 20);
            this.textBoxFLBH.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "分类编号:";
            // 
            // textBoxKFMC
            // 
            this.textBoxKFMC.Location = new System.Drawing.Point(89, 181);
            this.textBoxKFMC.Name = "textBoxKFMC";
            this.textBoxKFMC.Size = new System.Drawing.Size(228, 20);
            this.textBoxKFMC.TabIndex = 7;
            this.textBoxKFMC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxKFMC_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "库房名称:";
            // 
            // textBoxKFBH
            // 
            this.textBoxKFBH.Location = new System.Drawing.Point(89, 145);
            this.textBoxKFBH.Name = "textBoxKFBH";
            this.textBoxKFBH.Size = new System.Drawing.Size(99, 20);
            this.textBoxKFBH.TabIndex = 5;
            this.textBoxKFBH.DoubleClick += new System.EventHandler(this.textBoxKFBH_DoubleClick);
            this.textBoxKFBH.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxKFBH_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "库房编号:";
            // 
            // textBoxSJFL
            // 
            this.textBoxSJFL.Location = new System.Drawing.Point(89, 109);
            this.textBoxSJFL.Name = "textBoxSJFL";
            this.textBoxSJFL.ReadOnly = true;
            this.textBoxSJFL.Size = new System.Drawing.Size(228, 20);
            this.textBoxSJFL.TabIndex = 3;
            this.textBoxSJFL.DoubleClick += new System.EventHandler(this.textBoxSJFL_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "上级分类:";
            // 
            // textBoxFLMC
            // 
            this.textBoxFLMC.Location = new System.Drawing.Point(89, 74);
            this.textBoxFLMC.Name = "textBoxFLMC";
            this.textBoxFLMC.Size = new System.Drawing.Size(228, 20);
            this.textBoxFLMC.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "分类名称:";
            // 
            // imageListTree
            // 
            this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTree.Images.SetKeyName(0, "CLSDFOLD.ICO");
            this.imageListTree.Images.SetKeyName(1, "OPENFOLD.ICO");
            // 
            // treeViewComm
            // 
            this.treeViewComm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewComm.ImageIndex = 0;
            this.treeViewComm.ImageList = this.imageListTree;
            this.treeViewComm.Location = new System.Drawing.Point(3, 16);
            this.treeViewComm.Name = "treeViewComm";
            this.treeViewComm.SelectedImageIndex = 0;
            this.treeViewComm.Size = new System.Drawing.Size(211, 328);
            this.treeViewComm.TabIndex = 0;
            this.treeViewComm.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewComm_AfterSelect);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeViewComm);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(217, 347);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "商品分类";
            // 
            // btnEDIT
            // 
            this.btnEDIT.Location = new System.Drawing.Point(348, 363);
            this.btnEDIT.Name = "btnEDIT";
            this.btnEDIT.Size = new System.Drawing.Size(104, 25);
            this.btnEDIT.TabIndex = 8;
            this.btnEDIT.Text = "修改";
            this.btnEDIT.UseVisualStyleBackColor = true;
            this.btnEDIT.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnADD
            // 
            this.btnADD.Location = new System.Drawing.Point(122, 363);
            this.btnADD.Name = "btnADD";
            this.btnADD.Size = new System.Drawing.Size(104, 25);
            this.btnADD.TabIndex = 9;
            this.btnADD.Text = "增加";
            this.btnADD.UseVisualStyleBackColor = true;
            this.btnADD.Click += new System.EventHandler(this.btnADD_Click);
            // 
            // FormSPFLWH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 397);
            this.Controls.Add(this.btnADD);
            this.Controls.Add(this.btnEDIT);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDEL);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSPFLWH";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "商品分类维护";
            this.Load += new System.EventHandler(this.FormSPFLWH_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDEL;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ImageList imageListTree;
        private System.Windows.Forms.TreeView treeViewComm;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnEDIT;
        private System.Windows.Forms.Button btnADD;
        private System.Windows.Forms.TextBox textBoxFLBH;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxKFMC;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxKFBH;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxSJFL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxFLMC;
        private System.Windows.Forms.Label label1;
    }
}