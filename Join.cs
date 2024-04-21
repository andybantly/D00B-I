using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;

/*
 * https://www.w3schools.com/sql/sql_join.asp
 * INNER -  The INNER JOIN keyword selects records that have matching values in both tables
 * LEFT -   The LEFT JOIN keyword returns all records from the left table (table1), and the 
 *          matching records from the right table (table2). 
 *          The result is 0 records from the right side, if there is no match.
 * RIGHT -  The RIGHT JOIN keyword returns all records from the right table (table2), and the 
 *          matching records from the left table (table1). The result is 0 records from the left side,
 *          if there is no match.
 * FULL -   The FULL OUTER JOIN keyword returns all records when there is a match in left (table1) 
 *          or right (table2) table records.
 * SELF -   A self join is a regular join, but the table is joined with itself.
 */
namespace D00B
{
    public partial class DlgJoin : Form
    {
        private Dictionary<DBTableKey, DBTable> m_TableMap;
        private string m_strSrcSchema = string.Empty;
        private string m_strSrcTable = string.Empty;
        private string m_strSrcColumn = string.Empty;
        private string m_strJoinSchema = string.Empty;
        private string m_strJoinTable = string.Empty;
        private string m_strJoinColumn = string.Empty;
        private Utility.Join m_Join;
        public DlgJoin(Dictionary<DBTableKey, DBTable> TableMap)
        {
            InitializeComponent();
            m_TableMap = TableMap;
        }

        private void DlgJoin_Load(object sender, EventArgs e)
        {
            lvJoinTables.VirtualListSize = 0;
            bool bRHS = m_strJoinSchema != string.Empty;
            optInner.Enabled = bRHS;
            optLeft.Enabled = bRHS;
            optRight.Enabled = bRHS;
            optFull.Enabled = bRHS;
            optSelf.Enabled = true;

            optInner.Checked = bRHS;
            optSelf.Checked = !bRHS;

            txtJoin.Font = Utility.m_Font;

            // Setup the join table headers
            lvJoinTables.Font = Utility.m_Font;
            Utility.SetupListViewHeaders(lvJoinTables);

            int nTotalColumns = 0;
            foreach (KeyValuePair<DBTableKey, DBTable> KVP in m_TableMap) { nTotalColumns += KVP.Value.Columns.Count; }
            lvJoinTables.VirtualListSize = nTotalColumns;
            lvJoinTables.SelectedIndices.Clear();
        }

        private int JoinTablesIndex()
        {
            ListView.SelectedIndexCollection Col = lvJoinTables.SelectedIndices;
            if (Col == null || Col.Count == 0)
                return -1;
            return Col[0];
        }

        private void Opt_CheckedChanged(object sender, EventArgs e)
        {
            string strType;
            if (optInner.Checked)
            {
                strType = "INNER";
                m_Join = Utility.Join.Inner;
            }
            else if (optLeft.Checked)
            {
                strType = "LEFT";
                m_Join = Utility.Join.Left;
            }
            else if (optRight.Checked)
            {
                strType = "RIGHT";
                m_Join = Utility.Join.Right;
            }
            else if (optFull.Checked)
            {
                strType = "FULL";
                m_Join = Utility.Join.Full;
            }
            else
            {
                strType = "SELF";
                m_Join = Utility.Join.Self;
            }

            if (m_Join != Utility.Join.Self)
                txtJoin.Text = string.Format("Add a {0} JOIN from\r\n\r\n[{1}].[{2}].{3}\r\n\r\nTo\r\n\r\n[{4}].[{5}].{6}",
                    strType, m_strSrcSchema, m_strSrcTable, m_strSrcColumn,
                    m_strJoinSchema, m_strJoinTable, m_strJoinColumn);
            else
                txtJoin.Text = string.Format("Add a {0} JOIN from\r\n\r\n[{1}].[{2}].{3}\r\n\r\nTo\r\n\r\n[{1}].[{2}].{3}",
                    strType, m_strSrcSchema, m_strSrcTable, m_strSrcColumn);
        }

        public string SourceSchema
        {
            get { return m_strSrcSchema; }
            set { if (!string.IsNullOrEmpty(value)) m_strSrcSchema = value; }
        }
        public string SourceTable
        {
            get { return m_strSrcTable; }
            set { if (!string.IsNullOrEmpty(value)) m_strSrcTable = value; }
        }
        public string SourceColumn
        {
            get { return m_strSrcColumn; }
            set { if (!string.IsNullOrEmpty(value)) m_strSrcColumn = value; }
        }
        public string JoinSchema
        {
            get { return m_strJoinSchema; }
            set { if (!string.IsNullOrEmpty(value)) m_strJoinSchema = value; }
        }
        public string JoinTable
        {
            get { return m_strJoinTable; }
            set { if (!string.IsNullOrEmpty(value)) m_strJoinTable = value; }
        }
        public string JoinColumn
        {
            get { return m_strJoinColumn; }
            set { if (!string.IsNullOrEmpty(value)) m_strJoinColumn = value; }
        }

        public Utility.Join JoinType
        {
            get { return m_Join; }
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
    }
}
