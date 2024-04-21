namespace D00B
{
    partial class DlgJoin
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
            this.txtJoin = new System.Windows.Forms.TextBox();
            this.optInner = new System.Windows.Forms.RadioButton();
            this.optLeft = new System.Windows.Forms.RadioButton();
            this.optRight = new System.Windows.Forms.RadioButton();
            this.optFull = new System.Windows.Forms.RadioButton();
            this.optSelf = new System.Windows.Forms.RadioButton();
            this.lvJoinTables = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(679, 349);
            this.btnOK.Margin = new System.Windows.Forms.Padding(1);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(101, 28);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // txtJoin
            // 
            this.txtJoin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJoin.Location = new System.Drawing.Point(18, 207);
            this.txtJoin.Margin = new System.Windows.Forms.Padding(1);
            this.txtJoin.Multiline = true;
            this.txtJoin.Name = "txtJoin";
            this.txtJoin.ReadOnly = true;
            this.txtJoin.Size = new System.Drawing.Size(762, 128);
            this.txtJoin.TabIndex = 1;
            // 
            // optInner
            // 
            this.optInner.AutoSize = true;
            this.optInner.Enabled = false;
            this.optInner.Location = new System.Drawing.Point(24, 185);
            this.optInner.Margin = new System.Windows.Forms.Padding(1);
            this.optInner.Name = "optInner";
            this.optInner.Size = new System.Drawing.Size(57, 20);
            this.optInner.TabIndex = 2;
            this.optInner.TabStop = true;
            this.optInner.Text = "Inner";
            this.optInner.UseVisualStyleBackColor = true;
            this.optInner.CheckedChanged += new System.EventHandler(this.Opt_CheckedChanged);
            // 
            // optLeft
            // 
            this.optLeft.AutoSize = true;
            this.optLeft.Enabled = false;
            this.optLeft.Location = new System.Drawing.Point(84, 185);
            this.optLeft.Margin = new System.Windows.Forms.Padding(1);
            this.optLeft.Name = "optLeft";
            this.optLeft.Size = new System.Drawing.Size(49, 20);
            this.optLeft.TabIndex = 3;
            this.optLeft.TabStop = true;
            this.optLeft.Text = "Left";
            this.optLeft.UseVisualStyleBackColor = true;
            this.optLeft.CheckedChanged += new System.EventHandler(this.Opt_CheckedChanged);
            // 
            // optRight
            // 
            this.optRight.AutoSize = true;
            this.optRight.Enabled = false;
            this.optRight.Location = new System.Drawing.Point(135, 185);
            this.optRight.Margin = new System.Windows.Forms.Padding(1);
            this.optRight.Name = "optRight";
            this.optRight.Size = new System.Drawing.Size(59, 20);
            this.optRight.TabIndex = 4;
            this.optRight.TabStop = true;
            this.optRight.Text = "Right";
            this.optRight.UseVisualStyleBackColor = true;
            this.optRight.CheckedChanged += new System.EventHandler(this.Opt_CheckedChanged);
            // 
            // optFull
            // 
            this.optFull.AutoSize = true;
            this.optFull.Enabled = false;
            this.optFull.Location = new System.Drawing.Point(196, 185);
            this.optFull.Margin = new System.Windows.Forms.Padding(1);
            this.optFull.Name = "optFull";
            this.optFull.Size = new System.Drawing.Size(49, 20);
            this.optFull.TabIndex = 5;
            this.optFull.TabStop = true;
            this.optFull.Text = "Full";
            this.optFull.UseVisualStyleBackColor = true;
            this.optFull.CheckedChanged += new System.EventHandler(this.Opt_CheckedChanged);
            // 
            // optSelf
            // 
            this.optSelf.AutoSize = true;
            this.optSelf.Enabled = false;
            this.optSelf.Location = new System.Drawing.Point(247, 185);
            this.optSelf.Margin = new System.Windows.Forms.Padding(1);
            this.optSelf.Name = "optSelf";
            this.optSelf.Size = new System.Drawing.Size(51, 20);
            this.optSelf.TabIndex = 6;
            this.optSelf.TabStop = true;
            this.optSelf.Text = "Self";
            this.optSelf.UseVisualStyleBackColor = true;
            this.optSelf.CheckedChanged += new System.EventHandler(this.Opt_CheckedChanged);
            // 
            // lvJoinTables
            // 
            this.lvJoinTables.FullRowSelect = true;
            this.lvJoinTables.GridLines = true;
            this.lvJoinTables.HideSelection = false;
            this.lvJoinTables.Location = new System.Drawing.Point(18, 14);
            this.lvJoinTables.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.lvJoinTables.MultiSelect = false;
            this.lvJoinTables.Name = "lvJoinTables";
            this.lvJoinTables.Size = new System.Drawing.Size(762, 165);
            this.lvJoinTables.TabIndex = 46;
            this.lvJoinTables.UseCompatibleStateImageBehavior = false;
            this.lvJoinTables.View = System.Windows.Forms.View.Details;
            this.lvJoinTables.VirtualMode = true;
            this.lvJoinTables.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.LvJoinTables_RetrieveVirtualItem);
            // 
            // DlgJoin
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 387);
            this.Controls.Add(this.lvJoinTables);
            this.Controls.Add(this.optSelf);
            this.Controls.Add(this.optFull);
            this.Controls.Add(this.optRight);
            this.Controls.Add(this.optLeft);
            this.Controls.Add(this.optInner);
            this.Controls.Add(this.txtJoin);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgJoin";
            this.Text = "Table Join";
            this.Load += new System.EventHandler(this.DlgJoin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtJoin;
        private System.Windows.Forms.RadioButton optInner;
        private System.Windows.Forms.RadioButton optLeft;
        private System.Windows.Forms.RadioButton optRight;
        private System.Windows.Forms.RadioButton optFull;
        private System.Windows.Forms.RadioButton optSelf;
        private System.Windows.Forms.ListView lvJoinTables;
    }
}