using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TempLite.Constant;

namespace TempLite
{
    public partial class KeycodeInputForm : Form
    {
        public bool isFirstCopy = true;
        public bool isReset = false;

        public KeycodeInputForm()
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

        void CopyEmailList(string TargetEmail)
        {
            this.Visible = false;
            promptMessage.Text = string.Empty;
            DialogResult = DialogResult.OK;

            if (isReset)
            {
                DeleteAllEmailsfromList();
                File.WriteAllText(Email.path + EmailConstant.AllEmail, String.Empty);
                File.Copy(Email.path + TargetEmail, Email.path + EmailConstant.AllEmail, true);
                AddDefaultEmailstoList();
            }

            if (isFirstCopy)
            {
                isFirstCopy = false;
                File.Copy(Email.path + TargetEmail, Email.path + EmailConstant.AllEmail, true);
            }
        }

        void DeleteAllEmailsfromList()
        {
            string line;
            using (StreamReader sr = File.OpenText(Email.path + EmailConstant.AllEmail))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    Email.Delete(line, true);
                }
            }
        }

        void AddDefaultEmailstoList()
        {
            string line = string.Empty;

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
            };
        }

        #region Key Events
        string firstKeyDown = string.Empty;
        string secondKeyDown = string.Empty;

        void KeycodeInputForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (firstKeyDown == string.Empty && secondKeyDown == string.Empty)
                firstKeyDown = e.KeyCode.ToString();

            else if (secondKeyDown == string.Empty)
                secondKeyDown = e.KeyCode.ToString();
        }

        void KeycodeInputForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == firstKeyDown)
            {
                firstKeyDown = string.Empty;
                secondKeyDown = string.Empty;
            }

            else if (e.KeyCode.ToString() == secondKeyDown)
            {
                secondKeyDown = string.Empty;
            }

            if (!isReset && firstKeyDown == Keys.ControlKey.ToString() && secondKeyDown == Keys.ShiftKey.ToString() && e.KeyCode.ToString() == Keys.E.ToString())
            {
                DialogResult = DialogResult.Cancel;
            }
            else if (isReset && firstKeyDown == Keys.ControlKey.ToString() && secondKeyDown == Keys.ShiftKey.ToString() && e.KeyCode.ToString() == Keys.R.ToString())
            {
                DialogResult = DialogResult.Cancel;
            }

        }
        #endregion
    }
}
