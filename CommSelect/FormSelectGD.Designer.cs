namespace business
{
    partial class FormSelectGD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectGD));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxMC = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewLB = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelJE = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSL = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnALL = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLB)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxMC);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxLB);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(747, 47);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "商品选择";
            // 
            // textBoxMC
            // 
            this.textBoxMC.Location = new System.Drawing.Point(252, 16);
            this.textBoxMC.Name = "textBoxMC";
            this.textBoxMC.Size = new System.Drawing.Size(480, 21);
            this.textBoxMC.TabIndex = 3;
            this.textBoxMC.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxMC_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(188, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "商品名称:";
            // 
            // textBoxLB
            // 
            this.textBoxLB.Location = new System.Drawing.Point(71, 16);
            this.textBoxLB.Name = "textBoxLB";
            this.textBoxLB.Size = new System.Drawing.Size(100, 21);
            this.textBoxLB.TabIndex = 1;
            this.textBoxLB.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxLB_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "商品类别:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridViewLB);
            this.groupBox2.Location = new System.Drawing.Point(13, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(747, 394);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "勾兑列表";
            // 
            // dataGridViewLB
            // 
            this.dataGridViewLB.AllowUserToAddRows = false;
            this.dataGridViewLB.AllowUserToDeleteRows = false;
            this.dataGridViewLB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLB.Location = new System.Drawing.Point(3, 15);
            this.dataGridViewLB.Name = "dataGridViewLB";
            this.dataGridViewLB.RowTemplate.Height = 23;
            this.dataGridViewLB.Size = new System.Drawing.Size(741, 377);
            this.dataGridViewLB.TabIndex = 0;
            this.dataGridViewLB.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLB_CellContentClick);
            this.dataGridViewLB.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewLB_CellValidating);
            this.dataGridViewLB.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewLB_DataError);
            this.dataGridViewLB.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewLB_RowValidating);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelJE,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabelSL});
            this.statusStrip1.Location = new System.Drawing.Point(0, 500);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(772, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(95, 17);
            this.toolStripStatusLabel1.Text = "将支付金额合计:";
            // 
            // toolStripStatusLabelJE
            // 
            this.toolStripStatusLabelJE.Name = "toolStripStatusLabelJE";
            this.toolStripStatusLabelJE.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabelJE.Text = "0.00";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(24, 17);
            this.toolStripStatusLabel2.Text = "    ";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(95, 17);
            this.toolStripStatusLabel3.Text = "将支付数量合计:";
            // 
            // toolStripStatusLabelSL
            // 
            this.toolStripStatusLabelSL.Name = "toolStripStatusLabelSL";
            this.toolStripStatusLabelSL.Size = new System.Drawing.Size(15, 17);
            this.toolStripStatusLabelSL.Text = "0";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(575, 470);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 21);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "完成";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(656, 470);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 21);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "放弃";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnALL
            // 
            this.btnALL.Location = new System.Drawing.Point(39, 470);
            this.btnALL.Name = "btnALL";
            this.btnALL.Size = new System.Drawing.Size(75, 21);
            this.btnALL.TabIndex = 5;
            this.btnALL.Text = "结清全部";
            this.btnALL.UseVisualStyleBackColor = true;
            this.btnALL.Click += new System.EventHandler(this.btnALL_Click);
            // 
            // FormSelectGD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(772, 522);
            this.Controls.Add(this.btnALL);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectGD";
            this.ShowInTaskbar = false;
            this.Text = "勾兑";
            this.Load += new System.EventHandler(this.FormSelectGD_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLB)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewLB;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelJE;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSL;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnALL;
    }
}