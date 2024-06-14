# <img src='/Icons/package_icon.png' height='30px'> NServiceBus.Mailer

[![Build status](https://ci.appveyor.com/api/projects/status/on2svv1qboc4v4xc/branch/master?svg=true)](https://ci.appveyor.com/project/HEskandari/nservicebus-mailer)
[![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Mailer.svg?label=NServiceBus.Mailer)](https://www.nuget.org/packages/NServiceBus.Mailer/)


## Installing the package

https://nuget.org/packages/NServiceBus.Mailer/

    PM> Install-Package NServiceBus.Mailer


## Enabling

NServiceBus.Mailer can be enabled via `.EnableMailer(string domain)`. It is required to specify a domain name to be used for the [rfc2822 Message-ID header](#message-id).

```c#
var configuration = new EndpointConfiguration("NServiceBusMailSample");
var mailerOptions = endpointConfiguration.EnableMailer("mydomain.com");
```

> [!NOTE]
> The previous API is still supported but marked as obsolete

## Usage 

Mails can be send via the `SendMail(mail)` extension method on `IMessageHandlerContext`:
     
```c#
public class MyHandler : IHandleMessages<MyMessage>
{
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var mail = new Mail
        {
            To = "to@fake.email",
            From = "from@fake.email",
            Body = "This is the body",
            Subject = "Hello"
        };
        await context.SendMail(mail);
    }
}
```


## SmtpClient construction 

By default the `SmtpClient` will be used except with SSL enabled. This effectively uses the code

```c#
return new SmtpClient
    {
        EnableSsl = true
    };
```

This results in the `SmtpClient` defaulting to reading its settings from the application config.

### Custom SmtpClient

To use a custom `SmtpClient` register a builder via `MailerOptions.SmtpClientBuilder`:

```c#
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
```

## Attachments

Since it is not practical to send binary data as part of messages there is an alternative mechanism. The interface `IAttachmentFinder` can be used to defer attachment creation until the point of the email being sent.

### Register a attachment finder

```c#
var mailerOptions = endpointConfiguration.EnableMailer("mydomain.com");
mailerOptions.AttachmentsFinder = async attachmentContext =>
    {
        var id = attachmentContext["Id"];
        var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes("Hello"));
        var attachment = new Attachment(memoryStream, "example.txt", "text/plain");
        var attachments = new List<Attachment> { attachment };
        return attachments;
    };
```

### Register a cleanup routing

A cleanup routine can be register if attachments are not auto removed by your storage.

```c#
var mailerOptions = endpointConfiguration.EnableMailer("mydomain.com");
mailerOptions.AttachmentCleaner = attachmentContext =>
    {
        // Attachment cleanup can be performed here
        return Task.FromResult(0);
    };
```

### Pass an `AttachmentContext` when sending the email

Pass an `AttachmentContext` when calling `SendMail`. The `AttachmentContext` should contain enough information for you to derive how to find and return the attachments for the email. 

```c#
var mail = new Mail
{
    // ...
    AttachmentContext = {
        {"Id", "fakeEmail"}
    }
};
await context.SendMail(mail);
```

## Error handling

Retrying email is difficult due to the fact that when sending an email to multiple addresses a subset of those addresses may return an error. In this case re-sending to all addresses would result in some addresses receiving the email multiple times.

### When all addresses fail

This case is when there is a generic exception talking to the mail server or the server returns an error that indicates all addresses have failed.

This is handled by letting the exception bubble to NServiceBus. This will result in falling back on the standard NServiceBus retry logic.

### When a subset of addresses fail

This will most likely occur when there is a subset of invalid addresses however there are cases where the address can fail once and succeed after a retry. Have a look at [SmtpStatusCode](http://msdn.microsoft.com/en-us/library/system.net.mail.smtpstatuscode.aspx) for the possible error cases.

In this scenario it is not valid to retry the message since it would result in the message being resent to all recipients. It is also flawed to resend the verbatim email to the subset of failed addresses as this would effectively exclude them from some of the recipients in the conversation.

So the approach taken is to forward the original message to the failed recipients after prefixing the body with the following text

    This message was forwarded due to the original email failing to send
    -----Original Message-----
    To: XXX
    CC: XXX
    Sent: XXX

While this is a little hacky it achieves the desired of letting the failed recipients receive the email contents while also notifying them that there is a conversation happening with other recipients. It also avoids spamming the other recipients.


## Message-ID

It is recommended to always add message ID as recommended by rfc2822:

> Though optional, every message SHOULD have a "Message-ID:" field.

A [rfc2822 `Message-ID`](https://datatracker.ietf.org/doc/html/rfc2822#section-3.6.4) will be generated if the `.EnableMailer(string domain)` API is used.

This to make email send operations idempotent as sending the same email more than once can be de-duplicated by the SMTP server and email client.

### Override Message-ID

A rfc2822 `Message-ID` will only be generated if the message does not have a `Message-ID` value already set.

### Different domain per message

To use a different domain per message manually set the `Message-ID` header via `Mail.Headers["Message-Id"]`.

```c#
var messageId = Guid.NewGuid();
headers["Message-Id"] = "<{messageId}@{domain}}>";
```

An extension method is provided to easily set this header based on a `Guid`:

```c#
var messageId = Guid.Parse(context.MessageId);
mail.SetMessageId(messageId, "microsoft.com");
```

> [!NOTE]
> Instead of a random guid use a deterministic guid which is based on an incoming task. If this is not a guid you could based this on a MD5 of a string value. Keep in mind that this value should be unique per transactional email.


## Icon

<a href="http://thenounproject.com/noun/envelope/#icon-No15467" target="_blank">Envelope</a> designed by <a href="http://thenounproject.com/hspencer" target="_blank">Herbert Spencer</a> from The Noun Project
