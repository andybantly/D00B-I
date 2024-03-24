using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;
using SortOrder = System.Windows.Forms.SortOrder;
using System.Diagnostics;

namespace D00B
{
    public partial class D00B : Form
    {
        Dictionary<DBTableKey, DBTable> g_TableMap;
        List<DBTableKey> m_TableKeys = new List<DBTableKey>();
        List<DBJoinKey> m_JoinKeysFr = new List<DBJoinKey>();
        List<DBJoinKey> m_JoinKeysTo = new List<DBJoinKey>();
        int m_nCT = 0; // Number of correlation tables

        float g_nFontHeight = 0;

        CArray m_Arr;
        int[] m_oWidth;
        bool[] m_oSortOrder;

        int m_nColumns = -1;
        int m_nCount = -1;
        int m_nPreview = 100;
        public D00B()
        {
            InitializeComponent();
        }
        void UpdateUI(bool bEnabled)
        {
            lvTables.Enabled = bEnabled;
            lvAdjTables.Enabled = bEnabled;
            lvJoinTables.Enabled = bEnabled;
            txtConnString.Enabled = true;
            lvQuery.Enabled = bEnabled;
            btnRefresh.Enabled = true;
            txtPreview.Enabled = bEnabled;
            lblPreview.Enabled = bEnabled;
            btnExport.Enabled = bEnabled;
            chkHdr.Enabled = bEnabled;
            chkPrevAll.Enabled = bEnabled;
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

            g_nFontHeight = lvQuery.Font.Size;
            lvQuery.View = View.Details;
            lvQuery.Sorting = SortOrder.None;
            lvQuery.Font = Utility.MakeFont(g_nFontHeight, FontFamily.GenericMonospace, FontStyle.Bold);
            lvTables.View = View.Details;
            lvTables.Font = Utility.MakeFont(g_nFontHeight, FontFamily.GenericMonospace, FontStyle.Bold);
            lvColumns.View = View.Details;
            lvColumns.Font = Utility.MakeFont(g_nFontHeight, FontFamily.GenericMonospace, FontStyle.Bold);
            lvAdjTables.View = View.Details;
            lvAdjTables.Font = Utility.MakeFont(g_nFontHeight, FontFamily.GenericMonospace, FontStyle.Bold);
            lvJoinTables.View = View.Details;
            lvJoinTables.Font = Utility.MakeFont(g_nFontHeight, FontFamily.GenericMonospace, FontStyle.Bold);
            lvResults.View = View.Details;
            lvResults.Font = Utility.MakeFont(g_nFontHeight, FontFamily.GenericMonospace, FontStyle.Bold);
        }
        private void CountTablesAndRows()
        {
            int nData;
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

                int iSelectedIndex = 0;
                g_TableMap = new Dictionary<DBTableKey, DBTable>();
                if (SqlTables.ExecuteReader(out string strError))
                {
                    if (!string.IsNullOrEmpty(strError))
                        throw new Exception(strError);
                    while (SqlTables.Read())
                    {
                        string strSchema = SqlTables.GetValue(0).ToString();
                        string strTable = SqlTables.GetValue(1).ToString();
                        string strRows = SqlTables.GetValue(2).ToString();
                        string strColumn = string.Empty;

                        DBTableKey TableKey = new DBTableKey(strSchema, strTable, strColumn);
                        if (!g_TableMap.ContainsKey(TableKey))
                        {
                            DBTable Table = new DBTable(TableKey)
                            {
                                SelectedIndex = iSelectedIndex,
                                Rows = strRows
                            };
                            g_TableMap.Add(TableKey, Table);
                            iSelectedIndex++;
                        }
                    }
                }
                else
                    MessageBox.Show(strError);
                SqlTables.Close();

                foreach (KeyValuePair<DBTableKey, DBTable> kvp in g_TableMap)
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
                pbData.Maximum = g_TableMap.Count;
                nData = 0;
                foreach (KeyValuePair<DBTableKey, DBTable> KVP in g_TableMap)
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
            lvJoinTables.Columns.Add("Schema");
            lvJoinTables.Columns[0].Width = TextRenderer.MeasureText("XXXXXXXXXX", lvJoinTables.Font).Width;
            lvJoinTables.Columns.Add("Table");
            lvJoinTables.Columns[1].Width = TextRenderer.MeasureText("XXXXXXXXXXXXXXXXXXXXX", lvJoinTables.Font).Width;
            lvJoinTables.Columns.Add("Column");
            lvJoinTables.Columns[2].Width = TextRenderer.MeasureText("XXXXXXXXXXXXXXXXXXXXX", lvJoinTables.Font).Width;
        }

