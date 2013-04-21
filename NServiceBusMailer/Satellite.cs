using System.IO;
using System.Linq;
using System.Net.Mail;
using NServiceBus;
using NServiceBus.Satellites;
using NServiceBus.Serialization;

namespace NServiceBusMailer
{

    class Satellite : ISatellite
    {
        public ISmtpBuilder SmtpBuilder;
        public IMessageSerializer MessageSerializer;
        public IAttachmentFinder AttachmentFinder;
        public IBus Bus;

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
                AddAttachments(sendEmail, mailMessage);
                try
                {
                    smtpClient.Send(mailMessage);
                    CleanAttachments(sendEmail);
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    //TODO: should put some delay in here to back off from an overloaded smtp server
                    var originalRecipientCount = mailMessage.To.Count + mailMessage.Bcc.Count + mailMessage.CC.Count;
                    if (ex.InnerExceptions.Length == originalRecipientCount)
                    {
                        //All messages failed. So safe to throw and cause a re-handle of the message
                        throw;
                    }
                    var messageForwarder = new MessageForwarder
                        {
                            Bus = Bus, 
                            FailedRecipients = ex.InnerExceptions.Select(x => x.FailedRecipient).ToList(), 
                            OriginalMessage = sendEmail
                        };
                    messageForwarder.SendToFailedRecipients();
                }
            }
            return true;
        }

        void CleanAttachments(MailMessage sendEmail)
        {
            if (AttachmentFinder != null)
            {
                if (sendEmail.AttachmentContext != null)
                {
                    AttachmentFinder.CleanAttachments(sendEmail.AttachmentContext);
                }
            }
        }

        void AddAttachments(MailMessage sendEmail, System.Net.Mail.MailMessage mailMessage)
        {
            if (AttachmentFinder != null)
            {
                if (sendEmail.AttachmentContext != null)
                {
                    foreach (var attachment in AttachmentFinder.FindAttachments(sendEmail.AttachmentContext))
                    {
                        mailMessage.Attachments.Add(attachment);
                    }
                }
            }
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
