using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Mailer;
// ReSharper disable UnusedVariable

// attachmentContext will be the same dictionary you passed in on Mail.AttachmentContext when calling SendMail.
public class AttachmentFinder : IAttachmentFinder
{
    public IEnumerable<Attachment> FindAttachments(Dictionary<string, string> attachmentContext)
    {
        // Find the Attachments for the given context.
        var id = attachmentContext["Id"];
        var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes("Hello"));
        yield return new Attachment(memoryStream, "example.txt", "text/plain");
    }

    public Task CleanAttachments(Dictionary<string, string> attachmentContext)
    {
        // Attachment cleanup can be performed here
        return Task.FromResult(0);
    }
}