using System.Net.Mail;

namespace NServiceBusMail
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