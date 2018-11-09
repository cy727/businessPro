namespace business
{
    partial class FormWLDWYEDJ
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWLDWYEDJ));
            this.printPreviewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewDJMX = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonEDIT = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDJMX)).BeginInit();
            this.toolStrip.SuspendLayout();
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
            this.dataGridViewDJMX.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDJMX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDJMX.Location = new System.Drawing.Point(0, 25);
            this.dataGridViewDJMX.Name = "dataGridViewDJMX";
            this.dataGridViewDJMX.Size = new System.Drawing.Size(792, 475);
            this.dataGridViewDJMX.TabIndex = 8;
            this.dataGridViewDJMX.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewDJMX_DataError);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 500);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonEDIT,
            this.toolStripSeparator1,
            this.printToolStripButton,
            this.printPreviewToolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(792, 25);
            this.toolStrip.TabIndex = 6;
            this.toolStrip.Text = "ToolStrip";
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
            // FormWLDWYEDJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 522);
            this.Controls.Add(this.dataGridViewDJMX);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWLDWYEDJ";
            this.Text = "往来单位余额登记";
            this.Load += new System.EventHandler(this.FormWLDWYEDJ_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDJMX)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewDJMX;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonEDIT;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripButton printPreviewToolStripButton;
        public System.Windows.Forms.ToolStripButton printToolStripButton;
    }
}