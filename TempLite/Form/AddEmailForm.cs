using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TempLite.Constant;
using TempLite.Services;

namespace TempLite
{
    public partial class AddEmailForm : Form
    {
        public AddEmailForm()
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
                            promptMessage.ForeColor = Color.Tomato;
                            promptMessage.Text = LogConstant.InvalidEmail;
                        }
                    }
                    else
                    {
                        promptMessage.ForeColor = Color.Tomato;
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

        string firstKeyDown = string.Empty;
        string secondKeyDown = string.Empty;

        private void AddEmailForm_KeyUp(object sender, KeyEventArgs e)
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

            if (e.KeyCode.ToString() == Keys.Return.ToString())
            {
                AddEmailButton_Click(sender, e);
            }
            else if (firstKeyDown == Keys.ControlKey.ToString() && secondKeyDown == Keys.ShiftKey.ToString() && e.KeyCode.ToString() == Keys.A.ToString())
            {
                this.Dispose();
            }

        }

        private void AddEmailForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (firstKeyDown == string.Empty && secondKeyDown == string.Empty)
                firstKeyDown = e.KeyCode.ToString();

            else if (secondKeyDown == string.Empty)
                secondKeyDown = e.KeyCode.ToString();
        }
    }
}
