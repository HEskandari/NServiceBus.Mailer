using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace NServiceBusMail
{
    /// <summary>
    /// Represents an e-mail message that can be sent using the <see cref="BusExtensions.SendMail"/>.
    /// </summary>
    public class Mail
    {

        /// <summary>
        /// The address collection that contains the recipients of this e-mail message.
        /// </summary> 
        public AddressList To = new AddressList();

        /// <summary>
        /// The address collection that contains the blind carbon copy (BCC) recipients for this e-mail message.
        /// </summary>
        public AddressList Bcc = new AddressList();

        /// <summary>
        /// The message body.
        /// </summary>
        public string Body = string.Empty;

        /// <summary>
        /// The address collection that contains the carbon copy (CC) recipients for this e-mail message.
        /// </summary> 
        public List<string> Cc = new List<string>();

        /// <summary>
        /// The encoding used to encode the message body.
        /// </summary>
        public Encoding BodyEncoding;

        /// <summary>
        /// The delivery notifications for this e-mail message.
        /// </summary>
        public DeliveryNotificationOptions DeliveryNotificationOptions;

        /// <summary>
        /// The from address for this e-mail message.
        /// </summary> 
        public string From;

        /// <summary>
        /// The e-mail headers that are transmitted with this e-mail message.
        /// </summary> 
        public Dictionary<string, string> Headers = new Dictionary<string, string>();

        /// <summary>
        /// The encoding used for the user-defined custom headers for this e-mail message.
        /// </summary> 
        public Encoding HeadersEncoding;

        /// <summary>
        /// A value indicating whether the mail message body is in Html.
        /// </summary> 
        public bool IsBodyHtml;

        /// <summary>
        /// The list of addresses to reply to for the mail message.
        /// </summary> 
        public AddressList ReplyToList = new AddressList();

        /// <summary>
        /// The sender's address for this e-mail message.
        /// </summary> 
        public string Sender;

        /// <summary>
        /// The subject line for this e-mail message.
        /// </summary> 
        public string Subject;

        /// <summary>
        /// The encoding used for the subject content for this e-mail message.
        /// </summary> 
        public Encoding SubjectEncoding;


        /// <summary>
        /// The priority of this e-mail message.
        /// </summary> 
        public MailPriority Priority;

    }
}