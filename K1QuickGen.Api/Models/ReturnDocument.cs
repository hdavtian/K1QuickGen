using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class ReturnDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string CompanyId { get; set; }
    public string Form1065Id { get; set; }
    public DateTime FinalizedOn { get; set; } = DateTime.UtcNow;
}
