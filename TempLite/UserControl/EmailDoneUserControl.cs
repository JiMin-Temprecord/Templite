using System.Windows.Forms;

namespace UserControls
{
    public partial class EmailDoneUserControl : UserControl
    {
        public EmailDoneUserControl()
        {
            InitializeComponent();
        }

        private void emailCancelButton_Click(object sender, System.EventArgs e)
        {
            Visible = false;
            Parent.Enabled = true;
        }
    }
}
