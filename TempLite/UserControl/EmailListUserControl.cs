using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TempLite;
using TempLite.Constant;
using TempLite.Services;

namespace UserControls
{
    public partial class EmailListUserControl : UserControl
    {
        public EmailListUserControl()
        {
            InitializeComponent();
        }

        public void AddEmailButton_Click(object sender, EventArgs e)
        {
            var ownerID = loggerIDTextbox.Text.ToUpper(); //unless we will in the future have case sensitive ids
            var emailAddress = emailTextbox.Text;
            var confirmEmailAddress = confirmEmailTextbox.Text;
            var textFile = Email.path + ownerID + ".txt";

            var isEmailValid = Email.IsValid(emailAddress);

            if (emailTextbox.Text == string.Empty || confirmEmailTextbox.Text == string.Empty)
            {
                promptMessage.Text = LogConstant.FieldsEmpty;
                promptMessage.ForeColor = Color.Red;
            }
            else
            {
                if (emailAddress != EmailConstant.EmailText && confirmEmailAddress != EmailConstant.ConfirmEmailText)
                {
                    if (emailAddress.Equals(confirmEmailAddress))
                    {
                        if (isEmailValid)
                        {
                            if (File.Exists(textFile) && Email.IsExsist(textFile, emailAddress))
                            {
                                promptMessage.Text = LogConstant.EmailAlreadyExists;
                                promptMessage.ForeColor = Color.Orange;
                            }
                            else
                            {
                                Email.AddtoTextfile(textFile, emailAddress);
                                Email.AddtoTextfile(Email.path + EmailConstant.AllEmail, emailAddress + "(" + ownerID + ")");
                                Log.Write(LogConstant.EmailAddressAdded);
                                promptMessage.Text = LogConstant.EmailAddressAdded;
                                promptMessage.ForeColor = Color.Green;
                            }
                        }
                        else
                        {
                            promptMessage.ForeColor = Color.Red;
                            promptMessage.Text = LogConstant.InvalidEmail;
                        }
                    }
                    else
                    {
                        promptMessage.ForeColor = Color.Red;
                        promptMessage.Text = LogConstant.EmailDoNotMatch;
                    }
                }
                else
                {
                    Log.Write(LogConstant.AddEmailThrewError);
                    promptMessage.Text = LogConstant.FieldsEmpty;
                    promptMessage.ForeColor = Color.Red;
                }
            }
        }
        
        public void addEmailtoList()
        {
            string line;
            int y = 45;
            int i = 0;

            using (StreamReader sr = File.OpenText(Email.path + EmailConstant.AllEmail))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    var listEmailUserControl = new ListEmailUserControl();
                    listEmailUserControl.emailLabel.Text = line;
                    listEmailUserControl.Location = new Point(-1, y);
                    listEmailUserControl.SendToBack();
                    emailListPanel.Controls.Add(listEmailUserControl);
                    Email.emailList.Add(listEmailUserControl);
                    y = y + 45;
                    i++;
                }
            }
        }
        public void removeEmailfromList()
        {         
            if (Email.emailList.Count > 0)
            {
                for (int i = 0; i < Email.emailList.Count; i++)
                {
                    emailListPanel.Controls.Remove(Email.emailList[i]);
                }

                Email.emailList.Clear();
            }
        }

        private void EmailListUserControl_VisibleChanged(object sender, EventArgs e)
        {
            removeEmailfromList();
            addEmailtoList();
        }

        private void addEmailPanel_VisibleChanged(object sender, EventArgs e)
        {
            removeEmailfromList();
            addEmailtoList();
        }
    }
}
