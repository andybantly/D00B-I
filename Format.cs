using System;
using System.Windows.Forms;

namespace D00B
{
    public partial class Format : Form
    {
        // Composite Formatting
        // {index[,alignment][:formatString]} <- Index will be 0 as each format is associated with a single column
        private TypeCode m_TypeCode;
        public Format(string strColumnName, TypeCode TypeCode, string strFormatString, int iAlignment)
        {
            InitializeComponent();
            FmtLabel.Text = string.Format(FmtLabel.Text, strColumnName);
            AlignLabel.Text = string.Format(AlignLabel.Text, strColumnName);
            FormatString = strFormatString;
            Alignment = iAlignment;
            m_TypeCode = TypeCode;
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

        private void Format_Load(object sender, EventArgs e)
        {
            switch (m_TypeCode) 
            {
                default:
                    break;
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
    }
}
