using System;
using System.Windows.Forms;

namespace D00B
{
    public partial class CustomFormatBuilder : Form
    {
        private TypeCode m_TypeCode;

        public CustomFormatBuilder(TypeCode TypeCode)
        {
            InitializeComponent();

            m_TypeCode = TypeCode;
            lvFormat.Columns.Add("Format");
            lvFormat.Columns.Add("Descripion");

            switch (m_TypeCode)
            {
                case TypeCode.DateTime:
                    break;

                default:
                    break;
            }
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
