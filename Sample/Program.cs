using System;
using NLog.Targets;
using NServiceBus;
using NServiceBus.Installation.Environments;
using NServiceBusMail;

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
        configure.JsonSerializer();
        configure.NLog(consoleTarget);
        configure.UseTransport<Msmq>();
        configure.UnicastBus();
        var bus = configure
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());
        var mail = new Mail
            {
                To = "to@fake.com",
                From = "from@fake.com"
            };
        bus.SendMail(mail);
        Console.ReadLine();
    }
}