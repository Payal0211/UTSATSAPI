using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_Get_Replacement_Report_Result
    {
        public long? OnBoardId { get; set; }
        public string? EngagementID { get; set; }
        public string? Company { get; set; }
        public string? Role { get; set; }
        public string? OldHRID { get; set; }
        public string? Talent { get; set; }
        public decimal? TalentBRPR { get; set; }
        public decimal? DPBR { get; set; }
        public decimal? UplersNR { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? ReplacementDate { get; set; }
        public string? OldHRStatus { get; set; }
        public string? OldHRStatusCode { get; set; }
        public string? NewHRID { get; set; }
        public string? NewTalent { get; set; }
        public decimal? NewTalentBRPR { get; set; }
        public decimal? NewDPBR { get; set; }
        public decimal? NewUplersNR { get; set; }
        public string? NewStartDate { get; set; }
        public string? NewEndDate { get; set; }
        public int? TotalRecords { get; set; }
    }
}
