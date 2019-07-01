using System;
using System.IO;
using System.Windows.Forms;
using TempLite;
using TempLite.Constant;

namespace UserControls
{
    public partial class PasswordUserControl : UserControl
    {
        readonly string path = AppDomain.CurrentDomain.BaseDirectory + "Email\\";
        public bool isReset = false;

        public PasswordUserControl()
        {
            InitializeComponent();
        }

        private void keycodeTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == Keys.Return.ToString())
            {
                if (keycodeTextbox.Text == EmailConstant.AdminPassword)
                    CopyEmailList(EmailConstant.AdminEMail);
                else if (keycodeTextbox.Text == EmailConstant.ARCBSPassword)
                    CopyEmailList(EmailConstant.ARCBSEmail);
                else if (keycodeTextbox.Text == EmailConstant.DefaultPassword)
                    CopyEmailList(EmailConstant.DefaultEmail);
                else
                    promptMessage.Text = "Incorrect Key Code";
            }
        }

        void CopyEmailList (string TargetEmail)
        {
            if (isReset)
            {
                File.WriteAllText(path + EmailConstant.AllEmail, String.Empty);
            }

            File.Copy(path + TargetEmail, path + EmailConstant.AllEmail, true);
            Console.WriteLine("Copied text file");
            this.Visible = false;
            
        }
    }
}
