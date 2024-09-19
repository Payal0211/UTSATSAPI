namespace UTSATSAPI.Models.ViewModels
{
    public class UpdateTalentStatus
    {
        public long HiringRequestId { get; set; }
        public long CtpId { get; set; }
        public long TalentId { get; set; }
        public int StatusId { get; set; }
        public string? ProfileRejectionStage { get; set; }
        public int? RejectReasonID { get; set; }
        public string? Remark { get; set; }
    }
}
