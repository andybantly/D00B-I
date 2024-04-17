namespace D00B
{
    partial class CustomFormatBuilder
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
            this.btnOK = new System.Windows.Forms.Button();
            this.FmtLabel = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtFormat = new System.Windows.Forms.TextBox();
            this.lvFormat = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(356, 210);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FmtLabel
            // 
            this.FmtLabel.AutoSize = true;
            this.FmtLabel.Location = new System.Drawing.Point(9, 7);
            this.FmtLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FmtLabel.Name = "FmtLabel";
            this.FmtLabel.Size = new System.Drawing.Size(73, 13);
            this.FmtLabel.TabIndex = 8;
            this.FmtLabel.Text = "{0} Formatting";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(356, 22);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(76, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtFormat
            // 
            this.txtFormat.Location = new System.Drawing.Point(12, 24);
            this.txtFormat.Name = "txtFormat";
            this.txtFormat.Size = new System.Drawing.Size(339, 20);
            this.txtFormat.TabIndex = 10;
            // 
            // lvFormat
            // 
            this.lvFormat.GridLines = true;
            this.lvFormat.HideSelection = false;
            this.lvFormat.Location = new System.Drawing.Point(12, 50);
            this.lvFormat.Name = "lvFormat";
            this.lvFormat.Size = new System.Drawing.Size(420, 155);
            this.lvFormat.TabIndex = 11;
            this.lvFormat.UseCompatibleStateImageBehavior = false;
            this.lvFormat.View = System.Windows.Forms.View.Details;
            // 
            // CustomFormatBuilder
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(443, 244);
            this.Controls.Add(this.lvFormat);
            this.Controls.Add(this.txtFormat);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.FmtLabel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomFormatBuilder";
            this.Text = "Custom Format Builder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label FmtLabel;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtFormat;
        private System.Windows.Forms.ListView lvFormat;
    }
}