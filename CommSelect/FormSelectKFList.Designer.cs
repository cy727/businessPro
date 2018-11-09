namespace business
{
    partial class FormSelectKFList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectKFList));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkedListBoxKF = new System.Windows.Forms.CheckedListBox();
            this.buttonCLEAR = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkedListBoxKF);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(284, 499);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "库房";
            // 
            // checkedListBoxKF
            // 
            this.checkedListBoxKF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxKF.FormattingEnabled = true;
            this.checkedListBoxKF.Location = new System.Drawing.Point(3, 17);
            this.checkedListBoxKF.Name = "checkedListBoxKF";
            this.checkedListBoxKF.Size = new System.Drawing.Size(278, 479);
            this.checkedListBoxKF.TabIndex = 0;
            // 
            // buttonCLEAR
            // 
            this.buttonCLEAR.Location = new System.Drawing.Point(139, 505);
            this.buttonCLEAR.Name = "buttonCLEAR";
            this.buttonCLEAR.Size = new System.Drawing.Size(55, 23);
            this.buttonCLEAR.TabIndex = 12;
            this.buttonCLEAR.Text = "清除";
            this.buttonCLEAR.UseVisualStyleBackColor = true;
            this.buttonCLEAR.Click += new System.EventHandler(this.buttonCLEAR_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(200, 505);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(21, 505);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(104, 23);
            this.btnSelect.TabIndex = 10;
            this.btnSelect.Text = "选择";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // FormSelectKFList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 540);
            this.Controls.Add(this.buttonCLEAR);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectKFList";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "库房选择";
            this.Load += new System.EventHandler(this.FormSelectKFList_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.CheckedListBox checkedListBoxKF;
        private System.Windows.Forms.Button buttonCLEAR;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelect;
    }
}