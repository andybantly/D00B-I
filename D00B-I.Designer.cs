namespace D00B
{
    partial class D00B
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
            this.txtConnString = new System.Windows.Forms.TextBox();
            this.lvQuery = new System.Windows.Forms.ListView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.chkHdr = new System.Windows.Forms.CheckBox();
            this.chkPrevAll = new System.Windows.Forms.CheckBox();
            this.cbSchema = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTables = new System.Windows.Forms.TextBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.chkTable = new System.Windows.Forms.CheckBox();
            this.chkData = new System.Windows.Forms.CheckBox();
            this.txtData = new System.Windows.Forms.TextBox();
            this.pbData = new System.Windows.Forms.ProgressBar();
            this.chkExact = new System.Windows.Forms.CheckBox();
            this.lblPreview = new System.Windows.Forms.Label();
            this.txtPreview = new System.Windows.Forms.TextBox();
            this.lvTables = new System.Windows.Forms.ListView();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.lvAdjTables = new System.Windows.Forms.ListView();
            this.lvResults = new System.Windows.Forms.ListView();
            this.cbDataBases = new System.Windows.Forms.ComboBox();
            this.btnJoin = new System.Windows.Forms.Button();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.lvJoinTables = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnResetJoin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtConnString
            // 
            this.txtConnString.Location = new System.Drawing.Point(12, 79);
            this.txtConnString.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.txtConnString.Multiline = true;
            this.txtConnString.Name = "txtConnString";
            this.txtConnString.Size = new System.Drawing.Size(3222, 46);
            this.txtConnString.TabIndex = 6;
            // 
            // lvQuery
            // 
            this.lvQuery.AllowDrop = true;
            this.lvQuery.Enabled = false;
            this.lvQuery.FullRowSelect = true;
            this.lvQuery.GridLines = true;
            this.lvQuery.HideSelection = false;
            this.lvQuery.Location = new System.Drawing.Point(12, 643);
            this.lvQuery.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.lvQuery.MultiSelect = false;
            this.lvQuery.Name = "lvQuery";
            this.lvQuery.Size = new System.Drawing.Size(3636, 590);
            this.lvQuery.TabIndex = 7;
            this.lvQuery.UseCompatibleStateImageBehavior = false;
            this.lvQuery.View = System.Windows.Forms.View.Details;
            this.lvQuery.VirtualMode = true;
            this.lvQuery.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.LvQuery_ColumnClick);
            this.lvQuery.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.LvQuery_RetrieveVirtualItem);
            this.lvQuery.SelectedIndexChanged += new System.EventHandler(this.LvQuery_SelectedIndexChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(3413, 5);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(240, 65);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(2458, 560);
            this.btnExport.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(240, 65);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // chkHdr
            // 
            this.chkHdr.AutoSize = true;
            this.chkHdr.Checked = true;
            this.chkHdr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHdr.Enabled = false;
            this.chkHdr.Location = new System.Drawing.Point(3455, 3626);
            this.chkHdr.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.chkHdr.Name = "chkHdr";
            this.chkHdr.Size = new System.Drawing.Size(278, 41);
            this.chkHdr.TabIndex = 13;
            this.chkHdr.Text = "Include Header";
            this.chkHdr.UseVisualStyleBackColor = true;
            // 
            // chkPrevAll
            // 
            this.chkPrevAll.AutoSize = true;
            this.chkPrevAll.Enabled = false;
            this.chkPrevAll.Location = new System.Drawing.Point(879, 17);
            this.chkPrevAll.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.chkPrevAll.Name = "chkPrevAll";
            this.chkPrevAll.Size = new System.Drawing.Size(99, 41);
            this.chkPrevAll.TabIndex = 14;
            this.chkPrevAll.Text = "All";
            this.chkPrevAll.UseVisualStyleBackColor = true;
            this.chkPrevAll.CheckedChanged += new System.EventHandler(this.ChkPrevAll_CheckedChanged);
            // 
            // cbSchema
            // 
            this.cbSchema.Enabled = false;
            this.cbSchema.FormattingEnabled = true;
            this.cbSchema.Location = new System.Drawing.Point(178, 17);
            this.cbSchema.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.cbSchema.Name = "cbSchema";
            this.cbSchema.Size = new System.Drawing.Size(372, 45);
            this.cbSchema.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 37);
            this.label2.TabIndex = 16;
            this.label2.Text = "Schema";
            // 
            // tbTables
            // 
            this.tbTables.Location = new System.Drawing.Point(3261, 14);
            this.tbTables.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.tbTables.Name = "tbTables";
            this.tbTables.ReadOnly = true;
            this.tbTables.Size = new System.Drawing.Size(125, 44);
            this.tbTables.TabIndex = 18;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(2710, 216);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(938, 44);
            this.txtSearch.TabIndex = 19;
            this.txtSearch.TextChanged += new System.EventHandler(this.BtnSearch_TextChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(2458, 205);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(240, 65);
            this.btnSearch.TabIndex = 20;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // chkTable
            // 
            this.chkTable.AutoSize = true;
            this.chkTable.Enabled = false;
            this.chkTable.Location = new System.Drawing.Point(2629, 148);
            this.chkTable.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.chkTable.Name = "chkTable";
            this.chkTable.Size = new System.Drawing.Size(143, 41);
            this.chkTable.TabIndex = 23;
            this.chkTable.Text = "Table";
            this.chkTable.UseVisualStyleBackColor = true;
            this.chkTable.CheckedChanged += new System.EventHandler(this.ChkTable_CheckedChanged);
            // 
            // chkData
            // 
            this.chkData.AutoSize = true;
            this.chkData.Enabled = false;
            this.chkData.Location = new System.Drawing.Point(2796, 148);
            this.chkData.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.chkData.Name = "chkData";
            this.chkData.Size = new System.Drawing.Size(131, 41);
            this.chkData.TabIndex = 24;
            this.chkData.Text = "Data";
            this.chkData.UseVisualStyleBackColor = true;
            this.chkData.CheckedChanged += new System.EventHandler(this.chkData_CheckedChanged);
            // 
            // txtData
            // 
            this.txtData.Enabled = false;
            this.txtData.Location = new System.Drawing.Point(2945, 143);
            this.txtData.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(704, 44);
            this.txtData.TabIndex = 25;
            // 
            // pbData
            // 
            this.pbData.Location = new System.Drawing.Point(3265, 74);
            this.pbData.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.pbData.Name = "pbData";
            this.pbData.Size = new System.Drawing.Size(387, 54);
            this.pbData.TabIndex = 26;
            // 
            // chkExact
            // 
            this.chkExact.AutoSize = true;
            this.chkExact.Location = new System.Drawing.Point(2458, 148);
            this.chkExact.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.chkExact.Name = "chkExact";
            this.chkExact.Size = new System.Drawing.Size(142, 41);
            this.chkExact.TabIndex = 27;
            this.chkExact.Text = "Exact";
            this.chkExact.UseVisualStyleBackColor = true;
            this.chkExact.CheckedChanged += new System.EventHandler(this.ChkExact_CheckedChanged);
            // 
            // lblPreview
            // 
            this.lblPreview.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPreview.AutoSize = true;
            this.lblPreview.Location = new System.Drawing.Point(580, 20);
            this.lblPreview.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(128, 37);
            this.lblPreview.TabIndex = 30;
            this.lblPreview.Text = "Preview";
            // 
            // txtPreview
            // 
            this.txtPreview.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPreview.Enabled = false;
            this.txtPreview.Location = new System.Drawing.Point(734, 16);
            this.txtPreview.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.txtPreview.Name = "txtPreview";
            this.txtPreview.Size = new System.Drawing.Size(106, 44);
            this.txtPreview.TabIndex = 29;
            this.txtPreview.Text = "100";
            this.txtPreview.Validating += new System.ComponentModel.CancelEventHandler(this.TxtPreview_Validating);
            this.txtPreview.Validated += new System.EventHandler(this.TxtPreview_Validated);
            // 
            // lvTables
            // 
            this.lvTables.Enabled = false;
            this.lvTables.FullRowSelect = true;
            this.lvTables.GridLines = true;
            this.lvTables.HideSelection = false;
            this.lvTables.Location = new System.Drawing.Point(12, 190);
            this.lvTables.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.lvTables.MultiSelect = false;
            this.lvTables.Name = "lvTables";
            this.lvTables.Size = new System.Drawing.Size(600, 431);
            this.lvTables.TabIndex = 31;
            this.lvTables.UseCompatibleStateImageBehavior = false;
            this.lvTables.View = System.Windows.Forms.View.Details;
            this.lvTables.VirtualMode = true;
            this.lvTables.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.LvTables_RetrieveVirtualItem);
            this.lvTables.SelectedIndexChanged += new System.EventHandler(this.LvTables_SelectedIndexChanged);
            // 
            // lvColumns
            // 
            this.lvColumns.Enabled = false;
            this.lvColumns.FullRowSelect = true;
            this.lvColumns.GridLines = true;
            this.lvColumns.HideSelection = false;
            this.lvColumns.Location = new System.Drawing.Point(622, 190);
            this.lvColumns.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.lvColumns.MultiSelect = false;
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(600, 431);
            this.lvColumns.TabIndex = 34;
            this.lvColumns.UseCompatibleStateImageBehavior = false;
            this.lvColumns.View = System.Windows.Forms.View.Details;
            this.lvColumns.VirtualMode = true;
            this.lvColumns.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.LvColumns_RetrieveVirtualItem);
            // 
            // lvAdjTables
            // 
            this.lvAdjTables.Enabled = false;
            this.lvAdjTables.FullRowSelect = true;
            this.lvAdjTables.GridLines = true;
            this.lvAdjTables.HideSelection = false;
            this.lvAdjTables.Location = new System.Drawing.Point(1235, 190);
            this.lvAdjTables.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.lvAdjTables.MultiSelect = false;
            this.lvAdjTables.Name = "lvAdjTables";
            this.lvAdjTables.Size = new System.Drawing.Size(600, 431);
            this.lvAdjTables.TabIndex = 35;
            this.lvAdjTables.UseCompatibleStateImageBehavior = false;
            this.lvAdjTables.View = System.Windows.Forms.View.Details;
            this.lvAdjTables.SelectedIndexChanged += new System.EventHandler(this.LvAdjTables_SelectedIndexChanged);
            this.lvAdjTables.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvAdjTables_MouseDoubleClick);
            // 
            // lvResults
            // 
            this.lvResults.Enabled = false;
            this.lvResults.FullRowSelect = true;
            this.lvResults.GridLines = true;
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(2710, 282);
            this.lvResults.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.lvResults.MultiSelect = false;
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(938, 337);
            this.lvResults.TabIndex = 36;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            this.lvResults.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvResults_MouseDoubleClick);
            // 
            // cbDataBases
            // 
            this.cbDataBases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataBases.FormattingEnabled = true;
            this.cbDataBases.Location = new System.Drawing.Point(985, 14);
            this.cbDataBases.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.cbDataBases.Name = "cbDataBases";
            this.cbDataBases.Size = new System.Drawing.Size(2220, 45);
            this.cbDataBases.TabIndex = 37;
            this.cbDataBases.SelectionChangeCommitted += new System.EventHandler(this.CbDataBases_SelectionChangeCommitted);
            // 
            // btnJoin
            // 
            this.btnJoin.Location = new System.Drawing.Point(2458, 284);
            this.btnJoin.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(240, 65);
            this.btnJoin.TabIndex = 38;
            this.btnJoin.Text = "Join";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.BtnJoin_Click);
            // 
            // txtQuery
            // 
            this.txtQuery.Enabled = false;
            this.txtQuery.Location = new System.Drawing.Point(12, 1247);
            this.txtQuery.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(3640, 147);
            this.txtQuery.TabIndex = 39;
            // 
            // lvJoinTables
            // 
            this.lvJoinTables.Enabled = false;
            this.lvJoinTables.FullRowSelect = true;
            this.lvJoinTables.GridLines = true;
            this.lvJoinTables.HideSelection = false;
            this.lvJoinTables.Location = new System.Drawing.Point(1846, 190);
            this.lvJoinTables.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.lvJoinTables.MultiSelect = false;
            this.lvJoinTables.Name = "lvJoinTables";
            this.lvJoinTables.Size = new System.Drawing.Size(600, 431);
            this.lvJoinTables.TabIndex = 40;
            this.lvJoinTables.UseCompatibleStateImageBehavior = false;
            this.lvJoinTables.View = System.Windows.Forms.View.Details;
            this.lvJoinTables.VirtualMode = true;
            this.lvJoinTables.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.LvJoinTables_RetrieveVirtualItem);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 143);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(340, 37);
            this.label1.TabIndex = 41;
            this.label1.Text = "All Owners and Tables";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(620, 143);
            this.label3.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(328, 37);
            this.label3.TabIndex = 42;
            this.label3.Text = "Active Table Columns";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1232, 143);
            this.label4.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(583, 37);
            this.label4.TabIndex = 43;
            this.label4.Text = "Adjacent Owners, Tables, and Columns";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1836, 143);
            this.label5.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(517, 37);
            this.label5.TabIndex = 44;
            this.label5.Text = "Join Owners, Tables, and Columns";
            // 
            // btnResetJoin
            // 
            this.btnResetJoin.Location = new System.Drawing.Point(2458, 369);
            this.btnResetJoin.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnResetJoin.Name = "btnResetJoin";
            this.btnResetJoin.Size = new System.Drawing.Size(240, 65);
            this.btnResetJoin.TabIndex = 45;
            this.btnResetJoin.Text = "Reset Join";
            this.btnResetJoin.UseVisualStyleBackColor = true;
            this.btnResetJoin.Click += new System.EventHandler(this.btnResetJoin_Click);
            // 
            // D00B
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(19F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(3664, 1406);
            this.Controls.Add(this.btnResetJoin);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvJoinTables);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.cbDataBases);
            this.Controls.Add(this.lvResults);
            this.Controls.Add(this.lvAdjTables);
            this.Controls.Add(this.lvColumns);
            this.Controls.Add(this.lvTables);
            this.Controls.Add(this.lblPreview);
            this.Controls.Add(this.txtPreview);
            this.Controls.Add(this.chkExact);
            this.Controls.Add(this.pbData);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.chkData);
            this.Controls.Add(this.chkTable);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.tbTables);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbSchema);
            this.Controls.Add(this.chkPrevAll);
            this.Controls.Add(this.chkHdr);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lvQuery);
            this.Controls.Add(this.txtConnString);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "D00B";
            this.Text = "DóòB-👁";
            this.Load += new System.EventHandler(this.D00B_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtConnString;
        private System.Windows.Forms.ListView lvQuery;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.CheckBox chkHdr;
        private System.Windows.Forms.CheckBox chkPrevAll;
        private System.Windows.Forms.ComboBox cbSchema;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTables;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.CheckBox chkTable;
        private System.Windows.Forms.CheckBox chkData;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.ProgressBar pbData;
        private System.Windows.Forms.CheckBox chkExact;
        private System.Windows.Forms.Label lblPreview;
        private System.Windows.Forms.TextBox txtPreview;
        private System.Windows.Forms.ListView lvTables;
        private System.Windows.Forms.ListView lvColumns;
        private System.Windows.Forms.ListView lvAdjTables;
        private System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.ComboBox cbDataBases;
        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.ListView lvJoinTables;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnResetJoin;
    }
}

