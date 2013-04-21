using System.Net.Mail;

namespace NServiceBusMail
{
    public interface ISmtpBuilder
    {
        SmtpClient BuildClient();
    }
}