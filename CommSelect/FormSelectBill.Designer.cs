namespace business
{
    partial class FormSelectBill
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectBill));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridViewBILL = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonQX = new System.Windows.Forms.Button();
            this.buttonSX = new System.Windows.Forms.Button();
            this.textBoxJSSJ = new System.Windows.Forms.TextBox();
            this.textBoxKSSJ = new System.Windows.Forms.TextBox();
            this.comboBoxYWY = new System.Windows.Forms.ComboBox();
            this.textBoxDJBH = new System.Windows.Forms.TextBox();
            this.textBoxDWMC = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProviderS = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBILL)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridViewBILL);
            this.groupBox1.Location = new System.Drawing.Point(15, 116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(770, 365);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "单据列表";
            // 
            // dataGridViewBILL
            // 
            this.dataGridViewBILL.AllowUserToAddRows = false;
            this.dataGridViewBILL.AllowUserToDeleteRows = false;
            this.dataGridViewBILL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBILL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBILL.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewBILL.Location = new System.Drawing.Point(3, 15);
            this.dataGridViewBILL.MultiSelect = false;
            this.dataGridViewBILL.Name = "dataGridViewBILL";
            this.dataGridViewBILL.ReadOnly = true;
            this.dataGridViewBILL.RowTemplate.Height = 23;
            this.dataGridViewBILL.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBILL.Size = new System.Drawing.Size(764, 347);
            this.dataGridViewBILL.StandardTab = true;
            this.dataGridViewBILL.TabIndex = 0;
            this.dataGridViewBILL.DoubleClick += new System.EventHandler(this.dataGridViewBILL_DoubleClick);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(710, 486);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 21);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(629, 486);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 21);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "选取";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonQX);
            this.groupBox2.Controls.Add(this.buttonSX);
            this.groupBox2.Controls.Add(this.textBoxJSSJ);
            this.groupBox2.Controls.Add(this.textBoxKSSJ);
            this.groupBox2.Controls.Add(this.comboBoxYWY);
            this.groupBox2.Controls.Add(this.textBoxDJBH);
            this.groupBox2.Controls.Add(this.textBoxDWMC);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(15, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(764, 99);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "单据筛选";
            // 
            // buttonQX
            // 
            this.buttonQX.Location = new System.Drawing.Point(661, 66);
            this.buttonQX.Name = "buttonQX";
            this.buttonQX.Size = new System.Drawing.Size(75, 21);
            this.buttonQX.TabIndex = 11;
            this.buttonQX.TabStop = false;
            this.buttonQX.Text = "全选(F9)";
            this.buttonQX.UseVisualStyleBackColor = true;
            this.buttonQX.Click += new System.EventHandler(this.buttonQX_Click);
            // 
            // buttonSX
            // 
            this.buttonSX.Location = new System.Drawing.Point(580, 66);
            this.buttonSX.Name = "buttonSX";
            this.buttonSX.Size = new System.Drawing.Size(75, 21);
            this.buttonSX.TabIndex = 10;
            this.buttonSX.TabStop = false;
            this.buttonSX.Text = "筛选(F7)";
            this.buttonSX.UseVisualStyleBackColor = true;
            this.buttonSX.Click += new System.EventHandler(this.buttonSX_Click);
            // 
            // textBoxJSSJ
            // 
            this.textBoxJSSJ.Location = new System.Drawing.Point(348, 68);
            this.textBoxJSSJ.Name = "textBoxJSSJ";
            this.textBoxJSSJ.Size = new System.Drawing.Size(203, 21);
            this.textBoxJSSJ.TabIndex = 9;
            this.textBoxJSSJ.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxJSSJ_Validating);
            // 
            // textBoxKSSJ
            // 
            this.textBoxKSSJ.Location = new System.Drawing.Point(86, 68);
            this.textBoxKSSJ.Name = "textBoxKSSJ";
            this.textBoxKSSJ.Size = new System.Drawing.Size(183, 21);
            this.textBoxKSSJ.TabIndex = 8;
            this.textBoxKSSJ.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxKSSJ_Validating);
            // 
            // comboBoxYWY
            // 
            this.comboBoxYWY.FormattingEnabled = true;
            this.comboBoxYWY.Location = new System.Drawing.Point(348, 42);
            this.comboBoxYWY.Name = "comboBoxYWY";
            this.comboBoxYWY.Size = new System.Drawing.Size(200, 20);
            this.comboBoxYWY.TabIndex = 7;
            this.comboBoxYWY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxS_KeyPress);
            // 
            // textBoxDJBH
            // 
            this.textBoxDJBH.Location = new System.Drawing.Point(85, 43);
            this.textBoxDJBH.Name = "textBoxDJBH";
            this.textBoxDJBH.Size = new System.Drawing.Size(184, 21);
            this.textBoxDJBH.TabIndex = 6;
            this.textBoxDJBH.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxS_KeyPress);
            // 
            // textBoxDWMC
            // 
            this.textBoxDWMC.Location = new System.Drawing.Point(85, 16);
            this.textBoxDWMC.Name = "textBoxDWMC";
            this.textBoxDWMC.Size = new System.Drawing.Size(466, 21);
            this.textBoxDWMC.TabIndex = 5;
            this.textBoxDWMC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxS_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(284, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "结束时间:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "开始时间:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(296, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "业务员:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "单据编号:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "单位名称:";
            // 
            // errorProviderS
            // 
            this.errorProviderS.ContainerControl = this;
            // 
            // FormSelectBill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(794, 520);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectBill";
            this.ShowInTaskbar = false;
            this.Text = "单据选取";
            this.Load += new System.EventHandler(this.FormSelectBill_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBILL)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.DataGridView dataGridViewBILL;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ErrorProvider errorProviderS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonSX;
        private System.Windows.Forms.TextBox textBoxJSSJ;
        private System.Windows.Forms.TextBox textBoxKSSJ;
        private System.Windows.Forms.ComboBox comboBoxYWY;
        private System.Windows.Forms.TextBox textBoxDJBH;
        private System.Windows.Forms.TextBox textBoxDWMC;
        private System.Windows.Forms.Button buttonQX;
    }
}