namespace business
{
    partial class FormAccessLimit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccessLimit));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPASS = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonDEL = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEDIT = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelGW = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewGW = new System.Windows.Forms.DataGridView();
            this.dataGridViewMK = new System.Windows.Forms.DataGridView();
            this.toolStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMK)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripButton,
            this.toolStripButtonPASS,
            this.toolStripSeparator1,
            this.toolStripButtonDEL,
            this.toolStripButtonEDIT});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(746, 25);
            this.toolStrip.TabIndex = 12;
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
            // toolStripButtonPASS
            // 
            this.toolStripButtonPASS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPASS.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPASS.Image")));
            this.toolStripButtonPASS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPASS.Name = "toolStripButtonPASS";
            this.toolStripButtonPASS.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPASS.Text = "修改授权密码";
            this.toolStripButtonPASS.Click += new System.EventHandler(this.toolStripButtonPASS_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            this.toolStripButtonEDIT.Text = "授权";
            this.toolStripButtonEDIT.Click += new System.EventHandler(this.toolStripButtonEDIT_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelGW});
            this.statusStrip1.Location = new System.Drawing.Point(0, 502);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(746, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelGW
            // 
            this.toolStripStatusLabelGW.Name = "toolStripStatusLabelGW";
            this.toolStripStatusLabelGW.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabelGW.Text = "岗位";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewGW);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewMK);
            this.splitContainer1.Size = new System.Drawing.Size(746, 477);
            this.splitContainer1.SplitterDistance = 247;
            this.splitContainer1.TabIndex = 14;
            // 
            // dataGridViewGW
            // 
            this.dataGridViewGW.AllowUserToAddRows = false;
            this.dataGridViewGW.AllowUserToDeleteRows = false;
            this.dataGridViewGW.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewGW.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewGW.MultiSelect = false;
            this.dataGridViewGW.Name = "dataGridViewGW";
            this.dataGridViewGW.RowTemplate.Height = 23;
            this.dataGridViewGW.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewGW.Size = new System.Drawing.Size(247, 477);
            this.dataGridViewGW.TabIndex = 0;
            this.dataGridViewGW.SelectionChanged += new System.EventHandler(this.dataGridViewGW_SelectionChanged);
            // 
            // dataGridViewMK
            // 
            this.dataGridViewMK.AllowUserToAddRows = false;
            this.dataGridViewMK.AllowUserToDeleteRows = false;
            this.dataGridViewMK.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMK.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMK.Name = "dataGridViewMK";
            this.dataGridViewMK.RowTemplate.Height = 23;
            this.dataGridViewMK.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMK.Size = new System.Drawing.Size(495, 477);
            this.dataGridViewMK.TabIndex = 1;
            // 
            // FormAccessLimit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 524);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAccessLimit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "权限管理";
            this.Load += new System.EventHandler(this.FormAccessLimit_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMK)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelGW;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridViewGW;
        private System.Windows.Forms.DataGridView dataGridViewMK;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonDEL;
        private System.Windows.Forms.ToolStripButton toolStripButtonEDIT;
        private System.Windows.Forms.ToolStripButton toolStripButtonPASS;
    }
}