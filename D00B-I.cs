using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Globalization;

namespace D00B
{
    public partial class D00B : Form
    {
        BackgroundWorker m_BkgSQL;
        Dictionary<DBTableKey, DBTable> m_TableMap;
        private List<ListViewItem> m_AdjTables;
        int m_iLastTableIndex = -1;
        List<DBTableKey> m_TableKeys = new List<DBTableKey>();
        List<DBJoinKey> m_JoinKeysFr = new List<DBJoinKey>();
        List<DBJoinKey> m_JoinKeysTo = new List<DBJoinKey>();
        int m_nCT = 0; // Number of correlation tables

        CArray m_Arr;
        int[] m_Width;
        bool[] m_SortOrder;

        // Built during progress reporting
        List<DBColumn> m_ColumnHeaders;

        int m_nCount = -1;
        int m_nPreview = 100;
        Timer m_PBTimer = null;

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
        private int m_dx;

        public D00B()
        {
            InitializeComponent();
            InitializeBackgroundSQL();
        }

        private void D00B_Load(object sender, EventArgs e)
        {
            // Output the licensing disclaimer
            Debug.WriteLine(@"D00B-I.exe Copyright (C) 2022-Present Andrew S. Bantly");
            Debug.WriteLine(@"Andrew ""Andy"" S. Bantly can be reached at andybantly@hotmail.com");
            Debug.WriteLine(@"D00B-I.exe comes with ABSOLUTELY NO WARRANTY");
            Debug.WriteLine(@"This is free software, and you are welcome to redistribute it");
            Debug.WriteLine(@"under of the GNU General Public License as published by");
            Debug.WriteLine(@"the Free Software Foundation; version 2 of the License.");
            Debug.WriteLine(@"");
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

            dgvQuery.AllowUserToDeleteRows = false;
            dgvQuery.Font = Utility.m_Font;
            lvTables.View = View.Details;
            lvTables.Font = Utility.m_Font;
            lvColumns.View = View.Details;
            lvColumns.Font = Utility.m_Font;
            lvAdjTables.View = View.Details;
            lvAdjTables.Font = Utility.m_Font;
            lvResults.View = View.Details;
            lvResults.Font = Utility.m_Font;
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
            m_dx = lvColumns.Left - lvTables.Right;
            m_lvAdjTablesWidth = lvAdjTables.Width;

            // Resize initially for small artifacts of alignment issues
            OnSizing();
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
            int dX = PtDiff.X / 3;

            // Move and resize the list views
            lvTables.Width = m_lvTablesWidth + dX;
            lb1.Left = lvTables.Left;

            lvColumns.Left = lvTables.Right + m_dx;
            lvColumns.Width = m_lvColumnsWidth + dX;
            lb2.Left = lvColumns.Left;

            lvAdjTables.Left = lvColumns.Right + m_dx;
            lvAdjTables.Width = m_lvAdjTablesWidth + dX;
            lb3.Left = lvAdjTables.Left;
        }

        void UpdateUI(bool bEnabled)
        {
            txtConnString.Enabled = true;
            btnLoad.Enabled = true;
            chkPrevAll.Enabled = true;
            lvTables.Enabled = bEnabled;
            lvAdjTables.Enabled = bEnabled;
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
            btnJoin.Enabled = bEnabled && TableIndex() != -1 && ColumnIndex() != -1;
            btnResetJoin.Enabled = bEnabled && TableIndex() != -1 && ColumnIndex() != -1;
        }

