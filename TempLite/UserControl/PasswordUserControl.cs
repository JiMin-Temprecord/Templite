﻿using System;
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
            }
                ;
        }
    }
}
