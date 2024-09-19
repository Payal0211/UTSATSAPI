using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_Get_Inc_PlacementFees_Slab_Result
    {
        public int ID { get; set; }
        public string? PlacementFeesSlab { get; set; }
        public decimal? SalesConsultant { get; set; }
        public decimal? PODManagers { get; set; }
        public decimal? BDR { get; set; }
        public decimal? BDR_Lead { get; set; }
        public decimal? BDRManager_Head { get; set; }
        public decimal? MarketingTeam { get; set; }
        public decimal? MarketingLead { get; set; }
        public decimal? MarketingHead { get; set; }
        public decimal? AM { get; set; }
        public decimal? AMHead { get; set; }
        public int TotalRecords { get; set; }
    }
}
