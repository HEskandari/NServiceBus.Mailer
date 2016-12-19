using System.Threading.Tasks;
using NServiceBus.Features;
using NServiceBus.ObjectBuilder;
using NServiceBus.Serialization;
using NServiceBus.Transport;

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
            var inputAddress = "Mail";

            if (!context.Container.HasComponent<ISmtpBuilder>())
            {
                context
                    .Container
                    .ConfigureComponent<DefaultSmtpBuilder>(DependencyLifecycle.SingleInstance);
            }

            context.Container.ConfigureComponent<MailSender>(DependencyLifecycle.SingleInstance);
            context.Container.ConfigureComponent<MailSatellite>(DependencyLifecycle.SingleInstance);

            context.AddSatelliteReceiver(
                name: "MailSatelite",
                transportAddress: inputAddress,
                runtimeSettings: PushRuntimeSettings.Default,
                recoverabilityPolicy: (config, errorContext) => RecoverabilityAction.MoveToError(config.Failed.ErrorQueue),
                onMessage: OnMessageReceived);

        }

        private Task OnMessageReceived(IBuilder builder, MessageContext context)
        {
            var mailSatelite = builder.Build<MailSatellite>();
            return mailSatelite.OnMessageReceived(context);
        }
    }
}
