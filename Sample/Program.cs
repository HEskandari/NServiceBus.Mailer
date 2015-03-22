using System;
using NServiceBus;
using NServiceBus.Mailer;

class Program
{
    static void Main()
    {
        var configuration = new BusConfiguration();
        configuration.EndpointName("NServiceBusMailSample");

        configuration.UseSerialization<JsonSerializer>();
        configuration.UsePersistence<InMemoryPersistence>();
        configuration.EnableInstallers();

        var mailerSettings = configuration.EnableMailer();
        mailerSettings.UseSmtpBuilder<ToDirectorySmtpBuilder>();
        mailerSettings.UseAttachmentFinder<AttachmentFinder>();

        using (var bus = Bus.Create(configuration))
        {
            bus.Start();

            bus.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.Read();

        }

    }
}