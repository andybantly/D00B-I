namespace D00B
{
    partial class FormatBuilder
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
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnCustom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbFormat
            // 
            this.cbFormat.FormattingEnabled = true;
            this.cbFormat.Location = new System.Drawing.Point(10, 28);
            this.cbFormat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbFormat.Name = "cbFormat";
            this.cbFormat.Size = new System.Drawing.Size(211, 21);
            this.cbFormat.TabIndex = 0;
            this.cbFormat.SelectedIndexChanged += new System.EventHandler(this.FormatBuilder_SelectedIndexChanged);
            this.cbFormat.TextChanged += new System.EventHandler(this.FormatBuilder_SelectedIndexChanged);
            // 
            // FmtLabel
            // 
            this.FmtLabel.AutoSize = true;
            this.FmtLabel.Location = new System.Drawing.Point(8, 14);
            this.FmtLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FmtLabel.Name = "FmtLabel";
            this.FmtLabel.Size = new System.Drawing.Size(73, 13);
            this.FmtLabel.TabIndex = 1;
            this.FmtLabel.Text = "{0} Formatting";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(142, 160);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // AlignLabel
            // 
            this.AlignLabel.AutoSize = true;
            this.AlignLabel.Location = new System.Drawing.Point(8, 55);
            this.AlignLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.AlignLabel.Name = "AlignLabel";
            this.AlignLabel.Size = new System.Drawing.Size(70, 13);
            this.AlignLabel.TabIndex = 3;
            this.AlignLabel.Text = "{0} Alignment";
            // 
            // txtAlignment
            // 
            this.txtAlignment.Location = new System.Drawing.Point(10, 70);
            this.txtAlignment.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtAlignment.Name = "txtAlignment";
            this.txtAlignment.Size = new System.Drawing.Size(210, 20);
            this.txtAlignment.TabIndex = 4;
            this.txtAlignment.Validating += new System.ComponentModel.CancelEventHandler(this.Alignment_Validating);
            // 
            // txtDescription
            // 
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.Location = new System.Drawing.Point(10, 101);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(208, 56);
            this.txtDescription.TabIndex = 5;
            // 
            // btnCustom
            // 
            this.btnCustom.Location = new System.Drawing.Point(10, 161);
            this.btnCustom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCustom.Name = "btnCustom";
            this.btnCustom.Size = new System.Drawing.Size(76, 23);
            this.btnCustom.TabIndex = 6;
            this.btnCustom.Text = "Custom";
            this.btnCustom.UseVisualStyleBackColor = true;
            this.btnCustom.Click += new System.EventHandler(this.CustomFormat_Click);
            // 
            // FormatBuilder
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(227, 192);
            this.Controls.Add(this.btnCustom);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtAlignment);
            this.Controls.Add(this.AlignLabel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.FmtLabel);
            this.Controls.Add(this.cbFormat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormatBuilder";
            this.Text = "Column Format Builder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbFormat;
        private System.Windows.Forms.Label FmtLabel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label AlignLabel;
        private System.Windows.Forms.TextBox txtAlignment;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnCustom;
    }
}