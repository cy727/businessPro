namespace business
{
    partial class FormZBDAWH
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormZBDAWH));
            this.printPreviewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolStripButtonADD = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDEL = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEDIT = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.dataGridViewDJMX = new System.Windows.Forms.DataGridView();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDJMX)).BeginInit();
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
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 537);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(992, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // dataGridViewDJMX
            // 
            this.dataGridViewDJMX.AllowUserToAddRows = false;
            this.dataGridViewDJMX.AllowUserToDeleteRows = false;
            this.dataGridViewDJMX.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewDJMX.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDJMX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDJMX.Location = new System.Drawing.Point(0, 25);
            this.dataGridViewDJMX.Name = "dataGridViewDJMX";
            this.dataGridViewDJMX.ReadOnly = true;
            this.dataGridViewDJMX.RowTemplate.Height = 23;
            this.dataGridViewDJMX.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDJMX.Size = new System.Drawing.Size(992, 512);
            this.dataGridViewDJMX.TabIndex = 9;
            // 
            // FormZBDAWH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 559);
            this.Controls.Add(this.dataGridViewDJMX);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormZBDAWH";
            this.Text = "帐簿档案维护";
            this.Load += new System.EventHandler(this.FormZBDAWH_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDJMX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton ToolStripButtonADD;
        private System.Windows.Forms.ToolStripButton toolStripButtonDEL;
        private System.Windows.Forms.ToolStripButton toolStripButtonEDIT;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView dataGridViewDJMX;
        public System.Windows.Forms.ToolStripButton printPreviewToolStripButton;
        public System.Windows.Forms.ToolStripButton printToolStripButton;
    }
}