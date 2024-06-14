using NServiceBus.Configuration.AdvancedExtensibility;
using System;

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
        [Obsolete("Use EnableMailer(string domain)")]
        public static MailerConfigurationSettings EnableMailer(this EndpointConfiguration config)
        {
            Guard.AgainstNull(nameof(config), config);
            config.EnableFeature<MailerFeature>();
            return new MailerConfigurationSettings(config);
        }

        public static MailerOptions EnableMailer(this EndpointConfiguration config, string domain)
        {
            Guard.AgainstNull(nameof(config), config);
            Guard.AgainstNull(nameof(domain), domain);
            config.EnableFeature<MailerFeature>();
            var options = config.GetSettings().GetOrCreate<MailerOptions>();
            options.Domain = domain;
            return options;
        }
    }
}