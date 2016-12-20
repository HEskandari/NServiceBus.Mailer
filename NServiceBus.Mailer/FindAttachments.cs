using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NServiceBus.Mailer
{
    public delegate Task<IEnumerable<Attachment>> FindAttachments(IReadOnlyDictionary<string, string> attachmentContext);
}