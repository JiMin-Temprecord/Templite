using System.Net.Mail;
using System.Text;

namespace TempLite
{
    public class Email
    {
        public void SetUpEmail(string serialNumber)
        {
            SmtpClient client = new SmtpClient();
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
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(mail);
        }
    }
}
