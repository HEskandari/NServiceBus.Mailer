using System;

namespace NServiceBusMailer
{
    static class MailValidator
    {

        public static void ValidateMail(this Mail mail)
        {
            if (mail == null)
            {
                throw new ArgumentNullException("mail");
            }
            if (mail.Bcc == null)
            {
                throw new ArgumentException("Bcc cannot be null");
            }
            if (mail.Cc == null)
            {
                throw new ArgumentException("Cc cannot be null");
            }
            if (mail.To == null)
            {
                throw new ArgumentException("To cannot be null");
            }
            if (mail.ReplyTo == null)
            {
                throw new ArgumentException("ReplyTo cannot be null");
            }
            if (mail.AlternateViews == null)
            {
                throw new ArgumentException("AlternateViews cannot be null");
            }
            if (mail.Body == null)
            {
                throw new ArgumentException("Body cannot be null");
            }

            var totalRecipients = mail.Bcc.Count + mail.To.Count + mail.Cc.Count;
            if (totalRecipients == 0)
            {
                throw new ArgumentException("No recipients");
            }
        }
    }
}