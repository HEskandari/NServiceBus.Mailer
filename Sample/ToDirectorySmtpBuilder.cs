using System;
using System.IO;
using System.Net.Mail;
using NServiceBus.Mailer;

public class ToDirectorySmtpBuilder : ISmtpBuilder
{
    public SmtpClient BuildClient()
    {
        Directory.CreateDirectory(DirectoryLocation);
        return new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = DirectoryLocation
            };
    }

    public static string DirectoryLocation = Path.Combine(Environment.CurrentDirectory, "Emails");
}