using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K1QuickGen.Contracts.Dtos
{
    public class K1ScheduleDto
    {
        public string PartnerName { get; set; }
        public string PartnerType { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public decimal CapitalContribution { get; set; }
        public string Email { get; set; }
    }
}
