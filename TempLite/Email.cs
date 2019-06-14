using MimeKit;
using Microsoft.Office.Interop.Outlook;
using System.IO;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Outlook.Application;

namespace TempLite
{
    public class Email
    {
        public void OpenEmailApplication(string serialNumber, string emailID, int file = 2)
        {
            var PDF = Path.GetTempPath() + serialNumber + ".pdf";
            var EXCEL = Path.GetTempPath() + serialNumber + ".xlsx";
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
                else
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

        public void SendEmailAutomatically(string serialNumber, string emailID, int file = 2)
        {
            var message = SetUpEmail(serialNumber, emailID, file);

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Timeout = 10000;
                client.Connect("smtp.siteprotect.com", 587, false);
                client.Authenticate("temprecordapp@temprecord.com", "bEw7a!EtYRv@z6Q");
                client.Send(message);
                client.Disconnect(true);
            }
        }
        MimeMessage SetUpEmail(string serialNumber, string emailID, int file = 2)
        {
            var message = new MimeMessage();
            var builder = new BodyBuilder();
            var emailSubject = "Temprecord Logger " + serialNumber;

            var PDF = Path.GetTempPath() + serialNumber + ".pdf";
            var EXCEL = Path.GetTempPath() + serialNumber + ".xlsx";

            message.From.Add(new MailboxAddress(emailSubject, "temprecordapp@temprecord.com"));
            message.Subject = emailSubject;


            if (file == 2)
            {
                var emailTo = GetSenderEmail(emailID);
                message.To.Add(new MailboxAddress(emailTo));

                if (File.Exists(PDF) && (File.Exists(EXCEL)))
                {
                    builder.Attachments.Add(PDF);
                    builder.Attachments.Add(EXCEL);

                }
            }

            else if (file == 1)
            {
                string line;

                using (StreamReader sr = File.OpenText(emailID + ".txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        message.To.Add(new MailboxAddress(line));
                    }
                }

                if (File.Exists(EXCEL))
                builder.Attachments.Add(EXCEL);
            }

            else if (file == 0)
            {
                string line;

                using (StreamReader sr = File.OpenText(emailID + ".txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        message.To.Add(new MailboxAddress(line));
                    }
                }

                if (File.Exists(PDF))
                    builder.Attachments.Add(PDF);
            }


            message.Body = builder.ToMessageBody();
            return message;
        }
        
        string GetSenderEmail (string emailID)
        {
            switch (emailID)
            {
                /*case "TBS-NSW":
                    return "NSWDataLoggerCommunication@arcbs.redcross.org.au";
                case "TBS-SA":
                    return "SADataLoggerCommunication@arcbs.redcross.org.au";
                case "TBS-TAS":
                    return "TASDataLoggerCommunication@arcbs.redcross.org.au";
                case "TBS-VIC":
                    return "VICDataLoggerCommunication@arcbs.redcross.org.au";
                case "TBS-QLD":
                    return "QLDDataLoggerCommunication@arcbs.redcross.org.au";
                case "TBS-WA":
                    return "WADataLoggerCommunication@arcbs.redcross.org.au";
                case "TBS-NT":
                    return "NSWDataLoggerCommunication@arcbs.redcross.org.au";
                case "TBS-TEST":
                    return "jimin@temprecord.com";
                default:
                    return "jimin@temprecord.com";*/
                case "TBS-NSW":
                    return "jimin@temprecord.com";
                case "TBS-SA":
                    return "jimin@temprecord.com";
                case "TBS-TAS":
                    return "jimin@temprecord.com";
                case "TBS-VIC":
                    return "jimin@temprecord.com";
                case "TBS-QLD":
                    return "jimin@temprecord.com";
                case "TBS-WA":
                    return "jimin@temprecord.com";
                case "TBS-NT":
                    return "jimin@temprecord.com";
                case "TBS-TEST":
                    return "jimin@temprecord.com";
                default:
                    return "jimin@temprecord.com";
            }
        }

        public static bool IsValid(string emailAddress)
        {
            try
            {
                var emailCheck = new System.Net.Mail.MailAddress(emailAddress);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
