namespace UTSATSAPI.Models.ViewModels
{
    public class outTalentDetail
    {
        public long ATS_TalentID { get; set; }
        public string? TalentStatus { get; set; }
        public long UTS_TalentID { get; set; }
        public decimal Talent_USDCost { get; set; }
        public string? availibility { get; set; }
        public string? shift { get; set; }
        public string? noticeperiod { get; set; }
        public string? MatchMakingDateTime { get; set; }
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
