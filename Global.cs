using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D00B
{
    public static class Global
    {
        static private bool g_bSortOrder;

        static public bool SortOrder
        {
            get { return g_bSortOrder; }
            set { g_bSortOrder = value; }
        }
    }
}
