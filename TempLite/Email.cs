using MimeKit;

namespace TempLite
{
    public class Email
    {
        public void SetUpEmail(string serialNumber)
        {
            /*SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("jiminjang96@gmail.com", "Jimin0316");

            MailMessage mail = new MailMessage("jiminjang96@gmail.com", "jimin@temprecord.com");
            mail.Subject = "Temprecod Logger Read";
            mail.Attachments.Add(new Attachment(serialNumber+".pdf"));
            mail.Attachments.Add(new Attachment(serialNumber+".xls"));
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(mail);*/

            var message = new MimeMessage();
            var builder = new BodyBuilder();

            message.From.Add(new MailboxAddress("Temprecord Logger Read", "temprecordapp@temprecord.com"));
            message.To.Add(new MailboxAddress("jimin@temprecord.com"));
            message.Subject = "Temprecord Logger Read";
            
            var PDF = serialNumber + ".pdf";
            var EXCEL = serialNumber + ".xls";

            if (System.IO.File.Exists(PDF) && (System.IO.File.Exists(EXCEL)))
            {
                builder.Attachments.Add(PDF);
                builder.Attachments.Add(EXCEL);
            }

            message.Body = builder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Timeout = 10000;
                client.Connect("smtp.siteprotect.com", 587, false);
                client.Authenticate("temprecordapp@temprecord.com", "bEw7a!EtYRv@z6Q");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
