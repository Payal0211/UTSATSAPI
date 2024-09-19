namespace UTSATSAPI.Models.ViewModels
{
    public class UpdateAM_Payout
    {
        public long PayOutID { get; set; }
        public long? OnBoardID { get; set; }
        public long? HiringRequestID { get; set; }
        public long? ContactID { get; set; }
        public long? TalentID { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? OldAMPersonID { get; set; }
        public int? NewAMPersonID { get; set; }
        public string? Comment { get; set; }
        public string? EngagementId_HRID { get; set; }
    }
}
