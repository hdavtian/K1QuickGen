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
    }
}
