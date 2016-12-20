using System.Threading.Tasks;

namespace NServiceBus.Mailer
{
    /// <summary>
    /// Helper class for sending emails through an <see cref="IMessageHandlerContext"/>.
    /// </summary>
    public class MailSender
    {
        /// <summary>
        /// Sends the specified <see cref="NServiceBus.Mailer.Mail"/> via the <see cref="IMessageHandlerContext"/> to an SMTP server for delivery.
        /// </summary>
        /// <param name="mail">The <see cref="NServiceBus.Mailer.Mail"/> to send.</param>
        /// <param name="context"></param>
        public Task SendMail(Mail mail, IMessageHandlerContext context)
        {
            mail.ValidateMail();
            return context.Send("Mail", mail.ToMailMessage());
        }
    }
}