using System;
using NServiceBus.Mailer;
using static Headers;

public static class MessageIdExtensions
{
    public static void SetMessageId(this Mail instance, Guid messageId, string domain)
    {
        instance?.Headers.Add(MessageIdKey, $"<{messageId:n}@{domain}>");
    }

    internal static void SetMessageId(this NServiceBus.Mailer.MailMessage instance, Guid messageId, string domain)
    {
        instance?.Headers.Add(MessageIdKey, $"<{messageId:n}@{domain}>");
    }    
}