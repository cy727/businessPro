namespace business
{
    partial class FormSPDAWH
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSPDAWH));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.printPreviewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolStripButtonADD = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDEL = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEDIT = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewDJMX = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnLocation = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.radioButtonE = new System.Windows.Forms.RadioButton();
            this.radioButtonF = new System.Windows.Forms.RadioButton();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.textBoxMC = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDJMX)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // printPreviewToolStripButton
            // 
            this.printPreviewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printPreviewToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripButton.Image")));
            this.printPreviewToolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.printPreviewToolStripButton.Name = "printPreviewToolStripButton";
            this.printPreviewToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.printPreviewToolStripButton.Text = "打印预览";
            this.printPreviewToolStripButton.Click += new System.EventHandler(this.printPreviewToolStripButton_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripButtonADD,
            this.toolStripButtonDEL,
            this.toolStripButtonEDIT,
            this.toolStripSeparator1,
            this.printToolStripButton,
            this.printPreviewToolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(992, 25);
            this.toolStrip.TabIndex = 6;
            this.toolStrip.Text = "ToolStrip";
            // 
            // ToolStripButtonADD
            // 
            this.ToolStripButtonADD.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButtonADD.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButtonADD.Image")));
            this.ToolStripButtonADD.ImageTransparentColor = System.Drawing.Color.Black;
            this.ToolStripButtonADD.Name = "ToolStripButtonADD";
            this.ToolStripButtonADD.Size = new System.Drawing.Size(23, 22);
            this.ToolStripButtonADD.Text = "新增";
            this.ToolStripButtonADD.Click += new System.EventHandler(this.ToolStripButtonADD_Click);
            // 
            // toolStripButtonDEL
            // 
            this.toolStripButtonDEL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDEL.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDEL.Image")));
            this.toolStripButtonDEL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDEL.Name = "toolStripButtonDEL";
            this.toolStripButtonDEL.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDEL.Text = "删除";
            this.toolStripButtonDEL.Click += new System.EventHandler(this.toolStripButtonDEL_Click);
            // 
            // toolStripButtonEDIT
            // 
            this.toolStripButtonEDIT.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEDIT.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEDIT.Image")));
            this.toolStripButtonEDIT.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEDIT.Name = "toolStripButtonEDIT";
            this.toolStripButtonEDIT.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonEDIT.Text = "修改";
            this.toolStripButtonEDIT.Click += new System.EventHandler(this.toolStripButtonEDIT_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // printToolStripButton
            // 
            this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
            this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.printToolStripButton.Name = "printToolStripButton";
            this.printToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.printToolStripButton.Text = "打印";
            this.printToolStripButton.Click += new System.EventHandler(this.printToolStripButton_Click);
            // 
            // dataGridViewDJMX
            // 
            this.dataGridViewDJMX.AllowUserToAddRows = false;
            this.dataGridViewDJMX.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewDJMX.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewDJMX.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewDJMX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDJMX.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewDJMX.Name = "dataGridViewDJMX";
            this.dataGridViewDJMX.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewDJMX.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewDJMX.RowTemplate.Height = 23;
            this.dataGridViewDJMX.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDJMX.Size = new System.Drawing.Size(992, 432);
            this.dataGridViewDJMX.TabIndex = 8;
            this.dataGridViewDJMX.DoubleClick += new System.EventHandler(this.dataGridViewDJMX_DoubleClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 537);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(992, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelCount
            // 
            this.toolStripStatusLabelCount.Name = "toolStripStatusLabelCount";
            this.toolStripStatusLabelCount.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabelCount.Text = "商品";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewDJMX);
            this.splitContainer1.Size = new System.Drawing.Size(992, 512);
            this.splitContainer1.SplitterDistance = 76;
            this.splitContainer1.TabIndex = 10;
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
            this.groupBox3.Location = new System.Drawing.Point(12, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(630, 66);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "过滤";
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(429, 38);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(61, 21);
            this.btnAll.TabIndex = 7;
            this.btnAll.Text = "全选(F9)";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnLocation
            // 
            this.btnLocation.Location = new System.Drawing.Point(563, 38);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(61, 21);
            this.btnLocation.TabIndex = 6;
            this.btnLocation.Text = "定位(F8)";
            this.btnLocation.UseVisualStyleBackColor = true;
            this.btnLocation.Click += new System.EventHandler(this.btnLocation_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(496, 38);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(61, 21);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "筛选(F7)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // radioButtonE
            // 
            this.radioButtonE.AutoSize = true;
            this.radioButtonE.Location = new System.Drawing.Point(563, 17);
            this.radioButtonE.Name = "radioButtonE";
            this.radioButtonE.Size = new System.Drawing.Size(59, 16);
            this.radioButtonE.TabIndex = 4;
            this.radioButtonE.Text = "后匹配";
            this.radioButtonE.UseVisualStyleBackColor = true;
            // 
            // radioButtonF
            // 
            this.radioButtonF.AutoSize = true;
            this.radioButtonF.Location = new System.Drawing.Point(496, 17);
            this.radioButtonF.Name = "radioButtonF";
            this.radioButtonF.Size = new System.Drawing.Size(59, 16);
            this.radioButtonF.TabIndex = 3;
            this.radioButtonF.Text = "前匹配";
            this.radioButtonF.UseVisualStyleBackColor = true;
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Checked = true;
            this.radioButtonAll.Location = new System.Drawing.Point(429, 17);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(59, 16);
            this.radioButtonAll.TabIndex = 2;
            this.radioButtonAll.TabStop = true;
            this.radioButtonAll.Text = "全匹配";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            // 
            // textBoxMC
            // 
            this.textBoxMC.Location = new System.Drawing.Point(70, 18);
            this.textBoxMC.Name = "textBoxMC";
            this.textBoxMC.Size = new System.Drawing.Size(342, 21);
            this.textBoxMC.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "商品名称:";
            // 
            // FormSPDAWH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 559);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSPDAWH";
            this.Text = "商品档案维护";
            this.Load += new System.EventHandler(this.FormSPDAWH_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDJMX)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton ToolStripButtonADD;
        private System.Windows.Forms.ToolStripButton toolStripButtonDEL;
        private System.Windows.Forms.ToolStripButton toolStripButtonEDIT;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.DataGridView dataGridViewDJMX;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCount;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnLocation;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.RadioButton radioButtonE;
        private System.Windows.Forms.RadioButton radioButtonF;
        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.TextBox textBoxMC;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ToolStripButton printPreviewToolStripButton;
        public System.Windows.Forms.ToolStripButton printToolStripButton;
    }
}