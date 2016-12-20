using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.MessageInterfaces;
using NServiceBus.Routing;
using NServiceBus.Serialization;
using NServiceBus.Settings;
using NServiceBus.Transport;

namespace NServiceBus.Mailer
{
    class MailSatellite
    {
        public ISmtpBuilder SmtpBuilder;
        public IMessageMapper MessageMapper;
        public IAttachmentFinder AttachmentFinder;
        public ReadOnlySettings Settings;
        public IDispatchMessages DispatchMessages;

        public async Task OnMessageReceived(MessageContext messageContext)
        {
            var serializer = GetDefaultSerializer();
            var sendEmail = serializer.DeserializeMessage(messageContext);

            using (var smtpClient = SmtpBuilder.BuildClient())
            using (var mailMessage = sendEmail.ToMailMessage())
            {
                await AddAttachments(sendEmail, mailMessage);
                try
                {
                    await smtpClient.SendMailAsync(mailMessage)
                        .ConfigureAwait(false);
                    await CleanAttachments(sendEmail)
                        .ConfigureAwait(false);
                }
                catch (SmtpFailedRecipientsException exception)
                {
                    //TODO: should put some delay in here to back off from an overloaded smtp server
                    var originalRecipientCount = mailMessage.To.Count + mailMessage.Bcc.Count + mailMessage.CC.Count;
                    if (exception.InnerExceptions.Length == originalRecipientCount)
                    {
                        //All messages failed. So safe to throw and cause a re-handle of the message
                        throw;
                    }
                    var timeSent = TimeSent(messageContext);

                    var retries = RetryMessageBuilder.GetMessagesToRetry(sendEmail, timeSent, exception)
                        .Select(newMessage => DispatchMailMessage(messageContext, newMessage));
                    await Task.WhenAll(retries);
                }
            }
        }

        Task DispatchMailMessage(MessageContext messageContext, MailMessage newMessage)
        {
            var msg = Serialize(newMessage);
            var operation =
                new TransportOperations(
                    new TransportOperation(new OutgoingMessage(messageContext.MessageId, messageContext.Headers, msg),
                        new UnicastAddressTag("Mail")));
            return DispatchMessages.Dispatch(operation, new TransportTransaction(), new ContextBag());
        }

        IMessageSerializer GetDefaultSerializer()
        {
            var mainSerializer = Settings.Get<Tuple<SerializationDefinition, SettingsHolder>>("MainSerializer");
            var serializerFactory = mainSerializer.Item1.Configure(Settings);
            return serializerFactory(MessageMapper);
        }

        static DateTime TimeSent(MessageContext context)
        {
            return DateTimeExtensions.ToUtcDateTime(context.Headers[Headers.TimeSent]);
        }

        Task CleanAttachments(MailMessage sendEmail)
        {
            if (AttachmentFinder == null || sendEmail.AttachmentContext == null)
            {
                return Task.FromResult(0);
            }
            return AttachmentFinder.CleanAttachments(sendEmail.AttachmentContext);
        }

        async Task AddAttachments(MailMessage sendEmail, System.Net.Mail.MailMessage mailMessage)
        {
            if (AttachmentFinder == null)
            {
                return;
            }
            if (sendEmail.AttachmentContext == null)
            {
                return;
            }
            foreach (var attachment in await AttachmentFinder.FindAttachments(sendEmail.AttachmentContext).ConfigureAwait(false))
            {
                mailMessage.Attachments.Add(attachment);
            }
        }

        byte[] Serialize(MailMessage message)
        {
            var serializer = GetDefaultSerializer();

            using (var memoryStream = new MemoryStream())
            {
                serializer.Serialize(message, memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}