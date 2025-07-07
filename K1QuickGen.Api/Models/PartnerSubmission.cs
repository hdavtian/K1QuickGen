using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace K1QuickGen.Api.Models
{
    public class PartnerSubmission
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Form1065Id { get; set; }  // Link to the parent Form1065

        public string CompanyId { get; set; }   // Shard key and company grouping

        public string PartnerName { get; set; }
        public string PartnerType { get; set; }
        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;
    }
}
