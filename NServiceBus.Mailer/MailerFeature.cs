using System;
using System.Net.Mail;
using NServiceBus.Features;
using NServiceBus.MessageInterfaces.MessageMapper.Reflection;
using NServiceBus.Serialization;
using NServiceBus.Settings;
using NServiceBus.Transport;

namespace NServiceBus.Mailer
{
    class MailerFeature : Feature
    {
        public MailerFeature()
        {
            Prerequisite(IsSendableEndpoint, "Send only endpoints can't use the Mailer since it requires receive capabilities");
        }

        static bool IsSendableEndpoint(FeatureConfigurationContext context)
        {
            return !context.Settings.GetOrDefault<bool>("Endpoint.SendOnly");
        }
        
        protected override void Setup(FeatureConfigurationContext context)
        {
            
            var inputAddress = "Mail";
            var settings = context.Settings;
            var attachmentCleaner = settings.GetAttachmentCleaner();
            var attachmentFinder = settings.GetAttachmentFinder();
            var smtpBuilder = settings.GetSmtpBuilder();
            if (smtpBuilder == null)
            {
                smtpBuilder = () => new SmtpClient
                {
                    EnableSsl = true
                };
            }
            var serializer = GetDefaultSerializer(settings);
            var satellite = new MailSatellite(attachmentFinder, attachmentCleaner, smtpBuilder, serializer);



            context.AddSatelliteReceiver(
                name: "MailSatelite",
                transportAddress: inputAddress,
                runtimeSettings: PushRuntimeSettings.Default,
                recoverabilityPolicy: (config, errorContext) => RecoverabilityAction.MoveToError(config.Failed.ErrorQueue),
                onMessage: (builder, messageContext) => satellite.OnMessageReceived(builder, messageContext));
        }


        IMessageSerializer GetDefaultSerializer(ReadOnlySettings settings)
        {
            var mainSerializer = settings.Get<Tuple<SerializationDefinition, SettingsHolder>>("MainSerializer");
            var serializerFactory = mainSerializer.Item1.Configure(settings);
            return serializerFactory(new MessageMapper());
        }
    }
}