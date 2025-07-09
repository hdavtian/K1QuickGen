using System;
using System.ComponentModel.DataAnnotations;

namespace K1QuickGen.Contracts.Dtos
{
    /// <summary>
    /// DTO used in API POST request for creating a new Partner Submission.
    /// </summary>
    public class PartnerSubmissionCreateDto
    {
        [Required]
        public Guid Form1065Id { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        [MaxLength(200)]
        public string PartnerName { get; set; }

        [MaxLength(100)]
        public string? PartnerType { get; set; }

        [Range(0, 100)]
        public decimal OwnershipPercentage { get; set; }

        [Range(0, double.MaxValue)]
        public decimal CapitalContribution { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [RegularExpression(@"\d{3}-\d{2}-\d{4}|\d{2}-\d{7}", ErrorMessage = "Must be SSN (###-##-####) or EIN (##-#######)")]
        public string? SSNOrEIN { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(50)]
        public string? State { get; set; }

        [MaxLength(20)]
        public string? ZipCode { get; set; }

        [MaxLength(50)]
        public string? Country { get; set; }

        public bool IsForeignPartner { get; set; }
        public bool IsTaxExemptEntity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal BeginningCapitalAccount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal EndingCapitalAccount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ShareOfIncome { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ShareOfDeductions { get; set; }
    }

}
