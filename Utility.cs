using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace D00B
{
    public static class Utility
    {
        static float m_nFontHeight;
        public static Font m_Font;
        public static int m_nMaxSchemaWidth;
        public static int m_nMaxTableWidth;
        public static int m_nMaxColumnWidth;
        static Utility()
        {
            m_nFontHeight = 10.0F;
            m_Font = MakeFont(m_nFontHeight, FontFamily.GenericMonospace, FontStyle.Regular);

            m_nMaxSchemaWidth = 0;
            m_nMaxTableWidth = 0;
            m_nMaxColumnWidth = 0;
        }

        public enum Join // ILRFS
        {
            Inner, Left, Right, Full, Self
        }
        public static int GetHashCode(string strHash)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(strHash), Base64FormattingOptions.None).GetHashCode();
        }
        public static Font MakeFont(float nFontHeight, FontFamily FF, FontStyle FS)
        {
            return new System.Drawing.Font(FF, nFontHeight, FS);
        }

        public static void SetupListViewHeaders(ListView lv, bool bSchema = true, bool bTable = true, bool bColumn = true, bool bFormat = false)
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
                lv.Columns.Add("Align");
                lv.Columns[lv.Columns.Count - 1].Width = TextRenderer.MeasureText("XXXXXXXXXX", m_Font).Width;
                lv.Columns.Add("Culture");
                lv.Columns[lv.Columns.Count - 1].Width = TextRenderer.MeasureText("XXXXXXXXXX", m_Font).Width;
            }
        }
    }
}
