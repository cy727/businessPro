namespace business
{
    partial class FormSelectCommodities
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectCommodities));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.treeViewComm = new System.Windows.Forms.TreeView();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewComm = new System.Windows.Forms.DataGridView();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnLocation = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.radioButtonE = new System.Windows.Forms.RadioButton();
            this.radioButtonF = new System.Windows.Forms.RadioButton();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.textBoxMC = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComm)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeViewComm);
            this.groupBox1.Location = new System.Drawing.Point(10, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(217, 371);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "商品分类";
            // 
            // treeViewComm
            // 
            this.treeViewComm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewComm.ImageIndex = 0;
            this.treeViewComm.ImageList = this.imageListTree;
            this.treeViewComm.Location = new System.Drawing.Point(3, 16);
            this.treeViewComm.Name = "treeViewComm";
            this.treeViewComm.SelectedImageIndex = 0;
            this.treeViewComm.Size = new System.Drawing.Size(211, 352);
            this.treeViewComm.TabIndex = 0;
            this.treeViewComm.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewComm_AfterSelect);
            // 
            // imageListTree
            // 
            this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTree.Images.SetKeyName(0, "CLSDFOLD.ICO");
            this.imageListTree.Images.SetKeyName(1, "OPENFOLD.ICO");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridViewComm);
            this.groupBox2.Location = new System.Drawing.Point(230, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(410, 370);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "商品列表";
            // 
            // dataGridViewComm
            // 
            this.dataGridViewComm.AllowUserToAddRows = false;
            this.dataGridViewComm.AllowUserToDeleteRows = false;
            this.dataGridViewComm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewComm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewComm.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewComm.MultiSelect = false;
            this.dataGridViewComm.Name = "dataGridViewComm";
            this.dataGridViewComm.ReadOnly = true;
            this.dataGridViewComm.RowTemplate.Height = 23;
            this.dataGridViewComm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewComm.Size = new System.Drawing.Size(404, 351);
            this.dataGridViewComm.StandardTab = true;
            this.dataGridViewComm.TabIndex = 1;
            this.dataGridViewComm.DoubleClick += new System.EventHandler(this.dataGridViewComm_DoubleClick);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(386, 469);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(104, 25);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "选择";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(502, 469);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnAll);
            this.groupBox3.Controls.Add(this.btnLocation);
            this.groupBox3.Controls.Add(this.btnSearch);
            this.groupBox3.Controls.Add(this.radioButtonE);
            this.groupBox3.Controls.Add(this.radioButtonF);
            this.groupBox3.Controls.Add(this.radioButtonAll);
            this.groupBox3.Controls.Add(this.textBoxMC);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(10, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(630, 71);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "过滤";
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(429, 41);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(61, 23);
            this.btnAll.TabIndex = 7;
            this.btnAll.Text = "全选(F9)";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnLocation
            // 
            this.btnLocation.Location = new System.Drawing.Point(563, 41);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(61, 23);
            this.btnLocation.TabIndex = 6;
            this.btnLocation.Text = "定位(F8)";
            this.btnLocation.UseVisualStyleBackColor = true;
            this.btnLocation.Click += new System.EventHandler(this.btnLocation_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(496, 41);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(61, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "筛选(F7)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // radioButtonE
            // 
            this.radioButtonE.AutoSize = true;
            this.radioButtonE.Location = new System.Drawing.Point(563, 18);
            this.radioButtonE.Name = "radioButtonE";
            this.radioButtonE.Size = new System.Drawing.Size(61, 17);
            this.radioButtonE.TabIndex = 4;
            this.radioButtonE.Text = "后匹配";
            this.radioButtonE.UseVisualStyleBackColor = true;
            // 
            // radioButtonF
            // 
            this.radioButtonF.AutoSize = true;
            this.radioButtonF.Location = new System.Drawing.Point(496, 18);
            this.radioButtonF.Name = "radioButtonF";
            this.radioButtonF.Size = new System.Drawing.Size(61, 17);
            this.radioButtonF.TabIndex = 3;
            this.radioButtonF.Text = "前匹配";
            this.radioButtonF.UseVisualStyleBackColor = true;
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Checked = true;
            this.radioButtonAll.Location = new System.Drawing.Point(429, 18);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(61, 17);
            this.radioButtonAll.TabIndex = 2;
            this.radioButtonAll.TabStop = true;
            this.radioButtonAll.Text = "全匹配";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            // 
            // textBoxMC
            // 
            this.textBoxMC.Location = new System.Drawing.Point(70, 28);
            this.textBoxMC.Name = "textBoxMC";
            this.textBoxMC.Size = new System.Drawing.Size(342, 20);
            this.textBoxMC.TabIndex = 1;
            this.textBoxMC.TextChanged += new System.EventHandler(this.textBoxMC_TextChanged);
            this.textBoxMC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMC_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "商品名称:";
            // 
            // FormSelectCommodities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(651, 506);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectCommodities";
            this.Text = "商品选取";
            this.Load += new System.EventHandler(this.FormSelectCommodities_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComm)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView treeViewComm;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewComm;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ImageList imageListTree;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnLocation;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.RadioButton radioButtonE;
        private System.Windows.Forms.RadioButton radioButtonF;
        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.TextBox textBoxMC;
        private System.Windows.Forms.Label label1;
    }
}