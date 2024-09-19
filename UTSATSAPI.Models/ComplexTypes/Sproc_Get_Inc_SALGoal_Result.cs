using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Inc_SALGoal_Result
    {
        public int? ID { get; set; }
        public string? SALGoal { get; set; }
        public int? User_TeamID { get; set; }
        public decimal? SalesConsultant { get; set; }
        public decimal? PODManagers { get; set; }
        public decimal? BDR_USD { get; set; }
        public decimal? BDR_INR { get; set; }
        public decimal? BDRManagerHead_USD { get; set; }
        public decimal? MarketingTeam { get; set; }
        public decimal? MarketingLead { get; set; }
        public decimal? MarketingHead { get; set; }
        public decimal? AM { get; set; }
        public decimal? AMHead { get; set; }
        public int TotalRecords { get; set; }
        public decimal? BDR_LeadUSD { get; set; }
        public decimal? BDR_LeadINR { get; set; }
    }
}
