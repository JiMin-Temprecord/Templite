using System;
using System.Windows.Forms;

namespace UserControls
{
    public partial class ResetUserConfirmUserControl : UserControl
    {
        private void resetButton_Click(object sender, EventArgs e)
        {

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