        private int UpdateMaxWidth(string strField, int nCurrentWidth)
        {
            Size szWidth = TextRenderer.MeasureText(strField, Utility.m_Font);
            if (szWidth.Width > nCurrentWidth)
                nCurrentWidth = szWidth.Width;
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

                Utility.m_nMaxSchemaWidth = 0;
                Utility.m_nMaxTableWidth = 0;
                Utility.m_nMaxColumnWidth = 0;
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
                        int nRows = Convert.ToInt32(SqlTables.GetValue(2).ToString());
                        if (nRows == 0)
                        {
                            // Test ROW count
                            string strQueryStringCount = string.Format("select count(*) from [{0}].[{1}]", strSchema, strTable);
                            SQL SqlCount = new SQL(strConnectionString, strQueryStringCount);
                            int nCount = Convert.ToInt32(SqlCount.ExecuteScalar(out strError));
                            if (string.IsNullOrEmpty(strError))
                                nRows = nCount;
                            SqlCount.Close();
                        }

                        DBTableKey TableKey = new DBTableKey(strSchema, strTable, string.Empty);
                        if (!m_TableMap.ContainsKey(TableKey))
                        {
                            DBTable Table = new DBTable(TableKey)
                            {
                                SelectedIndex = iSelectedIndex,
                                Rows = nRows
                            };
                            m_TableMap.Add(TableKey, Table);
                            iSelectedIndex++;
                        }

                        // Measure the header text
                        Utility.m_nMaxSchemaWidth = UpdateMaxWidth(strSchema, Utility.m_nMaxSchemaWidth);
                        Utility.m_nMaxTableWidth = UpdateMaxWidth(strTable, Utility.m_nMaxTableWidth);
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
                        Utility.m_nMaxSchemaWidth = UpdateMaxWidth(strSchema, Utility.m_nMaxSchemaWidth);
                        Utility.m_nMaxTableWidth = UpdateMaxWidth(strTable, Utility.m_nMaxTableWidth);
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
                            Columns.Add(new DBColumn(strCol, Table.ContainsPK(TK)));
                        }
                        Table.Columns = Columns;
                        SqlCol.Close();
                    }
                }
            }
            catch { }
            finally
            {
                // Restore the cursor
                Cursor.Current = Cursors.Default;

                // Start the timer to reset the progress bar
                StartPBTimer();
            }
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

                // Collect information about the database
                CountTablesAndRows();

                // Setup the table headers
                Utility.SetupListViewHeaders(lvTables, true, true, false);

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
                dgvQuery.Rows.Clear();

                // Get the current table selection
                DBTableKey TableKey = m_TableKeys[0];
                string strSchema = TableKey.Schema;
                string strTable = TableKey.Table;
                string strColumn;
                string strOT = TableKey.JoinTag;

                // Update the current tables adjacent tables and find out where we can go
                UpdateAdjTables(strSchema, strTable);

                // Count the result tables data
                string strError = string.Empty;
                string strConnectionString = txtConnString.Text;
                string strQueryString = string.Format("select count(*) from [{0}].[{1}] {2}", strSchema, strTable, strOT);

                // Update ROW count
                SQL Sql = new SQL(strConnectionString, strQueryString);
                int nRows = Convert.ToInt32(Sql.ExecuteScalar(out strError));
                Sql.Close();
                m_TableMap[TableKey].Rows = nRows;
                bool bCount = false;

                if (m_TableKeys.Count > 1)
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
                            strSchema = lvTables.Items[TableIndex()].Text;
                            strTable = lvTables.Items[TableIndex()].SubItems[1].Text;
                            strColumn = lvColumns.Items[ColumnIndex()].Text;

                            string strSelect = "count(*)";
                            bool bIsNum = IsNumber(txtData.Text);
                            string strTest = bIsNum ? "=" : (chkExact.Checked ? "=" : "like");
                            string strTestVal = bIsNum ? txtData.Text : (chkExact.Checked ? string.Format("'{0}'", txtData.Text) : string.Format("'%{0}%'", txtData.Text));
                            strQueryString = string.Format("select {0} from [{1}].[{2}] where [{3}].[{4}].{5} {6} {7}", strSelect, strSchema, strTable, strSchema, strTable, strColumn, strTest, strTestVal);
                        }
                        else
                            MessageBox.Show("Please select a Table and Column to search within for the data", Text, MessageBoxButtons.OK);
                    }
                    else
                        MessageBox.Show("There is no text to search for in the Table and Column", Text, MessageBoxButtons.OK);
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
                    nCount = nRows;

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

                // If no search then build regular query
                if (string.IsNullOrEmpty(strQueryString))
                {
                    if (chkPrevAll.Checked)
                        strQueryString = string.Format("select * from [{0}].[{1}] {2}", strSchema, strTable, strOT);
                    else
                        strQueryString = string.Format("select top {0} * from [{1}].[{2}] {3}", nCount, strSchema, strTable, strOT);

                    // Join and preview the output
                    if (m_TableKeys.Count > 1)
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
        }

        private void UpdateAdjTables(string strSchema, string strTable)
        {
            // Track keys so that the loose/faux keys don't overlap
            List<DBTableKey> AdjTableKeys = new List<DBTableKey>();

            // Setup the adjacent table headers
            Utility.SetupListViewHeaders(lvAdjTables);

            m_AdjTables = new List<ListViewItem>();
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

                                TK = new DBTableKey(FK.Schema, FK.Table, string.Empty); // Essentially the parent function
                                if (m_TableMap.ContainsKey(TK))
                                {
                                    Table = m_TableMap[TK];
                                    if (Table.Rows == 0)
                                    {
                                        Item.BackColor = Color.Red;
                                        SubItem.BackColor = Color.Red;
                                    }

                                    // The case where the the foreign key is in the primary table
                                    SubItem2.ForeColor = Color.DarkBlue;
                                    SubItem2.BackColor = Color.Yellow;
                                }
                                else
                                {
                                    SubItem2.ForeColor = Color.DarkBlue;
                                    SubItem2.BackColor = Color.Yellow;
                                }

                                Item.SubItems.Add(SubItem);
                                Item.SubItems.Add(SubItem2);
                                lvAdjTables.Items.Add(Item); // DBTableKey FK
                                m_AdjTables.Add(Item);

                                // Track the table
                                AdjTableKeys.Add(FK);
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
                            if (Table.Rows == 0)
                            {
                                Item.BackColor = Color.Red;
                                SubItem.BackColor = Color.Red;
                            }
                            SubItem2.BackColor = Color.Yellow;
                            SubItem2.ForeColor = Color.DarkBlue;
                        }
                        Item.SubItems.Add(SubItem);
                        Item.SubItems.Add(SubItem2);
                        lvAdjTables.Items.Add(Item); // DBTableKey TK
                        m_AdjTables.Add(Item);

                        // Track the table
                        AdjTableKeys.Add(TK);
                    }
                }

                // BackKeys
                TK = new DBTableKey(strSchema, strTable, string.Empty);
                DBTable TableL = m_TableMap[TK];
                foreach (KeyValuePair<DBTableKey, DBTable> KVP2 in m_TableMap)
                {
                    if (KVP2.Key == TK)
                        continue;
                    DBTable TableR = KVP2.Value;
                    foreach (DBTableKey TK1 in TableL.Keys)
                    {
                        if (TableR.ContainsFK(TK1))
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
                                        if (Table.Rows == 0)
                                        {
                                            Item.BackColor = Color.Red;
                                            SubItem.BackColor = Color.Red;
                                        }
                                        SubItem2.BackColor = Color.DarkGreen;
                                    }
                                    else
                                        SubItem2.BackColor = Color.DarkGreen;
                                    SubItem2.ForeColor = Color.Yellow;

                                    Item.SubItems.Add(SubItem);
                                    Item.SubItems.Add(SubItem2);
                                    lvAdjTables.Items.Add(Item); // DBTableKey TK2
                                    m_AdjTables.Add(Item);

                                    // Track the table
                                    AdjTableKeys.Add(TK2);
                                }
                            }
                        }
                    }
                }

                // Faux/Loose Keys - column name the same
                TK = new DBTableKey(strSchema, strTable, string.Empty);
                TableL = m_TableMap[TK];
                foreach (KeyValuePair<DBTableKey, DBTable> KVP2 in m_TableMap)
                {
                    if (KVP2.Key == TK)
                        continue;
                    DBTable TableR = KVP2.Value;
                    foreach (DBColumn LC in TableL.Columns)
                    {
                        foreach (DBColumn RC in TableR.Columns)
                        {
                            if (string.Compare(LC.Name, RC.Name, true) == 0)
                            {
                                // If it was already added as a strong key, don't add it twice
                                DBTableKey LK = new DBTableKey(TableR.TableSchema, TableR.TableName, RC.Name);
                                if (AdjTableKeys.Contains(LK))
                                    continue;

                                ListViewItem Item = new ListViewItem(TableR.TableSchema);
                                Item.UseItemStyleForSubItems = false;
                                ListViewItem.ListViewSubItem SubItem = new ListViewItem.ListViewSubItem(Item, TableR.TableName);
                                ListViewItem.ListViewSubItem SubItem2 = new ListViewItem.ListViewSubItem(Item, RC.Name);
                                
                                if (Table.HasKey(LK))
                                {
                                    if (Table.Rows == 0)
                                    {
                                        Item.BackColor = Color.Red;
                                        SubItem.BackColor = Color.Red;
                                    }
                                    else
                                    {
                                        SubItem2.ForeColor = Color.DarkGreen;
                                        SubItem2.BackColor = Color.DarkGray;
                                    }
                                }
                                else
                                {
                                    SubItem2.ForeColor = Color.DarkGreen;
                                    SubItem2.BackColor = Color.DarkGray;
                                }

                                Item.SubItems.Add(SubItem);
                                Item.SubItems.Add(SubItem2);
                                lvAdjTables.Items.Add(Item);
                                m_AdjTables.Add(Item);
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
            // Set the busy cursor
            Cursor.Current = Cursors.WaitCursor;
            bool bReturn = true;

            try
            {
                ClearUI();
                ClearData();
                RefreshIndex();
                SetupSearchResults();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                bReturn = false;
            }
            finally
            {
                // Set the default cursor
                Cursor.Current = Cursors.Default;
            }
            return bReturn;
        }

        void ClearData()
        {
            // Clear storages used for UI
            m_Arr = null;
            m_ColumnHeaders = null;
            m_Width = null;
            m_SortOrder = null;
        }

        void ClearUI()
        {
            // Clear UI
            lvTables.Clear();
            lvColumns.Clear();
            lvAdjTables.Clear();
            lvResults.Clear();
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
            List<string> Header = new List<string>();
            for (int idx = 0; idx < m_TableKeys.Count; idx++)
            {
                DBTableKey TK = m_TableKeys[idx];
                DBTable Table = m_TableMap[TK];
                foreach (DBColumn Column in Table.Columns)
                    Header.Add(Column.Name);
            }

            if (dgvQuery.RowCount > 0 && ExportListView.ExportToExcel(m_Arr, m_ColumnHeaders, "SQL", out double dDuration))
                MessageBox.Show(string.Format("Successfully exported to Excel in {0} seconds", dDuration), Text);
            else
                MessageBox.Show("Failed to export to Excel", Text);
        }

        private void ChkPrevAll_CheckedChanged(object sender, EventArgs e)
        {
            lblPreview.Enabled = !chkPrevAll.Checked;
            txtPreview.Enabled = !chkPrevAll.Checked;

            // Reload the view
            UpdateUI(LoadView());
        }

        private void SetupSearchResults()
        {
            // Setup the search results headers
            Utility.SetupListViewHeaders(lvResults);
        }
        private void FillSearchResults()
        {
            foreach (KeyValuePair<DBTableKey, DBTable> KVP in m_TableMap)
            {
                string strOwn = KVP.Key.Schema;
                string strTable = KVP.Key.Table;
                bool bRows = KVP.Value.Rows != 0;

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

            if (m_BkgSQL.IsBusy)
            {
                MessageBox.Show(string.Format("{0} is busy, please wait until the query is completed.", Text), Text);
                return;
            }

            // Sort using the classes comparer
            Global.SortOrder = m_SortOrder[e.ColumnIndex];

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
                        e.Value = m_Arr[iCol][iRow]?.ToString(m_ColumnHeaders[iCol].Alignment, m_ColumnHeaders[iCol].FormatString, m_ColumnHeaders[iCol].FormatProvider);
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
                if (Table.View)
                {
                    e.Item.BackColor = Color.Orange;
                    lvSubValue.BackColor = Color.Orange;
                }
                else if (Table.Rows == 0)
                {
                    e.Item.BackColor = Color.Red;
                    lvSubValue.BackColor = Color.Red;
                }
                e.Item.SubItems.Add(lvSubValue);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0} : {1}", e.ItemIndex, ex.Message));
            }
            finally { }
        }
        private void ToggleColumn(int iColumn)
        {
            DBColumn Column = m_ColumnHeaders[iColumn];
            Column.Include = !Column.Include;

            // Trigger the new column visibility
            SetupHeaders();
            FinishBackgroundSQL();
        }
        private void LvColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI(true);
        }
        private void LvColumns_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Location.X > 20)
                return;
            ListViewItem Item = lvColumns.GetItemAt(e.Location.X, e.Location.Y);
            int iColumn = Item.Index;
            lvColumns.Select();
            lvColumns.EnsureVisible(iColumn);
            lvColumns.SelectedIndices.Add(iColumn);
            lvColumns.SelectedIndices.Add(iColumn);
            ToggleColumn(iColumn);
        }

        private void LvColumns_KeyPress(object sender, KeyPressEventArgs e)
        {
            int iColumn = ColumnIndex();
            if (iColumn < 0)
                return;
            ToggleColumn(iColumn);
        }

        private void LvColumns_EditFormat(object sender, MouseEventArgs e)
        {
            int iItemIndex = ColumnIndex();
            if (iItemIndex < 0)
                return;

            // Lookup the column based on the column index
            DBColumn Column = m_ColumnHeaders[iItemIndex];

            string strCultureName = Column.CultureName;
            string strFormatString = Column.FormatString;
            int iAlignment = Column.Alignment;

            // Edit the format
            FormatBuilder FmtDlg = new FormatBuilder(Column.Name, Column.TypeCode, strFormatString, iAlignment, strCultureName);
            DialogResult Res = FmtDlg.ShowDialog();
            if (Res == DialogResult.OK)
            {
                if (strFormatString != FmtDlg.FormatString || iAlignment != FmtDlg.Alignment || strCultureName != FmtDlg.CultureName)
                {
                    // Update the format string
                    Column.FormatString = FmtDlg.FormatString;
                    Column.Alignment = FmtDlg.Alignment;
                    Column.CultureName = FmtDlg.CultureName;
                    m_ColumnHeaders[iItemIndex] = Column;

                    // Trigger the new formatting
                    SetupHeaders();
                    FinishBackgroundSQL();
                }
            }
        }

        private void LvColumns_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            int iItemIndex = e.ItemIndex;
            try
            {
                if (m_Arr != null)
                {
                    // Lookup the column and table based on the item index
                    DBColumn Column = m_ColumnHeaders[iItemIndex];

                    DBTableKey TableKey = null;
                    DBTable Table = null;
                    for (int iTable = 0, idx = iItemIndex; iTable < m_TableKeys.Count; ++iTable)
                    {
                        TableKey = m_TableKeys[iTable];
                        Table = m_TableMap[TableKey];
                        if (Table.Columns.Count - 1 < idx)
                            idx -= Table.Columns.Count;
                        else
                            break;
                    }

                    e.Item = new ListViewItem(Column.Name)
                    {
                        Checked = Column.Include,
                        UseItemStyleForSubItems = false
                    };
                    e.Item.SubItems.Add(Column.FormatString);
                    e.Item.SubItems.Add(Column.Alignment.ToString());
                    e.Item.SubItems.Add(Column.CultureName);

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
            int iTableIndex = TableIndex();
            if (iTableIndex == -1)
                return;

            if (m_TableKeys.Count > 1)
            {
                ParseKey(TableCurSel(iTableIndex), out string strSchema, out string strTable);
                DBTableKey TK = new DBTableKey(strSchema, strTable, string.Empty);
                if (m_TableKeys.Contains(TK))
                {
                    // Select the table that is in the join list
                    m_iLastTableIndex = iTableIndex;
                    SelectIndex();
                    return;
                }
                else
                {
                    if (MessageBox.Show("The current join(s) will be lost when switching to a table outside of the join tables.  Is this OK?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        lvTables.Select();
                        lvTables.EnsureVisible(m_iLastTableIndex);
                        lvTables.SelectedIndices.Add(m_iLastTableIndex);
                        return;
                    }
                }
            }
            m_iLastTableIndex = iTableIndex;
            ChangeTables();
            UpdateUI(true);
        }

        private void ChangeTables()
        {
            int iTableIndex = TableIndex();
            if (iTableIndex == -1)
                return;

            // Stop the virtual list from asking for cell information
            dgvQuery.Rows.Clear();
            CancelBackgroundSQL();

            // Start a new list and add the selected table and prepare for joins
            m_nCT = 0;
            m_TableKeys = new List<DBTableKey>();
            m_JoinKeysFr = new List<DBJoinKey>();
            m_JoinKeysTo = new List<DBJoinKey>();

            // Setup the first key in case this isn't a join
            ParseKey(TableCurSel(iTableIndex), out string strSchema, out string strTable);
            DBTableKey TableKey = new DBTableKey(strSchema, strTable, string.Empty)
            {
                JoinTag = string.Format("T{0}", ++m_nCT)
            };
            m_TableKeys.Add(TableKey);

            // Setup the column and format list
            lvColumns.CheckBoxes = true;
            lvColumns.VirtualListSize = 0;
            Utility.m_nMaxColumnWidth = 0;
            DBTable Table = m_TableMap[TableKey];
            foreach (DBColumn Column in Table.Columns)
                Utility.m_nMaxColumnWidth = UpdateMaxWidth(Column.Name, Utility.m_nMaxColumnWidth);

            // Setup the column headers
            Utility.SetupListViewHeaders(lvColumns, false, false, true, true);

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

                    DlgJoin dlgJoin = new DlgJoin(m_TableMap, m_AdjTables)
                    {
                        SourceSchema = strSrcSchema,
                        SourceTable = strSrcTable,
                        SourceColumn = strSrcColumn
                    };

                    DialogResult res = dlgJoin.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        List<DBJoinKey> JoinKeys = dlgJoin.JoinKeys;
                        foreach (DBJoinKey JoinKey in JoinKeys)
                        {
                            string strJoinSchema = JoinKey.Schema;
                            string strJoinTable = JoinKey.Table;
                            string strJoinColumn = JoinKey.Column;

                            DBJoinKey FKJoin, TKJoin;
                            FKJoin = new DBJoinKey(strSrcSchema, strSrcTable, strSrcColumn, JoinKey.JoinType);
                            FKJoin.JoinTag = string.Format("T{0}", m_nCT++);
                            m_JoinKeysFr.Add(FKJoin);
                            if (JoinKey.JoinType != Utility.Join.Self)
                                TKJoin = new DBJoinKey(strJoinSchema, strJoinTable, strJoinColumn, JoinKey.JoinType);
                            else
                                TKJoin = new DBJoinKey(strSrcSchema, strSrcTable, strSrcColumn, JoinKey.JoinType);
                            TKJoin.JoinTag = string.Format("T{0}", m_nCT);
                            m_JoinKeysTo.Add(TKJoin);

                            // Add the join table to the list of tables
                            if (JoinKey.JoinType != Utility.Join.Self)
                                m_TableKeys.Add(new DBTableKey(strJoinSchema, strJoinTable, string.Empty));
                            else
                                m_TableKeys.Add(new DBTableKey(strSrcSchema, strSrcTable, string.Empty));
                        }

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
            ChangeTables();
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

        // Start the asynchronous background worker thread
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

        // Finish up and shpw the results
        private void FinishBackgroundSQL()
        {
            using (dgvQuery.SuspendDrawing())
            {
                // Operation succeeded
                lvTables.Select();

                // Set the virtual list size
                dgvQuery.RowCount = m_nCount + 1;
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
            int nReportProgress = (int)((Double)nCount * 0.10) + 1;

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
                m_Width = new int[nColumns];
                m_SortOrder = new bool[nColumns];

                // Column headers
                m_ColumnHeaders = new List<DBColumn>();
                Size szExtra = TextRenderer.MeasureText("XXXXXXXX", Utility.m_Font);
                for (int iField = 0; iField < nColumns; ++iField)
                {
                    string strColHdr = Sql.Columns[iField];
                    DBColumn ColumnHeader = new DBColumn(strColHdr);
                    m_SortOrder[iField] = false;
                    Size sz = szExtra + TextRenderer.MeasureText(new string('X', strColHdr.Length + 3), Utility.m_Font);
                    if (sz.Width > m_Width[iField])
                        m_Width[iField] = Math.Min(65535, sz.Width);
                    ColumnHeader.Include = true;
                    ColumnHeader.TypeCode = Sql.ColumnType(iField);
                    m_ColumnHeaders.Add(ColumnHeader);
                }

                // Report the number of columns
                SQLWorker.ReportProgress(0);

                // Extra column width
                for (int iRow = 0; !e.Cancel && Sql.Read() && iRow < nCount; ++iRow)
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

            // Set the progress bar to 100% and reset with a timer.
            StartPBTimer();

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
                // Finish and show the results
                FinishBackgroundSQL();
            }
        }

        // Start the timer that keeps the progress bar at 100% for a moment
        private void StartPBTimer()
        {
            pbData.Value = pbData.Maximum;
            if (m_PBTimer == null)
                m_PBTimer = new Timer();
            m_PBTimer.Stop();
            m_PBTimer.Tick += new EventHandler(TimerProcessor);
            m_PBTimer.Interval = 500;
            m_PBTimer.Start();
        }
        private void TimerProcessor(Object myObject, EventArgs myEventArgs)
        {
            pbData.Value = pbData.Minimum;
            m_PBTimer.Stop();
        }

        // Setup the column headers of the data grid view query results
        private void SetupHeaders()
        {
            // Columns
            int nTotColumns = 0;
            for (int iTable = 0; iTable < m_TableKeys.Count; ++iTable)
                nTotColumns += m_TableMap[m_TableKeys[iTable]].Columns.Count;
            lvColumns.VirtualListSize = nTotColumns;

            using (dgvQuery.SuspendDrawing())
            {
                Size szRowHeader = TextRenderer.MeasureText("XXXXXXXXXX", Utility.m_Font);
                dgvQuery.RowHeadersWidth = szRowHeader.Width;
                dgvQuery.Columns.Clear();

                for (int idx = 0, iField = 0; idx < m_TableKeys.Count; idx++)
                {
                    DBTableKey TK = m_TableKeys[idx];
                    DBTable Table = m_TableMap[TK];
                    for (int iCol = 0; iCol < Table.Columns.Count; ++iCol, ++iField)
                    {
                        if (!m_BkgSQL.CancellationPending)
                        {
                            DBColumn Column = m_ColumnHeaders[iField];
                            dgvQuery.Columns.Add(Column.Name, Column.Name);
                            dgvQuery.Columns[iField].Width = m_Width[iField];
                            dgvQuery.Columns[iField].ReadOnly = true;
                            dgvQuery.Columns[iField].DefaultCellStyle = new DataGridViewCellStyle { Alignment = Column.TypeCode != TypeCode.String ? DataGridViewContentAlignment.MiddleRight : DataGridViewContentAlignment.MiddleLeft };
                            dgvQuery.Columns[iField].Visible = Column.Include;
                        }
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
            }
        }

        private void BkgSQL_ProgressChanged(object sender, ProgressChangedEventArgs e) 
        {
            // Busy with background thread
            Cursor.Current = Cursors.WaitCursor;

            // Set virtual list box count
            m_nCount = e.ProgressPercentage;

            if (m_nCount == 0)
            {
                // Prepare the progress bar
                pbData.Minimum = 1;
                pbData.Maximum = m_Arr.RowLength;
                pbData.Value = pbData.Minimum;

                // Setup the column headers // move column widths, prevent from null
                SetupHeaders();
            }
            else
            {
                // Update the progress bar
                pbData.Value = m_nCount;

                if (dgvQuery.Columns.Count > 0)
                {
                    using (dgvQuery.SuspendDrawing())
                    {
                        // Update column widths
                        for (int iKey = 0, iField = 0; iKey < m_TableKeys.Count && !m_BkgSQL.CancellationPending; ++iKey)
                        {
                            int nColumns = m_TableMap[m_TableKeys[iKey]].Columns.Count;
                            for (int iCol = 0; iCol < nColumns && !m_BkgSQL.CancellationPending; ++iCol, ++iField)
                            {
                                if (m_Width[iField] > dgvQuery.Columns[iField].Width)
                                    dgvQuery.Columns[iField].Width = m_Width[iField];
                            }
                        }
                    }
                }

                dgvQuery.RowCount = m_nCount + 1;
            }
        }
        #endregion // THREADING
    }
}