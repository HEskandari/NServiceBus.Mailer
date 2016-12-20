using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Mailer;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        var configuration = new EndpointConfiguration("NServiceBusMailSample");

        configuration.UseSerialization<JsonSerializer>();
        configuration.UsePersistence<InMemoryPersistence>();
        configuration.EnableInstallers();

        var mailerSettings = configuration.EnableMailer();
        mailerSettings.UseSmtpBuilder<ToDirectorySmtpBuilder>();
        mailerSettings.UseAttachmentFinder<AttachmentFinder>();

        var endpointInstance = await Endpoint.Start(configuration).ConfigureAwait(false);

        try
        {
            while (true)
            {
                Console.WriteLine("Press [S] to send an email message or [Esc] key to exit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key == ConsoleKey.S)
                {
                    await endpointInstance.SendLocal(new MyMessage());
                }

                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
        finally
        {
            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}