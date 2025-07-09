using System;

namespace K1QuickGen.Contracts.Dtos
{
    /// <summary>
    /// DTO used in API responses for Partner Submission details.
    /// Includes all relevant partner fields for output.
    /// </summary>
    public class PartnerSubmissionOutputDto
    {
        public Guid Id { get; set; }
        public Guid Form1065Id { get; set; }
        public Guid CompanyId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerType { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public decimal CapitalContribution { get; set; }
        public string Email { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string SSNOrEIN { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool IsForeignPartner { get; set; }
        public bool IsTaxExemptEntity { get; set; }
        public decimal BeginningCapitalAccount { get; set; }
        public decimal EndingCapitalAccount { get; set; }
        public decimal ShareOfIncome { get; set; }
        public decimal ShareOfDeductions { get; set; }
    }
}
