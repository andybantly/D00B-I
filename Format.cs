using System;
using System.Windows.Forms;

namespace D00B
{
    public partial class Format : Form
    {
        private TypeCode m_TypeCode;
        public Format(string strColumnName, TypeCode TypeCode, string strFormatString)
        {
            InitializeComponent();
            FmtLabel.Text = string.Format(FmtLabel.Text, strColumnName); 
            FormatString = strFormatString;
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

        private void Format_Load(object sender, EventArgs e)
        {
            switch (m_TypeCode) 
            {
                default:
                    break;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
