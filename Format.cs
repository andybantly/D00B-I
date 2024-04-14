using System;
using System.Windows.Forms;

namespace D00B
{
    public partial class Format : Form
    {
        public Format(string strColumnName, string strFormatString)
        {
            InitializeComponent();
            FmtLabel.Text = string.Format(FmtLabel.Text, strColumnName); 
            FormatString = strFormatString;
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
