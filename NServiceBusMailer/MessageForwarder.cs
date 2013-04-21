using System.Collections.Generic;
using NServiceBus;

namespace NServiceBusMailer
{
    class MessageForwarder
    {
        public IBus Bus;
        public MailMessage OriginalMessage;
        public List<string> FailedRecipients;

        public void SendToFailedRecipients()
        {
            var newBody = GetForwardBody(OriginalMessage);
            foreach (var failedRecipient in FailedRecipients)
            {
                ReSend(failedRecipient, newBody);
            }
        }

        void ReSend(string failedRecipient, string newBody)
        {
            var retryMessage = new MailMessage
                {
                    To = new List<string> {failedRecipient},
                    From = OriginalMessage.From,
                    Body = newBody,
                    BodyEncoding = OriginalMessage.BodyEncoding,
                    DeliveryNotificationOptions = OriginalMessage.DeliveryNotificationOptions,
                    Headers = OriginalMessage.Headers,
                    HeadersEncoding = OriginalMessage.HeadersEncoding,
                    IsBodyHtml = OriginalMessage.IsBodyHtml,
                    Priority = OriginalMessage.Priority,
                    ReplyTo = OriginalMessage.ReplyTo,
                    Sender = OriginalMessage.Sender,
                    Subject = OriginalMessage.Subject,
                    SubjectEncoding = OriginalMessage.SubjectEncoding,
                    AttachmentContext = OriginalMessage.AttachmentContext
                };
            var scope = Configure.Instance.GetMasterNodeAddress().SubScope("Mail");
            Bus.Send(scope, retryMessage);
        }

        string GetForwardBody(MailMessage original)
        {
            if (original.IsBodyHtml)
            {
                return GetHtmlPrefix(original);
            }
            return GetTextPrefix(original);
        }

        string GetTextPrefix(MailMessage original)
        {
            return string.Format(
                @"
This message was forwarded due to the original email failing to send
-----Original Message-----
To: {0}
CC: {1}
Sent: {2}

{3}
",
                string.Join(",", original.To),
                string.Join(",", original.Cc),
                Bus.TimeSent().ToString("R"),
                original.Body);
        }

        string GetHtmlPrefix(MailMessage original)
        {
            return string.Format(
                @"
This message was forwarded due to the original email failing to send<br/>
-----Original Message-----<br/>
To: {0}<br/>
CC: {1}<br/>
Sent: {2}<br/><br/>
{3}
",
                string.Join(",", original.To),
                string.Join(",", original.Cc),
                Bus.TimeSent().ToString("R"),
                original.Body);
        }
    }
}