using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K1QuickGen.Contracts.Dtos
{
    public class Form1065OutputDto
    {
        public Guid Form1065Id { get; set; }
        public string CompanyName { get; set; }
        public int TaxYear { get; set; }
        public List<K1ScheduleDto> PartnerK1s { get; set; }
    }
}
