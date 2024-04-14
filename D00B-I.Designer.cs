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
            this.btnLoad = new System.Windows.Forms.Button();
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
            this.lb1 = new System.Windows.Forms.Label();
            this.lb2 = new System.Windows.Forms.Label();
            this.lb3 = new System.Windows.Forms.Label();
            this.lb4 = new System.Windows.Forms.Label();
            this.btnResetJoin = new System.Windows.Forms.Button();
            this.btnTestJoin = new System.Windows.Forms.Button();
            this.dgvQuery = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuery)).BeginInit();
            this.SuspendLayout();
            // 
            // txtConnString
            // 
            this.txtConnString.Location = new System.Drawing.Point(6, 42);
            this.txtConnString.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtConnString.Multiline = true;
            this.txtConnString.Name = "txtConnString";
            this.txtConnString.Size = new System.Drawing.Size(1528, 27);
            this.txtConnString.TabIndex = 6;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(1617, 3);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(114, 35);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(1166, 305);
            this.btnExport.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(114, 35);
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
            this.chkHdr.Location = new System.Drawing.Point(1636, 1960);
            this.chkHdr.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.chkHdr.Name = "chkHdr";
            this.chkHdr.Size = new System.Drawing.Size(144, 24);
            this.chkHdr.TabIndex = 13;
            this.chkHdr.Text = "Include Header";
            this.chkHdr.UseVisualStyleBackColor = true;
            // 
            // chkPrevAll
            // 
            this.chkPrevAll.AutoSize = true;
            this.chkPrevAll.Enabled = false;
            this.chkPrevAll.Location = new System.Drawing.Point(416, 9);
            this.chkPrevAll.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.chkPrevAll.Name = "chkPrevAll";
            this.chkPrevAll.Size = new System.Drawing.Size(52, 24);
            this.chkPrevAll.TabIndex = 14;
            this.chkPrevAll.Text = "All";
            this.chkPrevAll.UseVisualStyleBackColor = true;
            this.chkPrevAll.CheckedChanged += new System.EventHandler(this.ChkPrevAll_CheckedChanged);
            // 
            // cbSchema
            // 
            this.cbSchema.Enabled = false;
            this.cbSchema.FormattingEnabled = true;
            this.cbSchema.Location = new System.Drawing.Point(84, 6);
            this.cbSchema.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.cbSchema.Name = "cbSchema";
            this.cbSchema.Size = new System.Drawing.Size(178, 28);
            this.cbSchema.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "Schema";
            // 
            // tbTables
            // 
            this.tbTables.Location = new System.Drawing.Point(1546, 6);
            this.tbTables.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.tbTables.Name = "tbTables";
            this.tbTables.ReadOnly = true;
            this.tbTables.Size = new System.Drawing.Size(61, 26);
            this.tbTables.TabIndex = 18;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(1284, 117);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(446, 26);
            this.txtSearch.TabIndex = 19;
            this.txtSearch.TextChanged += new System.EventHandler(this.BtnSearch_TextChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(1166, 111);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(114, 35);
            this.btnSearch.TabIndex = 20;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // chkTable
            // 
            this.chkTable.AutoSize = true;
            this.chkTable.Enabled = false;
            this.chkTable.Location = new System.Drawing.Point(1245, 80);
            this.chkTable.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.chkTable.Name = "chkTable";
            this.chkTable.Size = new System.Drawing.Size(74, 24);
            this.chkTable.TabIndex = 23;
            this.chkTable.Text = "Table";
            this.chkTable.UseVisualStyleBackColor = true;
            this.chkTable.CheckedChanged += new System.EventHandler(this.ChkTable_CheckedChanged);
            // 
            // chkData
            // 
            this.chkData.AutoSize = true;
            this.chkData.Enabled = false;
            this.chkData.Location = new System.Drawing.Point(1324, 80);
            this.chkData.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.chkData.Name = "chkData";
            this.chkData.Size = new System.Drawing.Size(70, 24);
            this.chkData.TabIndex = 24;
            this.chkData.Text = "Data";
            this.chkData.UseVisualStyleBackColor = true;
            this.chkData.CheckedChanged += new System.EventHandler(this.ChkData_CheckedChanged);
            // 
            // txtData
            // 
            this.txtData.Enabled = false;
            this.txtData.Location = new System.Drawing.Point(1395, 77);
            this.txtData.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(336, 26);
            this.txtData.TabIndex = 25;
            // 
            // pbData
            // 
            this.pbData.Location = new System.Drawing.Point(1546, 42);
            this.pbData.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.pbData.Name = "pbData";
            this.pbData.Size = new System.Drawing.Size(183, 29);
            this.pbData.TabIndex = 26;
            // 
            // chkExact
            // 
            this.chkExact.AutoSize = true;
            this.chkExact.Location = new System.Drawing.Point(1164, 80);
            this.chkExact.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.chkExact.Name = "chkExact";
            this.chkExact.Size = new System.Drawing.Size(75, 24);
            this.chkExact.TabIndex = 27;
            this.chkExact.Text = "Exact";
            this.chkExact.UseVisualStyleBackColor = true;
            this.chkExact.CheckedChanged += new System.EventHandler(this.ChkExact_CheckedChanged);
            // 
            // lblPreview
            // 
            this.lblPreview.AutoSize = true;
            this.lblPreview.Location = new System.Drawing.Point(274, 9);
            this.lblPreview.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(63, 20);
            this.lblPreview.TabIndex = 30;
            this.lblPreview.Text = "Preview";
            // 
            // txtPreview
            // 
            this.txtPreview.Enabled = false;
            this.txtPreview.Location = new System.Drawing.Point(348, 6);
            this.txtPreview.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtPreview.Name = "txtPreview";
            this.txtPreview.Size = new System.Drawing.Size(52, 26);
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
            this.lvTables.Location = new System.Drawing.Point(6, 103);
            this.lvTables.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.lvTables.MultiSelect = false;
            this.lvTables.Name = "lvTables";
            this.lvTables.Size = new System.Drawing.Size(286, 235);
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
            this.lvColumns.Location = new System.Drawing.Point(296, 103);
            this.lvColumns.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.lvColumns.MultiSelect = false;
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(286, 235);
            this.lvColumns.TabIndex = 34;
            this.lvColumns.UseCompatibleStateImageBehavior = false;
            this.lvColumns.View = System.Windows.Forms.View.Details;
            this.lvColumns.VirtualMode = true;
            this.lvColumns.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.LvColumns_RetrieveVirtualItem);
            this.lvColumns.SelectedIndexChanged += new System.EventHandler(this.LvColumns_SelectedIndexChanged);
            this.lvColumns.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvColumns_EditFormat);
            // 
            // lvAdjTables
            // 
            this.lvAdjTables.Enabled = false;
            this.lvAdjTables.FullRowSelect = true;
            this.lvAdjTables.GridLines = true;
            this.lvAdjTables.HideSelection = false;
            this.lvAdjTables.Location = new System.Drawing.Point(585, 103);
            this.lvAdjTables.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.lvAdjTables.MultiSelect = false;
            this.lvAdjTables.Name = "lvAdjTables";
            this.lvAdjTables.Size = new System.Drawing.Size(286, 235);
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
            this.lvResults.Location = new System.Drawing.Point(1284, 152);
            this.lvResults.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.lvResults.MultiSelect = false;
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(446, 184);
            this.lvResults.TabIndex = 36;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            this.lvResults.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvResults_MouseDoubleClick);
            // 
            // cbDataBases
            // 
            this.cbDataBases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataBases.FormattingEnabled = true;
            this.cbDataBases.Location = new System.Drawing.Point(466, 4);
            this.cbDataBases.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.cbDataBases.Name = "cbDataBases";
            this.cbDataBases.Size = new System.Drawing.Size(1068, 28);
            this.cbDataBases.TabIndex = 37;
            this.cbDataBases.SelectionChangeCommitted += new System.EventHandler(this.CbDataBases_SelectionChangeCommitted);
            // 
            // btnJoin
            // 
            this.btnJoin.Location = new System.Drawing.Point(1166, 154);
            this.btnJoin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(114, 35);
            this.btnJoin.TabIndex = 38;
            this.btnJoin.Text = "Join";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.BtnJoin_Click);
            // 
            // txtQuery
            // 
            this.txtQuery.Enabled = false;
            this.txtQuery.Location = new System.Drawing.Point(6, 674);
            this.txtQuery.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(1726, 99);
            this.txtQuery.TabIndex = 39;
            // 
            // lvJoinTables
            // 
            this.lvJoinTables.Enabled = false;
            this.lvJoinTables.FullRowSelect = true;
            this.lvJoinTables.GridLines = true;
            this.lvJoinTables.HideSelection = false;
            this.lvJoinTables.Location = new System.Drawing.Point(874, 103);
            this.lvJoinTables.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.lvJoinTables.MultiSelect = false;
            this.lvJoinTables.Name = "lvJoinTables";
            this.lvJoinTables.Size = new System.Drawing.Size(286, 235);
            this.lvJoinTables.TabIndex = 40;
            this.lvJoinTables.UseCompatibleStateImageBehavior = false;
            this.lvJoinTables.View = System.Windows.Forms.View.Details;
            this.lvJoinTables.VirtualMode = true;
            this.lvJoinTables.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.LvJoinTables_RetrieveVirtualItem);
            this.lvJoinTables.SelectedIndexChanged += new System.EventHandler(this.LvJoinTables_SelectedIndexChanged);
            this.lvJoinTables.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvJoinTables_MouseDoubleClick);
            // 
            // lb1
            // 
            this.lb1.AutoSize = true;
            this.lb1.Location = new System.Drawing.Point(8, 77);
            this.lb1.Name = "lb1";
            this.lb1.Size = new System.Drawing.Size(179, 20);
            this.lb1.TabIndex = 41;
            this.lb1.Text = "All Schemas and Tables";
            // 
            // lb2
            // 
            this.lb2.AutoSize = true;
            this.lb2.Location = new System.Drawing.Point(294, 77);
            this.lb2.Name = "lb2";
            this.lb2.Size = new System.Drawing.Size(161, 20);
            this.lb2.TabIndex = 42;
            this.lb2.Text = "Active Table Columns";
            // 
            // lb3
            // 
            this.lb3.AutoSize = true;
            this.lb3.Location = new System.Drawing.Point(584, 77);
            this.lb3.Name = "lb3";
            this.lb3.Size = new System.Drawing.Size(299, 20);
            this.lb3.TabIndex = 43;
            this.lb3.Text = "Adjacent Schemas, Tables, and Columns";
            // 
            // lb4
            // 
            this.lb4.AutoSize = true;
            this.lb4.Location = new System.Drawing.Point(870, 77);
            this.lb4.Name = "lb4";
            this.lb4.Size = new System.Drawing.Size(265, 20);
            this.lb4.TabIndex = 44;
            this.lb4.Text = "Join Schemas, Tables, and Columns";
            // 
            // btnResetJoin
            // 
            this.btnResetJoin.Location = new System.Drawing.Point(1166, 198);
            this.btnResetJoin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnResetJoin.Name = "btnResetJoin";
            this.btnResetJoin.Size = new System.Drawing.Size(114, 35);
            this.btnResetJoin.TabIndex = 45;
            this.btnResetJoin.Text = "Reset Join";
            this.btnResetJoin.UseVisualStyleBackColor = true;
            this.btnResetJoin.Click += new System.EventHandler(this.BtnResetJoin_Click);
            // 
            // btnTestJoin
            // 
            this.btnTestJoin.Location = new System.Drawing.Point(1166, 249);
            this.btnTestJoin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnTestJoin.Name = "btnTestJoin";
            this.btnTestJoin.Size = new System.Drawing.Size(114, 35);
            this.btnTestJoin.TabIndex = 46;
            this.btnTestJoin.Text = "Test Join";
            this.btnTestJoin.UseVisualStyleBackColor = true;
            this.btnTestJoin.Click += new System.EventHandler(this.BtnTestJoin_Click);
            // 
            // dgvQuery
            // 
            this.dgvQuery.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvQuery.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQuery.Location = new System.Drawing.Point(6, 343);
            this.dgvQuery.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvQuery.Name = "dgvQuery";
            this.dgvQuery.RowHeadersWidth = 62;
            this.dgvQuery.Size = new System.Drawing.Size(1726, 333);
            this.dgvQuery.TabIndex = 47;
            this.dgvQuery.VirtualMode = true;
            this.dgvQuery.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.DgvQuery_CellValueNeeded);
            this.dgvQuery.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvQuery_ColumnHeaderMouseClick);
            // 
            // D00B
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1743, 780);
            this.Controls.Add(this.dgvQuery);
            this.Controls.Add(this.btnTestJoin);
            this.Controls.Add(this.btnResetJoin);
            this.Controls.Add(this.lb4);
            this.Controls.Add(this.lb3);
            this.Controls.Add(this.lb2);
            this.Controls.Add(this.lb1);
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
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtConnString);
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "D00B";
            this.Text = "DóòB-👁";
            this.Load += new System.EventHandler(this.D00B_Load);
            this.Shown += new System.EventHandler(this.D00B_Show);
            this.Resize += new System.EventHandler(this.D00B_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuery)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtConnString;
        private System.Windows.Forms.Button btnLoad;
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
        private System.Windows.Forms.Label lb1;
        private System.Windows.Forms.Label lb2;
        private System.Windows.Forms.Label lb3;
        private System.Windows.Forms.Label lb4;
        private System.Windows.Forms.Button btnResetJoin;
        private System.Windows.Forms.Button btnTestJoin;
        private System.Windows.Forms.DataGridView dgvQuery;
    }
}

