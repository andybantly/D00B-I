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
            this.label1 = new System.Windows.Forms.Label();
            this.cbCultures = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cbFormat
            // 
            this.cbFormat.FormattingEnabled = true;
            this.cbFormat.Location = new System.Drawing.Point(10, 73);
            this.cbFormat.Margin = new System.Windows.Forms.Padding(2);
            this.cbFormat.Name = "cbFormat";
            this.cbFormat.Size = new System.Drawing.Size(268, 24);
            this.cbFormat.TabIndex = 0;
            this.cbFormat.SelectedIndexChanged += new System.EventHandler(this.FormatBuilder_SelectedIndexChanged);
            this.cbFormat.TextChanged += new System.EventHandler(this.FormatBuilder_SelectedIndexChanged);
            // 
            // FmtLabel
            // 
            this.FmtLabel.AutoSize = true;
            this.FmtLabel.Location = new System.Drawing.Point(8, 56);
            this.FmtLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FmtLabel.Name = "FmtLabel";
            this.FmtLabel.Size = new System.Drawing.Size(90, 16);
            this.FmtLabel.TabIndex = 1;
            this.FmtLabel.Text = "{0} Formatting";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(202, 252);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
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
            this.AlignLabel.Location = new System.Drawing.Point(8, 102);
            this.AlignLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.AlignLabel.Name = "AlignLabel";
            this.AlignLabel.Size = new System.Drawing.Size(86, 16);
            this.AlignLabel.TabIndex = 3;
            this.AlignLabel.Text = "{0} Alignment";
            // 
            // txtAlignment
            // 
            this.txtAlignment.Location = new System.Drawing.Point(10, 117);
            this.txtAlignment.Margin = new System.Windows.Forms.Padding(2);
            this.txtAlignment.Name = "txtAlignment";
            this.txtAlignment.Size = new System.Drawing.Size(268, 22);
            this.txtAlignment.TabIndex = 4;
            this.txtAlignment.Validating += new System.ComponentModel.CancelEventHandler(this.Alignment_Validating);
            // 
            // txtDescription
            // 
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.Location = new System.Drawing.Point(10, 143);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(2);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(268, 105);
            this.txtDescription.TabIndex = 5;
            // 
            // btnCustom
            // 
            this.btnCustom.Location = new System.Drawing.Point(10, 252);
            this.btnCustom.Margin = new System.Windows.Forms.Padding(2);
            this.btnCustom.Name = "btnCustom";
            this.btnCustom.Size = new System.Drawing.Size(76, 23);
            this.btnCustom.TabIndex = 6;
            this.btnCustom.Text = "Custom";
            this.btnCustom.UseVisualStyleBackColor = true;
            this.btnCustom.Click += new System.EventHandler(this.CustomFormat_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Culture";
            // 
            // cbCultures
            // 
            this.cbCultures.FormattingEnabled = true;
            this.cbCultures.Location = new System.Drawing.Point(9, 26);
            this.cbCultures.Margin = new System.Windows.Forms.Padding(2);
            this.cbCultures.Name = "cbCultures";
            this.cbCultures.Size = new System.Drawing.Size(268, 24);
            this.cbCultures.TabIndex = 7;
            this.cbCultures.SelectedIndexChanged += new System.EventHandler(this.Culture_SelectedIndexChanged);
            // 
            // FormatBuilder
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(289, 286);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCultures);
            this.Controls.Add(this.btnCustom);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtAlignment);
            this.Controls.Add(this.AlignLabel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.FmtLabel);
            this.Controls.Add(this.cbFormat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbCultures;
    }
}