        private void UpdateJoinTable()
        {
            int nTotalColumns = 0;
            foreach (KeyValuePair<DBTableKey, DBTable> KVP in g_TableMap) { nTotalColumns += KVP.Value.Columns.Count; }
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
                lvQuery.VirtualListSize = 0;
                lvTables.VirtualListSize = 0;
                lvJoinTables.VirtualListSize = 0;
                lvColumns.VirtualListSize = 0;

                // Collect information about the database
                CountTablesAndRows();

                lvTables.Columns.Add("Schema");
                lvTables.Columns[0].Width = TextRenderer.MeasureText("XXXXXXXXXX", lvTables.Font).Width;
                lvTables.Columns.Add("Table");
                lvTables.Columns[1].Width = TextRenderer.MeasureText("XXXXXXXXXXXXXXXXXXXXX", lvTables.Font).Width;

                lvColumns.Columns.Add("Columns");
                lvColumns.Columns[0].Width = TextRenderer.MeasureText("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", lvTables.Font).Width;

                // Enable/Disable
                tbTables.Text = string.Format("{0}", g_TableMap.Count);
                lvTables.Enabled = g_TableMap.Count > 0;

                // Select the first column to activate the selectindex trigger that populates the columns and adjacent table list
                if (lvTables.Enabled)
                {
                    // Update the virtual sizes
                    lvTables.VirtualListSize = g_TableMap.Count;

                    // Select the first item
                    if (lvTables.VirtualListSize > 0)
                        lvTables.SelectedIndices.Add(0);
                }

                UpdateUI(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { }
        }

        private void SelectIndex()
        {
            Cursor.Current = Cursors.WaitCursor;

            // Flicker free drawing
            using (lvAdjTables.SuspendDrawing())
            {
                using (lvResults.SuspendDrawing())
                {
                    using (lvTables.SuspendDrawing())
                    {
                        using (lvColumns.SuspendDrawing())
                        {
                            using (lvQuery.SuspendDrawing())
                            {
                                // Do the work
                                UpdateIndex();
                            }
                        }
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }
        private void UpdateIndex()
        {
            if (g_TableMap.Count > 0 && m_TableKeys.Count > 0)
            {
                // Clear the output
                lvQuery.Clear();

                // Get the current table selection
                int iSelectedIndex = TableIndex();
                if (iSelectedIndex == -1)
                    return;

                try
                {
                    // Get the current table selection
                    DBTableKey TableKey = m_TableKeys[0];
                    string strSchema = TableKey.Schema;
                    string strTable = TableKey.Table;
                    string strColumn = string.Empty;
                    string strOT = TableKey.JoinTag;

                    // Colums
                    DBTable Table = g_TableMap[TableKey];
                    lvColumns.VirtualListSize = Table.Columns.Count;
                    lvColumns.SelectedIndices.Clear();

                    // Update the current tables adjacent tables and find out where we can go
                    UpdateAdjTables(strSchema, strTable);

                    // Count the result tables data
                    string strError = string.Empty;
                    string connectionString = txtConnString.Text;
                    string strQueryString = string.Format("select count(*) from [{0}].[{1}] {2}", strSchema, strTable, strOT);

                    // Update ROW count
                    SQL Sql = new SQL(connectionString, strQueryString);
                    string strRows = Convert.ToString(Sql.ExecuteScalar(out strError));
                    Sql.Close();
                    g_TableMap[TableKey].Rows = strRows;
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

                    // Get the virtual list size
                    int nCount;
                    if (bCount)
                    {
                        Sql = new SQL(connectionString, strQueryString);
                        nCount = Convert.ToInt32(Sql.ExecuteScalar(out strError));
                        Sql.Close();
                    }
                    else
                        nCount = Convert.ToInt32(strRows);

                    // Set the final count of rows in the view
                    m_nCount = chkPrevAll.Checked ? nCount : Math.Min(nCount, m_nPreview);

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
                                string strSelect = chkPrevAll.Checked ? "*" : string.Format("top {0} *", m_nCount);
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
                            strQueryString = string.Format("select top {0} * from [{1}].[{2}] {3}", m_nCount, strSchema, strTable, strOT);

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

                    // Perform the search
                    Sql = new SQL(connectionString, strQueryString);
                    if (Sql.ExecuteReader(out strError))
                    {
                        if (!string.IsNullOrEmpty(strError))
                            throw new Exception(strError);
                        txtQuery.Text = strQueryString;

                        // should be the same as the sum of all columns in the collective table list
                        m_nColumns = Sql.Columns.Count;

                        // Set up the backing for the virtual list view
                        m_Arr = new CArray(m_nCount, m_nColumns);
                        m_oWidth = new int[m_nColumns];
                        m_oSortOrder = new bool[m_nColumns];

                        // Column headers
                        int iField = 0;
                        foreach (string strColHdr in Sql.Columns)
                        {
                            lvQuery.Columns.Add(strColHdr);
                            m_oSortOrder[iField] = false;
                            Size sz = TextRenderer.MeasureText(new string('X', strColHdr.Length + 3), lvQuery.Font);
                            if (sz.Width > m_oWidth[iField])
                                m_oWidth[iField] = sz.Width;
                            iField++;
                        }

                        // Extra column width
                        Size szExtra = TextRenderer.MeasureText("XXXXX", lvQuery.Font);
                        for (int iRow = 0; Sql.Read(); ++iRow)
                        {
                            for (iField = 0; iField < m_nColumns; ++iField)
                            {
                                object oField = Sql.GetValue(iField);
                                if (oField == null)
                                    Debug.WriteLine("null");
                                TypeCode TypeCode = Sql.ColumnTypes[iField];
                                switch (TypeCode)
                                {
                                    case TypeCode.Boolean:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToBoolean(oField), iRow);
                                        break;

                                    case TypeCode.Char:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToChar(oField), iRow);
                                        break;

                                    case TypeCode.Byte:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToByte(oField), iRow);
                                        break;

                                    case TypeCode.SByte:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToSByte(oField), iRow);
                                        break;

                                    case TypeCode.Int16:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToInt16(oField), iRow);
                                        break;

                                    case TypeCode.UInt16:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToUInt16(oField), iRow);
                                        break;

                                    case TypeCode.Int32:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToInt32(oField), iRow);
                                        break;

                                    case TypeCode.UInt32:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToUInt32(oField), iRow);
                                        break;

                                    case TypeCode.Int64:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToInt64(oField), iRow);
                                        break;

                                    case TypeCode.UInt64:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToUInt64(oField), iRow);
                                        break;

                                    case TypeCode.Single:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToSingle(oField), iRow);
                                        break;

