using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_ManagedTalent_Result
    {
        public long? ID { get; set; }
        public string? ManagedTalentFirstName { get; set; }
        public string? ManagedTalentLastName { get; set; }
        public string? HR_Number { get; set; }
        public decimal? ManagedTalentCost { get; set; }
        public string? CreatedByDateTime { get; set; }
        public decimal? NRPercentage { get; set; }
        public decimal? HRCost { get; set; }
        public string? ManagedTalentEmailID { get; set; }
        public string? TalentRole { get; set; }
        public string? Talent_Type { get; set; }
        public string? ManagedTalent_Level { get; set; }
        public string? ScopeOfWork { get; set; }
        public string? POC_FullName { get; set; }
        public string? ProposalConfirmDate { get; set; }
        public int TotalRecords { get; set; }
    }
}
