namespace D00B
{
    partial class Format
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
            this.cbFormat = new System.Windows.Forms.ComboBox();
            this.FmtLabel = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.AlignLabel = new System.Windows.Forms.Label();
            this.txtAlignment = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cbFormat
            // 
            this.cbFormat.FormattingEnabled = true;
            this.cbFormat.Location = new System.Drawing.Point(16, 44);
            this.cbFormat.Name = "cbFormat";
            this.cbFormat.Size = new System.Drawing.Size(315, 28);
            this.cbFormat.TabIndex = 0;
            // 
            // FmtLabel
            // 
            this.FmtLabel.AutoSize = true;
            this.FmtLabel.Location = new System.Drawing.Point(12, 21);
            this.FmtLabel.Name = "FmtLabel";
            this.FmtLabel.Size = new System.Drawing.Size(109, 20);
            this.FmtLabel.TabIndex = 1;
            this.FmtLabel.Text = "{0} Formatting";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(217, 182);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(114, 35);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // AlignLabel
            // 
            this.AlignLabel.AutoSize = true;
            this.AlignLabel.Location = new System.Drawing.Point(12, 85);
            this.AlignLabel.Name = "AlignLabel";
            this.AlignLabel.Size = new System.Drawing.Size(103, 20);
            this.AlignLabel.TabIndex = 3;
            this.AlignLabel.Text = "{0} Alignment";
            // 
            // txtAlignment
            // 
            this.txtAlignment.Location = new System.Drawing.Point(16, 108);
            this.txtAlignment.Name = "txtAlignment";
            this.txtAlignment.Size = new System.Drawing.Size(313, 26);
            this.txtAlignment.TabIndex = 4;
            this.txtAlignment.Validating += new System.ComponentModel.CancelEventHandler(this.Alignment_Validating);
            // 
            // Format
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 228);
            this.Controls.Add(this.txtAlignment);
            this.Controls.Add(this.AlignLabel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.FmtLabel);
            this.Controls.Add(this.cbFormat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Format";
            this.Text = "Column Format";
            this.Load += new System.EventHandler(this.Format_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbFormat;
        private System.Windows.Forms.Label FmtLabel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label AlignLabel;
        private System.Windows.Forms.TextBox txtAlignment;
    }
}