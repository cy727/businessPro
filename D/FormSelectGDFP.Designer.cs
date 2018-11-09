namespace business
{
    partial class FormSelectGDFP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectGDFP));
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.toolStripStatusLabelJE = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.dataGridViewLB = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnALL = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLB)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel3.Text = "开票总额:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(681, 445);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "放弃";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(600, 445);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 8;
            this.btnSelect.Text = "完成";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // toolStripStatusLabelJE
            // 
            this.toolStripStatusLabelJE.Name = "toolStripStatusLabelJE";
            this.toolStripStatusLabelJE.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabelJE.Text = "0";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel3,
            this.toolStripStatusLabelJE});
            this.statusStrip1.Location = new System.Drawing.Point(0, 475);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(773, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // dataGridViewLB
            // 
            this.dataGridViewLB.AllowUserToAddRows = false;
            this.dataGridViewLB.AllowUserToDeleteRows = false;
            this.dataGridViewLB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLB.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewLB.Name = "dataGridViewLB";
            this.dataGridViewLB.Size = new System.Drawing.Size(748, 417);
            this.dataGridViewLB.TabIndex = 0;
            this.dataGridViewLB.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewLB_RowValidating);
            this.dataGridViewLB.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLB_CellContentClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridViewLB);
            this.groupBox2.Location = new System.Drawing.Point(7, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(754, 436);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "勾兑列表";
            // 
            // btnALL
            // 
            this.btnALL.Location = new System.Drawing.Point(25, 445);
            this.btnALL.Name = "btnALL";
            this.btnALL.Size = new System.Drawing.Size(75, 23);
            this.btnALL.TabIndex = 10;
            this.btnALL.Text = "选择全部";
            this.btnALL.UseVisualStyleBackColor = true;
            this.btnALL.Click += new System.EventHandler(this.btnALL_Click);
            // 
            // FormSelectGDFP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 497);
            this.Controls.Add(this.btnALL);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSelectGDFP";
            this.Text = "选择单据（多项选择）";
            this.Load += new System.EventHandler(this.FormSelectGDFP_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLB)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelJE;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView dataGridViewLB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnALL;
    }
}