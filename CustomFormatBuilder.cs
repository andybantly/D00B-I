using Microsoft.Office.Interop.Excel;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace D00B
{
    public partial class CustomFormatBuilder : Form
    {
        private TypeCode m_TypeCode;

        ListViewItem CustomFormat(string strFormat, string strDescription)
        {
            ListViewItem Item = new ListViewItem(strFormat);
            Item.UseItemStyleForSubItems = false;
            ListViewItem.ListViewSubItem SubItem = new ListViewItem.ListViewSubItem(Item, strDescription);
            Item.SubItems.Add(SubItem);
            return Item;
        }
        public CustomFormatBuilder(string strColumnName, TypeCode TypeCode)
        {
            InitializeComponent();

            txtCustomFormat.Text = string.Format(txtCustomFormat.Text, strColumnName);
            m_TypeCode = TypeCode;
            lvFormat.Columns.Add("Format");
            lvFormat.Columns.Add("Descripion");

            switch (m_TypeCode)
            {
                case TypeCode.DateTime:
                    lvFormat.Items.Add(CustomFormat("d", "The day of the month, from 1 to 31"));
                    lvFormat.Items.Add(CustomFormat("dd", "The day of the month, from 01 to 31"));
                    lvFormat.Items.Add(CustomFormat("ddd", "The abbreviated name of the day of the week"));
                    lvFormat.Items.Add(CustomFormat("dddd", "The full name of the day of the week"));
                    lvFormat.Items.Add(CustomFormat("f", "The tenths of a second in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("ff", "The hundredths of a second in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("fff", "The milliseconds in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("ffff", "The ten thousandths of a second in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("fffff", "The hundred thousandths of a second in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("ffffff", "The millionths of a second in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("fffffff", "The ten millionths of a second in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("F", "If non-zero, the tenths of a second in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("FF", "If non-zero, the hundredths of a second in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("FFF", "If non-zero, the milliseconds in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("FFFF", "If non-zero, the ten thousandths of a second in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("FFFFF", "If non-zero, the hundred thousandths of a second in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("FFFFFF", "If non-zero, the millionths of a second in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("FFFFFFF", "If non-zero, the ten millionths of a second in a date and time value"));
                    lvFormat.Items.Add(CustomFormat("g", "The period or era"));
                    lvFormat.Items.Add(CustomFormat("gg", "The period or era"));
                    lvFormat.Items.Add(CustomFormat("h", "The hour, using a 12-hour clock from 1 to 12"));
                    lvFormat.Items.Add(CustomFormat("hh", "The hour, using a 12-hour clock from 01 to 12"));
                    lvFormat.Items.Add(CustomFormat("H", "The hour, using a 24-hour clock from 0 to 23"));
                    lvFormat.Items.Add(CustomFormat("HH", "The hour, using a 24-hour clock from 00 to 23"));
                    lvFormat.Items.Add(CustomFormat("K", "Time zone information"));
                    lvFormat.Items.Add(CustomFormat("m", "The minute, from 0 to 59"));
                    lvFormat.Items.Add(CustomFormat("mm", "The minute, from 00 to 59"));
                    lvFormat.Items.Add(CustomFormat("M", "The month, from 1 to 12"));
                    lvFormat.Items.Add(CustomFormat("MM", "The month, from 01 to 12"));
                    lvFormat.Items.Add(CustomFormat("MMM", "The abbreviated name of the month"));
                    lvFormat.Items.Add(CustomFormat("MMMM", "The full name of the month"));
                    lvFormat.Items.Add(CustomFormat("s", "The second, from 0 to 59"));
                    lvFormat.Items.Add(CustomFormat("ss", "The second, from 00 to 59"));
                    lvFormat.Items.Add(CustomFormat("t", "The first character of the AM/PM designator"));
                    lvFormat.Items.Add(CustomFormat("tt", "The AM/PM designator"));
                    lvFormat.Items.Add(CustomFormat("y", "The year, from 0 to 99"));
                    lvFormat.Items.Add(CustomFormat("yy", "The year, from 00 to 99"));
                    lvFormat.Items.Add(CustomFormat("yyy", "The year, with a minimum of three digits"));
                    lvFormat.Items.Add(CustomFormat("yyyy", "The year as a four-digit number"));
                    lvFormat.Items.Add(CustomFormat("yyyyy", "The year as a five-digit number"));
                    lvFormat.Items.Add(CustomFormat("z", "Hours offset from UTC, with no leading zeros"));
                    lvFormat.Items.Add(CustomFormat("zz", "Hours offset from UTC, with a leading zero for a single-digit value"));
                    lvFormat.Items.Add(CustomFormat("zzz", "Hours and minutes offset from UTC"));
                    lvFormat.Items.Add(CustomFormat(":", "The time separator"));
                    lvFormat.Items.Add(CustomFormat("/", "The date separator"));
                    lvFormat.Items.Add(CustomFormat("\\", "The escape character"));
                    break;

                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    lvFormat.Items.Add(CustomFormat("0", "Zero placeholder. Replaces the zero with the corresponding digit if one is present; otherwise, zero appears in the result string"));
                    lvFormat.Items.Add(CustomFormat("#", "Digit placeholder. Replaces the \"#\" symbol with the corresponding digit if one is present; otherwise, no digit appears in the result string"));
                    lvFormat.Items.Add(CustomFormat(".", "Decimal point. Determines the location of the decimal separator in the result string"));
                    lvFormat.Items.Add(CustomFormat(",", "Group separator and number scaling. Serves as both a group separator and a number scaling specifier. As a group separator, it inserts a localized group separator character between each group. As a number scaling specifier, it divides a number by 1000 for each comma specified"));
                    lvFormat.Items.Add(CustomFormat("%", "Percentage placeholder. Multiplies a number by 100 and inserts a localized percentage symbol in the result string"));
                    lvFormat.Items.Add(CustomFormat("‰", "Per mille placeholder. Multiplies a number by 1000 and inserts a localized per mille symbol in the result string"));
                    lvFormat.Items.Add(CustomFormat("E0", "Exponential notation. If followed by at least one 0 (zero), formats the result using exponential notation. The case of \"E\" or \"e\" indicates the case of the exponent symbol in the result string. The number of zeros following the \"E\" or \"e\" character determines the minimum number of digits in the exponent. A plus sign (+) indicates that a sign character always precedes the exponent. A minus sign (-) indicates that a sign character precedes only negative exponents"));
                    lvFormat.Items.Add(CustomFormat("E+0", "Exponential notation. If followed by at least one 0 (zero), formats the result using exponential notation. The case of \"E\" or \"e\" indicates the case of the exponent symbol in the result string. The number of zeros following the \"E\" or \"e\" character determines the minimum number of digits in the exponent. A plus sign (+) indicates that a sign character always precedes the exponent. A minus sign (-) indicates that a sign character precedes only negative exponents"));
                    lvFormat.Items.Add(CustomFormat("E-0", "Exponential notation. If followed by at least one 0 (zero), formats the result using exponential notation. The case of \"E\" or \"e\" indicates the case of the exponent symbol in the result string. The number of zeros following the \"E\" or \"e\" character determines the minimum number of digits in the exponent. A plus sign (+) indicates that a sign character always precedes the exponent. A minus sign (-) indicates that a sign character precedes only negative exponents"));
                    lvFormat.Items.Add(CustomFormat("e0", "Exponential notation. If followed by at least one 0 (zero), formats the result using exponential notation. The case of \"E\" or \"e\" indicates the case of the exponent symbol in the result string. The number of zeros following the \"E\" or \"e\" character determines the minimum number of digits in the exponent. A plus sign (+) indicates that a sign character always precedes the exponent. A minus sign (-) indicates that a sign character precedes only negative exponents"));
                    lvFormat.Items.Add(CustomFormat("e+0", "Exponential notation. If followed by at least one 0 (zero), formats the result using exponential notation. The case of \"E\" or \"e\" indicates the case of the exponent symbol in the result string. The number of zeros following the \"E\" or \"e\" character determines the minimum number of digits in the exponent. A plus sign (+) indicates that a sign character always precedes the exponent. A minus sign (-) indicates that a sign character precedes only negative exponents"));
                    lvFormat.Items.Add(CustomFormat("e-0", "Exponential notation. If followed by at least one 0 (zero), formats the result using exponential notation. The case of \"E\" or \"e\" indicates the case of the exponent symbol in the result string. The number of zeros following the \"E\" or \"e\" character determines the minimum number of digits in the exponent. A plus sign (+) indicates that a sign character always precedes the exponent. A minus sign (-) indicates that a sign character precedes only negative exponents"));
                    lvFormat.Items.Add(CustomFormat("\\", "Escape character"));
                    break;

                default:
                    break;
            }

            float nFontHeight = 10.0F;
            System.Drawing.Font Font = Utility.MakeFont(nFontHeight, FontFamily.GenericMonospace, FontStyle.Regular);
            Size szColumn = TextRenderer.MeasureText(lvFormat.Columns[1].Text, Font);
            for (int i = 0; i < lvFormat.Items.Count; i++)
            {
                Size sz = TextRenderer.MeasureText(lvFormat.Items[i].SubItems[1].Text, Font);
                if (sz.Width > szColumn.Width)
                    szColumn = sz;
            }
            lvFormat.Columns[1].Width = szColumn.Width;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFormat.Text = string.Empty;
        }
    }
}
