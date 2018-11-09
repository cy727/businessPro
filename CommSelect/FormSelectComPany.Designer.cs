namespace business
{
    partial class FormSelectComPany
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectComPany));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridViewDWLB = new System.Windows.Forms.DataGridView();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnLocation = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.radioButtonE = new System.Windows.Forms.RadioButton();
            this.radioButtonF = new System.Windows.Forms.RadioButton();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.textBoxDWMC = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDWLB)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridViewDWLB);
            this.groupBox1.Location = new System.Drawing.Point(7, 90);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(630, 395);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "单位列表";
            // 
            // dataGridViewDWLB
            // 
            this.dataGridViewDWLB.AllowUserToAddRows = false;
            this.dataGridViewDWLB.AllowUserToDeleteRows = false;
            this.dataGridViewDWLB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDWLB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDWLB.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewDWLB.MultiSelect = false;
            this.dataGridViewDWLB.Name = "dataGridViewDWLB";
            this.dataGridViewDWLB.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDWLB.Size = new System.Drawing.Size(624, 376);
            this.dataGridViewDWLB.StandardTab = true;
            this.dataGridViewDWLB.TabIndex = 0;
            this.dataGridViewDWLB.DoubleClick += new System.EventHandler(this.dataGridViewDWLB_DoubleClick);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(446, 491);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 10;
            this.btnSelect.Text = "选取";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(537, 491);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAll);
            this.groupBox2.Controls.Add(this.btnLocation);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.radioButtonE);
            this.groupBox2.Controls.Add(this.radioButtonF);
            this.groupBox2.Controls.Add(this.radioButtonAll);
            this.groupBox2.Controls.Add(this.textBoxDWMC);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(7, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(630, 71);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "过滤";
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(429, 41);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(61, 23);
            this.btnAll.TabIndex = 24;
            this.btnAll.TabStop = false;
            this.btnAll.Text = "全选(F9)";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnLocation
            // 
            this.btnLocation.Location = new System.Drawing.Point(563, 41);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(61, 23);
            this.btnLocation.TabIndex = 26;
            this.btnLocation.TabStop = false;
            this.btnLocation.Text = "定位(F8)";
            this.btnLocation.UseVisualStyleBackColor = true;
            this.btnLocation.Click += new System.EventHandler(this.btnLocation_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(496, 41);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(61, 23);
            this.btnSearch.TabIndex = 25;
            this.btnSearch.TabStop = false;
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
            this.radioButtonE.TabIndex = 23;
            this.radioButtonE.Text = "后匹配";
            this.radioButtonE.UseVisualStyleBackColor = true;
            // 
            // radioButtonF
            // 
            this.radioButtonF.AutoSize = true;
            this.radioButtonF.Location = new System.Drawing.Point(496, 18);
            this.radioButtonF.Name = "radioButtonF";
            this.radioButtonF.Size = new System.Drawing.Size(61, 17);
            this.radioButtonF.TabIndex = 22;
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
            this.radioButtonAll.TabIndex = 21;
            this.radioButtonAll.TabStop = true;
            this.radioButtonAll.Text = "全匹配";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            // 
            // textBoxDWMC
            // 
            this.textBoxDWMC.Location = new System.Drawing.Point(70, 28);
            this.textBoxDWMC.Name = "textBoxDWMC";
            this.textBoxDWMC.Size = new System.Drawing.Size(342, 20);
            this.textBoxDWMC.TabIndex = 1;
            this.textBoxDWMC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDWMC_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "单位名称:";
            // 
            // FormSelectComPany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(643, 523);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectComPany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "单位选取";
            this.Load += new System.EventHandler(this.FormSelectComPany_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDWLB)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridViewDWLB;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxDWMC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnLocation;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.RadioButton radioButtonE;
        private System.Windows.Forms.RadioButton radioButtonF;
        private System.Windows.Forms.RadioButton radioButtonAll;
    }
}