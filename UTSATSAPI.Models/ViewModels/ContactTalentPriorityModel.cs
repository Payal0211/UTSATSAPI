namespace UTSATSAPI.Models.ViewModels
{
    public class ContactTalentPriorityModel
    {
        public ContactTalentPriorityModel()
        {
            TalentDetails = new();
        }
        public long HRID { get; set; }
        public long HRStatusID { get; set; }
        public string HRStatus { get; set; }

        public List<TalentDetail> TalentDetails { get; set; }
    }

    public class TalentDetail
    {
        public long ATS_TalentID { get; set; }
        public string? TalentStatus { get; set; }
        public long UTS_TalentID { get; set; }
        public decimal Talent_USDCost { get; set; }
        public string? Talent_RejectReason { get; set; }
        public string? RejectedBy { get; set; }
        public string? TalentStatusRoundDetails { get; set; }
        public string? RejectionComments { get; set; }
        public string? ActionUserName { get; set; }
        public string? ActionUserEmail { get; set; }
        public string? ActionBy { get; set; }
        public string? RejectionMessageForTalent { get; set; }
    }
}
