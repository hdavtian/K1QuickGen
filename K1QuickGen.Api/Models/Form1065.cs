using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace K1QuickGen.Api.Models
{
    public class Form1065
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] // Store Guid as string in MongoDB
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonRepresentation(BsonType.String)]
        public Guid CompanyId { get; set; }   // Shard key
        public string? CompanyName { get; set; }
        public int TaxYear { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? BusinessActivity { get; set; }
        public string? ProductOrService { get; set; }
        public string? BusinessCode { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public DateTime? DateBusinessStarted { get; set; }
        public bool IsFinalReturn { get; set; }
        public bool IsAmendedReturn { get; set; }
    }
}
