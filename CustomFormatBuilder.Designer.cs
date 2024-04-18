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
            this.btnClear = new System.Windows.Forms.Button();
            this.txtFormat = new System.Windows.Forms.TextBox();
            this.lvFormat = new System.Windows.Forms.ListView();
            this.txtCustomFormat = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(356, 232);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(356, 44);
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
            this.txtFormat.Location = new System.Drawing.Point(12, 44);
            this.txtFormat.Name = "txtFormat";
            this.txtFormat.Size = new System.Drawing.Size(339, 22);
            this.txtFormat.TabIndex = 10;
            // 
            // lvFormat
            // 
            this.lvFormat.FullRowSelect = true;
            this.lvFormat.GridLines = true;
            this.lvFormat.HideSelection = false;
            this.lvFormat.Location = new System.Drawing.Point(12, 72);
            this.lvFormat.Name = "lvFormat";
            this.lvFormat.Size = new System.Drawing.Size(420, 155);
            this.lvFormat.TabIndex = 11;
            this.lvFormat.UseCompatibleStateImageBehavior = false;
            this.lvFormat.View = System.Windows.Forms.View.Details;
            this.lvFormat.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvFormat_MouseDoubleClick);
            // 
            // txtCustomFormat
            // 
            this.txtCustomFormat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCustomFormat.Location = new System.Drawing.Point(12, 12);
            this.txtCustomFormat.Multiline = true;
            this.txtCustomFormat.Name = "txtCustomFormat";
            this.txtCustomFormat.ReadOnly = true;
            this.txtCustomFormat.Size = new System.Drawing.Size(420, 31);
            this.txtCustomFormat.TabIndex = 12;
            this.txtCustomFormat.Text = "{0} Custom Format - double click list to add a format to the custom format";
            // 
            // CustomFormatBuilder
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(443, 266);
            this.Controls.Add(this.txtCustomFormat);
            this.Controls.Add(this.lvFormat);
            this.Controls.Add(this.txtFormat);
            this.Controls.Add(this.btnClear);
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
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtFormat;
        private System.Windows.Forms.ListView lvFormat;
        private System.Windows.Forms.TextBox txtCustomFormat;
    }
}