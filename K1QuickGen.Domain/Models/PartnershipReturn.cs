using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace K1QuickGen.Domain.Models;

public class PartnershipReturn
{
    [BsonId]
    [BsonRepresentation(BsonType.String)] // Prevents Guid serialization issues
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PartnershipName { get; set; }
    public string CompanyId { get; set; }
    public string EIN { get; set; }
    public int TaxYear { get; set; }
}
