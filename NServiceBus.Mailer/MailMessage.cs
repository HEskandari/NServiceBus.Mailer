using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace NServiceBus.Mailer
{
    class MailMessage : IMessage
    {
        public List<string> To { get; set; }
        public List<string> Bcc { get; set; }
        public List<string> Cc { get; set; }
        public List<string> ReplyTo { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public Encoding BodyEncoding { get; set; }
        public DeliveryNotificationOptions DeliveryNotificationOptions { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Encoding HeadersEncoding { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public Encoding SubjectEncoding { get; set; }
        public MailPriority Priority { get; set; }
        public Dictionary<string, string> AttachmentContext { get; set; }

        public List<AlternateView> AlternateViews { get; set; }
    }
}