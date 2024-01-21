using System;
using System.Text;
using System.Drawing;

namespace D00B
{
    public class Utility
    {
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
    }
}