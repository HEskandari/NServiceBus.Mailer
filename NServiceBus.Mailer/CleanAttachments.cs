using System.Collections.Generic;
using System.Threading.Tasks;

namespace NServiceBus.Mailer
{
    public delegate Task CleanAttachments(IReadOnlyDictionary<string, string> attachmentContext);
}