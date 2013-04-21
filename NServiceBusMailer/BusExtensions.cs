using System;
using NServiceBus;

namespace NServiceBusMailer
{
    public static class BusExtensions
    {
        /// <summary>
        /// Sends the specified <see cref="NServiceBusMail.Mail"/> via the <see cref="IBus"/> to an SMTP server for delivery.
        /// </summary>
        /// <param name="bus">The <see cref="IBus"/> that is sending the message.</param>
        /// <param name="mail">The <see cref="NServiceBusMail.Mail"/> to send.</param>
        public static void SendMail(this IBus bus, Mail mail)
        {
            if (bus == null)
            {
                throw new ArgumentNullException("bus");
            }
            if (mail == null)
            {
                throw new ArgumentNullException("mail");
            }
            mail.ValidateMail();
            var message = new MailMessage
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
                };
            var scope = Configure.Instance.GetMasterNodeAddress().SubScope("Mail");
            bus.Send(scope, message);
        }


        internal static DateTime TimeSent(this IBus bus)
        {
            return DateTimeExtensions.ToUtcDateTime(bus.CurrentMessageContext.Headers[Headers.TimeSent]);
        }
    }
}
