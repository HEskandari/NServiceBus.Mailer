namespace NServiceBus.Mailer
{
    /// <summary>
    /// Represents the format to view an email message.
    /// </summary>
    public class AlternateView
    {
        /// <summary>
        /// The content type of this attachment
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// The content type of this attachment
        /// </summary>
        public string Content { get; set; }
    }
}