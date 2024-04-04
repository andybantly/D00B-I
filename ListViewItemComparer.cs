using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D00B
{
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
