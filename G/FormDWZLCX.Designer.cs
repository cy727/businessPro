namespace business
{
    partial class FormDWZLCX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDWZLCX));
            this.labelCZY = new System.Windows.Forms.Label();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonGD = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printPreviewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBoxDWMC = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanelDown = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelDown = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelMXJLS = new System.Windows.Forms.ToolStripStatusLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.labelZDRQ = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDWBH = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanelContent = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanelDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelDown.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanelContent.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelCZY
            // 
            this.labelCZY.AutoSize = true;
            this.labelCZY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCZY.Location = new System.Drawing.Point(350, 14);
            this.labelCZY.Name = "labelCZY";
            this.labelCZY.Size = new System.Drawing.Size(43, 13);
            this.labelCZY.TabIndex = 12;
            this.labelCZY.Text = "操作员";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonGD,
            this.toolStripSeparator2,
            this.printToolStripButton,
            this.printPreviewToolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(992, 25);
            this.toolStrip.TabIndex = 19;
            this.toolStrip.Text = "ToolStrip";
            // 
            // toolStripButtonGD
            // 
            this.toolStripButtonGD.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGD.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGD.Image")));
            this.toolStripButtonGD.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGD.Name = "toolStripButtonGD";
            this.toolStripButtonGD.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonGD.Text = "查询";
            this.toolStripButtonGD.Click += new System.EventHandler(this.toolStripButtonGD_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // panel3
            // 
            this.panel3.Controls.Add(this.textBoxDWMC);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(283, 9);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(696, 22);
            this.panel3.TabIndex = 2;
            // 
            // textBoxDWMC
            // 
            this.textBoxDWMC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDWMC.Location = new System.Drawing.Point(0, 0);
            this.textBoxDWMC.Name = "textBoxDWMC";
            this.textBoxDWMC.Size = new System.Drawing.Size(696, 21);
            this.textBoxDWMC.TabIndex = 0;
            this.textBoxDWMC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDWMC_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(213, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "单位名称:";
            // 
            // tableLayoutPanelDown
            // 
            this.tableLayoutPanelDown.ColumnCount = 1;
            this.tableLayoutPanelDown.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDown.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanelDown.Controls.Add(this.panelDown, 0, 1);
            this.tableLayoutPanelDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDown.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDown.Name = "tableLayoutPanelDown";
            this.tableLayoutPanelDown.RowCount = 2;
            this.tableLayoutPanelDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanelDown.Size = new System.Drawing.Size(988, 429);
            this.tableLayoutPanelDown.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(982, 398);
            this.dataGridView1.TabIndex = 0;
            // 
            // panelDown
            // 
            this.panelDown.Controls.Add(this.statusStrip1);
            this.panelDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDown.Location = new System.Drawing.Point(3, 407);
            this.panelDown.Name = "panelDown";
            this.panelDown.Size = new System.Drawing.Size(982, 19);
            this.panelDown.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelMXJLS});
            this.statusStrip1.Location = new System.Drawing.Point(0, -3);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(982, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "单据明细纪录数";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(952, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "纪录数:";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabelMXJLS
            // 
            this.toolStripStatusLabelMXJLS.Name = "toolStripStatusLabelMXJLS";
            this.toolStripStatusLabelMXJLS.Size = new System.Drawing.Size(15, 17);
            this.toolStripStatusLabelMXJLS.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(298, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "操作员:";
            // 
            // labelZDRQ
            // 
            this.labelZDRQ.AutoSize = true;
            this.labelZDRQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZDRQ.Location = new System.Drawing.Point(186, 14);
            this.labelZDRQ.Name = "labelZDRQ";
            this.labelZDRQ.Size = new System.Drawing.Size(91, 13);
            this.labelZDRQ.TabIndex = 10;
            this.labelZDRQ.Text = "2008年12月23日";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(131, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "当前日期:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "单位资料查询";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "单位编号:";
            // 
            // textBoxDWBH
            // 
            this.textBoxDWBH.Location = new System.Drawing.Point(67, 0);
            this.textBoxDWBH.Name = "textBoxDWBH";
            this.textBoxDWBH.Size = new System.Drawing.Size(135, 21);
            this.textBoxDWBH.TabIndex = 2;
            this.textBoxDWBH.DoubleClick += new System.EventHandler(this.textBoxDWBH_DoubleClick);
            this.textBoxDWBH.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDWBH_KeyPress);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBoxDWBH);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(274, 22);
            this.panel2.TabIndex = 1;
            // 
            // tableLayoutPanelContent
            // 
            this.tableLayoutPanelContent.ColumnCount = 2;
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelContent.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanelContent.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelContent.Location = new System.Drawing.Point(3, 15);
            this.tableLayoutPanelContent.Name = "tableLayoutPanelContent";
            this.tableLayoutPanelContent.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.tableLayoutPanelContent.RowCount = 1;
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelContent.Size = new System.Drawing.Size(982, 40);
            this.tableLayoutPanelContent.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanelContent);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(988, 57);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询内容";
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
            this.splitContainer1.Panel1.Controls.Add(this.labelZDRQ);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanelDown);
            this.splitContainer1.Size = new System.Drawing.Size(992, 534);
            this.splitContainer1.SplitterDistance = 100;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 20;
            // 
            // FormDWZLCX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 559);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDWZLCX";
            this.Text = "单位资料查询";
            this.Load += new System.EventHandler(this.FormDWZLCX_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tableLayoutPanelDown.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelDown.ResumeLayout(false);
            this.panelDown.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanelContent.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCZY;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonGD;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBoxDWMC;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDown;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panelDown;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMXJLS;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelZDRQ;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDWBH;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelContent;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.ToolStripButton printToolStripButton;
        public System.Windows.Forms.ToolStripButton printPreviewToolStripButton;
    }
}