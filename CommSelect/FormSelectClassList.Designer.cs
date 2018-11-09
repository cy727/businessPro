namespace business
{
    partial class FormSelectClassList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectClassList));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.buttonCLEAR = new System.Windows.Forms.Button();
            this.checkedListBoxFL = new System.Windows.Forms.CheckedListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkedListBoxFL);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(263, 668);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "商品分类";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(192, 674);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(13, 674);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(104, 23);
            this.btnSelect.TabIndex = 7;
            this.btnSelect.Text = "选择";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // buttonCLEAR
            // 
            this.buttonCLEAR.Location = new System.Drawing.Point(131, 674);
            this.buttonCLEAR.Name = "buttonCLEAR";
            this.buttonCLEAR.Size = new System.Drawing.Size(55, 23);
            this.buttonCLEAR.TabIndex = 9;
            this.buttonCLEAR.Text = "清除";
            this.buttonCLEAR.UseVisualStyleBackColor = true;
            this.buttonCLEAR.Click += new System.EventHandler(this.buttonCLEAR_Click);
            // 
            // checkedListBoxFL
            // 
            this.checkedListBoxFL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxFL.FormattingEnabled = true;
            this.checkedListBoxFL.Location = new System.Drawing.Point(3, 17);
            this.checkedListBoxFL.Name = "checkedListBoxFL";
            this.checkedListBoxFL.Size = new System.Drawing.Size(257, 648);
            this.checkedListBoxFL.TabIndex = 0;
            // 
            // FormSelectClassList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(263, 702);
            this.Controls.Add(this.buttonCLEAR);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSelectClassList";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "分类选择";
            this.Load += new System.EventHandler(this.FormSelectClassList_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button buttonCLEAR;
        public System.Windows.Forms.CheckedListBox checkedListBoxFL;
    }
}