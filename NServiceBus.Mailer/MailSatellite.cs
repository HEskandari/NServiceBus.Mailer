using System;
using System.Net.Mail;
using NServiceBus.Satellites;
using NServiceBus.Serialization;

namespace NServiceBus.Mailer
{
    class MailSatellite:ISatellite
    {
        public Address InputAddress { get; set; }
        public bool Disabled => false;
        public string EndpointName;
        public ISmtpBuilder SmtpBuilder;
        public IMessageSerializer MessageSerializer;
        public IAttachmentFinder AttachmentFinder;
        public IBus Bus;

        public bool Handle(TransportMessage transportMessage)
        {
            var sendEmail = MessageSerializer.DeserializeMessage(transportMessage);
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
                    var timeSent = TimeSent(transportMessage);

                    foreach (var newMessage in RetryMessageBuilder.GetMessagesToRetry(sendEmail,timeSent, ex))
                    {
                        Bus.Send(InputAddress, newMessage);
                    }
                }
            }
            return true;
        }

        static DateTime TimeSent(TransportMessage message)
        {
            return DateTimeExtensions.ToUtcDateTime(message.Headers[Headers.TimeSent]);
        }

        void CleanAttachments(MailMessage sendEmail)
        {
            if (AttachmentFinder == null)
            {
                return;
            }
            if (sendEmail.AttachmentContext == null)
            {
                return;
            }
            AttachmentFinder.CleanAttachments(sendEmail.AttachmentContext);
        }

        void AddAttachments(MailMessage sendEmail, System.Net.Mail.MailMessage mailMessage)
        {
            if (AttachmentFinder == null)
            {
                return;
            }
            if (sendEmail.AttachmentContext == null)
            {
                return;
            }
            foreach (var attachment in AttachmentFinder.FindAttachments(sendEmail.AttachmentContext))
            {
                mailMessage.Attachments.Add(attachment);
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

    }
}