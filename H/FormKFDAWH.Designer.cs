namespace business
{
    partial class FormKFDAWH
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormKFDAWH));
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printPreviewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolStripButtonADD = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDEL = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEDIT = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewDJMX = new System.Windows.Forms.DataGridView();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDJMX)).BeginInit();
            this.SuspendLayout();
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 500);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(692, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
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
            this.toolStrip.Size = new System.Drawing.Size(692, 25);
            this.toolStrip.TabIndex = 9;
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
            this.dataGridViewDJMX.Size = new System.Drawing.Size(692, 497);
            this.dataGridViewDJMX.TabIndex = 11;
            this.dataGridViewDJMX.DoubleClick += new System.EventHandler(this.dataGridViewDJMX_DoubleClick);
            // 
            // FormKFDAWH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 522);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataGridViewDJMX);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormKFDAWH";
            this.Text = "库房档案维护";
            this.Load += new System.EventHandler(this.FormKFDAWH_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDJMX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton ToolStripButtonADD;
        private System.Windows.Forms.ToolStripButton toolStripButtonDEL;
        private System.Windows.Forms.ToolStripButton toolStripButtonEDIT;
        private System.Windows.Forms.DataGridView dataGridViewDJMX;
        public System.Windows.Forms.ToolStripButton printToolStripButton;
        public System.Windows.Forms.ToolStripButton printPreviewToolStripButton;
    }
}