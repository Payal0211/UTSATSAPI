using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_TalentDetails_For_Managed_HR_Result
    {
        public string? TalentName { get; set; }
        public string? TalentStatus { get; set; }
        public string? BillRate { get; set; }
        public string? PayRate { get; set; }
        public string? NR { get; set; }
        public long TalentID { get; set; }
        public long HiringDetailID { get; set; }
        public long ContactId { get; set; }
        public int ClientOnBoarding_StatusID { get; set; }
        public long OnBoardId { get; set; }
        public string? ClientOnBoardStatus { get; set; }
        public int TalentOnBoarding_StatusID { get; set; }
        public int LegalClientOnBoarding_StatusID { get; set; }
        public int LegalTalentOnBoarding_StatusID { get; set; }
        public int Kickoff_StatusID { get; set; }
        public string? TalentOnBoardStatus { get; set; }
        public string? ManagedTalent_Level { get; set; }
        public string? POC_FullName { get; set; }
        public string? ScopeOfWork { get; set; }
        public string? TalentOnBoardDate { get; set; }
        public string? TalentSource { get; set; }
        public string? TalentRole { get; set; }
        public int ProfileStatusCode { get; set; }
    }
}
