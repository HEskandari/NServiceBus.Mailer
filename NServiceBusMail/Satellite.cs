using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using NServiceBus;
using NServiceBus.Satellites;
using NServiceBus.Serialization;

namespace NServiceBusMail
{

    class Satellite : ISatellite
    {
        public ISmtpBuilder SmtpBuilder { get; set; }
        public IMessageSerializer MessageSerializer { get; set; }
        public IBus Bus { get; set; }

        public Satellite()
        {
            SmtpBuilder = new DefaultSmtpBuilder();
        }

        public bool Handle(TransportMessage transportMessage)
        {
            var sendEmail = DeserializeMessage(transportMessage);
            using (var smtpClient = SmtpBuilder.BuildClient())
            using (var mailMessage = sendEmail.ToMailMessage())
            {
                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    //TODO: should put some delay in here to back off from an overloaded smtp server
                    var originalRecipientCount = mailMessage.To.Count + mailMessage.Bcc.Count + mailMessage.CC.Count;
                    if (ex.InnerExceptions.Length == originalRecipientCount)
                    {

                        //All messages failed. So safe to throw and cause a re-send
                        throw;
                    }
                    foreach (var recipientException in ex.InnerExceptions)
                    {
                        HandleFailedRecipient(recipientException, sendEmail, GetForwardBody(sendEmail));
                    }
                }
            }
            return true;
        }

        void HandleFailedRecipient(SmtpFailedRecipientException recipientException, MailMessage sendEmail, string newBody)
        {
            var retryMessage = new MailMessage
                {
                    To = new List<string> {recipientException.FailedRecipient},
                    From = sendEmail.From,
                    Body = newBody,
                    BodyEncoding = sendEmail.BodyEncoding,
                    DeliveryNotificationOptions = sendEmail.DeliveryNotificationOptions,
                    Headers = sendEmail.Headers,
                    HeadersEncoding = sendEmail.HeadersEncoding,
                    IsBodyHtml = sendEmail.IsBodyHtml,
                    Priority = sendEmail.Priority,
                    ReplyToList = sendEmail.ReplyToList,
                    Sender = sendEmail.Sender,
                    Subject = sendEmail.Subject,
                    SubjectEncoding = sendEmail.SubjectEncoding,
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


        MailMessage DeserializeMessage(TransportMessage message)
        {
            using (var stream = new MemoryStream(message.Body))
            {
                return (MailMessage) MessageSerializer.Deserialize(stream, new[] {typeof (MailMessage)}).First();
            }
        }

        public Address InputAddress
        {
            get
            {
                var masterNode = Configure.Instance.GetMasterNodeAddress();
                return masterNode.SubScope("Mail");
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public bool Disabled
        {
            get { return false; }
        }
    }
}