                                    case TypeCode.Double:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToDouble(oField), iRow);
                                        break;

                                    case TypeCode.Decimal:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToDecimal(oField), iRow);
                                        break;

                                    case TypeCode.DateTime:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToDateTime(oField), iRow);
                                        break;

                                    case TypeCode.String:
                                        m_Arr[iField][iRow] = new CVariant(Convert.ToString(oField), iRow);
                                        break;

                                    default:
                                        m_Arr[iField][iRow] = new CVariant(iRow);
                                        break;
                                }

                                if (iRow < 1000) // TODO - Make this a constant
                                {
                                    Size sz = szExtra + TextRenderer.MeasureText(m_Arr[iField][iRow].CellValue, lvQuery.Font);
                                    if (sz.Width > m_oWidth[iField])
                                        m_oWidth[iField] = sz.Width;
                                }
                            }
                        }

                        // Set the widths
                        for (iField = 0; iField < m_nColumns; ++iField)
                            lvQuery.Columns[iField].Width = m_oWidth[iField];
                    }
                    else
                        MessageBox.Show(strError);
                    Sql.Close();

                    if (!string.IsNullOrEmpty(strError))
                        return;

                    // Bring focus to the tables
                    lvTables.Select();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    // Set the virtual list size
                    lvQuery.VirtualListSize = m_nCount;
                    UpdateJoinTable();
                }
            }
        }
        private void UpdateAdjTables(string strSchema, string strTable)
        {
            lvAdjTables.Clear();
            lvAdjTables.SelectedIndices.Clear();
            lvAdjTables.Columns.Add("Schema");
            lvAdjTables.Columns[0].Width = TextRenderer.MeasureText("XXXXXXXXXX", lvAdjTables.Font).Width;
            lvAdjTables.Columns.Add("Table");
            lvAdjTables.Columns[1].Width = TextRenderer.MeasureText("XXXXXXXXXX", lvAdjTables.Font).Width;
            lvAdjTables.Columns.Add("Columns");
            lvAdjTables.Columns[2].Width = TextRenderer.MeasureText("XXXXXXXXXXXXXXXXXXXXXXXXXX", lvAdjTables.Font).Width;

            DBTableKey TK = new DBTableKey(strSchema, strTable, string.Empty);
            bool bInclude = g_TableMap.ContainsKey(TK);
            if (bInclude)
            {
                DBTable Table = g_TableMap[TK];
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
                                if (g_TableMap.ContainsKey(TK))
                                {
                                    Table = g_TableMap[TK];
                                    if (Table.Rows == "0")
                                        Item.BackColor = Color.Red;

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
                                lvAdjTables.Items.Add(Item);
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

                            Item.SubItems.Add(SubItem);
                            Item.SubItems.Add(SubItem2);
                            lvAdjTables.Items.Add(Item);
                        }
                    }
                }

                // BackKeys
                DBTableKey TableKey = new DBTableKey(strSchema, strTable, string.Empty);
                if (g_TableMap.ContainsKey(TableKey))
                {
                    DBTable TableL = g_TableMap[TableKey];
                    foreach (KeyValuePair<DBTableKey, DBTable> KVP2 in g_TableMap)
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
                                        TK = new DBTableKey(TK2.Schema, TK2.Table, TK2.Column);
                                        if (Table.HasKey(TK))
                                        {
                                            if (Table.Rows == "0")
                                            {
                                                Item.BackColor = Color.Red;
                                                SubItem.BackColor = Color.Red;
                                                SubItem2.BackColor = Color.Red;
                                            }
                                        }
                                        SubItem2.ForeColor = Color.Yellow;
                                        SubItem2.BackColor = Color.DarkGreen;
                                        Item.SubItems.Add(SubItem);
                                        Item.SubItems.Add(SubItem2);
                                        lvAdjTables.Items.Add(Item);
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
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            UpdateUI(false);
            RefreshView();
        }

        private void RefreshView()
        {
            ClearUI();
            ClearData();
            RefreshIndex();
            SetupJoinTables();
            SetupSearchResults();
        }

        void ClearData()
        {
            // Clear storages used for UI
            m_Arr = null;
            m_oWidth = null;
            m_oSortOrder = null;
        }

        void ClearUI()
        {
            // Clear UI
            lvTables.Clear();
            lvColumns.Clear();
            lvAdjTables.Clear();
            lvJoinTables.Clear();
            lvResults.Clear();
            lvQuery.Clear();

            // Get the schemas
            string strSchema = cbSchema.Text;
            cbSchema.Items.Clear();
            cbSchema.Items.Add("All");
            string connectionString = txtConnString.Text;
            string queryString = string.Format("select name from sys.schemas");
            SQL Sql = new SQL(connectionString, queryString);
            if (Sql.ExecuteReader(out string strError))
            {
                if (!string.IsNullOrEmpty(strError))
                    throw new Exception(strError);
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
            if (lvQuery.Items.Count > 0 && ExportListView.ExportToExcel(lvQuery, "Query Results"))
                MessageBox.Show("Successfully exported to Excel");
            else
                MessageBox.Show("Failed to export to Excel");
        }

        private void ChkPrevAll_CheckedChanged(object sender, EventArgs e)
        {
            lblPreview.Enabled = !chkPrevAll.Checked;
            txtPreview.Enabled = !chkPrevAll.Checked;
            
            RefreshView();
        }

        private void SetupSearchResults()
        {
            lvResults.Clear();
            lvResults.Columns.Add("Schema");
            lvResults.Columns[0].Width = TextRenderer.MeasureText("XXXXXXXXXX", lvResults.Font).Width;
            lvResults.Columns.Add("Table");
            lvResults.Columns[1].Width = TextRenderer.MeasureText("XXXXXXXXXX", lvResults.Font).Width;
            lvResults.Columns.Add("Column");
            lvResults.Columns[2].Width = TextRenderer.MeasureText("XXXXXXXXXXXXXXXXXXXXXXXXXX", lvResults.Font).Width;
        }
        private void FillSearchResults()
        {
            foreach (KeyValuePair<DBTableKey, DBTable> KVP in g_TableMap)
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
                    if (!bRows)
                        Item.BackColor = Color.Red;

                    ListViewItem.ListViewSubItem TableItem = new ListViewItem.ListViewSubItem(Item, strTable);
                    if (!bRows)
                        TableItem.BackColor = Color.Red;
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
                        if (g_TableMap[new DBTableKey(KVP.Key.Schema, KVP.Key.Table, KVP.Key.Column)].HasKey(TK))
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

        private void BtnSearch_Click(object sender, EventArgs e)
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
        public int Compare(bool bAsc, object lhs, object rhs)
        {
            string sLhs = lhs != null ? lhs.ToString() : string.Empty;
            string sRhs = rhs != null ? rhs.ToString() : string.Empty;
            bool bLhsNum = int.TryParse(sLhs, out int iLhs);
            bool bRhsNum = int.TryParse(sRhs, out int iRhs);

            bool bNumber = (bLhsNum && bRhsNum) ? true : false;
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
        private void LvQuery_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Sort using the classes comparer
            Global.g_bSortOrder = m_oSortOrder[e.Column];

            // Sort the indexed column and rearrange
            m_Arr.ParallelSort(e.Column);
//            m_Arr.Sort(e.Column);

            m_oSortOrder[e.Column] = !m_oSortOrder[e.Column];
            lvQuery.Invalidate();
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
            BtnSearch_Click(null, null);
        }

        private void BtnSearch_TextChanged(object sender, EventArgs e)
        {
            BtnSearch_Click(null, null);
        }
        private string TableCurSel(int idx)
        {
            string strSelection = string.Empty;
            if (g_TableMap != null && idx < g_TableMap.Count)
            {
                foreach (KeyValuePair<DBTableKey, DBTable> KVP in g_TableMap)
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
        private void LvQuery_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            int iRow = e.ItemIndex;
            try
            {
                if (m_Arr != null && iRow <= m_Arr.Length)
                {
                    for (int idx = 0; idx < m_nColumns; ++idx)
                    {
                        string strCellValue = m_Arr[idx][iRow].CellValue;
                        if (idx == 0)
                        {
                            e.Item = new ListViewItem(strCellValue);
                            e.Item.UseItemStyleForSubItems = false;
                        }
                        else
                        {
                            ListViewItem.ListViewSubItem lvSubItem = new ListViewItem.ListViewSubItem(e.Item, strCellValue);
                            e.Item.SubItems.Add(lvSubItem);
                        }
                    }

                    // Style the values
                    int iColStart = 0;
                    foreach (DBTableKey TableKey in m_TableKeys)
                    {
                        DBTable Table = g_TableMap[TableKey];
                        int nCols = g_TableMap[TableKey].Columns.Count;
                        for (int idx = 0; idx < nCols; ++idx)
                        {
                            int iCol = iColStart + idx;
                            ListViewItem.ListViewSubItem lvSI = e.Item.SubItems[iCol];

                            DBColumn Column = Table.Columns[idx];
                            if (Column.IsPrimaryKey)
                            {
                                if (Table.ContainsFK(TableKey.Schema, TableKey.Table, Column.Name))
                                {
                                    // The case where the the foreign key is in the primary table
                                    lvSI.ForeColor = Color.DarkBlue;
                                    lvSI.BackColor = Color.Yellow;
                                }
                                else
                                {
                                    lvSI.ForeColor = Color.DarkBlue;
                                    lvSI.BackColor = Color.Yellow;
                                }
                            }
                            else
                            {
                                DBTableKey TK = new DBTableKey(TableKey.Schema, TableKey.Table, Column.Name);
                                if (g_TableMap[TableKey].HasKey(TK))
                                {
                                    lvSI.ForeColor = Color.DarkBlue;
                                    lvSI.BackColor = Color.Yellow;
                                }
                            }
                        }
                        iColStart += nCols;
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("{0}", e.ItemIndex));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0} : {1}", e.ItemIndex, ex.Message));
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
                DBTable Table = g_TableMap[TK];
                e.Item = new ListViewItem(strSchema);
                e.Item.UseItemStyleForSubItems = false;
                ListViewItem.ListViewSubItem lvSubValue = new ListViewItem.ListViewSubItem(e.Item, strTable);
                if (Table.Rows == "0")
                {
                    e.Item.BackColor = Color.Red;
                    lvSubValue.BackColor = Color.Red;
                }
                e.Item.SubItems.Add(lvSubValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0} : {1}", e.ItemIndex, ex.Message));
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
                foreach (KeyValuePair<DBTableKey, DBTable> KVP in g_TableMap)
                {
                    if (nRows + KVP.Value.Columns.Count <= e.ItemIndex)
                        nRows += KVP.Value.Columns.Count;
                    else
                    {
                        DBTableKey TableKey = KVP.Key;
                        DBTable Table = KVP.Value;

                        idx = e.ItemIndex - nRows;
                        e.Item = new ListViewItem();
                        e.Item.UseItemStyleForSubItems = false;
                        e.Item.Text = TableKey.Schema;
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
                            if (g_TableMap[TableKey].HasKey(TK))
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
                MessageBox.Show(string.Format("{0} : {1}", e.ItemIndex, ex.Message));
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
        private void LvColumns_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            int iRow = e.ItemIndex;
            try
            {
                if (m_Arr != null)
                {
                    DBTableKey TableKey = m_TableKeys[0];
                    string strSchema = TableKey.Schema;
                    string strTable = TableKey.Table;
                    string strColumn = string.Empty;
                    DBTable Table = g_TableMap[TableKey];
                    DBColumn Column = Table.Columns[iRow];
                    e.Item = new ListViewItem(Column.Name);
                    e.Item.UseItemStyleForSubItems = false;

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
                        if (g_TableMap[TableKey].HasKey(TK))
                        {
                            e.Item.ForeColor = Color.DarkBlue;
                            e.Item.BackColor = Color.Yellow;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0} : {1}", e.ItemIndex, ex.Message));
            }
            finally { }
        }

        private void LvTables_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            if (g_TableMap.ContainsKey(TK))
            {
                int iSelectedIndex = g_TableMap[TK].SelectedIndex;
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
            if (g_TableMap.ContainsKey(TK))
            {
                int iSelectedIndex = g_TableMap[TK].SelectedIndex;
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
            BtnSearch_Click(null, null);
        }
        private void LvResults_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int iResIdx = ResultTableIndex();
            if (iResIdx == -1)
                return;

            string strSchema = lvResults.SelectedItems[0].Text;
            string strTable = lvResults.SelectedItems[0].SubItems[1].Text;

            DBTableKey TK = new DBTableKey(strSchema, strTable, string.Empty);
            if (g_TableMap.ContainsKey(TK))
            {
                DBTable Table = g_TableMap[TK];
                int iSelectedIndex = Table.SelectedIndex;
                lvTables.SelectedIndices.Add(iSelectedIndex);
                SelectIndex();
                lvTables.EnsureVisible(iSelectedIndex);
            }
        }

        private void LvQuery_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSel = lvQuery.SelectedIndices.Count;
            if (iSel == 0)
                return;
        }

        private void CbDataBases_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtConnString.Text = cbDataBases.Text;
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
                    Maze Path = new Maze(g_TableMap);
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
            foreach (KeyValuePair<DBTableKey, DBTable> KVP in g_TableMap)
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
    }

    class ListViewItemComparer : IComparer
    {
        private readonly int m_iColumn;
        private readonly bool m_bAscending;
        public ListViewItemComparer()
        {
            m_iColumn = 0;
            m_bAscending = false;
        }

        public ListViewItemComparer(int iColumn, bool bAscending) : this()
        {
            m_iColumn = iColumn;
            m_bAscending = bAscending;
        }
        public ListViewItemComparer(int iColumn)
        {
            m_iColumn = iColumn;
        }
        public int Compare(object lhs, object rhs)
        {
            string sLhs = ((ListViewItem)lhs).SubItems[m_iColumn].Text;
            string sRhs = ((ListViewItem)rhs).SubItems[m_iColumn].Text;
            bool bLhsNum = int.TryParse(sLhs, out int iLhs);
            bool bRhsNum = int.TryParse(sRhs, out int iRhs);

            bool bNumber = (bLhsNum && bRhsNum) ? true : false;
            if (bNumber)
                return m_bAscending ? (iLhs < iRhs ? -1 : (iLhs == iRhs ? 0 : 1)) : (iLhs < iRhs ? 1 : (iLhs == iRhs ? 0 : -1));
            else
                return m_bAscending ? string.Compare(sLhs, sRhs) : string.Compare(sRhs, sLhs); // Do we need insensitive?
        }
    }
}