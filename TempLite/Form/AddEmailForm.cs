using System;
using System.Drawing;
using System.IO;
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
            this.Focus();
        }
        
        public void AddEmailButton_Click(object sender, EventArgs e)
        {
            var ownerID = loggerIDTextbox.Text.ToUpper(); //unless we will in the future have case sensitive ids
            var emailAddress = emailTextbox.Text;
            var confirmEmailAddress = confirmEmailTextbox.Text;
            var textFile = Email.path + ownerID + ".txt";
            var isEmailValid = Email.IsValid(emailAddress);

            if (emailAddress == EmailConstant.EmailText && confirmEmailAddress == EmailConstant.ConfirmEmailText)
            {
                Log.Write(LogConstant.AddEmailThrewError);
                changePromptMessage(LogConstant.FieldsEmpty, Color.Tomato);
            }
            else if (emailTextbox.Text == string.Empty || confirmEmailTextbox.Text == string.Empty)
            {
                changePromptMessage(LogConstant.FieldsEmpty, Color.Tomato);
            }
            else if (emailAddress.Equals(confirmEmailAddress) == false)
            {
                changePromptMessage(LogConstant.EmailDoNotMatch, Color.Tomato);
            }
            else if (isEmailValid == false)
            {
                changePromptMessage(LogConstant.InvalidEmail, Color.Tomato);

            }
            else if (File.Exists(textFile) && Email.IsExsist(textFile, emailAddress))
            {
                changePromptMessage(LogConstant.EmailAlreadyExists, Color.Orange);
            }
            else
            {
                Email.AddtoTextfile(textFile, emailAddress);
                Email.AddtoTextfile(Email.path + EmailConstant.AllEmail, emailAddress + "(" + ownerID + ")");
                Log.Write(LogConstant.EmailAddressAdded);
                changePromptMessage(LogConstant.EmailAddressAdded, Color.Green);
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

        void changePromptMessage (string message, Color color)
        {
            promptMessage.Text = message;
            promptMessage.ForeColor = color;
        }
    }
}
