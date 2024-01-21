using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private string m_strSrcOwner = string.Empty;
        private string m_strSrcTable = string.Empty;
        private string m_strSrcColumn = string.Empty;
        private string m_strJoinOwner = string.Empty;
        private string m_strJoinTable = string.Empty;
        private string m_strJoinColumn = string.Empty;
        private Utility.Join m_Join;
        public DlgJoin()
        {
            InitializeComponent();
        }
        public string SourceOwner
        {
            get { return m_strSrcOwner; }
            set { if (!string.IsNullOrEmpty(value)) m_strSrcOwner = value; }
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
        public string JoinOwner
        {
            get { return m_strJoinOwner; }
            set { if (!string.IsNullOrEmpty(value)) m_strJoinOwner = value; }
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

        private void DlgJoin_Load(object sender, EventArgs e)
        {
            bool bRHS = m_strJoinOwner != string.Empty;
            optInner.Enabled = bRHS;
            optLeft.Enabled = bRHS;
            optRight.Enabled = bRHS;
            optFull.Enabled = bRHS;
            optSelf.Enabled = true;

            optInner.Checked = bRHS;
            optSelf.Checked = !bRHS;

            txtJoin.Font = Utility.MakeFont(txtJoin.Font.Size, txtJoin.Font.FontFamily, FontStyle.Bold);
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
                    strType, m_strSrcOwner, m_strSrcTable, m_strSrcColumn,
                    m_strJoinOwner, m_strJoinTable, m_strJoinColumn);
            else
                txtJoin.Text = string.Format("Add a {0} JOIN from\r\n\r\n[{1}].[{2}].{3}\r\n\r\nTo\r\n\r\n[{1}].[{2}].{3}",
                    strType, m_strSrcOwner, m_strSrcTable, m_strSrcColumn);
        }
    }
}
