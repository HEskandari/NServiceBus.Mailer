using System.Threading.Tasks;

namespace NServiceBus.Mailer
{
    /// <summary>
    /// Helper class for sending emails through an <see cref="IMessageHandlerContext"/>.
    /// </summary>
    public static class MailSender
    {
        /// <summary>
        /// Sends the specified <see cref="NServiceBus.Mailer.Mail"/> via the <see cref="IMessageHandlerContext"/> to an SMTP server for delivery.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mail">The <see cref="NServiceBus.Mailer.Mail"/> to send.</param>
        public static Task SendMail(this IMessageHandlerContext context, Mail mail)
        {
            mail.ValidateMail();
            return context.Send("Mail", mail.ToMailMessage());
        }
    }
}