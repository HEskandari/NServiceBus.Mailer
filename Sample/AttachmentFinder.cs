using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using NServiceBusMail;

public class AttachmentFinder : IAttachmentFinder
{
    public IEnumerable<Attachment> FindAttachments(Dictionary<string, string> mailContext)
    {
        //id can be used to retrieve attachments
        var id = mailContext["Id"];
        var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes("Hello"));
        yield return new Attachment(memoryStream, "example.txt", "text/plain");
    }

    public void CleanAttachments(Dictionary<string, string> mailContext)
    {
    }
}