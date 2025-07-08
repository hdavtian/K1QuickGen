using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace K1QuickGen.Api.Models
{
    public class ReturnDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? CompanyId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid Form1065Id { get; set; }

        public DateTime FinalizedOn { get; set; } = DateTime.UtcNow;
    }
}
