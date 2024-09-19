namespace UTSATSAPI.Models.ViewModels
{
    public class outTalentDetailATS
    {
        public long? HRID { get; set; }
        public long HRStatusID { get; set; }
        public string HRStatus { get; set; }
        public long ATS_TalentID { get; set; }
        public string TalentStatus { get; set; }
        public long? UTS_TalentID { get; set; }
        public decimal Talent_USDCost { get; set; }
        public string Reason { get; set; }
        public string? Talent_RejectReason { get; set; }
        public string? RejectedBy { get; set; }

    }
}
