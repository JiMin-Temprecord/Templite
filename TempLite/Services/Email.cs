using Microsoft.Office.Interop.Outlook;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TempLite.Constant;
using UserControls;
using Application = Microsoft.Office.Interop.Outlook.Application;

namespace TempLite
{
    public class Email
    {
        public static List<ListEmailUserControl> emailList { get; set; } = new List<ListEmailUserControl>();

        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Temprecord\\TempLite\\Email\\";

        public void OpenApplication(string serialNumber, string onwerID, int file = 2)
        {
            var saveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Temprecord\\TempLite\\";
            var PDF = saveFilePath + serialNumber + ".pdf";
            var EXCEL = saveFilePath + serialNumber + ".xlsx";
            var emailSubject = "Temprecord Logger " + serialNumber;

            try
            {
                var outlookApp = new Application();
                var outlookMail = outlookApp.CreateItem(OlItemType.olMailItem);
                outlookMail.Subject = emailSubject;

                if (file == 0)
                    outlookMail.Attachments.Add(PDF);
                else if (file == 1)
                    outlookMail.Attachments.Add(EXCEL);
                else if (file == 2)
                {
                    outlookMail.Attachments.Add(PDF);
                    outlookMail.Attachments.Add(EXCEL);
                }
                outlookMail.Display(true);
            }
            catch
            {
                MessageBox.Show("Unable to Detect Outlook. Please download Outlook and try again.");
            }
        }
        public void SendAutomatically(string serialNumber, string ownerID, int file = 2)
        {
            var message = SetUp(serialNumber, ownerID, file);

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Timeout = 10000;
                client.Connect("smtp.siteprotect.com", 587, false);
                client.Authenticate("temprecordapp@temprecord.com", "bEw7a!EtYRv@z6Q");
                client.Send(message);
                client.Disconnect(true);
            }
        }
        MimeMessage SetUp(string serialNumber, string ownerID, int file = 2)
        {
            var message = new MimeMessage();
            var builder = new BodyBuilder();
            var emailSubject = "Temprecord Logger " + serialNumber;
            
            message.From.Add(new MailboxAddress(emailSubject, "temprecordapp@temprecord.com"));
            message.Subject = emailSubject;
            
            message = AddSendto(message, ownerID);
            message = AddAttachments(message, builder, serialNumber, file);
            message.Body = builder.ToMessageBody();

            return message;
        }
        MimeMessage AddSendto( MimeMessage message, string ownerID)
        {
            string line;

            using (StreamReader sr = File.OpenText(path + ownerID + ".txt"))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    message.To.Add(new MailboxAddress(line));
                }
            }

            return message;
        }
        MimeMessage AddAttachments(MimeMessage message, BodyBuilder builder, string serialNumber, int file)
        {
            var PDF = Path.GetTempPath() + serialNumber + ".pdf";
            var EXCEL = Path.GetTempPath() + serialNumber + ".xlsx";

            if (file == 2)
            {
                if (File.Exists(PDF) && (File.Exists(EXCEL)))
                {
                    builder.Attachments.Add(PDF);
                    builder.Attachments.Add(EXCEL);

                }
            }

            else if (file == 1)
            {
                if (File.Exists(EXCEL))
                    builder.Attachments.Add(EXCEL);
            }

            else if (file == 0)
            {
                if (File.Exists(PDF))
                    builder.Attachments.Add(PDF);
            }
            return message;
        }
        public int Count(string ownerID)
        {
            string line;
            int count = 0;
            var filename = path + ownerID + ".txt";
            if (File.Exists(filename))
            {
                using (StreamReader sr = File.OpenText(filename))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        public string GetHiddenEmailAddress (string ownerID)
        {
            using (StreamReader sr = File.OpenText(path + ownerID + ".txt"))
            {
                var firstEmail = sr.ReadLine();
                var start = firstEmail.IndexOf("@");
                var hiddenEmail = "******" + firstEmail.Substring(start, firstEmail.Length - start);

                var numberOfEmail = Count(ownerID);
                if (numberOfEmail > 1)
                    hiddenEmail += " and " + (numberOfEmail - 1) + " other(s) ";

                return hiddenEmail;
            }
        }
        public static bool IsValid(string emailAddress)
        {
            try
            {
                new System.Net.Mail.MailAddress(emailAddress);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static void AddtoTextfile(string textFilename, string Email)
        {
            if (!File.Exists(textFilename))
                File.Create(textFilename);

            using (StreamWriter sw = File.AppendText(textFilename))
            {
                sw.WriteLine(Email);
                sw.Close();
            }
        }
        public static bool IsExsist(string textFile, string Email)
        {
            string line;
            using (StreamReader sr = File.OpenText(textFile))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == Email)
                        return true;
                }

                return false;
            }
        }
        public static void Delete(string EmailAddress, bool fromReset = false)
        {
            var start = EmailAddress.IndexOf("(");
            var end = EmailAddress.IndexOf(")");
            var emailAddress = EmailAddress.Substring(0, start);
            var emailFilename = EmailAddress.Substring(start + 1, end - start - 1) + ".txt";

            if (fromReset == false)
            {
                var newLines = File.ReadAllLines(path + EmailConstant.AllEmail).Where(line => line != EmailAddress).ToArray();
                File.WriteAllLines(path + EmailConstant.AllEmail, newLines);
            }

            if (File.Exists(path + emailFilename))
            {
                var newEmailList = File.ReadAllLines(path + emailFilename).Where(line => line != emailAddress).ToArray();
                File.WriteAllLines(path + emailFilename, newEmailList);
            }
        }
    }
}
