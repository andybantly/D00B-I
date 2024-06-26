﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        private List<ListViewItem> m_AdjTables;
        private bool m_bIncludeAll = true;
        private string m_strSrcSchema = string.Empty;
        private string m_strSrcTable = string.Empty;
        private string m_strSrcColumn = string.Empty;
        List<DBJoinKey> m_JoinKeys;
        public DlgJoin(Dictionary<DBTableKey, DBTable> TableMap, List<ListViewItem> AdjTables)
        {
            InitializeComponent();
            m_TableMap = TableMap;
            m_AdjTables = AdjTables;
            m_JoinKeys = new List<DBJoinKey>();
            rbAll.Checked = true;
        }

        private int UpdateUI()
        {
            int iTable = JoinTablesIndex();
            optInner.Enabled = iTable > -1;
            optFull.Enabled = iTable > -1;
            optLeft.Enabled = iTable > -1;
            optRight.Enabled = iTable > -1;
            optSelf.Enabled = iTable > -1;
            m_bIncludeAll = rbAll.Checked;
            return iTable;
        }

        private int CountListItems()
        {
            int nCount = 0;
            if (m_bIncludeAll)
            {
                foreach (KeyValuePair<DBTableKey, DBTable> KVP in m_TableMap)
                    nCount += KVP.Value.Columns.Count;
            }
            else
                nCount = m_AdjTables.Count;

            return nCount;
        }

        private void DlgJoin_Load(object sender, EventArgs e)
        {
            lvJoinTables.VirtualListSize = 0;

            bool bRHS = JoinTablesIndex() > -1;
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

            lvJoinTables.VirtualListSize = CountListItems();
            lvJoinTables.SelectedIndices.Clear();
        }

        private int JoinTablesIndex()
        {
            ListView.SelectedIndexCollection Col = lvJoinTables.SelectedIndices;
            if (Col == null || Col.Count == 0)
                return -1;
            return Col[0];
        }

        private void LvJoinTables_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            int nRows, idx;
            try
            {
                if (m_bIncludeAll)
                {
                    nRows = 0;
                    foreach (KeyValuePair<DBTableKey, DBTable> KVP in m_TableMap)
                    {
                        if (nRows + KVP.Value.Columns.Count <= e.ItemIndex)
                            nRows += KVP.Value.Columns.Count;
                        else
                        {
                            DBTableKey TableKey = KVP.Key;
                            DBTable Table = KVP.Value;

                            bool bNeighbor = false;
                            idx = e.ItemIndex - nRows;
                            DBColumn Column = Table.Columns[idx];
                            e.Item = new ListViewItem(TableKey.Schema);
                            e.Item.UseItemStyleForSubItems = false;
                            e.Item.SubItems.Add(TableKey.Table);
                            e.Item.SubItems.Add(Column.Name);

                            DBTableKey FK = new DBTableKey(TableKey.Schema, TableKey.Table, Column.Name);
                            if (Column.IsPrimaryKey)
                            {
                                if (Table.ContainsFK(FK))
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
                                if (m_TableMap[TableKey].HasKey(FK))
                                {
                                    e.Item.SubItems[2].ForeColor = Color.DarkBlue;
                                    e.Item.SubItems[2].BackColor = Color.Yellow;
                                }

                                if (bNeighbor)
                                {
                                    e.Item.ForeColor = Color.White;
                                    e.Item.BackColor = Color.Purple;
                                    e.Item.SubItems[1].ForeColor = Color.White;
                                    e.Item.SubItems[1].BackColor = Color.Purple;
                                }
                            }

                            if (Table.Rows == 0)
                            {
                                if (!bNeighbor)
                                    e.Item.BackColor = Color.Red;
                                e.Item.SubItems[1].BackColor = Color.Red;
                            }

                            break;
                        }
                    }
                }
                else
                    e.Item = m_AdjTables[e.ItemIndex];
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0} : {1}", e.ItemIndex, ex.Message));
            }
            finally { }
        }

        private void JoinTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iTable = UpdateUI();
            if (iTable < 0)
                return;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            int iItemIndex = JoinTablesIndex();

            string strType;
            Utility.Join Join;
            if (optInner.Checked)
            {
                strType = "INNER";
                Join = Utility.Join.Inner;
            }
            else if (optLeft.Checked)
            {
                strType = "LEFT";
                Join = Utility.Join.Left;
            }
            else if (optRight.Checked)
            {
                strType = "RIGHT";
                Join = Utility.Join.Right;
            }
            else if (optFull.Checked)
            {
                strType = "FULL";
                Join = Utility.Join.Full;
            }
            else
            {
                strType = "SELF";
                Join = Utility.Join.Self;
            }

            if (m_bIncludeAll)
            {
                int nRows = 0;
                foreach (KeyValuePair<DBTableKey, DBTable> KVP in m_TableMap)
                {
                    if (nRows + KVP.Value.Columns.Count <= iItemIndex)
                        nRows += KVP.Value.Columns.Count;
                    else
                    {
                        DBTableKey TableKey = KVP.Key;
                        DBTable Table = KVP.Value;

                        int idx = iItemIndex - nRows;
                        DBColumn Column = Table.Columns[idx];

                        if (Join != Utility.Join.Self)
                            m_JoinKeys.Add(new DBJoinKey(TableKey.Schema, TableKey.Table, Column.Name, Join));
                        else
                            m_JoinKeys.Add(new DBJoinKey(SourceSchema, SourceTable, SourceColumn, Join));
                        break;
                    }
                }
            }
            else
            {
                ListViewItem lvi = m_AdjTables[iItemIndex];
                if (Join != Utility.Join.Self)
                    m_JoinKeys.Add(new DBJoinKey(lvi.Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text, Join));
                else
                    m_JoinKeys.Add(new DBJoinKey(SourceSchema, SourceTable, SourceColumn, Join));
            }

            txtJoin.Text = string.Empty;
            foreach (DBJoinKey JoinKey in m_JoinKeys)
            {
                if (txtJoin.Text.Length > 0)
                    txtJoin.Text += "\r\n";
                txtJoin.Text += string.Format("{0} JOIN from [{1}].[{2}].{3} To [{4}].[{5}].{6}",
                    strType, m_strSrcSchema, m_strSrcTable, m_strSrcColumn,
                    JoinKey.Schema, JoinKey.Table, JoinKey.Column);
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            m_JoinKeys.Clear();
            txtJoin.Text = string.Empty;
        }
        private void Include_Changed(object sender, EventArgs e)
        {
            lvJoinTables.VirtualListSize = 0;
            m_bIncludeAll = rbAll.Checked;
            lvJoinTables.VirtualListSize = CountListItems();
            lvJoinTables.SelectedIndices.Clear();
            UpdateUI();
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

        public List<DBJoinKey> JoinKeys
        {
            get
            {
                return m_JoinKeys;
            }
        }
    }
}
