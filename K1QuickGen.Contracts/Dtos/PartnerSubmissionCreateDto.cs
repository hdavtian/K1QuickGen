using System;

namespace K1QuickGen.Contracts.Dtos
{
    public class PartnerSubmissionCreateDto
    {
        public Guid Form1065Id { get; set; }
        public Guid CompanyId { get; set; }

        public string PartnerName { get; set; }
        public string PartnerType { get; set; }

        public decimal OwnershipPercentage { get; set; }
        public decimal CapitalContribution { get; set; }

        public string Email { get; set; } // For digital delivery of K-1
    }
}
