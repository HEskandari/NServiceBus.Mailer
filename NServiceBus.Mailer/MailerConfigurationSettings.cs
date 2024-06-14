using NServiceBus.Configuration.AdvancedExtensibility;
using System;

namespace NServiceBus.Mailer
{
    /// <summary>
    /// Configuration settings for Mailer.
    /// </summary>
    [Obsolete("Use MailerOptions return type from EnableMailer(string domain)")]
    public class MailerConfigurationSettings
    {
        EndpointConfiguration config;

        internal MailerConfigurationSettings(EndpointConfiguration config)
        {
            this.config = config;
        }

        /// <summary>
        /// Register attachment discovery and cleanup.
        /// </summary>
        public void UseAttachmentFinder(
            FindAttachments findAttachments,
            CleanAttachments cleanAttachments)
        {
            Guard.AgainstNull(nameof(findAttachments), findAttachments);
            Guard.AgainstNull(nameof(cleanAttachments), cleanAttachments);
            var options = config.GetSettings().GetOrCreate<MailerOptions>();
            options.AttachmentsFinder = findAttachments;
            options.AttachmentCleaner = cleanAttachments;
        }

        /// <summary>
        /// Register attachment discovery and cleanup.
        /// </summary>
        public void UseAttachmentFinder(
            FindAttachments findAttachments)
        {
            Guard.AgainstNull(nameof(findAttachments), findAttachments);
            var options = config.GetSettings().GetOrCreate<MailerOptions>();
            options.AttachmentsFinder = findAttachments;
        }

        /// <summary>
        /// Register a custom SmtpClient builder.
        /// </summary>
        public void UseSmtpBuilder(BuildSmtpClient buildSmtpClient)
        {
            Guard.AgainstNull(nameof(buildSmtpClient), buildSmtpClient);
            var options = config.GetSettings().GetOrCreate<MailerOptions>();
            options.SmtpClientBuilder = buildSmtpClient;
        }
    }
}