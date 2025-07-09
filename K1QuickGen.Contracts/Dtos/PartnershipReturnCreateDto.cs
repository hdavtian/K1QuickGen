using System;
using System.ComponentModel.DataAnnotations;

namespace K1QuickGen.Contracts.Dtos
{
    /// <summary>
    /// DTO used in API POST request for creating a new Partnership Return.
    /// </summary>
    public class PartnershipReturnCreateDto
    {
        [Required]
        public Guid CompanyId { get; set; }     // Required for linking and sharding

        [Required]
        public string PartnershipName { get; set; }

        [Required]
        [RegularExpression(@"\d{2}-\d{7}", ErrorMessage = "EIN must be in format 12-3456789.")]
        public string EIN { get; set; }

        [Range(2000, 2100)]
        public int TaxYear { get; set; }
    }
}
