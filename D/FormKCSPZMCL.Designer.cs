namespace business
{
    partial class FormKCSPZMCL
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormKCSPZMCL));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printPreviewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelDown = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelMXJLS = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataGridViewDJMX = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labelCZY = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelDJBH = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelZDRQ = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelContent = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxBM = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.comboBoxYWY = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.textBoxBZ = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStrip.SuspendLayout();
            this.tableLayoutPanelDown.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDJMX)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanelContent.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripButton,
            this.toolStripSeparator1,
            this.printToolStripButton,
            this.printPreviewToolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(992, 25);
            this.toolStrip.TabIndex = 11;
            this.toolStrip.Text = "ToolStrip";
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "保存购进商品制单";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
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
            this.printToolStripButton.Text = "打印制单";
            this.printToolStripButton.Click += new System.EventHandler(this.printToolStripButton_Click);
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
            // tableLayoutPanelDown
            // 
            this.tableLayoutPanelDown.ColumnCount = 1;
            this.tableLayoutPanelDown.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDown.Controls.Add(this.statusStrip1, 0, 1);
            this.tableLayoutPanelDown.Controls.Add(this.dataGridViewDJMX, 0, 0);
            this.tableLayoutPanelDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDown.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDown.Name = "tableLayoutPanelDown";
            this.tableLayoutPanelDown.RowCount = 2;
            this.tableLayoutPanelDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanelDown.Size = new System.Drawing.Size(988, 437);
            this.tableLayoutPanelDown.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelMXJLS});
            this.statusStrip1.Location = new System.Drawing.Point(0, 415);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(988, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "单据明细纪录数";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(958, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "商品种类数:";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabelMXJLS
            // 
            this.toolStripStatusLabelMXJLS.Name = "toolStripStatusLabelMXJLS";
            this.toolStripStatusLabelMXJLS.Size = new System.Drawing.Size(15, 17);
            this.toolStripStatusLabelMXJLS.Text = "0";
            // 
            // dataGridViewDJMX
            // 
            this.dataGridViewDJMX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewDJMX.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDJMX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDJMX.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewDJMX.Name = "dataGridViewDJMX";
            this.dataGridViewDJMX.Size = new System.Drawing.Size(982, 408);
            this.dataGridViewDJMX.TabIndex = 0;
            this.dataGridViewDJMX.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDJMX_CellDoubleClick);
            this.dataGridViewDJMX.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewDJMX_CellValidating);
            this.dataGridViewDJMX.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewDJMX_DataError);
            this.dataGridViewDJMX.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewDJMX_RowValidating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "库存商品账目处理";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.labelCZY);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.labelDJBH);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.labelZDRQ);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanelDown);
            this.splitContainer1.Size = new System.Drawing.Size(992, 534);
            this.splitContainer1.SplitterDistance = 92;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 14;
            // 
            // labelCZY
            // 
            this.labelCZY.AutoSize = true;
            this.labelCZY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCZY.Location = new System.Drawing.Point(548, 13);
            this.labelCZY.Name = "labelCZY";
            this.labelCZY.Size = new System.Drawing.Size(43, 13);
            this.labelCZY.TabIndex = 14;
            this.labelCZY.Text = "操作员";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(504, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "操作员:";
            // 
            // labelDJBH
            // 
            this.labelDJBH.AutoSize = true;
            this.labelDJBH.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDJBH.Location = new System.Drawing.Point(393, 13);
            this.labelDJBH.Name = "labelDJBH";
            this.labelDJBH.Size = new System.Drawing.Size(40, 13);
            this.labelDJBH.TabIndex = 12;
            this.labelDJBH.Text = "CTZ临";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(334, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "单据编号:";
            // 
            // labelZDRQ
            // 
            this.labelZDRQ.AutoSize = true;
            this.labelZDRQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZDRQ.Location = new System.Drawing.Point(224, 13);
            this.labelZDRQ.Name = "labelZDRQ";
            this.labelZDRQ.Size = new System.Drawing.Size(91, 13);
            this.labelZDRQ.TabIndex = 10;
            this.labelZDRQ.Text = "2008年12月23日";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(169, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "制单日期:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanelContent);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(988, 54);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "库房选择";
            // 
            // tableLayoutPanelContent
            // 
            this.tableLayoutPanelContent.ColumnCount = 4;
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanelContent.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanelContent.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanelContent.Controls.Add(this.panel3, 2, 0);
            this.tableLayoutPanelContent.Controls.Add(this.panel4, 3, 0);
            this.tableLayoutPanelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelContent.Location = new System.Drawing.Point(3, 15);
            this.tableLayoutPanelContent.Name = "tableLayoutPanelContent";
            this.tableLayoutPanelContent.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.tableLayoutPanelContent.RowCount = 1;
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelContent.Size = new System.Drawing.Size(982, 36);
            this.tableLayoutPanelContent.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBoxBM);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(174, 18);
            this.panel1.TabIndex = 0;
            // 
            // comboBoxBM
            // 
            this.comboBoxBM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBM.FormattingEnabled = true;
            this.comboBoxBM.Location = new System.Drawing.Point(68, 0);
            this.comboBoxBM.MaxDropDownItems = 10;
            this.comboBoxBM.Name = "comboBoxBM";
            this.comboBoxBM.Size = new System.Drawing.Size(103, 20);
            this.comboBoxBM.TabIndex = 3;
            this.comboBoxBM.SelectedIndexChanged += new System.EventHandler(this.comboBoxBM_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "部　　门:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label9);
            this.panel2.Location = new System.Drawing.Point(183, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(64, 18);
            this.panel2.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "业务员:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.comboBoxYWY);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(253, 9);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(326, 18);
            this.panel3.TabIndex = 2;
            // 
            // comboBoxYWY
            // 
            this.comboBoxYWY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxYWY.FormattingEnabled = true;
            this.comboBoxYWY.Location = new System.Drawing.Point(0, 0);
            this.comboBoxYWY.Name = "comboBoxYWY";
            this.comboBoxYWY.Size = new System.Drawing.Size(326, 20);
            this.comboBoxYWY.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.textBoxBZ);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(585, 9);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(394, 18);
            this.panel4.TabIndex = 0;
            // 
            // textBoxBZ
            // 
            this.textBoxBZ.Location = new System.Drawing.Point(40, 0);
            this.textBoxBZ.Name = "textBoxBZ";
            this.textBoxBZ.Size = new System.Drawing.Size(351, 21);
            this.textBoxBZ.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "备注:";
            // 
            // FormKCSPZMCL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 559);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormKCSPZMCL";
            this.Text = "库存商品账目处理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormKCSPZMCL_FormClosing);
            this.Load += new System.EventHandler(this.FormKCSPZMCL_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tableLayoutPanelDown.ResumeLayout(false);
            this.tableLayoutPanelDown.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDJMX)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanelContent.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDown;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMXJLS;
        private System.Windows.Forms.DataGridView dataGridViewDJMX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelContent;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label labelCZY;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelDJBH;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelZDRQ;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxBM;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxYWY;
        private System.Windows.Forms.TextBox textBoxBZ;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ToolStripButton printToolStripButton;
        public System.Windows.Forms.ToolStripButton printPreviewToolStripButton;

    }
}