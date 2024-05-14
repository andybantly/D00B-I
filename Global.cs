using System.Windows.Forms;

namespace D00B
{
    public static class Global
    {
        static private SortOrder g_SortOrder;

        static public SortOrder SortOrder
        {
            get { return g_SortOrder; }
            set { g_SortOrder = value; }
        }
    }
}
