using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K1QuickGen.Contracts.Dtos
{
    /// <summary>
    /// DTO model returned by API after a Form 1065 submission.
    /// </summary>
    public class Form1065OutputDto
    {
        public Guid Form1065Id { get; set; }
        public string CompanyName { get; set; }
        public Guid CompanyId { get; set; }
        public int TaxYear { get; set; }

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

        public List<K1ScheduleDto> PartnerK1s { get; set; }
    }
}
