using System.Threading.Tasks;
using DiffEngine;
using VerifyNUnit;

namespace NServiceBus.Mailer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using VerifyTests;

    [TestFixture]
    class MessageConverterTests
    {
        private VerifySettings settings;

        public MessageConverterTests()
        {
            DiffTools.UseOrder(DiffTool.VisualStudioCode, DiffTool.Rider, DiffTool.VisualStudio);

            settings = new VerifySettings();
            settings.IgnoreMembersWithType<Stream>();
        }

        [Test]
        public async Task MailToMailMessage()
        {
            var mailMessage = CreateMail();
            var message = mailMessage.ToMailMessage();
            await Verifier.Verify(message, settings);
        }

        [Test]
        public async Task MailMessageToSystemMailMessage()
        {
            var nsbMail = CreateMailMessage();
            var message = nsbMail.ToMailMessage();
            var alternateView = message.AlternateViews.First();

            await Verifier.Verify(new
            {
                message,
                alternateView
            }, settings);
        }

        [Test]
        public async Task MailMessageToSystemMailMessageShouldBeAbleToReadStream()
        {
            var nsbMail = CreateMailMessage();
            var message = nsbMail.ToMailMessage();
            var alternateView = message.AlternateViews.First();
            ClassicAssert.AreEqual("AlternateView1", await new StreamReader(alternateView.ContentStream).ReadToEndAsync());
        }

        Mail CreateMail()
        {
            var encoding = Encoding.UTF32.BodyName;

            var mailMessage = new Mail
            {
                DeliveryNotificationOptions = DeliveryNotificationOptions.Delay,
                IsBodyHtml = true,
                AlternateViews = new List<AlternateView>
                {
                    new AlternateView
                    {
                        Content = "AlternateView1",
                        ContentType = "html/text"
                    }
                },
                Bcc = "bcc@b.com",
                Cc = "cc@b.com",
                To = "to@b.com",
                ReplyTo = "reply@b.com",
                Subject = "Subject",
                Body = "Body",
                From = "from@b.com",
                BodyEncoding = encoding,
                Headers = new Dictionary<string, string> { { "Key", "Value" } },
                HeadersEncoding = encoding,
                Sender = "sender@b.com",
                Priority = MailPriority.High,
                SubjectEncoding = encoding,
            };

            return mailMessage;
        }

        MailMessage CreateMailMessage()
        {
            var encoding = Encoding.UTF32.BodyName;

            var mailMessage = new MailMessage
            {
                DeliveryNotificationOptions = DeliveryNotificationOptions.Delay,
                IsBodyHtml = true,
                AlternateViews = new List<AlternateView>
                {
                    new AlternateView {Content = "AlternateView1", ContentType = "html/text"}
                },
                Bcc = new List<string> { "bcc@b.com" },
                Cc = new List<string> { "cc@b.com" },
                To = new List<string> { "to@b.com" },
                ReplyTo = new List<string> { "reply@b.com" },
                Subject = "Subject",
                Body = "Body",
                From = "from@b.com",
                BodyEncoding = encoding,
                Headers = new Dictionary<string, string> { { "Key", "Value" } },
                HeadersEncoding = encoding,
                Sender = "sender@b.com",
                Priority = MailPriority.High,
                SubjectEncoding = encoding,
            };

            return mailMessage;
        }
    }
}