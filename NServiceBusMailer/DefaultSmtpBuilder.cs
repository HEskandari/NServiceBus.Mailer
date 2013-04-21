using System.Net.Mail;

namespace NServiceBusMailer
{
    public class DefaultSmtpBuilder : ISmtpBuilder
    {
        public SmtpClient BuildClient()
        {
            return new SmtpClient
                {
                    EnableSsl = true
                };
        }
    }
}