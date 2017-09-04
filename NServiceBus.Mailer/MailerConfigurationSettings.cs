using NServiceBus.Configuration.AdvancedExtensibility;

namespace NServiceBus.Mailer
{
    /// <summary>
    /// Configuration settings for Mailer.
    /// </summary>
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
            var settings = config.GetSettings();
            settings.Set<FindAttachments>(findAttachments);
            settings.Set<CleanAttachments>(cleanAttachments);
        }

        /// <summary>
        /// Register a custom SmtpClient builder.
        /// </summary>
        public void UseSmtpBuilder(BuildSmtpClient buildSmtpClient)
        {
            Guard.AgainstNull(nameof(buildSmtpClient), buildSmtpClient);
            var settings = config.GetSettings();
            settings.Set<BuildSmtpClient>(buildSmtpClient);
        }
    }
}