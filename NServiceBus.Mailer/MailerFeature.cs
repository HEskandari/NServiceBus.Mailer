using NServiceBus.Features;

namespace NServiceBus.Mailer
{
    class MailerFeature : Feature
    {
        public MailerFeature()
        {
            Prerequisite(context => !context.Settings.GetOrDefault<bool>("Endpoint.SendOnly"), "Send only endpoints can't use the Mailer since it requires receive capabilities");
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            var localAddress = context.Settings.LocalAddress();
            var inputAddress = localAddress.SubScope("Mail");

            if (!context.Container.HasComponent<ISmtpBuilder>())
            {
                context
                    .Container
                    .ConfigureComponent<DefaultSmtpBuilder>(DependencyLifecycle.SingleInstance);
            }

            context.Container.ConfigureComponent<MailSatellite>(DependencyLifecycle.SingleInstance)
                .ConfigureProperty(t => t.InputAddress, inputAddress)
                .ConfigureProperty(t => t.EndpointName, context.Settings.EndpointName());

            context.Container.ConfigureComponent<MailSender>(DependencyLifecycle.SingleInstance)
                .ConfigureProperty(t => t.InputAddress, inputAddress);
        }
    }
}
