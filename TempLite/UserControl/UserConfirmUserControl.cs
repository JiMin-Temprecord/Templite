using System;
using System.Windows.Forms;

namespace UserControls
{
    public partial class UserConfirmUserControl : UserControl
    {
        public static bool isVisible { get; set; } = true;
        public static bool shouldDelete { get; set; } = false;
        public UserConfirmUserControl()
        {
            isVisible = true;
            shouldDelete = false;
            InitializeComponent();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            shouldDelete = true;
            isVisible = false;
            this.Dispose();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            shouldDelete = false;
            isVisible = false;
            this.Dispose();
        }

        private void UserConfirmUserControl_VisibleChanged(object sender, EventArgs e)
        {
        }
    }
}
