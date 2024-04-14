using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace D00B
{
    public partial class D00B : Form
    {
        BackgroundWorker m_BkgSQL;
        Dictionary<DBTableKey, DBTable> m_TableMap;
        List<DBTableKey> m_TableKeys = new List<DBTableKey>();
        List<DBJoinKey> m_JoinKeysFr = new List<DBJoinKey>();
        List<DBJoinKey> m_JoinKeysTo = new List<DBJoinKey>();
        int m_nCT = 0; // Number of correlation tables
        float m_nFontHeight = 0;
        int m_nMaxSchemaWidth = 0;
        int m_nMaxTableWidth = 0;
        int m_nMaxColumnWidth = 0;
        Font m_Font;

        CArray m_Arr;
        string[] m_Header;
        int[] m_Width;
        bool[] m_SortOrder;

        int m_nCount = -1;
        int m_nPreview = 100;

        // Screen rectangle and positions of the controls
        private Rectangle m_DialogRect;
        private int m_btnLoadLeft;
        private int m_pbDataLeft;
        private int m_txtDataLeft;
        private int m_txtSearchLeft;
        private int m_lvResultsLeft;
        private int m_chkExactLeft;
        private int m_chkTableLeft;
        private int m_chkDataLeft;
        private int m_btnSearchLeft;
        private int m_btnJoinLeft;
        private int m_btnResetJoinLeft;
        private int m_btnTestJoinLeft;
        private int m_btnExportLeft;
        private int m_tbTablesLeft;
        private int m_dgvQueryWidth;
        private int m_dgvQueryHeight;
        private int m_txtQueryWidth;
        private int m_txtQueryTop;
        private int m_cbDataBasesWidth;
        private int m_txtConnStringWidth;
        private int m_lvTablesWidth;
        private int m_lvColumnsWidth;
        private int m_lvAdjTablesWidth;
        private int m_lvJoinTablesWidth;

        public D00B()
        {
            InitializeComponent();
            InitializeBackgroundSQL();
        }

        private void D00B_Load(object sender, EventArgs e)
        {
            // Output the licensing disclaimer
            Debug.WriteLine(@"D00B-I.exe Copyright (C) 2023-Present Andrew S. Bantly");
            Debug.WriteLine("Andrew S. Bantly can be reached at andybantly@hotmail.com");
            Debug.WriteLine("D00B-I.exe comes with ABSOLUTELY NO WARRANTY");
            Debug.WriteLine("This is free software, and you are welcome to redistribute it");
            Debug.WriteLine("under of the GNU General Public License as published by");
            Debug.WriteLine("the Free Software Foundation; version 2 of the License.");
            Debug.WriteLine("");
            Debug.WriteLine(@"""We can forgive a man for making a useful thing as long as he does not admire it. The only excuse for making a useless thing is that one admires it intensely. All art is quite useless"" - Oscar Wilde, The Picture of Dorian Gray");

            ClearUI();
            UpdateUI(false);

            string strUserID = WindowsIdentity.GetCurrent().Name;
            cbDataBases.Items.Add(string.Format(@"Data Source=.\;Initial Catalog=AdventureWorks2022;MultipleActiveResultSets=True;Integrated Security=True;User ID={0};", strUserID));
            cbDataBases.Items.Add(string.Format(@"Data Source=.\;Initial Catalog=master;MultipleActiveResultSets=True;Integrated Security=True;User ID={0};", strUserID));  // accidentally installed this to master, if you install it properly then you will need to change the catalog
            cbDataBases.Items.Add(string.Format(@"Data Source=.\;Initial Catalog=WideWorldImporters;MultipleActiveResultSets=True;Integrated Security=True;User ID={0};", strUserID));
            cbDataBases.Items.Add(string.Format(@"Data Source=.\;Initial Catalog=pubs;MultipleActiveResultSets=True;Integrated Security=True;User ID={0};", strUserID));
            cbDataBases.SelectedIndex = 0;

            txtConnString.Text = cbDataBases.Text;
            chkPrevAll.Checked = false;
            txtPreview.Text = m_nPreview.ToString();

            m_nFontHeight = lvTables.Font.Size;
            m_Font = Utility.MakeFont(m_nFontHeight, FontFamily.GenericMonospace, FontStyle.Bold);
            dgvQuery.AllowUserToDeleteRows = false;
            dgvQuery.Font = m_Font;
            lvTables.View = View.Details;
            lvTables.Font = m_Font;
            lvColumns.View = View.Details;
            lvColumns.Font = m_Font;
            lvAdjTables.View = View.Details;
            lvAdjTables.Font = m_Font;
            lvJoinTables.View = View.Details;
            lvJoinTables.Font = m_Font;
            lvResults.View = View.Details;
            lvResults.Font = m_Font;
        }
        private void D00B_Resize(object sender, EventArgs e)
        {
            OnSizing();
        }
        private void D00B_Show(object sender, EventArgs e)
        {
            // Get the dialog rectangle
            MinimumSize = Size;

            // Get the coordinates that will be used for moving/resizing when the dialogs size changes
            m_DialogRect = ClientRectangle;
            m_btnLoadLeft = btnLoad.Left;
            m_pbDataLeft = pbData.Left;
            m_txtDataLeft = txtData.Left;
            m_txtSearchLeft = txtSearch.Left;
            m_lvResultsLeft = lvResults.Left;
            m_chkExactLeft = chkExact.Left;
            m_chkTableLeft = chkTable.Left;
            m_chkDataLeft = chkData.Left;
            m_btnSearchLeft = btnSearch.Left;
            m_btnJoinLeft = btnJoin.Left;
            m_btnResetJoinLeft = btnResetJoin.Left;
            m_btnTestJoinLeft = btnTestJoin.Left;
            m_btnExportLeft = btnExport.Left;
            m_tbTablesLeft = tbTables.Left;
            m_dgvQueryWidth = dgvQuery.Width;
            m_dgvQueryHeight = dgvQuery.Height;
            m_txtQueryWidth = txtQuery.Width;
            m_txtQueryTop = txtQuery.Top;
            m_cbDataBasesWidth = cbDataBases.Width;
            m_txtConnStringWidth = txtConnString.Width;
            m_lvTablesWidth = lvTables.Width;
            m_lvColumnsWidth = lvColumns.Width;
            m_lvAdjTablesWidth = lvAdjTables.Width;
            m_lvJoinTablesWidth = lvJoinTables.Width;
        }
        private void OnSizing()
        {
            // Get the dialog rectangle
            Rectangle DialogRect = ClientRectangle;

            // Get the dialog size delta
            Point PtDiff = new Point(DialogRect.Right - m_DialogRect.Right, DialogRect.Bottom - m_DialogRect.Bottom);

            // Move the load button
            btnLoad.Left = m_btnLoadLeft + PtDiff.X;

            // Move the progress bar
            pbData.Left = m_pbDataLeft + PtDiff.X;

            // Move the search text data
            txtData.Left = m_txtDataLeft + PtDiff.X;

            // Move the search text
            txtSearch.Left = m_txtSearchLeft + PtDiff.X;

            // Move the search results listview
            lvResults.Left = m_lvResultsLeft + PtDiff.X;
            
            // Move the exact check box
            chkExact.Left = m_chkExactLeft + PtDiff.X;

            // Move the table check box
            chkTable.Left = m_chkTableLeft + PtDiff.X;

            // Move the data check box
            chkData.Left = m_chkDataLeft + PtDiff.X;

            // Move the search button
            btnSearch.Left = m_btnSearchLeft + PtDiff.X;

            // Move the join button
            btnJoin.Left = m_btnJoinLeft + PtDiff.X;

            // Move the reset join button
            btnResetJoin.Left = m_btnResetJoinLeft + PtDiff.X;

            // Move the test join button
            btnTestJoin.Left = m_btnTestJoinLeft + PtDiff.X;

            // Move the export button
            btnExport.Left = m_btnExportLeft + PtDiff.X;

            // Move the tables count
            tbTables.Left = m_tbTablesLeft + PtDiff.X;

            // Expand the query results
            dgvQuery.Width = m_dgvQueryWidth + PtDiff.X;
            dgvQuery.Height = m_dgvQueryHeight + PtDiff.Y;

            // Expand the text query
            txtQuery.Width = m_txtQueryWidth + PtDiff.X;
            txtQuery.Top = m_txtQueryTop + PtDiff.Y;

            // Expand the database combobox
            cbDataBases.Width = m_cbDataBasesWidth + PtDiff.X;

            // Expand the connection string text box
            txtConnString.Width = m_txtConnStringWidth + PtDiff.X;

            // Move and size the labels and list views
            int dX = PtDiff.X / 4;

            // Move and resize the list views
            lvTables.Width = m_lvTablesWidth + dX;
            lb1.Left = lvTables.Left;

            lvColumns.Left = lvTables.Right + 1;
            lvColumns.Width = m_lvColumnsWidth + dX;
            lb2.Left = lvColumns.Left;

            lvAdjTables.Left = lvColumns.Right + 1;
            lvAdjTables.Width = m_lvAdjTablesWidth + dX;
            lb3.Left = lvAdjTables.Left;

            lvJoinTables.Left = lvAdjTables.Right + 1;
            lvJoinTables.Width = m_lvJoinTablesWidth + dX;
            lb4.Left = lvJoinTables.Left;
        }

        void UpdateUI(bool bEnabled)
        {
            txtConnString.Enabled = true;
            btnLoad.Enabled = true;
            chkPrevAll.Enabled = true;
            lvTables.Enabled = bEnabled;
            lvAdjTables.Enabled = bEnabled;
            lvJoinTables.Enabled = bEnabled;
            dgvQuery.Enabled = bEnabled;
            txtPreview.Enabled = bEnabled;
            lblPreview.Enabled = bEnabled;
            btnExport.Enabled = bEnabled;
            chkHdr.Enabled = bEnabled;
            cbSchema.Enabled = bEnabled;
            label2.Enabled = bEnabled;
            tbTables.Enabled = bEnabled;
            txtSearch.Enabled = bEnabled;
            btnSearch.Enabled = bEnabled;
            lvResults.Enabled = bEnabled;
            chkTable.Enabled = bEnabled;
            chkData.Enabled = bEnabled;
            txtData.Enabled = bEnabled;
            pbData.Enabled = bEnabled;
            chkExact.Enabled = bEnabled;
            lvColumns.Enabled = bEnabled;
            btnJoin.Enabled = bEnabled && ColumnIndex() != -1 && lvAdjTables.Items.Count > 0;
            btnResetJoin.Enabled = bEnabled && m_JoinKeysFr.Count > 0;
            btnTestJoin.Enabled = bEnabled && TableIndex() != -1 && ColumnIndex() != -1 && JoinTablesIndex() != -1;
            btnJoin.Visible = false;
            btnResetJoin.Visible = false;
            btnTestJoin.Visible = false;
        }

        private int UpdateMaxWidth(string strField, int nCurrentWidth)
        {
            Size szExtra = TextRenderer.MeasureText(strField, m_Font);
            if (szExtra.Width > nCurrentWidth)
                nCurrentWidth = szExtra.Width;
            return nCurrentWidth;
        }
        private void CountTablesAndRows()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string strConnectionString = txtConnString.Text;

                // Count of rows in each table
                string strQueryString;
                if (cbSchema.SelectedIndex == 0)
                    strQueryString = "select distinct schema_name(t.schema_id) as schema_name, t.name as table_name, p.[Rows] 'Rows' from sys.tables as t INNER JOIN sys.indexes as i ON t.OBJECT_ID = i.object_id INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id where p.[Rows] >= 0 order by schema_name;";
                else
                    strQueryString = string.Format("select distinct schema_name(t.schema_id) as schema_name, t.name as table_name, p.[Rows] 'Rows' from sys.tables as t INNER JOIN sys.indexes as i ON t.OBJECT_ID = i.object_id INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id where p.[Rows] >= 0 and schema_name(t.schema_id) = '{0}' order by schema_name;", cbSchema.Text);
                SQL SqlTables = new SQL(strConnectionString, strQueryString);

                m_nMaxSchemaWidth = 0;
                m_nMaxTableWidth = 0;
                m_nMaxColumnWidth = 0;
                int iSelectedIndex = 0;
                m_TableMap = new Dictionary<DBTableKey, DBTable>();
                if (SqlTables.ExecuteReader(out string strError))
                {
                    if (!string.IsNullOrEmpty(strError))
                        throw new Exception(strError);
                    while (SqlTables.Read())
                    {
                        string strSchema = SqlTables.GetValue(0).ToString();
                        string strTable = SqlTables.GetValue(1).ToString();
                        string strRows = SqlTables.GetValue(2).ToString();
                        if (strRows == "0")
                        {
                            // Test ROW count
                            string strQueryStringCount = string.Format("select count(*) from [{0}].[{1}]", strSchema, strTable);
                            SQL SqlCount = new SQL(strConnectionString, strQueryStringCount);
                            string strCount = Convert.ToString(SqlCount.ExecuteScalar(out strError));
                            if (string.IsNullOrEmpty(strError))
                                strRows = strCount;
                            SqlCount.Close();
                        }

                        DBTableKey TableKey = new DBTableKey(strSchema, strTable, string.Empty);
                        if (!m_TableMap.ContainsKey(TableKey))
                        {
                            DBTable Table = new DBTable(TableKey)
                            {
                                SelectedIndex = iSelectedIndex,
                                Rows = strRows
                            };
                            m_TableMap.Add(TableKey, Table);
                            iSelectedIndex++;
                        }

                        // Measure the header text
                        m_nMaxSchemaWidth = UpdateMaxWidth(strSchema, m_nMaxSchemaWidth);
                        m_nMaxTableWidth = UpdateMaxWidth(strTable, m_nMaxTableWidth);
                    }
                }
                else
                    MessageBox.Show(strError);
                SqlTables.Close();

                // Now do views
                if (cbSchema.SelectedIndex == 0)
                    strQueryString = "select distinct schema_name(v.schema_id) as schema_name, v.name as table_name from sys.views as v order by schema_name;";
                else
                    strQueryString = string.Format("select distinct schema_name(v.schema_id) as schema_name, v.name as table_name from sys.views as v where schema_name='{0}' order by schema_name;", cbSchema.Text);
                SQL SqlViews = new SQL(strConnectionString, strQueryString);

                if (SqlViews.ExecuteReader(out strError))
                {
                    if (!string.IsNullOrEmpty(strError))
                        throw new Exception(strError);
                    while (SqlViews.Read())
                    {
                        string strSchema = SqlViews.GetValue(0).ToString();
                        string strTable = SqlViews.GetValue(1).ToString();

                        DBTableKey TableKey = new DBTableKey(strSchema, strTable, string.Empty);
                        if (!m_TableMap.ContainsKey(TableKey))
                        {
                            DBTable Table = new DBTable(TableKey)
                            {
                                SelectedIndex = iSelectedIndex,
                                View = true
                            };
                            m_TableMap.Add(TableKey, Table);
                            iSelectedIndex++;
                        }

                        // Measure the header text
                        m_nMaxSchemaWidth = UpdateMaxWidth(strSchema, m_nMaxSchemaWidth);
                        m_nMaxTableWidth = UpdateMaxWidth(strTable, m_nMaxTableWidth);
                    }
                }
                else
                    MessageBox.Show(strError);
                SqlViews.Close();

                foreach (KeyValuePair<DBTableKey, DBTable> kvp in m_TableMap)
                {
                    DBTable Table = kvp.Value;
                    strQueryString = "[sys].[sp_pkeys]";
                    SQL SqlPK = new SQL(strConnectionString, strQueryString, true);
                    SqlPK.AddWithValue("@table_name", Table.TableName, SqlDbType.NVarChar);
                    SqlPK.AddWithValue("@table_owner", Table.TableSchema, SqlDbType.NVarChar);
                    if (SqlPK.ExecuteReader(out strError))
                    {
                        if (!string.IsNullOrEmpty(strError))
                            throw new Exception(strError);
                        while (SqlPK.Read())
                        {
                            string strTableOwner = SqlPK.GetValue("TABLE_OWNER").ToString();
                            string strTableName = SqlPK.GetValue("TABLE_NAME").ToString();
                            string strColumnName = SqlPK.GetValue("COLUMN_NAME").ToString();
                            DBTableKey TK = new DBTableKey(strTableOwner, strTableName, strColumnName);
                            Table.AddPK(TK);
                        }
                    }
                    SqlPK.Close();
                }

                // For every table get the list of the tables primary keys and foreign keys and map as two database keys, then get the table columns and if they are keys or not
                pbData.Minimum = 1;
                pbData.Maximum = m_TableMap.Count;
                int nData = 0;
                foreach (KeyValuePair<DBTableKey, DBTable> KVP in m_TableMap)
                {
                    pbData.Value = ++nData;
                    DBTable Table = KVP.Value;

                    // Build the list of adjacent tables for each column (edges!)
                    strQueryString = "[sys].[sp_fkeys]";
                    SQL SqlFK = new SQL(strConnectionString, strQueryString, true);
                    SqlFK.AddWithValue("@pktable_name", Table.TableName, SqlDbType.NVarChar);
                    SqlFK.AddWithValue("@pktable_owner", Table.TableSchema, SqlDbType.NVarChar);
                    if (SqlFK.ExecuteReader(out strError))
                    {
                        if (!string.IsNullOrEmpty(strError))
                            throw new Exception(strError);
                        while (SqlFK.Read())
                        {
                            string strPKOwn = SqlFK.GetValue("PKTABLE_OWNER").ToString();
                            string strPKTab = SqlFK.GetValue("PKTABLE_NAME").ToString();
                            string strPKCol = SqlFK.GetValue("PKCOLUMN_NAME").ToString();
                            string strFKOwn = SqlFK.GetValue("FKTABLE_OWNER").ToString();
                            string strFKTab = SqlFK.GetValue("FKTABLE_NAME").ToString();
                            string strFKCol = SqlFK.GetValue("FKCOLUMN_NAME").ToString();

                            // Keys
                            Table.AddKeyMap(strPKOwn, strPKTab, strPKCol, strFKOwn, strFKTab, strFKCol);
                        }
                        SqlFK.Close();
                    }

                    // Build the list of columns (edges) for each table (room), some doors will be keyed
                    strQueryString = "[sys].[sp_columns]";
                    SQL SqlCol = new SQL(strConnectionString, strQueryString, true);
                    SqlCol.AddWithValue("@table_name", Table.TableName, SqlDbType.NVarChar);
                    SqlCol.AddWithValue("@table_owner", Table.TableSchema, SqlDbType.NVarChar);
                    if (SqlCol.ExecuteReader(out strError))
                    {
                        if (!string.IsNullOrEmpty(strError))
                            throw new Exception(strError);
                        List<DBColumn> Columns = new List<DBColumn>();
                        while (SqlCol.Read())
                        {
                            string strCol = SqlCol.GetValue("COLUMN_NAME").ToString();
                            string strTableOwner = SqlCol.GetValue("TABLE_OWNER").ToString();
                            DBTableKey TK = new DBTableKey(strTableOwner, Table.TableName, strCol);
                            Columns.Add(new DBColumn(strCol, Table.ContainsPK(strTableOwner, Table.TableName, strCol)));
                        }
                        Table.Columns = Columns;
                        SqlCol.Close();
                    }
                }
            }
            catch { }
            finally
            {
                pbData.Value = pbData.Minimum;
                Cursor.Current = Cursors.Default;
            }
        }

        private void SetupJoinTables()
        {
            // Setup the join table headers
            SetupListViewHeaders(lvJoinTables);
        }

        private void UpdateJoinTable()
        {
            int nTotalColumns = 0;
            foreach (KeyValuePair<DBTableKey, DBTable> KVP in m_TableMap) { nTotalColumns += KVP.Value.Columns.Count; }
            lvJoinTables.VirtualListSize = nTotalColumns;
            lvJoinTables.SelectedIndices.Clear();
        }

        private void RefreshIndex()
        {
            if (!int.TryParse(txtPreview.Text, out int _))
                return;

            try
            {
                // Suspend retrieval of virtual items
                dgvQuery.Rows.Clear();
                lvTables.VirtualListSize = 0;
                lvJoinTables.VirtualListSize = 0;

                // Collect information about the database
                CountTablesAndRows();

                // Setup the table headers
                SetupListViewHeaders(lvTables, true, true, false);

                // Enable/Disable
                tbTables.Text = string.Format("{0}", m_TableMap.Count);
                lvTables.Enabled = m_TableMap.Count > 0;

                // Select the first column to activate the selectindex trigger that populates the columns and adjacent table list
                if (lvTables.Enabled)
                {
                    // Update the virtual sizes
                    lvTables.VirtualListSize = m_TableMap.Count;

                    // Select the first item
                    if (lvTables.VirtualListSize > 0)
                        lvTables.SelectedIndices.Add(0);
                }

                UpdateUI(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally { }
        }

        private void SelectIndex()
        {
            // Do the work
            UpdateIndex();
        }
        private void UpdateIndex()
        {
            if (m_TableMap.Count > 0 && m_TableKeys.Count > 0)
            {
                // Get the current table selection
                int iSelectedIndex = TableIndex();
                if (iSelectedIndex == -1)
                    return;

                // Clear the output
                dgvQuery.Columns.Clear();
                dgvQuery.Rows.Clear();

                try
                {
                    // Get the current table selection
                    DBTableKey TableKey = m_TableKeys[0];
                    string strSchema = TableKey.Schema;
                    string strTable = TableKey.Table;
                    string strColumn = string.Empty;
                    string strOT = TableKey.JoinTag;

                    // Colums
                    DBTable Table = m_TableMap[TableKey];
                    lvColumns.VirtualListSize = Table.Columns.Count;
                    lvColumns.SelectedIndices.Clear();

                    // Update the current tables adjacent tables and find out where we can go
                    UpdateAdjTables(strSchema, strTable);

                    // Count the result tables data
                    string strError = string.Empty;
                    string strConnectionString = txtConnString.Text;
                    string strQueryString = string.Format("select count(*) from [{0}].[{1}] {2}", strSchema, strTable, strOT);

                    // Update ROW count
                    SQL Sql = new SQL(strConnectionString, strQueryString);
                    string strRows = Convert.ToString(Sql.ExecuteScalar(out strError));
                    Sql.Close();
                    m_TableMap[TableKey].Rows = strRows;
                    bool bCount = false;

                    if (TableIndex() != -1 && ColumnIndex() != -1 && m_TableKeys.Count > 1)
                    {
                        // When switching tables during a join, the number of table keys is less than the join keys
                        // ? - loop through tables first, then joins
                        bCount = true;
                        for (int idx = 0; idx < m_TableKeys.Count - 1; idx++)
                        {
                            Utility.Join JoinType = m_JoinKeysFr[idx].JoinType;
                            string strJoinType = m_JoinKeysFr[idx].ToString();
                            string strCF = m_JoinKeysFr[idx].JoinTag;
                            string strCT = m_JoinKeysTo[idx].JoinTag;
                            string strSrcOwn = m_JoinKeysFr[idx].Schema;
                            string strSrcTable = m_JoinKeysFr[idx].Table;
                            string strSrcColumn = m_JoinKeysFr[idx].Column;
                            string strJoinSchema = m_JoinKeysTo[idx].Schema;
                            string strJoinTable = m_JoinKeysTo[idx].Table;
                            string strJoinColumn = m_JoinKeysTo[idx].Column;
                            if (JoinType != Utility.Join.Self)
                            {
                                strQueryString = string.Format("{0} {1} join [{2}].[{3}] {4} on {5}.{6} = {7}.{8}",
                                strQueryString, strJoinType,
                                strJoinSchema, strJoinTable, strCT,
                                strCF, strSrcColumn,
                                strCT, strJoinColumn);
                            }
                            else
                            {
                                strQueryString = string.Format("{0} join [{1}].[{2}] {3} on {4}.{5} = {6}.{7}",
                                strQueryString,
                                strSrcOwn, strSrcTable, strCT,
                                strCF, strSrcColumn,
                                strCT, strJoinColumn);
                            }
                        }
                    }

                    // Search?
                    if (chkData.Checked)
                    {
                        if (!string.IsNullOrEmpty(txtData.Text))
                        {
                            if (TableIndex() != -1 && ColumnIndex() != -1)
                            {
                                bCount = true;
                                strSchema = lvResults.SelectedItems[0].Text;
                                strTable = lvTables.Items[TableIndex()].SubItems[1].Text;
                                strColumn = lvColumns.Items[ColumnIndex()].Text;

                                string strSelect = "count(*)";
                                bool bIsNum = IsNumber(txtData.Text);
                                string strTest = bIsNum ? "=" : (chkExact.Checked ? "=" : "like");
                                string strTestVal = bIsNum ? txtData.Text : (chkExact.Checked ? string.Format("'{0}'", txtData.Text) : string.Format("'%{0}%'", txtData.Text));
                                strQueryString = string.Format("select {0} from [{1}].[{2}] where [{3}].[{4}].{5} {6} {7}", strSelect, strSchema, strTable, strSchema, strTable, strColumn, strTest, strTestVal);
                            }
                        }
                    }

                    // Get the list size which will become the virtual list size when the operation completes
                    int nCount;
                    if (bCount)
                    {
                        Sql = new SQL(strConnectionString, strQueryString);
                        nCount = Convert.ToInt32(Sql.ExecuteScalar(out strError));
                        Sql.Close();
                    }
                    else
                        nCount = Convert.ToInt32(strRows);

                    // Set the final count of rows in the view
                    nCount = chkPrevAll.Checked ? nCount : Math.Min(nCount, m_nPreview);

                    // Search criteria
                    strColumn = string.Empty;
                    if (ColumnIndex() != -1)
                        strColumn = lvColumns.Items[ColumnIndex()].Text;

                    strQueryString = string.Empty;

                    // Search?
                    if (chkData.Checked)
                    {
                        if (!string.IsNullOrEmpty(txtData.Text))
                        {
                            if (TableIndex() != -1 && ColumnIndex() != -1)
                            {
                                string strSelect = chkPrevAll.Checked ? "*" : string.Format("top {0} *", nCount);
                                bool bIsNum = IsNumber(txtData.Text);
                                string strTest = bIsNum ? "=" : (chkExact.Checked ? "=" : "like");
                                string strTestVal = bIsNum ? txtData.Text : (chkExact.Checked ? string.Format("'{0}'", txtData.Text) : string.Format("'%{0}%'", txtData.Text));
                                strQueryString = string.Format("select {0} from [{1}].[{2}] where [{3}].[{4}].{5} {6} {7}", strSelect, strSchema, strTable, strSchema, strTable, strColumn, strTest, strTestVal);
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(strQueryString))
                    {
                        if (chkPrevAll.Checked)
                            strQueryString = string.Format("select * from [{0}].[{1}] {2}", strSchema, strTable, strOT);
                        else
                            strQueryString = string.Format("select top {0} * from [{1}].[{2}] {3}", nCount, strSchema, strTable, strOT);

                        // Join and preview the output
                        if (TableIndex() != -1 && ColumnIndex() != -1 && m_TableKeys.Count > 1)
                        {
                            for (int idx = 0; idx < m_TableKeys.Count - 1; idx++)
                            {
                                Utility.Join JoinType = m_JoinKeysFr[idx].JoinType;
                                string strJoinType = m_JoinKeysFr[idx].ToString();
                                string strSrcOwn = m_JoinKeysFr[idx].Schema;
                                string strSrcTable = m_JoinKeysFr[idx].Table;
                                string strSrcColumn = m_JoinKeysFr[idx].Column;
                                string strCF = m_JoinKeysFr[idx].JoinTag;
                                string strJoinSchema = m_JoinKeysTo[idx].Schema;
                                string strJoinTable = m_JoinKeysTo[idx].Table;
                                string strJoinColumn = m_JoinKeysTo[idx].Column;
                                string strCT = m_JoinKeysTo[idx].JoinTag;
                                if (JoinType != Utility.Join.Self)
                                {
                                    strQueryString = string.Format("{0} {1} join [{2}].[{3}] {4} on {5}.{6} = {7}.{8}",
                                    strQueryString, strJoinType,
                                    strJoinSchema, strJoinTable, strCT,
                                    strCF, strSrcColumn,
                                    strCT, strJoinColumn);
                                }
                                else
                                {
                                    strQueryString = string.Format("{0} join [{1}].[{2}] {3} on {4}.{5} = {6}.{7}",
                                    strQueryString,
                                    strSrcOwn, strSrcTable, strCT,
                                    strCF, strSrcColumn,
                                    strCT, strJoinColumn);
                                }
                            }
                        }
                    }

                    lvTables.Invalidate();
                    lvColumns.Invalidate();

                    // Perform the search
                    txtQuery.Text = strQueryString;

                    // Process the SQL query in the background
                    StartBackgroundSQL(strConnectionString, strQueryString, nCount);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                }
            }
        }

        private void UpdateAdjTables(string strSchema, string strTable)
        {
            // Setup the adjacent table headers
            SetupListViewHeaders(lvAdjTables);

            DBTableKey TK = new DBTableKey(strSchema, strTable, string.Empty);
            bool bInclude = m_TableMap.ContainsKey(TK);
            if (bInclude)
            {
                DBTable Table = m_TableMap[TK];
                foreach (DBColumn Column in Table.Columns)
                {
                    // Is it a key?
                    string strColumn = Column.Name;
                    if (Column.IsPrimaryKey)
                    {
                        // Get the column it goes by in the adjacent foreign table
                        List<DBTableKey> FKeyList = Table.FKKeyList(strSchema, strTable, strColumn);
                        if (FKeyList != null)
                        {
                            foreach (DBTableKey FK in FKeyList)
                            {
                                ListViewItem Item = new ListViewItem(FK.Schema);
                                Item.UseItemStyleForSubItems = false;
                                ListViewItem.ListViewSubItem SubItem = new ListViewItem.ListViewSubItem(Item, FK.Table);
                                ListViewItem.ListViewSubItem SubItem2 = new ListViewItem.ListViewSubItem(Item, FK.Column);

                                TK = new DBTableKey(strSchema, FK.Table, string.Empty); // Essentially the parent function
                                if (m_TableMap.ContainsKey(TK))
                                {
                                    Table = m_TableMap[TK];
                                    if (Table.Rows == "0")
                                    {
                                        Item.BackColor = Color.Red;
                                        SubItem.BackColor = Color.Red;
                                    }
                                    else
                                    {
                                        // The case where the the foreign key is in the primary table
                                        SubItem2.ForeColor = Color.DarkBlue;
                                        SubItem2.BackColor = Color.Yellow;
                                    }
                                }
                                else
                                {
                                    SubItem2.ForeColor = Color.DarkBlue;
                                    SubItem2.BackColor = Color.Yellow;
                                }

                                Item.SubItems.Add(SubItem);
                                Item.SubItems.Add(SubItem2);
                                lvAdjTables.Items.Add(Item); // DBTableKey FK
                            }
                        }
                    }
                    else
                    {
                        ListViewItem Item = new ListViewItem(strSchema);
                        Item.UseItemStyleForSubItems = false;
                        ListViewItem.ListViewSubItem SubItem = new ListViewItem.ListViewSubItem(Item, strTable);
                        ListViewItem.ListViewSubItem SubItem2 = new ListViewItem.ListViewSubItem(Item, strColumn);

                        TK = new DBTableKey(strSchema, strTable, strColumn);
                        if (Table.HasKey(TK))
                        {
                            if (Table.Rows == "0")
                            {
                                Item.BackColor = Color.Red;
                                SubItem.BackColor = Color.Red;
                                SubItem2.BackColor = Color.Red;
                            }
                            else
                                SubItem2.BackColor = Color.Yellow;
                            SubItem2.ForeColor = Color.DarkBlue;
                        }
                        Item.SubItems.Add(SubItem);
                        Item.SubItems.Add(SubItem2);
                        lvAdjTables.Items.Add(Item); // DBTableKey TK
                    }
                }

                // BackKeys
                DBTableKey TableKey = new DBTableKey(strSchema, strTable, string.Empty);
                if (m_TableMap.ContainsKey(TableKey))
                {
                    DBTable TableL = m_TableMap[TableKey];
                    foreach (KeyValuePair<DBTableKey, DBTable> KVP2 in m_TableMap)
                    {
                        if (KVP2.Key == TableKey)
                            continue;
                        DBTable TableR = KVP2.Value;
                        foreach (DBTableKey TK1 in TableL.Keys)
                        {
                            if (TableR.ContainsFK(TK1.Schema, TK1.Table, TK1.Column))
                            {
                                foreach (DBTableKey TK2 in TableR.Keys)
                                {
                                    if ((TableR.TableSchema == TK2.Schema) && (TableR.TableName == TK2.Table))
                                    {
                                        ListViewItem Item = new ListViewItem(TK2.Schema);
                                        Item.UseItemStyleForSubItems = false;
                                        ListViewItem.ListViewSubItem SubItem = new ListViewItem.ListViewSubItem(Item, TK2.Table);
                                        ListViewItem.ListViewSubItem SubItem2 = new ListViewItem.ListViewSubItem(Item, TK2.Column);
                                        if (Table.HasKey(TK2))
                                        {
                                            if (Table.Rows == "0")
                                            {
                                                Item.BackColor = Color.Red;
                                                SubItem.BackColor = Color.Red;
                                                SubItem2.BackColor = Color.Red;
                                            }
                                            else
                                                SubItem2.BackColor = Color.DarkGreen;
                                        }
                                        else
                                            SubItem2.BackColor = Color.DarkGreen;
                                        SubItem2.ForeColor = Color.Yellow;
                                        
                                        Item.SubItems.Add(SubItem);
                                        Item.SubItems.Add(SubItem2);
                                        lvAdjTables.Items.Add(Item); // DBTableKey TK2
                                    }
                                }
                            }
                        }
                    }
                }
            }
            UpdateUI(true);
        }

        private void ParseKey(string strIn, out string strSchema, out string strTable)
        {
            strSchema = string.Empty;
            strTable = string.Empty;
            if (string.IsNullOrEmpty(strIn))
                return;

            string[] dbInfo = strIn.Split('.');
            strSchema = dbInfo[0];
            strTable = dbInfo[1];
            for (int i = 2; i < dbInfo.Length; ++i)
                strTable += "." + dbInfo[i];
        }
        private int TableIndex()
        {
            ListView.SelectedIndexCollection Col = lvTables.SelectedIndices;
            if (Col == null || Col.Count == 0)
                return -1;
            return Col[0];
        }
        private int ColumnIndex()
        {
            ListView.SelectedIndexCollection Col = lvColumns.SelectedIndices;
            if (Col == null || Col.Count == 0)
                return -1;
            return Col[0];
        }
        private int AdjTablesIndex()
        {
            if (lvAdjTables.SelectedItems.Count > 0)
                return lvAdjTables.SelectedItems[0].Index;
            return -1;
        }
        private int JoinTablesIndex()
        {
            ListView.SelectedIndexCollection Col = lvJoinTables.SelectedIndices;
            if (Col == null || Col.Count == 0)
                return -1;
            return Col[0];
        }
        private int ResultTableIndex()
        {
            if (lvResults.SelectedItems.Count > 0)
                return lvResults.SelectedItems[0].Index;
            return -1;
        }
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            UpdateUI(LoadView());
        }

        private bool LoadView()
        {
            bool bReturn = true;
            try
            {
                ClearUI();
                ClearData();
                RefreshIndex();
                SetupJoinTables();
                SetupSearchResults();
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex.Message);
                bReturn = false;
            }
            return bReturn;
        }

        void ClearData()
        {
            // Clear storages used for UI
            m_Arr = null;
            m_Header = null;
            m_Width = null;
            m_SortOrder = null;
        }

        void ClearUI()
        {
            // Clear UI
            lvTables.Clear();
            lvColumns.Clear();
            lvAdjTables.Clear();
            lvJoinTables.Clear();
            lvResults.Clear();
            dgvQuery.Columns.Clear();
            dgvQuery.Rows.Clear();

            // Get the schemas
            string strSchema = cbSchema.Text;
            cbSchema.Items.Clear();
            cbSchema.Items.Add("All");
            string strConnectionString = txtConnString.Text;
            string strQueryString = string.Format("select name from sys.schemas");
            SQL Sql = new SQL(strConnectionString, strQueryString);
            if (!Sql.ExecuteReader(out string strError))
            {
                if (!string.IsNullOrEmpty(strError))
                    throw new Exception(strError);
            }
            else
            {
                while (Sql.Read())
                {
                    string strItem = Sql.GetValue(0).ToString();
                    if (!string.IsNullOrEmpty(strItem))
                        cbSchema.Items.Add(strItem);
                }
            }
            cbSchema.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(strSchema))
                cbSchema.SelectedItem = strSchema;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvQuery.RowCount > 0 && ExportListView.ExportToExcel(m_Arr, m_Header, "SQL"))
                MessageBox.Show("Successfully exported to Excel");
            else
                MessageBox.Show("Failed to export to Excel");
        }

        private void ChkPrevAll_CheckedChanged(object sender, EventArgs e)
        {
            lblPreview.Enabled = !chkPrevAll.Checked;
            txtPreview.Enabled = !chkPrevAll.Checked;

            // Reload the view
            UpdateUI(LoadView());
        }

        private void SetupListViewHeaders(ListView lv, bool bSchema = true, bool bTable = true, bool bColumn = true, bool bFormat = false)
        {
            lv.Clear();
            if (bSchema)
            {
                lv.Columns.Add("Schema");
                lv.Columns[lv.Columns.Count - 1].Width = Math.Max(TextRenderer.MeasureText("XXXXXXXXXX", m_Font).Width, m_nMaxSchemaWidth);
            }
            if (bTable)
            {
                lv.Columns.Add("Table");
                lv.Columns[lv.Columns.Count - 1].Width = Math.Max(TextRenderer.MeasureText("XXXXXXXXXX", m_Font).Width, m_nMaxTableWidth);
            }
            if (bColumn)
            {
                lv.Columns.Add("Column");
                lv.Columns[lv.Columns.Count - 1].Width = Math.Max(TextRenderer.MeasureText("XXXXXXXXXX", m_Font).Width, m_nMaxColumnWidth);
            }
            if (bFormat)
            {
                lv.Columns.Add("Format");
                lv.Columns[lv.Columns.Count - 1].Width = TextRenderer.MeasureText("XXXXXXXXXX", m_Font).Width;
            }
        }

        private void SetupSearchResults()
        {
            // Setup the search results headers
            SetupListViewHeaders(lvResults);
        }
        private void FillSearchResults()
        {
            foreach (KeyValuePair<DBTableKey, DBTable> KVP in m_TableMap)
            {
                string strOwn = KVP.Key.Schema;
                string strTable = KVP.Key.Table;
                bool bRows = KVP.Value.Rows != "0";

                DBTable Table = KVP.Value;
                List<DBColumn> Columns = Table.Columns;
                foreach (DBColumn Column in Columns)
                {
                    bool bAdd = false;
                    ListViewItem Item = new ListViewItem(strOwn)
                    {
                        UseItemStyleForSubItems = false
                    };

                    ListViewItem.ListViewSubItem TableItem = new ListViewItem.ListViewSubItem(Item, strTable);

                    if (Table.View)
                    {
                        Item.BackColor = Color.Orange;
                        TableItem.BackColor = Color.Orange;
                    }

                    if (!bRows)
                    {
                        Item.BackColor = Color.Red;
                        TableItem.BackColor = Color.Red;
                    }

                    Item.SubItems.Add(TableItem);

                    ListViewItem.ListViewSubItem ColumnItem = new ListViewItem.ListViewSubItem(Item, Column.Name);
                    if (Column.IsPrimaryKey)
                    {
                        if (Table.ContainsFK(strOwn, strTable, Column.Name))
                        {
                            ColumnItem.ForeColor = Color.DarkBlue;
                            ColumnItem.BackColor = Color.Yellow;
                        }
                        else
                        {
                            ColumnItem.ForeColor = Color.DarkBlue;
                            if (bRows)
                                ColumnItem.BackColor = Color.Yellow;
                            else
                                ColumnItem.BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        // See if it is solely a PK
                        DBTableKey TK = new DBTableKey(strOwn, strTable, Column.Name);
                        if (m_TableMap[new DBTableKey(KVP.Key.Schema, KVP.Key.Table, KVP.Key.Column)].HasKey(TK))
                        {
                            ColumnItem.ForeColor = Color.DarkBlue;
                            ColumnItem.BackColor = Color.Yellow;
                        }
                        if (!bRows)
                            ColumnItem.BackColor = Color.Red;
                    }

                    if (!chkTable.Checked && !chkExact.Checked && Column.Name.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        bAdd = true;
                    else if (!chkTable.Checked && chkExact.Checked && string.Equals(Column.Name, txtSearch.Text, StringComparison.OrdinalIgnoreCase))
                        bAdd = true;
                    else if (chkTable.Checked && !chkExact.Checked && strTable.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        bAdd = true;
                    else if (chkTable.Checked && chkExact.Checked && string.Equals(strTable, txtSearch.Text, StringComparison.OrdinalIgnoreCase))
                        bAdd = true;

                    Item.SubItems.Add(ColumnItem);
                    if (bAdd)
                        lvResults.Items.Add(Item);
                }
            }
        }

        private void Search()
        {
            try
            {
                if (txtSearch.Text.Length > 0 && !chkData.Checked)
                {
                    SetupSearchResults();
                    FillSearchResults();
                }
                else if (chkData.Checked)
                {
                    SelectIndex();
                    chkData.Checked = false;
                }
            }
            catch { }
            finally { }

        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }
        public int Compare(bool bAsc, object lhs, object rhs)
        {
            string sLhs = lhs != null ? lhs.ToString() : string.Empty;
            string sRhs = rhs != null ? rhs.ToString() : string.Empty;
            bool bLhsNum = int.TryParse(sLhs, out int iLhs);
            bool bRhsNum = int.TryParse(sRhs, out int iRhs);

            bool bNumber = bLhsNum && bRhsNum;
            if (bNumber)
                return bAsc ? (iLhs < iRhs ? -1 : (iLhs == iRhs ? 0 : 1)) : (iLhs < iRhs ? 1 : (iLhs == iRhs ? 0 : -1));
            else
                return bAsc ? string.Compare(sLhs, sRhs) : string.Compare(sRhs, sLhs);
        }
        private bool IsNumber(object o)
        {
            string s = o != null ? o.ToString() : string.Empty;
            bool bNum = int.TryParse(s, out int _);
            if (!bNum)
                bNum = double.TryParse(s, out _);
            return bNum;
        }
        private void DgvQuery_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Set the busy cursor
            Cursor.Current = Cursors.WaitCursor;

            // Sort using the classes comparer
            Global.g_bSortOrder = m_SortOrder[e.ColumnIndex];

            // Sort the indexed column and rearrange
            m_Arr.ParallelSort(e.ColumnIndex);

            m_SortOrder[e.ColumnIndex] = !m_SortOrder[e.ColumnIndex];
            dgvQuery.Invalidate();

            // Set the default cursor
            Cursor.Current = Cursors.Default;
        }

        private void ChkData_CheckedChanged(object sender, EventArgs e)
        {
            if (chkData.Checked)
                chkTable.Checked = false;
        }
        private void ChkTable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTable.Checked)
                chkData.Checked = false;
            Search();
        }

        private void BtnSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }
        private string TableCurSel(int idx)
        {
            string strSelection = string.Empty;
            if (m_TableMap != null && idx < m_TableMap.Count)
            {
                foreach (KeyValuePair<DBTableKey, DBTable> KVP in m_TableMap)
                {
                    if (KVP.Value.SelectedIndex == idx)
                    {
                        strSelection = string.Format("{0}.{1}", KVP.Key.Schema, KVP.Key.Table);
                        break;
                    }
                }
            }
            return strSelection;
        }
        private void DgvQuery_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                int iRow = e.RowIndex;
                int iCol = e.ColumnIndex;

                if (dgvQuery.Rows[iRow].HeaderCell.Value == null)
                    dgvQuery.Rows[iRow].HeaderCell.Value = (iRow + 1).ToString();
                if (m_Arr != null && iRow < m_Arr.RowLength && iCol < m_Arr.ColLength)
                {
                    try
                    {
                        e.Value = m_Arr[iCol][iRow]?.ToString(dgvQuery.Columns[iCol].DefaultCellStyle.Format); // IFormattable
                    }
                    catch (Exception ex) 
                    {
                        Debug.WriteLine(string.Format("{0} {1},{2}", ex.Message, e.ColumnIndex, e.RowIndex));
                        e.Value = string.Empty;
                    }
                }
                else
                    e.Value = string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0} {1},{2}", ex.Message, e.ColumnIndex, e.RowIndex));
            }
            finally { }
        }

        private void LvTables_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            string strCurSel = TableCurSel(e.ItemIndex);
            ParseKey(strCurSel, out string strSchema, out string strTable);
            try
            {
                DBTableKey TK = new DBTableKey(strSchema, strTable, string.Empty);
                DBTable Table = m_TableMap[TK];
                e.Item = new ListViewItem(strSchema);
                e.Item.UseItemStyleForSubItems = false;
                ListViewItem.ListViewSubItem lvSubValue = new ListViewItem.ListViewSubItem(e.Item, strTable);
                if (Table.Rows == "0")
                {
                    e.Item.BackColor = Color.Red;
                    lvSubValue.BackColor = Color.Red;
                }
                else if (Table.View)
                {
                    e.Item.BackColor = Color.Orange;
                    lvSubValue.BackColor = Color.Orange;
                }
                e.Item.SubItems.Add(lvSubValue);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0} : {1}", e.ItemIndex, ex.Message));
            }
            finally { }
        }
        private void LvJoinTables_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            int nRows, idx;
            try
            {
                // Pivot table
                nRows = 0;
                foreach (KeyValuePair<DBTableKey, DBTable> KVP in m_TableMap)
                {
                    if (nRows + KVP.Value.Columns.Count <= e.ItemIndex)
                        nRows += KVP.Value.Columns.Count;
                    else
                    {
                        DBTableKey TableKey = KVP.Key;
                        DBTable Table = KVP.Value;

                        idx = e.ItemIndex - nRows;
                        e.Item = new ListViewItem(TableKey.Schema);
                        e.Item.UseItemStyleForSubItems = false;
                        e.Item.SubItems.Add(TableKey.Table);
                        DBColumn Column = Table.Columns[idx];
                        e.Item.SubItems.Add(Column.Name);

                        if (Table.Rows == "0")
                        {
                            e.Item.BackColor = Color.Red;
                            e.Item.SubItems[1].BackColor = Color.Red;
                        }

                        if (Column.IsPrimaryKey)
                        {
                            if (Table.ContainsFK(TableKey.Schema, TableKey.Table, Column.Name))
                            {
                                // The case where the the foreign key is in the primary table
                                e.Item.SubItems[2].ForeColor = Color.DarkBlue;
                                e.Item.SubItems[2].BackColor = Color.Yellow;
                            }
                            else
                            {
                                e.Item.SubItems[2].ForeColor = Color.DarkBlue;
                                e.Item.SubItems[2].BackColor = Color.Yellow;
                            }
                        }
                        else
                        {
                            DBTableKey TK = new DBTableKey(TableKey.Schema, TableKey.Table, Column.Name);
                            if (m_TableMap[TableKey].HasKey(TK))
                            {
                                e.Item.SubItems[2].ForeColor = Color.DarkBlue;
                                e.Item.SubItems[2].BackColor = Color.Yellow;
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0} : {1}", e.ItemIndex, ex.Message));
            }
            finally { }
        }
        private void LvJoinTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI(true);
        }
        private void LvColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI(true);
        }
        private void LvColumns_EditFormat(object sender, MouseEventArgs e)
        {
            DBTableKey TableKey = m_TableKeys[0];
            DBTable Table = m_TableMap[TableKey];
            int iColumn = ColumnIndex();
            DBColumn Column = Table.Columns[iColumn];
            string strFormatString = Column.FormatString;

            // Edit the format
            Format FmtDlg = new Format(Column.Name, strFormatString);
            DialogResult Res = FmtDlg.ShowDialog();
            if (Res == DialogResult.OK)
            {
                if (strFormatString != FmtDlg.FormatString)
                {
                    // Update the format string
                    strFormatString = FmtDlg.FormatString;
                    Column.FormatString = strFormatString;

                    // Trigger the new formatting
                    ChangeTables();
                }
            }
        }

        private void LvColumns_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            int iRow = e.ItemIndex;
            try
            {
                if (m_Arr != null)
                {
                    DBTableKey TableKey = m_TableKeys[0]; // Loop through this for all table columns
                    string strSchema = TableKey.Schema;
                    string strTable = TableKey.Table;
                    string strColumn = string.Empty;
                    DBTable Table = m_TableMap[TableKey];
                    DBColumn Column = Table.Columns[iRow];
                    e.Item = new ListViewItem(Column.Name);
                    e.Item.UseItemStyleForSubItems = false;
                    e.Item.SubItems.Add(Column.FormatString);

                    if (Column.IsPrimaryKey)
                    {
                        if (Table.ContainsFK(TableKey.Schema, TableKey.Table, Column.Name))
                        {
                            // The case where the the foreign key is in the primary table
                            e.Item.ForeColor = Color.DarkBlue;
                            e.Item.BackColor = Color.Yellow;
                        }
                        else
                        {
                            e.Item.ForeColor = Color.DarkBlue;
                            e.Item.BackColor = Color.Yellow;
                        }
                    }
                    else
                    {
                        // See if it is solely a PK
                        DBTableKey TK = new DBTableKey(TableKey.Schema, TableKey.Table, Column.Name);
                        if (m_TableMap[TableKey].HasKey(TK))
                        {
                            e.Item.ForeColor = Color.DarkBlue;
                            e.Item.BackColor = Color.Yellow;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0} : {1}", e.ItemIndex, ex.Message));
            }
            finally { }
        }

        private void LvTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTables();
        }

        private void ChangeTables()
        {
            // Stop the virtual list from asking for cell information
            dgvQuery.Columns.Clear();
            dgvQuery.Rows.Clear();
            CancelBackgroundSQL();

            // Start a new list and add the selected table
            // and prepare for joins
            m_nCT = 0;
            m_TableKeys = new List<DBTableKey>();
            m_JoinKeysFr = new List<DBJoinKey>();
            m_JoinKeysTo = new List<DBJoinKey>();

            // Setup the first key in case this isn't a join
            ParseKey(TableCurSel(TableIndex()), out string strSchema, out string strTable);
            if (string.IsNullOrEmpty(strSchema))
                return;

            DBTableKey TableKey = new DBTableKey(strSchema, strTable, string.Empty)
            {
                JoinTag = string.Format("T{0}", ++m_nCT)
            };
            m_TableKeys.Add(TableKey);

            // Setup the column and format list
            lvColumns.VirtualListSize = 0;
            m_nMaxColumnWidth = 0;
            DBTable Table = m_TableMap[TableKey];
            foreach (DBColumn Column in Table.Columns)
                m_nMaxColumnWidth = UpdateMaxWidth(Column.Name, m_nMaxColumnWidth);

            // Setup the column headers
            SetupListViewHeaders(lvColumns, false, false, true, true);

            // Select the table
            SelectIndex();
        }

        private void LvAdjTables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int iAdjTabIdx = AdjTablesIndex();
            if (iAdjTabIdx == -1)
                return;

            string strSchema = lvAdjTables.SelectedItems[0].Text;
            string strTable = lvAdjTables.SelectedItems[0].SubItems[1].Text;
            DBTableKey TK = new DBTableKey(strSchema, strTable, string.Empty);
            if (m_TableMap.ContainsKey(TK))
            {
                int iSelectedIndex = m_TableMap[TK].SelectedIndex;
                lvTables.SelectedIndices.Add(iSelectedIndex);
                SelectIndex();
                lvTables.EnsureVisible(iSelectedIndex);
            }
        }

        private void LvJoinTables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int iJoinTabIdx = JoinTablesIndex();
            if (iJoinTabIdx == -1)
                return;

            string strSchema = lvJoinTables.Items[iJoinTabIdx].Text;
            string strTable = lvJoinTables.Items[iJoinTabIdx].SubItems[1].Text;
            DBTableKey TK = new DBTableKey(strSchema, strTable, string.Empty);
            if (m_TableMap.ContainsKey(TK))
            {
                int iSelectedIndex = m_TableMap[TK].SelectedIndex;
                lvTables.SelectedIndices.Add(iSelectedIndex);
                SelectIndex();
                lvTables.EnsureVisible(iSelectedIndex);
            }
        }
        private void TxtPreview_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool bValid = int.TryParse(txtPreview.Text, out m_nPreview);
            if (bValid && m_nPreview < 1)
                bValid = false;
            txtPreview.BackColor = bValid ? SystemColors.Window : Color.Red;
            e.Cancel = !bValid;
        }
        private void TxtPreview_Validated(object sender, EventArgs e)
        {
            txtPreview.BackColor = SystemColors.Window;
            RefreshIndex();
        }
        private void ChkExact_CheckedChanged(object sender, EventArgs e)
        {
            Search();
        }
        private void LvResults_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int iResIdx = ResultTableIndex();
            if (iResIdx == -1)
                return;

            string strSchema = lvResults.SelectedItems[0].Text;
            string strTable = lvResults.SelectedItems[0].SubItems[1].Text;

            DBTableKey TK = new DBTableKey(strSchema, strTable, string.Empty);
            if (m_TableMap.ContainsKey(TK))
            {
                DBTable Table = m_TableMap[TK];
                int iSelectedIndex = Table.SelectedIndex;
                lvTables.SelectedIndices.Add(iSelectedIndex);
                SelectIndex();
                lvTables.EnsureVisible(iSelectedIndex);
            }
        }

        private void CbDataBases_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtConnString.Text = cbDataBases.Text;

            // Reload the view
            UpdateUI(LoadView());
        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            try
            {
                if (TableIndex() != -1 && ColumnIndex() != -1)
                {
                    string strSrcSchema = lvTables.Items[TableIndex()].Text;
                    string strSrcTable = lvTables.Items[TableIndex()].SubItems[1].Text;
                    string strSrcColumn = lvColumns.Items[ColumnIndex()].Text;
                    string strJoinSchema = string.Empty;
                    string strJoinTable = string.Empty;
                    string strJoinColumn = string.Empty;
                    if (JoinTablesIndex() != -1)
                    {
                        strJoinSchema = lvJoinTables.Items[JoinTablesIndex()].Text;
                        strJoinTable = lvJoinTables.Items[JoinTablesIndex()].SubItems[1].Text;
                        strJoinColumn = lvJoinTables.Items[JoinTablesIndex()].SubItems[2].Text;
                    }

                    DlgJoin dlgJoin = new DlgJoin
                    {
                        SourceSchema = strSrcSchema,
                        SourceTable = strSrcTable,
                        SourceColumn = strSrcColumn,
                        JoinSchema = strJoinSchema,
                        JoinTable = strJoinTable,
                        JoinColumn = strJoinColumn
                    };

                    DialogResult res = dlgJoin.ShowDialog();

                    if (res == DialogResult.OK)
                    {
                        DBJoinKey FKJoin, TKJoin;
                        FKJoin = new DBJoinKey(strSrcSchema, strSrcTable, strSrcColumn, dlgJoin.JoinType);
                        FKJoin.JoinTag = string.Format("T{0}", m_nCT++);
                        m_JoinKeysFr.Add(FKJoin);
                        if (dlgJoin.JoinType != Utility.Join.Self)
                            TKJoin = new DBJoinKey(strJoinSchema, strJoinTable, strJoinColumn, dlgJoin.JoinType);
                        else
                            TKJoin = new DBJoinKey(strSrcSchema, strSrcTable, strSrcColumn, dlgJoin.JoinType);
                        TKJoin.JoinTag = string.Format("T{0}", m_nCT);
                        m_JoinKeysTo.Add(TKJoin);

                        // Add the join table to the list of tables
                        if (dlgJoin.JoinType != Utility.Join.Self)
                            m_TableKeys.Add(new DBTableKey(strJoinSchema, strJoinTable, string.Empty));
                        else
                            m_TableKeys.Add(new DBTableKey(strSrcSchema, strSrcTable, string.Empty));

                        // Select the table
                        SelectIndex();
                    }
                }
            }
            catch { }
            finally { }
        }

        private void BtnResetJoin_Click(object sender, EventArgs e)
        {
            m_nCT = 0;
            m_TableKeys = new List<DBTableKey>();
            ParseKey(TableCurSel(TableIndex()), out string strSchema, out string strTable);
            if (string.IsNullOrEmpty(strSchema))
                return;
            DBTableKey TableKey = new DBTableKey(strSchema, strTable, string.Empty)
            {
                JoinTag = string.Format("T{0}", ++m_nCT)
            };
            m_TableKeys.Add(TableKey);

            // Prepare for joins
            m_JoinKeysFr = new List<DBJoinKey>();
            m_JoinKeysTo = new List<DBJoinKey>();

            // Select the table
            SelectIndex();
        }
        private void BtnTestJoin_Click(object sender, EventArgs e)
        {
            bool bJoin = false;
            if (TableIndex() != -1 && ColumnIndex() != -1 && JoinTablesIndex() != -1)
            {
                string strSrcSchema = lvTables.Items[TableIndex()].Text;
                string strSrcTable = lvTables.Items[TableIndex()].SubItems[1].Text;
                string strSrcColumn = lvColumns.Items[ColumnIndex()].Text;
                string strJoinSchema = lvJoinTables.Items[JoinTablesIndex()].Text;
                string strJoinTable = lvJoinTables.Items[JoinTablesIndex()].SubItems[1].Text;
                string strJoinColumn = lvJoinTables.Items[JoinTablesIndex()].SubItems[2].Text;

                bJoin = strSrcSchema == strJoinSchema &&
                    strSrcTable == strJoinTable &&
                    strSrcColumn == strJoinColumn;
                if (!bJoin)
                {
                    Maze Path = new Maze(m_TableMap);
                    Path.SourceSchema = strSrcSchema;
                    Path.SourceTable = strSrcTable;
                    Path.SourceColumn = strSrcColumn;
                    Path.JoinSchema = strJoinSchema;
                    Path.JoinTable = strJoinTable;
                    Path.JoinColumn = strJoinColumn;
                    Path.DFS();
                }
            }
            MessageBox.Show(bJoin ? "YES" : "NO");
        }

        private void LvAdjTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iAdjTableIndex = AdjTablesIndex();
            if (iAdjTableIndex == -1)
                return;
            string strSchema = lvAdjTables.Items[iAdjTableIndex].SubItems[0].Text;
            string strTable = lvAdjTables.Items[iAdjTableIndex].SubItems[1].Text;
            DBTableKey TK = new DBTableKey(strSchema, strTable, string.Empty);

            int iRow = 0;
            foreach (KeyValuePair<DBTableKey, DBTable> KVP in m_TableMap)
            {
                if (KVP.Key != TK)
                    iRow += KVP.Value.Columns.Count;
                else
                {
                    string strColumn = lvAdjTables.Items[iAdjTableIndex].SubItems[2].Text;
                    int iColumn = 0;
                    foreach (DBColumn Column in KVP.Value.Columns)
                    {
                        if (Column.Name == strColumn)
                        {
                            iRow += iColumn;
                            lvJoinTables.SelectedIndices.Add(iRow);
                            lvJoinTables.EnsureVisible(iRow);
                            break;
                        }
                        iColumn++;
                    }
                }
            }
        }

        #region THREADING
        // Background Worker Thread area
        private void InitializeBackgroundSQL()
        {
            m_BkgSQL = new BackgroundWorker();
            m_BkgSQL.DoWork += new DoWorkEventHandler(BkgSQL_DoWork);
            m_BkgSQL.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BkgSQL_RunWorkerCompleted);
            m_BkgSQL.ProgressChanged += new ProgressChangedEventHandler(BkgSQL_ProgressChanged);
            m_BkgSQL.WorkerReportsProgress = true;
            m_BkgSQL.WorkerSupportsCancellation = true;
        }

        private bool StartBackgroundSQL(string strConnectionString, string strQueryString, int nCount)
        {
            bool bRet = !m_BkgSQL.IsBusy;
            if (bRet)
            {
                // Start the asynchronous SQL query operation
                List<string> Parms = new List<string>
                {
                    strConnectionString,
                    strQueryString,
                    nCount.ToString()
                };
                m_BkgSQL.RunWorkerAsync(Parms);
            }
            return bRet;
        }
        private void CancelBackgroundSQL()
        {
            if (m_BkgSQL.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous SQL query operation
                m_BkgSQL.CancelAsync();
            }
        }

        private void BkgSQL_DoWork(object sender, DoWorkEventArgs e) 
        {
            e.Result = BackgroundQuery(sender, e);
        }

        private long BackgroundQuery(object sender, DoWorkEventArgs e)
        {
            long iResult = 0;
            
            BackgroundWorker SQLWorker = sender as BackgroundWorker;
            List<string> Parms = e.Argument as List<string>;

            string strConnectionString = Parms[0];
            string strQueryString = Parms[1];
            int nCount = Convert.ToInt32(Parms[2]);
            int nReportProgress = (int)((Double)nCount * 0.01) + 1;

            SQL Sql = new SQL(strConnectionString, strQueryString);
            if (Sql.ExecuteReader(out string strError))
            {
                if (SQLWorker.CancellationPending)
                    e.Cancel = true;

                if (!string.IsNullOrEmpty(strError))
                {
                    Sql.Close();
                    if (SQLWorker.CancellationPending)
                        e.Cancel = true;
                    return 0;
                }

                int nColumns = Sql.Columns.Count;
                m_Arr = new CArray(nCount, nColumns);
                m_Header = new string[nColumns];
                m_Width = new int[nColumns];
                m_SortOrder = new bool[nColumns];

                // Column headers
                Size szExtra = TextRenderer.MeasureText("XXXXX", m_Font);
                for (int iField = 0; iField < nColumns; ++iField)
                {
                    string strColHdr = Sql.Columns[iField];
                    m_Header[iField] = strColHdr;
                    m_SortOrder[iField] = false;
                    Size sz = szExtra + TextRenderer.MeasureText(new string('X', strColHdr.Length + 3), m_Font);
                    if (sz.Width > m_Width[iField])
                        m_Width[iField] = Math.Min(65535, sz.Width);
                }

                // Report the number of columns
                SQLWorker.ReportProgress(0);

                // Extra column width
                for (int iRow = 0; !e.Cancel && Sql.Read(); ++iRow)
                {
                    if (SQLWorker.CancellationPending)
                        e.Cancel = true;

                    for (int iField = 0; !e.Cancel && iField < nColumns; ++iField)
                    {
                        TypeCode TypeCode = Sql.ColumnType(iField);
                        object oField = Sql.GetValue(iField);
                        if (oField == null)
                        {
                            m_Arr[iField][iRow] = new CVariant(TypeCode);
                            continue;
                        }

                        switch (TypeCode)
                        {
                            case TypeCode.Boolean:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToBoolean(oField));
                                break;

                            case TypeCode.Char:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToChar(oField));
                                break;

                            case TypeCode.Byte:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToByte(oField));
                                break;

                            case TypeCode.SByte:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToSByte(oField));
                                break;

                            case TypeCode.Int16:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToInt16(oField));
                                break;

                            case TypeCode.UInt16:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToUInt16(oField));
                                break;

                            case TypeCode.Int32:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToInt32(oField));
                                break;

                            case TypeCode.UInt32:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToUInt32(oField));
                                break;

                            case TypeCode.Int64:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToInt64(oField));
                                break;

                            case TypeCode.UInt64:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToUInt64(oField));
                                break;

                            case TypeCode.Single:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToSingle(oField));
                                break;

                            case TypeCode.Double:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToDouble(oField));
                                break;

                            case TypeCode.Decimal:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToDecimal(oField));
                                break;

                            case TypeCode.DateTime:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToDateTime(oField));
                                break;

                            case TypeCode.String:
                                m_Arr[iField][iRow] = new CVariant(Convert.ToString(oField));
                                break;

                            default:
                                m_Arr[iField][iRow] = new CVariant(iRow);
                                break;
                        }

                        if (iRow < 1000) // TODO - Make this a constant
                        {
                            // IFormattable
                            Size sz = szExtra + TextRenderer.MeasureText(m_Arr[iField][iRow].ToString(), Font);
                            if (sz.Width > m_Width[iField])
                                m_Width[iField] = Math.Min(65535, sz.Width);
                        }

                        if (SQLWorker.CancellationPending)
                            e.Cancel = true;
                    }

                    // Report the progress
                    if ((iRow + 1) % nReportProgress == 0)
                        SQLWorker.ReportProgress(iRow + 1);
                }
            }
            Sql.Close();
            SQLWorker.ReportProgress(nCount);

            return iResult;
        }
        private void BkgSQL_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Reset the cursor
            Cursor.Current = Cursors.Default;
            pbData.Value = pbData.Minimum;

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // Canceled - try again
                ChangeTables();
            }
            else
            {
                // Operation succeeded
                lvTables.Select();

                Size szRowHeader = TextRenderer.MeasureText("XXXXXXXXXX", m_Font);
                dgvQuery.RowHeadersWidth = szRowHeader.Width;
                dgvQuery.Columns.Clear();

                for (int idx = 0, iField = 0; idx < m_TableKeys.Count; idx++)
                {
                    DBTableKey TK = m_TableKeys[idx];
                    DBTable Table = m_TableMap[TK];
                    foreach (DBColumn Column in Table.Columns)
                    {
                        dgvQuery.Columns.Add(Column.Name, Column.Name);
                        dgvQuery.Columns[iField].Width = m_Width[iField];
                        dgvQuery.Columns[iField].ReadOnly = true;
                        dgvQuery.Columns[iField].DefaultCellStyle = new DataGridViewCellStyle { Format = Column.FormatString };
                        iField++;
                    }
                }

                // Set the background color of the columns for the keys
                int iColStart = 0;
                foreach (DBTableKey TableKey in m_TableKeys)
                {
                    DBTable Table = m_TableMap[TableKey];
                    int nCols = m_TableMap[TableKey].Columns.Count;
                    for (int idx = 0; idx < nCols; ++idx)
                    {
                        int iCol = iColStart + idx;
                        DBColumn Column = Table.Columns[idx];
                        if (Column.IsPrimaryKey)
                        {
                            if (Table.ContainsFK(TableKey.Schema, TableKey.Table, Column.Name))
                            {
                                // The case where the the foreign key is in the primary table
                                dgvQuery.Columns[iCol].DefaultCellStyle.ForeColor = Color.DarkBlue;
                                dgvQuery.Columns[iCol].DefaultCellStyle.BackColor = Color.DarkBlue;
                            }
                            else
                            {
                                dgvQuery.Columns[iCol].DefaultCellStyle.ForeColor = Color.DarkBlue;
                                dgvQuery.Columns[iCol].DefaultCellStyle.BackColor = Color.Yellow;
                            }
                        }
                        else
                        {
                            DBTableKey TK = new DBTableKey(TableKey.Schema, TableKey.Table, Column.Name);
                            if (m_TableMap[TableKey].HasKey(TK))
                            {
                                dgvQuery.Columns[iCol].DefaultCellStyle.ForeColor = Color.DarkBlue;
                                dgvQuery.Columns[iCol].DefaultCellStyle.BackColor = Color.Yellow;
                            }
                        }
                    }
                    iColStart += nCols;
                }

                // Set the virtual list size
                dgvQuery.RowCount = m_nCount + 1;
                UpdateJoinTable();
            }
        }

        private void BkgSQL_ProgressChanged(object sender, ProgressChangedEventArgs e) 
        {
            // Set virtual list box count
            m_nCount = e.ProgressPercentage;

            if (m_nCount == 0)
            {
                // Prepare the progress bar
                pbData.Minimum = 1;
                pbData.Maximum = m_Arr.RowLength;
            }
            else
            {
                // Set the wait cursor, rowcount, and progress percentage
                pbData.Value = m_nCount;
                dgvQuery.RowCount = m_nCount + 1;
                Cursor.Current = Cursors.WaitCursor;
            }
        }
        #endregion // THREADING
    }
}