namespace NServiceBus.Mailer
{
    /// <summary>
    /// Helper class for sending emails through an <see cref="IBus"/>.
    /// </summary>
    public class MailSender
    {
        public IBus Bus { get; set; }
        public Address InputAddress { get; set; }

        /// <summary>
        /// Sends the specified <see cref="NServiceBus.Mailer.Mail"/> via the <see cref="IBus"/> to an SMTP server for delivery.
        /// </summary>
        /// <param name="mail">The <see cref="NServiceBus.Mailer.Mail"/> to send.</param>
        public void SendMail(Mail mail)
        {
            mail.ValidateMail();
            Bus.Send(InputAddress, mail.ToMailMessage());
        }
    }
}