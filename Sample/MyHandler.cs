using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Mailer;

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();
    MailSender mailSender;

    public MyHandler(MailSender mailSender)
    {
        this.mailSender = mailSender;
    }

    public void Handle(MyMessage message)
    {
        var mail = new Mail
                   {
                       To = "to@fake.email",
                       From = "from@fake.email",
                       Body = "This is the body",
                       Subject = "Hello",
                       AttachmentContext = new Dictionary<string, string> { { "Id", "fakeEmail" } }
                   };
        mailSender.SendMail(mail);
        log.Info("Mail sent and written to " +ToDirectorySmtpBuilder.DirectoryLocation);
    }
}