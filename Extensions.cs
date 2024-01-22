using System.Drawing;
using System.Windows.Forms;

namespace D00B
{
    public static class Extensions
    {
        public static System.Drawing.Color ForeColor(this ListViewItem Item)
        {
            Color ForeColor = SystemColors.WindowText;
            return ForeColor;
        }
    }
}