namespace NServiceBus.Mailer
{
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Text;


    /// <summary>
    /// Represents an e-mail message that can be sent using the <see cref="MailSender.SendMail"/>.
    /// </summary>
    public class Mail
    {

        /// <summary>
        /// The address collection that contains the recipients of this e-mail message.
        /// </summary> 
        public AddressList To { get; set; } = new AddressList();

        /// <summary>
        /// The address collection that contains the blind carbon copy (BCC) recipients for this e-mail message.
        /// </summary>
        public AddressList Bcc { get; set; } = new AddressList();

        /// <summary>
        /// The message body.
        /// </summary>
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// The address collection that contains the carbon copy (CC) recipients for this e-mail message.
        /// </summary> 
        public AddressList Cc { get; set; } = new AddressList();

        /// <summary>
        /// The encoding used to encode the message body.
        /// </summary>
        public Encoding BodyEncoding { get; set; }

        /// <summary>
        /// The delivery notifications for this e-mail message.
        /// </summary>
        public DeliveryNotificationOptions DeliveryNotificationOptions { get; set; }

        /// <summary>
        /// The from address for this e-mail message.
        /// </summary> 
        public string From { get; set; }

        /// <summary>
        /// The e-mail headers that are transmitted with this e-mail message.
        /// </summary> 
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// The (optional) context of the message that can be used by <see cref="MailerConfigurationSettings.UseAttachmentFinder"/> to retrieve attachments.
        /// </summary> 
        public Dictionary<string, string> AttachmentContext { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// The encoding used for the user-defined custom headers for this e-mail message.
        /// </summary> 
        public Encoding HeadersEncoding { get; set; }

        /// <summary>
        /// A value indicating whether the mail message body is in Html.
        /// </summary> 
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// The list of addresses to reply to for the mail message.
        /// </summary> 
        public AddressList ReplyTo { get; set; } = new AddressList();

        /// <summary>
        /// The sender's address for this e-mail message.
        /// </summary> 
        public string Sender { get; set; }

        /// <summary>
        /// The subject line for this e-mail message.
        /// </summary> 
        public string Subject { get; set; }

        /// <summary>
        /// The encoding used for the subject content for this e-mail message.
        /// </summary> 
        public Encoding SubjectEncoding { get; set; }

        /// <summary>
        /// The priority of this e-mail message.
        /// </summary> 
        public MailPriority Priority { get; set; }

        /// <summary>
        /// The collection used to store alternate forms of the message body.
        /// </summary>
        public List<AlternateView> AlternateViews { get; set; } = new List<AlternateView>();
    }
}