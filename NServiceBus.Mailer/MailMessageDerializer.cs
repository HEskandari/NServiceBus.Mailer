using System.IO;
using System.Linq;
using NServiceBus.Serialization;

namespace NServiceBus.Mailer
{
    static class MailMessageDeserializer
    {

        public static MailMessage DeserializeMessage(this IMessageSerializer messageSerializer, TransportMessage message)
        {
            using (var stream = new MemoryStream(message.Body))
            {
                return (MailMessage) messageSerializer.Deserialize(stream, new[] {typeof (MailMessage)}).Single();
            }
        }
    }
}