using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TempLite;
using TempLite.Constant;

namespace UserControls
{
    public partial class PasswordUserControl : UserControl
    {
        public bool isFirstCopy = true;
        public bool isReset = false;

        public PasswordUserControl()
        {
            InitializeComponent();
        }

        //should only come if it is the first time that email is being copied
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
            string line = string.Empty;

            promptMessage.Text = string.Empty;

            if (isReset)
            {
                using (StreamReader sr = File.OpenText(Email.path + EmailConstant.AllEmail))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        Email.Delete(line,true);
                    }
                }

                File.WriteAllText(Email.path + EmailConstant.AllEmail, String.Empty);
                File.Copy(Email.path + TargetEmail, Email.path + EmailConstant.AllEmail, true);

                using (StreamReader sr = File.OpenText(Email.path + EmailConstant.AllEmail))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        var start = line.IndexOf("(");
                        var end = line.IndexOf(")");
                        var emailAddress = line.Substring(0, start);
                        var emailFilename = line.Substring(start + 1, end - start - 1) + ".txt";

                        Email.AddtoTextfile(Email.path + emailFilename, emailAddress);
                    }
                }

                ResetSuccessUserControl resetSuccessUserControl = new ResetSuccessUserControl();
                this.Parent.Parent.Controls.Add(resetSuccessUserControl);
                resetSuccessUserControl.Size = new Size(300, 150);
                resetSuccessUserControl.Location = new Point(200, 250);
                resetSuccessUserControl.BringToFront();

                Console.WriteLine("Reset Copy");
            }

            if (isFirstCopy)
            {
                isFirstCopy = false;
                File.Copy(Email.path + TargetEmail, Email.path + EmailConstant.AllEmail, true);
                Console.WriteLine("First Copy");
            }

            this.Visible = false;
            
        }
    }
}
