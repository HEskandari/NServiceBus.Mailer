using System.Linq;
using NServiceBus.Mailer;
using NServiceBus.Serialization;
using NServiceBus.Transport;

static class MailMessageDeserializer
{
    public static MailMessage DeserializeMessage(this IMessageSerializer messageSerializer, MessageContext context)
    {
        return (MailMessage)messageSerializer.Deserialize(context.Body, new[] { typeof(MailMessage) }).Single();
    }
}