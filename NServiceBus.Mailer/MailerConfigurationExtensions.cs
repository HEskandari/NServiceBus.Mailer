using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
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
            config.EnableFeature<MailerFeature>();
            return new MailerConfigurationSettings(config);
        }


        internal static Func<SmtpClient> GetSmtpBuilder(this ReadOnlySettings settings)
        {
            return settings.GetOrDefault<Func<SmtpClient>>("NServiceBus.Mailer.BuildSmtpClient");
        }

        internal static Func<IReadOnlyDictionary<string, string>, Task> GetAttachmentCleaner(this ReadOnlySettings settings)
        {
            return settings.GetOrDefault<Func<IReadOnlyDictionary<string, string>, Task>>("NServiceBus.Mailer.CleanAttachments");
        }

        internal static Func<IReadOnlyDictionary<string, string>, Task<IEnumerable<Attachment>>> GetAttachmentFinder(this ReadOnlySettings settings)
        {
            return settings.GetOrDefault<Func<IReadOnlyDictionary<string, string>, Task<IEnumerable<Attachment>>>>("NServiceBus.Mailer.FindAttachments");
        }
    }
}