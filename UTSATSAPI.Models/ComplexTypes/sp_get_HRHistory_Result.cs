using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sp_get_HRHistory_Result
    {
        public long? ActionID { get; set; }
        public string? ActionName { get; set; }
        public string? DisplayName { get; set; }
        public Nullable<DateTime> ActionDate { get; set; }
        public long HiringRequest_ID { get; set; }
        public string? ActionPerformedBy { get; set; }
        public string? ATSTalentLiveURL { get; set; }
        public string? ATSNonNDAURL { get; set; }
        public string? TalentName { get; set; }
        public string? InterviewDateTime { get; set; }
        public string? Comments { get; set; }
        public Nullable<DateTime> SLA_DueDate { get; set; }
        public int IsNotes { get; set; }
        public string? AssignedUsers { get; set; }
        public string? Remark { get; set; }
        public string? TSCPerson { get; set; }
        public int? IsDisplayUpdateHR { get; set; }
        public long? HistoryID { get; set; }
        public int? IsDisplayUpdateHRDetail { get; set; }
        public long? DetailHistoryID { get; set; }
    }
}
