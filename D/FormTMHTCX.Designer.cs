namespace business
{
    partial class FormTMHTCX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTMHTCX));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printPreviewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.btnDel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelWARN = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewTM = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxLX = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnTM = new System.Windows.Forms.Button();
            this.textBoxDJBH = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDJCX = new System.Windows.Forms.Button();
            this.textBoxDJBH1 = new System.Windows.Forms.TextBox();
            this.toolStrip.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTM)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printToolStripButton,
            this.printPreviewToolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.ShowItemToolTips = false;
            this.toolStrip.Size = new System.Drawing.Size(439, 25);
            this.toolStrip.TabIndex = 25;
            this.toolStrip.Text = "ToolStrip";
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
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(56, 565);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(108, 21);
            this.btnDel.TabIndex = 32;
            this.btnDel.Text = "删除条码记录";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelWARN);
            this.groupBox3.Location = new System.Drawing.Point(12, 142);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(414, 60);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "消息提示";
            // 
            // labelWARN
            // 
            this.labelWARN.AutoSize = true;
            this.labelWARN.ForeColor = System.Drawing.Color.Red;
            this.labelWARN.Location = new System.Drawing.Point(22, 27);
            this.labelWARN.Name = "labelWARN";
            this.labelWARN.Size = new System.Drawing.Size(65, 12);
            this.labelWARN.TabIndex = 4;
            this.labelWARN.Text = "          ";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(305, 565);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridViewTM);
            this.groupBox2.Location = new System.Drawing.Point(9, 210);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(417, 348);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "商品条码查询";
            // 
            // dataGridViewTM
            // 
            this.dataGridViewTM.AllowUserToAddRows = false;
            this.dataGridViewTM.AllowUserToDeleteRows = false;
            this.dataGridViewTM.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTM.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTM.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTM.Location = new System.Drawing.Point(3, 15);
            this.dataGridViewTM.Name = "dataGridViewTM";
            this.dataGridViewTM.ReadOnly = true;
            this.dataGridViewTM.RowTemplate.Height = 23;
            this.dataGridViewTM.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTM.Size = new System.Drawing.Size(411, 330);
            this.dataGridViewTM.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxLX);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnTM);
            this.groupBox1.Controls.Add(this.textBoxDJBH);
            this.groupBox1.Location = new System.Drawing.Point(12, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(417, 54);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "合同编码";
            // 
            // comboBoxLX
            // 
            this.comboBoxLX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLX.FormattingEnabled = true;
            this.comboBoxLX.Location = new System.Drawing.Point(44, 22);
            this.comboBoxLX.Name = "comboBoxLX";
            this.comboBoxLX.Size = new System.Drawing.Size(103, 20);
            this.comboBoxLX.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "类型:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(154, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "编号:";
            // 
            // btnTM
            // 
            this.btnTM.Location = new System.Drawing.Point(360, 18);
            this.btnTM.Name = "btnTM";
            this.btnTM.Size = new System.Drawing.Size(52, 23);
            this.btnTM.TabIndex = 1;
            this.btnTM.Text = "查询";
            this.btnTM.UseVisualStyleBackColor = true;
            this.btnTM.Click += new System.EventHandler(this.btnTM_Click);
            // 
            // textBoxDJBH
            // 
            this.textBoxDJBH.Location = new System.Drawing.Point(188, 20);
            this.textBoxDJBH.Name = "textBoxDJBH";
            this.textBoxDJBH.Size = new System.Drawing.Size(167, 21);
            this.textBoxDJBH.TabIndex = 0;
            this.textBoxDJBH.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDJBH_KeyPress);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.btnDJCX);
            this.groupBox4.Controls.Add(this.textBoxDJBH1);
            this.groupBox4.Location = new System.Drawing.Point(12, 86);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(417, 52);
            this.groupBox4.TabIndex = 33;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "其他单据";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "编号:";
            // 
            // btnDJCX
            // 
            this.btnDJCX.Location = new System.Drawing.Point(321, 16);
            this.btnDJCX.Name = "btnDJCX";
            this.btnDJCX.Size = new System.Drawing.Size(90, 23);
            this.btnDJCX.TabIndex = 7;
            this.btnDJCX.Text = "单据条码查询";
            this.btnDJCX.UseVisualStyleBackColor = true;
            this.btnDJCX.Click += new System.EventHandler(this.btnDJCX_Click);
            // 
            // textBoxDJBH1
            // 
            this.textBoxDJBH1.Location = new System.Drawing.Point(63, 17);
            this.textBoxDJBH1.Name = "textBoxDJBH1";
            this.textBoxDJBH1.Size = new System.Drawing.Size(230, 21);
            this.textBoxDJBH1.TabIndex = 6;
            this.textBoxDJBH1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDJBH1_KeyPress);
            // 
            // FormTMHTCX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 597);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTMHTCX";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "合同条码查询";
            this.Load += new System.EventHandler(this.FormTMHTCX_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTM)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelWARN;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewTM;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnTM;
        private System.Windows.Forms.TextBox textBoxDJBH;
        private System.Windows.Forms.ComboBox comboBoxLX;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDJCX;
        private System.Windows.Forms.TextBox textBoxDJBH1;
        public System.Windows.Forms.ToolStripButton printToolStripButton;
        public System.Windows.Forms.ToolStripButton printPreviewToolStripButton;
    }
}