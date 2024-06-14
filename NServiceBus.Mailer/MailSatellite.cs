using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.Routing;
using NServiceBus.Serialization;
using NServiceBus.Transport;

namespace NServiceBus.Mailer
{
    class MailSatellite
    {
        FindAttachments findAttachments;
        CleanAttachments cleanAttachments;
        BuildSmtpClient buildSmtpClient;
        IMessageSerializer serializer;
        IMessageDispatcher dispatchMessages;

        public MailSatellite(FindAttachments findAttachments, CleanAttachments cleanAttachments, BuildSmtpClient buildSmtpClient, IMessageSerializer serializer)
        {
            this.findAttachments = findAttachments;
            this.cleanAttachments = cleanAttachments;
            this.buildSmtpClient = buildSmtpClient;
            this.serializer = serializer;
        }

        public async Task OnMessageReceived(IServiceProvider serviceProvider, MessageContext messageContext, CancellationToken cancellationToken)
        {
            if (dispatchMessages == null)
            {
                dispatchMessages = serviceProvider.GetRequiredService<IMessageDispatcher>();
            }
            var sendEmail = serializer.DeserializeMessage(messageContext);

            using (var smtpClient = buildSmtpClient())
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
            var serializedMessage = Serialize(newMessage);
            var operation = new TransportOperations(
                    new TransportOperation(
                        message: new OutgoingMessage(messageContext.NativeMessageId, messageContext.Headers, serializedMessage),
                        addressTag: new UnicastAddressTag("Mail")));
            return dispatchMessages.Dispatch(operation, new TransportTransaction());
        }

        static DateTimeOffset TimeSent(MessageContext context)
        {
            return DateTimeOffsetHelper.ToDateTimeOffset(context.Headers[Headers.TimeSent]);
        }

        Task CleanAttachments(MailMessage sendEmail)
        {
            if (cleanAttachments == null || sendEmail.AttachmentContext == null)
            {
                return Task.FromResult(0);
            }
            return cleanAttachments(sendEmail.AttachmentContext);
        }

        async Task AddAttachments(MailMessage sendEmail, System.Net.Mail.MailMessage mailMessage)
        {
            if (findAttachments == null)
            {
                return;
            }
            if (sendEmail.AttachmentContext == null)
            {
                return;
            }
            foreach (var attachment in await findAttachments(sendEmail.AttachmentContext).ConfigureAwait(false))
            {
                mailMessage.Attachments.Add(attachment);
            }
        }

        byte[] Serialize(MailMessage message)
        {
            using (var memoryStream = new MemoryStream())
            {
                serializer.Serialize(message, memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}