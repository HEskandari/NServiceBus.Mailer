using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using NServiceBus;

namespace NServiceBusMail
{
    class MailMessage:IMessage
    {
        public List<string> Bcc;
        public string Body;
        public List<string> Cc;
        public Encoding BodyEncoding;
        public DeliveryNotificationOptions DeliveryNotificationOptions;
        public string From;
        public Dictionary<string, string> Headers;
        public Encoding HeadersEncoding;
        public bool IsBodyHtml;
        public List<string> ReplyToList;
        public string Sender;
        public string Subject;
        public Encoding SubjectEncoding;
        public List<string> To;
        public MailPriority Priority;
    }
}