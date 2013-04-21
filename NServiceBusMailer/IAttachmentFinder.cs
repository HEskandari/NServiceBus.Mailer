using System.Collections.Generic;
using System.Net.Mail;

namespace NServiceBusMailer
{
    /// <summary>
    /// Find attachemnts for an email.
    /// </summary>
    public interface IAttachmentFinder
    {
        /// <summary>
        /// Find the <see cref="Attachment"/>s for the given context.
        /// </summary>
        /// <param name="attachmentContext">the same <see cref="Dictionary{TKey,TValue}"/> you passed in on <see cref="Mail.AttachmentContext"/> when calling <see cref="BusExtensions.SendMail"/></param>.
        IEnumerable<Attachment> FindAttachments(Dictionary<string, string> attachmentContext);

        /// <summary>
        /// Clean up the attachments for a given context
        /// </summary>
        /// <param name="attachmentContext">the same <see cref="Dictionary{TKey,TValue}"/> you passed in on <see cref="Mail.AttachmentContext"/> when calling <see cref="BusExtensions.SendMail"/></param>.
        void CleanAttachments(Dictionary<string, string> attachmentContext);
    }
}