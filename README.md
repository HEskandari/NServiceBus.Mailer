NServiceBus.Mailer
===============

## The nuget package  [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Mailer.svg?style=flat)](https://www.nuget.org/packages/NServiceBus.Mailer/)

https://nuget.org/packages/NServiceBus.Mailer/

    PM> Install-Package NServiceBus.Mailer

## Usage 
     
    using NServiceBus.Mailer; 
    var mail = new Mail
            {
                To = "to@fake.email",
                From = "from@fake.email",
                Body = "This is the body",
                Subject = "Hello",
            }
    bus.SendMail(mail);

## SmtpClient construction 

The interface `ISmtpBuilder` can be used to control how an [`SmtpClient`](http://msdn.microsoft.com/en-us/library/system.net.mail.smtpclient.aspx) is constructed.

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
    
### Inject `ISmtpBuilder` into the Container

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

### Inject `IAttachmentFinder` into the Container

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

## Error handling

Retrying email is difficult due to the fact that when sending an email to multiple addresses a subset of those addresses may return an error. In this case resending to all addresses would result in some addresses receiving the email multiple times.

### When all addresses fail

This case is when there is a generic exception talking to the mail server or the server returns an error that indicates all addresses have failed.

This is handling by letting the exception bubble to NServiceBus. This will result in falling back on the standard NServiceBus retry logic.

### When a subset of addresses fail

This will most likely occur when there is a subset of invalid addresses however there cases where the address can fail once and succeed after a retry. Have a look at [SmtpStatusCode](http://msdn.microsoft.com/en-us/library/system.net.mail.smtpstatuscode.aspx) for the possible error cases.

In this scenario it is not valid to retry the message since it would result in the message being resent to all recipients. It is also flawed to resend the verbatim email to the subset of failed addresses as this would effectively exclude them from some of the recipients in the conversation.

So the approach taken is to forward the original message to the failed recipients after prefixing the body with the following text

    This message was forwarded due to the original email failing to send
    -----Original Message-----
    To: XXX
    CC: XXX
    Sent: XXX

While this is a little hacky it achieves the desired of letting the failed recipients receive the email contents while also notifying them that there is a conversation happening with other recipients. It also avoids spamming the other recipients.

## Icon

<a href="http://thenounproject.com/noun/envelope/#icon-No15467" target="_blank">Envelope</a> designed by <a href="http://thenounproject.com/hspencer" target="_blank">Herbert Spencer</a> from The Noun Project
