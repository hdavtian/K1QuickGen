using System;
using System.ComponentModel.DataAnnotations;

namespace K1QuickGen.Contracts.Dtos
{
    /// <summary>
    /// DTO to API communication
    /// Data Transfer Object for creating a new Form 1065 (U.S. Return of Partnership Income).
    /// Used as the request body when submitting a new Form1065 via the API.
    /// </summary>
    public class Form1065CreateDto
    {
        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; }
        
        [Required]
        public string? Ein { get; set; }
        public decimal? TotalAssets { get; set; }

        [Range(2000, 2100)]
        public int TaxYear { get; set; }

        [MaxLength(100)]
        public string? BusinessActivity { get; set; }

        [MaxLength(100)]
        public string? ProductOrService { get; set; }

        [MaxLength(6)]
        public string? BusinessCode { get; set; }

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

        [DataType(DataType.Date)]
        public DateTime? DateBusinessStarted { get; set; }

        public bool IsFinalReturn { get; set; }
        public bool IsAmendedReturn { get; set; }

        public bool AutoGenerateK1s { get; set; }
    }

}
