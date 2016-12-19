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
            config.EnableFeature<MailerFeature>();
            return new MailerConfigurationSettings(config);
        }
    }
}