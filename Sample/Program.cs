using System;
using System.Collections.Generic;
using NLog.Targets;
using NServiceBus;
using NServiceBus.Installation.Environments;
using NServiceBus.Mailer;

class Program
{
    static void Main()
    {
        Configure.GetEndpointNameAction = () => "NServiceBusMailSample";

        var consoleTarget = new ConsoleTarget();
        var configure = Configure
            .With().DefaultBuilder();

        configure
            .Configurer
            .ConfigureComponent<ISmtpBuilder>(_ => new ToDirectorySmtpBuilder(), DependencyLifecycle.SingleInstance);
        configure
            .Configurer
            .ConfigureComponent<IAttachmentFinder>(_ => new AttachmentFinder(), DependencyLifecycle.SingleInstance);

        Configure.Serialization.Json();
        configure.NLog(consoleTarget);
        configure.UseTransport<Msmq>();
        configure.UnicastBus();
        var bus = configure
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());
        var mail = new Mail
            {
                To = "to@fake.email",
                From = "from@fake.email",
                Body = "This is the body",
                Subject = "Hello",
                AttachmentContext = new Dictionary<string, string>{{"Id","fakeEmail"}}
            };
        bus.SendMail(mail);
        Console.ReadLine();
    }
}