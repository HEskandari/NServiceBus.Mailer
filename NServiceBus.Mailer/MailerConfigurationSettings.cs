using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using NServiceBus.Configuration.AdvanceExtensibility;

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
            Func<IReadOnlyDictionary<string, string>, Task<IEnumerable<Attachment>>> findAttachments,
            Func<IReadOnlyDictionary<string, string>, Task> cleanAttachments
            )
        {
            var settings = config.GetSettings();
            settings.Set("NServiceBus.Mailer.FindAttachments", findAttachments);
            settings.Set("NServiceBus.Mailer.CleanAttachments", cleanAttachments);
        }

        /// <summary>
        /// Register a custom SmtpClient builder.
        /// </summary>
        public void UseSmtpBuilder(Func<SmtpClient> buildSmtpClient)
        {
            var settings = config.GetSettings();
            settings.Set("NServiceBus.Mailer.BuildSmtpClient", buildSmtpClient);
        }


    }
}