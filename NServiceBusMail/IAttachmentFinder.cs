using System.Collections.Generic;
using System.Net.Mail;

namespace NServiceBusMail
{
    /// <summary>
    /// Find attachemnts for an email.
    /// </summary>
    public interface IAttachmentFinder
    {
        IEnumerable<Attachment> FindAttachments(Dictionary<string, string> mailContext);
        void CleanAttachments(Dictionary<string, string> mailContext);
    }
}