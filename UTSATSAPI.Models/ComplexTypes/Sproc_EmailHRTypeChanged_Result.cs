using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_EmailHRTypeChanged_Result
    {
        public string? HR_Number { get; set; }
        public string? ClientName { get; set; }
        public string? TalentName { get; set; }
        public string? FinalCost { get; set; }
        public decimal? DPPercentage { get; set; }
        public string? TalentRole { get; set; }
        public string? SalesEmailId { get; set; }
        public string? SalesEmailName { get; set; }
        public long? HRSalesPersonID { get; set; }
    }
}
