using System;

namespace K1QuickGen.Api.Messages
{
    /// <summary>
    /// Base class for all message types with common metadata
    /// </summary>
    public class MessageBase
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
        public string MessageType => GetType().Name;
    }

    /// <summary>
    /// Message sent when a Form1065 is submitted
    /// </summary>
    public class Form1065SubmittedMessage : MessageBase
    {
        // Essential identifiers for lookups
        public Guid Form1065Id { get; set; }
        public Guid CompanyId { get; set; }

        // Basic info useful for logging/tracking without DB lookups
        public string CompanyName { get; set; } = string.Empty;
        public int TaxYear { get; set; }
    }

    /// <summary>
    /// Message sent when a partner is submitted
    /// </summary>
    public class PartnerSubmittedMessage : MessageBase
    {
        // Essential identifiers for lookups
        public Guid PartnerId { get; set; }
        public Guid Form1065Id { get; set; }
        public Guid CompanyId { get; set; }

        // Basic info useful for logging/tracking without DB lookups
        public string PartnerName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Command to generate a PDF
    /// </summary>
    public class GeneratePdfCommand : MessageBase
    {
        public Guid Form1065Id { get; set; }
        public bool IncludeK1s { get; set; } = true;
    }
}