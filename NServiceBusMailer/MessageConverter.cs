using System.Net.Mail;
using System.Net.Mime;
using SystemAlternateView = System.Net.Mail.AlternateView;
using SystemMailMessage = System.Net.Mail.MailMessage;

namespace NServiceBusMailer
{
    static class MessageConverter
    {
        public static SystemMailMessage ToMailMessage(this MailMessage mail)
        {
            var message = new SystemMailMessage
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
            {
                message.From = new MailAddress(mail.From);
            }
            
            if (mail.Sender != null)
            {
                message.Sender = new MailAddress(mail.Sender);
            }

            foreach (var alternateView in mail.AlternateViews)
            {
                var mimeType = new ContentType(alternateView.ContentType);
                var systemAlternateView = SystemAlternateView.CreateAlternateViewFromString(alternateView.Content, mimeType);
                message.AlternateViews.Add(systemAlternateView);
            }

            mail.To.ForEach(a => message.To.Add(new MailAddress(a)));
            mail.ReplyTo.ForEach(a => message.ReplyToList.Add(new MailAddress(a)));
            mail.Bcc.ForEach(a => message.Bcc.Add(new MailAddress(a)));
            mail.Cc.ForEach(a => message.CC.Add(new MailAddress(a)));

            foreach (var header in mail.Headers)
            {
                message.Headers[header.Key] = header.Value;
            }

            return message;
        }
    }
}