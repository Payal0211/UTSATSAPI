using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes.UpChat
{
    [Keyless]
    public class sp_UTS_get_HRHistoryUpChat_Result
    {
        public string? ActionName { get; set; }
        public string? DisplayName { get; set; }
        public Nullable<DateTime> ActionDate { get; set; }
        public long HiringRequest_ID { get; set; }
        public string? ActionPerformedBy { get; set; }
        public string? UserEmployeeID { get; set; }
        public string? UserDesignation { get; set; }
        public string? TalentName { get; set; }
        public string? Comments { get; set; }
        public string? Remark { get; set; }
        public int IsNotes { get; set; }
        public int? HRStatusID { get; set; }
        public string? HRStatus { get; set; }
        
    }
}
