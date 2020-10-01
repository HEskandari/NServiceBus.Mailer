using NServiceBus.Settings;

namespace NServiceBus.Mailer
{
    /// <summary>
    /// Provides config options for the mailer feature.
    /// </summary>
    public static class MailerConfigurationExtensions
    {

        /// <summary>
        /// Provides config options for the mailer feature.
        /// </summary>
        public static MailerConfigurationSettings EnableMailer(this EndpointConfiguration config)
        {
            Guard.AgainstNull(nameof(config), config);
            config.EnableFeature<MailerFeature>();
            return new MailerConfigurationSettings(config);
        }

        internal static BuildSmtpClient GetSmtpBuilder(this ReadOnlySettings settings)
        {
            return settings.GetOrDefault<BuildSmtpClient>();
        }

        internal static CleanAttachments GetAttachmentCleaner(this ReadOnlySettings settings)
        {
            return settings.GetOrDefault<CleanAttachments>();
        }

        internal static FindAttachments GetAttachmentFinder(this ReadOnlySettings settings)
        {
            return settings.GetOrDefault<FindAttachments>();
        }
    }
}