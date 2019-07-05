using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TempLite;

namespace UserControls
{
    public partial class ListEmailUserControl : UserControl
    {
        public ListEmailUserControl()
        {
            InitializeComponent();
        }
        void emailDeleteButton_Click(object sender, EventArgs e)
        {
            var userConfirmationForm = new UserConfirmationForm();
            var dialogResult = userConfirmationForm.ShowDialog();

            if(dialogResult == DialogResult.Yes)
            {
                Email.Delete(emailLabel.Text);
                this.Dispose();
            }

            userConfirmationForm.Dispose();
        }
    }
}
