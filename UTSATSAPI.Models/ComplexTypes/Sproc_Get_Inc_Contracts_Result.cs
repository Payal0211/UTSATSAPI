using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Inc_Contracts_Result
    {
        public int? ID { get; set; }
        public string? ContractMonth { get; set; }
        public decimal? SalesConsultant { get; set; }
        public decimal? PODManagers { get; set; }
        public decimal? BDR_USD { get; set; }
        public decimal? BDRLead_USD { get; set; }
        public decimal? BDRManagerHead_USD { get; set; }
        public decimal? MarketingTeam { get; set; }
        public decimal? MarketingLead { get; set; }
        public decimal? MarketingHead { get; set; }
        public decimal? AM { get; set; }
        public decimal? AMHead { get; set; }
        public int? TotalRecords { get; set; }
    }
}
