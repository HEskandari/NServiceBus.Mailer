using System.Net.Mail;

namespace NServiceBusMail
{
    static class MailMessageConverter
    {
        public static System.Net.Mail.MailMessage ToMailMessage(this MailMessage mail)
        {
            var message = new System.Net.Mail.MailMessage
                {
                    DeliveryNotificationOptions = mail.DeliveryNotificationOptions,
                    IsBodyHtml = mail.IsBodyHtml,
                    Priority = mail.Priority,
                    Body = mail.Body,
                    Subject = mail.Subject,
                    BodyEncoding = mail.BodyEncoding,
                    SubjectEncoding = mail.SubjectEncoding,
                    HeadersEncoding = mail.HeadersEncoding
                };

            if (mail.From != null)
                message.From = new MailAddress(mail.From);

            mail.To.ForEach(a => message.To.Add(new MailAddress(a)));
            mail.ReplyToList.ForEach(a => message.ReplyToList.Add(new MailAddress(a)));
            mail.Bcc.ForEach(a => message.Bcc.Add(new MailAddress(a)));
            mail.Cc.ForEach(a => message.CC.Add(new MailAddress(a)));
            if (mail.Sender != null)
                message.Sender = new MailAddress(mail.Sender);

            foreach (var header in mail.Headers)
            {
                message.Headers[header.Key] = header.Value;
            }

            return message;
        }
    }
}