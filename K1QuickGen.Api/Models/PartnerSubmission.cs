using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace K1QuickGen.Api.Models
{
    public class PartnerSubmission
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonRepresentation(BsonType.String)]
        public Guid Form1065Id { get; set; }  // Link to the parent Form1065

        [BsonRepresentation(BsonType.String)]
        public Guid CompanyId { get; set; }   // Updated to Guid for consistency

        public string? PartnerName { get; set; }
        public string? PartnerType { get; set; }

        public decimal OwnershipPercentage { get; set; }
        public decimal CapitalContribution { get; set; }
        public string? Email { get; set; }

        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;
    }
}
