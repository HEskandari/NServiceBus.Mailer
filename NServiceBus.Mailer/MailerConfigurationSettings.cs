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
        /// Register a custom <see cref="IAttachmentFinder"/>.
        /// </summary>
        public void UseAttachmentFinder<T>() where T : IAttachmentFinder
        {
            config.RegisterComponents(x => x.ConfigureComponent<T>(DependencyLifecycle.SingleInstance));
        }

        /// <summary>
        /// Register a custom <see cref="IAttachmentFinder"/>.
        /// </summary>
        public void UseAttachmentFinder<T>(T instance) where T : IAttachmentFinder
        {
            config.RegisterComponents(x => x.RegisterSingleton(instance));
        }

        /// <summary>
        /// Register a custom <see cref="ISmtpBuilder"/>.
        /// </summary>
        public void UseSmtpBuilder<T>() where T : ISmtpBuilder
        {
            config.RegisterComponents(x => x.ConfigureComponent<T>(DependencyLifecycle.SingleInstance));
        }

        /// <summary>
        /// Register a custom <see cref="ISmtpBuilder"/>.
        /// </summary>
        public void UseSmtpBuilder<T>(T instance) where T : ISmtpBuilder
        {
            config.RegisterComponents(x => x.RegisterSingleton(instance));
        }
    }
}