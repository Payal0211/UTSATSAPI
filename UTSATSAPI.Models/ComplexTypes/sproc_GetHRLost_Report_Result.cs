using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_GetHRLost_Report_Result
    {
        public long? HRID { get; set; }
        public string? HR_Number { get; set; }
        public string? SalesUser {  get; set; }
        public string? ClientType { get; set; }
        public string? Client { get; set;}
        public string? Company { get; set; }
        public decimal? TotalTR { get; set; }
        public decimal? TRLostCount { get; set; }

        public DateTime? HRCreatedDate { get; set; }
        public string? LostReason { get; set; }
        public string? LostDoneBy { get; set; }

        public DateTime? LostDate { get; set; }
        public int? Talents { get;set; }
        public int TotalRecords { get; set; }

    }
}
