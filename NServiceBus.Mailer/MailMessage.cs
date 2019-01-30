using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Tests")]
namespace NServiceBus.Mailer
{
    class MailMessage : IMessage
    {
        public List<string> To;
        public List<string> Bcc;
        public List<string> Cc;
        public List<string> ReplyTo;
        public string From;
        public string Body;
        public Encoding BodyEncoding;
        public DeliveryNotificationOptions DeliveryNotificationOptions;
        public Dictionary<string, string> Headers;
        public Encoding HeadersEncoding;
        public bool IsBodyHtml;
        public string Sender;
        public string Subject;
        public Encoding SubjectEncoding;
        public MailPriority Priority;
        public Dictionary<string, string> AttachmentContext;

        public List<AlternateView> AlternateViews;
    }
}