using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_TalentBackout_Report_Result
    {
        public long HRID { get; set; }
        public string? HR_Number { get; set; }
        public string? Company { get; set; }
        public string? Client { get; set; }
        public string? JobTitle { get; set; }
        public string? Talent { get; set; }
        public string? RejectedReason { get; set; }
        public decimal? PR { get; set; }
        public decimal? BR { get; set; }
        public decimal? UplersNRDP { get; set; }
        public string? HRStatus { get; set; }
        public string? CreatedDateTime { get; set; }
        public long? TotalRecords { get; set; }
        public string? SalesUser { get; set; }
    }
}
