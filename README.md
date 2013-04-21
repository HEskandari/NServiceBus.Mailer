NServiceBusMailer
===============

## Nuget

https://nuget.org/packages/NServiceBusMailer/
    
    PM> Install-Package NServiceBusMailer -Pre

## Usage 
     
    using NServiceBusMailer; 
    var mail = new Mail
            {
                To = "to@fake.email",
                From = "from@fake.email",
                Body = "This is the body",
                Subject = "Hello",
    bus.SendMail(mail);

## SmtpClient construction 

The interface `ISmtpBuilder` can be used to control how an `SmtpClient` is constructed.

By default the  `DefaultSmtpBuilder` will be used. This effectively uses the code

    return new SmtpClient
        {
            EnableSsl = true
        };
       
This results in the `SmtpClient` defaulting to reading its settings from the application config.

To create a custom `ISmtpBuilder` take the following actions.

### Create a class inheriting from `ISmtpBuilder` 

To have your own custom `SmtpClient` simply inherit from `ISmtpBuilder`. 

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
    
### Inject ISmtpBuilder into Container

Then configure the instance to be injected into the NSerivceBus Container.

        configure
            .Configurer
            .ConfigureComponent<ISmtpBuilder>(_ => new ToDirectorySmtpBuilder(), DependencyLifecycle.SingleInstance);
            
## Attachments

Since it is not practical to send binary data as part of messages there is an alternative mechanism. The interface `IAttachmentFinder` can be used to defer attachment creation until the point of the email being sent.

### Create a class inheriting from `AttachmentContext` 

    // attachmentContext will be the same dictionary you passed in on Mail.AttachmentContext when calling BusExtensions.SendMail.
    public class AttachmentFinder : IAttachmentFinder
    {
        public IEnumerable<Attachment> FindAttachments(Dictionary<string, string> attachmentContext)
        {
            // Find the Attachments for the given context. 
            var id = attachmentContext["Id"];
            var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes("Hello"));
            yield return new Attachment(memoryStream, "example.txt", "text/plain");
        }

        public void CleanAttachments(Dictionary<string, string> attachmentContext)
        {
            // Attachment cleanup can be performed here
        }
    }

### Inject IAttachmentFinder into Container

Then configure the instance to be injected into the NSerivceBus Container.

        configure
            .Configurer
            .ConfigureComponent<IAttachmentFinder>(_ => new AttachmentFinder(), DependencyLifecycle.SingleInstance);

### Pass an `AttachmentContext` when sending the email

Pass an `AttachmentContext` when calling `SendMail`. The `AttachmentContext` should contain enough information for you to derive how to find and return the attachments for the email. 

        var mail = new Mail
            {
                To = "to@fake.email",
                From = "from@fake.email",
                Body = "This is the body",
                Subject = "Hello",
                AttachmentContext = new Dictionary<string, string>{{"Id","fakeEmail"}}
            };
        bus.SendMail(mail);

## Icon

<a href="http://thenounproject.com/noun/envelope/#icon-No15467" target="_blank">Envelope</a> designed by <a href="http://thenounproject.com/hspencer" target="_blank">Herbert Spencer</a> from The Noun Project