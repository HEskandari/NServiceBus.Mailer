using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Mailer;
// ReSharper disable ConvertToLambdaExpression
// ReSharper disable UnusedVariable

class Program
{
    static async Task Main()
    {
        var endpointConfiguration = new EndpointConfiguration("NServiceBusMailSample");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UsePersistence<NonDurablePersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        endpointConfiguration.Recoverability().AddUnrecoverableException(typeof(ArgumentException));

        var mailerOptions = endpointConfiguration.EnableMailer("mydomain.com");
        mailerOptions.SmtpClientBuilder = () =>
        {
            Directory.CreateDirectory(DirectoryLocation);
            return new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = DirectoryLocation
            };
        };
        mailerOptions.AttachmentsFinder = async attachmentContext =>
            {
                var id = attachmentContext["Id"];
                var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes("Hello"));
                var attachment = new Attachment(memoryStream, "example.txt", "text/plain");
                var attachments = new List<Attachment> { attachment };
                return attachments;
            };

        mailerOptions.AttachmentCleaner = attachmentContext =>
            {
                // Attachment cleanup can be performed here
                return Task.FromResult(0);
            };

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

    public static readonly string DirectoryLocation = Path.Combine(Environment.CurrentDirectory, "Emails");
}