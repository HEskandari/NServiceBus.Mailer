using System.IO;
using System.Linq;
using NServiceBus.Mailer;
using NServiceBus.Serialization;
using NServiceBus.Transport;

static class MailMessageDeserializer
{
    public static MailMessage DeserializeMessage(this IMessageSerializer messageSerializer, MessageContext context)
    {
        using (var stream = new MemoryStream(context.Body))
        {
            return (MailMessage)messageSerializer.Deserialize(stream, new[] { typeof(MailMessage) }).Single();
        }
    }
}