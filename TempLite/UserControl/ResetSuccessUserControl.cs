using System;
using System.IO;
using System.Windows.Forms;
using TempLite;
using TempLite.Constant;

namespace UserControls
{
    public partial class ResetSuccessUserControl : UserControl
    {
        public bool isFirstCopy = true;
        public bool isReset = false;

        public ResetSuccessUserControl()
        {
            InitializeComponent();
        }

        private void emailCancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
