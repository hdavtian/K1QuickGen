using System;

namespace K1QuickGen.Contracts.Dtos
{
    public class Form1065CreateDto
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int TaxYear { get; set; }
        public bool AutoGenerateK1s { get; set; } // Optional flag to control backend logic
    }
}
