using System.Net.Mail;

namespace NServiceBus.Mailer
{
    /// <summary>
    /// The default <see cref="ISmtpBuilder"/>.
    /// </summary>
    public class DefaultSmtpBuilder : ISmtpBuilder
    {
        /// <summary>
        /// Build a new <see cref="SmtpClient"/>.
        /// </summary>
        public SmtpClient BuildClient()
        {
            return new SmtpClient
            {
                EnableSsl = true
            };
        }
    }
}