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
        var endpointConfiguration = new EndpointConfiguration("NServiceBusMailSample");

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var mailerSettings = endpointConfiguration.EnableMailer();
        mailerSettings.UseSmtpBuilder<ToDirectorySmtpBuilder>();
        mailerSettings.UseAttachmentFinder<AttachmentFinder>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        try
        {
            while (true)
            {
                Console.WriteLine("Press [S] to send an email message or [Esc] key to exit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key == ConsoleKey.S)
                {
                    var message = new MyMessage();
                    await endpointInstance.SendLocal(message)
                        .ConfigureAwait(false);
                }

                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}