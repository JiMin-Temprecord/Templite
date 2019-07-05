using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TempLite
{
    public partial class ResetSuccessfulForm : Form
    {
        public ResetSuccessfulForm()
        {
            InitializeComponent();
        }

        private void emailCancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
