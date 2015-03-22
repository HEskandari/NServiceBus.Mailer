using System.Net.Mail;

namespace NServiceBus.Mailer
{
    /// <summary>
    /// Factory for constructing a <see cref="SmtpClient"/>/
    /// </summary>
    public interface ISmtpBuilder
    {
        /// <summary>
        /// Build a new <see cref="SmtpClient"/>.
        /// </summary>
        SmtpClient BuildClient();
    }
}