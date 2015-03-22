namespace NServiceBus.Mailer
{
    using System.Net.Mail;
    using System.Net.Mime;
    using SystemAlternateView = System.Net.Mail.AlternateView;
    using SystemMailMessage = System.Net.Mail.MailMessage;

    static class MessageConverter
    {
        public static MailMessage ToMailMessage(this Mail mail)
        {
            return new MailMessage
            {
                Bcc = mail.Bcc,
                Body = mail.Body,
                BodyEncoding = mail.BodyEncoding,
                Cc = mail.Cc,
                DeliveryNotificationOptions = mail.DeliveryNotificationOptions,
                From = mail.From,
                Headers = mail.Headers,
                HeadersEncoding = mail.HeadersEncoding,
                IsBodyHtml = mail.IsBodyHtml,
                Priority = mail.Priority,
                ReplyTo = mail.ReplyTo,
                Sender = mail.Sender,
                Subject = mail.Subject,
                SubjectEncoding = mail.SubjectEncoding,
                To = mail.To,
                AttachmentContext = mail.AttachmentContext,
                AlternateViews = mail.AlternateViews,
            };
        }

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
                HeadersEncoding = mail.HeadersEncoding,
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