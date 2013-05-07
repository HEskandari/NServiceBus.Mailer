using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using NUnit.Framework;

namespace NServiceBus.Mailer
{
    [TestFixture]
    class MessageConverterTests
    {
        [Test]
        public void MailToMailMessage()
        {
            var mailMessage = new Mail
                {
                    DeliveryNotificationOptions = DeliveryNotificationOptions.Delay,
                    IsBodyHtml = true,
                    AlternateViews = new List<AlternateView>
                        {
                            new AlternateView {Content = "AlternateView1", ContentType = "html/text"}
                        },
                    Bcc = "bcc@b.com",
                    Cc = "cc@b.com",
                    To = "to@b.com",
                    ReplyTo = "reply@b.com",
                    Subject = "Subject",
                    Body = "Body",
                    From = "from@b.com",
                    BodyEncoding = Encoding.UTF32,
                    Headers = new Dictionary<string, string> {{"Key", "Value"}},
                    HeadersEncoding = Encoding.UTF32,
                    Sender = "sender@b.com",
                    Priority = MailPriority.High,
                    SubjectEncoding = Encoding.UTF32,
                };
            var message = mailMessage.ToMailMessage();
            Assert.AreEqual(DeliveryNotificationOptions.Delay, message.DeliveryNotificationOptions);
            Assert.IsTrue(message.IsBodyHtml);
            var alternateView = message.AlternateViews.First();
            Assert.AreEqual("AlternateView1", alternateView.Content);
            Assert.AreEqual("html/text", alternateView.ContentType);
            Assert.AreEqual("bcc@b.com", message.Bcc.First());
            Assert.AreEqual("cc@b.com", message.Cc.First());
            Assert.AreEqual("to@b.com", message.To.First());
            Assert.AreEqual("reply@b.com", message.ReplyTo.First());
            Assert.AreEqual("sender@b.com", message.Sender);
            Assert.AreEqual("from@b.com", message.From);
            Assert.AreEqual("Subject", message.Subject);
            Assert.AreEqual("Body", message.Body);
            Assert.AreEqual(Encoding.UTF32, message.BodyEncoding);
            Assert.AreEqual(Encoding.UTF32, message.SubjectEncoding);
            Assert.AreEqual(Encoding.UTF32, message.HeadersEncoding);
            Assert.AreEqual(MailPriority.High, message.Priority);
            Assert.AreEqual("Value", message.Headers["Key"]);
        }

        [Test]
        public void MailMessageToSystemMailMessage()
        {
            var mailMessage = new MailMessage
                {
                    DeliveryNotificationOptions = DeliveryNotificationOptions.Delay,
                    IsBodyHtml = true,
                    AlternateViews = new List<AlternateView>
                        {
                            new AlternateView {Content = "AlternateView1", ContentType = "html/text"}
                        },
                    Bcc = new List<string> {"bcc@b.com"},
                    Cc = new List<string> {"cc@b.com"},
                    To = new List<string> {"to@b.com"},
                    ReplyTo = new List<string> {"reply@b.com"},
                    Subject = "Subject",
                    Body = "Body",
                    From = "from@b.com",
                    BodyEncoding = Encoding.UTF32,
                    Headers = new Dictionary<string, string> {{"Key", "Value"}},
                    HeadersEncoding = Encoding.UTF32,
                    Sender = "sender@b.com",
                    Priority = MailPriority.High,
                    SubjectEncoding = Encoding.UTF32,
                };
            var message = mailMessage.ToMailMessage();
            Assert.AreEqual(DeliveryNotificationOptions.Delay, message.DeliveryNotificationOptions);
            Assert.IsTrue(message.IsBodyHtml);
            var alternateView = message.AlternateViews.First();
            Assert.AreEqual("AlternateView1", new StreamReader(alternateView.ContentStream).ReadToEnd());
            Assert.AreEqual("html/text", alternateView.ContentType.MediaType);
            Assert.AreEqual("bcc@b.com", message.Bcc.First().Address);
            Assert.AreEqual("cc@b.com", message.CC.First().Address);
            Assert.AreEqual("to@b.com", message.To.First().Address);
            Assert.AreEqual("reply@b.com", message.ReplyToList.First().Address);
            Assert.AreEqual("sender@b.com", message.Sender.Address);
            Assert.AreEqual("from@b.com", message.From.Address);
            Assert.AreEqual("Subject", message.Subject);
            Assert.AreEqual("Body", message.Body);
            Assert.AreEqual(Encoding.UTF32, message.BodyEncoding);
            Assert.AreEqual(Encoding.UTF32, message.SubjectEncoding);
            Assert.AreEqual(Encoding.UTF32, message.HeadersEncoding);
            Assert.AreEqual(MailPriority.High, message.Priority);
            Assert.AreEqual("Value", message.Headers["Key"]);
        }
    }
}