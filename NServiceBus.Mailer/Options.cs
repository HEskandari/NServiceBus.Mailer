namespace NServiceBus.Mailer
{
    public class MailerOptions
    {
        public FindAttachments AttachmentsFinder { get; set; }
        public BuildSmtpClient SmtpClientBuilder { get; set; }
        public CleanAttachments AttachmentCleaner { get; set; }
        public string Domain { get; internal set; }
    }
}