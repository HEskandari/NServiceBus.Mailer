using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Mailer;

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var mail = new Mail
        {
            To = "to@fake.email",
            From = "from@fake.email",
            Body = "This is the body",
            Subject = "Hello",
            AttachmentContext = {
                {"Id", "fakeEmail"}
            }
        };

        var messageId = Guid.Parse(context.MessageId);
        mail.SetMessageId(messageId, "microsoft.com");

        await context.SendMail(mail);
        log.Info($"Mail sent and written to {Program.DirectoryLocation}");
    }
}