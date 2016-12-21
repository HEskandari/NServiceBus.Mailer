using System.Threading.Tasks;

namespace NServiceBus.Mailer
{
    /// <summary>
    /// Helper class for sending emails through an <see cref="IMessageHandlerContext"/>.
    /// </summary>
    public static class MailSender
    {
        /// <summary>
        /// Sends the specified <see cref="Mail"/> via the <see cref="IMessageHandlerContext"/> to an SMTP server for delivery.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mail">The <see cref="Mail"/> to send.</param>
        public static Task SendMail(this IMessageHandlerContext context, Mail mail)
        {
            Guard.AgainstNull(nameof(mail), mail);
            Guard.AgainstNull(nameof(context), context);
            mail.ValidateMail();
            return context.Send("Mail", mail.ToMailMessage());
        }
    }
}