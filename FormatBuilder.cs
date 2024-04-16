using Microsoft.Office.Core;
using System;
using System.Windows.Forms;

namespace D00B
{
    public partial class FormatBuilder : Form
    {
        // Composite Formatting
        // {index[,alignment][:formatString]} <- Index will be 0 as each format is associated with a single column
        private TypeCode m_TypeCode;
        public FormatBuilder(string strColumnName, TypeCode TypeCode, string strFormatString, int iAlignment)
        {
            InitializeComponent();
            FmtLabel.Text = string.Format(FmtLabel.Text, strColumnName);
            AlignLabel.Text = string.Format(AlignLabel.Text, strColumnName);
            FormatString = strFormatString;
            Alignment = iAlignment;
            m_TypeCode = TypeCode;

            switch (m_TypeCode)
            {
                case TypeCode.DateTime:
                    cbFormat.Items.Add("d");
                    cbFormat.Items.Add("D");
                    cbFormat.Items.Add("f");
                    cbFormat.Items.Add("F");
                    cbFormat.Items.Add("g");
                    cbFormat.Items.Add("G");
                    cbFormat.Items.Add("M");
                    cbFormat.Items.Add("m");
                    cbFormat.Items.Add("o");
                    cbFormat.Items.Add("R");
                    cbFormat.Items.Add("r");
                    cbFormat.Items.Add("s");
                    cbFormat.Items.Add("t");
                    cbFormat.Items.Add("T");
                    cbFormat.Items.Add("u");
                    cbFormat.Items.Add("U");
                    cbFormat.Items.Add("Y");
                    cbFormat.Items.Add("y");
                    break;
                default:
                    break;
            }

            cbFormat.SelectedIndex = 0;
        }

        public string FormatString
        {
            get
            {
                return cbFormat.Text;
            }

            set
            {
                cbFormat.Text = value;
            }
        }

        public int Alignment
        {
            get
            {
                return Convert.ToInt32(txtAlignment.Text);
            }

            set
            {
                txtAlignment.Text = value.ToString();
            }
        }
        private void BtnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Alignment_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox TextAlign = sender as TextBox;
            if (!Int32.TryParse(TextAlign.Text, out int _))
            {
                e.Cancel = true;
                MessageBox.Show(string.Format("\"{0}\" is not a valid value for the alignment", TextAlign.Text));
                TextAlign.Focus();
                TextAlign.SelectAll();
            }
            else
                e.Cancel = false;
        }

        private void CbFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strDescription = string.Empty;
            if (m_TypeCode == TypeCode.DateTime)
            {
                switch (cbFormat.Text)
                {
                    case "d":
                        strDescription = "Short date pattern";
                        break;
                    case "D":
                        strDescription = "Long date pattern";
                        break;
                    case "f":
                        strDescription = "Full date/time pattern (short time)";
                        break;
                    case "F":
                        strDescription = "Full date/time pattern (long time)";
                        break;
                    case "g":
                        strDescription = "General date/time pattern (short time)";
                        break;
                    case "G":
                        strDescription = "General date/time pattern (long time)";
                        break;
                    case "M":
                    case "m":
                        strDescription = "Month/day pattern";
                        break;
                    case "O":
                    case "o":
                        strDescription = "round-trip date/time pattern";
                        break;
                    case "R":
                    case "r":
                        strDescription = "RFC1123 pattern";
                        break;
                    case "s":
                        strDescription = "Sortable date/time pattern";
                        break;
                    case "t":
                        strDescription = "Short time pattern";
                        break;
                    case "T":
                        strDescription = "Long time pattern";
                        break;
                    case "u":
                        strDescription = "Universal sortable date/time pattern";
                        break;
                    case "U":
                        strDescription = "Universal full date/time pattern";
                        break;
                    case "Y":
                    case "y":
                        strDescription = "Year month pattern";
                        break;
                    default:
                        strDescription = "Unknown specifier";
                        break;
                }
            }
            txtDescription.Text = strDescription;            
        }
    }
}
