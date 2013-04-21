using System;
using System.IO;
using System.Net.Mail;
using NServiceBusMailer;

public class ToDirectorySmtpBuilder : ISmtpBuilder
{
    public SmtpClient BuildClient()
    {
        var directoryLocation = Path.Combine(Environment.CurrentDirectory, "Emails");
        Directory.CreateDirectory(directoryLocation);
        return new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = directoryLocation
            };
    }
}