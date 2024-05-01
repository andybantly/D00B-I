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
            this.lvJoinTables = new System.Windows.Forms.ListView();
            this.gbJoin = new System.Windows.Forms.GroupBox();
            this.optSelf = new System.Windows.Forms.RadioButton();
            this.optFull = new System.Windows.Forms.RadioButton();
            this.optRight = new System.Windows.Forms.RadioButton();
            this.optLeft = new System.Windows.Forms.RadioButton();
            this.optInner = new System.Windows.Forms.RadioButton();
            this.gbInclude = new System.Windows.Forms.GroupBox();
            this.rbNeighbors = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.btnAdd = new System.Windows.Forms.Button();
            this.gbJoin.SuspendLayout();
            this.gbInclude.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(998, 349);
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
            this.txtJoin.Location = new System.Drawing.Point(18, 185);
            this.txtJoin.Margin = new System.Windows.Forms.Padding(1);
            this.txtJoin.Multiline = true;
            this.txtJoin.Name = "txtJoin";
            this.txtJoin.ReadOnly = true;
            this.txtJoin.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtJoin.Size = new System.Drawing.Size(1081, 133);
            this.txtJoin.TabIndex = 1;
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
            this.lvJoinTables.Size = new System.Drawing.Size(1081, 165);
            this.lvJoinTables.TabIndex = 46;
            this.lvJoinTables.UseCompatibleStateImageBehavior = false;
            this.lvJoinTables.View = System.Windows.Forms.View.Details;
            this.lvJoinTables.VirtualMode = true;
            this.lvJoinTables.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.LvJoinTables_RetrieveVirtualItem);
            this.lvJoinTables.SelectedIndexChanged += new System.EventHandler(this.JoinTables_SelectedIndexChanged);
            // 
            // gbJoin
            // 
            this.gbJoin.Controls.Add(this.optSelf);
            this.gbJoin.Controls.Add(this.optFull);
            this.gbJoin.Controls.Add(this.optRight);
            this.gbJoin.Controls.Add(this.optLeft);
            this.gbJoin.Controls.Add(this.optInner);
            this.gbJoin.Location = new System.Drawing.Point(18, 322);
            this.gbJoin.Name = "gbJoin";
            this.gbJoin.Size = new System.Drawing.Size(307, 55);
            this.gbJoin.TabIndex = 47;
            this.gbJoin.TabStop = false;
            this.gbJoin.Text = "Join Type";
            // 
            // optSelf
            // 
            this.optSelf.AutoSize = true;
            this.optSelf.Enabled = false;
            this.optSelf.Location = new System.Drawing.Point(240, 19);
            this.optSelf.Margin = new System.Windows.Forms.Padding(1);
            this.optSelf.Name = "optSelf";
            this.optSelf.Size = new System.Drawing.Size(51, 20);
            this.optSelf.TabIndex = 11;
            this.optSelf.TabStop = true;
            this.optSelf.Text = "Self";
            this.optSelf.UseVisualStyleBackColor = true;
            // 
            // optFull
            // 
            this.optFull.AutoSize = true;
            this.optFull.Enabled = false;
            this.optFull.Location = new System.Drawing.Point(189, 19);
            this.optFull.Margin = new System.Windows.Forms.Padding(1);
            this.optFull.Name = "optFull";
            this.optFull.Size = new System.Drawing.Size(49, 20);
            this.optFull.TabIndex = 10;
            this.optFull.TabStop = true;
            this.optFull.Text = "Full";
            this.optFull.UseVisualStyleBackColor = true;
            // 
            // optRight
            // 
            this.optRight.AutoSize = true;
            this.optRight.Enabled = false;
            this.optRight.Location = new System.Drawing.Point(128, 19);
            this.optRight.Margin = new System.Windows.Forms.Padding(1);
            this.optRight.Name = "optRight";
            this.optRight.Size = new System.Drawing.Size(59, 20);
            this.optRight.TabIndex = 9;
            this.optRight.TabStop = true;
            this.optRight.Text = "Right";
            this.optRight.UseVisualStyleBackColor = true;
            // 
            // optLeft
            // 
            this.optLeft.AutoSize = true;
            this.optLeft.Enabled = false;
            this.optLeft.Location = new System.Drawing.Point(77, 19);
            this.optLeft.Margin = new System.Windows.Forms.Padding(1);
            this.optLeft.Name = "optLeft";
            this.optLeft.Size = new System.Drawing.Size(49, 20);
            this.optLeft.TabIndex = 8;
            this.optLeft.TabStop = true;
            this.optLeft.Text = "Left";
            this.optLeft.UseVisualStyleBackColor = true;
            // 
            // optInner
            // 
            this.optInner.AutoSize = true;
            this.optInner.Enabled = false;
            this.optInner.Location = new System.Drawing.Point(17, 19);
            this.optInner.Margin = new System.Windows.Forms.Padding(1);
            this.optInner.Name = "optInner";
            this.optInner.Size = new System.Drawing.Size(57, 20);
            this.optInner.TabIndex = 7;
            this.optInner.TabStop = true;
            this.optInner.Text = "Inner";
            this.optInner.UseVisualStyleBackColor = true;
            // 
            // gbInclude
            // 
            this.gbInclude.Controls.Add(this.rbNeighbors);
            this.gbInclude.Controls.Add(this.rbAll);
            this.gbInclude.Location = new System.Drawing.Point(331, 322);
            this.gbInclude.Name = "gbInclude";
            this.gbInclude.Size = new System.Drawing.Size(168, 55);
            this.gbInclude.TabIndex = 48;
            this.gbInclude.TabStop = false;
            this.gbInclude.Text = "Include";
            // 
            // rbNeighbors
            // 
            this.rbNeighbors.AutoSize = true;
            this.rbNeighbors.Location = new System.Drawing.Point(71, 19);
            this.rbNeighbors.Name = "rbNeighbors";
            this.rbNeighbors.Size = new System.Drawing.Size(91, 20);
            this.rbNeighbors.TabIndex = 1;
            this.rbNeighbors.TabStop = true;
            this.rbNeighbors.Text = "Neighbors";
            this.rbNeighbors.UseVisualStyleBackColor = true;
            this.rbNeighbors.CheckedChanged += new System.EventHandler(this.Include_Changed);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(11, 19);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(43, 20);
            this.rbAll.TabIndex = 0;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.CheckedChanged += new System.EventHandler(this.Include_Changed);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(884, 349);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(101, 28);
            this.btnAdd.TabIndex = 49;
            this.btnAdd.Text = "Add Join";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // DlgJoin
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 386);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.gbInclude);
            this.Controls.Add(this.gbJoin);
            this.Controls.Add(this.lvJoinTables);
            this.Controls.Add(this.txtJoin);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgJoin";
            this.Text = "Table Join";
            this.Load += new System.EventHandler(this.DlgJoin_Load);
            this.gbJoin.ResumeLayout(false);
            this.gbJoin.PerformLayout();
            this.gbInclude.ResumeLayout(false);
            this.gbInclude.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtJoin;
        private System.Windows.Forms.ListView lvJoinTables;
        private System.Windows.Forms.GroupBox gbJoin;
        private System.Windows.Forms.RadioButton optSelf;
        private System.Windows.Forms.RadioButton optFull;
        private System.Windows.Forms.RadioButton optRight;
        private System.Windows.Forms.RadioButton optLeft;
        private System.Windows.Forms.RadioButton optInner;
        private System.Windows.Forms.GroupBox gbInclude;
        private System.Windows.Forms.RadioButton rbNeighbors;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.Button btnAdd;
    }
}