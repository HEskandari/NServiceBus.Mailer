using System;

namespace NServiceBus.Mailer
{
    /// <summary>
    /// Helper class for sending emails through an <see cref="IBus"/>.
    /// </summary>
    public static class BusExtensions
    {
        /// <summary>
        /// Sends the specified <see cref="NServiceBus.Mailer.Mail"/> via the <see cref="IBus"/> to an SMTP server for delivery.
        /// </summary>
        /// <param name="bus">The <see cref="IBus"/> that is sending the message.</param>
        /// <param name="mail">The <see cref="NServiceBus.Mailer.Mail"/> to send.</param>
        public static void SendMail(this IBus bus, Mail mail)
        {
            if (bus == null)
            {
                throw new ArgumentNullException("bus");
            }
            mail.ValidateMail();
            var scope = Configure.Instance.GetMasterNodeAddress().SubScope("Mail");
            bus.Send(scope, mail.ToMailMessage());
        }


        internal static DateTime TimeSent(this IBus bus)
        {
            return DateTimeExtensions.ToUtcDateTime(bus.CurrentMessageContext.Headers[Headers.TimeSent]);
        }
    }
}
